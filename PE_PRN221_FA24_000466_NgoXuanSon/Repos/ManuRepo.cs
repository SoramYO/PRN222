using BOs;
using DAOs;

namespace Repos
{
	public class ManuRepo : IManuRepo
	{
		public async Task<List<Manufacturer>> GetManufacturerList()
		{
			return await MedicineInformationDAO.Instance.GetManufacturer();
		}
	}
}
