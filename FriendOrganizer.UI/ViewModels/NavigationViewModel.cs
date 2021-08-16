using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using FriendOrganizer.UI.Data;
using FriendOrganizer.UI.Data.Lookups;
using FriendOrganizer.UI.Events;
using Prism.Events;

namespace FriendOrganizer.UI.ViewModels
{
    public class NavigationViewModel : ViewModelBase, INavigationViewModel
    {
        private readonly IFriendLookupDataService _friendLookupDataService;
        private readonly IEventAggregator _eventAggregator;

        public NavigationViewModel(IFriendLookupDataService friendLookupDataService, IEventAggregator eventAggregator)
        {
            _friendLookupDataService = friendLookupDataService;
            _eventAggregator = eventAggregator;

            _eventAggregator.GetEvent<AfterDetailSavedEvent>().Subscribe(AfterFriendSaved);
            
            Friends = new ObservableCollection<NavigationItemViewModel>();

            _eventAggregator.GetEvent<AfterDetailDeletedEvent>().Subscribe(AfterFriendDeleted);
        }


        public ObservableCollection<NavigationItemViewModel> Friends { get; set; }

        public async Task LoadAsync()
        {
            var items = await _friendLookupDataService.GetFriendLookupAsync();

            Friends.Clear();

            foreach (var item in items)
            {
                Friends.Add(new NavigationItemViewModel(item.Id, item.DisplayMember, _eventAggregator, nameof(FriendDetailViewModel)));
            }
        }

        private void AfterFriendSaved(AfterDetailSavedEventArgs args)
        {
            switch (args.ViewModelName)
            {
                case nameof(FriendDetailViewModel):
                {
                    var lookupItem = Friends.SingleOrDefault(li => li.Id == args.Id);

                    if (lookupItem == null)
                    {
                        Friends.Add(new NavigationItemViewModel(args.Id, args.DisplayMember, _eventAggregator, nameof(FriendDetailViewModel)));
                    }
                    else
                    {
                        lookupItem.DisplayMember = args.DisplayMember;
                    }
                }break;
            }
        }

        private void AfterFriendDeleted(AfterDetailDeletedEventArgs args)
        {
            switch (args.ViewModelName)
            {
                case nameof(FriendDetailViewModel):
                {
                    var friend = Friends.SingleOrDefault(f => f.Id == args.Id);

                    if (friend != null)
                    {
                        Friends.Remove(friend);
                    }
                } break;
            }
        }
    }
}