using BOs;
using static DAOs.MedicineInformationDAO;

namespace Repos
{
	public interface IMedicineRepo
	{
		Task<List<MedicineInformation>> GetMedicineInformationAsync();

		Task<PaintingResponse> GetList(string searchTerm, int pageIndex, int pageSize);
		Task<MedicineInformation> GetMedicineInformationByIdAsync(string id);
		Task AddMedicineInformationAsync(MedicineInformation medicineInformation);
		Task UpdateMedicineInformationAsync(MedicineInformation medicineInformation);
		Task DeleteMedicineInformationAsync(string id);
	}
}
