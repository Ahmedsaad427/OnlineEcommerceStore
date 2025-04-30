using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.OrderModels
    {
    public class Order : BaseEntity<Guid>
        {
        public Order()
            {
            }

        public Order(string userEmail, Address shippingAddress, ICollection<OrderItem> orderItems, DeliveryMethod deliveryMethod, decimal subTotal, string paymentIntentId)
            {
            Id = Guid.NewGuid();
            UserEmail = userEmail;
            ShippingAddress = shippingAddress;
            OrderItems = orderItems;
            DeliveryMethod = deliveryMethod;
            SubTotal = subTotal;
            PaymentIntentId = paymentIntentId;
            }

        public string UserEmail { get; set; }

        public Address ShippingAddress { get; set; }


        // Order Items
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>(); //Navigational property


        //Delivery Method
        public DeliveryMethod DeliveryMethod { get; set; } //Navigational property
        public int? DeliveryMethodId { get; set; } //Foreign key property


        // Payment Status
        public OrderPaymentStatus paymentStatus { get; set; } = OrderPaymentStatus.Pending;


        // Subtotal
        public decimal SubTotal { get; set; }


        //Order Date
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;


        //Payment Intent Id
        public string PaymentIntentId { get; set; }


        }
    }
