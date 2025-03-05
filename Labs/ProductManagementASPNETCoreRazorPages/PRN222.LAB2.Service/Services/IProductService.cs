using PRN222.LAB2.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static PRN222.LAB2.Service.Services.ProductService;

namespace PRN222.LAB2.Service.Services
{
	public interface IProductService
	{
		Task AddProductAsync(Product product);

		Task DeleteProductAsync(Product product);

		Task UpdateProductAsync(Product product);

		Task<PaintingResponse> GetProductsAsync(
			int pageIndex,
			int pageSize,
			Expression<Func<Product, bool>> filter = null,
			Func<IQueryable<Product>, IOrderedQueryable<Product>> orderBy = null
		);

		Task<Product> GetProductByIdAsync(int id);
	}
}
