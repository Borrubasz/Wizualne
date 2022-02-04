using Boruta.BooksCatalog.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Boruta.BooksCatalog.BLC
{
    public sealed class BLC
    {
        IDAO dao;
        private static BLC blc = null;
        public static string LibraryName { get; set; }

        private BLC(string libraryName)
        {
            Assembly a = Assembly.LoadFrom(libraryName);

            Type typeToCreate = null;
            Type IDAOType = typeof(IDAO);
            foreach(var t in a.GetTypes())
            {
                if(IDAOType.IsAssignableFrom(t))
                {
                    typeToCreate = t;
                    break;
                }
            }
            if(typeToCreate != null)
            {
                dao = (IDAO)Activator.CreateInstance(typeToCreate);
            }
        }
        
        public static BLC GetBLC()
        {
            if (blc == null && !string.IsNullOrEmpty(LibraryName))
            {
                blc = new BLC(LibraryName);
            }
            return blc;
        }
        public IEnumerable<IBook> GetBooks()
        {
            return dao.GetAllBooks();
        }

        public IBook CreateNewBook()
        {
            return dao.CreateNewBook();
        }

        public void SaveBook(IBook book, bool editing)
        {
            dao.SaveBook(book, editing);
        }

        public void DeleteBook(IBook book)
        {
            dao.DeleteBook(book);
        }

        public IEnumerable<IPublisher> GetPublishers()
        {
            return dao.GetAllPublishers();
        }

        public IPublisher CreateNewPublisher()
        {
            return dao.CreateNewPublisher();
        }

        public void SavePublisher(IPublisher publisher, bool editing)
        {
            dao.SavePublisher(publisher, editing);
        }

        public void DeletePublisher(IPublisher publisher)
        {
            dao.DeletePublisher(publisher);
        }
    }
}
