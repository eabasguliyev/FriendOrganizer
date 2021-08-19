using System.Windows;
using FriendOrganizer.UI.ViewModels;
using MahApps.Metro.Controls;

namespace FriendOrganizer.UI
{
    public partial class MainWindow : MetroWindow
    {
        private readonly MainViewModel _viewModel;

        public MainWindow(MainViewModel viewModel)
        {
            InitializeComponent();

            _viewModel = viewModel;

            this.DataContext = viewModel;

            Loaded += OnLoaded;
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            await _viewModel.LoadAsync();
        }
    }
}