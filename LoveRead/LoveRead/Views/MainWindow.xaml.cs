using LoveRead.ViewModel;

namespace LoveRead.Views
{
    public partial class MainWindow : IMainView
    {
        public MainWindow()
        {
            InitializeComponent();
            ((MainViewModel) DataContext).MainView = this;
        }

        public void ScrollToEnd()
        {
            this.LogList.ScrollToEnd();
        }
    }

    public interface IMainView
    {
        void ScrollToEnd();
    }
}
