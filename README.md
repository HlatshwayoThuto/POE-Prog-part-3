# POE-Prog-part-3



YoutubeVideo Link: https://youtu.be/yRoE22b7K04

GitHub repo Link : https://github.com/HlatshwayoThuto/POE-Prog-part-3.git



🛡️ Cybersecurity WPF Bot

Cybersecurity is a user-friendly WPF desktop chatbot that promotes cybersecurity awareness through interactive conversations, a quiz game, and task/reminder features.



📦 Features

🤖 Cybersecurity Chatbot

Ask questions about phishing, scams, passwords, privacy, browsing, etc.



Understands casual phrases like “I feel unsafe online” or “Give me advice.”



Detects sentiment and responds accordingly (e.g., worried, frustrated).



Supports memory for favorite topics and suggestions.



🎯 Cybersecurity Quiz Game

Type quiz to start the game.



Answer multiple-choice questions using answer \[number].



Get real-time feedback and a final score summary.



📋 Task Manager

Manage your daily cybersecurity tasks with simple commands:



add task check antivirus



complete task check antivirus



delete task check antivirus



show tasks



Optional reminder system (timers coming soon).



📝 Activity Log

Type show activity log to see what you’ve done in the session.



🧠 Example Commands

plaintext

Copy

Edit

> menu

> quiz

> answer 2

> add task update passwords

> complete task update passwords

> delete task update passwords

> show tasks

> my favourite topic is phishing

> suggest something

🛠️ Technologies Used

.NET WPF (C#)



MVVM-style logic separation (Models, UI, Logic)



System.Media for greeting audio



ObservableCollection for real-time task updates



Dictionary-based NLP-style pattern matching



🚀 Setup Instructions

Clone the repository or download the ZIP.



Ensure you have Visual Studio with .NET Desktop Development workload installed.



Run the project.



Ensure greeting.wav exists in the /resources folder (or update the path in AudioPlayer.cs).



🧪 Testing Tips

Try entering emotional cues: I'm frustrated, I'm curious.



Explore topic learning: Tell me about phishing.



Try the quiz: quiz → answer 2 → continue.



Add and manage tasks: add task change passwords.



📁 Folder Structure

Copy

Edit

CyberSecurityWPF/

│

├── Models/

│   └── TaskItems.cs

│

├── Resources/

│   └── greeting.wav

│

├── CyberBot.cs

├── QuizGame.cs

├── TaskManager.cs

├── MainWindow.xaml / MainWindow.xaml.cs

└── AudioPlayer.cs

📌 Future Enhancements

Timer-based reminders for scheduled tasks ⏰



Exportable quiz and task summaries



Multi-language support



👨‍🎓 Creator

This project was created for an academic portfolio to promote cybersecurity awareness and practical programming skills by Thuto Hlatshawayo

