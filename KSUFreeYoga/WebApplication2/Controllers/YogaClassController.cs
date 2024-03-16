﻿using Microsoft.AspNetCore.Http;
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
            string query = "select distinct TeacherName from dbo.yogaclass";
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
            string query = "select distinct LocationID from dbo.yogaclass";
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
        public JsonResult AddYogaClassInformation(int classID, string className, string startTime, 
            int duration, string classDate, string teacherName, int locationID, 
            int matsAvailable, string classDescription)
        {
            string query = "insert into dbo.yogaclass values(@classID, @className, @startTime, @duration, @classDate, " +
                "@teacherName, @locationID, @matsAvailable, @classDescription)";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Ksufreeyoga");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@classID", classID);
                    myCommand.Parameters.AddWithValue("@className", className);
                    myCommand.Parameters.AddWithValue("@startTime", startTime);
                    myCommand.Parameters.AddWithValue("@duration", duration);
                    myCommand.Parameters.AddWithValue("@classDate", classDate);
                    myCommand.Parameters.AddWithValue("@teacherName", teacherName);
                    myCommand.Parameters.AddWithValue("@locationID", locationID);
                    myCommand.Parameters.AddWithValue("@matsAvailable", matsAvailable);
                    myCommand.Parameters.AddWithValue("@classDescription", classDescription);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Added Sucessfully");
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
