﻿using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shanti.Api.DispatcherService;
using Shanti.Api.Models;
using System.Data.SqlClient;
using System.Windows.Input;

namespace Shanti.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TriggerController : ControllerBase
    {
        string con = "Data Source=TestApiDB.mssql.somee.com;Initial Catalog=TestApiDB;User ID=DeadHatred_SQLLogin_2;Password=u5y75krtih";
        [HttpPost("trigger")]
        public string AddNewTrigger([FromBody] NewTrigger newtrigger)
        {
            SqlConnection connection = new SqlConnection(con);
            DispatcherTrigger trigger = new DispatcherTrigger();
            trigger.DeviceId = newtrigger.DeviceId;
            trigger.ControllerId = newtrigger.ControllerId;
            trigger.CommandValue = newtrigger.CommandValue;
            trigger.TriggerValue = newtrigger.TriggerValue;

            int typeId = 0;
            SqlCommand command = new SqlCommand(
                "SELECT TYPE_ID FROM CONTROLLER WHERE ID = @id;"
                , connection);
            command.Parameters.AddWithValue("@id", trigger.ControllerId);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                typeId = (int)reader["TYPE_ID"];
            }
            reader.Close();

            command = new SqlCommand(
                "SELECT * FROM [TYPE_CONTROLLER_DEVICE] WHERE TYPE_DEVICE_ID = @did AND TYPE_CONTROLLER_ID=@cid;"
                , connection);
            command.Parameters.AddWithValue("@did", newtrigger.DeviceId);
            command.Parameters.AddWithValue("@cid", typeId);

            reader = command.ExecuteReader();
            if (reader.Read())
            {
                trigger.IsPwm = (bool)reader["PWM"];
                trigger.Pin = (int)reader["PIN"];
            }
            reader.Close();
            connection.Close();

            TcpServer tcp = TcpServer.getInstance();
            tcp.SendTrigger(trigger);
            return JsonConvert.SerializeObject(trigger);
        }
    }
}