using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly Sp25PharmaceuticalDbContext _context;
		private IGenericRepository<Manufacturer> _manufacturerRepo;
		private IGenericRepository<MedicineInformation> _medicineInformationRepo;
		private IGenericRepository<StoreAccount> _storeAccountRepo;

		public UnitOfWork(Sp25PharmaceuticalDbContext context)
		{
			_context = context;
		}

		public IGenericRepository<Manufacturer> Manufacturerrepo => _manufacturerRepo ??= new GenericRepository<Manufacturer>(_context);

		public IGenericRepository<StoreAccount> StoreAccountRepo => _storeAccountRepo ??= new GenericRepository<StoreAccount>(_context);

		public IGenericRepository<MedicineInformation> MedicineInformationRepo => _medicineInformationRepo ??= new GenericRepository<MedicineInformation>(_context);

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
