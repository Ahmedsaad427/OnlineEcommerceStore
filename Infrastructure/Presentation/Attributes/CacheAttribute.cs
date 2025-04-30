using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Attributes
    {
    public class CacheAttribute(int durationInSeconds) : Attribute, IAsyncActionFilter
        {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<IServiceManager>().CacheService;
            var cacheKey = GenerateCacheKey(context.HttpContext.Request);
            var result = await cacheService.GetCacheValueAsync(cacheKey);

            if ( !string.IsNullOrEmpty(result) )
                {
                context.Result = new ContentResult()
                    {
                    ContentType = "application/json",
                    StatusCode = StatusCodes.Status200OK,
                    Content = result
                    };
                return;
                }

            var contextResult = await next.Invoke();
            if ( contextResult.Result is OkObjectResult okObject )
                {
                await cacheService.SetCacheValueAsync(cacheKey, okObject.Value, TimeSpan.FromSeconds(durationInSeconds));
                }
            }



        private string GenerateCacheKey(HttpRequest request)
            {
            var cacheKey = new StringBuilder();

            cacheKey.Append(request.Path);
            foreach ( var item in request.Query.OrderBy(q => q.Key) )
                {
                cacheKey.Append($"|{item.Key}-{item.Value}");

                }
            return cacheKey.ToString();

            }

        }
    }
