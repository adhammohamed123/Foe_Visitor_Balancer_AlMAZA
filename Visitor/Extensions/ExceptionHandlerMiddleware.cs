using Core.Contracts;
using Core.Entities.Response;
using Core.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
namespace FOE.Maintainance.Extensions
{
    public static class ExceptionHandlerMiddleware
    {

        public static void HandleExceptions(this WebApplication app)
            => app.ExceptionsMiddleware(app.Services.GetRequiredService<ILoggerManager>());

         static void ExceptionsMiddleware(this WebApplication app,ILoggerManager logger)
            => app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    var errorOccured = context.Features.Get<IExceptionHandlerFeature>();
                    if(errorOccured!=null)
                    {
                        logger.LogError($"Something went wrong: {errorOccured.Error}");
                        var statuscode = errorOccured.Error switch
                        {
                            NotFoundException => StatusCodes.Status404NotFound,
                            BadRequestException=> StatusCodes.Status400BadRequest,
                            _ => StatusCodes.Status500InternalServerError
                        };
                        context.Response.StatusCode = statuscode;
                            context.Response.ContentType = "application/json";
                        // if(errorOccured.Error.InnerException is not null)
                        //  await context.Response.WriteAsJsonAsync(new Error(statuscode, errorOccured.Error.InnerException.Message));
                        // else
                        //await   context.Response.WriteAsJsonAsync(new Error(statuscode,errorOccured.Error.Message));
                        await context.Response.WriteAsJsonAsync(new ResponseShape<object>(
                            StatusCode: statuscode,
                            message: errorOccured.Error.Message,
                            errors: new Dictionary<string, string>
                            {
                             { "1", errorOccured.Error.Message }
                            },
                            data: null
                        ));

                    }
                });
            });
    } 
}
