using MilkTeaShopBOs.Models;
using MilkTeaShopRepositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MilkTeaShopDAOs;

namespace MilkTeaShopRepositories.Implements
{
    public class ExtraProductRepository : IExtraProductRepository
    {

        Task<List<ExtraProduct>> IExtraProductRepository.GetExtraProductLists()
        {
            return ExtraProductDAO.Instance.GetExtraProductLists();
        }
    }
}
