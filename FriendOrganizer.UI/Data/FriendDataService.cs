using System.Collections;
using System.Collections.Generic;
using FriendOrganizer.Model;

namespace FriendOrganizer.UI.Data
{
    public class FriendDataService : IFriendDataService
    {
        public IEnumerable<Friend> GetAll()
        {
            yield return new Friend()
            {
                FirstName = "Thomas",
                LastName = "Huber"
            };

            yield return new Friend()
            {
                FirstName = "Andreas",
                LastName = "Boehler"
            };

            yield return new Friend()
            {
                FirstName = "Julia",
                LastName = "Huber"
            };

            yield return new Friend()
            {
                FirstName = "Chrissi",
                LastName = "Egin"
            };
        }
    }
}