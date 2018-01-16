using LoveRead.Model;
using Xceed.Words.NET;

namespace LoveRead.Infrastructure
{
    public class DocService : IDocService
    {
        public void Save(WebBook book)
        {
            DocX doc = DocX.Create("test.docx");

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
            DocX doc = DocX.Create("test");
            doc.SaveAs($"{book.Name}.docx");
        }

        private void InsertParagraph(DocX doc, string text) 
            => doc.InsertParagraph(text);

        private void InsertHeader(DocX doc, string text) 
            => doc.InsertParagraph(text).FontSize(20).Alignment = Alignment.center;
    }

    public interface IDocService
    {
        void Save(WebBook book);
        void SaveAs(WebBook book, string path);
    }
}
