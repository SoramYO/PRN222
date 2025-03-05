using MilkTeaShopBOs.Models;
using MilkTeaShopDAOs;
using MilkTeaShopRepositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkTeaShopRepositories.Implements
{
    public class ProductRepository : IProductRepository
    {
        public async Task<Product> GetProductById(int id)
        {
            return await ProductDAO.Instance.GetProductById(id);
        }

        public async Task<ProductDAO.PaintingResponse> GetProducts(string searchTerm, int pageIndex, int pageSize)
        {
            return await ProductDAO.Instance.GetProducts(searchTerm, pageIndex, pageSize);
        }
    }
}
