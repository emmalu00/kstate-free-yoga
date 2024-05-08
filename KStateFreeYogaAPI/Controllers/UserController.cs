using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

using System.Data;

//using Microsoft.IdentityModel.JsonWebTokens;
using System.IdentityModel.Tokens.Jwt;


namespace KStateFreeYogaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUser()
        {

            var userId = HttpContext.User.FindFirst("id")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            // Use the UserID to get information from the database
            DataTable table = new DataTable();
            string query = "SELECT * FROM dbo.Users WHERE UserID = @UserID";

            string sqlDataSource = _configuration.GetConnectionString("kstatefreeyogadata");
            SqlDataReader myReader;

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@UserID", userId);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                }
                myCon.Close();
            }

            // Convert the DataTable to a model if necessary, or just return the JSON result
            return new JsonResult(table);
        }

    }
}
