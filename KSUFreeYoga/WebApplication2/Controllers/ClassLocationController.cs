using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace KSUFreeYogaAPI.Controllers
{
    /// <summary>
    /// FILL THIS IN TO DO
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ClassLocationController : ControllerBase
    {
        /// <summary>
        /// Holds the configuration settings of the application.
        /// </summary>
        private IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the YogaClassController with the specified configuration settings.
        /// </summary>
        /// <param name="configuration"> Represent's the application's configuration settings. </param>
        public ClassLocationController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult GetLocations()
        {
            string query = "select * from dbo.ClassLocation";
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

        [HttpPost]
        public JsonResult AddLocation(ClassLocation classLocation)
        {
            string query = $@"insert into dbo.ClassLocation (BuildingName, RoomNumber, LocationAddress)
                            values (@BuildingName, @RoomNumber, @LocationAddress);
                            SELECT CAST(SCOPE_IDENTITY() AS int);";
            int classLocationID = 0;
            string sqlDataSource = _configuration.GetConnectionString("Ksufreeyoga");
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@BuildingName", classLocation.BuildingName);
                    myCommand.Parameters.AddWithValue("@RoomNumber", classLocation.RoomNumber);
                    myCommand.Parameters.AddWithValue("@LocationAddress", classLocation.LocationAddress);
                    classLocationID = (int)myCommand.ExecuteScalar();

                   
                }
                myCon.Close();
            }
            return new JsonResult(classLocationID);
        }
    }
}
