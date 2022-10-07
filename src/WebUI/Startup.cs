using Common.Extensions;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NSwag;
using NSwag.Generation.Processors.Security;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Text;
using WeShare.Application;
using WeShare.Application.Common.Security;
using WeShare.Infrastructure;
using WeShare.Infrastructure.Options;
using WeShare.Infrastructure.Persistence;
using WeShare.WebAPI.Filters;
using WeShare.WebAPI.Services.Configuration;
using WeShare.WebUI.Filters;

namespace WeShare.WebAPI;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddApplication(Configuration, options =>
        {
            options.UseServiceLevels = true;
            options.ValidateServiceLevelsOnInitialize = true;
            options.IgnoreIServiceWithoutLifetime = false;
        },
        Assembly.GetExecutingAssembly(),
        Assembly.GetAssembly(typeof(ShareDbContext)),
        Assembly.GetAssembly(typeof(AuthorizationHandler<,>)));

        services.Configure<ApiBehaviorOptions>(options =>
            options.SuppressModelStateInvalidFilter = true);

        services.AddApplication();
        services.AddInfrastructure();

        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddHttpContextAccessor();

        services.AddHealthChecks()
            .AddDbContextCheck<ShareDbContext>();

        services.AddOpenApiDocument(configure =>
        {
            configure.Title = "WeShare API";
            configure.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme
            {
                Type = OpenApiSecuritySchemeType.ApiKey,
                Name = "Authorization",
                In = OpenApiSecurityApiKeyLocation.Header,
                Description = "Type into the textbox: Bearer {your JWT token}."
            });

            configure.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
        });

        services.AddControllers(options => options.Filters.Add<ApiExceptionFilterAttribute>());

        services.AddSingleton(provider =>
        {
            var securityOptions = provider.GetRequiredService<JwtOptions>();
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityOptions.JwtKey));

            return new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                RequireExpirationTime = true,
                ValidIssuer = securityOptions.JwtIssuer,
                ValidAudience = securityOptions.JwtIssuer,
                IssuerSigningKey = securityKey,
            };
        });

        services.ConfigureOptions<JwtBearerOptionsConfiguration>();

        services.AddSingleton<JwtSecurityTokenHandler>();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddScheme<JwtBearerOptions, JwtBearerHandler>(JwtBearerDefaults.AuthenticationScheme, null);
        services.AddAuthorization();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseHealthChecks("/health");

        app.UseStaticFiles();

        app.UseOpenApi();
        app.UseSwaggerUi3(settings =>
        {
            settings.Path = "/api";
            settings.DocumentPath = "/api/specification.json";
        });

        app.UseHangfireDashboard(options: new DashboardOptions()
        {
            Authorization = new[] { new HangfireDashboardAuthorizationFilter() },
            IsReadOnlyFunc = _ => env.IsProduction()
        });

        app.UseRouting();

        app.UseAuthentication();

        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapFallbackToFile("index.html");
        });
    }
}
