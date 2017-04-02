using System.Configuration;
using System.Globalization;

namespace TeachMe.Helpers.Settings
{
    public static class ApplicationSettings
    {
        public static string[] AllowedUploadFileExtensions => ConfigurationManager.AppSettings["AllowedUploadFileExtensions"]?.Split(',') ?? new string[0];
        public static string MongoDatabasePrefix => SafeGetString("MongoDatabasePrefix") ?? "";
        public static double TeacherInitialCash => SafeGetDouble("TeacherInitialCash") ?? 0.0;
        public static double StudentInitialCash => SafeGetDouble("StudentInitialCash") ?? 0.0;
        public static double JobCommissionRate => SafeGetDouble("JobCommissionRate") ?? 0.25;
        public static double JobMinPrepaymentAmount => SafeGetDouble("JobMinPrepaymentAmount") ?? 100;
        public static string StudentProjectName => SafeGetString("StudentProjectName") ?? "";
        public static string StudentProjectTitle => SafeGetString("StudentProjectTitle") ?? "";
        public static string TeacherProjectName => SafeGetString("TeacherProjectName") ?? "";
        public static string TeacherProjectTitle => SafeGetString("TeacherProjectTitle") ?? "";

        public static string StudentContactEmail => SafeGetString("StudentContactEmail") ?? "";
        public static string TeacherContactEmail => SafeGetString("TeacherContactEmail") ?? "";
        public static string StudentContactPhone => SafeGetString("StudentContactPhone") ?? "";
        public static string TeacherContactPhone => SafeGetString("TeacherContactPhone") ?? "";

        public static string RobokassaLogin => ConfigurationManager.AppSettings["RobokassaLogin"] ?? string.Empty;
        public static string RobokassaPassword1 => ConfigurationManager.AppSettings["RobokassaPassword1"] ?? string.Empty;
        public static string RobokassaPassword2 => ConfigurationManager.AppSettings["RobokassaPassword2"] ?? string.Empty;
        public static bool RobokassaIsInTest => SafeGetBool("RobokassaIsInTest") ?? false;

        public static bool SmsNotificationEnabled => SafeGetBool("SmsNotificationEnabled") ?? false;
        public static int TeachersCountForNewJobNotification => SafeGetInt("TeachersCountForNewJobNotification") ?? 0;

        public static int SmsAeroServicePriority => SafeGetInt("SmsAeroServicePriority") ?? 0;
        public static string SmsAeroLogin => ConfigurationManager.AppSettings["SmsAeroLogin"] ?? string.Empty;
        public static string SmsAeroApiKey => ConfigurationManager.AppSettings["SmsAeroApiKey"] ?? string.Empty;
        public static string SmsAeroSenderName => ConfigurationManager.AppSettings["SmsAeroSenderName"] ?? string.Empty;
        public static int SmsAeroType => SafeGetInt("SmsAeroType") ?? 0;
        public static int SmsAeroDigital => SafeGetInt("SmsAeroDigital") ?? 0;

        public static int SmsIntelServicePriority => SafeGetInt("SmsIntelServicePriority") ?? 0;
        public static string SmsIntelLogin => ConfigurationManager.AppSettings["SmsIntelLogin"] ?? string.Empty;
        public static string SmsIntelPassword => ConfigurationManager.AppSettings["SmsIntelPassword"] ?? string.Empty;
        public static string SmsIntelSenderName => ConfigurationManager.AppSettings["SmsIntelSenderName"] ?? string.Empty;
        public static int SmsIntelUseAlfaSource => SafeGetInt("SmsIntelUseAlfaSource") ?? 0;
        public static int SmsIntelChannel => SafeGetInt("SmsIntelChannel") ?? 0;

        private static string SafeGetString(string settingName)
        {
            return ConfigurationManager.AppSettings[settingName];
        }

        private static double? SafeGetDouble(string settingName)
        {
            double value;
            if (double.TryParse(ConfigurationManager.AppSettings[settingName], NumberStyles.Number, CultureInfo.InvariantCulture, out value))
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

        private static int? SafeGetInt(string settingName)
        {
            int value;
            if (int.TryParse(ConfigurationManager.AppSettings[settingName], out value))
            {
                return value;
            }
            return null;
        }
    }
}