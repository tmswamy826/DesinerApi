using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BigBazarService.ServiceId.AuthorizeUser
{
    /// <summary>
    /// AuthorizeUser Response model received from BigBazar
    /// </summary>
    public class AuthorizeUserResponse
    {
        public AuthorizeUser AuthorizeUser { get; set; }
        public string StatusCode { get; set; }
        public string Message { get; set; }
    }


}
