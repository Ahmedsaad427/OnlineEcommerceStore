using Domain.Models.OrderModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared;
using Shared.ErrorModels;
using Shared.OrderModelsDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Presentation
    {
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrdersController(IServiceManager serviceManager) : ControllerBase
        {

        [HttpPost] // api/orders
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderResultDto))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetails))]
        public async Task<IActionResult> CreateOrders(OrderRequestDto request)
            {
            var email = User.FindFirstValue(ClaimTypes.Email);

            var result = await serviceManager.OrderService.CreateOrderAsync(request, email);
            return Ok(result);
            }



        [HttpGet] // api/orders/orders
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<OrderResultDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetails))]
        public async Task<IActionResult> GetOrders()
            {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var orders = await serviceManager.OrderService.GetOrdersByUserEmailAsync(email);
            return Ok(orders);
            }


        [HttpGet("{id}")] // api/orders/{id}
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderResultDto))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetails))]
        public async Task<IActionResult> GetOrderById(Guid id)
            {
            var order = await serviceManager.OrderService.GetOrderByIdAsync(id);
            return Ok(order);
            }



        [HttpGet("deliveryMethods")] // api/orders/deliveryMethods
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<DeliveryMethodDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetails))]
        public async Task<IActionResult> GetDeliveryMethods()
            {
            var deliveryMethods = await serviceManager.OrderService.GetAllDeliveryMethods();
            return Ok(deliveryMethods);
            }

        }
    }
