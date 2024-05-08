
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Google.Apis.Auth;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using IdentityModel;

namespace KStateFreeYogaAPI.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [HttpPost("signin-google")]
        public async Task<IActionResult> ValidateGoogleToken([FromBody] GoogleTokenModel tokenModel)
        {
            try
            {
                string googleClientID = _configuration["Authentication:Google:ClientID"];
                var validPayload = await GoogleJsonWebSignature.ValidateAsync(tokenModel.Token, new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new[] { googleClientID }
                });

                DataTable table = new DataTable();
                string query = @"
                        if not exists (select 1 from dbo.Users where GoogleID = @GoogleID)
                        begin
                            insert into dbo.Users (GoogleID, Email, FirstName, LastName)
                            values (@GoogleID, @Email, @FirstName, @LastName);
                        end
                        select UserID, GoogleID, Email, FirstName, LastName from dbo.Users where GoogleID = @GoogleID;";

                string sqlDataSource = _configuration.GetConnectionString("kstatefreeyogadata");
                SqlDataReader myReader;

                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.Parameters.AddWithValue("@GoogleID", validPayload.Subject);
                        myCommand.Parameters.AddWithValue("@Email", validPayload.Email);
                        myCommand.Parameters.AddWithValue("@FirstName", validPayload.GivenName);
                        myCommand.Parameters.AddWithValue("@LastName", validPayload.FamilyName);
                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader);
                        myReader.Close();
                    }
                    myCon.Close();
                }

                var user = table.Rows[0];
                var validToken = GenerateJwtToken(user);

                return Ok(new { token = validToken });

                //return new JsonResult(table);

            }
            catch (Exception ex)
            {
                return BadRequest(new { error = "Failed to validate token or database error", details = ex.Message });
            }
        }


      
        private string GenerateJwtToken(DataRow user)
        {
            var key = Encoding.ASCII.GetBytes(_configuration["JWT:Key"]);

            //var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var userClaims = BuildUserClaims(user);
            
            var securityKey = new SymmetricSecurityKey(key);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                claims: userClaims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private List<Claim> BuildUserClaims(DataRow user)
        {
            var userClaims = new List<Claim>()
            {
                new Claim(JwtClaimTypes.Id, user["UserID"].ToString()),
                new Claim(JwtClaimTypes.Email, user["Email"].ToString()), 
                new Claim(JwtClaimTypes.GivenName, user["FirstName"].ToString()), 
                new Claim(JwtClaimTypes.FamilyName, user["LastName"].ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            return userClaims;
        }

    

    }

    public class GoogleTokenModel
    {
        public string Token { get; set; }
    }

}
