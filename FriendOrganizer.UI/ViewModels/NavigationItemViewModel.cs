namespace FriendOrganizer.UI.ViewModels
{
    public class NavigationItemViewModel:ViewModelBase
    {
        private string _displayMember;


        public NavigationItemViewModel(int id, string displayMember)
        {
            Id = id;
            DisplayMember = displayMember;
        }

        public int Id { get; set; }

        public string DisplayMember
        {
            get => _displayMember;
            set
            {
                _displayMember = value;
                OnPropertyChanged();
            }
        }
    }
}