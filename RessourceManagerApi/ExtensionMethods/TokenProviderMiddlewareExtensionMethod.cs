using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using RessourceManagerApi.TokenProvider;

namespace RessourceManagerApi.ExtensionMethods
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseTokenProvider(
            this IApplicationBuilder builder, TokenProviderOptions parameters)
        {
            return builder.UseMiddleware<TokenProviderMiddleware>(Options.Create(parameters));
        }
    }
}
