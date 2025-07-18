using yaup.Hubs;

var builder = WebApplication.CreateSlimBuilder(args);

var corsOptions = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: corsOptions,
                      policy =>
                      {
                          policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                      });
});

builder.Services.AddSignalR(e => e.EnableDetailedErrors = true);
builder.Services.AddSingleton<IGameService, GameService>();

var app = builder.Build();

app.UseCors(corsOptions);
app.MapHub<GameHub>("/game");
app.Run();