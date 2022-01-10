using System.Collections.Generic;
using System.Threading.Tasks;

namespace SuperPanel.API.Service
{
    public interface IGenericService<V> where V : class
    {
        Task<IEnumerable<V>> GetAllAsync();

        Task<V> GetByIdAsync(int id);


        Task<V> InsertAsync(V entityDto);

        Task<bool> UpdateAsync(V entityDto);

        Task<bool> DeleteAsync(int id);
    }
}
