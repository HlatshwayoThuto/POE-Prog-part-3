using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CyberSecurityWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CyberBot bot = new CyberBot();
        private bool isNameCaptured = false;

        public MainWindow()
        {
            InitializeComponent();
            AudioPlayer.PlayGreeting(); // Play startup sound
            AppendChat("Bot: Welcome to the Cybersecurity Awareness Bot! What’s your name?");
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            string input = UserInput.Text.Trim();
            if (string.IsNullOrWhiteSpace(input)) return;

            AppendChat("You: " + input);
            UserInput.Clear();

            if (!isNameCaptured)
            {
                bot.SetUserName(input);
                isNameCaptured = true;
                AppendChat($"Bot: Nice to meet you, {input}! Ask me anything about cybersecurity or type 'menu'.");
                return;
            }

            string response = bot.GetResponse(input);
            AppendChat("Bot: " + response);
        }


        private void AppendChat(string message)
        {
            ChatHistory.Items.Add(message);
            ChatHistory.ScrollIntoView(ChatHistory.Items[ChatHistory.Items.Count - 1]);
        }
    }
}