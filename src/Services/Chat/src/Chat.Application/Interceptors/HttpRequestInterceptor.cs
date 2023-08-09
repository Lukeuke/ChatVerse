using HotChocolate.AspNetCore;
using HotChocolate.Execution;
using Microsoft.AspNetCore.Http;
using JwtValidator = Chat.Domain.Helpers.Authorization.JwtValidator;

namespace Chat.Application.Interceptors;

public class HttpRequestInterceptor : DefaultHttpRequestInterceptor
{
    public override async ValueTask OnCreateAsync(
        HttpContext context,
        IRequestExecutor requestExecutor,
        IQueryRequestBuilder requestBuilder,
        CancellationToken cancellationToken)
    {
        if (context.Request.Headers.TryGetValue("Authorization", out var authHeader))
        {
            var jwt = authHeader.ToString().Split(" ")[1];

            if (!JwtValidator.Validate(jwt))
            {
                context.Response.StatusCode = 401;
                context.Response.Headers.Clear();
                await context.Response.WriteAsync("Unauthorized", cancellationToken);
            }
        }
        else
        {
            context.Response.StatusCode = 401;
            context.Response.Headers.Clear();
            await context.Response.WriteAsync("Unauthorized", cancellationToken);
        }

        await base.OnCreateAsync(context, requestExecutor, requestBuilder, cancellationToken);
    }
}