using Domain.Exceptions;
using Microsoft.AspNetCore.Http;
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
    public class AuthController(IServiceManager serviceManager) : ControllerBase
        {

        // POST: api/auth/login
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserResultDto))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ValidationErrorResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationErrorResponse))]
        public async Task<IActionResult> Login(LoginDto loginDto)
            {
            var result = await serviceManager.AuthService.LoginAsync(loginDto);
            return Ok(result);
            }



        // POST: api/auth/register
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserResultDto))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ValidationErrorResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationErrorResponse))]
        public async Task<IActionResult> register(RegisterDto registerDto)
            {
            var result = await serviceManager.AuthService.RegisterAsync(registerDto);
            return Ok(result);
            }

        }
    }
