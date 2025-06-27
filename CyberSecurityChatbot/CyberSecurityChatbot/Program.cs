namespace CyberSecurityChatbot
{
    internal class Program //This is the main class for launching the application
    {
        static void Main(string[] args)
        {
            try
            {
                AudioPlayer.PlayGreeting(); // Play audio from resources folder

                CyberBot bot = new CyberBot(); // Instantiate bot
                bot.StartConversation(); // Launch chat
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Oops! Something went wrong while running the chatbot.");
                Console.WriteLine("Error: " + ex.Message);
                Console.ResetColor();
            }
        }
    }
}