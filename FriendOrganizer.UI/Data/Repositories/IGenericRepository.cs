using System.Data.Entity;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.Data.Repositories
{
    public interface IGenericRepository<TEntity>
    {
        Task<TEntity> GetByIdAsync(int id);
        Task SaveAsync();
        bool HasChanges();
        void Add(TEntity entity);
        void Remove(TEntity entity);
    }
}