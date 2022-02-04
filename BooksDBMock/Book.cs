using Boruta.BooksCatalog.Core;
using Boruta.BooksCatalog.Interfaces;

namespace Boruta.BooksCatalog.BooksDBMock
{
    class Book : IBook
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public IPublisher Publisher { get; set; }
        public BookGenre Genre { get; set; }
        public int YearPublished { get; set; }
    }
}
