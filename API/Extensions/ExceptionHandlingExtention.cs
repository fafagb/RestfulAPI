using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace API.Extensions
{
    public static class ExceptionHandlingExtention
    {
        public static void UseExceptionHandler(this IApplicationBuilder app, ILoggerFactory loggerFactory)
        {

            app.UseExceptionHandler(builder =>
            {
                builder.Run(async context =>
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    context.Response.ContentType = "application/json";
                    var ex = context.Features.Get<IExceptionHandlerFeature>();
                    if (ex != null)
                    {
                        var logger = loggerFactory.CreateLogger("API.Extentions.ExceptionHandlingExtention");
                        logger.LogError(500, ex.Error, ex.Error.Message);
                    }
                    await context.Response.WriteAsync(ex?.Error?.Message ?? "An Error Occurred");

                });
               
            });




        }
    }
}
