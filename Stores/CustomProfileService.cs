using AuthenticationService.Helper;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using System;
using System.Threading.Tasks;

namespace AuthenticationService.Stores
{
    public class CustomProfileService : IProfileService
    {
        private readonly ProfileHelper _profileHelper;
        public CustomProfileService(ProfileHelper profileHelper)
        {
            _profileHelper = profileHelper;
        }
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            try
            {
                //获得登录用户的ID
                var sub = context?.Subject?.GetSubjectId();
                context.IssuedClaims = await _profileHelper.GetCustomProfile(Convert.ToInt64(sub), context.ValidatedRequest.Raw["grant_type"]);

            }
            catch (Exception ex)
            {
               
                throw;
            }
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            context.IsActive = true;
            return Task.CompletedTask;
        }
    }
}
