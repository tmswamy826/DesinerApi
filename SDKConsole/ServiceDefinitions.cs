using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace SDKConsole
{
    public class ServiceDefinitions
    {
        public static List<JObject> Definitions =>
            new List<JObject>()
            {
                new JObject()
                {
                    new JProperty("Input", new JObject()
                    {
                        new JProperty("token",null),
                        new JProperty("serviceUrl",null),
                        new JProperty("quantity",null),
                        new JProperty("transponderType",null),
                    }),
                    new JProperty("Output", new JObject()
                    {
                        new JProperty("statusCode", null),
                        new JProperty("message", null)
                    }),
                    new JProperty("ServiceName","TollTag"),
                    new JProperty("MethodName" , "OrderTollTag")
                },
                new JObject()
                {
                    new JProperty("Input",new JObject()
                    {
                        new JProperty("token",null),
                        new JProperty("serviceUrl",null),
                    }),
                    new JProperty("Output", new JObject()
                    {
                        new JProperty("statusCode", null),
                        new JProperty("message", null)
                    }),
                    new JProperty("ServiceName","AccountDetails"),
                    new JProperty("MethodName" , "GetAccountDetails")
                },
                new JObject()
                {
                    new JProperty("Input",new JObject()
                    {
                        new JProperty("token",null),
                        new JProperty("serviceUrl",null),
                    }),
                    new JProperty("Output", new JObject()
                    {
                        new JProperty("statusCode", null),
                        new JProperty("message", null),
                        new JProperty("currentBalance", null),
                    }),
                    new JProperty("ServiceName","AccountDetails"),
                    new JProperty("MethodName" , "GetCurrentBalance")
                },
                new JObject()
                {
                    new JProperty("Input",new JObject()
                    {
                        new JProperty("token",null),
                        new JProperty("serviceUrl",null),
                    }),
                    new JProperty("Output", new JObject()
                    {
                        new JProperty("statusCode", null),
                        new JProperty("message", null),
                        new JProperty("cardNumber", null),
                    }),
                    new JProperty("ServiceName","Payment"),
                    new JProperty("MethodName" , "GetCardOnFile")
                },
                new JObject()
                {
                    new JProperty("Input",new JObject()
                    {
                        new JProperty("token",null),
                        new JProperty("serviceUrl",null),
                        new JProperty("transactionAmount",null)
                    }),
                    new JProperty("Output", new JObject()
                    {
                        new JProperty("statusCode", null),
                        new JProperty("message", null),
                        new JProperty("transactionId", null),
                    }),
                    new JProperty("ServiceName","Payment"),
                    new JProperty("MethodName" , "PayWithCardOnFile")
                },
                new JObject()
                {
                    new JProperty("Input",new JObject()
                    {
                        new JProperty("token",null),
                        new JProperty("serviceUrl",null),
                        new JProperty("plateNumber",null),
                        new JProperty("plateState",null),
                    }),
                    new JProperty("Output", new JObject()
                    {
                        new JProperty("statusCode", null),
                        new JProperty("message", null),
                    }),
                    new JProperty("ServiceName","Vehicle"),
                    new JProperty("MethodName" , "AddVehicle")
                },
            };
    }
}
