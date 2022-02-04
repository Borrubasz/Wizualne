namespace Boruta.BooksCatalog.Interfaces
{
    public interface IBook
    {
        int ID { get; set; }
        string Title { get; set; }
        string Author { get; set; }
        IPublisher Publisher { get; set; }
        Core.BookGenre Genre { get; set; }
        int YearPublished { get; set; }

    }
}
