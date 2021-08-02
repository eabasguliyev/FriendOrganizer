using System.Threading.Tasks;
using System.Windows.Input;
using FriendOrganizer.Model;
using FriendOrganizer.UI.Data;
using FriendOrganizer.UI.Events;
using Prism.Commands;
using Prism.Events;

namespace FriendOrganizer.UI.ViewModels
{
    public class FriendDetailViewModel:ViewModelBase, IFriendDetailViewModel
    {
        private readonly IFriendDataService _friendDataService;
        private readonly IEventAggregator _eventAggregator;
        private Friend _friend;

        public FriendDetailViewModel(IFriendDataService friendDataService, IEventAggregator eventAggregator)
        {
            _friendDataService = friendDataService;
            _eventAggregator = eventAggregator;

            _eventAggregator.GetEvent<OpenFriendDetailViewEvent>().Subscribe(OnOpenFriendDetailView);

            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
        }

        private async void OnSaveExecute()
        {
            await _friendDataService.SaveAsync(Friend);
            _eventAggregator.GetEvent<AfterFriendSavedEvent>().Publish(new AfterFriendSavedEventArgs()
            {
                Id = Friend.Id,
                DisplayMember = $"{Friend.FirstName} {Friend.LastName}"
            });
        }

        private bool OnSaveCanExecute()
        {
            // TODO: if friend is valid

            return true;
        }


        public Friend Friend
        {
            get => _friend;
            set
            {
                _friend = value;
                OnPropertyChanged();
            }
        }


        // Commands

        public ICommand SaveCommand { get; set; }

        private async void OnOpenFriendDetailView(int friendId)
        {
            await LoadAsync(friendId);
        }


        public async Task LoadAsync(int friendId)
        {
            Friend = await _friendDataService.GetByIdAsync(friendId);
        }
    }
}