using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Repository.Models;
using Service;

namespace PharmaceuticalManagement_NgoXuanSon.Pages.Product
{
	[Authorize(Roles = "2")]
	public class CreateModel : PageModel
    {
		private readonly MedicineInfomationService _medicineInformationService;

		public CreateModel(MedicineInfomationService context)
        {
			_medicineInformationService = context;
        }

        public async Task<IActionResult> OnGet()
        {
        ViewData["ManufacturerId"] = new SelectList(await _medicineInformationService.GetManufacturer(), "ManufacturerId", "ManufacturerName");
            return Page();
        }

        [BindProperty]
        public MedicineInformation MedicineInformation { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
			//validate Active Ingredient > 10 characters
			if (MedicineInformation.ActiveIngredients.Length < 10)
			{
				ModelState.AddModelError("MedicineInformation.ActiveIngredient", "Active Ingredient must be at least 10 characters long.");
				return Page();
			}


			_medicineInformationService.AddMedicineInformation(MedicineInformation);


			return RedirectToPage("./Index");
        }
    }
}
