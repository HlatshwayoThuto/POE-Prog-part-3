using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media; // Provides the SoundPlayer class to play WAV audio files

namespace CyberSecurityChatbot
{
    internal class AudioPlayer
    {
        // Public static method to play the greeting audio
        public static void PlayGreeting()
        {
            // Build the absolute path to the greeting.wav file in the 'resources' folder
            // AppDomain.CurrentDomain.BaseDirectory points to the bin/Debug/netX.Y folder at runtime
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "resources", "greeting.wav");

            // Check if the audio file actually exists at the specified path
            if (File.Exists(filePath))
            {
                try
                {
                    // Create a SoundPlayer instance and load the WAV file
                    using (SoundPlayer player = new SoundPlayer(filePath))
                    {
                        // Play the audio synchronously, i.e., wait until it finishes
                        player.PlaySync();
                    }
                }
                catch (Exception ex) // If there's an error (e.g., file corrupt or audio device issue)
                {
                    Console.WriteLine("Audio error: " + ex.Message); // Display the error message
                }
            }
            else // File does not exist at the constructed path
            {
                Console.WriteLine("Greeting audio not found at: " + filePath); // Debug message
                Console.WriteLine("⚠️ Make sure the 'greeting.wav' file exists in the 'resources' folder.");
                Console.WriteLine("💡 If using .NET Core, also ensure the System.Windows.Extensions NuGet package is installed.");
            }
        }
    }
}