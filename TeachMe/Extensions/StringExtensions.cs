using TeachMe.Helpers.Cryptography;

namespace TeachMe.Extensions
{
    public static class StringExtensions
    {
        public static string GetMd5Hash(this string input)
        {
            return HashHelper.GetMd5Hash(input);
        }
    }
}