using System.Configuration;
using System.Dynamic;

namespace TeachMe.Helpers.Settings
{
    public static class ApplicationSettings
    {
        public static string[] AllowedUploadFileExtensions => ConfigurationManager.AppSettings["AllowedUploadFileExtensions"]?.Split(',') ?? new string[0];
        public static decimal TeacherInitialCash => SafeGetDecimal("TeacherInitialCash") ?? 0.0m;
        public static decimal StudentInitialCash => SafeGetDecimal("StudentInitialCash") ?? 0.0m;

        private static decimal? SafeGetDecimal(string settingName)
        {
            decimal initialCash;
            if (decimal.TryParse(ConfigurationManager.AppSettings[settingName], out initialCash))
            {
                return initialCash;
            }
            return null;
        }
    }
}