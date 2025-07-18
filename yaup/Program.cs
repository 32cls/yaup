using yaup.Hubs;

const string CORS_OPTIONS = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateSlimBuilder(args);

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