using System.Windows.Input;
using FriendOrganizer.UI.Events;
using Prism.Commands;
using Prism.Events;

namespace FriendOrganizer.UI.ViewModels
{
    public class NavigationItemViewModel:ViewModelBase
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly string _detailViewModelName;
        private string _displayMember;


        public NavigationItemViewModel(int id, string displayMember, IEventAggregator eventAggregator, string detailViewModelName)
        {
            Id = id;
            DisplayMember = displayMember;
            _eventAggregator = eventAggregator;
            _detailViewModelName = detailViewModelName;

            OpenDetailViewCommand = new DelegateCommand(OnOpenDetailViewExecute);
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

        public ICommand OpenDetailViewCommand { get; }


        private void OnOpenDetailViewExecute()
        {
            _eventAggregator.GetEvent<OpenDetailViewEvent>().Publish(new OpenDetailViewEventArgs()
            {
                Id = Id,
                ViewModelName = _detailViewModelName
            });
        }
    }
}