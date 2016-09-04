using System.Configuration;

namespace TeachMe.Helpers.Settings
{
    public static class ApplicationSettings
    {
        public static string[] AllowedUploadFileExtensions => ConfigurationManager.AppSettings["AllowedUploadFileExtensions"]?.Split(',') ?? new string[0];
        public static double TeacherInitialCash => SafeGetDouble("TeacherInitialCash") ?? 0.0;
        public static double StudentInitialCash => SafeGetDouble("StudentInitialCash") ?? 0.0;

        private static double? SafeGetDouble(string settingName)
        {
            double initialCash;
            if (double.TryParse(ConfigurationManager.AppSettings[settingName], out initialCash))
            {
                return initialCash;
            }
            return null;
        }
    }
}