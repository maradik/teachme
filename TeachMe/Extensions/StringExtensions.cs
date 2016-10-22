using System;
using TeachMe.Helpers.Cryptography;

namespace TeachMe.Extensions
{
    public static class StringExtensions
    {
        private const string TruncatedStringSuffix = "...";

        public static string GetMd5Hash(this string input)
        {
            return HashHelper.GetMd5Hash(input);
        }

        public static string Truncate(this string subject, int maxLength)
        {
            if (maxLength < TruncatedStringSuffix.Length)
                throw new ArgumentOutOfRangeException(nameof(maxLength), maxLength, $"Минимальное значение {TruncatedStringSuffix.Length}");

            if (subject.Length <= maxLength)
                return subject;

            var truncatedTrimmedString = subject.Substring(0, maxLength - TruncatedStringSuffix.Length).TrimEnd(' ', '.', ',', ';', ':', '?', '!', '\r', '\n');

            return truncatedTrimmedString + TruncatedStringSuffix;
        }
    }
}