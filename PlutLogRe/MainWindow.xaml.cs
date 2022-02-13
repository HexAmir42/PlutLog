using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using Microsoft.Win32;
using PlutLogRe.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace PlutLogRe
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Model Model { get; set; }
        public Runner Runner { get; set; }
        private OpenFileDialog fileDialog;
        private SaveFileDialog saveFileDialog;

        private double width;
        private double height;
        private double left;
        private double top;
        private bool isMaximized;

        public MainWindow()
        {
            InitializeComponent();

            fileDialog = new OpenFileDialog
            {
                DefaultExt = ".plt",
                Filter = "Text documents (.plt)|*.plt"
            };
            saveFileDialog = new SaveFileDialog
            {
                FileName = "Document",
                DefaultExt = ".plt",
                Filter = "Text documents (.plt)|*.plt"
            };

            var myAssembly = Assembly.GetExecutingAssembly();
            using (var stream = new StreamReader(myAssembly.GetManifestResourceStream(@"PlutLogRe.Resources.PlutLogSyntax.xshd")))
            {
                using (var reader = new XmlTextReader(stream))
                {
                    CodeBlock.SyntaxHighlighting = HighlightingLoader.Load(reader, HighlightingManager.Instance);
                }
            }

            Model = new Model();
            isMaximized = false; 
            CodeBlock.TextArea.TextEntering += textEditor_TextArea_TextEntering;
            CodeBlock.TextArea.TextEntered += textEditor_TextArea_TextEntered;

        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(!isMaximized)
            {
                this.DragMove();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(Runner != null)
            {
                Runner.Finish();
            }
            this.Close();
        }

        private void Minimizer_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void SaveButtonClick(object sender, RoutedEventArgs e)
        {
            Save();
        }

        private void Save()
        {
            var content = CodeBlock.Text;
            if (Model.Document is null)
            {

                bool? result = saveFileDialog.ShowDialog();
                if (result == true)
                {
                    string filename = saveFileDialog.FileName;
                    Model.SaveDocument(filename, content);
                    ConsoleRTB.AddText("[File Saved]", Brushes.Khaki);
                }
            }
            else
            {
                Model.Document.Update(content);
                Model.Document.Save();
            }
        }

        private void FileOpenButton_Click(object sender, RoutedEventArgs e)
        {
            bool? result = fileDialog.ShowDialog();
            if (result == true)
            {
                Model.OpenDocument(fileDialog.FileName);
                CodeBlock.Text = Model.Document.Content;
                if(Runner != null && Runner.Running)
                {
                    Runner.Finish();
                }
                Runner = new Runner(fileDialog.FileName, ConsoleRTB_ReceiveOutput, ConsoleRTB_ReceiveError);
            }
        }

        private void ConsoleRTB_ReceiveOutput(string input)
        {
            ConsoleRTB.AddText(input, Brushes.White);
        }

        private void ConsoleRTB_ReceiveError(string input)
        {
            ConsoleRTB.AddText(input, Brushes.Red);
        }

        private void Maximizer_Click(object sender, RoutedEventArgs e)
        {
            if (isMaximized)
            {
                WindowState = WindowState.Normal;
                Width = width;
                Height = height;
                Left = left;
                Top = top;
                isMaximized = false;
            }
            else
            {
                width = Width;
                height = Height;
                left = Left;
                top = Top;
                Left = 0;
                Top = 0;
                Height = SystemParameters.MaximizedPrimaryScreenHeight - 15;
                Width = SystemParameters.MaximizedPrimaryScreenWidth - 15;
                isMaximized = true;
            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            isMaximized = false;
        }

        private void StartButtonClick(object sender, RoutedEventArgs e)
        {
            Save();
            if(Runner != null)
            {
                ConsoleRTB.SetText("");
                if (Runner.Running)
                {
                    Runner.Finish();
                }
                Runner.Startup();
            }
        }

        private void OnEnter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if(Runner != null)
                {
                    ConsoleRTB.AddText(InputField.Text, Brushes.Gray);
                    Runner.PutString(InputField.Text);
                }
                InputField.Text = "";
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (Runner != null)
            {
                Runner.PutString("");
            }
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            if (Runner != null)
            {
                ConsoleRTB.AddText("[Program terminated]", Brushes.Orange);
                Runner.Finish();
            }
        }



        CompletionWindow completionWindow;

        void textEditor_TextArea_TextEntered(object sender, TextCompositionEventArgs e)
        {
            if (e.Text == "_")
            {
                // Open code completion after the user has pressed dot:
                completionWindow = new CompletionWindow(CodeBlock.TextArea);
                completionWindow.Background = Brushes.Gray;
                completionWindow.Foreground = Brushes.LightCyan;
                completionWindow.BorderBrush = Brushes.Transparent;
                IList<ICompletionData> data = completionWindow.CompletionList.CompletionData;
                foreach (var predDescription in Model.SysPredicatesDescriptions)
                {
                    data.Add(new MyCompletionData(predDescription.Key, predDescription.Value));
                }
                completionWindow.Show();
                completionWindow.Closed += delegate {
                    completionWindow = null;
                };
            }
        }

        void textEditor_TextArea_TextEntering(object sender, TextCompositionEventArgs e)
        {
            if (e.Text.Length > 0 && completionWindow != null)
            {
                if (!char.IsLetterOrDigit(e.Text[0]))
                {
                    // Whenever a non-letter is typed while the completion window is open,
                    // insert the currently selected element.
                    completionWindow.CompletionList.RequestInsertion(e);
                }
            }
            // Do not set e.Handled=true.
            // We still want to insert the character that was typed.
        }

    }
}
