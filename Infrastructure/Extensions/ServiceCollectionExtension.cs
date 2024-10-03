using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
namespace Infrastructure.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddDbContexts(this IServiceCollection services, IConfiguration configuration)
        {

            string sqlConnString = configuration.GetConnectionString("SQLEntities");


            services.AddDbContext<DbModelContext>(options =>
            {
                options.UseSqlServer(sqlConnString);
            }, ServiceLifetime.Scoped);

            // Configuración Dapper
            return services;
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration configuration, string xmlFileName)
        {
            // Configuracion Swagger
            services.AddSwaggerGen(doc =>
            {
                doc.SwaggerDoc(configuration["Swagger:Version"], new OpenApiInfo
                {
                    Title = configuration["Swagger:Title"],
                    Version = configuration["Swagger:Version"]
                });

                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFileName);
                doc.IncludeXmlComments(xmlPath);

                doc.AddSecurityDefinition(configuration["Swagger:SecurityName"], new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.ApiKey,
                    In = ParameterLocation.Header,
                    Name = configuration["Swagger:HeaderName"],
                    Description = configuration["Swagger:DescriptionToken"],
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });


                doc.AddSecurityRequirement(new OpenApiSecurityRequirement {
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

            return services;
        }

        /*public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<IAccountService, AccountService>();


            return services;
        }
        */

        public static IServiceCollection AddCorsApp(this IServiceCollection services)
        {
            // Configuracion CORS
            services.AddCors(options =>
            {
                options.AddPolicy("ApiCors", builder =>
                {
                    builder
                    .AllowAnyOrigin()
                    // Esto no va en produccion, sólo local
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithExposedHeaders("Authorization"); // Expone el token para que las Apps lo puedan ver
                    // .AllowCredentials()
                });
            });

            return services;
        }

        public static IServiceCollection AddElasticLogging(this IServiceCollection services, IConfiguration configuration, string environment)
        {
            ConfigureLogging(configuration, environment);

            return services;
        }

        public static void ConfigureLogging(IConfiguration configuration, string environment)
        {


        }
    }
}