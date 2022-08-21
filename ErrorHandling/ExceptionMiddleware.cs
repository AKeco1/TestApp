using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using TestProjectWthAngular.Models.ErrorModels;

namespace TestProjectWthAngular.ErrorHandling
{
    public static class ExceptionMiddleware
    {
        public static void ConfigExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(error =>
            {
                error.Run(async c =>
                {
                    c.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    c.Response.ContentType = "application/json";

                    var contextFeature = c.Features.Get<IExceptionHandlerFeature>();

                    if (contextFeature != null)
                    {
                        await c.Response.WriteAsync(new ErrorDetails()
                        {
                            StatusCode = c.Response.StatusCode,
                            ErrorMessage = "Internal Server Error"
                        }.ToString());
                    }
                });
            });
        }
    }
}
