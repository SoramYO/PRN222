using BOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repos;

namespace PharmaceuticalManagement_NgoXuanSon.Pages.PharmaceuticalManagementPage
{
	public class DeleteModel : PageModel
	{
		private readonly IMedicineRepo _medicineRepo;

		public DeleteModel(IMedicineRepo medicineRepo)
		{
			_medicineRepo = medicineRepo;
		}

		[BindProperty]
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

		public async Task<IActionResult> OnPostAsync(string id)
		{
			if (id == null)
			{
				return NotFound();
			}
			try
			{
				var medicineinformation = await _medicineRepo.GetMedicineInformationByIdAsync(id);

				if (medicineinformation != null)
				{
					MedicineInformation = medicineinformation;
					await _medicineRepo.DeleteMedicineInformationAsync(id);
					TempData["Message"] = "Medicine deleted successfully!";

				}

				return RedirectToPage("./Index");
			}
			catch (Exception ex)
			{
				TempData["Message"] = ex.Message;
				return await OnGetAsync(id);
			}


		}
	}
}
