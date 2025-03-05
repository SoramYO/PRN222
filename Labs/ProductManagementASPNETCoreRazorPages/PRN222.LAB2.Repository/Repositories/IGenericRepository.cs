using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PRN222.LAB2.Repository.Repositories
{
	public interface IGenericRepository<T> where T : class
	{
		Task<List<T>> Get(params Expression<Func<T, object>>[] includes);
		Task<T> GetById(object id, params Expression<Func<T, object>>[] includes);
		void Add(T entity);
		void Update(T entity);
		void Delete(T entity);
	}
}