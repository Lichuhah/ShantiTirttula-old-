using ApiTest.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shanti.Api.Models;
using System.Data.SqlClient;
using System.Security.Cryptography;

namespace Shanti.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class McCommandController : ControllerBase
    {
        string con = "Data Source=TestApiDB.mssql.somee.com;Initial Catalog=TestApiDB;User ID=DeadHatred_SQLLogin_2;Password=u5y75krtih";

        //[HttpGet("getcommand")]
        //public string GetLastCommand(string key)
        //{
        //    SqlConnection connection = new SqlConnection(con);
        //    SqlCommand command = new SqlCommand(
        //        "SELECT * FROM [MC_COMMAND] WHERE MC_KEY = @key;"
        //        , connection);
        //    command.Parameters.AddWithValue("@key", key);
        //    connection.Open();
        //    SqlDataReader reader = command.ExecuteReader();
        //    string result = string.Empty;
        //    McCommand mccommand = new McCommand();
        //    try
        //    {
        //        while (reader.Read())
        //        {
        //            mccommand.a = reader["A"].ToString();
        //            mccommand.b = reader["B"].ToString();
        //        }
        //        reader.Close();
        //        return JsonConvert.SerializeObject(mccommand);
        //    } catch (Exception e) { throw new Exception(e.Message); }
        //}

        [HttpPost("addcommand")]
        public string AddNewCommand(string key, [FromBody] McCommand com)
        {
            SqlConnection connection = new SqlConnection(con);
            SqlCommand command = new SqlCommand(
                "INSERT INTO [MC_COMMAND] (MC_KEY, PIN, VALUE) VALUES (@key, @pin, @val);"
                , connection);
            command.Parameters.AddWithValue("@key", key);
            command.Parameters.AddWithValue("@pin", com.Pin);
            command.Parameters.AddWithValue("@val", com.Value);
            connection.Open();
            try
            {
                command.ExecuteNonQuery();
                return "true";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}
