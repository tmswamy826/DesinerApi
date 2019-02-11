using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzPassService.ServiceId.LoginUser
{
    /// <summary>
    /// LoginUserRequest model sent to EzPass
    /// Note** Naming convention must be lowercase to match api requirements
    /// </summary>
    public class LoginUserRequest
    {
        public string action => "loginUser";
        public string username { get; set; }
        public string password { get; set; }
        public string vendorId { get; set; }
        public string model { get; set; } 
        public string systemVersion { get; set; }   
        public string appVersion { get; set; }
        public string verificationToken { get; set; }
    }
}
