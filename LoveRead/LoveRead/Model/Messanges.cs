namespace LoveRead.Model
{
    public class LogMessange
    {
        public string Text { get; set; }
    }

    public class TabSwitchMessange
    {
        public object Data { get; set; }

        public TabSwitchMessange(object data)
        {
            this.Data = data;
        }
    }
}
