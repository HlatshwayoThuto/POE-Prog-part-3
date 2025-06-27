using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberSecurityWPF
{
    public class QuizGame
    {
        // Quiz data: Question -> (List of Options, Index of Correct Answer)
        private Dictionary<string, (List<string> Options, int CorrectIndex)> quizData;
        private int score = 0;
        private int currentQuestionIndex = 0;
        private List<string> questionList;

        public QuizGame()
        {
            quizData = new Dictionary<string, (List<string>, int)>
            {
                { "What is phishing?", (new List<string> {
                    "A technique to improve password strength",
                    "A scam pretending to be a trusted entity",
                    "A type of VPN",
                    "A software update method"
                }, 1) },

                { "What makes a strong password?", (new List<string> {
                    "Your birthday",
                    "Just letters",
                    "Combination of letters, numbers & symbols",
                    "Repeating your name"
                }, 2) },

                { "What is a VPN used for?", (new List<string> {
                    "Speeding up internet",
                    "Sharing files",
                    "Protecting online privacy",
                    "Blocking ads"
                }, 2) },

                { "Which of these is an online scam?", (new List<string> {
                    "Job offer asking for money upfront",
                    "Government news",
                    "Antivirus updates",
                    "Browser refresh"
                }, 0) },

                { "What should you do before clicking a link?", (new List<string> {
                    "Hover to preview it",
                    "Ignore it",
                    "Click immediately",
                    "Forward it to others"
                }, 0) },

                { "What is 2FA?", (new List<string> {
                    "Two Friends Authentication",
                    "Two-Factor Authentication",
                    "2-Files Analyzer",
                    "None of the above"
                }, 1) },

                { "What is malware?", (new List<string> {
                    "A safe app",
                    "Software that protects you",
                    "Malicious software",
                    "Your browser extension"
                }, 2) },

                { "How often should you update your password?", (new List<string> {
                    "Never",
                    "Every 2-3 months",
                    "Once a year",
                    "Only when hacked"
                }, 1) },

                { "Why avoid public Wi-Fi for banking?", (new List<string> {
                    "It’s slow",
                    "It drains your battery",
                    "It’s insecure and easy to intercept",
                    "It costs money"
                }, 2) },

                { "What’s a common sign of a scam email?", (new List<string> {
                    "Perfect grammar",
                    "Asking for urgent money",
                    "Sent from a friend",
                    "Looks like an ad"
                }, 1) },
            };

            questionList = new List<string>(quizData.Keys);
        }

        // Returns true if more questions remain
        public bool HasNextQuestion()
        {
            return currentQuestionIndex < questionList.Count;
        }

        // Gets current question text and options
        public (string Question, List<string> Options) GetNextQuestion()
        {
            string question = questionList[currentQuestionIndex];
            return (question, quizData[question].Options);
        }

        // Check user answer input
        public string CheckAnswer(int userAnswerIndex)
        {
            string currentQuestion = questionList[currentQuestionIndex];
            int correctIndex = quizData[currentQuestion].CorrectIndex;

            string feedback;
            if (userAnswerIndex == correctIndex)
            {
                score++;
                feedback = "✅ Correct!";
            }
            else
            {
                string correctAnswer = quizData[currentQuestion].Options[correctIndex];
                feedback = $"❌ Incorrect. The right answer was: '{correctAnswer}'";
            }

            currentQuestionIndex++;
            return feedback;
        }

        // Final score feedback
        public string GetFinalFeedback()
        {
            return score >= 5
                ? $"You scored {score}/10. 🎉 You are a PRO!! Keep going!"
                : $"You scored {score}/10. 🧠 Try harder!";
        }

        // Resets quiz
        public void Reset()
        {
            score = 0;
            currentQuestionIndex = 0;
        }
    }
}
