using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;
using static IdentityServer4.IdentityServerConstants;

namespace IDS
{
    public class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResourceResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(), //必须要添加，否则报无效的scope错误
                new IdentityResources.Profile()
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("api1","My API"),
                new ApiResource("API2","WebApi")
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "client1",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes =
                    {
                        "api1",
                        StandardScopes.OpenId, //必须要添加，否则报forbidden错误
                        StandardScopes.Profile
                    },
                },

                new Client
                {
                    ClientId = "client2",
                    AccessTokenLifetime = 3600*24*5,
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowOfflineAccess = true,
                    AbsoluteRefreshTokenLifetime = 3600*24*15,
                    RefreshTokenExpiration = TokenExpiration.Sliding,
                    SlidingRefreshTokenLifetime = 3600,

                    ClientSecrets =
                    {
                        new Secret("secret2".Sha256())
                    },
                    AllowedScopes =
                    {
                        "API2",
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        IdentityServerConstants.StandardScopes.OpenId, //必须要添加，否则报forbidden错误
                        IdentityServerConstants.StandardScopes.Profile }
                     },
                new Client
                {
                    ClientId = "xamarin",
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    AccessTokenLifetime = 1800,//设置AccessToken过期时间
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    
                    //RefreshTokenExpiration = TokenExpiration.Absolute,//刷新令牌将在固定时间点到期
                    AbsoluteRefreshTokenLifetime = 2592000,//RefreshToken的最长生命周期,默认30天
                    RefreshTokenExpiration = TokenExpiration.Sliding,//刷新令牌时，将刷新RefreshToken的生命周期。RefreshToken的总生命周期不会超过AbsoluteRefreshTokenLifetime。
                    SlidingRefreshTokenLifetime = 3600,//以秒为单位滑动刷新令牌的生命周期。
                    //按照现有的设置，如果3600内没有使用RefreshToken，那么RefreshToken将失效。即便是在3600内一直有使用RefreshToken，RefreshToken的总生命周期不会超过30天。所有的时间都可以按实际需求调整。

                    AllowOfflineAccess = true,//如果要获取refresh_tokens ,必须把AllowOfflineAccess设置为true
                    AllowedScopes = new List<string>
                    {
                        "api1",
                        StandardScopes.OfflineAccess, //如果要获取refresh_tokens ,必须在scopes中加上OfflineAccess
                        StandardScopes.OpenId,//如果要获取id_token,必须在scopes中加上OpenId和Profile，id_token需要通过refresh_tokens获取AccessToken的时候才能拿到（还未找到原因）
                        StandardScopes.Profile//如果要获取id_token,必须在scopes中加上OpenId和Profile
                    }
                }
            };
        }
    }
}
