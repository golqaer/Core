using Database;
using DTO.AppSettings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace IdentityServer;

public class IdentityServerContext:Context
{
    public IdentityServerContext(DbContextOptions<IdentityServerContext> options, IOptions<SystemUserSettings> sysUser) : base(options, sysUser)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.UseOpenIddict();
    }
}