using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PRN222.Lab1.Repository.Repositories
{
	public class GenericRepository<T> : IGenericRepository<T> where T : class
	{
		protected readonly MyStoreContext _context;
		protected readonly DbSet<T> _dbSet;

		public GenericRepository(MyStoreContext context)
		{
			_context = context;
			_dbSet = context.Set<T>();
		}

		#region Get all
		public List<T> GetAll(params Expression<Func<T, object>>[] includes)
		{
			try
			{
				IQueryable<T> query = _dbSet;
				foreach (var include in includes)
				{
					query = query.Include(include);
				}
				return query.ToList();
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
		#endregion

		#region Find
		public List<T> Find(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] includes)
		{
			try
			{
				IQueryable<T> query = _dbSet.Where(expression);
				foreach (var include in includes)
				{
					query = query.Include(include);
				}
				return query.ToList();
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
		#endregion

		#region Get by id
		public T GetById(object id, params Expression<Func<T, object>>[] includes)
		{
			try
			{
				IQueryable<T> query = _dbSet;
				foreach (var include in includes)
				{
					query = query.Include(include);
				}
				return query.FirstOrDefault();


			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
		#endregion

		#region Add, Update, Delete
		public void Add(T entity)
		{
			try
			{
				_dbSet.Add(entity);
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public void Update(T entity)
		{
			try
			{
				_dbSet.Update(entity);
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public void Delete(T entity)
		{
			try
			{
				_dbSet.Remove(entity);
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
		#endregion
	}
}
