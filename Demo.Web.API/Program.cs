using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Clear default claim type mapping to use claims as they are in the token
//uncomment for auth
//JsonWebTokenHandler.DefaultInboundClaimTypeMap.Clear();

//uncomment for auth
// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//     .AddJwtBearer(options =>
//     {
//         // URL of the Identity Provider (IDP) to fetch metadata (discovery endpoint)
//         options.Authority = "https://localhost:5001";
//         
//         // Expected audience value in the token (API name)
//         options.Audience = "demowebapi";
//
//         options.TokenValidationParameters = new TokenValidationParameters
//         {
//             // Use "given_name" from the token as the Name claim
//             NameClaimType = "given_name",
//             // Use "role" from the token as the Role claim
//             RoleClaimType = "role",
//             // Accept only access tokens (type "at+jwt")
//             ValidTypes = new[] { "at+jwt" }
//         };
//     });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//uncomment for auth
//app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();