using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using UNOSChat.ConversationAPI.Data;
using UNOSChat.ConversationAPI.Hubs;
using UNOSChat.ConversationAPI.Services;
using UNOSChat.ConversationAPI.Services.Interfaces;

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

builder.Services.AddControllers();


builder.Services.AddDbContext<UNOSChatDbContext>(options =>
    options.UseMongoDB(@"mongodb://mongoadmin:LikeAnd@chatdb:27017", "UNOSChat")
);

builder.Services.AddSignalR();

var jwtSettings = builder.Configuration.GetSection("JwtSettings");


builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["validIssuer"],
            ValidAudience = jwtSettings["validAudience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["securityKey"]))
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];

                // If the request is for our hub...
                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) &&
                    (path.StartsWithSegments("/chathub")))
                {
                    // Read the token out of the query string
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            }
        };
    });



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "UNOs Chat API", Version = "v1" });

    // Agregar configuración de seguridad para Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] { }
            }
        });
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IConversationService, ConversationService>();


var app = builder.Build();

app.UseCors(allowThisOrigins);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "UNOs Chat API"));
}

//app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHub<ChatHub>("/chathub");

app.Run();
