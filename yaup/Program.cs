using yaup.Hubs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;

const string CORS_OPTIONS = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateSlimBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: CORS_OPTIONS,
                      policy =>
                      {
                          policy.AllowAnyHeader().WithMethods(["POST", "GET"]).WithOrigins(["http://localhost:5500"]).AllowCredentials();
                      });
});

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddIdentityApiEndpoints<IdentityUser>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/"; // Your login path
        options.LogoutPath = "/"; // Your logout path
        options.SlidingExpiration = true;
        
        // For cross-origin requests, ensure cookies are sent across domains
        options.Cookie.SameSite = SameSiteMode.Lax;  // Needed for cross-origin cookies
        options.Cookie.HttpOnly = true;  // Cookie should be accessible only via HTTP(S), not JS
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;  // Set to Always if using HTTPS
    });

builder.Services.AddSignalR(e => e.EnableDetailedErrors = true);
builder.Services.AddSingleton<IGameService, GameService>();

var app = builder.Build();

app.UseCors(CORS_OPTIONS);
app.MapIdentityApi<IdentityUser>();
app.UseAuthentication();
app.MapHub<GameHub>("/game");
app.Run();