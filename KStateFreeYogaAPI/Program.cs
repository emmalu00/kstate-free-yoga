using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var services = builder.Services;
var configuration = builder.Configuration;

// Configure  Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

    //options.DefaultAuthenticateScheme = Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme;
   // options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
//.AddCookie() // Adds cookie authentication
.AddGoogle(googleOptions =>
{
    googleOptions.ClientId = "1099353898213-senp1ppvp9hugivm11uid6ur7aksle2h.apps.googleusercontent.com"; // Add to appsettings.json
    googleOptions.ClientSecret = "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";
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        // Validate the JWT Issuer (iss) claim
        ValidateIssuer = true,
        ValidIssuer = "http://localhost:44374",

        // Validate the JWT Audience (aud) claim
        ValidateAudience = true,
        ValidAudience = "http://localhost:44374",

        // Validate the token expiry
        ValidateLifetime = true,

        // The signing key must match!
        ValidateIssuerSigningKey = true,
        // You should replace this with your actual secret key
        // and keep it out of your codebase
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("RmmCM3qeODEq5LsvZrxaJphbQGYE2nD2")),

        // If you want to allow a certain amount of clock skew for your tokens, you can set that here:
        // ClockSkew = TimeSpan.Zero,
    };
}); 

//JSON serializer settiings
builder.Services.AddControllers().AddNewtonsoftJson(options =>
options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore).AddNewtonsoftJson(
    options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());

services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins("http://localhost:3000") // Set the correct client URL here
                           .AllowAnyHeader()
                           .AllowAnyMethod());
});


/*builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // Validate the JWT Issuer (iss) claim
            ValidateIssuer = true,
            ValidIssuer = "http://localhost:44374",

            // Validate the JWT Audience (aud) claim
            ValidateAudience = true,
            ValidAudience = "http://localhost:44374",

            // Validate the token expiry
            ValidateLifetime = true,

            // The signing key must match!
            ValidateIssuerSigningKey = true,
            // You should replace this with your actual secret key
            // and keep it out of your codebase
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("RmmCM3qeODEq5LsvZrxaJphbQGYE2nD2")),

            // If you want to allow a certain amount of clock skew for your tokens, you can set that here:
           // ClockSkew = TimeSpan.Zero,
        };

        // You can add more options here if needed, for example, to wire up events like OnMessageReceived
    });*/

builder.Services.AddAuthorization();

var app = builder.Build();



//Enadble CORS
//app.UseCors(c => c.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.UseCors("AllowSpecificOrigin");


app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();