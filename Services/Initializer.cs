using Database;
using DTO.AppSettings;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Services;

public static partial class Initializer
{
    private static void CommonAddCoreSerices(WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog();

        builder.Services.Configure<SystemUserSettings>(builder.Configuration.GetSection("SystemUser"));

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.File(Path.Combine(AppContext.BaseDirectory, "logs", "app.log"), rollingInterval: RollingInterval.Day)
            .CreateLogger();

        builder.Services.AddControllers();
        builder.Services
            .AddEndpointsApiExplorer()
            .AddSwaggerGen()
            .AddHttpContextAccessor()
            .AddServiceInjection()
            ;
    }

    public static WebApplicationBuilder AddCoreServices(this WebApplicationBuilder builder)
    {
        CommonAddCoreSerices(builder);
        return builder;
    }
    public static WebApplicationBuilder AddCoreServices<TDbContext>(this WebApplicationBuilder builder) where TDbContext : Context
    {
        builder.AddCoreDbContext<TDbContext>();
        CommonAddCoreSerices(builder);
        return builder;
    }

    public static WebApplicationBuilder AddCoreDbContext<TDbContext>(this WebApplicationBuilder builder) where TDbContext : Context
    {
        builder.Services.AddDbContext<Context, TDbContext>(options =>
            options.UseSqlServer(builder.Configuration["DbConnectionString"]));

        return builder;
    }

    public static IApplicationBuilder UserCoreService(this IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.UseRouting();
        app.UseEndpoints(ep => ep.MapControllers());

        return app;
    }
}