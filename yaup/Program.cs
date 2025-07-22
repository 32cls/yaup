using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using yaup.Hubs;

const string CORS_OPTIONS = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateSlimBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<Applicatio>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: CORS_OPTIONS,
                      policy =>
                      {
                          policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                      });
});

builder.Services.AddSignalR(e => e.EnableDetailedErrors = true);
builder.Services.AddSingleton<IGameService, GameService>();

var app = builder.Build();

app.UseCors(CORS_OPTIONS);
app.MapHub<GameHub>("/game");
app.Run();