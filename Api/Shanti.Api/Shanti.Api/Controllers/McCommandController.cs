using ApiTest.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace Shanti.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class McCommandController : ControllerBase
    {
        string con = "Data Source=TestApiDB.mssql.somee.com;Initial Catalog=TestApiDB;User ID=DeadHatred_SQLLogin_2;Password=u5y75krtih";


       
    }
}
