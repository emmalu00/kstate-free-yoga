using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;


namespace KStateFreeYogaAPI.Controllers
{
    /// <summary>
    /// Contains CRUD operations for yoga classes 
    /// </summary>
    [Route("api/yogaclass")]
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
        /// Gets yoga class(es) from the database
        /// </summary>
        /// <param name="buildingName"> Building which class is held </param>
        /// <param name="instructorFullName"> Instructor full name </param>
        /// <param name="matsProvided"> Whether or not mats are provided </param>
        /// <returns> List of yoga classes </returns>
        [HttpGet]
        public JsonResult GetYogaClasses(int? classID, string? buildingName, string? instructorFullName, bool? matsProvided)
        {
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("kstatefreeyogadata");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                List<string> whereClauses = new List<string>();

                string instructorFirst = null;
                string instructorLast = null;
                if (!string.IsNullOrEmpty(instructorFullName))
                {
                    string[] names = instructorFullName.Trim().Split(' ');
                    instructorFirst = names[0];
                    instructorLast = names[1]; 
                }

                if (classID.HasValue) { whereClauses.Add("yc.ClassID = @ClassID"); }
                if (!string.IsNullOrEmpty(buildingName)) { whereClauses.Add("cl.BuildingName = @BuildingName"); }
                if (instructorFirst != null) { whereClauses.Add("i.FirstName = @FirstName"); }
                if (instructorLast != null) { whereClauses.Add("i.LastName = @LastName"); }
                if (matsProvided.HasValue) { whereClauses.Add("yc.MatsProvided = @MatsProvided"); }

                string whereClause = whereClauses.Any() ? "where " + string.Join(" and ", whereClauses) : string.Empty;
                string query = $@"select yc.ClassID, yc.ClassName, yc.StartTime, yc.EndTime, yc.ClassDate, yc.MatsProvided, yc.ClassDescription,
                    cl.LocationID, cl.BuildingName, cl.RoomNumber, cl.LocationAddress, i.InstructorID, i.FirstName, i.LastName, i.Certified
                    from dbo.YogaClass as yc
                    inner join dbo.instructor i on yc.InstructorID = i.InstructorID
                    inner join dbo.classLocation cl on yc.LocationID = cl.LocationID
                    {whereClause}; ";

                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    if (classID.HasValue) { myCommand.Parameters.AddWithValue("@classID", classID.Value); }
                    if (!string.IsNullOrEmpty(buildingName)) { myCommand.Parameters.AddWithValue("@BuildingName", buildingName); }
                    if (!string.IsNullOrEmpty(instructorFirst)) { myCommand.Parameters.AddWithValue("@FirstName", instructorFirst); }
                    if (!string.IsNullOrEmpty(instructorLast)) { myCommand.Parameters.AddWithValue("@LastName", instructorLast); }
                    if (matsProvided.HasValue) { myCommand.Parameters.AddWithValue("@MatsProvided", matsProvided.Value); }
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                }
                myCon.Close();
            }
            return new JsonResult(table);
        }

        /// <summary>
        /// Adds a yoga class to the database
        /// </summary>
        /// <param name="yogaClass"> Yoga class that is being added to the database </param>
        /// <returns> Message that the class was added successfully </returns>
        [HttpPost]
        public JsonResult AddClass(YogaClass yogaClass)
        {
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("kstatefreeyogadata");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                string query = @"
                        if not exists (
                            select 1 from dbo.yogaClass 
                            where StartTime = @StartTime and ClassDate = @ClassDate and 
                                   LocationID = @LocationID
                        )
                        begin
                            insert into dbo.YogaClass (ClassName, StartTime, EndTime, ClassDate, InstructorID, LocationID, MatsProvided, ClassDescription)
                            values (@ClassName, @StartTime, @EndTime, @ClassDate, @InstructorID, @LocationID, @MatsProvided, @ClassDescription)
                        end";
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@ClassName", yogaClass.ClassName);
                    myCommand.Parameters.AddWithValue("@StartTime", yogaClass.StartTime);
                    myCommand.Parameters.AddWithValue("@EndTime", yogaClass.EndTime);
                    myCommand.Parameters.AddWithValue("@ClassDate", yogaClass.ClassDate);
                    myCommand.Parameters.AddWithValue("@InstructorID", yogaClass.InstructorID);
                    myCommand.Parameters.AddWithValue("@LocationID", yogaClass.LocationID);
                    myCommand.Parameters.AddWithValue("@MatsProvided", yogaClass.MatsProvided);
                    myCommand.Parameters.AddWithValue("@ClassDescription", yogaClass.ClassDescription);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                }
                myCon.Close();
            }
            return new JsonResult("Class added Successfully");
        }

        /// <summary>
        /// Updates yoga class in the database
        /// </summary>
        /// <param name="classID"> ID of class to be updated </param>
        /// <param name="updatedYogaClass"> Updated class information </param>
        /// <returns> Message that class was updated successfully </returns>
        [HttpPut]
        public JsonResult UpdateClass(int classID, YogaClass updatedYogaClass)
        {
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("kstatefreeyogadata");
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                string query = @"
                    if exists (
                        select 1 
                        from dbo.yogaClass 
                        where StartTime = @StartTime and ClassDate = @ClassDate and 
                              InstructorID = @InstructorID and LocationID = @LocationID and ClassID != @ClassID
                    )
                    begin
                        delete from dbo.yogaClass 
                        where ClassID = @ClassID
                    end
                    else
                    begin
                        declare @OldClassDate DATE;
                        select @OldClassDate = ClassDate from dbo.yogaClass where ClassID = @ClassID;

                        update dbo.yogaClass
                        set ClassName = @ClassName, StartTime = @StartTime, EndTime = @EndTime, 
                            ClassDate = @ClassDate, InstructorID = @InstructorID, LocationID = @LocationID, 
                            MatsProvided = @MatsProvided, ClassDescription = @ClassDescription
                        where ClassID = @ClassID;

                        if @OldClassDate != @ClassDate
                        begin
                            update dbo.ClassAttendance
                            set AttendanceDate = @ClassDate
                            where ClassID = @ClassID and AttendanceDate = @OldClassDate;
                        end
                    end";

                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@ClassID", classID);
                    myCommand.Parameters.AddWithValue("@ClassName", updatedYogaClass.ClassName);
                    myCommand.Parameters.AddWithValue("@StartTime", updatedYogaClass.StartTime);
                    myCommand.Parameters.AddWithValue("@EndTime", updatedYogaClass.EndTime);
                    myCommand.Parameters.AddWithValue("@ClassDate", updatedYogaClass.ClassDate);
                    myCommand.Parameters.AddWithValue("@InstructorID", updatedYogaClass.InstructorID);
                    myCommand.Parameters.AddWithValue("@LocationID", updatedYogaClass.LocationID);
                    myCommand.Parameters.AddWithValue("@MatsProvided", updatedYogaClass.MatsProvided);
                    myCommand.Parameters.AddWithValue("@ClassDescription", updatedYogaClass.ClassDescription);

                    using (SqlDataReader myReader = myCommand.ExecuteReader())
                    {
                        table.Load(myReader);
                    }
                }
                myCon.Close();
            }
            return new JsonResult("Class updated successfully");
        }

        /// <summary>
        /// Deletes yoga class from the database if the class is in the future.
        /// Additionally, it deletes the instructor and location if they are not linked to any other classes.
        /// </summary>
        /// <param name="classID">ID of the class to be deleted</param>
        /// <returns>Whether or not the deletion is successful</returns>
        [HttpDelete]
        public JsonResult DeleteYogaClass(int classID)
        {
            string sqlDataSource = _configuration.GetConnectionString("kstatefreeyogadata");
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();

                // Check if the class exists
                string queryCheck = "select 1 from dbo.YogaClass where ClassID = @ClassID";
                using (SqlCommand myCommandCheck = new SqlCommand(queryCheck, myCon))
                {
                    myCommandCheck.Parameters.AddWithValue("@ClassID", classID);
                    object result = myCommandCheck.ExecuteScalar();
                    if (result == null)
                    {
                        return new JsonResult("No class with the provided ID exists.");
                    }
                }

                // Get instructor and location ID before deletion
                string queryInfo = "select InstructorID, LocationID from dbo.yogaclass where ClassID = @ClassID";
                int instructorID, locationID;
                using (SqlCommand commandInfo = new SqlCommand(queryInfo, myCon))
                {
                    commandInfo.Parameters.AddWithValue("@ClassID", classID);
                    using (SqlDataReader reader = commandInfo.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            return new JsonResult("Error retrieving class details.");
                        }
                        instructorID = reader.GetInt32(0);
                        locationID = reader.GetInt32(1);
                    }
                }

                // Delete all attendance records for the class
                string queryDeleteAttendance = "delete from dbo.ClassAttendance where ClassID = @ClassID";
                using (SqlCommand commandDeleteAttendance = new SqlCommand(queryDeleteAttendance, myCon))
                {
                    commandDeleteAttendance.Parameters.AddWithValue("@ClassID", classID);
                    commandDeleteAttendance.ExecuteNonQuery();
                }

                // Delete the class
                string queryDelete = "delete from dbo.yogaclass where ClassID = @ClassID";
                using (SqlCommand myCommandDelete = new SqlCommand(queryDelete, myCon))
                {
                    myCommandDelete.Parameters.AddWithValue("@ClassID", classID);
                    myCommandDelete.ExecuteNonQuery();
                }

                // Check if the instructor is linked to any other classes
                string queryInstructor = "select count(1) from dbo.yogaclass where InstructorID = @InstructorID";
                using (SqlCommand commandInstructor = new SqlCommand(queryInstructor, myCon))
                {
                    commandInstructor.Parameters.AddWithValue("@InstructorID", instructorID);
                    int count = Convert.ToInt32(commandInstructor.ExecuteScalar());
                    if (count == 0)
                    {
                        // Delete the instructor
                        string deleteInstructor = "delete from dbo.instructor where InstructorID = @InstructorID";
                        using (SqlCommand commandDeleteInstructor = new SqlCommand(deleteInstructor, myCon))
                        {
                            commandDeleteInstructor.Parameters.AddWithValue("@InstructorID", instructorID);
                            commandDeleteInstructor.ExecuteNonQuery();
                        }
                    }
                }

                // Check if the location is linked to any other classes
                string queryLocation = "select count(1) from dbo.yogaclass where LocationID = @LocationID";
                using (SqlCommand commandLocation = new SqlCommand(queryLocation, myCon))
                {
                    commandLocation.Parameters.AddWithValue("@LocationID", locationID);
                    int count = Convert.ToInt32(commandLocation.ExecuteScalar());
                    if (count == 0)
                    {
                        // Delete the location
                        string deleteLocation = "delete from dbo.ClassLocation where LocationID = @LocationID";
                        using (SqlCommand commandDeleteLocation = new SqlCommand(deleteLocation, myCon))
                        {
                            commandDeleteLocation.Parameters.AddWithValue("@LocationID", locationID);
                            commandDeleteLocation.ExecuteNonQuery();
                        }
                    }
                }

                myCon.Close();
            }
            return new JsonResult("Deleted Successfully");
        }
    }
}
