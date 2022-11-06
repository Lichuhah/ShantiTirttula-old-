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

        public void OnLight(string key)
        {
            SqlConnection connection = new SqlConnection(con);
            SqlCommand command = new SqlCommand(
                "INSERT INTO [MC_COMMAND] (MC_KEY, A, B) VALUES (@key, @a, @b);"
                , connection);
            command.Parameters.AddWithValue("@key", key);
            command.Parameters.AddWithValue("@val", "light");
            command.Parameters.AddWithValue("@data", true);
            connection.Open();
            try
            {
                command.ExecuteNonQuery();
                
            }
            catch (Exception e)
            {
                
            }
        }

        public void OffLight(string key)
        {
            SqlConnection connection = new SqlConnection(con);
            SqlCommand command = new SqlCommand(
                "INSERT INTO [MC_COMMAND] (MC_KEY, A, B) VALUES (@key, @a, @b);"
                , connection);
            command.Parameters.AddWithValue("@key", key);
            command.Parameters.AddWithValue("@val", "light");
            command.Parameters.AddWithValue("@data", false);
            connection.Open();
            try
            {
                command.ExecuteNonQuery();
                
            }
            catch (Exception e)
            {
               
            }
        }

        [HttpGet("getcommand")]
        public string TestSite(string key)
        {
            SqlConnection connection = new SqlConnection(con);
            SqlCommand command = new SqlCommand(
                "SELECT * FROM [MC_COMMAND] WHERE MC_KEY = @key;"
                , connection);
            command.Parameters.AddWithValue("@key", key);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            string result = string.Empty;
            McCommand mccommand = new McCommand();
            try
            {
                while (reader.Read())
                {
                    mccommand.a = reader["A"].ToString();
                    mccommand.b = reader["B"].ToString();
                }
                reader.Close();
                return JsonConvert.SerializeObject(mccommand);
            } catch (Exception e) { throw new Exception(e.Message); }
        }

        [HttpPost("addcommand")]
        public string TestSite(string key, [FromBody] McCommand com)
        {
            SqlConnection connection = new SqlConnection(con);
            SqlCommand command = new SqlCommand(
                "INSERT INTO [MC_COMMAND] (MC_KEY, A, B) VALUES (@key, @a, @b);"
                , connection);
            command.Parameters.AddWithValue("@key", key);
            command.Parameters.AddWithValue("@a", com.a);
            command.Parameters.AddWithValue("@b", com.b);
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
