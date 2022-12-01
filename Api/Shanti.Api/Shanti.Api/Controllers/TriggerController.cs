using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shanti.Api.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Input;

namespace Shanti.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TriggerController : ControllerBase
    {
        static string con = "Data Source=TestApiDB.mssql.somee.com;Initial Catalog=TestApiDB;User ID=DeadHatred_SQLLogin_2;Password=u5y75krtih";
        [HttpPost("trigger")]
        public string AddNewTrigger([FromBody] NewTrigger newtrigger)
        {
            SqlConnection connection = new SqlConnection(con);
            DispatcherTrigger trigger = new DispatcherTrigger();
            trigger.DeviceId = newtrigger.DeviceId;
            trigger.RightCommandValue = newtrigger.RightCommandValue;
            trigger.LeftCommandValue = newtrigger.LeftCommandValue;
            trigger.TriggerValue = newtrigger.TriggerValue;
            trigger.SensorId = newtrigger.SensorId;

            int typeId = 0;
            SqlCommand command = new SqlCommand(
                "SELECT * FROM CONTROLLER WHERE ID = @id;"
                , connection);
            command.Parameters.AddWithValue("@id", newtrigger.ControllerId);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                typeId = (int)reader["TYPE_ID"];
                trigger.Serial = reader["SERIAL"].ToString();
            }
            reader.Close();

            command = new SqlCommand(
                "SELECT * FROM [TYPE_CONTROLLER_DEVICE] WHERE TYPE_DEVICE_ID = @did AND TYPE_CONTROLLER_ID=@cid;"
                , connection);
            command.Parameters.AddWithValue("@did", newtrigger.DeviceId);
            command.Parameters.AddWithValue("@cid", typeId);
            int tcd = 0;
            reader = command.ExecuteReader();
            if (reader.Read())
            {
                tcd = (int)reader["ID"];
                trigger.IsPwm = (bool)reader["PWM"];
                trigger.Pin = (int)reader["PIN"];
            }
            reader.Close();
            connection.Close();

            SaveTrigger(newtrigger, typeId, tcd);
            SendTrigger(trigger);
            return JsonConvert.SerializeObject(trigger);
        }

        private void SaveTrigger(NewTrigger newtrigger, int typeId, int typeDevId)
        {
            SqlConnection connection = new SqlConnection(con);
            connection.Open();
            SqlCommand command = new SqlCommand(
               "SELECT ID FROM [TYPE_CONTROLLER_SENSOR] WHERE TYPE_CONTROLLER_ID=@tci AND TYPE_SENSOR_ID=@tdi;"
               , connection);
            command.Parameters.AddWithValue("@tci", typeId);
            command.Parameters.AddWithValue("@tdi", newtrigger.SensorId);
            SqlDataReader reader = command.ExecuteReader();
            int tsi = 0;
            if (reader.Read())
                tsi = (int)reader[0];
            reader.Close();

            command = new SqlCommand(
                "INSERT INTO [TRIGGER] (CONTROLLER_ID, TYPE_CONTROLLER_SENSOR_ID, " +
                "TYPE_CONTROLLER_DEVICE_ID, TRIGGER_VALUE, LEFT_COMMAND_VALUE, RIGHT_COMMAND_VALUE) " +
                "VALUES (@cid, @tcsi, @tcdi, @tv, @lcv, @rcv);"
                , connection);
            command.Parameters.AddWithValue("@cid", newtrigger.ControllerId);
            command.Parameters.AddWithValue("@tcsi", tsi);
            command.Parameters.AddWithValue("@tcdi", typeDevId);
            command.Parameters.AddWithValue("@tv", newtrigger.TriggerValue);
            command.Parameters.AddWithValue("@lcv", newtrigger.LeftCommandValue);
            command.Parameters.AddWithValue("@rcv", newtrigger.RightCommandValue);
            try
            {
                command.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception e)
            {
                connection.Close();
            }
        }

        [HttpGet("get")]
        public static List<DispatcherTrigger> GetTriggers(string serial)
        {
            SqlConnection connection = new SqlConnection(con);
            SqlCommand command = new SqlCommand(
                "SELECT * FROM [TRIGGER] JOIN [TYPE_CONTROLLER_DEVICE] TCD on [TRIGGER].TYPE_CONTROLLER_DEVICE_ID = TCD.ID  JOIN CONTROLLER C on C.ID = [TRIGGER].CONTROLLER_ID  JOIN TYPE_CONTROLLER_SENSOR TCS on TCS.ID = [TRIGGER].TYPE_CONTROLLER_SENSOR_ID  WHERE SERIAL = @ser"
                , connection);
            command.Parameters.AddWithValue("@ser", serial);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            List<DispatcherTrigger> triggers = new List<DispatcherTrigger>();
            while (reader.Read())
            {
                DispatcherTrigger trigger = new DispatcherTrigger();
                trigger.DeviceId = (int)reader["TYPE_DEVICE_ID"];
                trigger.RightCommandValue = (float)reader["TRIGGER_VALUE"];
                trigger.LeftCommandValue = (float)reader["TRIGGER_VALUE"];
                trigger.TriggerValue = (float)reader["TRIGGER_VALUE"];
                trigger.SensorId = (int)reader["TYPE_SENSOR_ID"];
                trigger.IsPwm = (bool)reader["PWM"];
                trigger.Pin = (int)reader["PIN"];
                trigger.Serial = serial;
                triggers.Add(trigger);
            }
            reader.Close();
            connection.Close();
            return triggers;
        }

        [HttpDelete("delete")]
        public bool DeleteTriggers(int id)
        {
            SqlConnection connection = new SqlConnection(con);
            SqlCommand command = new SqlCommand(
                "DELETE FROM [TRIGGER] WHERE ID=@id"
                , connection);
            command.Parameters.AddWithValue("@id", id);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
            return true;
        }

        public static bool SendTrigger(DispatcherTrigger trigger)
        {
            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://ShantiDisp.somee.com/trigger/send");
            request.Content = new StringContent(JsonConvert.SerializeObject(trigger), Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = client.Send(request);
                string answer = response.Content.ReadAsStringAsync().Result;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
