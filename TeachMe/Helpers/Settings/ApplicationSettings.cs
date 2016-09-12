using System.Configuration;

namespace TeachMe.Helpers.Settings
{
    public static class ApplicationSettings
    {
        public static string[] AllowedUploadFileExtensions => ConfigurationManager.AppSettings["AllowedUploadFileExtensions"]?.Split(',') ?? new string[0];
        public static double TeacherInitialCash => SafeGetDouble("TeacherInitialCash") ?? 0.0;
        public static double StudentInitialCash => SafeGetDouble("StudentInitialCash") ?? 0.0;

        public static string RobokassaLogin => ConfigurationManager.AppSettings["RobokassaLogin"] ?? string.Empty;
        public static string RobokassaPassword1 => ConfigurationManager.AppSettings["RobokassaPassword1"] ?? string.Empty;
        public static string RobokassaPassword2 => ConfigurationManager.AppSettings["RobokassaPassword2"] ?? string.Empty;
        public static bool RobokassaIsInTest => SafeGetBool("RobokassaIsInTest") ?? false;

        private static double? SafeGetDouble(string settingName)
        {
            double value;
            if (double.TryParse(ConfigurationManager.AppSettings[settingName], out value))
            {
                return value;
            }
            return null;
        }

        private static bool? SafeGetBool(string settingName)
        {
            bool value;
            if (bool.TryParse(ConfigurationManager.AppSettings[settingName], out value))
            {
                return value;
            }
            return null;
        }
    }
}