using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text;

namespace KSUFreeYogaAPI.Controllers
{
    /// <summary>
    /// FILL THIS IN TO DO
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class YogaClassController : ControllerBase
    {
        /// <summary>
        /// Holds the configuration settings of the application.
        /// </summary>
        private IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the YogaClassController with the specified configuration settings.
        /// </summary>
        /// <param name="configuration"> Represent's the application's configuration settings. </param>
        public YogaClassController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Gets list of yoga classes in the database
        /// </summary>
        /// <param name="buildingName"> Building which class is held </param>
        /// <param name="instructorFirst"> Instructor first name </param>
        /// <param name="instructorLast"> Instructor last name </param>
        /// <param name="matsProvided"> Whether or not mats are provided </param>
        /// <returns> List of yoga classes </returns>
        [HttpGet]
        public JsonResult GetYogaClasses(string? buildingName, string? instructorFirst, string? instructorLast, bool? matsProvided)
        {
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Ksufreeyoga");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                List<string> whereClauses = new List<string>();

                if (!string.IsNullOrEmpty(buildingName)) { whereClauses.Add("cl.BuildingName = @BuildingName"); }
                if (!string.IsNullOrEmpty(instructorFirst)) { whereClauses.Add("i.FirstName = @FirstName"); }
                if (!string.IsNullOrEmpty(instructorLast)) { whereClauses.Add("i.LastName = @LastName"); }
                if (matsProvided.HasValue) { whereClauses.Add("yc.MatsAvailable = @MatsAvailable"); }

                string whereClause = whereClauses.Any() ? "WHERE " + string.Join(" AND ", whereClauses) : string.Empty;
                string query = $@"select yc.ClassID, yc.ClassName, yc.StartTime, yc.EndTime, yc.ClassDate, yc.MatsAvailable, yc.ClassDescription,
                cl.BuildingName, cl.RoomNumber, cl.LocationAddress, i.FirstName, i.LastName
                from dbo.YogaClass as yc
                inner join dbo.instructor i on yc.InstructorID = i.InstructorID
                inner join dbo.classLocation cl on yc.LocationID = cl.LocationID
                {whereClause}; ";

                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    if (!string.IsNullOrEmpty(buildingName)) { myCommand.Parameters.AddWithValue("@BuildingName", buildingName); }
                    if (!string.IsNullOrEmpty(instructorFirst)) { myCommand.Parameters.AddWithValue("@FirstName", instructorFirst); }
                    if (!string.IsNullOrEmpty(instructorLast)) { myCommand.Parameters.AddWithValue("@LastName", instructorLast); }
                    if (matsProvided.HasValue) { myCommand.Parameters.AddWithValue("@MatsAvailable", matsProvided.Value); }
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                }
                myCon.Close();
            }
            return new JsonResult(table);
        }

        [HttpGet]
        [Route("GetTeacherNames")]
        public JsonResult GetTeacherNames()
        {
            string query = "select * from dbo.Instructor";
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
        [Route("GetLocations")]
        public JsonResult GetLocations()
        {
            string query = "select * from dbo.classLocation";
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

        /// <summary>
        /// Adds a yoga class to the database
        /// </summary>
        /// <param name="yogaClass"> Yoga class object that contains information about the class </param>
        /// <returns> Message that a class was added successfully </returns>
        [HttpPost]
        public JsonResult AddClass(YogaClass yogaClass)
        {
            string procedureName = "AddYogaClass";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Ksufreeyoga");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(procedureName, myCon))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.Parameters.AddWithValue("@ClassName", yogaClass.ClassName);
                    myCommand.Parameters.AddWithValue("@StartTime", yogaClass.StartTime);
                    myCommand.Parameters.AddWithValue("@EndTime", yogaClass.EndTime);
                    myCommand.Parameters.AddWithValue("@ClassDate", yogaClass.ClassDate);
                    myCommand.Parameters.AddWithValue("@InstructorID", yogaClass.InstructorID);
                    myCommand.Parameters.AddWithValue("@LocationID", yogaClass.LocationID);
                    myCommand.Parameters.AddWithValue("@MatsAvailable", yogaClass.MatsProvided);
                    myCommand.Parameters.AddWithValue("@ClassDescription", yogaClass.ClassDescription);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Class added Successfully");
        }

        /// <summary>
        /// Deletes yoga class from the database
        /// </summary>
        /// <param name="id"> ID of class to be deleted </param>
        /// <returns> Whether or not deletion is successful. </returns>
        [HttpDelete]
        public JsonResult DeleteYogaClass(int id)
        {
            string queryCheck = "select count(1) from dbo.yogaclass where ClassID = @id";
            string queryDelete = "delete from dbo.yogaclass where ClassID = @id";
            string sqlDataSource = _configuration.GetConnectionString("Ksufreeyoga");
            int rowsAffected = 0;

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                // Check if the class exists
                using (SqlCommand myCommandCheck = new SqlCommand(queryCheck, myCon))
                {
                    myCommandCheck.Parameters.AddWithValue("@id", id);
                    int count = Convert.ToInt32(myCommandCheck.ExecuteScalar());
                    if (count == 0) { return new JsonResult("Class with provided ID does not exist."); }
                }
                // Proceed with deletion
                using (SqlCommand myCommandDelete = new SqlCommand(queryDelete, myCon))
                {
                    myCommandDelete.Parameters.AddWithValue("@id", id);
                    rowsAffected = myCommandDelete.ExecuteNonQuery();
                }
                myCon.Close();
            }
            if (rowsAffected > 0) { return new JsonResult("Deleted Successfully"); }
            else { return new JsonResult("Deletion failed. Class does not exist."); }
        }
    }
}
