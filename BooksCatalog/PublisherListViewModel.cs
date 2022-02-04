using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;

namespace Boruta.BooksCatalog.UI
{
    public class PublisherListViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<PublisherViewModel> publishers;

        public ObservableCollection<PublisherViewModel> Publishers
        {
            get => publishers;
            set
            {
                publishers = value;
                RaisePropertyChanged(nameof(Publishers));
            }
        }

        private ListCollectionView view;

        private ObservableCollection<Interfaces.IPublisher> iPublishers;
        public ObservableCollection<Interfaces.IPublisher> IPublishers
        {
            get => iPublishers;
            set
            {
                iPublishers = value;
                RaisePropertyChanged(nameof(IPublishers));
            }
        }
        private void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private BLC.BLC blc;

        public PublisherListViewModel()
        {
            BLC.BLC.LibraryName = new Properties.Settings().Source;
            blc = BLC.BLC.GetBLC();
            publishers = new ObservableCollection<PublisherViewModel>();
            IPublishers = new ObservableCollection<Interfaces.IPublisher>(blc.GetPublishers());
            view = (ListCollectionView)CollectionViewSource.GetDefaultView(publishers);
            foreach (var p in IPublishers)
            {
                publishers.Add(new PublisherViewModel(p));
            }
            RaisePropertyChanged(nameof(Publishers));
            #region publisher commands
            addNewPublisherCommand = new RelayCommand(addNewPublisher);
            savePublisherCommand = new RelayCommand(savePublisher, canSavePublisher);
            deletePublisherCommand = new RelayCommand(deletePublisher, canDeletePublisher);
            filterCommand = new RelayCommand(filter);
            #endregion
        }

        private PublisherViewModel editedPublisher = null;

        public PublisherViewModel EditedPublisher
        {
            get => editedPublisher;
            set
            {
                editedPublisher = value;
                RaisePropertyChanged(nameof(EditedPublisher));
            }
        }

        private PublisherViewModel selectedPublisher = null;

        public PublisherViewModel SelectedPublisher
        {
            get => selectedPublisher;
            set
            {
                selectedPublisher = value;
                if (SelectedPublisher != null)
                {
                    EditedPublisher = SelectedPublisher.Copy();
                }
                else
                {
                    EditedPublisher = null;
                }
                RaisePropertyChanged(nameof(SelectedPublisher));
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

        #region publisher commands implementation
        private RelayCommand addNewPublisherCommand;

        public ICommand AddNewPublisherCommand
        {
            get => addNewPublisherCommand;
        }

        private void addNewPublisher(object obj)
        {
            PublisherViewModel pvm = new PublisherViewModel(blc.CreateNewPublisher());
            EditedPublisher = pvm;
        }

        private RelayCommand savePublisherCommand;

        public ICommand SavePublisherCommand
        {
            get => savePublisherCommand;
        }

        private void savePublisher(object obj)
        {
            if (selectedPublisher != null)
            {
                iPublishers[iPublishers.IndexOf(selectedPublisher.GetIPublisher())] = editedPublisher.GetIPublisher();
                SelectedPublisher.Name = EditedPublisher.Name;
                SelectedPublisher.SavePublisher(true);
                SelectedPublisher = null;
            }
            else
            {
                EditedPublisher.SavePublisher(false);
                iPublishers.Add(editedPublisher.GetIPublisher());
                Publishers.Add(editedPublisher);
                EditedPublisher = null;
            }
        }

        private bool canSavePublisher(object obj)
        {
            if (editedPublisher == null)
            {
                return false;
            }
            else
            {
                return !editedPublisher.HasErrors;
            }
        }

        private RelayCommand deletePublisherCommand;

        public ICommand DeletePublisherCommand
        {
            get => deletePublisherCommand;
        }

        private void deletePublisher(object obj)
        {
            EditedPublisher.DeletePublisher();
            iPublishers.Remove(editedPublisher.GetIPublisher());
            Publishers.Remove(selectedPublisher);
        }

        private bool canDeletePublisher(object obj)
        {
            if (selectedPublisher == null)
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
                view.Filter = (p) => ((PublisherViewModel)p).Name.Contains(filterValue);
            }
            else
            {
                view.Filter = null;
            }
        }
        #endregion
    }
}
