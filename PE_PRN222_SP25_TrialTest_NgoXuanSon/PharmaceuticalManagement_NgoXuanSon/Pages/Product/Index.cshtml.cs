using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Repository.Models;
using Service;

namespace PharmaceuticalManagement_NgoXuanSon.Pages.Product
{
	public class IndexModel : PageModel
	{

		[BindProperty(SupportsGet = true)]
		public string SearchTerm { get; set; } = default!;
		[BindProperty(SupportsGet = true)]
		public string OrderBy { get; set; } = default!;
		[BindProperty(SupportsGet = true)]
		public int PageIndex { get; set; } = 1;
		public int TotalPages { get; set; }
		public int PageSize { get; set; } = 3;
		private readonly MedicineInfomationService _medicineInformationService;

		public IndexModel(MedicineInfomationService medicineInformationService)
		{
			_medicineInformationService = medicineInformationService;
		}

		public IList<MedicineInformation> MedicineInformation { get; set; } = default!;

		public async Task OnGetAsync()
		{
			var result = await _medicineInformationService.GetAllMedicineInformationList(
				searchTerm: SearchTerm,
				pageIndex: PageIndex,
				pageSize: PageSize
			);
			PageIndex = result.PageIndex;
			TotalPages = result.TotalPages;
			MedicineInformation = result.MedicineInformation;
		}
	}
}
