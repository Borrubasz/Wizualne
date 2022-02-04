using Boruta.BooksCatalog.Interfaces;

namespace Boruta.BooksCatalog.BooksDBEF
{
    public class Publisher : IPublisher
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
