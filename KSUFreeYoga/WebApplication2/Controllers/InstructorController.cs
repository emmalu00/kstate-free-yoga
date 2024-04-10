using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace KSUFreeYogaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructorController : ControllerBase
    {
        /// <summary>
        /// Holds the configuration settings of the application.
        /// </summary>
        private IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the YogaClassController with the specified configuration settings.
        /// </summary>
        /// <param name="configuration"> Represent's the application's configuration settings. </param>
        public InstructorController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult GetInstructors(bool? certified) //change this function to have optional parameters
        {
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Ksufreeyoga");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                List<string> whereClauses = new List<string>();

                if (certified.HasValue) { whereClauses.Add("i.Certified = @Certified"); }

                string whereClause = whereClauses.Any() ? "WHERE " + string.Join(" AND ", whereClauses) : string.Empty;
                string query = $@"select i.FirstName, i.LastName, i.Certified
                                from dbo.Instructor as i
                                {whereClause}; ";

                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    if (certified.HasValue) { myCommand.Parameters.AddWithValue("@Certified", certified.Value); }
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                }
                myCon.Close();
            }
            return new JsonResult(table);
        }


/*        [HttpPost]
        public JsonResult AddInstructor(Instructor instructor)
        {
            string query = $@"insert into Instructor (FirstName, LastName, Certified)
                            values (@FirstName, @LastName, @Certified);";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Ksufreeyoga");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@FirstName", instructor.FirstName);
                    myCommand.Parameters.AddWithValue("@LastName", instructor.LastName);
                    myCommand.Parameters.AddWithValue("@Certified", instructor.Certified);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Added Successfully");
        }*/

        [HttpPost]
        public JsonResult AddInstructor(Instructor instructor)
        {
            string query = @"
        INSERT INTO Instructor (FirstName, LastName, Certified)
        VALUES (@FirstName, @LastName, @Certified);
        SELECT CAST(SCOPE_IDENTITY() AS int);"; // Cast is optional depending on  ID column type

            int newInstructorId = 0;
            string sqlDataSource = _configuration.GetConnectionString("Ksufreeyoga");
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@FirstName", instructor.FirstName);
                    myCommand.Parameters.AddWithValue("@LastName", instructor.LastName);
                    myCommand.Parameters.AddWithValue("@Certified", instructor.Certified);

                    // ExecuteScalar is used here because we expect a single value to be returned (the new ID)
                    newInstructorId = (int)myCommand.ExecuteScalar();
                }
                myCon.Close();
            }

            return new JsonResult(newInstructorId);
        }
    }
}
