using BOs;
using DAOs;

namespace Repos
{
	public class MedicineRepo : IMedicineRepo
	{
		public Task AddMedicineInformationAsync(MedicineInformation medicineInformation)
		{
			return MedicineInformationDAO.Instance.Insert(medicineInformation);
		}

		public Task<MedicineInformationDAO.PaintingResponse> GetList(string searchTerm, int pageIndex, int pageSize)
		{
			return MedicineInformationDAO.Instance.GetList(searchTerm, pageIndex, pageSize);
		}

		Task IMedicineRepo.DeleteMedicineInformationAsync(string id)
		{
			return MedicineInformationDAO.Instance.Delete(id);
		}

		Task<List<MedicineInformation>> IMedicineRepo.GetMedicineInformationAsync()
		{
			return MedicineInformationDAO.Instance.GetList();
		}

		Task<MedicineInformation> IMedicineRepo.GetMedicineInformationByIdAsync(string id)
		{
			return MedicineInformationDAO.Instance.GetById(id);
		}

		Task IMedicineRepo.UpdateMedicineInformationAsync(MedicineInformation medicineInformation)
		{
			return MedicineInformationDAO.Instance.Update(medicineInformation);
		}
	}
}
