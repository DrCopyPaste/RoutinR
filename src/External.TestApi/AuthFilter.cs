using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net.Http.Headers;
using System.Text;

namespace External.TestApi.Authentication
{
    public class AuthFilter : IEndpointFilter
    {
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(Constants.Api_User, out var extractedUser)) return TypedResults.Unauthorized();
            if (!context.HttpContext.Request.Headers.TryGetValue(Constants.Api_Key, out var extractedKey)) return TypedResults.Unauthorized();

            if (extractedUser != Constants.Api_User) return TypedResults.Unauthorized();
            if (extractedKey != Constants.Api_Key) return TypedResults.Unauthorized();

            return await next(context);
        }
    }
}
