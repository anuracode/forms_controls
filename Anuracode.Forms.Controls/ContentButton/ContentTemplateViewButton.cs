// <copyright file="ContentTemplateViewButton.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using System;
using Xamarin.Forms;

namespace Anuracode.Forms.Controls
{
    /// <summary>
    /// Content view button.
    /// </summary>
    public class ContentTemplateViewButton : ContentViewButton
    {
        /// <summary>
        /// Button template.
        /// </summary>
        private DataTemplate template;

        /// <summary>
        /// Templated view.
        /// </summary>
        private View templateView;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="hasText">Has text.</param>
        /// <param name="hasImage">Has image.</param>
        /// <param name="orientation">Orientation.</param>
        /// <param name="hasBorder">Has border.</param>
        /// <param name="hasBackground">Has background.</param>
        /// <param name="useDisableBox">Use disable box.</param>
        public ContentTemplateViewButton(DataTemplate buttonTemplate, bool hasBorder = false, bool hasBackground = false, bool useDisableBox = false)
            : base(false, false, hasBorder: hasBorder, hasBackground: hasBackground, useDisableBox: useDisableBox)
        {
            template = buttonTemplate;
                        
            RenderContent(hasText: HasText, hasImage: HasImage, orientation: ImageOrientation.ImageToLeft, hasBorder: hasBorder, hasBackground: hasBackground, useDisableBox: useDisableBox);
        }

        /// <summary>
        /// Binding context changed.
        /// </summary>
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (templateView != null)
            {
                if (templateView.BindingContext != this.BindingContext)
                {
                    templateView.BindingContext = this.BindingContext;
                }
            }
        }

        /// <summary>
        /// Render button content.
        /// </summary>
        /// <param name="hasText">Has text.</param>
        /// <param name="hasImage">Has image.</param>
        /// <param name="orientation">Orientation.</param>
        /// <returns>View to use.</returns>
        protected override View RenderButtonContent(bool hasText, bool hasImage, ImageOrientation orientation)
        {
            if (template == null)
            {
                throw new ArgumentException("buttonTemplate");
            }

            var content = template.CreateContent();

            if (!(content is View) && !(content is ViewCell))
            {
                throw new ArgumentException(content.GetType().Name);
            }

            templateView = (content is View) ? content as View : ((ViewCell)content).View;
            templateView.BindingContext = this.BindingContext;

            return templateView;
        }

        /// <summary>
        /// Set the content disable or enabled.
        /// </summary>
        /// <param name="canExecute">Can execute command.</param>
        protected override void SetContentEnable(bool canExecute)
        {
            SetImageEnable(canExecute);
        }
    }
}