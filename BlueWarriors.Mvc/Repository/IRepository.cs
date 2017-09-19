using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bluewarriors.Mvc.Repository
{
    public interface IRepository<T> where T: class
    {
        Task AddAsync(T newItem);
        Task<T> GetAsync(int itemId);
        Task<IEnumerable<T>> GetAsync();
        Task UpdateAsync(T itemUpdate);
    }
}