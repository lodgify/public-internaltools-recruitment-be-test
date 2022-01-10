using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SuperPanel.API.Infraestructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        #region .: Properties :.
        protected SuperPanelContext _context;
        protected DbSet<T> _table;
        #endregion .: Properties :.

        #region .: Constructor :.
        public GenericRepository(SuperPanelContext context)
        {
            _context = context;
            _table = _context.Set<T>();
        }

        #endregion .: Constructor :.

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var item = await _table.FindAsync(id);

                if (item == null)
                {
                    return false;
                }

                _context.Remove(item);

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception _e)
            {
                Console.WriteLine(_e); // Change to logger when logger enabled
                return false;
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _table.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _table.FindAsync(id);
        }

        public async Task<T> InsertAsync(T entity)
        {
            try
            {
                _table.Add(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }

            return entity;
        }

        public async Task<IEnumerable<T>> InsertRangeAsync(IEnumerable<T> entities)
        {
            try
            {
                _table.AddRange(entities);

                await _context.SaveChangesAsync();
                return entities;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            try
            {
                _table.Update(entity);

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex); // Change to logger when logger enabled
                return false;
            }
        }
    }
}
