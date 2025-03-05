using Microsoft.AspNetCore.Mvc.RazorPages;
using MilkTeaShopBOs.Models;
using MilkTeaShopRepositories.Interface;
using Microsoft.AspNetCore.Mvc;
using MilkTeaShopBOs.DTOs;

public class CartModel : PageModel
{
    private readonly IPaymentMethodRepository _paymentMethodRepository;
    private readonly IOrderRepository _orderRepository;

    public List<PaymentMethod> PaymentMethods { get; set; } = new();

    public CartModel(IPaymentMethodRepository paymentMethodRepository,
                    IOrderRepository orderRepository)
    {
        _paymentMethodRepository = paymentMethodRepository;
        _orderRepository = orderRepository;
    }

    public async Task OnGetAsync()
    {
        PaymentMethods = await _paymentMethodRepository.GetAllPaymentMethods();
    }

    public async Task<IActionResult> OnPostCheckoutAsync([FromBody] OrderDTO orderDTO)
    {
        try
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            userId = 1;
            if (!userId.HasValue)
            {
                return new JsonResult(new { success = false, message = "Vui lòng đăng nhập để tiếp tục" });
            }

            orderDTO.CustomerId = userId.Value;
            var orderId = await _orderRepository.CreateOrder(orderDTO);

            return new JsonResult(new { success = true, orderId = orderId });
        }
        catch (Exception ex)
        {
            return new JsonResult(new { success = false, message = ex.Message });
        }
    }
}