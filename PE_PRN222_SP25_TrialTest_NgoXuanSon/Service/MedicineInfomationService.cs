using Repository;
using Repository.Models;
using System.Linq.Expressions;

namespace Service
{
	public class MedicineInfomationService
	{
		private readonly IUnitOfWork _unitOfWork;

		public MedicineInfomationService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		public class PaintingResponse
		{
			public List<MedicineInformation> MedicineInformation { get; set; }
			public int TotalPages { get; set; }
			public int PageIndex { get; set; }
		}

		public async Task<MedicineInformation> GetMedicineInformationById(string id)
		{
			return await _unitOfWork.MedicineInformationRepo.GetById(id);
		}

		public async void AddMedicineInformation(MedicineInformation medicineInformation)
		{
			_unitOfWork.MedicineInformationRepo.Add(medicineInformation);
			_unitOfWork.Complete();
		}

		public async void UpdateMedicineInformation(MedicineInformation medicineInformation)
		{
			_unitOfWork.MedicineInformationRepo.Update(medicineInformation);
			_unitOfWork.Complete();
		}

		public async void DeleteMedicineInformation(string id)
		{
			var medicineInformation = await _unitOfWork.MedicineInformationRepo.GetById(id);
			_unitOfWork.MedicineInformationRepo.Delete(medicineInformation);
			_unitOfWork.Complete();
		}

		public Task<List<Manufacturer>> GetManufacturer()
		{
			return _unitOfWork.Manufacturerrepo.Get();

		}

		public async Task<PaintingResponse> GetAllMedicineInformationList(int pageIndex,
			int pageSize,
			string searchTerm)
		{
			Expression<Func<MedicineInformation, object>>[] include = { p => p.Manufacturer };
			var medicines = await _unitOfWork.MedicineInformationRepo.Get(include);
			if (!string.IsNullOrEmpty(searchTerm))
			{
				medicines = medicines.Where(m =>
					m.ActiveIngredients.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
					m.ExpirationDate.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
					m.WarningsAndPrecautions.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
					(m.Manufacturer != null && m.Manufacturer.ManufacturerName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
				).ToList();
			}
			int count = medicines.Count;
			int totalPages = (int)Math.Ceiling(count / (double)pageSize);
			var pagedProducts = medicines.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
			return new PaintingResponse
			{
				MedicineInformation = pagedProducts,
				TotalPages = totalPages,
				PageIndex = pageIndex
			};
		}

	}
}
