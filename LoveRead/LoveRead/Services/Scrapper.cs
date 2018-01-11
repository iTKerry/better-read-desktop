using System;
using System.Linq;
using System.Threading.Tasks;
using ScrapySharp.Extensions;
using ScrapySharp.Network;

namespace LoveRead.Services
{
    public class Scrapper : IScrapper
    {
        private readonly ScrapingBrowser _scrapingBrowser;

        public Scrapper(ScrapingBrowser scrapingBrowser)
        {
            _scrapingBrowser = scrapingBrowser;

            Test1();
        }

        private async void Test1()
        {
            await Test2();
        }

        private async Task Test2()
        {
            var page = await _scrapingBrowser.NavigateToPageAsync(new Uri("http://loveread.ec/read_book.php?id=69223&p=1"));

            var nodes = page.Html.CssSelect("p.MsoNormal");
            string[] pages = nodes.Select(n => n.InnerHtml).ToArray();
        }
    }

    public interface IScrapper
    {
    }
}
