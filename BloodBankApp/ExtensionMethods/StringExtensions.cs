using System.Globalization;

namespace BloodBankApp.ExtensionMethods
{
    public static class StringExtensions
    {
        public static string ToTitleCase(this string title)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(title.ToLower());
        }
    }
}
