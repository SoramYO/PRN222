using System.ComponentModel.DataAnnotations;
using PRN222.LAB2.Repository.Models;

namespace PRN222.LAB2.RazorPages.Models
{
	public class ProductModel
	{
		public int ProductId { get; set; }

		[Required(ErrorMessage = "Product name is required")]
		[StringLength(40, ErrorMessage = "Product name cannot exceed 40 characters")]
		public string ProductName { get; set; }

		[Required(ErrorMessage = "Category is required")]
		[Display(Name = "Category")]
		public int CategoryId { get; set; }

		[Display(Name = "Units In Stock")]
		[Range(0, 32767, ErrorMessage = "Stock must be between 0 and 32,767")]
		public short? UnitsInStock { get; set; }

		[Display(Name = "Unit Price")]
		[Range(0.01, 9999.99, ErrorMessage = "Price must be between 0.01 and 9,999.99")]
		[DataType(DataType.Currency)]
		public decimal? UnitPrice { get; set; }

		// Method to convert to Repository model
		public Product ToRepositoryModel()
		{
			return new Product
			{
				ProductId = ProductId,
				ProductName = ProductName,
				CategoryId = CategoryId,
				UnitsInStock = UnitsInStock,
				UnitPrice = UnitPrice
			};
		}

		// Method to create from Repository model
		public static ProductModel FromRepositoryModel(Product product)
		{
			if (product == null) return null;

			return new ProductModel
			{
				ProductId = product.ProductId,
				ProductName = product.ProductName,
				CategoryId = product.CategoryId,
				UnitsInStock = product.UnitsInStock,
				UnitPrice = product.UnitPrice
			};
		}
	}

	public class ProductResponseModel
	{
		public int ProductId { get; set; }
		public string ProductName { get; set; }
		public int CategoryId { get; set; }
		public string CategoryName { get; set; }
		public short? UnitsInStock { get; set; }
		public decimal? UnitPrice { get; set; }
		public string FormattedPrice => UnitPrice.HasValue ? $"${UnitPrice.Value:F2}" : "N/A";

		// Method to create from Repository model
		public static ProductResponseModel FromRepositoryModel(Product product)
		{
			if (product == null) return null;

			return new ProductResponseModel
			{
				ProductId = product.ProductId,
				ProductName = product.ProductName,
				CategoryId = product.CategoryId,
				CategoryName = product.Category?.CategoryName,
				UnitsInStock = product.UnitsInStock,
				UnitPrice = product.UnitPrice
			};
		}

		// Method to create list from Repository models
		public static List<ProductResponseModel> FromRepositoryModelList(List<Product> products)
		{
			return products?.Select(FromRepositoryModel).ToList() ?? new List<ProductResponseModel>();
		}
	}

	public class ProductListResponse
	{
		public List<ProductResponseModel> Products { get; set; } = new List<ProductResponseModel>();
		public int TotalPages { get; set; }
		public int CurrentPage { get; set; }
		public string SearchTerm { get; set; }
		public int TotalCount { get; set; }
	}
}
