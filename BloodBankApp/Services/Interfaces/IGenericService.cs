using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloodBankApp.Services.Interfaces
{
    public interface IGenericService<T>       
    {
        Task<List<T>> GetAll();
        Task<List<T>> GetByPageNumber(int pageNumber);
        Task<T> GetById(Guid id);
        Task Update(T entity);
        Task Create(T entity);
        Task Delete(Guid id);
    }
}
