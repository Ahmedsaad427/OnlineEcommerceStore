using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.OrderModelsDtos
    {
    public class OrderResultDto
        {

        public Guid Guid { get; set; }

        public string UserEmail { get; set; }

        public AddressDto ShippingAddress { get; set; }


        // Order Items
        public ICollection<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>(); //Navigational property


        //Delivery Method
        public string DeliveryMethod { get; set; }


        // Payment Status
        public string paymentStatus { get; set; }


        // Subtotal
        public decimal SubTotal { get; set; }


        //Order Date
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;


        //Payment Intent Id
        public string PaymentIntentId { get; set; } = string.Empty;

        public decimal Total { get; set; }

        }
    }
