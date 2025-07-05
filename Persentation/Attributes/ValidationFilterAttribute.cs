
using Core.Entities.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentaion.Attributes
{
    public class ValidationFilterAttribute : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            //throw new NotImplementedException();
          
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var action = context.RouteData.Values["action"];
            var controller = context.RouteData.Values["controller"];
            var param = context.ActionArguments
                .SingleOrDefault(x => x.Value?.ToString()?.Contains("Dto") == true).Value;

            if (param is null)
            {
                context.Result = new BadRequestObjectResult(new ResponseShape<object>(
                    StatusCode: 400,
                    message: "Object is null.",
                    errors: new Dictionary<string, string>
                    {
                { "Location", $"Controller: {controller}, Action: {action}" }
                    },
                    data: null
                ));
                return;
            }

            if (!context.ModelState.IsValid)
            {
                var errorDict = context.ModelState
                    .Where(kvp => kvp.Value.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.Errors.First().ErrorMessage // Just the first error per field
                    );

                context.Result = new UnprocessableEntityObjectResult(new ResponseShape<object>(
                    StatusCode: 422,
                    message: "Model validation failed.",
                    errors: errorDict,
                    data: null
                ));
            }
        }


        //context.Result = new UnprocessableEntityObjectResult(context.ModelState);
    }
}
    

