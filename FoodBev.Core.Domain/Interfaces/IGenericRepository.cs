using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodBev.Core.Domain.Interfaces
{
    /// <summary>
    /// Defines a generic repository interface for standard CRUD operations on an entity.
    /// </summary>
    /// <typeparam name="T">The entity type this repository handles.</typeparam>
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        
        // Adds a Save method for the Unit of Work pattern (will be implemented in the DbContext)
        Task<int> SaveChangesAsync();
    }
}