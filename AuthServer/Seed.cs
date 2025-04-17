using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using System.IO;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace IdentityServer;

public static class Seed
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var services = scope.ServiceProvider;

        var appManager = services.GetRequiredService<IOpenIddictApplicationManager>();

        var clientDefs = services
            .GetRequiredService<IOptions<List<OpenIddictClientDefinition>>>()
            .Value;

        foreach (var client in clientDefs)
        {
            var existingClient = await appManager.FindByClientIdAsync(client.ClientId);
            if (existingClient is not null) await appManager.DeleteAsync(existingClient);

            var descriptor = new OpenIddictApplicationDescriptor
            {
                ClientId = client.ClientId,
                DisplayName = client.DisplayName,
                ClientType = client.Type.ToString(),
                ConsentType = client.ConsentType.ToString(),
            };
            descriptor.RedirectUris.Add(new Uri($"{client.BaseUrl.TrimEnd('/')}/callback"));
            descriptor.PostLogoutRedirectUris.Add(new Uri($"{client.BaseUrl.TrimEnd('/')}"));

            if (client.RequirePkce)
            {
                descriptor.Requirements.Add(Requirements.Features.ProofKeyForCodeExchange);
            }

            foreach (var permission in client.Permissions)
            {
                descriptor.Permissions.Add(permission switch
                {
                    "authorization" => Permissions.Endpoints.Authorization,
                    "token" => Permissions.Endpoints.Token,
                    "userinfo" => CustomOpenIddictConstants.Permissions.Endpoints.Userinfo,
                    "logout" => CustomOpenIddictConstants.Permissions.Endpoints.Logout,
                    "revocation" => Permissions.Endpoints.Revocation,
                    "introspection" => Permissions.Endpoints.Introspection,
                    _ => throw new InvalidOperationException($"Unknown permission: {permission}")
                });
            }

            foreach (var grant in client.GrantTypes)
            {
                descriptor.Permissions.Add(grant switch
                {
                    "authorization_code" => Permissions.GrantTypes.AuthorizationCode,
                    "refresh_token" => Permissions.GrantTypes.RefreshToken,
                    _ => throw new InvalidOperationException($"Unknown grant_type: {grant}")
                });
            }

            foreach (var response in client.ResponseTypes)
            {
                descriptor.Permissions.Add(response switch
                {
                    "code" => Permissions.ResponseTypes.Code,
                    _ => throw new InvalidOperationException($"Unknown response_type: {response}")
                });
            }

            foreach (var sc in client.Scopes)
            {
                descriptor.Permissions.Add(sc switch
                {
                    "openid" => Permissions.Prefixes.Scope + Scopes.OpenId,
                    "email" => Permissions.Scopes.Email,
                    "profile" => Permissions.Scopes.Profile,
                    "offline_access" => Permissions.Prefixes.Scope + Scopes.OfflineAccess,
                    _ => Permissions.Prefixes.Scope + sc
                });
            }

            await appManager.CreateAsync(descriptor);
        }
    }
}