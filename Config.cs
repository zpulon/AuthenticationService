using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService
{
    public class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources()
        {
            return new List<IdentityResource>() {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
                };
        }
        public static IEnumerable<ApiScope> ApiScopes()
        {
            return new List<ApiScope> { new ApiScope("OnlineSchool_Scope"), new ApiScope("Wisdom_ClassRoom_Scope") };
        }
        public static IEnumerable<ApiResource> ApiResources()
        {
            return new List<ApiResource>() { new ApiResource("OnlineSchool_Api") { Scopes = { "OnlineSchool_Scope" } },
                                             new ApiResource("Wisdom_ClassRoom_Api") { Scopes = { "Wisdom_ClassRoom_Scope" } } };
        }
        public static IEnumerable<Client> Clients()
        {
            return
              new List<Client>
              {
                new Client
                {
                    ClientId = "OnlineSchool",
                    AllowedGrantTypes = new List<string> {"password","admin" },
                    ClientSecrets =
                    {
                        new Secret("OnlineSchool_PWD".Sha256())
                    },
                    AllowedScopes = { "OnlineSchool_Scope" },
                    AccessTokenLifetime=3600*10
                },
                new Client
                {
                    ClientId = "Wisdom_ClassRoom",
                    AllowedGrantTypes = new List<string> {"password","admin" },
                    ClientSecrets =
                    {
                        new Secret("Wisdom_ClassRoom_PWD".Sha256())
                    },
                    AllowedScopes = { "Wisdom_ClassRoom_Scope"},
                    AccessTokenLifetime=3600*10
                },
                 new Client
                {
                    ClientId = "OnlineSchoolClient",
                    AllowedGrantTypes = new List<string> {"client_credentials"},
                    ClientSecrets =
                    {
                        new Secret("OnlineSchoolSecret".Sha256())
                    },
                    AllowedScopes = { "OnlineSchool_Scope" },
                     AccessTokenLifetime=3600*10
                },
                 new Client
                {
                    ClientId = "Wisdom_ClassRoomClient",
                    AllowedGrantTypes =new List<string> {"client_credentials"},
                    ClientSecrets =
                    {
                        new Secret("Wisdom_ClassRoomSecret".Sha256())
                    },
                    AllowedScopes = { "Wisdom_ClassRoom_Scope" },
                     AccessTokenLifetime=3600*10
                }
              };
        }
    }
}
