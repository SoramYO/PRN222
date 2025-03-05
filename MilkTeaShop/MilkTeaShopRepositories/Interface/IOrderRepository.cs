using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MilkTeaShopBOs.DTOs;

namespace MilkTeaShopRepositories.Interface
{
    public interface IOrderRepository
    {
        Task<int> CreateOrder(OrderDTO orderDTO);
    }
}
