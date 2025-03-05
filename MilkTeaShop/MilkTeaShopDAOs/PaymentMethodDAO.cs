using Microsoft.EntityFrameworkCore;
using MilkTeaShopBOs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkTeaShopDAOs
{
    public class PaymentMethodDAO
    {
        private readonly MilkTeaShopContext _context;

        private static PaymentMethodDAO instance = null;

        private PaymentMethodDAO()
        {
            _context = new MilkTeaShopContext();
        }

        public static PaymentMethodDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PaymentMethodDAO();
                }
                return instance;
            }
            private set { instance = value; }
        }

        public async Task<List<PaymentMethod>> GetPaymentMethodsList()
        {
            return await _context.PaymentMethods
                .Where(p => p.Status == true)
                .ToListAsync();
        }
    }
}
