using LoveRead.Model;

namespace LoveRead.Views.Tabs.SaveBook
{
    public partial class SaveBookViewModel
    {
        public SaveBookView SaveBookView { get; set; }

        private WebBook _book;
        public WebBook Book
        {
            get => _book;
            set => Set(() => Book, ref _book, value);
        }

        private string _saveAsPath;
        public string SaveAsPath
        {
            get => _saveAsPath;
            set => Set(() => SaveAsPath, ref _saveAsPath, value);
        }
    }
}