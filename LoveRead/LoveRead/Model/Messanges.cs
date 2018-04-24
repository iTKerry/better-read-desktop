namespace LoveRead.Model
{
    public class LogMessange
    {
        public string Text { get; set; }
    }

    public class ProgressMessange
    {
        public int Current { get; set; }
        public int Total { get; set; }
        public ProgressType ProgressType { get; set; }
    }

    public enum ProgressType
    {
        Reading,
        Writing
    }

    public class TabSwitchMessange
    {
        public object Data { get; set; }
    }
}
