﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace SuperPanel.API.Infraestructure.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();

        Task<T> GetByIdAsync(int id);

        Task<T> InsertAsync(T entity);
        Task<IEnumerable<T>> InsertRangeAsync(IEnumerable<T> entity);

        Task<bool> UpdateAsync(T entity);

        Task<bool> DeleteAsync(int id);
    }
}
