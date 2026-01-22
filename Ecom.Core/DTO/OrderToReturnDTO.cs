using Ecom.Core.Entites.Order;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom.Core.DTO
{
    public record OrderToReturnDTO
    {
        public int Id { get; set; }
        public string BuyerEmail { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }
        public DateTime OrderData { get; set; }
        public ShippingAddress shippingAddress { get; set; }
        public string deliveryMethod { get; set; }
        public IReadOnlyList<OrderItemDTO> orderItems { get; set; }
        public string status { get; set; }
    }

    public record OrderItemDTO
    {
        public int ProductItemId { get; set; }
        public string MainImage { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quntity { get; set; }
    }
    public record UpdateOrderStatusDTO
    {
        public string Status { get; set; }
    }
}
