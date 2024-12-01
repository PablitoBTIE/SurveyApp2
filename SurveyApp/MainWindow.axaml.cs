using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SurveyCreator
{
    public partial class MainWindow : Window
    {
        private List<SurveyQuestion> questions = new();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void AddQuestionButton_Click(object sender, RoutedEventArgs e)
        {
            string questionText = QuestionTextBox.Text;
            bool isSingleChoice = SingleChoiceRadioButton.IsChecked == true;
            List<string> options = OptionsTextBox.Text.Split(',').Select(o => o.Trim()).ToList();

            if (!string.IsNullOrEmpty(questionText) && options.Count > 0)
            {
                questions.Add(new SurveyQuestion(questionText, isSingleChoice, options));
                RefreshQuestionsList();
                ClearInputFields();
            }
        }

        private void RefreshQuestionsList()
        {
            QuestionsListBox.Items = questions
                .Select(q => $"{q.QuestionText} ({(q.IsSingleChoice ? "Jednokrotnego wyboru" : "Wielokrotnego wyboru")})")
                .ToList();
        }

        private void ClearInputFields()
        {
            QuestionTextBox.Text = string.Empty;
            OptionsTextBox.Text = string.Empty;
            SingleChoiceRadioButton.IsChecked = true;
        }

        private async void SaveSurveyButton_Click(object sender, RoutedEventArgs e)
        {
            using (StreamWriter writer = new StreamWriter("survey.txt"))
            {
                foreach (var q in questions)
                {
                    await writer.WriteLineAsync(q.QuestionText);
                    await writer.WriteLineAsync(q.IsSingleChoice ? "Jednokrotnego wyboru" : "Wielokrotnego wyboru");
                    await writer.WriteLineAsync(string.Join(", ", q.Options));
                    await writer.WriteLineAsync();
                }
            }
            var messageBox = new Window
            {
                Content = new TextBlock { Text = "Ankieta została zapisana.", Margin = new Avalonia.Thickness(10) },
                Width = 300,
                Height = 100
            };
            messageBox.ShowDialog(this);
        }
    }

    public class SurveyQuestion
    {
        public string QuestionText { get; }
        public bool IsSingleChoice { get; }
        public List<string> Options { get; }

        public SurveyQuestion(string questionText, bool isSingleChoice, List<string> options)
        {
            QuestionText = questionText;
            IsSingleChoice = isSingleChoice;
            Options = options;
        } 
    }
}
