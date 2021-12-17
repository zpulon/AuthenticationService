using AuthenticationService.Helper;
using AuthenticationService.Manager;
using AuthenticationService.Model;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AuthenticationService.Stores
{
    public class CustomResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly UserManager _userManager;
        private readonly ISystemClock _isystemClock;
        public CustomResourceOwnerPasswordValidator(UserManager manager, ISystemClock systemClock)
        {
            _isystemClock = systemClock;
            _userManager = manager;
        }
        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var userName = context.UserName;
            var passWord = EncryptHelper.GetInstance().MD5(context.Password);
            //string RegexStr = @"^1(3|4|5|6|7|8|9)\d{9}$";
            OS_User user = null;
            //if (Regex.IsMatch(context.UserName, RegexStr))
            //{ 
            //    user = _userManager.Get_UserByPredicate(t => t.PH == context.UserName && t.Password == clientGroup);
            //}

             user = await _userManager.Get_UserByPredicate(t => t.Account == userName && t.Password == passWord);
            if (user == null)
            {
                context.Result.IsError = true;
                context.Result.Error = TokenRequestErrors.InvalidGrant.ToString();
                context.Result.ErrorDescription = "账号名或者密码错误";
                return;
            }
            context.Result = new GrantValidationResult(user.Id.ToString(), OidcConstants.AuthenticationMethods.Password, _isystemClock.UtcNow.UtcDateTime);

        }
    }
}
