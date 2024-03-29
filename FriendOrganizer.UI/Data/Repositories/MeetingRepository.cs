﻿using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using FriendOrganizer.DataAccess;
using FriendOrganizer.Model;

namespace FriendOrganizer.UI.Data.Repositories
{
    public class MeetingRepository:GenericRepository<Meeting, FriendOrganizerDbContext>, IMeetingRepository
    {
        public MeetingRepository(FriendOrganizerDbContext context) : base(context)
        {
        }

        public override async Task<Meeting> GetByIdAsync(int id)
        {
            return await Context.Meetings.Include(m => m.Friends)
                .SingleAsync(m => m.Id == id);
        }

        public async Task<List<Friend>> GetAllFriendsAsync()
        {
            return await Context.Friends.ToListAsync();
        }

        public async Task ReloadFriendAsync(int friendId)
        {
            var changeTrackerEntity =
                Context.ChangeTracker.Entries<Friend>().SingleOrDefault(e => e.Entity.Id == friendId);

            if (changeTrackerEntity != null)
                await changeTrackerEntity.ReloadAsync();
        }
    }
}