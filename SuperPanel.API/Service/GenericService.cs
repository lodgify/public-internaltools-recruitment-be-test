using Mapster;
using SuperPanel.API.Infraestructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperPanel.API.Service
{
    public class GenericService<T, V> : IGenericService<V> where T : class where V : class
    {
        private readonly IGenericRepository<T> _genericRepository;

        public GenericService(IGenericRepository<T> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                return await _genericRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex); // Change to logger when logger enabled
                return false;
            }
        }

        public async Task<IEnumerable<V>> GetAllAsync()
        {
            try
            {
                var entities = await _genericRepository.GetAllAsync();
                var entitiesDto = entities.Adapt<IEnumerable<V>>();

                return entitiesDto;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex); // Change to logger when logger enabled
                return null;
            }
        }

        public async Task<V> GetByIdAsync(int id)
        {
            try
            {
                var entity = await _genericRepository.GetByIdAsync(id);
                var entityDto = entity.Adapt<V>();

                return entityDto;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex); // Change to logger when logger enabled
                return null;
            }
        }

        public async Task<V> InsertAsync(V entityDto)
        {
            try
            {
                var entity = entityDto.Adapt<T>();
                entity = await _genericRepository.InsertAsync(entity);
                entityDto = entity.Adapt<V>();

                return entityDto;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex); // Change to logger when logger enabled
                return null;
            }
        }

        public async Task<bool> UpdateAsync(V entityDto)
        {
            try
            {
                var entity = entityDto.Adapt<T>();
                return await _genericRepository.UpdateAsync(entity);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex); // Change to logger when logger enabled
                return false;
            }
        }
    }
}
