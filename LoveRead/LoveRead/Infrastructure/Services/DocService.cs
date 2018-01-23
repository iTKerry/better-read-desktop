using System;
using System.IO;
using System.Linq;
using LoveRead.Model;
using Xceed.Words.NET;

namespace LoveRead.Infrastructure.Services
{
    public class DocService : IDocService
    {
        private readonly IDownloadService _download;

        private static readonly string DownloadsPath = Directory.GetCurrentDirectory() + "\\Downloads";
        private const string FileTemplate = "{0}\\{1}.docx";

        public DocService(IDownloadService download)
        {
            _download = download;
        }

        public void Save(WebBook book)
        {
            var fileName = string.Format(FileTemplate, "Downloads", book.Name);

            if (!Directory.Exists(DownloadsPath))
                Directory.CreateDirectory(DownloadsPath);

            if(new DirectoryInfo(DownloadsPath).GetFiles().Any(f => f.Name.Contains(book.Name)))
                return;

            DocX doc = DocX.Create(fileName);
            doc.DifferentFirstPage = true;
            doc.AddFooters();
            
            InsertImage(doc, _download.DownloadFile(book.ImageUrl, book.Name));
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

            var even = doc.Footers.Even.InsertParagraph("Page №");
            even.Alignment = Alignment.center;
            even.AppendPageNumber(PageNumberFormat.normal);
            var odd = doc.Footers.Odd.InsertParagraph("Page №");
            odd.Alignment = Alignment.center;
            odd.AppendPageNumber(PageNumberFormat.normal);

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

        private void InsertImage(DocX doc, string imgPath)
        {
            var image = doc.AddImage(imgPath);
            var picture = image.CreatePicture();
            var p = doc.InsertParagraph();
            p.Alignment = Alignment.center;
            p.AppendPicture(picture);
            p.SpacingAfter(30)
                .InsertPageBreakAfterSelf();
        }
    }

    public interface IDocService
    {
        void Save(WebBook book);
        void SaveAs(WebBook book, string path);
    }
}
