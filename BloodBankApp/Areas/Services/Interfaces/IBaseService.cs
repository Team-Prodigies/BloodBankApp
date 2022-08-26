using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BloodBankApp.Areas.Services.Interfaces
{
    public interface IBaseService<T>
    {
        Task<List<T>> GetAll();
        Task<List<T>> GetByCondition(Expression<Func<T, bool>> expression);
        Task<T> FindById(Guid id);
        Task Add(T entity);
        Task Update(T entity);
        Task Delete(T entity);
    }
}
