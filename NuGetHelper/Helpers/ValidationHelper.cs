using System;
using System.IO;

namespace NuGetHelper.Helpers
{
    internal static class ValidationHelper
    {
        public static void ThrowIfStringIsNullOrEmpty(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException(nameof(value));
        }

        public static void ThrowIfFileNotExists(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException("File not found");
        }
    }
}