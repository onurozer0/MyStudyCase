using System.Linq.Expressions;

namespace BFB.Core.Repositories
{
	public interface IGenericRepository<T> where T : class
	{
		Task<T> GetByIdAsync(string id);
		Task<T> GetByIdAsync(Guid id);
		IQueryable<T> Where(Expression<Func<T, bool>> expression);
		Task AddAsync(T entity);
		Task<bool> AnyAsync(Expression<Func<T, bool>> expression);
		void Update(T entity);
		Task AddRangeAsync(IEnumerable<T> entities);
		void Remove(T entity);


		void RemoveRange(IEnumerable<T> entities);
		IQueryable<T> GetAll(Expression<Func<T, bool>> expression);
		IQueryable<T> GetAll();
		Task<int> CountAsync();
	}
}
