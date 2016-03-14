// <copyright file="SignatureViewModel.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

namespace Anuracode.Forms.Controls.Sample.ViewModels
{
    /// <summary>
    /// View model for the signature.
    /// </summary>
    public class SignatureViewModel : BaseViewModel
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public SignatureViewModel()
            : base()
        {
            Title = App.LocalizationResources.SignatureLabel;
        }        
    }
}