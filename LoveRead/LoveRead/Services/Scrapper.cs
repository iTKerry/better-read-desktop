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
    }

    public interface IScrapper
    {
    }
}
