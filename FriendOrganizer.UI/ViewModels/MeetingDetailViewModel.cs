using System;
using System.Threading.Tasks;
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

        public MeetingDetailViewModel(IMeetingRepository meetingRepository, IMessageDialogService messageDialogService,
            IEventAggregator eventAggregator) : base(eventAggregator)
        {
            _meetingRepository = meetingRepository;
            _messageDialogService = messageDialogService;
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

        public override async Task LoadAsync(int? meetingId)
        {
            var meeting = meetingId.HasValue
                ? await _meetingRepository.GetByIdAsync(meetingId.Value)
                : CreateNewMeeting();

            InitializeMeeting(meeting);
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
    }
}