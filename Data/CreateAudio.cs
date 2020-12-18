using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
namespace IIsRobot.Data
{
    public class CreateAudio
    {


        public static async Task Run(string text, string _key)
        {
            await SynthesizeAudioAsync(text, _key);
        }

        static async Task SynthesizeAudioAsync(string text, string _key)
        {
            Console.WriteLine("Synthesizing audio...");
            var config = SpeechConfig.FromSubscription(_key, "westus");
            using var audioConfig = AudioConfig.FromWavFileOutput("./main.wav");
            using var synthesizer = new SpeechSynthesizer(config, audioConfig);
            await synthesizer.SpeakTextAsync(text);
        }

    }
}
