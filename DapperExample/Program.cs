using FluentValidation;
using DapperExample.Employees;
using DapperExample.Database;

namespace DapperExample;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddSingleton<IDbConnectionFactory>(_ => 
                new SqliteDbConnectionFactory(
                    builder.Configuration["DbConnectionString"]!
                )
        );

        builder.Services.AddSingleton<IEmployeeService, EmployeeService>();
        builder.Services.AddValidatorsFromAssemblyContaining<Program>(ServiceLifetime.Singleton);

        var app = builder.Build();

        if(app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.MapEmployeeEndpoints();

        app.Run();
    }
}
