using IIsRobot.Data;
using IIsRobot.Utils;
using Microsoft.Extensions.Configuration;
using RestSharp;
using System;
using System.Linq;
using System.Threading.Tasks;
namespace IIsRobot
{
    class Program
    {
        async static Task Main()
        {
            var videoWorking = false;
            // --- All of this works ---
            if (videoWorking)
            {
                //Get Secrets
                var config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
                var secrets = config.Providers.First();
                //Get tweet to generate text from
                secrets.TryGet("Bearer", out var se);
                var tweets = new GetTweet(se);
                var text = tweets.GetTopTweet();
                //Get text that is generated from tweet
                secrets.TryGet("AIToken", out var aiToken);
                var textGen = new GetText(aiToken, text);
                string script = textGen.RequestText();
                //Synthesize audio from script
                secrets.TryGet("AudioKey", out var audioToken);
                await CreateAudio.Run(script, audioToken);
                //Get the word from the script to search Bing for images
                var getWords = new FindWord(script);
                var word = getWords.GetThreeWords();
                //Get images and send them to the tmp folder
                secrets.TryGet("BingSearchKey", out var BingToken);
                var imgGet = new GetImages(word, BingToken);
                imgGet.Run();
            }
            // --- Broken --- Plans are to create video from images, but this is not working.
            var createVideo = new CreateVideo();
            createVideo.CreateMp4Async();
        }
    }
}
