using MilkTeaShopBOs.Models;
using MilkTeaShopDAOs;
using MilkTeaShopRepositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkTeaShopRepositories.Implements
{
    public class PaymentMethodRepository : IPaymentMethodRepository
    {
        public async Task<List<PaymentMethod>> GetAllPaymentMethods()
        {
            return await PaymentMethodDAO.Instance.GetPaymentMethodsList();
        }
    }
}
