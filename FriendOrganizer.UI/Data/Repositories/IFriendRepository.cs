using FriendOrganizer.Model;
using FriendOrganizer.UI.Wrappers;

namespace FriendOrganizer.UI.Data.Repositories
{
    public interface IFriendRepository:IGenericRepository<Friend>
    {
        void RemovePhoneNumber(FriendPhoneNumber phoneNumber);
    }
}