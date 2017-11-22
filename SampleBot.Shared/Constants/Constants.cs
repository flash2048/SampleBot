using System.Collections.Generic;
using System.Globalization;

namespace SampleBot.Shared.Constants
{
    public static class Constants
    {
        public static string DefaultLocale = "ru-RU";


        private static Dictionary<string, string> Languages => new Dictionary<string, string>()
        {
            {"ru", "ru-RU"},
            {"ru-RU", "ru-RU"},
            {"en", "en-US"},
            {"en-US", "en-US"},
        };

        public static CultureInfo GetCulture(string locale)
        {
            if (locale == null)
                return new CultureInfo(DefaultLocale);

            string overridenLanguage = null;
            if (Languages.ContainsKey(locale))
            {
                overridenLanguage = Languages[locale];
            }

            var culture = new CultureInfo(overridenLanguage ?? locale);
            if (culture.Name != locale)
            {
                var datetimeCulture = new CultureInfo(locale);
                culture.DateTimeFormat = datetimeCulture.DateTimeFormat;
                culture.NumberFormat = datetimeCulture.NumberFormat;
            }
            return culture;
        }
    }
}
