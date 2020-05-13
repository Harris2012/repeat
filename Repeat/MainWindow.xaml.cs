using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

namespace Repeat
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var path = PathTextBox.Text;
                if (string.IsNullOrEmpty(path) || !Directory.Exists(path))
                {
                    return;
                }

                var fileName = FileNameTextBox.Text;
                if (string.IsNullOrEmpty(fileName) || !fileName.Contains("{item}"))
                {
                    return;
                }

                var request = ToRequest();
                if (request == null)
                {
                    return;
                }

                foreach (var item in request.Items)
                {
                    var itemFileName = fileName.Replace("{item}", item);
                    var itemFileFullName = System.IO.Path.Combine(path, itemFileName);
                    var content = Transform(request.Template, item);
                    File.WriteAllText(itemFileFullName, content, Encoding.UTF8);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void KeywordsTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextChanged();
        }

        private void TemplateTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextChanged();
        }

        private Request ToRequest()
        {
            string template = TemplateTextBox.Text;
            if (string.IsNullOrEmpty(template))
            {
                return null;
            }

            string keywords = KeywordsTextBox.Text;
            if (string.IsNullOrEmpty(keywords))
            {
                return null;
            }

            var items = keywords.Split(new string[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries).Distinct().ToList();
            if (items.Count == 0)
            {
                return null;
            }

            Request request = new Request();
            request.Items = items;
            request.Template = template;

            return request;
        }

        private void TextChanged()
        {
            var request = ToRequest();
            if (request == null)
            {
                return;
            }

            string template = TemplateTextBox.Text;
            var items = KeywordsTextBox.Text.Split(new string[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            List<string> contents = new List<string>();
            foreach (var item in items)
            {
                var content = Transform(template, item);
                contents.Add(content);
            }

            PreviewTextBox.Text = string.Join(Environment.NewLine, contents);
        }

        private string Transform(string template, string item)
        {
            return template.Replace("{item}", item);
        }
    }
}
