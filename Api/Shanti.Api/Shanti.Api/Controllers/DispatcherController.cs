using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Shanti.Api.Models;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Shanti.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DispatcherController : ControllerBase
    {
        string con = "Data Source=TestApiDB.mssql.somee.com;Initial Catalog=TestApiDB;User ID=DeadHatred_SQLLogin_2;Password=u5y75krtih";

        private readonly IConfiguration _config;
        public DispatcherController(IConfiguration config)
        {
            _config = config;
        }

        [AllowAnonymous]
        [HttpPost("token")]
        public string GetToken([FromBody] LoginDataJwt data)
        {
            HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            SqlConnection connection = new SqlConnection(con);
            SqlCommand command = new SqlCommand(
                "SELECT * FROM [CONTROLLER] WHERE MAC = @mac AND U_KEY = @key;"
                , connection);
            command.Parameters.AddWithValue("@mac", data.MAC);
            command.Parameters.AddWithValue("@key", data.Serial);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            string result = string.Empty;
            try
            {
                while (reader.Read())
                {
                    result += reader["ID"];
                }
                reader.Close();
                if (String.IsNullOrEmpty(result))
                {
                    return "false data";
                } else
                {
                    return CreateToken(data); 
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        private string CreateToken(LoginDataJwt data)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.SerialNumber, data.Serial)
            };
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}
