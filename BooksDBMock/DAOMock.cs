using Boruta.BooksCatalog.Interfaces;
using System.Collections.Generic;

namespace Boruta.BooksCatalog.BooksDBMock
{
    public class DAOMock : IDAO
    {
        private List<Publisher> publishers;
        private List<Book> books;
        public DAOMock()
        {
            publishers = new List<Publisher>()
            {
                new Publisher() { ID = 1, Name = "Fabryka Słów"},
                new Publisher() { ID = 2, Name = "Agora"}
            };

            books = new List<Book>()
            {
                new Book() { ID = 1, Title = "Księga Jesiennych Demonów", Author = "Jarosław Grzędowicz", Genre = Core.BookGenre.Urban_fantasy, Publisher = publishers[0], YearPublished = 2003},
                new Book() { ID = 2, Title = "Kroniki Jakuba Wędrowycza", Author = "Andrzej Pilipiuk", Genre = Core.BookGenre.Rural_fantasy, Publisher = publishers[0], YearPublished = 2001},
                new Book() { ID = 3, Title = "Zmorojewo", Author = "Jakub Żulczyk", Genre = Core.BookGenre.Rural_fantasy, Publisher = publishers[1], YearPublished = 2011}
            };
        }

        public IBook CreateNewBook()
        {
            return new Book();
        }

        public IPublisher CreateNewPublisher()
        {
            return new Publisher();
        }

        public void DeleteBook(IBook book)
        {
            
        }

        public void DeletePublisher(IPublisher producer)
        {
            
        }

        public IEnumerable<IBook> GetAllBooks()
        {
            return books;
        }

        public IEnumerable<IPublisher> GetAllPublishers()
        {
            return publishers;
        }

        public void SaveBook(IBook book, bool editing)
        {
            
        }

        public void SavePublisher(IPublisher publisher, bool editing)
        {
            
        }
    }
}
