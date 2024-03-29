using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text;
using WebApplication2;

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

/*        [HttpGet]
        [Route("GetYogaClassInformation")]
        public JsonResult GetYogaClassInformation()
        {
            string query = "select yc.ClassID, yc.ClassName, yc.StartTime, yc.EndTime, yc.ClassDate, yc.MatsAvailable, yc.ClassDescription, " +
                "cl.BuildingName, cl.RoomNumber, cl.LocationAddress, i.FirstName, i.LastName " +
                "from dbo.YogaClass as yc " +
                "inner join dbo.instructor i on yc.InstructorID = i.InstructorID " +
                "inner join dbo.classLocation cl on yc.LocationID = cl.LocationID; ";
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
        }*/

        [HttpGet]
        public JsonResult GetYogaClasses(string? buildingName, string? teacherFirstName, string? teacherLastName, bool? matsAvailable)
        {
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Ksufreeyoga");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                List<string> whereClauses = new List<string>();

                if (!string.IsNullOrEmpty(buildingName))
                {
                    whereClauses.Add("cl.BuildingName = @BuildingName");
                }
                if (!string.IsNullOrEmpty(teacherFirstName))
                {
                    whereClauses.Add("i.FirstName = @FirstName");
                }
                if (!string.IsNullOrEmpty(teacherLastName))
                {
                    whereClauses.Add("i.LastName = @LastName");
                }
                if (matsAvailable.HasValue)
                {
                    whereClauses.Add("yc.MatsAvailable = @MatsAvailable");
                }

                string whereClause = whereClauses.Any() ? "WHERE " + string.Join(" AND ", whereClauses) : string.Empty;

                string query = $@"select yc.ClassID, yc.ClassName, yc.StartTime, yc.EndTime, yc.ClassDate, yc.MatsAvailable, yc.ClassDescription,
                cl.BuildingName, cl.RoomNumber, cl.LocationAddress, i.FirstName, i.LastName
                from dbo.YogaClass as yc
                inner join dbo.instructor i on yc.InstructorID = i.InstructorID
                inner join dbo.classLocation cl on yc.LocationID = cl.LocationID
                {whereClause}; ";

                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    if (!string.IsNullOrEmpty(buildingName))
                    {
                        myCommand.Parameters.AddWithValue("@BuildingName", buildingName);
                    }
                    if (!string.IsNullOrEmpty(teacherFirstName))
                    {
                        myCommand.Parameters.AddWithValue("@FirstName", teacherFirstName);
                    }
                    if (!string.IsNullOrEmpty(teacherLastName))
                    {
                        myCommand.Parameters.AddWithValue("@LastName", teacherLastName);
                    }
                    if (matsAvailable.HasValue)
                    {
                        myCommand.Parameters.AddWithValue("@MatsAvailable", matsAvailable.Value);
                    }

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

        [HttpPost]
        [Route("AddYogaClassInformation")]
        public JsonResult AddYogaClassInformation(string className, string startTime,
        string endTime, string classDate, int instructorID, int locationID, int matsAvailable,
        string classDescription)
        {
            // Assume duration is in minutes and startTime is a string that can be parsed into a TimeSpan
            //TimeSpan durationTimeSpan = TimeSpan.FromMinutes(duration);
            TimeSpan startTimeSpan = TimeSpan.Parse(startTime); // Make sure to validate and handle exceptions in production code
            //TimeSpan endTimeSpan = startTimeSpan.Add(durationTimeSpan);
            TimeSpan endTimeSpan = TimeSpan.Parse(endTime);

            // Convert matsAvailable from int to boolean
            bool matsAvailableBool = matsAvailable > 0;

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

                    // Add parameters for the stored procedure here
                    myCommand.Parameters.AddWithValue("@ClassName", className);
                    myCommand.Parameters.AddWithValue("@StartTime", startTimeSpan);
                    myCommand.Parameters.AddWithValue("@EndTime", endTimeSpan);
                    myCommand.Parameters.AddWithValue("@ClassDate", DateTime.Parse(classDate)); // Ensure classDate is in a correct format
                    myCommand.Parameters.AddWithValue("@InstructorID", instructorID);
                    myCommand.Parameters.AddWithValue("@LocationID", locationID);
                    myCommand.Parameters.AddWithValue("@MatsAvailable", matsAvailableBool);
                    myCommand.Parameters.AddWithValue("@ClassDescription", classDescription);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Added Successfully");
        }

        [HttpPost]
        [Route("AddClass")]
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

                    // Add parameters for the stored procedure here
                    myCommand.Parameters.AddWithValue("@ClassName", yogaClass.className);
                    myCommand.Parameters.AddWithValue("@StartTime", yogaClass.startTime);
                    myCommand.Parameters.AddWithValue("@EndTime", yogaClass.endTime);
                    myCommand.Parameters.AddWithValue("@ClassDate", yogaClass.classDate);
                    myCommand.Parameters.AddWithValue("@InstructorID", yogaClass.instructorID);
                    myCommand.Parameters.AddWithValue("@LocationID", yogaClass.locationID);
                    myCommand.Parameters.AddWithValue("@MatsAvailable", yogaClass.matsAvailable);
                    myCommand.Parameters.AddWithValue("@ClassDescription", yogaClass.classDescription);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Added Successfully");
        }




        [HttpDelete]
        [Route("DeleteYogaClassInformation")]
        public JsonResult DeleteYogaClassInformation(int id)
        {
            string query = "delete from dbo.yogaclass where ClassID=@id";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Ksufreeyoga");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@id", id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Deleted Sucessfully");
        }
    }
}
