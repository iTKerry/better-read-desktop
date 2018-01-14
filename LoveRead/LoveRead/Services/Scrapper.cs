using System;
using System.Linq;
using System.Threading.Tasks;
using LoveRead.Model;
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
        }

        public async Task<WordPage> ReadBook(string adress)
        {
            return await Test2(adress);

            async Task<WordPage> Test2(string adr)
            {
                var webPage = await NavigateBrowserToPage(adr);



                bool next = HasNextPage(webPage);

                //TODO: TEMP
                return new WordPage();
            }
        }

        private async Task<WebPage> NavigateBrowserToPage(string pageUrl) 
            => await _scrapingBrowser.NavigateToPageAsync(new Uri(pageUrl));

        private static bool HasNextPage(WebPage webPage)
            => webPage.Html.CssSelect("div.navigation").CssSelect("a").Any(node => node.InnerHtml == "Вперед");
    }

    public interface IScrapper
    {
        Task<WordPage> ReadBook(string adress);
    }
}
