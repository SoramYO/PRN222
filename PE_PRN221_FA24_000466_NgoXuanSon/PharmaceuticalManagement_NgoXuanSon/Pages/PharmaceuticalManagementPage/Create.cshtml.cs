using BOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Repos;

namespace PharmaceuticalManagement_NgoXuanSon.Pages.PharmaceuticalManagementPage
{
	public class CreateModel : PageModel
	{
		private readonly IMedicineRepo _medicineRepo;
		private readonly IManuRepo _manuRepo;

		public CreateModel(IMedicineRepo medicineRepo, IManuRepo manuRepo)
		{
			_medicineRepo = medicineRepo;
			_manuRepo = manuRepo;
		}

		public async Task<IActionResult> OnGet()
		{
			var selectListItems = await _manuRepo.GetManufacturerList();
			ViewData["ManufacturerId"] = new SelectList((System.Collections.IEnumerable)selectListItems, "ManufacturerId", "ManufacturerName");
			return Page();
		}

		[BindProperty]
		public MedicineInformation MedicineInformation { get; set; } = default!;

		// For more information, see https://aka.ms/RazorPagesCRUD.
		public async Task<IActionResult> OnPostAsync()
		{
			try
			{
				if (!ModelState.IsValid)
				{
					return Page();
				}

				await _medicineRepo.AddMedicineInformationAsync(MedicineInformation);
				TempData["Message"] = "Medicine added successfully!";

				return RedirectToPage("./Index");
			}
			catch (Exception ex)
			{
				TempData["Message"] = ex.Message;
				return await OnGet();
			}
		}
	}
}
