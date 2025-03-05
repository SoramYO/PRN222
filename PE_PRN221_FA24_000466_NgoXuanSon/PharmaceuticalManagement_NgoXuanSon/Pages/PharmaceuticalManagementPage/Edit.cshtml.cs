using BOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Repos;

namespace PharmaceuticalManagement_NgoXuanSon.Pages.PharmaceuticalManagementPage
{
	public class EditModel : PageModel
	{
		private readonly IMedicineRepo _medicineRepo;
		private readonly IManuRepo _manuRepo;

		public EditModel(IMedicineRepo medicineRepo, IManuRepo manuRepo)
		{
			_medicineRepo = medicineRepo;
			_manuRepo = manuRepo;
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
			MedicineInformation = medicineinformation;
			var selectListItems = await _manuRepo.GetManufacturerList();
			ViewData["ManufacturerId"] = new SelectList((System.Collections.IEnumerable)selectListItems, "ManufacturerId", "ManufacturerName");
			return Page();
		}

		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more information, see https://aka.ms/RazorPagesCRUD.
		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
			{
				return Page();
			}

			try
			{
				await _medicineRepo.UpdateMedicineInformationAsync(MedicineInformation);
				TempData["Message"] = "Medicine updated successfully!";
				return RedirectToPage("./Index");
			}
			catch (Exception ex)
			{

				TempData["Message"] = ex.Message;
				return Page();

			}
		}

		private bool MedicineInformationExists(string id)
		{
			return _medicineRepo.GetMedicineInformationByIdAsync(id) != null;
		}
	}
}
