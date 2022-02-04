using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Boruta.BooksCatalog.UI
{
    public class BookViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        private Interfaces.IBook book;
        public BookViewModel(Interfaces.IBook _book)
        {
            book = _book;
        }

        public BookViewModel Copy()
        {
            return new BookViewModel(book);
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }

            if (propertyName != nameof(HasErrors))
            {
                Validate();
            }
        }
        #endregion

        #region INotifyDataErrorInfo

        private Dictionary<string, ICollection<string>> _validationErrors = new Dictionary<string, ICollection<string>>();

        public bool HasErrors
        {
            get => _validationErrors.Count > 0;
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        protected void RaiseErrorChanged(string property)
        {
            if (ErrorsChanged != null)
            {
                ErrorsChanged(this, new DataErrorsChangedEventArgs(property));
                RaisePropertyChanged(nameof(HasErrors));
            }
        }

        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName) || !_validationErrors.ContainsKey(propertyName))
            {
                return null;
            }
            return _validationErrors[propertyName];
        }

        public void Validate()
        {
            var validationContext = new ValidationContext(this, null, null);
            var validationResults = new List<ValidationResult>();

            Validator.TryValidateObject(this, validationContext, validationResults, true);

            //usunięcie tych wpisów w słowniku dla których nie ma już błędów
            foreach (var kv in _validationErrors.ToList())
            {
                if (validationResults.All(r => r.MemberNames.All(m => m != kv.Key)))
                {
                    _validationErrors.Remove(kv.Key);
                    RaiseErrorChanged(kv.Key);
                }
            }

            var q = from result in validationResults
                    from member in result.MemberNames
                    group result by member into gr
                    select gr;

            foreach (var prop in q)
            {
                var messages = prop.Select(r => r.ErrorMessage).ToList();

                if (_validationErrors.ContainsKey(prop.Key))
                {
                    _validationErrors.Remove(prop.Key);
                }
                _validationErrors.Add(prop.Key, messages);
                RaiseErrorChanged(prop.Key);
            }
        }
        #endregion

        [Required]
        [Key]
        public int BookID
        {
            get => book.ID;
            set
            {
                book.ID = value;
                RaisePropertyChanged(nameof(BookID));
            }
        }
        [Required(ErrorMessage = "Title is required")]
        public string Title
        {
            get => book.Title;
            set
            {
                book.Title = value;
                RaisePropertyChanged(nameof(Title));
            }
        }
        [Required(ErrorMessage = "Author name is required")]
        [StringLength(40, MinimumLength = 5, ErrorMessage = "Author name must have at least 5 and not more than 40 characters")]
        public string Author
        {
            get => book.Author;
            set
            {
                book.Author = value;
                RaisePropertyChanged(nameof(Author));
            }
        }
        public Interfaces.IPublisher Publisher
        {
            get => book.Publisher;
            set
            {
                book.Publisher = value;
                RaisePropertyChanged(nameof(Publisher));
            }
        }
        public Core.BookGenre Genre
        {
            get => book.Genre;
            set
            {
                book.Genre = value;
                RaisePropertyChanged(nameof(Genre));
            }
        }
        public int YearPublished
        {
            get => book.YearPublished;
            set
            {
                book.YearPublished = value;
                RaisePropertyChanged(nameof(YearPublished));
            }
        }

        public void SaveBook(bool editing)
        {
            BLC.BLC blc = BLC.BLC.GetBLC();
            blc.SaveBook(book, editing);
        }

        public void DeleteBook()
        {
            BLC.BLC blc = BLC.BLC.GetBLC();
            blc.DeleteBook(book);
        }
    }
}
