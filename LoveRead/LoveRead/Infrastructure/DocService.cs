using Xceed.Words.NET;

namespace LoveRead.Infrastructure
{
    public class DocService
    {
        public DocService()
        {
            DocX temp = DocX.Create("test", DocumentTypes.Document);
        }
    }
}
