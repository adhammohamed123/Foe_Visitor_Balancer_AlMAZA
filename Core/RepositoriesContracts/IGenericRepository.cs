using Core.Contracts;
using System.Linq.Expressions;

namespace Core.RepositoryContracts
{
    public interface IGenericRepository<T> where T :class,ISoftDeletedModel
    {
         IQueryable<T>  FindAll(bool trackchanges);
         IQueryable<T> FindByCondition(Expression<Func<T,bool>> expression ,bool trackchanges);

        Task Create(T entity);
        void Update(T entity);
        void SoftDelete(T entity);
        void HardDelete(T entity);

    }

}
