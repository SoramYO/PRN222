using PRN222.Lab1.Repository.Models;
using PRN222.Lab1.Repository.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace PRN222.Lab1.Service.Services
{
	public class ProductService : IProductService
	{
		private readonly IUnitOfWork _unitOfWork;

		public ProductService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public void AddProduct(Product product)
		{
			_unitOfWork.Products.Add(product);
			_unitOfWork.Complete();
		}

		public void DeleteProduct(Product product)
		{
			_unitOfWork.Products.Delete(product);
			_unitOfWork.Complete();
		}

		public Product GetProductById(int id )
		{
			Expression<Func<Product, object>>[] includes = { p => p.Category };
			return _unitOfWork.Products.GetById(id, includes);
		}

		public List<Product> GetProducts()
		{
			Expression<Func<Product, object>>[] includes = { p => p.Category };
			return _unitOfWork.Products.GetAll(includes);
		}

		public void UpdateProduct(Product product)
		{
			_unitOfWork.Products.Update(product);
			_unitOfWork.Complete();
		}
	}
}
