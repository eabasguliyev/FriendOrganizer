using System.Threading.Tasks;

namespace FriendOrganizer.UI.ViewModels
{
    public interface IDetailViewModel
    {
        Task LoadAsync(int? id);
        bool HasChanges { get; }
    }
}