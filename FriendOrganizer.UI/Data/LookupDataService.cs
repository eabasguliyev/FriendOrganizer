using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using FriendOrganizer.DataAccess;
using FriendOrganizer.Model;

namespace FriendOrganizer.UI.Data
{
    public class LookupDataService : IFriendLookupDataService
    {
        private readonly Func<FriendOrganizerDbContext> _contextCreator;

        public LookupDataService(Func<FriendOrganizerDbContext> contextCreator)
        {
            _contextCreator = contextCreator;
        }

        public async Task<List<LookupItem>> GetFriendLookupAsync()
        {
            using (var context = _contextCreator())
            {
                return await context.Friends.AsNoTracking().Select(f =>
                    new LookupItem()
                    {
                        Id = f.Id,
                        DisplayMember = f.FirstName + " " + f.LastName
                    }).ToListAsync();
            }
        }
    }
}