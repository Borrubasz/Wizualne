using Boruta.BooksCatalog.Core;
using Boruta.BooksCatalog.Interfaces;

namespace Boruta.BooksCatalog.BooksDBEF
{
    public class Book : IBook
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public Publisher Publisher { get; set; }

        IPublisher IBook.Publisher
        {
            get => Publisher;
            set
            {
                Publisher = value as Publisher;
            }
        }
        public BookGenre Genre { get; set; }
        public int YearPublished { get; set; }
    }
}
