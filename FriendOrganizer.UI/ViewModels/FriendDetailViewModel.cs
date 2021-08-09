using System.Threading.Tasks;
using System.Windows.Input;
using FriendOrganizer.Model;
using FriendOrganizer.UI.Data;
using FriendOrganizer.UI.Data.Repositories;
using FriendOrganizer.UI.Events;
using FriendOrganizer.UI.Views.Services;
using FriendOrganizer.UI.Wrappers;
using Prism.Commands;
using Prism.Events;

namespace FriendOrganizer.UI.ViewModels
{
    public class FriendDetailViewModel:ViewModelBase, IFriendDetailViewModel
    {
        private readonly IFriendRepository _friendRepository;
        private readonly IEventAggregator _eventAggregator;
        private readonly IMessageDialogService _messageDialogService;
        private FriendWrapper _friend;
        private bool _hasChanges;

        public FriendDetailViewModel(IFriendRepository friendRepository, IEventAggregator eventAggregator, IMessageDialogService messageDialogService)
        {
            _friendRepository = friendRepository;
            _eventAggregator = eventAggregator;
            _messageDialogService = messageDialogService;

            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
            DeleteCommand = new DelegateCommand(OnDeleteExecute);
        }


        public async Task LoadAsync(int? friendId)
        {
            var friend = friendId.HasValue ? await _friendRepository.GetByIdAsync(friendId.Value) : CreateNewFriend();

            Friend = new FriendWrapper(friend);

            Friend.PropertyChanged += (s, e) =>
            {
                if (!HasChanges)
                {
                    HasChanges = _friendRepository.HasChanges();
                }

                if (e.PropertyName == nameof(Friend.HasErrors))
                {
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            };

            if (Friend.Id == 0)
            {
                // Little trick for trigger firstname validation

                Friend.FirstName = "";
                HasChanges = false;
            }

            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        private Friend CreateNewFriend()
        {
            var friend = new Friend();

            _friendRepository.Add(friend);

            return friend;
        }


        // Commands
        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }


        public FriendWrapper Friend
        {
            get => _friend;
            set
            {
                _friend = value;
                OnPropertyChanged();
            }
        }

        public bool HasChanges
        {
            get => _hasChanges;
            set
            {
                if (_hasChanges != value)
                {
                    _hasChanges = value;
                    //OnPropertyChanged(); // ??
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }

        private async void OnSaveExecute()
        {
            await _friendRepository.SaveAsync();
            
            HasChanges = _friendRepository.HasChanges();

            _eventAggregator.GetEvent<AfterFriendSavedEvent>().Publish(new AfterFriendSavedEventArgs()
            {
                Id = Friend.Id,
                DisplayMember = $"{Friend.FirstName} {Friend.LastName}"
            });
        }

        private bool OnSaveCanExecute()
        {
            return Friend != null && !Friend.HasErrors && HasChanges;
        }

        private async void OnDeleteExecute()
        {
            var result =
                _messageDialogService.ShowOkCancelDialog(
                    $"Do you really want to delete friend {Friend.FirstName} {Friend.LastName}", "Info");

            if (result == MessageDialogResult.Cancel)
                return;

            _friendRepository.Remove(Friend.Model);

            await _friendRepository.SaveAsync();

            _eventAggregator.GetEvent<AfterFriendDeletedEvent>().Publish(Friend.Id);
        }
    }
}