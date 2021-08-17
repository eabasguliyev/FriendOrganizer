using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using FriendOrganizer.Model;
using FriendOrganizer.UI.Data.Repositories;
using FriendOrganizer.UI.Views.Services;
using FriendOrganizer.UI.Wrappers;
using Prism.Commands;
using Prism.Events;

namespace FriendOrganizer.UI.ViewModels
{
    public class MeetingDetailViewModel:DetailViewModelBase, IMeetingDetailViewModel
    {
        private readonly IMeetingRepository _meetingRepository;
        private readonly IMessageDialogService _messageDialogService;
        private MeetingWrapper _meeting;
        private Friend _selectedAddedFriend;
        private Friend _selectedAvailableFriend;
        private List<Friend> _allFriends;

        public MeetingDetailViewModel(IMeetingRepository meetingRepository, IMessageDialogService messageDialogService,
            IEventAggregator eventAggregator) : base(eventAggregator)
        {
            _meetingRepository = meetingRepository;
            _messageDialogService = messageDialogService;

            AddedFriends = new ObservableCollection<Friend>();
            AvailableFriends = new ObservableCollection<Friend>();

            AddFriendCommand = new DelegateCommand(OnAddFriendExecute, OnAddFriendCanExecute);
            RemoveFriendCommand = new DelegateCommand(OnRemoveFriendExecute, OnRemoveFriendCanExecute);
        }

        public MeetingWrapper Meeting
        {
            get => _meeting;
            set
            {
                _meeting = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddFriendCommand { get; }
        public ICommand RemoveFriendCommand { get; }

        public ObservableCollection<Friend> AddedFriends { get; }
        public ObservableCollection<Friend> AvailableFriends { get; }


        public Friend SelectedAddedFriend
        {
            get => _selectedAddedFriend;
            set
            {
                _selectedAddedFriend = value;
                OnPropertyChanged();

                ((DelegateCommand)RemoveFriendCommand).RaiseCanExecuteChanged();
            }
        }


        public Friend SelectedAvailableFriend
        {
            get => _selectedAvailableFriend;
            set
            {
                _selectedAvailableFriend = value;
                OnPropertyChanged();
                ((DelegateCommand)AddFriendCommand).RaiseCanExecuteChanged();
            }
        }


        public override async Task LoadAsync(int? meetingId)
        {
            var meeting = meetingId.HasValue
                ? await _meetingRepository.GetByIdAsync(meetingId.Value)
                : CreateNewMeeting();

            InitializeMeeting(meeting);

            _allFriends = await _meetingRepository.GetAllFriendsAsync();
            SetupPicklist();
        }

        protected override async void OnDeleteExecute()
        {
            var result =
                _messageDialogService.ShowOkCancelDialog($"Do you really want to delete meeting {Meeting.Title}?",
                    "Question");

            if (result == MessageDialogResult.Cancel)
                return;

            _meetingRepository.Remove(Meeting.Model);
            await _meetingRepository.SaveAsync();

            RaiseDetailDeletedEvent(Meeting.Id);

        }

        protected override async void OnSaveExecute()
        {
            await _meetingRepository.SaveAsync();
            HasChanges = _meetingRepository.HasChanges();

            RaiseDetailSavedEvent(Meeting.Id, Meeting.Title);
        }

        protected override bool OnSaveCanExecute()
        {
            return Meeting != null && !Meeting.HasErrors && HasChanges;
        }

        private void InitializeMeeting(Meeting meeting)
        {
            Meeting = new MeetingWrapper(meeting);

            Meeting.PropertyChanged += (sender, args) =>
            {
                if (!HasChanges)
                    HasChanges = _meetingRepository.HasChanges();

                if (args.PropertyName == nameof(Meeting.HasErrors))
                    ((DelegateCommand) SaveCommand).RaiseCanExecuteChanged();
            };

            if (Meeting.Id == 0)
            {
                Meeting.Title = "";
                HasChanges = false;
            }

            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        private Meeting CreateNewMeeting()
        {
            var meeting =  new Meeting()
            {
                DateFrom = DateTime.Now,
                DateTo = DateTime.Now,
            };

            _meetingRepository.Add(meeting);

            return meeting;
        }

        private void SetupPicklist()
        {
            var meetingFriendIds = Meeting.Model.Friends.Select(f => f.Id).ToList();
            var addedFriends = _allFriends.Where(f => meetingFriendIds.Contains(f.Id)).OrderBy(f => f.FirstName);
            var availableFriends = _allFriends.Except(addedFriends).OrderBy(f => f.FirstName);


            AddedFriends.Clear();
            AvailableFriends.Clear();

            foreach (var addedFriend in addedFriends)
            {
                AddedFriends.Add(addedFriend);
            }

            foreach (var availableFriend in availableFriends)
            {
                AvailableFriends.Add(availableFriend);
            }
        }

        private void OnRemoveFriendExecute()
        {
            var friendToRemove = SelectedAddedFriend;

            Meeting.Model.Friends.Remove(friendToRemove);
            AddedFriends.Remove(friendToRemove);
            AvailableFriends.Add(friendToRemove);

            HasChanges = _meetingRepository.HasChanges();
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        private bool OnRemoveFriendCanExecute()
        {
            return SelectedAddedFriend != null;
        }

        private void OnAddFriendExecute()
        {
            var friendToAdd = SelectedAvailableFriend;

            Meeting.Model.Friends.Add(friendToAdd);
            AddedFriends.Add(friendToAdd);
            AvailableFriends.Remove(friendToAdd);

            HasChanges = _meetingRepository.HasChanges();
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        private bool OnAddFriendCanExecute()
        {
            return SelectedAvailableFriend != null;
        }
    }
}