using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Dto.Response
{
    public class PhoneAuthResponse
    {
        public string Code { get; set; }
        public string RequestId { get; set; }
        public string Message { get; set; }
        public string Recommend { get; set; }
        public MobileResult GetMobileResultDTO { get; set; }
        public class MobileResult
        {
            public string Mobile { get; set; }
        }
        public bool IsSuccess()
        {
            return "OK" == Code;
        }
    }
}
