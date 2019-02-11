using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzPassService.ServiceId.AuthorizeUser
{
    /// <summary>
    /// AuthorizeUser Request model sent to EzPass
    /// Note** Naming convention must be lowercase to match api requirements
    /// </summary>
    public class AuthorizeUserRequest
    {
        public string action => "authorizeUser";
        public string vendorId { get; set; }
        public string verificationToken { get; set; }
        public string accessId { get; set; }
    }
}
