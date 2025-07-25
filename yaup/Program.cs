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
                          policy.AllowAnyHeader()
                                .WithMethods(["POST", "GET"])
                                .WithOrigins(["http://localhost:5500"])
                                .AllowCredentials();
                      });
});

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddIdentityApiEndpoints<IdentityUser>()
    .AddEntityFrameworkStores<ApplicationDbContext>();    

builder.Services.AddSignalR(e => e.EnableDetailedErrors = true);
builder.Services.AddSingleton<IGameService, GameService>();

var app = builder.Build();
app.UseStaticFiles();
app.UseCors(CORS_OPTIONS);
app.UseAuthentication();
app.UseAuthorization();
app.MapIdentityApi<IdentityUser>();
app.MapHub<GameHub>("/game");
app.Run();