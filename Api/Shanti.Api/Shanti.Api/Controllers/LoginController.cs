using ApiTest.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace Shanti.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        string con = "Data Source=TestApiDB.mssql.somee.com;Initial Catalog=TestApiDB;User ID=DeadHatred_SQLLogin_2;Password=u5y75krtih";

        [HttpGet("testsite")]
        public string TestSite()
        {
            return "success";
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public bool Login([FromBody] LoginData data)
        {
            HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            SqlConnection connection = new SqlConnection(con);
            SqlCommand command = new SqlCommand(
                "SELECT * FROM [User] WHERE LOGIN = @log AND PASSWORD = @pas;"
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
                    result += reader["Login"];
                }
                reader.Close();
                return result != string.Empty ? true : false;
            }
            catch (Exception e)
            {
                return true;
            }
        }

    }
}
