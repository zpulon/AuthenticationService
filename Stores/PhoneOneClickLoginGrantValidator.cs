using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Profile;
using AuthenticationService.Dto.Common;
using AuthenticationService.Dto.Request;
using AuthenticationService.Dto.Response;
using AuthenticationService.Manager;
using AuthenticationService.Model;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Stores
{
    /// <summary>
    /// 手机一键登录
    /// </summary>
    public class PhoneOneClickLoginGrantValidator : IExtensionGrantValidator
    {
        private readonly UserManager _userManager;
        private readonly ISystemClock _isystemClock;
        private readonly IConfiguration _config;
        public PhoneOneClickLoginGrantValidator(IConfiguration configuration, UserManager manager, ISystemClock systemClock)
        {
            _config = configuration;
            _isystemClock = systemClock;
            _userManager = manager;
        }
        public string GrantType => "phoneclicklogin";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task ValidateAsync(ExtensionGrantValidationContext context)
        {
            var token = "";
            var guid = "";
            var errorvalidationResult = new GrantValidationResult(TokenRequestErrors.InvalidGrant);
            errorvalidationResult.CustomResponse = new Dictionary<string, object>();
            try
            {
                // 阿里云号码认证API的AccessToken
                token = context.Request.Raw["token"];
                // 流水号、请求标识
                guid = Guid.NewGuid().ToString();
                var clientId = context.Request.Client.ClientId;
                if (string.IsNullOrWhiteSpace(token))
                {
                    context.Result = errorvalidationResult;
                    errorvalidationResult.CustomResponse.Add("error_description", "acessToken不能为空");
                    return;
                }
                //验证验证码，模板是登陆CapchaType = CapchaType.Login
                var codeRequest = new PhoneAuthRequest { AccessToken = token, Guid = guid };
                var res = VerifyPhoneAuth(codeRequest);
                if (!res.IsSuccess())
                {
                    errorvalidationResult.CustomResponse.Add("error_description", res.Message);
                    return;
                }
                var phone = res.Extension.GetMobileResultDTO.Mobile;
                OS_User user = null;
                user = await _userManager.Get_UserByPredicate(t => t.Account == phone);
                if (user == null)
                {
                    context.Result.IsError = true;
                    context.Result.Error = TokenRequestErrors.InvalidGrant.ToString();
                    context.Result.ErrorDescription = "账号名或者密码错误";
                    return;
                }
                context.Result = new GrantValidationResult(user.Id.ToString(), OidcConstants.AuthenticationMethods.Password, _isystemClock.UtcNow.UtcDateTime);
            }
            catch (Exception)
            {

                throw;
            }

        }


        private ResponseMessage<PhoneAuthResponse> VerifyPhoneAuth(PhoneAuthRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.AccessToken))
            {
                return new ResponseMessage<PhoneAuthResponse>
                {
                    Code = ResponseCodeDefines.ArgumentNullError,
                    Message = "请求参数不合法"
                };
            }
            string regionId = _config["PhoneAuth_AliyunRegionId"];
            string accessKeyId = _config["PhoneAuth_AliyunAccessKeyId"];
            string secret = _config["PhoneAuth_AliyunSecret"];

            if (string.IsNullOrWhiteSpace(regionId)) throw new Exception("配置读取失败");

            IClientProfile profile = DefaultProfile.GetProfile(regionId, accessKeyId, secret);
            DefaultAcsClient client = new DefaultAcsClient(profile);
            CommonRequest comRequest = new CommonRequest
            {
                Method = Aliyun.Acs.Core.Http.MethodType.POST,
                Domain = "dypnsapi.aliyuncs.com",
                Version = "2017-05-25",
                Action = "GetMobile",
                Protocol = Aliyun.Acs.Core.Http.ProtocolType.HTTPS
            };
            comRequest.AddQueryParameters("OutId", request.Guid);
            comRequest.AddQueryParameters("AccessToken", request.AccessToken);
            try
            {
                CommonResponse res = client.GetCommonResponse(comRequest);
                var content = Newtonsoft.Json.JsonConvert.DeserializeObject<PhoneAuthResponse>(System.Text.Encoding.Default.GetString(res.HttpResponse.Content));
                if (content.IsSuccess())
                {
                    return new ResponseMessage<PhoneAuthResponse>
                    {
                        Extension = content
                    };
                }
                else
                {
                    return new ResponseMessage<PhoneAuthResponse>
                    {
                        Code = ResponseCodeDefines.ServiceError,
                        Message = content.Message,
                        Extension = content
                    };
                }
                // 阿里云接口响应体的几种情况
                // - 没传AccessKeyId和AccessKeySecret
                // {"Recommend":"https://error-center.aliyun.com/status/search?Keyword=InvalidAccessKeyId.NotFound&source=PopGw","Message":"Specified access key is not found.","RequestId":"6DE84857-756A-46A6-856A-2081DD814412","HostId":"dypnsapi.aliyuncs.com","Code":"InvalidAccessKeyId.NotFound"}
                // - AccessToken 有误
                // {"Message":"accessCode参数不合法","RequestId":"7A37A913-5E01-4EC0-9EFF-F57B2CA6658F","Code":"isv.ACCESS_CODE_ILLEGAL"}
                // - 获取成功
                // {"GetMobileResultDTO":{"Mobile":"11111111111"},"Message":"OK","RequestId":"65B90460-E6E9-47C4-BAE1-FD963D6AFD10","Code":"OK"}
                // - 未知异常 (第二次使用同一AccessToken获取电话号码的时候-应该是Token失效了)
                //"code":"isp.UNKNOWN","requestId":"891FB26C-1266-4E61-961F-280382F69D08","message":"未知异常"}
            }
            catch (Aliyun.Acs.Core.Exceptions.ServerException e)
            {
                return new ResponseMessage<PhoneAuthResponse>
                {
                    Code = ResponseCodeDefines.ServiceError,
                    Message = e.Message
                };
            }
            catch (Aliyun.Acs.Core.Exceptions.ClientException e)
            {
                return new ResponseMessage<PhoneAuthResponse>
                {
                    Code = ResponseCodeDefines.ServiceError,
                    Message = e.Message
                };
            }
        }
    }


    
    
}
