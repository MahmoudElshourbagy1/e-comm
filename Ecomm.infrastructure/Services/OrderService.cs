using AutoMapper;
using Ecom.Core.DTO;
using Ecom.Core.Entites.Order;
using Ecom.Core.Interfaces;
using Ecom.Core.Services;
using Ecomm.infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Stripe;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecomm.infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IPaymentService _paymentService;
        public OrderService(IUnitOfWork unitOfWork, AppDbContext context, IMapper mapper, IPaymentService paymentService)
        {
            _unitOfWork = unitOfWork;
            _context = context;
            _mapper = mapper;
            _paymentService = paymentService;
        }

        public async Task<Orders> CreateOrdersAsync(OrderDTO orderDTO, string BuyerEmail)
        {
            var basket = await _unitOfWork.customerBasket.GetBasketAsync(orderDTO.basketId);
            List<OrderItem> orderItems = new List<OrderItem>();
            foreach (var item in basket.basketItems) {
                var Product = await _unitOfWork.productRepositry.GetByIdAsync(item.Id);
                var orderItem = new OrderItem(Product.Id,item.Image,Product.Name,item.Price,item.Quantity);
                orderItems.Add(orderItem);
            }
            var deliverMethod=await _context.DeliveryMethods.FirstOrDefaultAsync(m=>m.Id==orderDTO.DeliveryMethodId);
            var subTotal = orderItems.Sum(m=>m.Price * m.Quntity);
            var ship = _mapper.Map<ShippingAddress>(orderDTO.shipAddress);
            var ExisitOrder = await _context.Orders.Where(m => m.PaymentIntentId == basket.PaymentIntentId).FirstOrDefaultAsync();
            if (ExisitOrder is not null)
            {
                _context.Orders.Remove(ExisitOrder);
                await _paymentService.CreateOrUpdatePaymentAsync(basket.PaymentIntentId, deliverMethod.Id);
            }
            var order = new Orders(BuyerEmail, subTotal, ship, deliverMethod, orderItems ,basket.PaymentIntentId);
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            await _unitOfWork.customerBasket.DeleteBasketAsync(orderDTO.basketId);
            return order;
        }

        public async Task<IReadOnlyList<OrderToReturnDTO>> GetAllOrdersForUserAsync(string BuyerEmail)
        {
            var orders = await _context.Orders.Where(m => m.BuyerEmail == BuyerEmail)
                .Include(i => i.orderItems).Include(i => i.deliveryMethod).
                ToListAsync();
            var result = _mapper.Map<IReadOnlyList<OrderToReturnDTO>>(orders);
            return result;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodAsync()
            => await _context.DeliveryMethods.AsNoTracking().ToListAsync();

        public async Task<OrderToReturnDTO> GetOrderByIdAsync(int Id, string BuyerEmail)
        {
            var order = await _context.Orders.Where(m=>m.Id==Id&&m.BuyerEmail==BuyerEmail)
                .Include(i=>i.orderItems).Include(i=>i.deliveryMethod)
                .FirstOrDefaultAsync();
            var result = _mapper.Map<OrderToReturnDTO>(order);
            return result;
        }
    }
}
