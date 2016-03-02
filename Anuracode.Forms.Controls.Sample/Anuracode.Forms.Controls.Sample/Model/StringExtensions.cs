// <copyright file="StringExtensions.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

namespace Anuracode.Forms.Controls.Sample.Model
{
    /// <summary>
    /// String extension.
    /// </summary>    
    public static class StringExtensions
    {
        /// <summary>
        /// Returns the string with only the first letter in caps.
        /// </summary>
        /// <param name="originalValue">Original value.</param>
        /// <returns>Converted value.</returns>
        public static string LetterCasingSentence(this string originalValue)
        {
            string newValue = originalValue;

            if (originalValue.Length >= 1)
            {
                newValue = string.Concat(originalValue.Substring(0, 1).ToUpper(), originalValue.Substring(1));
            }
            else
            {
                newValue = originalValue.ToUpper();
            }

            return newValue;
        }

        /// <summary>
        /// Truncate the string
        /// </summary>
        /// <param name="value">The string to be truncated</param>
        /// <param name="length">The length to truncate to</param>
        /// <param name="truncationString">The string used to truncate with</param>
        /// <param name="from">The enum value used to determine from where to truncate the string</param>
        /// <returns>The truncated string</returns>
        public static string Truncate(this string value, int length, string truncationString = "…")
        {
            if (value == null)
            {
                return null;
            }

            if (value.Length == 0)
            {
                return value;
            }

            if (truncationString == null || truncationString.Length > length)
            {
                return value.Substring(0, length);
            }

            return value.Length > length
                ? value.Substring(0, length - truncationString.Length) + truncationString
                : value;
        }
    }
}
