using PRN222.Lab1.Repository.Models;
using PRN222.Lab1.Repository.Repositories;
using PRN222.Lab1.Repository.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN222.Lab1.Service.Services
{
	public class CategoryService : ICatergoryService
	{
		private readonly IUnitOfWork _unitOfWork;

		public CategoryService(IUnitOfWork unitOfWork )
		{
			_unitOfWork = unitOfWork;
		}
		public List<Category> GetCategories()
		{
			return _unitOfWork.Categories.GetAll();

		}
	}
}
