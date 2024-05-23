using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UNOSChat.AuthenticationAPI.Extensions;
using UNOSChat.AuthenticationAPI.Models;
using UNOSChat.AuthenticationAPI.Repository;
using UNOSChat.AuthenticationAPI.Services;

var builder = WebApplication.CreateBuilder(args);

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

// Add services to the container.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureCors();
builder.Services.ConfigureSqlContext(builder.Configuration);

builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<RepositoryContext>();

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["validIssuer"],
        ValidAudience = jwtSettings["validAudience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
            .GetBytes(jwtSettings["securityKey"]))
    };

});

builder.Services.AddHttpClient();


builder.Services.AddScoped<JwtHandler>();

builder.Services.Configure<AccountServiceOptions>(options => options.ChatUrl = $"{builder.Configuration["ChatAPI"]}/user");
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddControllers();


var app = builder.Build();

app.UseCors(allowThisOrigins);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseAuthentication();

app.MapControllers();

ApplyMigration();

app.Run();

void ApplyMigration()
{
    using (var scope = app.Services.CreateScope())
    {
        var _db = scope.ServiceProvider.GetRequiredService<RepositoryContext>();

        if (_db.Database.GetPendingMigrations().Count() > 0)
        {
            _db.Database.Migrate();
        }
    }
}