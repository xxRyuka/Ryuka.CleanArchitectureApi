using System.Linq.Expressions;

namespace Ryuka.NlayerApi.Core.Abstractions;

public interface IRepository<T> where T : class
{
    Task<T> FindAsync(int id);
    
    Task CreateAsync(T entity);
    T Update(T entity);
    T Delete(T entity);
    
    Task<IReadOnlyList<T>> GetAllAsync(); // Readonly olusturduk
    
    IQueryable<T> Where(Expression<Func<T, bool>> predicate );
    IQueryable<T> Table { get; }
    
    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
    Task<int> CountAsync(Expression<Func<T, bool>> predicate);
    Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);

        
}