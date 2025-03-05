using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MilkTeaShopBOs.Models;
using MilkTeaShopRepositories.Interface;

namespace MilkTeaShopUI.Pages.Products
{
    public class DetailsModel : PageModel
    {
        private readonly IProductRepository _productRepository;
        private readonly IExtraProductRepository _extraProductRepository;

        public Product? Product { get; set; }
        public List<ExtraProduct> ExtraProducts { get; set; } = new List<ExtraProduct>();

        public DetailsModel(IProductRepository productRepository, IExtraProductRepository extraProductRepository)
        {
            _productRepository = productRepository;
            _extraProductRepository = extraProductRepository;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Product = await _productRepository.GetProductById(id);

            if (Product == null)
            {
                return NotFound();
            }

            ExtraProducts = await _extraProductRepository.GetExtraProductLists();
            return Page();
        }
    }
}
