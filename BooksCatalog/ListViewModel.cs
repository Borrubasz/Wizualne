using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;

namespace Boruta.BooksCatalog.UI
{
    public class ListViewModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<BookViewModel> books;

        public ObservableCollection<BookViewModel> Books
        {
            get => books;
            set
            {
                books = value;
                RaisePropertyChanged(nameof(Books));
            }
        }

        private PublisherListViewModel publisherListViewModel = new PublisherListViewModel();
        public PublisherListViewModel PublisherListViewModel
        {
            get => publisherListViewModel;
        }

        private ListCollectionView view;

        private BLC.BLC blc;

        public ListViewModel()
        {
            BLC.BLC.LibraryName = new Properties.Settings().Source;
            blc = BLC.BLC.GetBLC();
            books = new ObservableCollection<BookViewModel>();
            view = (ListCollectionView) CollectionViewSource.GetDefaultView(books);
            foreach(var b in blc.GetBooks())
            {
                books.Add(new BookViewModel(b));
            }
            RaisePropertyChanged(nameof(Books));
            #region book commands
            addNewBookCommand = new RelayCommand(addNewBook);
            saveBookCommand = new RelayCommand(saveBook, canSaveBook);
            deleteBookCommand = new RelayCommand(deleteBook, canDeleteBook);
            filterCommand = new RelayCommand(filter);
            #endregion
        }
        private void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private BookViewModel editedBook = null;

        public BookViewModel EditedBook
        {
            get => editedBook;
            set
            {
                editedBook = value;
                RaisePropertyChanged(nameof(EditedBook));
            }
        }

        private BookViewModel selectedBook = null;

        public BookViewModel SelectedBook
        {
            get => selectedBook;
            set
            {
                selectedBook = value;
                if (SelectedBook != null)
                {
                    EditedBook = SelectedBook.Copy();
                }
                else
                {
                    EditedBook = null;
                }
                RaisePropertyChanged(nameof(SelectedBook));
            }
        }

        private string filterValue;

        public string FilterValue
        {
            get => filterValue;
            set
            {
                filterValue = value;
            }
        }

        #region book commands implementation
        private RelayCommand addNewBookCommand;
        
        public ICommand AddNewBookCommand
        {
            get => addNewBookCommand;
        }

        private void addNewBook(object obj)
        {
            BookViewModel bvm = new BookViewModel(blc.CreateNewBook());
            EditedBook = bvm;
        }

        private RelayCommand saveBookCommand;

        public ICommand SaveBookCommand
        {
            get => saveBookCommand;
        }

        private void saveBook(object obj)
        {
            if (selectedBook != null)
            {
                SelectedBook.Title = EditedBook.Title;
                SelectedBook.Author = EditedBook.Author;
                SelectedBook.Publisher = EditedBook.Publisher;
                SelectedBook.Genre = EditedBook.Genre;
                SelectedBook.YearPublished = EditedBook.YearPublished;
                SelectedBook.SaveBook(true);
                SelectedBook = null;
            }
            else
            {
                EditedBook.SaveBook(false);
                books.Add(editedBook);
                EditedBook = null;
            }
        }

        private bool canSaveBook(object obj)
        {
            if (editedBook == null)
            {
                return false;
            }
            else
            {
                return !editedBook.HasErrors;
            }
        }

        private RelayCommand deleteBookCommand;

        public ICommand DeleteBookCommand
        {
            get => deleteBookCommand;
        }

        private void deleteBook(object obj)
        {
            SelectedBook.DeleteBook();
            Books.Remove(selectedBook);
            SelectedBook = null;
        }

        private bool canDeleteBook(object obj)
        {
            if(selectedBook == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private RelayCommand filterCommand;

        public ICommand FilterCommand
        {
            get => filterCommand;
        }

        private void filter(object obj)
        {
            if (!string.IsNullOrWhiteSpace(filterValue))
            {
                view.Filter = (b) => ((BookViewModel)b).Title.Contains(filterValue); 
            }
            else
            {
                view.Filter = null;
            }
        }
        #endregion
    }
}
