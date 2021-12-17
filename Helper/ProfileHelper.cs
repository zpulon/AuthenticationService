using AuthenticationService.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AuthenticationService.Helper
{
    public class ProfileHelper
    {
        private readonly UserManager  userManager;
        private readonly AdminManager  adminManager;
        public ProfileHelper(UserManager  user, AdminManager  admin)
        {
            userManager = user;
            adminManager = admin;
        }
        public async Task<List<Claim>> GetCustomProfile(long userId, string grant_type)
        {
            var claims = new List<Claim>();
            if (string.Compare(grant_type, "admin", StringComparison.OrdinalIgnoreCase) != 0)
            {
                var user = await userManager.Get_UserById(userId);
                if (user == null)
                {
                    return claims;
                }
                claims.Add(new Claim("UserId", user?.Id.ToString()));
                claims.Add(new Claim("UserName", user.Name ?? ""));
                claims.Add(new Claim("SchoolName", user.SchoolName ?? ""));
                claims.Add(new Claim("GraduationYear", user.GraduationYear ?? ""));
                claims.Add(new Claim("OnlineSchoolNumber", user.OnlineSchoolNumber ?? ""));
                claims.Add(new Claim("ClassName", user.ClassName ?? ""));
                claims.Add(new Claim("grant_type", grant_type));
                return claims;
            }
            var admin = await adminManager.Get_AdminById(userId);
            if (admin == null)
            {
                return claims;
            }
            claims.Add(new Claim("UserId", admin?.Id.ToString()));
            claims.Add(new Claim("UserName", admin.Name ?? ""));
            claims.Add(new Claim("grant_type", grant_type));
            return claims;
        }
           
     }
}
