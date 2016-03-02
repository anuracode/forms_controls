// <copyright file="HeaderCell.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using Xamarin.Forms;

namespace Anuracode.Forms.Controls
{
    /// <summary>
    /// Cell for the headers of grouped lists.
    /// </summary>
    public class HeaderCell : ViewCell
    {
        /// <summary>
        /// Header cell.
        /// </summary>
        public HeaderCell()
        {
            Height = 25;

            var title = new Label
            {
                FontSize = Styles.ThemeManager.CommonResourcesBase.TextSizeSmall,
                TextColor = Styles.ThemeManager.CommonResourcesBase.DefaultLabelTextColor,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };

            title.SetBinding(Label.TextProperty, "Key");

            View = new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HeightRequest = 25,
                BackgroundColor = Styles.ThemeManager.CommonResourcesBase.Accent,
                Padding = 5,
                Orientation = StackOrientation.Horizontal,
                Children = { title }
            };
        }
    }
}