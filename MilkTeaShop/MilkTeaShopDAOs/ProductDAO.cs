using Microsoft.EntityFrameworkCore;
using MilkTeaShopBOs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkTeaShopDAOs
{
    public class ProductDAO
    {
        private readonly MilkTeaShopContext _context;

        private static ProductDAO instance = null;

        private ProductDAO()
        {
            _context = new MilkTeaShopContext();
        }

        public static ProductDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ProductDAO();
                }
                return instance;
            }
            private set => instance = value;

        }

        public class PaintingResponse
        {
            public List<Product> Products { get; set; }
            public int TotalPages { get; set; }
            public int PageIndex { get; set; }
        }

        public async Task<PaintingResponse> GetProducts(string searchTerm, int pageIndex, int pageSize)
        {

            var query = _context.Products
                .Include(p => p.ProductVariants)
                .Include(p => p.ExtraProducts)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(p => p.ProductName.Contains(searchTerm));
            }

            int count = await query.CountAsync();
            int totalPages = (int)Math.Ceiling(count / (double)pageSize);
            query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            return new PaintingResponse
            {
                Products = await query.ToListAsync(),
                TotalPages = totalPages,
                PageIndex = pageIndex
            };
        }

        public async Task<Product> GetProductById(int id)
        {
            return await _context.Products
                .Include(p => p.ProductVariants)
                .Include(p => p.ExtraProducts)
                .FirstOrDefaultAsync(p => p.ProductId == id);
        }



    }

}