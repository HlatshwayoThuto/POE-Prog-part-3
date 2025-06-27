// CyberBot.cs (Enhanced with Detailed Comments)
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CyberSecurityWPF
{
    public class CyberBot
    {
        private Dictionary<string, List<string>> topicResponses = new Dictionary<string, List<string>>();
        private Dictionary<string, string> memory = new Dictionary<string, string>();
        private Dictionary<string, string> generalAnswers = new Dictionary<string, string>();
        private Dictionary<string, string> sentimentKeywords = new Dictionary<string, string>();
        private Dictionary<string, Func<string>> phraseActions = new Dictionary<string, Func<string>>();
        private List<string> activityLog = new List<string>();
        private string lastTopic = null;

        private readonly QuizGame quizGame;
        private readonly TaskManager taskManager;

        public CyberBot()
        {
            InitializeTopicResponses();
            InitializeGeneralAnswers();
            InitializeSentimentResponses();
            InitializePhraseActions();

            quizGame = new QuizGame();
            taskManager = new TaskManager();
        }

        public void SetUserName(string name)
        {
            memory["name"] = name;
        }

        public string GreetUser()
        {
            string name = memory.ContainsKey("name") ? memory["name"] : "friend";
            int hour = DateTime.Now.Hour;
            string timeGreeting = hour < 12 ? "Good morning" : hour < 18 ? "Good afternoon" : "Good evening";
            return $"{timeGreeting}, I'm here to help you with cybersecurity. Ask me anything or type 'menu'.";
        }

        public string GetResponse(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return "I didn’t quite understand that. Could you rephrase?";

            input = SanitizeInput(input.ToLower());

            if (input == "exit")
                return "Thanks for chatting! Stay safe online 👋";

            if (HandleQuizCommands(input, out string quiz)) return quiz;
            if (HandleTaskCommands(input, out string task)) return task;
            if (HandleSpecialCommands(input, out string special)) return special;
            if (HandleSentiments(input, out string sentiment)) return sentiment;
            if (HandleMemoryFeatures(input, out string memoryRes)) return memoryRes;
            if (HandleGeneralQuestions(input, out string general)) return general;
            if (HandleTopicResponses(input, out string topic)) return topic;
            if (HandlePhraseMatching(input, out string phraseMatched)) return phraseMatched;

            LogActivity("Input not recognized.");
            return "I’m not sure I understand. Try asking something else.";
        }

        #region Initialization

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

        private void InitializeGeneralAnswers()
        {
            generalAnswers["how are you"] = "I'm doing great and ready to help you stay safe online!";
            generalAnswers["what is your purpose"] = "My purpose is to raise cybersecurity awareness through friendly tips.";
            generalAnswers["what can i ask"] = "You can ask about phishing, scams, passwords, privacy, browsing and more.";
            generalAnswers["who made you"] = "I was created as a university project on cybersecurity education.";
            generalAnswers["how do you help"] = "I help by explaining cybersecurity risks and safe practices.";
        }

        private void InitializeSentimentResponses()
        {
            sentimentKeywords["worried"] = "It's okay to feel worried. Let's look at how to stay safe together.";
            sentimentKeywords["frustrated"] = "I understand. Cybersecurity can be tricky but I'm here to help.";
            sentimentKeywords["curious"] = "Curiosity is great! Let's learn together.";
        }

        private void InitializePhraseActions()
        {
            phraseActions["show me tips"] = () => GetHelpMenu();
            phraseActions["tell me something useful"] = () => "Always use multi-factor authentication when available.";
            phraseActions["give me advice"] = () => "Keep your software updated to patch security vulnerabilities.";
            phraseActions["what should i do"] = () => "Let’s start with checking your password habits.";
            phraseActions["i feel unsafe online"] = () => "It’s okay—start by reviewing your privacy settings.";
            phraseActions["what’s next"] = () => "Try asking about 'phishing' or 'safe browsing'!";
        }

        #endregion

        #region Handlers

        private bool HandleSentiments(string input, out string response)
        {
            foreach (var sentiment in sentimentKeywords)
            {
                if (input.Contains(sentiment.Key))
                {
                    response = sentiment.Value;
                    LogActivity($"Detected sentiment: {sentiment.Key}");
                    return true;
                }
            }
            response = null;
            return false;
        }

        private bool HandleMemoryFeatures(string input, out string response)
        {
            if (input.Contains("my favourite topic is"))
            {
                string fav = input.Replace("my favourite topic is", "").Trim();
                memory["favorite"] = fav;
                response = $"Great! I’ll remember that you're interested in {fav}.";
                LogActivity($"Stored favourite topic: {fav}");
                return true;
            }
            if (input.Contains("what’s my favourite") || input.Contains("what is my favourite"))
            {
                response = memory.ContainsKey("favorite") ?
                    $"You told me your favorite topic is {memory["favorite"]}." :
                    "I don’t think you've told me your favorite topic yet.";
                return true;
            }
            if (input.Contains("suggest something") || input.Contains("recommend something"))
            {
                response = memory.ContainsKey("favorite") ?
                    $"Since you're interested in {memory["favorite"]}, check your settings on that today." :
                    "Let me know your favorite topic first by saying: 'My favourite topic is ...'";
                return true;
            }
            response = null;
            return false;
        }

        private bool HandleGeneralQuestions(string input, out string response)
        {
            foreach (var question in generalAnswers)
            {
                if (input.Contains(question.Key))
                {
                    response = generalAnswers[question.Key];
                    LogActivity($"Answered general question: {question.Key}");
                    return true;
                }
            }
            response = null;
            return false;
        }

        private bool HandleTopicResponses(string input, out string response)
        {
            foreach (var topic in topicResponses)
            {
                if (input.Contains(topic.Key))
                {
                    lastTopic = topic.Key;
                    response = GetRandomResponse(topic.Value);
                    LogActivity($"Responded to topic: {topic.Key}");
                    return true;
                }
            }
            if (input.Contains("tell me more") || input.Contains("more info"))
            {
                if (!string.IsNullOrEmpty(lastTopic) && topicResponses.ContainsKey(lastTopic))
                {
                    response = GetRandomResponse(topicResponses[lastTopic]);
                    LogActivity($"Provided more info on: {lastTopic}");
                }
                else
                {
                    response = "Could you please tell me what topic you'd like to hear more about?";
                }
                return true;
            }
            response = null;
            return false;
        }

        private bool HandleSpecialCommands(string input, out string response)
        {
            if (input == "menu")
            {
                response = GetHelpMenu();
                return true;
            }

            if (input.Contains("show activity log") || input.Contains("what have you done"))
            {
                response = string.Join("\n• ", activityLog);
                return true;
            }

            response = null;
            return false;
        }

        private bool HandlePhraseMatching(string input, out string response)
        {
            foreach (var phrase in phraseActions)
            {
                if (input.Contains(phrase.Key))
                {
                    response = phrase.Value.Invoke();
                    LogActivity($"Executed phrase command: {phrase.Key}");
                    return true;
                }
            }
            response = null;
            return false;
        }

        private bool HandleQuizCommands(string input, out string response)
        {
            if (input == "quiz")
            {
                quizGame.Reset();
                if (quizGame.HasNextQuestion())
                {
                    var (question, options) = quizGame.GetNextQuestion();
                    response = FormatQuiz(question, options);
                    return true;
                }
                response = "No more questions.";
                return true;
            }

            if (input.StartsWith("answer "))
            {
                string[] parts = input.Split(' ');
                if (parts.Length == 2 && int.TryParse(parts[1], out int choice))
                {
                    response = quizGame.CheckAnswer(choice - 1);
                    if (quizGame.HasNextQuestion())
                    {
                        var (question, options) = quizGame.GetNextQuestion();
                        response += "\n" + FormatQuiz(question, options);
                    }
                    else
                    {
                        response += "\n" + quizGame.GetFinalFeedback();
                    }
                    return true;
                }
                response = "Invalid answer format. Use 'answer 1', 'answer 2', etc.";
                return true;
            }

            response = null;
            return false;
        }

        private bool HandleTaskCommands(string input, out string response)
        {
            if (input.StartsWith("add task "))
            {
                string desc = input.Substring(9).Trim();
                taskManager.AddTask(desc);
                response = $"✅ Task added: {desc}";
                return true;
            }

            if (input.StartsWith("complete task "))
            {
                string desc = input.Substring(14).Trim();
                var task = taskManager.Tasks.FirstOrDefault(t => t.Description.Equals(desc, StringComparison.OrdinalIgnoreCase));
                if (task != null)
                {
                    taskManager.CompleteTask(task);
                    response = $"✅ Marked as completed: {desc}";
                }
                else response = "❌ Task not found.";
                return true;
            }

            if (input.StartsWith("delete task "))
            {
                string desc = input.Substring(12).Trim();
                var task = taskManager.Tasks.FirstOrDefault(t => t.Description.Equals(desc, StringComparison.OrdinalIgnoreCase));
                if (task != null)
                {
                    taskManager.DeleteTask(task);
                    response = $"🗑️ Deleted task: {desc}";
                }
                else response = "❌ Task not found.";
                return true;
            }

            if (input == "show tasks")
            {
                var tasks = taskManager.ShowTasks();
                response = tasks.Count > 0 ? string.Join("\n• ", tasks) : "📭 No tasks found.";
                return true;
            }

            response = null;
            return false;
        }

        #endregion

        #region Utility

        private string GetHelpMenu()
        {
            var result = "📚 Here are some things you can ask me about:\n";

            result += "\n🔐 Cybersecurity Topics:\n";
            foreach (var topic in topicResponses.Keys)
                result += $"• {topic}\n";

            result += "\n💬 Casual Questions:\n";
            foreach (var casual in generalAnswers.Keys)
                result += $"• {casual}\n";

            result += "\n🛠️ Task Commands:\n";
            result += "• add task [description]\n";
            result += "• complete task [description]\n";
            result += "• delete task [description]\n";
            result += "• show tasks\n";

            result += "\n🎮 Fun & Interactive:\n";
            result += "• quiz — Start the cybersecurity quiz\n";
            result += "• answer [number] — Answer a quiz question\n";

            result += "\n💡 Example Phrases:\n";
            result += "• 'I'm worried about scams'\n";
            result += "• 'My favourite topic is privacy'\n";
            result += "• 'Remind me to do homework at 18:00'\n";

            return result;
        }

        private string GetRandomResponse(List<string> responses)
        {
            return responses[new Random().Next(responses.Count)];
        }

        private string SanitizeInput(string input)
        {
            return input.Replace(".", "").Replace("?", "").Replace("!", "").Trim();
        }

        private void LogActivity(string message)
        {
            string timestamp = DateTime.Now.ToShortTimeString();
            activityLog.Add($"{timestamp}: {message}");
            if (activityLog.Count > 10)
                activityLog.RemoveAt(0);
        }

        private string FormatQuiz(string question, List<string> options)
        {
            string result = $"{question}\n";
            for (int i = 0; i < options.Count; i++)
            {
                result += $"  {i + 1}. {options[i]}\n";
            }
            result += "\nType 'answer [number]' to respond.";
            return result;
        }

        #endregion
    }
}
