using System.Collections.Generic;

namespace Boruta.BooksCatalog.Interfaces
{
    public interface IDAO
    {
        IEnumerable<IPublisher> GetAllPublishers();
        IEnumerable<IBook> GetAllBooks();

        IBook CreateNewBook();

        IPublisher CreateNewPublisher();

        void SaveBook(IBook book, bool editing);

        void DeleteBook(IBook book);

        void SavePublisher(IPublisher publisher, bool editing);
        void DeletePublisher(IPublisher producer);
    }
}
