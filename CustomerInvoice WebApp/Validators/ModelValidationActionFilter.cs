using CustomerInvoice_WebApp.Exceptions;
using CustomerInvoice_WebApp.Utils;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CustomerInvoice_WebApp.Validators
{
    public class ModelValidationActionFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errorDetails = ErrorHandlingService.CreateValidationErrorMessage(context.ModelState);
                throw new InvalidInputException(errorDetails);
            }

        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
