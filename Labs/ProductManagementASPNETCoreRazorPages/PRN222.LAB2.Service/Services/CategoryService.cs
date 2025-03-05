using PRN222.LAB2.Repository.Models;
using PRN222.LAB2.Repository.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PRN222.LAB2.Service.Services
{
public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork _unitOfWork;

    public CategoryService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public Task<List<Category>> GetCategoriesAsync()
    {
        var category = _unitOfWork.Categories.Get();
        return category;
    }
}
}
