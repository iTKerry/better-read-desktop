namespace LoveRead.Model
{
    public class WordPage
    {
        public IWordText[] WordTexts { get; set; }
    }

    public interface IWordText
    {
        string Text { get; set; }
    }

    public class WordParagraph : IWordText
    {
        public string Text { get; set; }
    }

    public class WordHeader : IWordText
    {
        public string Text { get; set; }
    }
}
