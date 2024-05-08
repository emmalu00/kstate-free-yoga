using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace KStateFreeYogaAPI.Controllers
{
    /// <summary>
    /// Contains CRUD operations for an attendance record
    /// </summary>
    [Route("api/classattendance")]
    [ApiController]
    public class ClassAttendanceController : ControllerBase
    {

        /// <summary>
        /// Holds the configuration settings of the application.
        /// </summary>
        private IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the ClassLocationController with the specified configuration settings.
        /// </summary>
        /// <param name="configuration"> Represent's the application's configuration settings. </param>
        public ClassAttendanceController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult GetAttendanceRecords(int? attendanceID, int? userID, int? classID, string? attendanceStatus, bool? favorited)
        {
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("kstatefreeyogadata");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                List<string> whereClauses = new List<string>();

                if (attendanceID.HasValue) { whereClauses.Add("a.AttendanceID = @AttendanceID"); }
                if (userID.HasValue) { whereClauses.Add("a.UserID = @UserID"); }
                if (classID.HasValue) { whereClauses.Add("a.ClassID = @ClassID"); }
                if (!string.IsNullOrEmpty(attendanceStatus)) { whereClauses.Add("a.AttendanceStatus = @AttendanceStatus"); }
                if (favorited.HasValue) { whereClauses.Add("a.Favorited = @Favorited"); }

                string whereClause = whereClauses.Any() ? " where " + string.Join(" and ", whereClauses) : string.Empty;
                string query = $@"
                    select a.AttendanceID, a.UserID, a.ClassID, a.AttendanceDate, a.AttendanceStatus, a.Favorited
                    from dbo.ClassAttendance a
                    {whereClause}
                    order by a.AttendanceDate desc;";

                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    if (attendanceID.HasValue) { myCommand.Parameters.AddWithValue("@AttendanceID", attendanceID.Value); }
                    if (userID.HasValue) { myCommand.Parameters.AddWithValue("@UserID", userID.Value); }
                    if (classID.HasValue) { myCommand.Parameters.AddWithValue("@ClassID", classID.Value); }
                    if (!string.IsNullOrEmpty(attendanceStatus)) { myCommand.Parameters.AddWithValue("@AttendanceStatus", attendanceStatus); }
                    if (favorited.HasValue) { myCommand.Parameters.AddWithValue("@Favorited", favorited.Value); }
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                }
                myCon.Close();
            }
            return new JsonResult(table);
        }

        [HttpPost]
        public JsonResult AddClassAttendance(int userID, int classID)
        {
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("kstatefreeyogadata");
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();

                // Retrieve the ClassDate and StartTime for the given classID
                string queryClassDateTime = "SELECT ClassDate, StartTime FROM dbo.YogaClass WHERE ClassID = @ClassID";
                SqlCommand myCommandClassDateTime = new SqlCommand(queryClassDateTime, myCon);
                myCommandClassDateTime.Parameters.AddWithValue("@ClassID", classID);
                SqlDataReader reader = myCommandClassDateTime.ExecuteReader();

                DateTime classDate = DateTime.MinValue;
                TimeSpan startTime = TimeSpan.MinValue;

                if (reader.Read())
                {
                    classDate = Convert.ToDateTime(reader["ClassDate"]);
                    startTime = TimeSpan.Parse(reader["StartTime"].ToString());
                }
                reader.Close();

                if (classDate == DateTime.MinValue || startTime == TimeSpan.MinValue)
                {
                    return new JsonResult("No such class exists.");
                }

                // Combine ClassDate and StartTime to form AttendanceDate as a string
                string attendanceDateAsString = classDate.Date.ToString("yyyy-MM-dd") + " " + startTime.ToString(@"hh\:mm\:ss");

                // Insert the new ClassAttendance record
                string queryInsert = @"
                    INSERT INTO dbo.ClassAttendance (UserID, ClassID, AttendanceDate, AttendanceStatus, Favorited)
                    VALUES (@UserID, @ClassID, CONVERT(DATETIME, @AttendanceDate, 120), 'attended', 0);";

                using (SqlCommand myCommandInsert = new SqlCommand(queryInsert, myCon))
                {
                    myCommandInsert.Parameters.AddWithValue("@UserID", userID);
                    myCommandInsert.Parameters.AddWithValue("@ClassID", classID);
                    myCommandInsert.Parameters.AddWithValue("@AttendanceDate", attendanceDateAsString);
                    myCommandInsert.ExecuteNonQuery();
                }

                myCon.Close();
            }
            return new JsonResult("Attendance successfully added.");
        }


        [HttpDelete]
        public JsonResult DeleteClassAttendance(int attendanceID)
        {
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("kstatefreeyogadata");
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();

                // Retrieve the ClassDate for the given classID
                string queryDelete = "delete from dbo.classattendance where AttendanceID = @AttendanceID"; ;
                using (SqlCommand myCommandDelete = new SqlCommand(queryDelete, myCon))
                {
                    myCommandDelete.Parameters.AddWithValue("@AttendanceID", attendanceID);
                    myCommandDelete.ExecuteNonQuery();
                }
                myCon.Close();
            }
            return new JsonResult("Attendance successfully deleted.");
        }

        [HttpPut]
        public JsonResult UpdateAttendanceFavorited(int attendanceID, bool favorited)
        {
            string sqlDataSource = _configuration.GetConnectionString("kstatefreeyogadata");
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();

                // Check if the attendance record exists
                string queryCheckAttendance = "SELECT COUNT(*) FROM dbo.ClassAttendance WHERE AttendanceID = @AttendanceID";
                SqlCommand myCommandCheckAttendance = new SqlCommand(queryCheckAttendance, myCon);
                myCommandCheckAttendance.Parameters.AddWithValue("@AttendanceID", attendanceID);
                int attendanceCount = (int)myCommandCheckAttendance.ExecuteScalar();

                if (attendanceCount == 0)
                {
                    return new JsonResult("Attendance record not found.");
                }

                // Update the favorited status
                string queryUpdateFavorited = @"
                    UPDATE dbo.ClassAttendance
                    SET Favorited = @Favorited
                    WHERE AttendanceID = @AttendanceID;";

                using (SqlCommand myCommandUpdateFavorited = new SqlCommand(queryUpdateFavorited, myCon))
                {
                    myCommandUpdateFavorited.Parameters.AddWithValue("@Favorited", favorited);
                    myCommandUpdateFavorited.Parameters.AddWithValue("@AttendanceID", attendanceID);
                    myCommandUpdateFavorited.ExecuteNonQuery();
                }

                myCon.Close();
            }
            return new JsonResult("Attendance record favorited status updated successfully.");
        }

    }
}
