using System;
using System.IO;
using System.Diagnostics;
using System.Linq;
using Xabe.FFmpeg;

namespace IIsRobot.Utils
{
    public class CreateVideo
    {

        /// <summary>
        /// Haven't even tested this, might not even work. 
        /// </summary>
        public void AddAudio()
        {
            string args = "/c ffmpeg -i \"video.mp4\" -i \"./main.wav\" -shortest finish.mp4";
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                CreateNoWindow = true,
                FileName = "cmd.exe",
                WorkingDirectory = @"" + "./finish",
                Arguments = args
            };
            using Process exeProcess = Process.Start(startInfo);
            exeProcess.WaitForExit();
        }
        /// <summary>
        /// Use Xabe.FFmpeg to convert a folder of images to video. I thought this was how it is done, but it 
        /// is not working. 
        /// </summary>
        public async void CreateMp4Async()
        {
            var files = Directory.EnumerateFiles("./tmp").ToList();
            var t = await new Conversion()
                .SetInputFrameRate(1)
                .BuildVideoFromImages(files)
                .SetFrameRate(1)
                .SetPixelFormat(PixelFormat.yuv420p)
                .SetOutput("./outputfile.mp4")
                .Start();
            Console.WriteLine(t.StartTime);
        }
    }
}
