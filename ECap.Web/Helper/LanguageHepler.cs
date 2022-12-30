namespace ECap.Web.Helper
{
    public class LanguageHepler : ILanguageHelper
    {
        public string GetResource(string key, string culture)
        {
            return key.ToCulture(culture);
        }
    }
}
