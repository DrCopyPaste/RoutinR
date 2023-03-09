
using External.TestApi.Authentication;
using Microsoft.OpenApi.Models;

namespace External.TestApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(option =>
                {
                    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
                    option.AddSecurityDefinition("ApiUserDefinition", new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Description = "Please enter a valid api user",
                        Name = Constants.Api_User,
                        Type = SecuritySchemeType.ApiKey,
                        //BearerFormat = "JWT",
                        //Scheme = "Basic"
                    });
                    option.AddSecurityDefinition("ApiKeyDefinition", new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Description = "Please enter a valid api key",
                        Name = Constants.Api_Key,
                        Type = SecuritySchemeType.ApiKey,
                        //BearerFormat = "JWT",
                        //Scheme = "Basic"
                    });
                    option.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type=ReferenceType.SecurityScheme,
                                    Id="ApiUserDefinition"
                                }
                            },
                            new string[]{}
                        },
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type=ReferenceType.SecurityScheme,
                                    Id="ApiKeyDefinition"
                                }
                            },
                            new string[]{}
                        }
                    });
                });

            builder.Services.AddScoped<AuthFilter>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            var summaries = new[]
            {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

            app.MapGet("/weatherforecast", (HttpContext httpContext) =>
            {
                var forecast = Enumerable.Range(1, 5).Select(index =>
                    new WeatherForecast
                    {
                        Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                        TemperatureC = Random.Shared.Next(-20, 55),
                        Summary = summaries[Random.Shared.Next(summaries.Length)]
                    })
                    .ToArray();
                return forecast;
            })
            .WithName("GetWeatherForecast")
            .WithOpenApi();

            app.MapPost("/" +  Constants.Timesheet_Post_Route, /*async*/ (ApiTimeSheetEntry timeSheetEntry) =>
            {
                return timeSheetEntry;
            })
           .WithName("PostTimeSheet")
           .WithOpenApi()
           .AddEndpointFilter<AuthFilter>();

            app.Run();
        }
    }
}