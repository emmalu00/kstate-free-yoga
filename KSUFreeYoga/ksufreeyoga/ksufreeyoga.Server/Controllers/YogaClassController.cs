using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class YogaClassController : ControllerBase
    {

        private IConfiguration _configuration;

        public YogaClassController(IConfiguration configuration)
        {
            _configuration = configuration; 
        }

        [HttpGet]
        [Route("GetYogaClassInformation")]
        public JsonResult GetYogaClassInformation()
        {
            string query = "select * from dbo.yogaclass";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Ksufreeyoga");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult(table);
        }


        [HttpGet]
        [Route("GetYogaClassInformationWithMats")]
        public JsonResult GetYogaClassInformationWithMats()
        {
            // Modify the query to include a WHERE clause that checks if MatsAvailable is equal to 1
            string query = "SELECT * FROM dbo.yogaclass WHERE MatsAvailable = 1";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Ksufreeyoga");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
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
