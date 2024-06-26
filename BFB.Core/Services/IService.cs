﻿using System.Linq.Expressions;

namespace BFB.Core.Services
{
	public interface IService<T> where T : class
	{
		Task<T> GetByIdAsync(Guid id);
		IQueryable<T> Where(Expression<Func<T, bool>> expression);
		Task<T> AddAsync(T entity);
		Task<bool> AnyAsync(Expression<Func<T, bool>> expression);
		Task UpdateAsync(T entity);
		Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities);
		Task RemoveAsync(T entity);


		Task RemoveRangeAsync(IEnumerable<T> entities);
		Task<IEnumerable<T>> GetAllAsync();
	}
}
