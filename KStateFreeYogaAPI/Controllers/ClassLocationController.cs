using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Globalization;

namespace KStateFreeYogaAPI.Controllers
{
    /// <summary>
    /// Contains CRUD operations for class locations
    /// </summary>
    [Route("api/classlocation")]
    [ApiController]
    public class ClassLocationController : ControllerBase
    {
        /// <summary>
        /// Holds the configuration settings of the application.
        /// </summary>
        private IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the ClassLocationController with the specified configuration settings.
        /// </summary>
        /// <param name="configuration"> Represent's the application's configuration settings. </param>
        public ClassLocationController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Get class location(s) from the database
        /// </summary>
        /// <returns> List of class locations </returns>
        [HttpGet]
        public JsonResult GetLocations()
        {
            string query = @"select * from dbo.ClassLocation";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("kstatefreeyogadata");
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

        /// <summary>
        /// Adds a new class location to the database
        /// </summary>
        /// <param name="classLocation"> Location being added to the database </param>
        /// <returns> ID of added or existing location </returns>
        [HttpPost]
        public JsonResult AddLocation(ClassLocation classLocation)
        {
            string sqlDataSource = _configuration.GetConnectionString("kstatefreeyogadata");
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();

                // Check if the location already exists
                string queryCheck = @"
                    select LocationID from dbo.classlocation
                    where BuildingName = @BuildingName and RoomNumber = @RoomNumber";

                using (SqlCommand myCommandCheck = new SqlCommand(queryCheck, myCon))
                {
                    myCommandCheck.Parameters.AddWithValue("@BuildingName", classLocation.BuildingName.ToLower());
                    myCommandCheck.Parameters.AddWithValue("@RoomNumber", classLocation.RoomNumber);

                    object existingLocationId = myCommandCheck.ExecuteScalar();
                    if (existingLocationId != null)
                    {
                        return new JsonResult(existingLocationId); // Return existing location ID if found
                    }
                }

                // Insert new location if not already existing
                string queryInsert = @"
                    insert into dbo.classlocation (BuildingName, RoomNumber, LocationAddress)
                    values (@BuildingName, @RoomNumber, @LocationAddress);
                    select cast(scope_identity() as int);"; 

                using (SqlCommand myCommandInsert = new SqlCommand(queryInsert, myCon))
                {
                    myCommandInsert.Parameters.AddWithValue("@BuildingName", classLocation.BuildingName);
                    myCommandInsert.Parameters.AddWithValue("@RoomNumber", classLocation.RoomNumber);
                    myCommandInsert.Parameters.AddWithValue("@LocationAddress", classLocation.LocationAddress);

                    int newLocationId = (int)myCommandInsert.ExecuteScalar();
                    return new JsonResult(newLocationId);
                }
            }
        }
    }
}
