using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MilkTeaShopBOs.DTOs;
using MilkTeaShopBOs.Models;
namespace MilkTeaShopDAOs
{
    public class OrderDAO
    {
        private readonly MilkTeaShopContext _context;

        private static OrderDAO instance;
        private OrderDAO()
        {
            _context = new MilkTeaShopContext();
        }
        public static OrderDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new OrderDAO();
                }
                return instance;
            }
            private set => instance = value;
        }

        public async Task<int> CreateOrder(OrderDTO orderDTO)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Create Order
                var order = new Order
                {
                    CustomerId = orderDTO.CustomerId,
                    OrderDate = DateTime.Now,
                    TotalAmount = orderDTO.TotalAmount,
                    Status = "Pending"
                };
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();
                // Create OrderDetails
                foreach (var detail in orderDTO.OrderDetails)
                {
                    var orderDetail = new OrderDetail
                    {
                        OrderId = order.OrderId,
                        ProductId = detail.ProductId,
                        Quantity = detail.Quantity,
                        UnitPrice = detail.UnitPrice
                    };
                    _context.OrderDetails.Add(orderDetail);
                    await _context.SaveChangesAsync();
                    // Create OrderExtraProducts
                    foreach (var extra in detail.ExtraProducts)
                    {
                        var orderExtraProduct = new OrderExtraProduct
                        {
                            OrderDetailId = orderDetail.OrderDetailId,
                            ExtraProductId = extra.ExtraProductId,
                            Quantity = extra.Quantity
                        };
                        _context.OrderExtraProducts.Add(orderExtraProduct);
                    }
                }
                // Create Payment
                var payment = new Payment
                {
                    OrderId = order.OrderId,
                    PaymentMethodId = orderDTO.PaymentMethodId,
                    PaymentDate = DateTime.Now,
                    Status = "Pending"
                };
                _context.Payments.Add(payment);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return order.OrderId;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return -1;
            }
        }

    }
}
