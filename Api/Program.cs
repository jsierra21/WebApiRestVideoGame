using Core.Interfaces;
using Core.Services;
using FluentValidation.AspNetCore;
using Infrastructure.Data;
using Infrastructure.Extensions;
using Infrastructure.Filters;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration; // allows both to access and to set up the config

// Add services to the container.

// Configuracion para implementar AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddAutoMapper(Assembly.Load("Application"));

// Configuracion para implementar MediatR

builder.Services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());

// Configurar la autenticación JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Authentication:Issuer"], // Cambiar a "Issuer" si lo has definido
        ValidAudience = builder.Configuration["Authentication:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Authentication:SecretKey"])),
    };
});

builder.Services.AddControllers(options =>
{
    options.Filters.Add<GlobalExceptionFilter>();
})
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;

        // Opciones de SOAP
        options.SerializerSettings.Formatting = Formatting.Indented;
        options.SerializerSettings.ContractResolver = new DefaultContractResolver();
    })
    .ConfigureApiBehaviorOptions(options =>
    {
        // Configuracion para definir si el ApiController controla las validaciones de ModelState
        // Si deseamos solo utilizar FluentValidation como validador debemos colocarlo en false
        options.SuppressModelStateInvalidFilter = false;
    })
    .AddFluentValidation(options =>
    {
        //options.AutomaticValidationEnabled = false;
        options.DisableDataAnnotationsValidation = true;
        options.ConfigureClientsideValidation(enabled: false);
        // Colocar true si se desea que Fluentvalidation se ejecute antes de llegar al controlador
        // Se buscan todas las validaciones de todos los modelos
        options.ImplicitlyValidateChildProperties = false;
        options.RegisterValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
        //options.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc(configuration["Swagger:Version"], new OpenApiInfo
    {
        Title = configuration["Swagger:Title"],
        Version = configuration["Swagger:Version"]
    });

    // using System.Reflection;
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

// Configuración de Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console() // o a un archivo, etc.
    .CreateLogger();

builder.Host.UseSerilog(); // Usa Serilog como el registrador


// Configuración de CORS
// Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("ApiCors",
        builder => builder
            .AllowAnyOrigin() // Permitir cualquier origen
            .AllowAnyHeader() // Permitir cualquier encabezado
            .AllowAnyMethod()); // Permitir cualquier método (GET, POST, etc.)
});

builder.Services.AddHttpContextAccessor(); // Para poder utilizar HttpContext en ShouldBeAnAdminRequirementHandler

// Configuramos el evento de Firebase
// Registrar servicios

builder.Services.AddDbContext<DbModelContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SQLEntities")));

builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddSingleton<IPasswordService, PasswordService>();
builder.Services.AddTransient<IUsuarioService, UsuarioService>();
builder.Services.AddTransient<IVideoJuegosService, VideoJuegosService>();


// Configuracion para controlar Filtros del Request y las Validaciones de las entidades
builder.Services.AddMvc();

_ = bool.TryParse(builder.Configuration["ElasticConfig:Enabled"], out bool isEnabledElastic);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Configuracion Swagger
app.UseSwagger();
app.UseSwaggerUI(option =>
{
    option.SwaggerEndpoint(configuration["Swagger:Url"], configuration["Swagger:DefinitionName"]);
    option.RoutePrefix = string.Empty;

    option.DocumentTitle = configuration["Swagger:DocumentTitle"];
    option.DocExpansion(DocExpansion.None);
});

// Version Net 5
app.UseRouting();
app.UseCors("ApiCors");
// Configuracion JWT
app.UseAuthentication();

app.UseAuthorization();
app.UseSerilogRequestLogging();
// Configurar la ruta SOAP

// Version Net 5
// Configurar la ruta RESTful
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
