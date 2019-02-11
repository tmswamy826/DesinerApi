using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public interface IServiceFactory
    {
        void CallService(string className, string methodName, JObject input,ref JObject output);
    }
}
