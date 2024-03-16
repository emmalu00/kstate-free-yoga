using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace KSUFreeYoga.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class YogaController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public YogaController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                            select ClassID, Name from 
                            YogaClass
                            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("MyDatabase");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using(SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult(table);
        }

    }
}
