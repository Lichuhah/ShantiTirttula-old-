using ApiTest.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shanti.Api.Models;
using Shanti.Dispatcher.Models.Mc;
using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace Shanti.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CommandController : ControllerBase
    {
        string con = "Data Source=TestApiDB.mssql.somee.com;Initial Catalog=TestApiDB;User ID=DeadHatred_SQLLogin_2;Password=u5y75krtih";

        [HttpPost("addcommand")]
        public string AddNewCommand([FromBody] Command com)
        {
            McCommand mccommand = new McCommand();
            mccommand.Value = com.Value;

            SqlConnection connection = new SqlConnection(con);
            SqlCommand command = new SqlCommand(
               "SELECT * FROM CONTROLLER WHERE ID = @id;"
               , connection);
            command.Parameters.AddWithValue("@id", com.ControllerId);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            int typeId = 0;
            if (reader.Read())
            {
                typeId = (int)reader["TYPE_ID"];
                mccommand.Serial = reader["SERIAL"].ToString();
            }
            reader.Close();

            command = new SqlCommand(
                "SELECT * FROM [TYPE_CONTROLLER_DEVICE] WHERE TYPE_DEVICE_ID = @did AND TYPE_CONTROLLER_ID=@cid;"
                , connection);
            command.Parameters.AddWithValue("@did", com.DeviceId);
            command.Parameters.AddWithValue("@cid", typeId);

            reader = command.ExecuteReader();
            if (reader.Read())
            {
                mccommand.IsPwm = (bool)reader["PWM"];
                mccommand.Pin = (int)reader["PIN"];
            }
            reader.Close();
            connection.Close();
            SendCommand(mccommand);
            return JsonConvert.SerializeObject(mccommand);
            
        }

        public static bool SendCommand(McCommand command)
        {
            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://ShantiDisp.somee.com/command/send");
            request.Content = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");
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
