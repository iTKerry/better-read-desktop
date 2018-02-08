namespace LoveRead.Views.Tabs.ReadBook
{
    public partial class ReadBookView 
    {
        public ReadBookView()
        {
            InitializeComponent();
            ((ReadBookViewModel) DataContext).ReadBookView = this;
        }
    }
}
