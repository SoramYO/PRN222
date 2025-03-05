using Microsoft.EntityFrameworkCore;
using PRN222.LAB2.Repository.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PRN222.LAB2.Repository.Repositories
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

		#region Get All
		public async Task<List<T>> Get(params Expression<Func<T, object>>[] includes)
		{
			try
			{
				IQueryable<T> query = _dbSet;

				foreach (var include in includes)
				{
					query = query.Include(include);
				}

				return await query.ToListAsync();
			}
			catch (Exception ex)
			{
				throw new Exception($"Error retrieving data: {ex.Message}", ex);
			}
		}
		#endregion

		#region Get by Id
		public async Task<T?> GetById(object id, params Expression<Func<T, object>>[] includes)
		{
			IQueryable<T> query = _dbSet;
			foreach (var include in includes)
			{
				query = query.Include(include);
			}

			var keyProperty = typeof(T).GetProperties().FirstOrDefault(p => p.GetCustomAttributes(typeof(KeyAttribute), true).Any())
							  ?? typeof(T).GetProperties().FirstOrDefault(p => p.Name.Equals("ProductId", StringComparison.OrdinalIgnoreCase));


			return await query.AsNoTracking().FirstOrDefaultAsync(e => EF.Property<object>(e, keyProperty.Name).Equals(id));
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
				throw new Exception($"Error adding entity: {ex.Message}", ex);
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
				throw new Exception($"Error updating entity: {ex.Message}", ex);
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
				throw new Exception($"Error deleting entity: {ex.Message}", ex);
			}
		}
		#endregion
	}
}