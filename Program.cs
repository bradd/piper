using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;
using System.Json;
using System.Diagnostics;

namespace piper
{
    class Program
    {
        static void Main(string[] args)
        {
            fetchOutput("google-video-mp3", "https://www.googleapis.com/customsearch/v1?key=AIzaSyC1donp6S9SSFNgwyiYJ5uMTqjeR4tZt5A&cx=001639569463475826709:dnj_z4ow37q&q=youtube+Calling%20Dr.%20Funk%20(Live%20At%20The%20Black%20Hawk)%20-%20The%20Vince%20Guaraldi%20Quartet", "folder", "output");
            
        }

        static void fetchOutput(string inputType, string inputAddress, string outputType, string outputAddress)
        {
            fetchGoogleVideoMp3Output(inputAddress, outputType, outputAddress);
        }

        static void fetchGoogleVideoMp3Output(string inputAddress, string outputType, string outputAddress)
        {
            string jsonString;
            jsonString = System.IO.File.ReadAllText("output/search.json");
            
            // using (var webClient = new WebClient())
            // {
            //     jsonString = webClient.DownloadString(inputAddress);

            //     using (var writer = File.CreateText("output/search.json"))
            //     {
            //         writer.Write(jsonString);
            //     }
            // }

            Console.WriteLine(jsonString);

            JsonObject jsonDoc = (JsonObject)JsonObject.Parse(jsonString);

            JsonArray jsonArray = (JsonArray)jsonDoc["items"];

            foreach (JsonObject obj in jsonArray)
            {
                JsonValue init;
                obj.TryGetValue("link", out init);
                Console.WriteLine(init.ToString());

                saveYoutubeMp3(init.ToString());

                break;
            }
        }

        static void saveYoutubeMp3(string youtubeUrl)
        {
            string cmd = "youtube-dl --extract-audio --audio-format mp3 https://www.youtube.com/watch?v=lkAqusfHYns --output --output \"%(title)s.%(ext)s\"";
            Console.WriteLine(cmd);
        }
    }

    public static class ShellHelper
    {
        public static string Bash(this string cmd)
        {
            var escapedArgs = cmd.Replace("\"", "\\\"");
            
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{escapedArgs}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            process.Start();
            string result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return result;
        }
    }
}
