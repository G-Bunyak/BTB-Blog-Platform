#region Imports
using BlogPlatform.Helpers;
using BlogPlatform.Interfaces.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BlogPlatform.Interfaces.Services;
using BlogPlatform.Services;
#endregion

var builder = WebApplication.CreateBuilder(args);

#region Services
// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(p => p.AddPolicy("corspolicy", builder =>
{
    builder.WithOrigins("https://localhost:3000", "http://localhost:3000").AllowAnyMethod().AllowAnyHeader();
    builder.WithOrigins("https://127.0.0.1:5173", "http://127.0.0.1:5173").AllowAnyMethod().AllowAnyHeader();
}));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.RequireHttpsMetadata = false;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidIssuer = builder.Configuration["AuthOptions:Issuer"],
                            ValidateAudience = true,
                            ValidAudience = builder.Configuration["AuthOptions:Audience"],
                            ValidateLifetime = true,
                            ClockSkew = TimeSpan.Zero,
                            IssuerSigningKey = new SymmetricSecurityKey(
                                Encoding.ASCII.GetBytes(builder.Configuration["AuthOptions:Key"])),
                            ValidateIssuerSigningKey = true,
                        };
                    });

builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseSqlServer(builder.Configuration["DatabaseConnectionString"]));
builder.Services.AddScoped(typeof(IDatabaseHelper), typeof(DatabaseHelper));

builder.Services.AddSingleton(builder.Configuration);
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<IPostsService, PostsService>();
#endregion

#region App
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("corspolicy");

app.UseMiddleware<MiddlewareLoggerHelper>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
#endregion