using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SDKConsole
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            TestAsm();
        }

        public static void TestAsm()
        {
            //Assembly asm = Assembly.LoadFrom(@"C:\Users\rysantos\Documents\Projects\Conduent\Scorpio.SDKs\EzPassService\bin\Debug\netcoreapp2.0\EzPassService.dll");
            //Type t = asm.GetType("EzPassService.EzPassService");
            //IServiceFactory _serviceFactory = (IServiceFactory)asm.CreateInstance(t.FullName, true, BindingFlags.Default, null, null, null, null);
            IServiceFactory serviceFactory = new EzPassService.EzPassService();
            IEnumerable<JObject> serviceDefinitions = ServiceDefinitions.Definitions;
            while (true)
            {
                Console.WriteLine("Found  Methods:");
                foreach (var serviceDef in serviceDefinitions)
                {
                    Console.WriteLine("\t -{0}", serviceDef.GetValue("MethodName"));
                }
                Console.WriteLine("Which Service Method would you like to call?");
                var serviceCall = Console.ReadLine();
                if (string.IsNullOrEmpty(serviceCall))
                    break;
                foreach (var serviceDef in serviceDefinitions)
                {
                    if (!string.Equals(serviceDef.GetValue("MethodName").ToString(), serviceCall, StringComparison.CurrentCultureIgnoreCase)) continue;
                    var input =  JObject.Parse(serviceDef.GetValue("Input").ToString());
                    var output = JObject.Parse(serviceDef.GetValue("Output").ToString());
                    input.Property("token").Value.Replace("");
                    input.Property("serviceUrl").Value.Replace("http://13.68.102.186/vectorwsweb/account/home/viewController.do?json=true");
                    Console.WriteLine("Method takes input: \n{0}\n", input);
                    foreach (var param in input)
                    {
                        if (param.Key == "token" || param.Key == "serviceUrl")
                            continue;
                        {
                            Console.Write("{0}:", param.Key);
                            param.Value.Replace(Console.ReadLine());
                        }
                    }
                    try
                    {
                        Console.WriteLine("Sending: \n{0}", input);
                        serviceFactory.CallService(serviceDef.GetValue("ServiceName").ToString(), serviceDef.GetValue("MethodName").ToString(), input, ref output);
                        Console.WriteLine("Service Call returned back with output: {0}", output.ToString());
                        break;
                    }
                    catch(Exception ex)
                    {
                        var e = ex;
                        Console.WriteLine("{0}", e.Message);
                        while(e.InnerException != null)
                        {
                            e = e.InnerException;
                            Console.WriteLine("--->{0}", e.Message);

                        }
                    }
                }
                Console.Write("\n\n");
            }
            Console.WriteLine("Done");
            Console.ReadKey();

        }
    }
}
