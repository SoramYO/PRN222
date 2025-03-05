using PRN222.Lab1.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN222.Lab1.Service.Services
{
	public interface IProductService
	{
		void AddProduct(Product product);

		void DeleteProduct(Product product);

		void UpdateProduct(Product product);

		List<Product> GetProducts();

		Product GetProductById(int id);

	}
}
