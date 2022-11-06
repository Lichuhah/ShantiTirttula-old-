using ApiTest.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace Shanti.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class McDataController : ControllerBase
    {
        string con = "Data Source=TestApiDB.mssql.somee.com;Initial Catalog=TestApiDB;User ID=DeadHatred_SQLLogin_2;Password=u5y75krtih";


        //private int GetIdBySerial(string serial)
        //{
        //    SqlConnection connection = new SqlConnection(con);
        //    SqlCommand command = new SqlCommand(
        //        "SELECT ID FROM [CONTROLLER] WHERE U_KEY = @code;"
        //        , connection);
        //    command.Parameters.AddWithValue("@code", serial);
        //    connection.Open();
        //    SqlDataReader reader = command.ExecuteReader();
        //    reader.Read();
        //    return Convert.ToInt32(reader["ID"]);
        //}

        [HttpPost("sendLight")]
        public string SendLight([FromBody] LightData data)
        {
            //int controllerId = GetIdBySerial(data.serial);
            SqlConnection connection = new SqlConnection(con);
            SqlCommand command = new SqlCommand(
                "INSERT INTO [MC_LIGHT_DATA] (MC_KEY, VALUE, DATETIME) VALUES (@key, @val, @data);"
                , connection);
            command.Parameters.AddWithValue("@key", data.serial);
            command.Parameters.AddWithValue("@val", data.value);
            command.Parameters.AddWithValue("@data", DateTime.UtcNow);
            connection.Open();
            try
            {
                command.ExecuteNonQuery();
                if (Convert.ToInt32(data.value) < 30)
                {
                    McCommandController contr = new McCommandController();
                    contr.OnLight(data.serial);
                } else
                {
                    McCommandController contr = new McCommandController();
                    contr.OffLight(data.serial);
                }
                return "true";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}
