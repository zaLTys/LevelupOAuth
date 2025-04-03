using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace Duende.IDP;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        { 
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
            { };

    public static IEnumerable<Client> Clients =>
       new Client[]
       {
            // Define a new client configuration for IdentityServer
            new Client()
            {
                // The name of the client application
                ClientName = "WebUIClient",

                // Unique identifier for the client
                ClientId = "webuiclient",

                // Specifies the authentication flow to be used
                // 'Code' grant type is used for OAuth 2.0 authorization code flow
                AllowedGrantTypes = GrantTypes.Code,

                // List of allowed redirect URIs after authentication
                RedirectUris =
                {
                    "https://localhost:7188/signin-oidc" // Redirect URI for OpenID Connect authentication
                },
                PostLogoutRedirectUris =
                {
                    "https://localhost:7188/signout-callback-oidc" // Redirect URI after logout
                },

                // Defines the scopes that this client can request
                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId, // Includes the 'sub' (subject) claim
                    IdentityServerConstants.StandardScopes.Profile // Includes profile-related claims
                },

                // Defines the secret(s) associated with the client for authentication
                ClientSecrets =
                {
                    new Secret("secret".Sha256()) // Hashed secret used for client authentication
                }
            }
       };
}