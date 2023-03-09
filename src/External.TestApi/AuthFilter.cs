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

            if (extractedUser != "user") return TypedResults.Unauthorized();
            if (extractedKey != "key") return TypedResults.Unauthorized();

            return await next(context);
        }
    }
}
