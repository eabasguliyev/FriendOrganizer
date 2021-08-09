﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using FriendOrganizer.DataAccess;
using FriendOrganizer.Model;

namespace FriendOrganizer.UI.Data.Lookups
{
    public class LookupDataService : IFriendLookupDataService, IProgrammingLanguageLookupDataService
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

        public async Task<List<LookupItem>> GetProgrammingLanguageLookupAsync()
        {
            using (var context = _contextCreator())
            {
                return await context.ProgrammingLanguages.AsNoTracking().Select(pl =>
                    new LookupItem()
                    {
                        Id = pl.Id,
                        DisplayMember = pl.Name
                    }).ToListAsync();
            }
        }
    }
}