using Domain.Exceptions;
using Shared.ErrorModels;

namespace Store.G03.API.Middlewares
    {
    public class GlobalErrorHandlingMiddleware
        {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalErrorHandlingMiddleware> _loggger;

        public GlobalErrorHandlingMiddleware(RequestDelegate next, ILogger<GlobalErrorHandlingMiddleware> loggger)
            {
            _next = next;
            _loggger = loggger;
            }

        public async Task InvokeAsync(HttpContext context)
            {
            try
                {
                await _next.Invoke(context);

                // check if the response status code is 404
                if ( context.Response.StatusCode == StatusCodes.Status404NotFound )
                    {
                    await HandleNotFoundResponseAsync(context);
                    }
                }
            catch ( Exception ex )
                {
                // log the exception
                _loggger.LogError(ex, ex.Message);

                await HandleExceptionAsync(context, ex);

                }
            }

        private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
            {

            // set the response content type
            context.Response.ContentType = "application/json";


            // set the response body
            var response = new ErrorDetails()
                {
                ErrorMessage = ex.Message,
                };


            // set the response status code
            response.StatusCode = ex switch
                {
                    NotFoundException => StatusCodes.Status404NotFound,
                    BadRequestException => StatusCodes.Status400BadRequest,
                    UnAuthorizedException => StatusCodes.Status401Unauthorized,
                    ValidationException => HandleValidationExceptionAsync((ValidationException) ex, response),

                    _ => StatusCodes.Status500InternalServerError
                    };
            context.Response.StatusCode = response.StatusCode;


            // return the response
            await context.Response.WriteAsJsonAsync(response);
            }

        private static async Task HandleNotFoundResponseAsync(HttpContext context)
            {
            context.Response.ContentType = "application/json";
            var response = new ErrorDetails()
                {
                StatusCode = StatusCodes.Status404NotFound,
                ErrorMessage = $"This URL: {context.Request.Path} was not found",
                };
            await context.Response.WriteAsJsonAsync(response);
            }



        private static int HandleValidationExceptionAsync(ValidationException exception, ErrorDetails errorDetails)
            {

            errorDetails.Errors = exception.Errors;

            return StatusCodes.Status400BadRequest;

            }

        }
    }
