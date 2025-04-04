using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;
using Microsoft.Win32;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddJsonOptions(configure =>
        configure.JsonSerializerOptions.PropertyNamingPolicy = null);

JsonWebTokenHandler.DefaultInboundClaimTypeMap.Clear();

builder.Services.AddOpenIdConnectAccessTokenManagement();

// create an HttpClient used for accessing the API
builder.Services.AddHttpClient("DemoWebApiClient", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["DemoWebApiRoot"]);
    client.DefaultRequestHeaders.Clear();
    client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
//add token handler to client
}).AddUserAccessTokenHandler()
 .AddHttpMessageHandler(() => new LoggingHandler());


//add to configura authentication middleware
builder.Services.AddAuthentication(options =>
    {
        // Specifies the default authentication scheme used for authentication
        // Cookie-based authentication scheme is set as the default
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;

        // Specifies the default challenge scheme used when authentication is required
        // OpenID Connect is used for authentication challenges
        options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
    })
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
    {
        options.AccessDeniedPath = "/Authentication/AccessDenied";
    })
    .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
    {
        // Specifies the sign-in scheme to use cookie authentication for maintaining sessions
        options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

        // Sets the authority (Identity Provider, IDP) URL for authentication
        options.Authority = "https://localhost:5001"; //our IDP 
        options.ClientId = "webuiclient"; //should match client id in IDP
        options.ClientSecret = "secret"; //should match client secret in IDP
        options.ResponseType = "code"; //code flow, PKCE auto enabled, later about that

        //options.Scope.Add("openid"); //<<<requested by middleware by default
        //options.Scope.Add("profile"); //<<<requested by middleware by default
        //options.CallbackPath = new PathString("signin-oidc"); //redirect uri in IDP, also default

        options.ClaimActions.Remove("aud"); //remove filter
        options.ClaimActions.DeleteClaim("sid");//remove claim (acutally)
        options.ClaimActions.DeleteClaim("idp");//remove claim (acutally)

        options.Scope.Add("roles");
        
        options.Scope.Add("demowebapi.fullaccess");
        
        options.ClaimActions.MapJsonKey("role", "role");
        
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            NameClaimType = "name",
            RoleClaimType = "role"
        };
        
        options.GetClaimsFromUserInfoEndpoint = true;
        options.SaveTokens = true; //save tokens in cookie
        //options.SignedOutCallbackPath : default = host / port / signout - callback - oidc - register in IDP
    });
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler();
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=UI}/{action=Index}/{id?}");

app.Run();