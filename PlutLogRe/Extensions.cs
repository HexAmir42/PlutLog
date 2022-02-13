using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace PlutLogRe
{
    public static class Extensions
    {
        public static void SetText(this RichTextBox richTextBox, string text)
        {
            richTextBox.Document.Blocks.Clear();
            richTextBox.Document.Blocks.Add(new Paragraph(new Run(text)));
        }
        public static void AddText(this RichTextBox richTextBox, string text, Brush brush)
        {
            var par = new Paragraph(new Run(text));
            par.Foreground = brush;
            richTextBox.Document.Blocks.Add(par);
        }

        public static string GetText(this RichTextBox richTextBox)
        {
            return new TextRange(richTextBox.Document.ContentStart,
                richTextBox.Document.ContentEnd).Text;
        }
    }
}
