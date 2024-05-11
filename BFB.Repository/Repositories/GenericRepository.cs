using BFB.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BFB.Repository.Repositories
{
	public class GenericRepository<T> : IGenericRepository<T> where T : class
	{

		protected readonly DbContext _appDbContext;
		private readonly DbSet<T> _dbSet;

		public GenericRepository(AppDbContext appDbContext)
		{
			_appDbContext = appDbContext;
			_dbSet = _appDbContext.Set<T>();
		}


		public async Task AddAsync(T entity)
		{
			await _dbSet.AddAsync(entity);
		}

		public async Task AddRangeAsync(IEnumerable<T> entities)
		{
			await _dbSet.AddRangeAsync(entities);
		}

		public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression)
		{
			return await _dbSet.AnyAsync(expression);
		}

		public IQueryable<T> GetAll(Expression<Func<T, bool>> expression)
		{
			return _dbSet.AsNoTracking().AsQueryable();
		}

		public IQueryable<T> GetAll()
		{
			return _dbSet.AsQueryable();
		}

		public void Remove(T entity)
		{
			_dbSet.Remove(entity);
		}

		public void RemoveRange(IEnumerable<T> entities)
		{
			_dbSet.RemoveRange(entities);
		}

		public void Update(T entity)
		{
			_dbSet.Update(entity);
		}

		public IQueryable<T> Where(Expression<Func<T, bool>> expression)
		{
			return _dbSet.Where(expression);
		}

		public async Task<T> GetByIdAsync(string id)
		{
			return await _dbSet.FindAsync(id);
		}

		public async Task<T> GetByIdAsync(Guid id)
		{
			return await _dbSet.FindAsync(id);
		}
		public async Task<int> CountAsync()
		{
			return await _dbSet.CountAsync();
		}
	}
}
