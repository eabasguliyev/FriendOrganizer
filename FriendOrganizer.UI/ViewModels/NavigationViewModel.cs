using System.Collections.ObjectModel;
using System.Threading.Tasks;
using FriendOrganizer.Model;
using FriendOrganizer.UI.Data;

namespace FriendOrganizer.UI.ViewModels
{
    public class NavigationViewModel : INavigationViewModel
    {
        private readonly IFriendLookupDataService _friendLookupDataService;

        public NavigationViewModel(IFriendLookupDataService friendLookupDataService)
        {
            _friendLookupDataService = friendLookupDataService;
            Friends = new ObservableCollection<LookupItem>();
        }


        public ObservableCollection<LookupItem> Friends { get; set; }

        public async Task LoadAsync()
        {
            var items = await _friendLookupDataService.GetFriendLookupAsync();

            Friends.Clear();

            foreach (var item in items)
            {
                Friends.Add(item);
            }
        }
    }
}