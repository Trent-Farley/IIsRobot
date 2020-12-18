using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DeepAI; // Add this line to the top of your file
using Newtonsoft.Json.Linq;

namespace IIsRobot.Data
{
    public class GetText
    {
        private readonly string _token;
        private string _text;
        public GetText(string token, string text)
        {
            _token = token;
            _text = text;
        }
        public string RequestText()
        {
            Console.WriteLine("Requesting text from ai services...");
            DeepAI_API api = new DeepAI_API(apiKey: _token);

            StandardApiResponse resp = api.callStandardApi("text-generator", new
            {
                text = _text,
            });
            var output = JObject.Parse(api.objectAsJsonString(resp))["output"].ToString();
            output = Regex.Replace(output, @"http[^\s]+", "");
            return output;
        }
    }
}
