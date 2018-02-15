namespace LoveRead.Views.Tabs.SaveBook
{
    public partial class SaveBookView
    {
        public SaveBookView()
        {
            InitializeComponent();
            ((SaveBookViewModel)DataContext).SaveBookView = this;
        }
    }
}
