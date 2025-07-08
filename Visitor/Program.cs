using Core.Entities.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using NLog;
using Presentaion.Attributes;
using System.Text.Json.Serialization;
using Presentaion;
using Microsoft.AspNetCore.Mvc.Formatters;
using FOE.Maintainance.Extensions;
using Persentation;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Microsoft.AspNetCore.ResponseCompression;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

LogManager.Setup().LoadConfigurationFromFile(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

builder.Services.AddControllers(cofig =>
{
    cofig.InputFormatters.Insert(0, GetJsonPatchInputFormatter());
}).AddJsonOptions(opt => {
    opt.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    opt.JsonSerializerOptions.WriteIndented = true;

})
 .AddApplicationPart(typeof(AssemblyReference).Assembly);
builder.Services.AddAuthentication();
builder.Services.AddScoped<ValidationFilterAttribute>();
builder.Services.ConfigureCORS()
.AddAutoMapper(typeof(Program))
.ConfigureLogger()
.ConfigureDbContext(builder.Configuration)
.ConfigureRepositoryManager()
.ConfigureServiceManager()
.ConfigureIdentity()
.ConfigureJWT(builder.Configuration);

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(e => e.Value.Errors.Count > 0)
            .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Errors.First().ErrorMessage // Only first error per field
            );

        var customResponse = new ResponseShape<object>(
            StatusCode: 400,
            message: "One or more validation errors occurred.",
            errors: errors,
            data: null
        );

        return new BadRequestObjectResult(customResponse);
    };
});
builder.Services.AddEndpointsApiExplorer();
// Add response compression services
//builder.Services.AddResponseCompression(options =>
//{
//    //  options.EnableForHttps = true;
//    options.Providers.Add<GzipCompressionProvider>();
//    // options.Providers.Add<BrotliCompressionProvider>(); // Optional
//});

// Configure compression level (optional)
//builder.Services.Configure<GzipCompressionProviderOptions>(options =>
//{
//    options.Level = System.IO.Compression.CompressionLevel.Fastest; // Or Optimal
//});


builder.Services.AddSwaggerGen(s =>
{
   // s.EnableAnnotations();
    s.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
    s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Place to add JWT with Bearer",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    s.AddSecurityRequirement(new OpenApiSecurityRequirement()
        {
        {
        new OpenApiSecurityScheme
        {
        Reference = new OpenApiReference
        {
        Type = ReferenceType.SecurityScheme,
        Id = "Bearer"

        },
        Name = "Bearer",
        },
        new List<string>()
        }
        });
});

builder.WebHost.UseUrls("http://*:5555", "http://0.0.0.0:5555");
var app = builder.Build();
//app.UseResponseCompression();
//.UseDirectoryBrowser();
app.HandleExceptions();
app.UseStaticFiles();
app.UseSwagger();
app.UseSwaggerUI();
//app.UseSwaggerUI(c =>
//{
//	//c.SwaggerEndpoint("/swagger/v2/swagger.json", "My API v2");
//	//c.RoutePrefix = "swagger";
//});
app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter() =>
new ServiceCollection().AddLogging().AddMvc().AddNewtonsoftJson()
.Services.BuildServiceProvider()
.GetRequiredService<IOptions<MvcOptions>>().Value.InputFormatters
.OfType<NewtonsoftJsonPatchInputFormatter>().First();

