using System.Collections.Generic;

namespace LoveRead.Model
{
    public class WebBookPage
    {
        public IEnumerable<IWebBookText> WebBookTexts { get; set; }
    }

    public interface IWebBookText
    {
        string Text { get; set; }
    }

    public class WebBookParagraph : IWebBookText
    {
        public string Text { get; set; }
    }

    public class WebBookHeader : IWebBookText
    {
        public string Text { get; set; }
    }
}
