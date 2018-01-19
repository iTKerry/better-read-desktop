using System.Collections.Generic;

namespace LoveRead.Model
{
    public class WebBook
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PagesCount { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
        public List<WebBookPage> Pages { get; set; }
    }
}
