using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;

namespace ECap.Web.Helper
{
    public static class ModelStateHelper
    {
        public static string ErrorsToJsonResult(this ModelStateDictionary modelState)
        {
            IEnumerable<KeyValuePair<string, string[]>> errors = modelState.IsValid
                ? null
                : modelState
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray())
                    .Where(m => m.Value.Any());
            if (errors != null)
            {
                return JsonConvert.SerializeObject(errors);
            }
            return "[]";
        }
    }
}
