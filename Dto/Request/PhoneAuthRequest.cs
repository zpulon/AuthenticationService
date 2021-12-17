using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Dto.Request
{
    public class PhoneAuthRequest
    {
        /// <summary>
        /// 访问Token
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// GUID
        /// 用作外部流水号 本机号码验证的时候阿里云才会使用
        /// </summary>
        public string Guid { get; set; }
    }
}
