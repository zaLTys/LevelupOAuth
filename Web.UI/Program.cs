using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;
using Microsoft.Win32;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddJsonOptions(configure => 
        configure.JsonSerializerOptions.PropertyNamingPolicy = null);

// create an HttpClient used for accessing the API
builder.Services.AddHttpClient("APIClient", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["WebApiRoot"]);
    client.DefaultRequestHeaders.Clear();
    client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
});


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
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
// Adds OpenID Connect authentication for authenticating users
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
