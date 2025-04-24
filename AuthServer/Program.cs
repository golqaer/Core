using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace IdentityServer
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services
                .AddEndpointsApiExplorer()
                .AddSwaggerGen()
                .AddHttpContextAccessor()
                ;

            builder.Services.AddDbContext<IdentityServerContext>(options =>
                options.UseSqlServer(builder.Configuration["DbConnectionString"]));

            builder.Services.AddIdentity<User, IdentityRole>(opts =>
                {
                    opts.Password.RequireDigit = true;
                    opts.Password.RequiredLength = 8;
                    opts.User.RequireUniqueEmail = true;
                })
                .AddEntityFrameworkStores<IdentityServerContext>()
                .AddDefaultTokenProviders();

            builder.Services.Configure<List<OpenIddictClientDefinition>>(builder.Configuration.GetSection("OpenIddictClients"));

            string[] allowedOrigins = builder.Configuration.GetSection("OpenIddictClients")
                .Get<List<OpenIddictClientDefinition>>()?
                .Select(c => c.BaseUrl.TrimEnd('/'))
                .Distinct()
                .ToArray() ?? []
                ;
            builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
            {
                policy
                    .WithOrigins(allowedOrigins)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
                    ;
            }));

            builder.Services
                .AddOpenIddict()
                .AddCore(options => options.UseEntityFrameworkCore().UseDbContext<IdentityServerContext>())
                .AddServer(options =>
                {
                    options.SetAuthorizationEndpointUris("/connect/authorize");
                    options.SetTokenEndpointUris("/connect/token");
                    options.SetUserInfoEndpointUris("/connect/userinfo");
                    options.SetEndSessionEndpointUris("/connect/logout");

                    options.AllowAuthorizationCodeFlow()
                        .AllowRefreshTokenFlow()
                        .RequireProofKeyForCodeExchange();

                    options.AcceptAnonymousClients();

                    options.AddEphemeralEncryptionKey()
                        .AddEphemeralSigningKey();

                    options.RegisterScopes(Scopes.OpenId, Scopes.Profile, Scopes.Email, Scopes.OfflineAccess);

                    options.UseAspNetCore();
                })
                ;

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app
                .UseHttpsRedirection()
                .UseAuthorization()
                .UseRouting()
                .UseEndpoints(ep => ep.MapControllers())
                .UseAuthentication()
                ;

            using(var scope = app.Services.CreateScope())
            {
                var srvs = scope.ServiceProvider;
                await Seed.SeedAsync(srvs);
            }

            app.Run();
        }
    }
}
