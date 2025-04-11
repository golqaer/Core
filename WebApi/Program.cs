
using DTO.AppSettings;
using Services;

namespace WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Add services to the container.

            builder.Services
                .AddCoreService(builder.Configuration)
                ;

            var app = builder.Build();

            app
                .UserCoreService(app.Environment)
                ;

            app.Run();
        }
    }
}
