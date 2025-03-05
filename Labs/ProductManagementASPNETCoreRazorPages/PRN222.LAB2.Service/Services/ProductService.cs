using PRN222.LAB2.Repository.Models;
using PRN222.LAB2.Repository.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace PRN222.LAB2.Service.Services
{
	public class ProductService : IProductService
	{
		private readonly IUnitOfWork _unitOfWork;

		public ProductService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
		}

		public Task AddProductAsync(Product product)
		{
			if (product == null)
				throw new ArgumentNullException(nameof(product));

			_unitOfWork.Products.Add(product);
			_unitOfWork.Complete();
			return Task.CompletedTask;
		}

		public Task DeleteProductAsync(Product product)
		{
			if (product == null)
				throw new ArgumentNullException(nameof(product));

			_unitOfWork.Products.Delete(product);
			_unitOfWork.Complete();
			return Task.CompletedTask;
		}

		public async Task<Product> GetProductByIdAsync(int id)
		{
			Expression<Func<Product, object>>[] include = { p => p.Category };
			var product = await _unitOfWork.Products.GetById(id, include);
			return product;
		}

		public class PaintingResponse
		{
			public List<Product> Products { get; set; }
			public int TotalPages { get; set; }
			public int PageIndex { get; set; }
		}

		public async Task<PaintingResponse> GetProductsAsync(
			int pageIndex,
			int pageSize,
			string searchTerm,
			string orderBy)
		{
			Expression<Func<Product, object>>[] include = { p => p.Category };

			var products = await _unitOfWork.Products.Get(include);
			if (!string.IsNullOrEmpty(searchTerm))
			{
				products = products.Where(p => p.ProductName != null &&
								  p.ProductName.Contains(searchTerm ?? "", StringComparison.CurrentCultureIgnoreCase))
								 .ToList();
			}

			int count = products.Count();
			int totalPages = (int)Math.Ceiling(count / (double)pageSize);
			var pagedProducts = products.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

			return new PaintingResponse
			{
				Products = pagedProducts,
				TotalPages = totalPages,
				PageIndex = pageIndex
			};
		}

		public Task UpdateProductAsync(Product product)
		{
			if (product == null)
				throw new ArgumentNullException(nameof(product));

			_unitOfWork.Products.Update(product);
			_unitOfWork.Complete();
			return Task.CompletedTask;

		}
	}
}