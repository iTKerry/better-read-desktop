using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LoveRead.Model;
using ScrapySharp.Extensions;
using ScrapySharp.Network;

namespace LoveRead.Infrastructure
{
    public class LibraryScrapper : ILibraryScrapper
    {
        private readonly ScrapingBrowser _scrapingBrowser;
        private readonly Regex _idFromUrlRegex = new Regex(@"[^?]+(?:\?id=([^&]+).*)?");

        private const string BookPattern = "http://loveread.ec/read_book.php?id={0}&p={1}";
        

        public LibraryScrapper(ScrapingBrowser scrapingBrowser)
        {
            _scrapingBrowser = scrapingBrowser;
        }

        public async Task<WebBook> ReadBook(string bookUrl)
        {
            var firstPage = await NavigateBrowserToPage(bookUrl);
            WebBook book = new WebBook
            {
                Id = int.Parse(_idFromUrlRegex.Match(bookUrl).Groups[1].Value),
                Url = bookUrl,
                PagesCount = firstPage.Html.CssSelect("div.navigation > a").Select(n => n.InnerHtml)
                    .Where(t => int.TryParse(t, out int _)).Select(int.Parse).Max(),
                Pages = new List<WebBookPage>()
            };

            int currentPageNumber = 1;
            while (currentPageNumber < book.PagesCount)
            {
                var currentPageUrl = string.Format(BookPattern, book.Id, currentPageNumber++);
                var currentPage = await NavigateBrowserToPage(currentPageUrl);
                book.Pages.Add(new WebBookPage {WebBookTexts = GetPageText(currentPage)});
            }

            return book;
        }

        private IEnumerable<IWebBookText> GetPageText(WebPage webPage)
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
    }

    public interface ILibraryScrapper
    {
        Task<WebBook> ReadBook(string bookUrl);
    }
}
