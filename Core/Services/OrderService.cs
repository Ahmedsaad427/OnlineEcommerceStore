using AutoMapper;
using Domain.Contracts;
using Domain.Exceptions;
using Domain.Models;
using Domain.Models.OrderModels;
using Microsoft.AspNetCore.Http.HttpResults;
using Services.Abstractions;
using Services.Specifications;
using Shared.OrderModelsDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
    {
    public class OrderService(IMapper mapper, IBasketRepository basketRepository, IUnitOfWork unitOfWork) : IOrderService
        {
        #region CreateOrder

        public async Task<OrderResultDto> CreateOrderAsync(OrderRequestDto orderRequest, string userEmail)
            {

            var address = mapper.Map<Address>(orderRequest.ShipToAddress);

            var basket = await basketRepository.GetBasketAsync(orderRequest.BasketId);
            if ( basket is null ) throw new BasketNotFoundException(orderRequest.BasketId);

            var orderItems = new List<OrderItem>();
            foreach ( var item in basket.Items )
                {
                var productItem = await unitOfWork.GetRepository<Product, int>().GetAsync(item.Id);
                if ( productItem is null ) throw new ProductNotFoundException(item.Id);

                var orderItem = new OrderItem(new ProductInOrderItem(productItem.Id, productItem.Name, productItem.PictureUrl), item.Quantity, productItem.Price);

                orderItems.Add(orderItem);
                }

            var deliveryMethod = await unitOfWork.GetRepository<DeliveryMethod, int>().GetAsync(orderRequest.DeliveryMethodId);
            if ( deliveryMethod is null ) throw new DeliveryMethodNotFoundException(orderRequest.DeliveryMethodId);

            var subTotal = orderItems.Sum(item => item.Price * item.Quantity);

            var Order = new Order(userEmail, address, orderItems, deliveryMethod, subTotal, "");

            await unitOfWork.GetRepository<Order, Guid>().AddAsync(Order);
            var count = await unitOfWork.SaveChangesAsync();
            if ( count == 0 ) throw new OrderCreateBadRequest();

            var orderResult = mapper.Map<OrderResultDto>(Order);
            return orderResult;

            }

        #endregion

        #region GetDeliveryMethods

        public async Task<IEnumerable<DeliveryMethodDto>> GetAllDeliveryMethods()
            {
            var deliveryMethods = await unitOfWork.GetRepository<DeliveryMethod, int>().GetAllAsync();
            var result = mapper.Map<IEnumerable<DeliveryMethodDto>>(deliveryMethods);
            return result;
            }

        #endregion

        #region GetOrderByID

        public async Task<OrderResultDto> GetOrderByIdAsync(Guid id)
            {

            var spec = new OrderSpecifications(id);
            var order = await unitOfWork.GetRepository<Order, Guid>().GetAsync(spec);
            if ( order is null ) throw new OrderNotFoundException(id);

            var result = mapper.Map<OrderResultDto>(order);

            return result;

            }

        #endregion

        #region GetAllOrdersByUserEmail

        public async Task<IEnumerable<OrderResultDto>> GetOrdersByUserEmailAsync(string userEmail)
            {
            var spec = new OrderSpecifications(userEmail);
            var order = await unitOfWork.GetRepository<Order, Guid>().GetAllAsync(spec);

            var result = mapper.Map<IEnumerable<OrderResultDto>>(order);

            return result;
            }

        #endregion

        }
    }
