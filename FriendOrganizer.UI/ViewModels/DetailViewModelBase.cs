using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using FriendOrganizer.Model;
using FriendOrganizer.UI.Events;
using FriendOrganizer.UI.Views.Services;
using Prism.Commands;
using Prism.Events;

namespace FriendOrganizer.UI.ViewModels
{
    public abstract class DetailViewModelBase : ViewModelBase, IDetailViewModel
    {
        protected readonly IEventAggregator EventAggregator;
        protected readonly IMessageDialogService MessageDialogService;
        private bool _hasChanges;
        private int _id;
        private string _title;

        public DetailViewModelBase(IEventAggregator eventAggregator, IMessageDialogService messageDialogService)
        {
            EventAggregator = eventAggregator;
            MessageDialogService = messageDialogService;

            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
            DeleteCommand = new DelegateCommand(OnDeleteExecute);
            CloseDetailViewCommand = new DelegateCommand(OnCloseDetailViewExecute);
        }


        public abstract Task LoadAsync(int id);

        public bool HasChanges
        {
            get => _hasChanges;
            set
            {
                if (_hasChanges != value)
                {
                    _hasChanges = value;
                    OnPropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public int Id
        {
            get => _id;
            protected set => _id = value;
        }

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand CloseDetailViewCommand { get; }

        protected abstract void OnDeleteExecute();
        protected abstract void OnSaveExecute();
        protected abstract bool OnSaveCanExecute();


        protected virtual void RaiseDetailDeletedEvent(int entityId)
        {
            EventAggregator.GetEvent<AfterDetailDeletedEvent>().Publish(new AfterDetailDeletedEventArgs()
            {
                Id = entityId,
                ViewModelName = this.GetType().Name
            });
        }

        protected virtual void RaiseDetailSavedEvent(int entityId, string displayMember)
        {
            EventAggregator.GetEvent<AfterDetailSavedEvent>().Publish(new AfterDetailSavedEventArgs()
            {
                Id = entityId,
                DisplayMember = displayMember,
                ViewModelName = this.GetType().Name
            });
        }

        protected virtual void RaiseCollectionSavedEvent()
        {
            EventAggregator.GetEvent<AfterCollectionSavedEvent>().Publish(new AfterCollectionSavedEventArgs()
            {
                ViewModelName = this.GetType().Name
            });
        }
        protected virtual void OnCloseDetailViewExecute()
        {
            if (HasChanges)
            {
                var result = MessageDialogService.ShowOkCancelDialog("You've made changes. Do you want to close this item?",
                    "Question");

                if (result == MessageDialogResult.Cancel)
                    return;
            }

            EventAggregator.GetEvent<AfterDetailClosedEvent>().Publish(new AfterDetailClosedEventArgs()
            {
                Id = this.Id,
                ViewModelName = this.GetType().Name
            });
        }

        protected async Task SaveWithOptimisticConcurrencyAsync(Func<Task> saveAsync, Action afterSaveAction)
        {
            try
            {
                await saveAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var databaseValues = await ex.Entries.Single().GetDatabaseValuesAsync();

                if (databaseValues == null)
                {
                    MessageDialogService.ShowInfoDialog("The entity has been removed by another user");

                    RaiseDetailDeletedEvent(Id);
                    return;
                }


                var result = MessageDialogService.ShowOkCancelDialog("The entity has been changed in " +
                                                                     "the meantime by someone else. Click OK to save your changes anyway, Click Cancel " +
                                                                     "to reload the entity from the database",
                    "Question");

                if (result == MessageDialogResult.Ok)
                {
                    var entry = ex.Entries.Single();

                    entry.OriginalValues.SetValues(await entry.GetDatabaseValuesAsync());
                    await saveAsync();
                }
                else
                {
                    await ex.Entries.Single().ReloadAsync();

                    await LoadAsync(Id);
                }
            }

            afterSaveAction();
        }
    }
}