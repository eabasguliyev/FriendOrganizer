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
        private readonly IMeetingLookupDataService _meetingLookupDataService;
        private readonly IEventAggregator _eventAggregator;

        public NavigationViewModel(IFriendLookupDataService friendLookupDataService, IMeetingLookupDataService meetingLookupDataService, IEventAggregator eventAggregator)
        {
            _friendLookupDataService = friendLookupDataService;
            _meetingLookupDataService = meetingLookupDataService;
            _eventAggregator = eventAggregator;

            _eventAggregator.GetEvent<AfterDetailSavedEvent>().Subscribe(AfterDetailSaved);
            
            Friends = new ObservableCollection<NavigationItemViewModel>();
            Meetings = new ObservableCollection<NavigationItemViewModel>();

            _eventAggregator.GetEvent<AfterDetailDeletedEvent>().Subscribe(AfterFriendDeleted);
        }


        public ObservableCollection<NavigationItemViewModel> Friends { get; }
        public ObservableCollection<NavigationItemViewModel> Meetings { get; }

        public async Task LoadAsync()
        {
            var items = await _friendLookupDataService.GetFriendLookupAsync();

            Friends.Clear();

            foreach (var item in items)
            {
                Friends.Add(new NavigationItemViewModel(item.Id, item.DisplayMember, _eventAggregator, nameof(FriendDetailViewModel)));
            }

            items = await _meetingLookupDataService.GetMeetingLookupAsync();

            Meetings.Clear();
            
            foreach (var item in items)
            {
                Meetings.Add(new NavigationItemViewModel(item.Id, item.DisplayMember, _eventAggregator, nameof(MeetingDetailViewModel)));
            }
        }

        private void AfterDetailSaved(AfterDetailSavedEventArgs args)
        {
            switch (args.ViewModelName)
            {
                case nameof(FriendDetailViewModel):
                {
                    AfterDetailSaved(Friends, args);
                }break;
                case nameof(MeetingDetailViewModel):
                {
                    AfterDetailSaved(Meetings, args);
                }break;
            }
        }

        private void AfterDetailSaved(ObservableCollection<NavigationItemViewModel> items, AfterDetailSavedEventArgs args)
        {
            var lookupItem = items.SingleOrDefault(li => li.Id == args.Id);

            if (lookupItem == null)
            {
                items.Add(new NavigationItemViewModel(args.Id, args.DisplayMember, _eventAggregator, args.ViewModelName));
            }
            else
            {
                lookupItem.DisplayMember = args.DisplayMember;
            }
        }

        private void AfterFriendDeleted(AfterDetailDeletedEventArgs args)
        {
            switch (args.ViewModelName)
            {
                case nameof(FriendDetailViewModel):
                {
                    AfterFriendDeleted(Friends, args);
                } break;
                case nameof(MeetingDetailViewModel):
                {
                    AfterFriendDeleted(Meetings, args);
                } break;
            }
        }

        private void AfterFriendDeleted(ObservableCollection<NavigationItemViewModel> items,
            AfterDetailDeletedEventArgs args)
        {
            var lookupItem = items.SingleOrDefault(li => li.Id == args.Id);

            if (lookupItem != null)
            {
                items.Remove(lookupItem);
            }
        }
    }
}