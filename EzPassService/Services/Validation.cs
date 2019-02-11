using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace EzPassService.Services
{
    /// <summary>
    /// Validation Functions
    /// Used to compare the input/output passed by the client and the input/output expected
    /// Throws an argument exception if they do not match
    /// </summary>
    public static class Validation
    {
        /// <summary>
        /// Validates that the client passed in the correct input
        /// </summary>
        /// <param name="clientInput"></param>
        /// <param name="functionInput"></param>
        public static void ValidateInput(JObject clientInput, JObject functionInput)
        {
            foreach(var item in functionInput)
            {
                if (!clientInput.ContainsKey(item.Key))
                {
                    throw new ArgumentException(string.Format("Cannot find field '{0}' in input", item.Key), item.Key);
                }
            }
        }

        /// <summary>
        /// Function that validates the client is expecting the correct output
        /// </summary>
        /// <param name="clientOutput"></param>
        /// <param name="functionOutput"></param>
        public static void ValidateOutput(JObject clientOutput, JObject functionOutput)
        {
            foreach (var item in functionOutput)
            {
                if (!clientOutput.ContainsKey(item.Key))
                    throw new ArgumentException(string.Format("Cannot find field '{0}' in output", item.Key), item.Key);
            }
        }
    }
}
