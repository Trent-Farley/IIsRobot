
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IIsRobot.Data
{
    public class GetTweet
    {
        private readonly string _endpoint = "https://api.twitter.com/1.1/search/tweets.json?q=life&result_type=popular&count=1";
        private readonly string _bearer;
        public GetTweet(string bearer)
        {
            _bearer = bearer;
        }

        public string GetTopTweet()
        {
            Console.WriteLine("Getting top tweet...");
            string json = SendRequest(_endpoint, _bearer);
            var parser = JObject.Parse(json);
            Console.WriteLine("Top tweet:" + parser["statuses"][0]["text"].ToString());
            return parser["statuses"][0]["text"].ToString();
        }
        private string SendRequest(string endpoint, string bearer)
        {
            Console.WriteLine("Sending request for tweet...");
            var client = new RestClient(endpoint)
            {
                Timeout = -1
            };
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", $"Bearer {bearer}");
            IRestResponse response = client.Execute(request);
            return response.Content;
        }
    }
}
