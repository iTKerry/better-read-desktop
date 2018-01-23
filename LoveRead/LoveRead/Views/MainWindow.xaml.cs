using System.ComponentModel;
using LoveRead.Properties;
using LoveRead.ViewModel;

namespace LoveRead.Views
{
    public partial class MainWindow : IMainView
    {
        private MainViewModel MainViewModel => (MainViewModel) DataContext;

        public MainWindow()
        {
            InitializeComponent();
            MainViewModel.MainView = this;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            Settings.Default.DownloadPath = MainViewModel.SaveAsPath;
            Settings.Default.Save();
        }

        public void ScrollLogToEnd() => LogList.ScrollToEnd();
    }

    public interface IMainView
    {
        void ScrollLogToEnd();
    }
}
