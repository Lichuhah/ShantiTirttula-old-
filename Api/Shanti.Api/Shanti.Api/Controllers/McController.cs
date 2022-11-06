using ApiTest.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace Shanti.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class McController : ControllerBase
    {
        string con = "Data Source=TestApiDB.mssql.somee.com;Initial Catalog=TestApiDB;User ID=DeadHatred_SQLLogin_2;Password=u5y75krtih";

        private string generateNewCode()
        {
            List<string> codes = new List<string>();
            SqlConnection connection = new SqlConnection(con);
            SqlCommand command = new SqlCommand(
                "SELECT U_KEY FROM [CONTROLLER]"
                , connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                codes.Add(reader["U_KEY"].ToString());
            }
            reader.Close();
            Random random = new Random(DateTime.UtcNow.Millisecond);
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            while (true)
            {
                string res = new string(Enumerable.Repeat(chars, 16)
                .Select(s => s[random.Next(s.Length)]).ToArray());
                if (!codes.Contains(res)) return res;
            }

        }

        private bool addNewCode(string code, int userId, string MAC)
        {
            SqlConnection connection = new SqlConnection(con);
            SqlCommand command = new SqlCommand(
                "INSERT INTO [CONTROLLER] (MAC, U_KEY, USER_ID) VALUES (@mac, @code, @uid);"
                , connection);
            command.Parameters.AddWithValue("@uid", userId);
            command.Parameters.AddWithValue("@code", code);
            command.Parameters.AddWithValue("@mac", MAC);
            connection.Open();
            try
            {
                command.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        [HttpPost("getcode")]
        public string GetCode([FromBody] LoginDataWithMac data)
        {
            SqlConnection connection = new SqlConnection(con);
            SqlCommand command = new SqlCommand(
                "SELECT * FROM [USER] WHERE LOGIN = @log AND PASSWORD = @pas;"
                , connection);
            command.Parameters.AddWithValue("@log", data.Login);
            command.Parameters.AddWithValue("@pas", data.Password);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            string result = string.Empty;
            try
            {
                while (reader.Read())
                {
                    result += reader["ID"].ToString();
                }
                reader.Close();
                if (result != string.Empty)
                {
                    string code = generateNewCode();
                    addNewCode(code, Convert.ToInt32(result), data.MAC);
                    return code;
                }
                else
                {
                    return "wronguser";
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}
