using System.Threading.Tasks;
using FriendOrganizer.Model;
using FriendOrganizer.UI.Wrappers;

namespace FriendOrganizer.UI.Data.Repositories
{
    public interface IFriendRepository
    {
        Task<Friend> GetByIdAsync(int friendId);
        Task SaveAsync();
        bool HasChanges();
        void Add(Friend friend);
        void Remove(Friend friend);
        void RemovePhoneNumber(FriendPhoneNumber phoneNumber);
    }
}