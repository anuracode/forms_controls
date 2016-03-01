// <copyright file="AboutViewModel.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Anuracode.Forms.Controls.Sample.ViewModels
{
    /// <summary>
    /// Login view model
    /// </summary>
    public class AboutViewModel : BaseViewModel
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public AboutViewModel()
            : base()
        {
            Title = App.LocalizationResources.AboutMenu;

            Type currentType = this.GetType();

            FileVersion = GetFileVersion(currentType);
            AssemblyVersion = GetAssemblyVersion(currentType);
        }

        /// <summary>
        /// Assembly version.
        /// </summary>
        public string AssemblyVersion
        {
            get;
            private set;
        }

        /// <summary>
        /// Id of the user.
        /// </summary>
        public string FileVersion
        {
            get;
            private set;
        }

        /// <summary>
        /// Get assembly version.
        /// </summary>
        /// <param name="assemblyType">Type to look the assambly for.</param>
        /// <returns>File version.</returns>
        public static string GetAssemblyVersion(Type assemblyType)
        {
            string assemblyVersion = string.Empty;

            if (assemblyType != null)
            {
                string fullclassAssemblyQualifiedName = assemblyType.AssemblyQualifiedName;

                List<string> assemblyParts = new List<string>(fullclassAssemblyQualifiedName.Split(new char[] { ',' }));

                assemblyParts.RemoveAt(0);

                string assemblyName = string.Join(", ", assemblyParts);
                Assembly currentAssembly = Assembly.Load(new AssemblyName(assemblyName));

                if (currentAssembly != null)
                {
                    assemblyVersion = currentAssembly.FullName.Split(new char[] { ',', '=' })[2].ToString();
                }
            }

            return assemblyVersion;
        }

        /// <summary>
        /// Get file version.
        /// </summary>
        /// <param name="assemblyType">Type to look the assambly for.</param>
        /// <returns>File version.</returns>
        public static string GetFileVersion(Type assemblyType)
        {
            string fileVersion = string.Empty;

            if (assemblyType != null)
            {
                string fullclassAssemblyQualifiedName = assemblyType.AssemblyQualifiedName;

                List<string> assemblyParts = new List<string>(fullclassAssemblyQualifiedName.Split(new char[] { ',' }));

                assemblyParts.RemoveAt(0);

                string assemblyName = string.Join(", ", assemblyParts);
                Assembly currentAssembly = Assembly.Load(new AssemblyName(assemblyName));

                if (currentAssembly != null)
                {
                    fileVersion = currentAssembly.GetCustomAttributes<AssemblyFileVersionAttribute>().First().Version;
                }
            }

            return fileVersion;
        }
    }
}