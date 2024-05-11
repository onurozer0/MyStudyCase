using BFB.Core.Repositories;
using BFB.Core.Services;
using BFB.Core.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BFB.Service.Services
{
	public class Service<T> : IService<T> where T : class
	{
		private readonly IGenericRepository<T> _repository;
		private readonly IUnitOfWork _unitOfWork;

		public Service(IUnitOfWork unitOfWork, IGenericRepository<T> repository)
		{
			_unitOfWork = unitOfWork;
			_repository = repository;
		}

		public async Task<T> AddAsync(T entity)
		{
			await _repository.AddAsync(entity);
			await _unitOfWork.CommitAsync();
			return entity;
		}

		public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
		{
			await _repository.AddRangeAsync(entities);
			await _unitOfWork.CommitAsync();
			return entities;
		}

		public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression)
		{
			return await _repository.AnyAsync(expression);
		}

		public async Task<IEnumerable<T>> GetAllAsync()
		{
			return await _repository.GetAll().ToListAsync();
		}

		public async Task<T> GetByIdAsync(Guid id)
		{
			var isExist = await _repository.GetByIdAsync(id);
			return isExist;
		}

		public async Task RemoveAsync(T entity)
		{
			_repository.Remove(entity);
			await _unitOfWork.CommitAsync();
		}

		public async Task RemoveRangeAsync(IEnumerable<T> entities)
		{
			_repository.RemoveRange(entities);
			await _unitOfWork.CommitAsync();
		}

		public async Task UpdateAsync(T entity)
		{
			_repository.Update(entity);
			await _unitOfWork.CommitAsync();
		}

		public IQueryable<T> Where(Expression<Func<T, bool>> expression)
		{
			return _repository.Where(expression);

		}
	}
}
