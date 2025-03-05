using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MilkTeaShopBOs.Models;
using MilkTeaShopRepositories.Interface;

namespace MilkTeaShopUI.Pages
{
    public class ProductsModel : PageModel
    {
        private readonly IProductRepository _productRepository;
        private const int PageSize = 9;

        public List<Product> Products { get; set; } = new();
        public int PageIndex { get; set; } = 1;
        public int TotalPages { get; set; }
        public string SearchTerm { get; set; } = "";

        public ProductsModel(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task OnGetAsync(string searchTerm, int pageIndex = 1)
        {
            if (pageIndex < 1) pageIndex = 1;
            PageIndex = pageIndex;
            SearchTerm = searchTerm ?? "";

            var response = await _productRepository.GetProducts(SearchTerm, PageIndex, PageSize);
            Products = response.Products;
            TotalPages = response.TotalPages;
        }
    }
}
