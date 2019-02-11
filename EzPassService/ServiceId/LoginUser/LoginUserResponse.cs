using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzPassService.ServiceId.LoginUser
{
    /// <summary>
    /// LoginUser Response model received from EzPass
    /// </summary>
    public class LoginUserResponse
    {
        public LoginUser LoginUser { get; set; }
        public string StatusCode { get; set; }
        public string Message { get; set;   }
        public string AccountNumber { get; set; }
        public string Permission { get; set; }
    }
}


