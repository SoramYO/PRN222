using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Repository.Models;
using Service;

namespace PharmaceuticalManagement_StudentName.Pages.Product
{
	[Authorize]
	public class DetailsModel : PageModel
    {
		private readonly MedicineInformationService _medicineInformationService;

		public DetailsModel(MedicineInformationService medicineInformationService)
        {
			_medicineInformationService = medicineInformationService;
        }

        public MedicineInformation MedicineInformation { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicineinformation = await _medicineInformationService.GetMedicineInformationById(id);
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
