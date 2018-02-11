namespace LoveRead.Views.Tabs.BookDetails
{
    public partial class BookDetailsView
    {
        public BookDetailsView()
        {
            InitializeComponent();
            ((BookDetailsViewModel) DataContext).BookDetailsView = this;
        }
    }
}
