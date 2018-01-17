using System;
using System.Linq;
using System.Xml.Linq;
using LoveRead.Model;
using Xceed.Words.NET;

namespace LoveRead.Infrastructure
{
    public class DocService : IDocService
    {
        public void Save(WebBook book)
        {
            DocX doc = DocX.Create($"{book.Name}.docx");
            
            foreach (var page in book.Pages)
            {
                foreach (var data in page.WebBookTexts)
                {
                    switch (data)
                    {
                        case WebBookHeader _:
                            InsertHeader(doc, data.Text);
                            break;
                        case WebBookParagraph _:
                            InsertParagraph(doc, data.Text);
                            break;
                    }
                }
            }

            doc.Save();
        }

        public void SaveAs(WebBook book, string path)
        {
            throw new NotImplementedException();
        }

        private void InsertParagraph(DocX doc, string text)
        {
            var paragraph = doc.InsertParagraph();
            paragraph.Append(text).IndentationFirstLine = 1;
            paragraph.SpacingAfter(5);
            if (Equals(text.First(), '—'))
                paragraph.Italic();
        }

        private void InsertHeader(DocX doc, string text)
        {
            var header = doc.InsertParagraph();
            header.Append(text);
            header.FontSize(20);
            header.Alignment = Alignment.center;
            header.Bold();
            header.SpacingBefore(8);
            header.SpacingAfter(13);
        }
    }

    public interface IDocService
    {
        void Save(WebBook book);
        void SaveAs(WebBook book, string path);
    }
}
