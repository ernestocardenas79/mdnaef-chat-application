using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("ocelot.json", reloadOnChange:true, optional:false)
    .AddEnvironmentVariables();

builder.Services.AddOcelot(builder.Configuration);

var allowThisOrigins = "allowThisOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(allowThisOrigins, policy =>
    {
        policy.SetIsOriginAllowed(a => true)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});
var app = builder.Build();
app.UseCors(allowThisOrigins);
app.UseWebSockets();
await app.UseOcelot();

app.Run();




