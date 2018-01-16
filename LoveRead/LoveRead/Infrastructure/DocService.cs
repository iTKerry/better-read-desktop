using System;
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
            => doc.InsertParagraph(text).IndentationFirstLine = 1;

        private void InsertHeader(DocX doc, string text) 
            => doc.InsertParagraph(text).FontSize(20).Alignment = Alignment.center;
    }

    public interface IDocService
    {
        void Save(WebBook book);
        void SaveAs(WebBook book, string path);
    }
}
