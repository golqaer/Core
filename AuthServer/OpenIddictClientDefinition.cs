using IdentityServer.Enums;

namespace IdentityServer;

public class OpenIddictClientDefinition
{
    public string ClientId { get; set; } = default!;
    public string DisplayName { get; set; } = default!;
    public ClientType Type { get; set; }
    public ConsentType ConsentType { get; set; }
    public bool RequirePkce { get; set; } = true;
    public string BaseUrl { get; set; } = default!;
    public IEnumerable<string> Permissions { get; set; }  = new List<string>();
    public IEnumerable<string> GrantTypes { get; set; }  = new List<string>();
    public IEnumerable<string> ResponseTypes { get; set; }  = new List<string>();
    public IEnumerable<string> Scopes { get; set; } = new List<string>();
}