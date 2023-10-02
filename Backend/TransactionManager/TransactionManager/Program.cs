
using TransactionManager.Models.DBModels;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using TransactionManager.Repository;
using Microsoft.Extensions.DependencyInjection;
using TransactionManager.Service.APILoggingService;
using System.Text;

namespace TransactionManager;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var configuration = new ConfigurationBuilder()
            .SetBasePath(builder.Environment.ContentRootPath)
            .AddJsonFile("appsettings.json")
            .AddJsonFile("usersettings.json").Build();
        builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
            .AddJsonFile("usersettings.json", optional: false, reloadOnChange: false)
            .AddEnvironmentVariables()
            .Build();
        // Add services to the container.
        builder.Services.AddDbContext<DatabaseContext>(options =>
        {
            options.UseMySql(configuration.GetConnectionString("DefaultConnection"),
                new MySqlServerVersion(new Version(8, 1, 0)));
        });
        builder.Services.AddScoped<APILogsRepository>();

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience = configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Secret"]))
            };
        });
        var app = builder.Build();
        using (var scope = app.Services.CreateScope())
        {
            var serviceProvider = scope.ServiceProvider;

            try
            {
                var context = serviceProvider.GetRequiredService<DatabaseContext>();
                // Test the database connection by opening and closing it
                context.Database.OpenConnection();
                context.Database.CloseConnection();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error connecting to the database: " + ex.Message);
                Environment.Exit(1);
            }
        }
        app.UsePathBase("/api");
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        // Load the SSL certificate files
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseRouting();
        app.MapControllers();
       
        app.Run();
    }
    
}

