using System.Globalization;

namespace TBCTask.API.Middlewares;

public class CultureMiddleware
{
    private readonly RequestDelegate _next;

    public CultureMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        string cultureName = null;

        if (context.Request.Query.ContainsKey("culture"))
        {
            cultureName = context.Request.Query["culture"];
        }

        if (string.IsNullOrEmpty(cultureName) && context.Request.Headers.ContainsKey("Accept-Language"))
        {
            var acceptLanguageHeader = context.Request.Headers["Accept-Language"].ToString();
            var language = acceptLanguageHeader.Split(',').FirstOrDefault()?.Trim().ToLowerInvariant();

            if (!string.IsNullOrEmpty(language))
            {
                if (language.Contains("-"))
                {
                    var parts = language.Split('-');
                    cultureName = $"{parts[0]}-{parts[1].ToUpper()}";
                }
                else
                {
                    cultureName = language;
                }
            }
        }

        if (!string.IsNullOrEmpty(cultureName))
        {
            var culture = CultureInfo.GetCultureInfo(cultureName);
            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;
        }

        await _next(context);
    }
}