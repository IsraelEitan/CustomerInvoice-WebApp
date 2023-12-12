using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text;

namespace CustomerInvoice_WebApp.Utils
{
    public static class ErrorHandlingService
    {
        public static string CreateValidationErrorMessage(ModelStateDictionary modelState)
        {
            var errorMessage = new StringBuilder("Validation failed:");

            foreach (var entry in modelState)
            {
                foreach (var error in entry.Value.Errors)
                {
                    errorMessage.Append($" {entry.Key}: {error.ErrorMessage};");
                }
            }

            return errorMessage.ToString();
        }
    }
}
