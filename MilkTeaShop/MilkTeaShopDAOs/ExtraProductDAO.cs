using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MilkTeaShopBOs.Models;
namespace MilkTeaShopDAOs
{
    public class ExtraProductDAO
    {
        private readonly MilkTeaShopContext _context;

        private static ExtraProductDAO instance = null;

        private ExtraProductDAO()
        {
            _context = new MilkTeaShopContext();
        }

        public static ExtraProductDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ExtraProductDAO();
                }
                return instance;
            }
            private set => instance = value;
        }

        public async Task<List<ExtraProduct>> GetExtraProductLists()
        {
            return await _context.ExtraProducts
                .Where(e => e.Status == true)
                .ToListAsync();
        }
    }
}
