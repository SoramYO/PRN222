using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PRN222.Lab1.Repository.Repositories
{
	public interface IGenericRepository<T> where T : class
	{
		List<T> GetAll(params Expression<Func<T, object>>[] includes);
		List<T> Find(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] includes);
		T GetById(object id , params Expression<Func<T, object>>[] includes);
		void Add(T entity);
		void Update(T entity);
		void Delete(T entity);
	}
}
