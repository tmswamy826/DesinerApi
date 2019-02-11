using EzPassService.Services;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Scorpio.SDK.Tests.ApiTests
{
    /// <summary>
    /// Tests the Validation Class
    /// </summary>
    public class ValidationTests
    {
        /// <summary>
        /// Tests the Validate input function
        /// </summary>
        [Fact]
        public void InputValidationTest()
        {
            var input = new JObject()
            {
                new JProperty("A","1"),
                new JProperty("B","2"),
                new JProperty("C", "3")
            };
            var inputClient = new JObject()
            {
                new JProperty("A","1"),
                new JProperty("B","2"),
                new JProperty("C", "3")
            };
            Validation.ValidateInput(inputClient, input);
        }

        /// <summary>
        /// Tests the Validate output function
        /// </summary>
        [Fact]
        public void OutputValidationTest()
        {
            var output = new JObject()
            {
                new JProperty("A","1"),
                new JProperty("B","2"),
                new JProperty("C", "3")
            };
            var outputClient = new JObject()
            {
                new JProperty("A","1"),
                new JProperty("B","2"),
                new JProperty("C", "3")
            };
            Validation.ValidateOutput(outputClient, output);
        }
    }
}
