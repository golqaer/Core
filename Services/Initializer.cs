using Database;
using DTO.AppSettings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Services;
public static class Initializer
{
    public static IServiceCollection AddCoreService(this IServiceCollection services, IConfiguration config)
    {
        var jwtSection = config.GetSection("Jwt");
        var jwtSettings = jwtSection.Get<JwtAppSettings>();

        services.Configure<JwtAppSettings>(jwtSection);

        services.AddDbContext<Context>(options =>
            options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

        return services;
    }
}