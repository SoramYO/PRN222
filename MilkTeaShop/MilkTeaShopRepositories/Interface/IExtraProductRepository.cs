using MilkTeaShopBOs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkTeaShopRepositories.Interface
{
    public interface IExtraProductRepository
    {
        Task<List<ExtraProduct>> GetExtraProductLists();
    }
}
