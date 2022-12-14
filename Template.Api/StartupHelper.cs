using Template.Application;
using Template.Application.Interfaces;
using Template.Infrastructure.DataAccess;
using Template.Infrastructure.DataAccess.Repository;
using Template.Infrastructure.DataAccess.UnitOfWork;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Template.Common.Models;
using Template.Api.Filters;
using Template.Api.Services;

namespace Template.Api;

public static class StartupHelper
{
    public static void ConfigureServices(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

        services.AddControllers(opt => { opt.Filters.Add(typeof(GlobalExceptionFilter)); }).AddFluentValidation();

        services.AddHttpContextAccessor();
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy",
                builder => builder
                    .SetIsOriginAllowed((host) => true)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
        });

        services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });

        services.AddDbContext(config);
        services.AddMediatRDependency();
        services.AddApplicationDependencies(config);
        services.AddAuthentication(config);

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }

    public static void ConfigureMiddleware(this WebApplication app)
    {
        app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();
        app.UseCors("CorsPolicy");
        app.UseAuthorization();

        app.MapControllers();
    }

    public static void AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        IConfigurationSection appSettingsSection = configuration.GetSection("AppSettings");
        services.Configure<AppSettings>(appSettingsSection);
        var appSettings = appSettingsSection.Get<AppSettings>();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = appSettings.Authority;
                // options.RequireHttpsMetadata = true;
                // name of the API resource
                options.Audience = appSettings.Audience;
                // options.MetadataAddress = appSettings.MetadataAddress;
                options.BackchannelHttpHandler = new HttpClientHandler { ServerCertificateCustomValidationCallback = delegate { return true; } };
            });
        services.AddAuthorization(options =>
        {
            options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .Build();
        });

    }

    public static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationContext>(options => { options.UseSqlServer(configuration["DATABASE_CON"]); });

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IUnitOfWork, UnitofWork>();
    }

    public static void AddApplicationDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<ICurrentUserService, CurrentUserService>();

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetValue<string>("REDIS");
            options.InstanceName = "Template.Notifications";
        });
    }
}