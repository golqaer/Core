using Microsoft.AspNetCore.Identity;

namespace IdentityServer;

public class ApplicationUser : IdentityUser<Guid>
{
    public string? FullName { get; set; }
}