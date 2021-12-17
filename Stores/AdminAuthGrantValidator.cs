using AuthenticationService.Helper;
using AuthenticationService.Manager;
using AuthenticationService.Model;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Stores
{
    public class AdminAuthGrantValidator : IExtensionGrantValidator
    {
        public AdminManager adminManager;
        private readonly ISystemClock _isystemClock;
        public AdminAuthGrantValidator(AdminManager manager, ISystemClock systemClock)
        {
            adminManager = manager;
            _isystemClock = systemClock;
        }
        public string GrantType => "admin";

        public async Task ValidateAsync(ExtensionGrantValidationContext context)
        {
            var userName = context.Request.Raw["username"];
            var passWord = EncryptHelper.GetInstance().MD5(context.Request.Raw["password"]);

            OS_Admin  admin = null;

            admin = await adminManager.Get_AdminByPredicate(t => t.Account == userName && t.Password == passWord);
            if (admin == null)
            {
                context.Result.IsError = true;
                context.Result.Error = TokenRequestErrors.InvalidGrant.ToString();
                context.Result.ErrorDescription = "账号名或者密码错误";
                return;
            }
            context.Result = new GrantValidationResult(admin.Id.ToString(), GrantType, _isystemClock.UtcNow.UtcDateTime);
   
        }
    }
}
