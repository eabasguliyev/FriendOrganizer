using System.Collections.ObjectModel;
using System.ComponentModel;
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
    public class ProgrammingLanguageDetailViewModel:DetailViewModelBase
    {
        private readonly IProgrammingLanguageRepository _programmingLanguageRepository;
        private ProgrammingLanguageWrapper _selectedProgrammingLanguage;

        public ProgrammingLanguageDetailViewModel(IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService,
            IProgrammingLanguageRepository programmingLanguageRepository) : base(eventAggregator, messageDialogService)
        {
            _programmingLanguageRepository = programmingLanguageRepository;

            Title = "Programming Languages";

            ProgrammingLanguages = new ObservableCollection<ProgrammingLanguageWrapper>();

            AddProgrammingLanguageCommand = new DelegateCommand(OnAddProgrammingLanguageExecute);
            RemoveProgrammingLanguageCommand = new DelegateCommand(OnRemoveProgrammingLanguageExecute, OnRemoveProgrammingLanguageCanExecute);
        }

        public ICommand AddProgrammingLanguageCommand { get; }
        public ICommand RemoveProgrammingLanguageCommand { get; }
        public ObservableCollection<ProgrammingLanguageWrapper> ProgrammingLanguages { get; }

        public ProgrammingLanguageWrapper SelectedProgrammingLanguage
        {
            get => _selectedProgrammingLanguage;
            set
            {
                _selectedProgrammingLanguage = value;
                OnPropertyChanged();

                ((DelegateCommand)RemoveProgrammingLanguageCommand).RaiseCanExecuteChanged();
            }
        }

        public override async Task LoadAsync(int id)
        {
            // TODO: load data here


            Id = id;

            foreach (var languageWrapper in ProgrammingLanguages)
            {
                languageWrapper.PropertyChanged -= Wrapper_OnPropertyChanged;
            }

            var programmingLanguages = await _programmingLanguageRepository.GetAllAsync();

            ProgrammingLanguages.Clear();

            foreach (var language in programmingLanguages)
            {
                var languageWrapper = new ProgrammingLanguageWrapper(language);
                languageWrapper.PropertyChanged += Wrapper_OnPropertyChanged;
                ProgrammingLanguages.Add(languageWrapper);
            }
        }

        private void Wrapper_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!HasChanges)
                HasChanges = _programmingLanguageRepository.HasChanges();

            if(e.PropertyName == nameof(ProgrammingLanguageWrapper.HasErrors))
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        protected override void OnDeleteExecute()
        {
            throw new System.NotImplementedException();
        }

        protected override async void OnSaveExecute()
        {
            await _programmingLanguageRepository.SaveAsync();

            HasChanges = _programmingLanguageRepository.HasChanges();

            RaiseCollectionSavedEvent();
        }

        protected override bool OnSaveCanExecute()
        {
            return HasChanges && ProgrammingLanguages.All(pl => !pl.HasErrors);
        }

        private void OnAddProgrammingLanguageExecute()
        {
            var newWrapper = new ProgrammingLanguageWrapper(new ProgrammingLanguage());

            newWrapper.PropertyChanged += Wrapper_OnPropertyChanged;

            _programmingLanguageRepository.Add(newWrapper.Model);

            ProgrammingLanguages.Add(newWrapper);

            newWrapper.Name = "";
            HasChanges = false;
        }

        private async void OnRemoveProgrammingLanguageExecute()
        {
            var isReferenced = await _programmingLanguageRepository.IsReferencedByFriendAsync(SelectedProgrammingLanguage.Id);

            if (isReferenced)
            {
                MessageDialogService.ShowInfoDialog($"The language {SelectedProgrammingLanguage.Name}"
                        + " can't be removed, as it is referenced by at least one friend.");

                return;
            }

            SelectedProgrammingLanguage.PropertyChanged -= Wrapper_OnPropertyChanged;

            _programmingLanguageRepository.Remove(SelectedProgrammingLanguage.Model);
            ProgrammingLanguages.Remove(SelectedProgrammingLanguage);
            SelectedProgrammingLanguage = null;

            HasChanges = _programmingLanguageRepository.HasChanges();
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        private bool OnRemoveProgrammingLanguageCanExecute()
        {
            return SelectedProgrammingLanguage != null;
        }
    }
}