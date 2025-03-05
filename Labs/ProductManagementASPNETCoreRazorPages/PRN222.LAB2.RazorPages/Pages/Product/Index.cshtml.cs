using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN222.LAB2.Repository.Models;
using PRN222.LAB2.Service.Services;

namespace PRN222.LAB2.RazorPages.Pages.Product
{
	[Authorize]
	public class IndexModel : PageModel
	{
		private readonly IProductService _productService;

		[BindProperty(SupportsGet = true)]
		public string SearchTerm { get; set; } = default!;
		[BindProperty(SupportsGet = true)]
		public string OrderBy { get; set; } = default!;

		public int PageIndex { get; set; } = 1;

		public int TotalPages { get; set; }
		public int PageSize { get; set; } = 10;
		public IndexModel(IProductService productService)
		{
			_productService = productService;
		}

		public IList<PRN222.LAB2.Repository.Models.Product> Product { get; set; } = default!;

		public async Task<IActionResult> OnGetAsync(int pageIndex = 1)
		{
			// if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Account")))
			// {
			// 	var result = await _productService.GetProductsAsync
			// 	(
			// 		filter: !string.IsNullOrEmpty(SearchTerm) ? p => p.ProductName.Contains(SearchTerm) : null,
			// 		orderBy: null,
			// 		pageIndex: pageIndex,
			// 		pageSize: PageSize
			// 	);
			// 	PageIndex = result.PageIndex;
			// 	TotalPages = result.TotalPages;
			// 	Product = result.Products;
			// 	return Page();
			// }
			// return RedirectToPage("/Login");
			var result = await _productService.GetProductsAsync(
				searchTerm: SearchTerm,
				orderBy: OrderBy,
				pageIndex: pageIndex,
				pageSize: PageSize
			);
			PageIndex = result.PageIndex;
			TotalPages = result.TotalPages;
			Product = result.Products;
			return Page();
		}
	}
}
