using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;
using Origam.ServerCore.Configuration;

namespace Origam.ServerCore
{
    static class Settings
    {
        internal static ApiResource[] GetIdentityApiResources()
        {
            return new[]
            {
                new ApiResource(IdentityServerConstants.LocalApi.ScopeName)
            };
        }

        internal static IdentityResource[] GetIdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        }

        internal static Client[] GetIdentityClients(
            IdentityServerConfig identityServerConfig)
        {
            return new[]
            {
                new Client
                {
                    ClientId = "origamMobileClient",
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    ClientSecrets =
                    {
                        new Secret(identityServerConfig.MobileClient.ClientSecret.Sha256())
                    },
                    RedirectUris = identityServerConfig.MobileClient.RedirectUris,
                    RequireConsent = false,
                    RequirePkce = true,
                    PostLogoutRedirectUris = identityServerConfig.MobileClient.PostLogoutRedirectUris,
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.LocalApi.ScopeName,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                    },
                    AllowOfflineAccess = true,
                    AllowAccessTokensViaBrowser = true,
                    AccessTokenType = AccessTokenType.Reference
                },
                new Client
                {
                    ClientId = "origamWebClient",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequireClientSecret = false,
                    RedirectUris =  identityServerConfig.WebClient.RedirectUris,
                    RequireConsent = false,
                    RequirePkce = true,
                    PostLogoutRedirectUris = identityServerConfig.WebClient.PostLogoutRedirectUris,
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.LocalApi.ScopeName,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                    },
                    AllowOfflineAccess = true,
                    AllowAccessTokensViaBrowser = true,
                    AccessTokenType = AccessTokenType.Reference
                },
            };
        }
    }
}