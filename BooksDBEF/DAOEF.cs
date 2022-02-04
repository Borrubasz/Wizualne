using Boruta.BooksCatalog.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Boruta.BooksCatalog.BooksDBEF
{
    public class DAOEF: DbContext, IDAO
    {
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Book> Books { get; set; }

        public IEnumerable<IPublisher> GetAllPublishers()
        {
            return Publishers.ToList();
        }

        public IEnumerable<IBook> GetAllBooks()
        {
            return Books.ToList();
        }

        public IBook CreateNewBook()
        {
            return new Book();
        }

        public IPublisher CreateNewPublisher()
        {
            return new Publisher();
        }        

        public void SaveBook(IBook book, bool editing)
        {
            if(!editing)
            {
                Books.Add(book as Book);
            }
            else
            {
                Books.Update(book as Book);
            }
            SaveChanges();
        }

        public void DeleteBook(IBook book)
        {
            Books.Remove(book as Book);
            SaveChanges();
        }

        public void SavePublisher(IPublisher publisher, bool editing)
        {
            if (!editing)
            {
                Publishers.Add(publisher as Publisher);
            }
            else
            {
                Publishers.Update(publisher as Publisher);
            }
            SaveChanges();
        }

        public void DeletePublisher(IPublisher publisher)
        {
            Publishers.Remove(publisher as Publisher);
            SaveChanges();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            string db_path = @"C:\Users\pawel\source\repos\BooksCatalog\Books_db.db";
            optionsBuilder.UseSqlite($"Filename={db_path}");

        }
    }
}
