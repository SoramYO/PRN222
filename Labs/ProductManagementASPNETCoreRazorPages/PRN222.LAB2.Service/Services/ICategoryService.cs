using PRN222.LAB2.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PRN222.LAB2.Service.Services
{
	public interface ICategoryService
    {
		Task<List<Category>> GetCategoriesAsync();
	}
}
