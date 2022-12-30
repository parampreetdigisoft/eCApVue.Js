using System.Globalization;
using System.Reflection;

namespace ECap.Web.Helper
{
    public static class LanguageHelperResource
    {
        public static string ToCulture(this string key, string currentCulture)
        {
            string result = string.Empty;
            try
            {
                var _currentCulture = CultureInfo.GetCultureInfo(currentCulture);
                result = new System.Resources.ResourceManager("ECap.Web.Resources.Resource", Assembly.GetExecutingAssembly()).GetString(key, _currentCulture);
            }
            catch (Exception ex)
            {

            }
            return !string.IsNullOrEmpty(result) ? result : key;
        }
    }


    
   
}
