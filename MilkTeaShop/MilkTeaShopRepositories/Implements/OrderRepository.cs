using MilkTeaShopBOs.DTOs;
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
    public class OrderRepository : IOrderRepository
    {

        public async Task<int> CreateOrder(OrderDTO orderDTO)
        {
            return await OrderDAO.Instance.CreateOrder(orderDTO);
        }
    }
}
