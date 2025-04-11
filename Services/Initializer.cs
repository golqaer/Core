using Database;
using DTO.AppSettings;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Services;

public static partial class Initializer
{
    public static IServiceCollection AddCoreService(this IServiceCollection services, IConfiguration config)
    {
        services.AddControllers();
        services.AddDbContext<Context>(options =>
            options.UseSqlServer(config["DbConnectionString"]))
            .AddEndpointsApiExplorer()
            .AddSwaggerGen()
            .AddHttpContextAccessor()
            .AddServiceInjection()
            ;

        return services;
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