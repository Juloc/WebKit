using Microsoft.AspNetCore.Mvc.ModelBinding;
using WebKit.Core.Validation;

namespace WebKit.Web.Validation;

public static class ModelStateValidation
{
    public static void AddErrors(this ModelStateDictionary modelState, ValidationResult validation)
    {
        foreach (ValidationError error in validation.Errors)
        {
            modelState.AddModelError(error.Field, error.Message);
        }
    }
}
