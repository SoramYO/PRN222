using BOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repos;

namespace PharmaceuticalManagement_NgoXuanSon.Pages.PharmaceuticalManagementPage
{
	public class DetailsModel : PageModel
	{
		private readonly IMedicineRepo _medicineRepo;

		public DetailsModel(IMedicineRepo medicineRepo)
		{
			_medicineRepo = medicineRepo;
		}

		public MedicineInformation MedicineInformation { get; set; } = default!;

		public async Task<IActionResult> OnGetAsync(string id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var medicineinformation = await _medicineRepo.GetMedicineInformationByIdAsync(id);
			if (medicineinformation == null)
			{
				return NotFound();
			}
			else
			{
				MedicineInformation = medicineinformation;
			}
			return Page();
		}
	}
}
