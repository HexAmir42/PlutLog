using System;
using System.IO;
using System.Text;

namespace PlutLogRe.Models
{
    public class PlutDocument
    {
        public readonly string FileName;
        private FileInfo file;
        public string Content;

        public PlutDocument(string fileName)
        {
            FileName = fileName;
            file = new FileInfo(fileName);
            using (StreamReader streamReader = file.OpenText())
            {
                string content = "";
                string line = "";
                while ((line = streamReader.ReadLine()) != null)
                {
                    content += line + "\n";
                }
                Content = content;
            }
        }
        public PlutDocument(string fileName, string content)
        {
            FileName = fileName;
            file = new FileInfo(fileName);
            Content = content;
            Save();
        }

        public void Update(string content)
        {
            Content = content;
        }

        public void Save()
        {
            using (StreamWriter writer = new StreamWriter(FileName, false))
            {
                writer.Write(Content);
            }
        }
    }
}
