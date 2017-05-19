// <copyright file="DeviceExtension.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

namespace Anuracode.Forms.Controls
{
    /// <summary>
    /// Device extension.
    /// </summary>
    public static class DeviceExtension
    {
        /// <summary>
        /// Represent Android platfrom.
        /// </summary>
        public const string Android = "Android";

        /// <summary>
        /// Represent iOS platfrom.
        /// </summary>
        public const string iOS = "iOS";

        /// <summary>
        /// Represent Mac platfrom.
        /// </summary>
        public const string macOS = "macOS";

        /// <summary>
        /// Represent Windows 10 platfrom.
        /// </summary>
        public const string UWP = "UWP";

        /// <summary>
        /// Represent Windows phone platfrom.
        /// </summary>
        public const string WinPhone = "WinPhone";

        /// <summary>
        /// Represent Windows 8 platfrom.
        /// </summary>
        public const string WinRT = "WinRT";

        /// <summary>
        /// Platform delegate.
        /// </summary>
        /// <typeparam name="TValue">Type of the value.</typeparam>
        /// <param name="os">Os to use.</param>
        /// <param name="iOS">Delegate to use.</param>
        /// <param name="android">Delegate to use.</param>
        /// <param name="windowsPhone">Delegate to use.</param>
        /// <param name="windows">Delegate to use.</param>
        /// <param name="other">Delegate to use.</param>
        /// <returns>Value to use.</returns>
        public static TValue OnPlatform<TValue>(this string os, TValue iOS, TValue android, TValue windowsPhone, TValue windows, TValue other = default(TValue))
        {
            TValue returnValue = default(TValue);

            switch (os)
            {
                case DeviceExtension.Android:
                    returnValue = android;
                    break;

                case DeviceExtension.WinPhone:
                    returnValue = windowsPhone;
                    break;

                case DeviceExtension.UWP:
                case DeviceExtension.WinRT:
                    returnValue = windows;
                    break;

                case DeviceExtension.iOS:
                    returnValue = iOS;
                    break;

                default:
                    returnValue = other;
                    break;
            }

            return returnValue;
        }
    }
}