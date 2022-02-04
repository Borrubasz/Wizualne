using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Boruta.BooksCatalog.UI
{
    public class PublisherViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        private Interfaces.IPublisher publisher;
        public PublisherViewModel(Interfaces.IPublisher _publisher)
        {
            publisher = _publisher;
        }

        public PublisherViewModel Copy()
        {
            return new PublisherViewModel(publisher);
        }

        public Interfaces.IPublisher GetIPublisher()
        {
            return publisher;
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
        public int PublisherID
        {
            get => publisher.ID;
            set
            {
                publisher.ID = value;
                RaisePropertyChanged(nameof(PublisherID));
            }
        }
        [Required(ErrorMessage = "Name is required")]
        public string Name
        {
            get => publisher.Name;
            set
            {
                publisher.Name = value;
                RaisePropertyChanged(nameof(Name));
            }
        }

        public void SavePublisher(bool editing)
        {
            BLC.BLC blc = BLC.BLC.GetBLC();
            blc.SavePublisher(publisher, editing);
        }

        public void DeletePublisher()
        {
            BLC.BLC blc = BLC.BLC.GetBLC();
            blc.DeletePublisher(publisher);
        }
    }
}
