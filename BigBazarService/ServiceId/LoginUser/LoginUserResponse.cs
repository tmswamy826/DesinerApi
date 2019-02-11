using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BigBazarService.ServiceId.LoginUser
{
    /// <summary>
    /// LoginUser Response model received from BigBazar
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


