using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shanti.Api.Models;
using System.Data.SqlClient;
using System.Text;
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
            trigger.CommandValue = newtrigger.CommandValue;
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

            reader = command.ExecuteReader();
            if (reader.Read())
            {
                trigger.IsPwm = (bool)reader["PWM"];
                trigger.Pin = (int)reader["PIN"];
            }
            reader.Close();
            connection.Close();

            SendTrigger(trigger);
            return JsonConvert.SerializeObject(trigger);
        }

        public static bool SendTrigger(DispatcherTrigger trigger)
        {
            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7160/trigger/send");
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
