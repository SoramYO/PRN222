
using Repository.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
	public interface IUnitOfWork : IDisposable
	{
		IGenericRepository<StoreAccount> StoreAccountRepo { get; }
		IGenericRepository<MedicineInformation> MedicineInformationRepo { get; }
		IGenericRepository<Manufacturer> Manufacturerrepo { get; }

		int Complete();
	}
}
