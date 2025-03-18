using Repository;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
	public class MedicineInformationService
	{
		private readonly IUnitOfWork _unitOfWork;
		public MedicineInformationService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public class PaintingResponse
		{
			public List<MedicineInformation> MedicineInfomation { get; set; }
			public int TotalPages { get; set; }
			public int PageIndex { get; set; }
		}

		public async Task<PaintingResponse> GetAllMedicineInfomationList(int pageIndex,
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
				MedicineInfomation = pagedProducts,
				TotalPages = totalPages,
				PageIndex = pageIndex
			};
		}

		public async Task<MedicineInformation> GetMedicineInformationById(string id)
		{
			Expression<Func<MedicineInformation, object>>[] include = { p => p.Manufacturer };

			var medicineInformation = await _unitOfWork.MedicineInformationRepo.GetById(id, include);
			return medicineInformation;
		}
		public Task<List<Manufacturer>> GetManufacturer()
		{
			var category = _unitOfWork.Manufacturerrepo.Get();
			return category;
		}
		public void AddMedicineInformation(MedicineInformation medicineInformation)
		{
			_unitOfWork.MedicineInformationRepo.Add(medicineInformation);
			_unitOfWork.Complete();
		}
		public void UpdateMedicineInformation(MedicineInformation medicineInformation)
		{
			_unitOfWork.MedicineInformationRepo.Update(medicineInformation);
			_unitOfWork.Complete();
		}
		public void DeleteMedicineInformation(MedicineInformation medicineInformation)
		{
			_unitOfWork.MedicineInformationRepo.Delete(medicineInformation);
			_unitOfWork.Complete();
		}

	}
}
