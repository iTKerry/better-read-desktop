using System.Collections.Generic;

namespace LoveRead.Model
{
    public class WebBook
    {
        public int Id { get; set; }
        public int PagesCount { get; set; }
        public string Url { get; set; }
        public List<WebBookPage> Pages { get; set; }
    }
}
