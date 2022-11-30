using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shanti.Api.Models;
using System.Data.SqlClient;
using System.Security.Claims;

namespace Shanti.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class SensorDataController : ControllerBase
    {
        string con = "Data Source=TestApiDB.mssql.somee.com;Initial Catalog=TestApiDB;User ID=DeadHatred_SQLLogin_2;Password=u5y75krtih";

        [HttpPost("send")]
        public string SendLight([FromBody] SensorData data)
        {
            int controllerId = GetIdBySerial();
            SqlConnection connection = new SqlConnection(con);
            SqlCommand command = new SqlCommand(
                "INSERT INTO [CONTROLLER_SENSOR_DATA] (CONTROLLER_ID, TYPE_SENSOR_ID, VALUE, DATE) VALUES (@cid, @tid, @val, @data);"
                , connection);
            command.Parameters.AddWithValue("@cid", controllerId);
            command.Parameters.AddWithValue("@tid", data.SensorId);
            command.Parameters.AddWithValue("@data", DateTime.UtcNow);
            command.Parameters.AddWithValue("@val", data.Value);
            connection.Open();
            try
            {
                command.ExecuteNonQuery();
                connection.Close();
                return "true";
            }
            catch (Exception e)
            {
                connection.Close();
                return e.Message;
            }
        }

        private int GetIdBySerial()
        {
            SqlConnection connection = new SqlConnection(con);
            SqlCommand command = new SqlCommand("SELECT ID FROM [CONTROLLER] WHERE SERIAL = @ser;", connection);
            string serial = User.Claims.Where(x => x.Type == ClaimTypes.SerialNumber).FirstOrDefault()?.Value;
            command.Parameters.AddWithValue("@ser", serial);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            int id = 0;
            try
            {
                if (reader.Read())
                {
                    id = (int)reader["ID"];
                }
                reader.Close();
                connection.Close();
                return id;
            }
            catch (Exception e)
            {
                connection.Close();
                return 0;
            }
        }
    }
}
