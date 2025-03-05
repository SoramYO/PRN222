using PRN222.LAB2.Repository.Models;
using PRN222.LAB2.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN222.LAB2.Repository.UnitOfWork
{
	public interface IUnitOfWork : IDisposable
	{
		IGenericRepository<Category> Categories { get; }
		IGenericRepository<Product> Products { get; }
		IGenericRepository<AccountMember> AccountMembers { get; }

		int Complete();
	}
}
