using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text;

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
        }


        [HttpGet]
        [Route("GetYogaClassInformationWithMats")]
        public JsonResult GetYogaClassInformationWithMats()
        {
            string query = "select * from dbo.yogaclass where MatsAvailable = 1";
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
        [Route("FilterYogaClasses")]
        public JsonResult FilterYogaClasses(string? buildingName, string? teacherName, bool? matsAvailable)
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
                if (!string.IsNullOrEmpty(teacherName))
                {
                    whereClauses.Add("yc.TeacherName = @TeacherName");
                }
                if (matsAvailable.HasValue)
                {
                    whereClauses.Add("yc.MatsAvailable = @MatsAvailable");
                }

                string whereClause = whereClauses.Any() ? "WHERE " + string.Join(" AND ", whereClauses) : string.Empty;

                string query = $@"SELECT yc.ClassID, yc.ClassName, yc.StartTime, yc.Duration, yc.ClassDate, yc.TeacherName, yc.MatsAvailable, yc.ClassDescription,
                          cl.BuildingName, cl.RoomNumber, cl.LocationAddress
                          FROM dbo.YogaClass as yc
                          INNER JOIN dbo.ClassLocation as cl ON yc.LocationID = cl.LocationID
                          {whereClause};";

                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    if (!string.IsNullOrEmpty(buildingName))
                    {
                        myCommand.Parameters.AddWithValue("@BuildingName", buildingName);
                    }
                    if (!string.IsNullOrEmpty(teacherName))
                    {
                        myCommand.Parameters.AddWithValue("@TeacherName", teacherName);
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
        [Route("FilterYogaClassInformation")]
        public JsonResult FilterYogaClassInformation(bool? matsAvailable, int? locationID, string teacherName)
        {
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Ksufreeyoga");
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand())
                {
                    myCommand.Connection = myCon;
                    StringBuilder queryBuilder = new StringBuilder("SELECT * FROM dbo.yogaclass WHERE 1=1");

                    if (matsAvailable.HasValue)
                    {
                        queryBuilder.Append(" AND MatsAvailable = @MatsAvailable");
                        myCommand.Parameters.AddWithValue("@MatsAvailable", matsAvailable.Value);
                    }
                    if (locationID.HasValue)
                    {
                        queryBuilder.Append(" AND LocationID = @LocationID");
                        myCommand.Parameters.AddWithValue("@LocationID", locationID.Value);
                    }
                    if (!string.IsNullOrEmpty(teacherName))
                    {
                        queryBuilder.Append(" AND TeacherName = @TeacherName");
                        myCommand.Parameters.AddWithValue("@TeacherName", teacherName);
                    }

                    myCommand.CommandText = queryBuilder.ToString();

                    SqlDataReader myReader = myCommand.ExecuteReader();
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
        int duration, string classDate, string teacherName, string buildingName,
        string roomNumber, string locationAddress, int matsAvailable,
        string classDescription)
        {
            // You will call a stored procedure here instead of a raw SQL query.
            string procedureName = "AddYogaClassWithLocation";
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
                    myCommand.Parameters.AddWithValue("@className", className);
                    myCommand.Parameters.AddWithValue("@startTime", startTime);
                    myCommand.Parameters.AddWithValue("@duration", duration);
                    myCommand.Parameters.AddWithValue("@classDate", classDate);
                    myCommand.Parameters.AddWithValue("@teacherName", teacherName); // You'll likely need to change this to TeacherID if you normalize your tables.
                    myCommand.Parameters.AddWithValue("@buildingName", buildingName);
                    myCommand.Parameters.AddWithValue("@roomNumber", roomNumber);
                    myCommand.Parameters.AddWithValue("@locationAddress", locationAddress);
                    myCommand.Parameters.AddWithValue("@matsAvailable", matsAvailable);
                    myCommand.Parameters.AddWithValue("@classDescription", classDescription);

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
