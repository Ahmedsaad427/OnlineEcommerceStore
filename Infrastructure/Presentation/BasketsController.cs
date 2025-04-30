using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared;
using Shared.ErrorModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation
    {
    [ApiController]
    [Route("api/[controller]")]
    public class BasketsController(IServiceManager serviceManager) : ControllerBase
        {


        // GET: api/baskets?id=id
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BasketDto))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetails))]
        [Authorize]

        public async Task<IActionResult> GetBasketById(string id)
            {

            var basket = await serviceManager.BasketService.GetBasketAsync(id);

            return Ok(basket);
            }


        // POST: api/baskets
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BasketDto))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetails))]
        [Authorize]
        public async Task<IActionResult> UpdateBasket(BasketDto basketDto)
            {
            var basket = await serviceManager.BasketService.UpdateBasketAsync(basketDto);
            return Ok(basket);
            }


        // DELETE: api/baskets?id=id
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(NoContent))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetails))]
        [Authorize]

        public async Task<IActionResult> DeleteBasket(string id)
            {
            await serviceManager.BasketService.DeleteBasketAsync(id);
            return NoContent();
            }

        }
    }
