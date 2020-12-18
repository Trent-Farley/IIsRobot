using System;
using System.Net;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Linq;

namespace IIsRobot.Data
{
    public class GetImages
    {
        private static string _subscriptionKey;
        private static string _baseUri = "https://api.bing.microsoft.com/v7.0/images/search";
        private static string searchString = "hummingbird";
        private const string QUERY_PARAMETER = "?q=";  // Required
        private const string MKT_PARAMETER = "&mkt=";  // Strongly suggested
        private const string LICENSE_PARAMETER = "&license=";

        public GetImages(string search, string imageKey)
        {
            _subscriptionKey = imageKey;
            searchString = search;
        }

        public void Run()
        {
            RunAsync().Wait();
        }

        static async Task RunAsync()
        {
            try
            {
                // Remember to encode the q query parameter.

                var queryString = QUERY_PARAMETER + Uri.EscapeDataString(searchString);
                queryString += MKT_PARAMETER + "en-us";
                queryString += LICENSE_PARAMETER + "Modify";
                HttpResponseMessage response = await MakeRequestAsync(queryString);

                // This example uses dictionaries instead of objects to access the response data.
                Console.WriteLine("Sending request for images...");
                var contentString = await response.Content.ReadAsStringAsync();
                Dictionary<string, object> searchResponse = JsonConvert.DeserializeObject<Dictionary<string, object>>(contentString);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Got images now downloading...");
                    DownloadImages(searchResponse);
                }
                else
                {
                    PrintErrors(response.Headers, searchResponse);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }
        static async Task<HttpResponseMessage> MakeRequestAsync(string queryString)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _subscriptionKey);
            return (await client.GetAsync(_baseUri + queryString));
        }
        static void DownloadImages(Dictionary<string, object> response)
        {
            var images = response["value"] as JToken;
            Console.WriteLine("Downloading Images...");
            for (var i = 0; i < images.Count(); ++i)
            {
                AddImageToDirectory(images[i]["contentUrl"].ToString(), i);
            }
        }

        private static void AddImageToDirectory(string url, int i)
        {

            string path = @"./tmp";
            if (Directory.Exists(path))
            {
                using WebClient client = new WebClient();
                client.DownloadFile(new Uri(url), @$"./tmp/img{i}" + Path.GetExtension(url));
            }
            else
            {
                Directory.CreateDirectory(path);
                using WebClient client = new WebClient();
                client.DownloadFile(new Uri(url), @$"./tmp/img{i}" + Path.GetExtension(url));
            }

        }

        // Print any errors that occur. Depending on which part of the service is
        // throwing the error, the response may contain different error formats.
        static void PrintErrors(HttpResponseHeaders headers, Dictionary<String, object> response)
        {
            Console.WriteLine("The response contains the following errors:\n");


            if (response.TryGetValue("error", out object value))  // typically 401, 403
            {
                PrintError(response["error"] as Newtonsoft.Json.Linq.JToken);
            }
            else if (response.TryGetValue("errors", out value))
            {
                // Bing API error

                foreach (Newtonsoft.Json.Linq.JToken error in response["errors"] as Newtonsoft.Json.Linq.JToken)
                {
                    PrintError(error);
                }

                // Included only when HTTP status code is 400; not included with 401 or 403.

                if (headers.TryGetValues("BingAPIs-TraceId", out IEnumerable<string> headerValues))
                {
                    Console.WriteLine("\nTrace ID: " + headerValues.FirstOrDefault());
                }
            }

        }

        static void PrintError(Newtonsoft.Json.Linq.JToken error)
        {
            string value = null;

            Console.WriteLine("Code: " + error["code"]);
            Console.WriteLine("Message: " + error["message"]);

            if ((value = (string)error["parameter"]) != null)
            {
                Console.WriteLine("Parameter: " + value);
            }

            if ((value = (string)error["value"]) != null)
            {
                Console.WriteLine("Value: " + value);
            }
        }
    }
}
