using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN222.LAB2.Repository.Models;
using PRN222.LAB2.Service.Services;

namespace PRN222.LAB2.RazorPages.Pages.Product
{
    public class DetailsModel : PageModel
    {
		private readonly IProductService _productService;

		public DetailsModel(IProductService productService)
        {
			_productService = productService;
        }

        public PRN222.LAB2.Repository.Models.Product Product { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productService.GetProductByIdAsync(id.Value);
			if (product == null)
            {
                return NotFound();
            }
            else
            {
                Product = product;
            }
            return Page();
        }
    }
}
