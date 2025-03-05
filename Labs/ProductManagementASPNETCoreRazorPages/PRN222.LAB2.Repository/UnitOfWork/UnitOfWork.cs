using PRN222.LAB2.Repository.Models;
using PRN222.LAB2.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN222.LAB2.Repository.UnitOfWork
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly MyStoreContext _context;
		private IGenericRepository<Category> _categoryRepository;
		private IGenericRepository<Product> _productRepository;
		private IGenericRepository<AccountMember> _accountMemberRepository;

		public UnitOfWork(MyStoreContext context)
		{
			_context = context;
		}

		public IGenericRepository<Category> Categories =>
			_categoryRepository ??= new GenericRepository<Category>(_context);

		public IGenericRepository<Product> Products =>
			_productRepository ??= new GenericRepository<Product>(_context);

		public IGenericRepository<AccountMember> AccountMembers =>
			_accountMemberRepository ??= new GenericRepository<AccountMember>(_context);

		public int Complete()
		{
			return _context.SaveChanges();
		}

		public void Dispose()
		{
			_context.Dispose();
		}
	}
}
