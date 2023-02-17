using System.Globalization;
using FluentValidation;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TBCTask.API.Middlewares;
using TBCTask.Domain.Interfaces;
using TBCTask.Domain.Interfaces.IServices;
using TBCTask.Domain.Models;
using TBCTask.Infrastructure;
using TBCTask.Services;
using TBCTask.Services.Validators;

namespace TBCTask.API;

public class Startup
{
    public IConfiguration configRoot { get; }

    public Startup(IConfiguration configuration)
    {
        configRoot = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddLocalization(options => options.ResourcesPath = "Resources");
        services.Configure<RequestLocalizationOptions>(options =>
        {
            var supportedCultures = new List<CultureInfo>
            {
                new CultureInfo("en-US"),
                new CultureInfo("ka-GE"),
            };

            options.DefaultRequestCulture = new RequestCulture("en-US");
            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;
        });
        services.AddDbContext<TBCTaskDbContext>(options =>
            options.UseSqlServer(configRoot.GetConnectionString("DefaultConnection")));
        services.AddScoped<IValidator<PersonModel>, PersonValidator>();
        services.AddScoped<IValidator<PersonPhoneNumberModel>, NumberValidator>();
        services.AddScoped<IValidator<RelatedPersonModel>, RelatedPersonValidator>();
        services.AddScoped<IPersonService, PersonService>();
        services.AddScoped<IRelatedPersonService, RelatedPersonService>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }

    public void Configure(WebApplication app, IWebHostEnvironment env)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseMiddleware<CultureMiddleware>();
        app.UseMiddleware<ErrorLoggingMiddleware>();
        app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}