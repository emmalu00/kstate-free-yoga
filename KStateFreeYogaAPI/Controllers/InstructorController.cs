using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Globalization;

namespace KStateFreeYogaAPI.Controllers
{
    /// <summary>
    /// Contains CRUD operations for instructors
    /// </summary>
    [Route("api/instructor")]
    [ApiController]
    public class InstructorController : ControllerBase
    {
        /// <summary>
        /// Holds the configuration settings of the application.
        /// </summary>
        private IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the InstructorController with the specified configuration settings.
        /// </summary>
        /// <param name="configuration"> Represent's the application's configuration settings. </param>
        public InstructorController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Gets instructor(s) from the database
        /// </summary>
        /// <param name="instructorID"> ID of instructor </param>
        /// <returns> List of instructors </returns>
        [HttpGet]
        public JsonResult GetInstructors(int? instructorID)
        {
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("kstatefreeyogadata");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                
                    myCon.Open();
                List<string> whereClauses = new List<string>();

                if (instructorID.HasValue) { whereClauses.Add("i.InstructorID = @InstructorID"); }

                string whereClause = whereClauses.Any() ? "where " + string.Join(" and ", whereClauses) : string.Empty;
                string query = $@"select i.InstructorID, i.FirstName, i.LastName, i.Certified 
                                from dbo.Instructor as i
                                {whereClause}
                                order by i.FirstName asc";

                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    if (instructorID.HasValue) { myCommand.Parameters.AddWithValue("@InstructorID", instructorID.Value); }
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                }
                myCon.Close();
            }
            return new JsonResult(table);
        }

        /// <summary>
        /// Adds an instructor to the database
        /// </summary>
        /// <param name="instructor"> Instructor that is being added to the database </param>
        /// <returns> ID of added or existing instructor </returns>
        [HttpPost]
        public JsonResult AddInstructor(Instructor instructor)
        {
            string sqlDataSource = _configuration.GetConnectionString("kstatefreeyogadata");
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();

                // Check if the instructor already exists
                string queryCheck = @"
                    select InstructorID from dbo.instructor
                    where FirstName = @FirstName and LastName = @LastName";

                using (SqlCommand myCommandCheck = new SqlCommand(queryCheck, myCon))
                {
                    myCommandCheck.Parameters.AddWithValue("@FirstName", instructor.FirstName.ToLower());
                    myCommandCheck.Parameters.AddWithValue("@LastName", instructor.LastName.ToLower());

                    object existingInstructorId = myCommandCheck.ExecuteScalar();
                    if (existingInstructorId != null)
                    {
                        return new JsonResult(existingInstructorId); // Return existing instructor ID if found
                    }
                }
                // Insert new instructor if not already existing

                string queryInsert = @"
                    insert into instructor (FirstName, LastName, Certified)
                    values (@FirstName, @LastName, @Certified);
                    select cast(scope_identity() as int);";

                using (SqlCommand myCommandInsert = new SqlCommand(queryInsert, myCon))
                {
                    myCommandInsert.Parameters.AddWithValue("@FirstName", instructor.FirstName);
                    myCommandInsert.Parameters.AddWithValue("@LastName", instructor.LastName);
                    myCommandInsert.Parameters.AddWithValue("@Certified", instructor.Certified);

                    int newInstructorId = (int)myCommandInsert.ExecuteScalar();
                    return new JsonResult(newInstructorId);
                }
            }
        }

    }
}
