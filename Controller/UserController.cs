using AspNet.Security.OAuth.Validation;
using AuthenticationService.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Controller
{
    [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
    [Route("api/user")]
    [ApiController]
    public class UserController: BaseController
    {
        //Controller=》Manager=》Stores
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("info")]
        [AuthorizationLocal]
        public  ActionResult<UserInfo> GetUserInfo()
        {
            try
            {
                return User;
            }
            catch (Exception e)
            {
                throw;
            }
        }

    }
}
