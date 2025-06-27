// CyberBot.cs (Enhanced with Detailed Comments)
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CyberSecurityChatbot
{
    
    //The CyberBot class is the main logic unit of the console chatbot.
    //It contains all dictionaries, memory structures, and behavior for interacting with the user.
    //The bot provides educational responses to cybersecurity-related topics, responds to sentiment cues,
    //remembers preferences, and maintains a log of recent interactions.
   
    internal class CyberBot
    {
        // Dictionary to store predefined cybersecurity topics and associated list of random responses.
        // Keys are topic keywords like "phishing", and values are lists of responses.
        private Dictionary<string, List<string>> topicResponses = new Dictionary<string, List<string>>();

        // Dictionary used to store user-related memory such as name and favorite topic.
        // Allows the bot to personalize its responses and remember previous interactions.
        private Dictionary<string, string> memory = new Dictionary<string, string>();

        // Dictionary of casual or general questions and mapped predefined responses.
        // These handle common questions like "what can I ask" or "how are you".
        private Dictionary<string, string> generalAnswers = new Dictionary<string, string>();

        // Dictionary mapping keywords that express emotional sentiment to comfort responses.
        // Helps the bot respond empathetically when the user is "worried", "frustrated", or "curious".
        private Dictionary<string, string> sentimentKeywords = new Dictionary<string, string>();

        // Stores the most recent 10 activity logs of user input and bot actions.
        // Each entry is timestamped and used to display a summary upon request.
        private List<string> activityLog = new List<string>();

        // Tracks the last topic the bot responded to so it can continue the conversation contextually.
        private string lastTopic = null;

        
        /// Constructor method for the CyberBot class.
        /// Automatically initializes all topic responses, general responses, and sentiment mappings.
      
        public CyberBot()
        {
            InitializeTopicResponses();      // Set up cybersecurity knowledge base
            InitializeGeneralAnswers();      // Set up casual Q&A support
            InitializeSentimentResponses();  // Set up empathy-based interactions
        }

        
        //Starts the conversation with the user. Handles name capture, dynamic greeting,
        //and begins the main interaction loop which processes typed input.
        
        public void StartConversation()
        {
            ShowWelcomeScreen(); // Display a visually styled greeting panel

            Console.Write("What’s your name? ");
            string name = Console.ReadLine();
            memory["name"] = name; // Remember user's name for personalized responses

            GreetUserByTime(); // Time-aware greeting (Good morning/afternoon/evening)

            // Begin main loop to accept and respond to user input
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("You: ");
                Console.ResetColor();

                string input = Console.ReadLine().ToLower().Trim();
                input = SanitizeInput(input); // Clean up punctuation and spacing for easier matching

                // If no input was entered, prompt for clarification
                if (string.IsNullOrWhiteSpace(input))
                {
                    Respond("I didn’t quite understand that. Could you rephrase?");
                    continue;
                }

                // If user wants to exit the program
                if (input == "exit")
                {
                    Respond("Thanks for chatting! Stay safe online 👋");
                    break;
                }

                // Check if input matches any type of known interaction
                if (HandleSpecialCommands(input)) continue;   // 'menu', 'show activity log', etc.
                if (HandleSentiments(input)) continue;        // Emotional keyword detected
                if (HandleMemoryFeatures(input)) continue;    // Favorite topic or name recall
                if (HandleGeneralQuestions(input)) continue;  // Casual questions like "how are you"
                if (HandleTopicResponses(input)) continue;    // Matches cybersecurity keywords like "scam"

                // If input doesn’t match any known pattern, show a fallback message
                Respond("I’m not sure I understand. Try asking something else.");
                LogActivity("Input not recognized.");
            }
        }

        #region Initialization Methods


        //Populates the topicResponses dictionary with key cybersecurity concepts
        //and multiple in-depth responses that can be selected randomly.
      
        private void InitializeTopicResponses()
        {
            topicResponses["phishing"] = new List<string>
            {
                "Phishing is a cyberattack where scammers impersonate trusted entities...",
                "Be cautious with emails that urge you to click links...",
                "Always verify the sender’s address and avoid suspicious links."
            };

            topicResponses["password"] = new List<string>
            {
                "A strong password includes a mix of character types...",
                "Avoid password reuse across sites...",
                "Use a password manager to store credentials."
            };

            topicResponses["scam"] = new List<string>
            {
                "Online scams trick people through deception...",
                "Scams often appeal to emotion and urgency...",
                "Be skeptical of fake job offers and payment requests."
            };

            topicResponses["privacy"] = new List<string>
            {
                "Online privacy means controlling your data...",
                "Adjust app permissions and avoid oversharing...",
                "Use VPNs and encrypted apps for better privacy."
            };

            topicResponses["browsing"] = new List<string>
            {
                "Safe browsing means using secure HTTPS sites...",
                "Avoid public Wi-Fi for sensitive transactions...",
                "Use extensions that block trackers and clear cache regularly."
            };
        }

       
        //Loads responses for everyday casual questions the user might ask.
        //These make the bot feel friendlier and more engaging.
      
        private void InitializeGeneralAnswers()
        {
            generalAnswers["how are you"] = "I'm doing great and ready to help you stay safe online!";
            generalAnswers["what is your purpose"] = "My purpose is to raise cybersecurity awareness through friendly tips.";
            generalAnswers["what can i ask"] = "You can ask about phishing, scams, passwords, privacy, browsing and more.";
            generalAnswers["who made you"] = "I was created as a university project on cybersecurity education.";
            generalAnswers["how do you help"] = "I help by explaining cybersecurity risks and safe practices.";
        }

  
        //Provides emotional intelligence by letting the bot respond empathetically to
        //emotions like worry, curiosity, and frustration.
  
        private void InitializeSentimentResponses()
        {
            sentimentKeywords["worried"] = "It's okay to feel worried. Let's look at how to stay safe together.";
            sentimentKeywords["frustrated"] = "I understand. Cybersecurity can be tricky but I'm here to help.";
            sentimentKeywords["curious"] = "Curiosity is great! Let's learn together.";
        }

        #endregion

        #region Response Handlers

        // This method checks if the user's input matches any known sentiment keywords.
        // If a match is found, it responds accordingly and logs the activity.
        private bool HandleSentiments(string input)
        {
            foreach (var sentiment in sentimentKeywords)
            {
                if (input.Contains(sentiment.Key))
                {
                    Respond(sentiment.Value); // Respond with the associated message
                    LogActivity($"Detected sentiment: {sentiment.Key}");
                    return true; // Stop processing other handlers
                }
            }
            return false; // No sentiment detected
        }

        // This method manages memory-related features such as remembering and recalling the user's favorite topic.
        private bool HandleMemoryFeatures(string input)
        {
            // Store user's favorite topic
            if (input.Contains("my favourite topic is"))
            {
                string fav = input.Replace("my favourite topic is", "").Trim();
                memory["favorite"] = fav; // Store in memory dictionary
                Respond($"Great! I’ll remember that you're interested in {fav}.");
                LogActivity($"Stored favourite topic: {fav}");
                return true;
            }

            // Retrieve the stored favorite topic
            if (input.Contains("what’s my favourite") || input.Contains("what is my favourite"))
            {
                if (memory.ContainsKey("favorite"))
                    Respond($"You told me your favorite topic is {memory["favorite"]}.");
                else
                    Respond("I don’t think you've told me your favorite topic yet.");
                return true;
            }

            // Suggest something based on the user's favorite topic
            if (input.Contains("suggest something") || input.Contains("recommend something"))
            {
                if (memory.ContainsKey("favorite"))
                    Respond($"Since you're interested in {memory["favorite"]}, check your settings on that today.");
                else
                    Respond("Let me know your favorite topic first by saying: 'My favourite topic is ...'");
                return true;
            }

            return false; // No memory-related input detected
        }

        // This method answers general non-cybersecurity-related questions using predefined answers.
        private bool HandleGeneralQuestions(string input)
        {
            foreach (var question in generalAnswers)
            {
                if (input.Contains(question.Key))
                {
                    Respond(generalAnswers[question.Key]); // Give predefined answer
                    LogActivity($"Answered general question: {question.Key}");
                    return true;
                }
            }
            return false; // No general question matched
        }

        // This method handles responses related to specific cybersecurity topics and follow-up questions.
        private bool HandleTopicResponses(string input)
        {
            foreach (var topic in topicResponses)
            {
                if (input.Contains(topic.Key))
                {
                    lastTopic = topic.Key; // Save last mentioned topic for future reference
                    Respond(GetRandomResponse(topic.Value)); // Provide a random response from list
                    LogActivity($"Responded to topic: {topic.Key}");
                    return true;
                }
            }

            // Handle "tell me more" or "more info" if a topic was recently discussed
            if (input.Contains("tell me more") || input.Contains("more info"))
            {
                if (!string.IsNullOrEmpty(lastTopic) && topicResponses.ContainsKey(lastTopic))
                {
                    Respond(GetRandomResponse(topicResponses[lastTopic]));
                    LogActivity($"Provided more info on: {lastTopic}");
                }
                else
                {
                    Respond("Could you please tell me what topic you'd like to hear more about?");
                }
                return true;
            }

            return false; // No topic-related input matched
        }

        // This method handles special commands such as displaying a menu or showing the activity log.
        private bool HandleSpecialCommands(string input)
        {
            if (input == "menu")
            {
                ShowTopics(); // Display list of topics and questions
                return true;
            }

            if (input.Contains("show activity log") || input.Contains("what have you done"))
            {
                Respond("Here’s what I’ve done so far:", false); // Print log without user's name
                foreach (var entry in activityLog)
                    Console.WriteLine("  • " + entry); // Show each log entry
                Console.WriteLine();
                return true;
            }

            return false; // No special command detected
        }

        #endregion

        #region Helpers and UI

        // Clears the console and shows a welcome screen with app description.
        private void ShowWelcomeScreen()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                                                                ║");
            Console.WriteLine("║        🛡️  WELCOME TO THE CYBERSECURITY AWARENESS BOT  🛡️        ║");
            Console.WriteLine("║                                                                ║");
            Console.WriteLine("║    Ask cybersecurity questions, get safe browsing tips, and    ║");
            Console.WriteLine("║         learn how to protect your digital life better!         ║");
            Console.WriteLine("║                                                                ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════════╝");
            Console.ResetColor();
        }

        // Greets the user based on the current time of day, using the name stored in memory (if available).
        private void GreetUserByTime()
        {
            string name = memory.ContainsKey("name") ? memory["name"] : "friend";
            int hour = DateTime.Now.Hour;
            string timeGreeting = hour < 12 ? "Good morning" : hour < 18 ? "Good afternoon" : "Good evening";
            Respond($"{timeGreeting}, I'm here to help you with cybersecurity. Ask me anything or type 'menu'.");
        }

        // Displays a categorized list of cybersecurity topics and example inputs for user guidance.
        private void ShowTopics()
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("\nYou can ask me about the following cybersecurity topics:");
            foreach (var key in topicResponses.Keys)
                Console.WriteLine($"• {key}");

            Console.WriteLine("\nYou can also ask general/casual questions like:");
            foreach (var key in generalAnswers.Keys)
                Console.WriteLine($"• {key}");

            Console.WriteLine("\nTry saying things like 'I'm worried about scams' or 'My favourite topic is privacy'.");
            Console.ResetColor();
        }

        // Outputs a response to the console. If includeName is true, it appends the user's name (if known).
        private void Respond(string message, bool includeName = true)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            string name = memory.ContainsKey("name") ? memory["name"] : "";
            Console.WriteLine($"\nBot: {message} {(includeName ? name : "")}\n");
            Console.ResetColor();
            LogActivity($"Bot responded: {message}");
        }

        // Removes punctuation and whitespace from input string for easier processing.
        private string SanitizeInput(string input)
        {
            return input.Replace(".", "").Replace("?", "").Replace("!", "").Trim();
        }

        // Selects a random string from a list of responses (for varied and dynamic replies).
        private string GetRandomResponse(List<string> responses)
        {
            return responses[new Random().Next(responses.Count)];
        }

        // Logs each activity with a timestamp. Keeps only the 10 most recent entries.
        private void LogActivity(string message)
        {
            string timestamp = DateTime.Now.ToShortTimeString();
            activityLog.Add($"{timestamp}: {message}");
            if (activityLog.Count > 10)
                activityLog.RemoveAt(0); // Maintain a rolling log
        }

        #endregion
    }
}
