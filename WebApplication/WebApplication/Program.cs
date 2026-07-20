using Asp.Versioning;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Options;
using Scalar.AspNetCore;
using WebApplication.Middleware;
using WebApplication.Options;
using WebApplication.Services;
using WebApplication.Services.Interfaces;
using WebApplication.Validators;

using WebApplicationApp = Microsoft.AspNetCore.Builder.WebApplication;

var builder = WebApplicationApp.CreateBuilder(args);

builder.Services.Configure<PersonStoreOptions>(
    builder.Configuration.GetSection(PersonStoreOptions.SectionName));

builder.Services.AddSingleton<IPersonService>(sp =>
{
    var options = sp.GetRequiredService<IOptions<PersonStoreOptions>>().Value;
    var logger = sp.GetRequiredService<ILogger<PersonService>>();
    var filePath = Path.IsPathRooted(options.FilePath)
        ? options.FilePath
        : Path.Combine(builder.Environment.ContentRootPath, options.FilePath);
    return new PersonService(filePath, logger);
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new QueryStringApiVersionReader("api-version"),
        new HeaderApiVersionReader("X-API-Version"));
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<PersonCreateDtoValidator>();

builder.Services.AddProblemDetails(options =>
{
    options.CustomizeProblemDetails = ctx =>
    {
        ctx.ProblemDetails.Extensions["instance"] = ctx.HttpContext.Request.Path.Value ?? string.Empty;
    };
});
builder.Services.AddResponseCaching();
builder.Services.AddHealthChecks();
builder.Services.AddCors();

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseStaticFiles();
    
    app.MapOpenApi();

    app.MapScalarApiReference(options =>
    {
        options.WithTitle("Person API")
               .WithTheme(ScalarTheme.DeepSpace)
               .WithJavaScriptConfiguration("/scalar/config.js");
    });
}

app.Use(async (context, next) =>
{
    if (context.Request.Path == "/")
    {
        context.Response.Redirect("/scalar/");
        return;
    }
    await next();
});

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

var allowedOrigins = app.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];
app.UseCors(policy => policy
    .WithOrigins(allowedOrigins)
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseResponseCaching();

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health");

app.Run();