// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;

namespace CarRental.IdentityServer
{
    public static class Config
    {
        public static IEnumerable<ApiResource> ApiResources => new ApiResource[]
        {
            new ApiResource("catalog_source"){Scopes={"catalog"}},
               new ApiResource("photo_source"){Scopes={"photos"}},
                  new ApiResource("basket_source"){Scopes={"basket"}},
                      new ApiResource("discount_source"){Scopes={"discount"}},
                        new ApiResource("order_source"){Scopes={"order"}},
                             new ApiResource("payment_source"){Scopes={"payment"}},
                                new ApiResource("gatweway_source"){Scopes={"gateway"}},
                                 new ApiResource(IdentityServerConstants.LocalApi.ScopeName)
        };

        public static IEnumerable<IdentityResource> IdentityResources =>
                   new IdentityResource[]
                   {
                       new IdentityResources.Email(),
                       new IdentityResources.OpenId(),
                       new IdentityResources.Profile(),
                       new IdentityResource(){ Name="roles", DisplayName="Roles", Description="Roller", UserClaims=new []{ "role"} }
                   };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("catalog","Catalog API için full erişim"),

                new ApiScope("photos","Photo Stock API için full erişim"),

                     new ApiScope("basket","Basket API için full erişim"),
                            new ApiScope("discount","Discount API için full erişim"),
                             new ApiScope("order","Order API için full erişim"),
                                 new ApiScope("payment","Payment API için full erişim"),
                                   new ApiScope("gateway","Gateway API için full erişim"),
                new ApiScope(IdentityServerConstants.LocalApi.ScopeName)
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                   ClientName="MVC",
                    ClientId="NormalUser",
                    ClientSecrets= {new Secret("secret".Sha256())},
                    AllowedGrantTypes= GrantTypes.ClientCredentials,
                    AllowedScopes={ "catalog", "photos", "gateway", IdentityServerConstants.LocalApi.ScopeName }
                },
                   new Client
                {
                   ClientName="MVC",
                    ClientId="ClientForUser",
                    AllowOfflineAccess=true,
                    ClientSecrets= {new Secret("secret".Sha256())},
                    AllowedGrantTypes= GrantTypes.ResourceOwnerPassword,
                    AllowedScopes={ "basket", "payment", "photos", "catalog", "order", "discount", "gateway", IdentityServerConstants.StandardScopes.Email, IdentityServerConstants.StandardScopes.OpenId,IdentityServerConstants.StandardScopes.Profile, IdentityServerConstants.StandardScopes.OfflineAccess, IdentityServerConstants.LocalApi.ScopeName,"roles" },
                    AccessTokenLifetime=1*60*60,
                    RefreshTokenExpiration=TokenExpiration.Absolute,
                    AbsoluteRefreshTokenLifetime= (int) (DateTime.Now.AddDays(60)- DateTime.Now).TotalSeconds,
                    RefreshTokenUsage= TokenUsage.ReUse
                },

            };
    }
}