using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MilkTeaShopDAOs.ProductDAO;
using MilkTeaShopBOs.Models;

namespace MilkTeaShopRepositories.Interface
{
    public interface IProductRepository
    {
        Task<PaintingResponse> GetProducts(string searchTerm, int pageIndex, int pageSize);

        Task<Product> GetProductById(int id);
    }
}
