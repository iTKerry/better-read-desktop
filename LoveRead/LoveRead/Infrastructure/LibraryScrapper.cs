using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LoveRead.Infrastructure.Services;
using LoveRead.Model;
using ScrapySharp.Extensions;
using ScrapySharp.Network;

namespace LoveRead.Infrastructure
{
    public interface ILibraryScrapper
    {
        Task<WebBook> ReadBook(string bookUrl);
    }

    public class LibraryScrapper : ILibraryScrapper
    {
        private readonly IMessangerService _messanger;
        private readonly ScrapingBrowser _scrapingBrowser;
        
        private readonly Regex _idFromUrlRegex = new Regex(@"[^?]+(?:\?id=([^&]+).*)?");

        private const string BookPattern = "http://loveread.ec/read_book.php?id={0}&p={1}";
        private const string ImagePattern = "http://loveread.ec/img/photo_books/{0}.jpg";

        public LibraryScrapper(ScrapingBrowser scrapingBrowser, IMessangerService messanger)
        {
            _scrapingBrowser = scrapingBrowser;
            _messanger = messanger;
        }

        public async Task<WebBook> ReadBook(string bookUrl)
        {
            Log("Reading started!");
            var book = await GetBookBaseInfo();
            Log($"Book '{book.Name}' has ID={book.Id} with {book.PagesCount} pages");
            await GetBookPages();
            Log("Done!");
            return book;

            async Task<WebBook> GetBookBaseInfo()
            {
                var bookId = int.Parse(_idFromUrlRegex.Match(bookUrl).Groups[1].Value);
                var firstPage = bookUrl.Contains("view_global.php?")
                    ? await NavigateBrowserToPage(string.Format(BookPattern, bookId, 1))
                    : await NavigateBrowserToPage(bookUrl);

                var webBook = new WebBook
                {
                    Id = bookId,
                    Url = bookUrl,
                    Name = firstPage.Html.CssSelect("td.tb_read_book > h2 > a")
                        .Where(n => n.GetAttributeValue("href").Contains($"view_global.php?id={bookId}"))
                        .Select(a => a.GetAttributeValue("title")).First(),
                    Author = firstPage.Html.CssSelect("a").First(a => a.GetAttributeValue("href").Contains("author=")).GetAttributeValue("title"),
                    ImageUrl = string.Format(ImagePattern, bookId),
                    PagesCount = firstPage.Html.CssSelect("div.navigation > a").Select(n => n.InnerHtml)
                        .Where(t => int.TryParse(t, out _)).Select(int.Parse).Max(),
                    Pages = new List<WebBookPage>()
                };
                return webBook;
            }

            async Task GetBookPages()
            {
                var currentPageNumber = 1;
                while (currentPageNumber <= book.PagesCount)
                {
                    var currentPageUrl = string.Format(BookPattern, book.Id, currentPageNumber);
                    Log($"Navigating to: {currentPageUrl}");
                    var currentPage = await NavigateBrowserToPage(currentPageUrl);
                    book.Pages.Add(new WebBookPage {WebBookTexts = GetPageText(currentPage)});

                    currentPageNumber++;
                }
            }
        }

        private static IEnumerable<IWebBookText> GetPageText(WebPage webPage)
        {
            var nodes = webPage.Html.CssSelect("div.MsoNormal").SingleOrDefault()?.ChildNodes.Nodes();
            if (nodes == null) yield break;

            foreach (var node in nodes)
            {
                if (node.Attributes.Any(attr => attr.Value == "take_h1"))
                    yield return new WebBookHeader {Text = node.InnerText};
                if (node.Attributes.Any(attr => attr.Value == "MsoNormal"))
                    yield return new WebBookParagraph {Text = node.InnerText};
            }
        }

        private async Task<WebPage> NavigateBrowserToPage(string pageUrl) 
            => await _scrapingBrowser.NavigateToPageAsync(new Uri(pageUrl));

        private void Log(string messange) 
            => _messanger.NotifyLog(new LogMessange {Text = messange});
    }
}
