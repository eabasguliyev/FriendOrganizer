using System.Collections.Generic;
using System.Threading.Tasks;
using FriendOrganizer.Model;

namespace FriendOrganizer.UI.Data.Repositories
{
    public interface IProgrammingLanguageRepository : IGenericRepository<ProgrammingLanguage>
    {
        Task<List<ProgrammingLanguage>> GetAllAsync();
        Task<bool> IsReferencedByFriendAsync(int programmingLanguageId);
    }
}