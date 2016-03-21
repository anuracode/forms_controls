// <copyright file="RenderUtilBase.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Anuracode.Forms.Controls.Views.Extensions;

namespace Anuracode.Forms.Controls.Styles
{
    /// <summary>
    /// Render utilities.
    /// </summary>
    public class RenderUtilBase
    {
        /// <summary>
        /// Format for the two points.
        /// </summary>
        private const string TWO_POINTS_FORMAT = "{0}: ";

        /// <summary>
        /// Animate fade int of the element, set the opacity only.
        /// </summary>
        /// <param name="viewElement">Element to use.</param>
        /// <param name="lenght">Time to animate.</param>
        /// <param name="easing">Easing to use.</param>
        /// <returns>Task to await.</returns>
        public virtual async Task AnimateFadeInViewAsync(View viewElement, uint lenght = 250, Easing easing = null)
        {
            if (viewElement != null)
            {
                await viewElement.FadeTo(1, lenght, easing);
            }
        }

        /// <summary>
        /// Animate fade out of the element, set the visible and opacity too.
        /// </summary>
        /// <param name="viewElement">Element to use.</param>
        /// <param name="lenght">Time to animate.</param>
        /// <param name="easing">Easing to use.</param>
        /// <returns>Task to await.</returns>
        public virtual async Task AnimateFadeOutViewAsync(View viewElement, uint lenght = 250, Easing easing = null)
        {
            if (viewElement != null)
            {
                await viewElement.FadeTo(0, lenght, easing);

                viewElement.Opacity = 0;
                viewElement.UpdateIsVisible(false);
            }
        }

        /// <summary>
        /// Animate fade out of the element, set the visible and opacity too.
        /// </summary>
        /// <param name="viewElement">Element to use.</param>
        /// <returns>Task to await.</returns>
        public virtual void AnimatePrepareFadeInView(View viewElement)
        {
            if (viewElement != null)
            {
                if (viewElement.Opacity != 0)
                {
                    viewElement.Opacity = 1;
                }

                if (viewElement.IsVisible != true)
                {
                    viewElement.IsVisible = true;
                }
            }
        }

        /// <summary>
        /// Instance line separator.
        /// </summary>
        /// <returns></returns>
        public virtual View InstaceLineSeparator(double heightRequest = 2)
        {
            return InstaceLineSeparator(ThemeManager.CommonResourcesBase.Accent, heightRequest);
        }

        /// <summary>
        /// Instance line separator.
        /// </summary>
        /// <returns></returns>
        public virtual View InstaceLineSeparator(Color lineColor, double heightRequest = 2)
        {
            var boxLine = new BoxView()
            {
                Color = lineColor,
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HeightRequest = heightRequest
            };

            return boxLine;
        }

        /// <summary>
        /// Render background filter.
        /// </summary>
        /// <returns>View to use.</returns>
        public virtual View InstanceBackgroundDetail(Action actionTapped)
        {
            BoxView backgroundBox = new BoxView()
            {
                Color = ThemeManager.CommonResourcesBase.BackgroundColorTranslucent,
                IsVisible = false,
                Opacity = 0
            };

            if (actionTapped != null)
            {
                TapGestureRecognizer tapBackground = new TapGestureRecognizer();

                tapBackground.Tapped += (object sender, System.EventArgs e) =>
                {
                    actionTapped();
                };

                backgroundBox.GestureRecognizers.Add(tapBackground);
            }

            return backgroundBox;
        }

        /// <summary>
        /// Render a field.
        /// </summary>
        /// <typeparam name="TViewModel">Viewmodel to use.</typeparam>
        /// <param name="stackContainer">Container to use.</param>
        /// <param name="expressionLabel">Expression for the label.</param>
        /// <param name="expressionIsVisible">Expression for the group to be visible.</param>
        /// <param name="orientation">Orientation of the group.</param>
        /// <param name="labelConverter">Converter for the label.</param>
        /// <param name="isVisibleConverter">Is visible converter.</param>
        /// <param name="labelValue">Label as value.</param>
        /// <param name="addTwoPoint">Add to points to the label.</param>
        /// <param name="contentView">Control to add as content.</param>
        /// <param name="labelSytle">Style to use.</param>
        /// <param name="horizontalOptions">Option to center.</param>
        public virtual void RenderField<TViewModel>(
            StackLayout stackContainer,
            Expression<Func<TViewModel, object>> expressionLabel = null,
            Expression<Func<TViewModel, object>> expressionIsVisible = null,
            StackOrientation orientation = StackOrientation.Vertical,
            IValueConverter labelConverter = null,
            IValueConverter isVisibleConverter = null,
            string labelValue = null,
            bool addTwoPoint = true,
            View contentView = null,
            Style labelSytle = null,
            TextAlignment horizontalOptions = TextAlignment.Start)
        {
            StackLayout stackElement = new StackLayout()
            {
                Style = ThemeManager.ApplicationStyles.FormRowContainerStyle,
                Orientation = orientation
            };

            switch (horizontalOptions)
            {
                case TextAlignment.Center:
                    stackElement.HorizontalOptions = LayoutOptions.Center;
                    break;

                case TextAlignment.End:
                    stackElement.HorizontalOptions = LayoutOptions.End;
                    break;

                case TextAlignment.Start:
                default:
                    stackElement.HorizontalOptions = LayoutOptions.Fill;
                    break;
            }

            if (expressionIsVisible != null)
            {
                stackElement.SetBinding<TViewModel>(StackLayout.IsVisibleProperty, expressionIsVisible, converter: isVisibleConverter);
            }

            if ((expressionLabel != null) || (labelValue != null))
            {
                ExtendedLabel labelElement = new ExtendedLabel();

                if (orientation == StackOrientation.Horizontal)
                {
                    labelElement.VerticalOptions = LayoutOptions.Center;
                    labelElement.HorizontalOptions = LayoutOptions.Start;
                }
                else
                {
                    labelElement.VerticalOptions = LayoutOptions.Start;
                    labelElement.HorizontalOptions = LayoutOptions.Start;
                }

                if (labelSytle == null)
                {
                    labelElement.Style = ThemeManager.ApplicationStyles.DetailNameExtendedLabelStyle;
                }
                else
                {
                    labelElement.Style = labelSytle;
                }

                if (labelValue != null)
                {
                    labelElement.Text = string.Format(TWO_POINTS_FORMAT, labelValue);
                }
                else if (expressionLabel != null)
                {
                    if (addTwoPoint)
                    {
                        labelElement.SetBinding<TViewModel>(ExtendedLabel.TextProperty, expressionLabel, converter: labelConverter, stringFormat: TWO_POINTS_FORMAT);
                    }
                    else
                    {
                        labelElement.SetBinding<TViewModel>(ExtendedLabel.TextProperty, expressionLabel, converter: labelConverter);
                    }
                }

                stackElement.Children.Add(labelElement);
            }

            if (contentView != null)
            {
                stackElement.Children.Add(contentView);
            }

            stackContainer.Children.Add(stackElement);
        }

        /// <summary>
        /// Render a field for a form.
        /// </summary>
        /// <typeparam name="TViewModel">Viewmodel to use.</typeparam>
        /// <param name="stackContainer">Container to use.</param>
        /// <param name="expressionLabel">Expression for the label.</param>
        /// <param name="expressionIsVisible">Expression for the group to be visible.</param>
        /// <param name="orientation">Orientation of the group.</param>
        /// <param name="labelConverter">Converter for the label.</param>
        /// <param name="isVisibleConverter">Is visible converter.</param>
        /// <param name="labelValue">Label as value.</param>
        /// <param name="addTwoPoint">Add to points to the label.</param>
        /// <param name="editorView">Control to add as content.</param>
        /// <param name="labelSytle">Style for the label.</param>
        /// <param name="addSpaceAfter">Add space before field.</param>
        public virtual void RenderFormField<TViewModel>(
            StackLayout stackContainer,
            Expression<Func<TViewModel, object>> expressionLabel = null,
            Expression<Func<TViewModel, object>> expressionIsVisible = null,
            StackOrientation orientation = StackOrientation.Vertical,
            IValueConverter labelConverter = null,
            IValueConverter isVisibleConverter = null,
            string labelValue = null,
            bool addTwoPoint = true,
            View editorView = null,
            Style labelSytle = null,
            TextAlignment horizontalOptions = TextAlignment.Start,
            bool addSpaceAfter = true)
        {
            Style formLabelStyle = null;

            if (labelSytle == null)
            {
                formLabelStyle = ThemeManager.ApplicationStyles.FormExtendedLabelStyle;
            }
            else
            {
                formLabelStyle = labelSytle;
            }

            RenderField<TViewModel>(stackContainer, expressionLabel, expressionIsVisible, orientation, labelConverter, isVisibleConverter, labelValue, addTwoPoint, editorView, formLabelStyle, horizontalOptions: horizontalOptions);

            if (addSpaceAfter)
            {
                RenderSpace(stackContainer);
            }
        }

        /// <summary>
        /// Creates a stack panel with spaces in start and end.
        /// </summary>
        /// <param name="stackContainer">Container to use.</param>
        /// <returns>The new container that has been idented.</returns>
        public virtual StackLayout RenderIdentedVerticalContainer(StackLayout stackContainer, double spaceRequest = 10)
        {
            StackLayout spacerContainer = new StackLayout()
            {
                Style = ThemeManager.ApplicationStyles.FormRowContainerStyle,
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            StackLayout identedContainer = new StackLayout()
            {
                Style = ThemeManager.ApplicationStyles.FormRowContainerStyle,
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            RenderSpace(spacerContainer, spaceRequest, spaceRequest);
            spacerContainer.Children.Add(identedContainer);
            RenderSpace(spacerContainer, spaceRequest, spaceRequest);

            stackContainer.Children.Add(spacerContainer);

            return identedContainer;
        }

        /// <summary>
        /// Render a legend.
        /// </summary>
        /// <param name="container">Containder to use.</param>
        /// <param name="labelLegend">Text to use.</param>
        /// <param name="colorLegend">Color to use.</param>
        public virtual void RenderLegend(StackLayout container, string labelLegend, Color colorLegend)
        {
            if (container != null)
            {
                container.Children.Add(new BoxView { Color = colorLegend, WidthRequest = 10 });

                var labelAssigned = new ExtendedLabel()
                {
                    Style = ThemeManager.ApplicationStyles.DescriptionExtendedLabelStyle,
                    Text = labelLegend,
                    TextColor = colorLegend,
                    HorizontalOptions = LayoutOptions.Start,
                    VerticalOptions = LayoutOptions.Center
                };

                container.Children.Add(labelAssigned);

                container.Children.Add(new BoxView { Color = Color.Transparent, WidthRequest = 10 });
            }
        }

        /// <summary>
        /// Render a legend.
        /// </summary>
        /// <param name="container">Containder to use.</param>
        /// <param name="labelLegend">Text to use.</param>
        /// <param name="labelLegendValue">Color to use.</param>
        public virtual void RenderLegend(StackLayout container, string labelLegend, string labelLegendValue)
        {
            if (container != null)
            {
                container.Children.Add(new BoxView { Color = Color.Transparent, WidthRequest = 10 });

                var labelvalue1 = new ExtendedLabel()
                {
                    Style = ThemeManager.ApplicationStyles.DetailNameExtendedLabelStyle,
                    Text = labelLegend + ": ",
                    HorizontalOptions = LayoutOptions.Start,
                    VerticalOptions = LayoutOptions.Center
                };

                container.Children.Add(labelvalue1);

                container.Children.Add(new BoxView { Color = Color.Transparent, WidthRequest = 5 });

                var labelvalue2 = new ExtendedLabel()
                {
                    Style = ThemeManager.ApplicationStyles.DetailValueExtendedLabelStyle,
                    Text = labelLegendValue,
                    HorizontalOptions = LayoutOptions.Start,
                    VerticalOptions = LayoutOptions.Center
                };

                container.Children.Add(labelvalue2);

                container.Children.Add(new BoxView { Color = Color.Transparent, WidthRequest = 10 });
            }
        }

        /// <summary>
        /// Render a legend.
        /// </summary>
        /// <param name="container">Containder to use.</param>
        /// <param name="labelLegend">Text to use.</param>
        /// <param name="colorLegend">Color to use.</param>
        public virtual void RenderLegendWithCommand(StackLayout container, string labelLegend, Color colorLegend, ICommand command, object commandParamter)
        {
            if (container != null)
            {
                var buttonLegend = new ContentTemplateViewButton(new DataTemplate(
                    () =>
                    {
                        StackLayout panelLegendLayout = new StackLayout()
                        {
                            Style = ThemeManager.ApplicationStyles.SimpleStackContainerStyle,
                            Orientation = StackOrientation.Horizontal,
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            VerticalOptions = LayoutOptions.FillAndExpand
                        };

                        RenderLegend(panelLegendLayout, labelLegend, colorLegend);

                        return panelLegendLayout;
                    }))
                {
                    Command = command,
                    Style = ThemeManager.ApplicationStyles.TextOnlyContentButtonStyle,
                    CommandParameter = commandParamter
                };

                if (command == null)
                {
                    buttonLegend.ButtonDisableBackgroundColor = Color.Transparent;
                }

                container.Children.Add(buttonLegend);
            }
        }

        /// <summary>
        /// Render line separator.
        /// </summary>
        /// <param name="container">Containder to use.</param>
        public virtual void RenderLineSeparator(Layout<View> container, Color lineColor, double heightRequest = 2)
        {
            if (container != null)
            {
                container.Children.Add(InstaceLineSeparator(lineColor, heightRequest: heightRequest));
            }
        }

        /// <summary>
        /// Render line separator.
        /// </summary>
        /// <param name="container">Containder to use.</param>
        public virtual void RenderLineSeparator(Layout<View> container, double heightRequest = 2)
        {
            if (container != null)
            {
                container.Children.Add(InstaceLineSeparator(heightRequest: heightRequest));
            }
        }

        /// <summary>
        /// Render a read field.
        /// </summary>
        /// <typeparam name="TViewModel">ViewModel to use.</typeparam>
        /// <param name="stackContainer">Container to use.</param>
        /// <param name="expressionLabel">Expression for the lable.</param>
        /// <param name="expressionValue">Expression for the value.</param>
        /// <param name="expressionIsVisible">Expression for when the container is visible.</param>
        /// <param name="orientation">Orientation.</param>
        /// <param name="labelConverter">Converter for the label.</param>
        /// <param name="valueConverter">Converter for the value.</param>
        /// <param name="isVisibleConverter">Converter for the is visible.</param>
        /// <param name="labelValue">Value for the label (when a expression do not apply.</param>
        /// <param name="valueValue">Value for the value (when a expression do not apply.</param>
        /// <param name="addTwoPoint">Add two points to the label.</param>
        public virtual void RenderReadField<TViewModel>(
            StackLayout stackContainer,
            Expression<Func<TViewModel, object>> expressionLabel = null,
            Expression<Func<TViewModel, object>> expressionValue = null,
            Expression<Func<TViewModel, object>> expressionIsVisible = null,
            StackOrientation orientation = StackOrientation.Vertical,
            IValueConverter labelConverter = null,
            IValueConverter valueConverter = null,
            IValueConverter isVisibleConverter = null,
            string labelValue = null,
            string valueValue = null,
            bool addTwoPoint = true,
            TextAlignment horizontalOptions = TextAlignment.Start)
        {
            View contentValue = null;

            if ((expressionValue != null) || (valueValue != null))
            {
                ExtendedLabel extendedLabelValue = new ExtendedLabel()
                {
                    Style = ThemeManager.ApplicationStyles.DetailValueExtendedLabelStyle
                };

                if (orientation == StackOrientation.Horizontal)
                {
                    extendedLabelValue.VerticalOptions = LayoutOptions.Center;
                    extendedLabelValue.HorizontalOptions = LayoutOptions.Start;
                }
                else
                {
                    extendedLabelValue.VerticalOptions = LayoutOptions.Start;
                    extendedLabelValue.HorizontalOptions = LayoutOptions.Start;
                }

                if (valueValue != null)
                {
                    extendedLabelValue.Text = valueValue;
                }
                else if (expressionValue != null)
                {
                    extendedLabelValue.SetBinding<TViewModel>(ExtendedLabel.TextProperty, expressionValue, converter: valueConverter);
                }

                contentValue = extendedLabelValue;
            }

            RenderField<TViewModel>(stackContainer, expressionLabel, expressionIsVisible, orientation, labelConverter, isVisibleConverter, labelValue, addTwoPoint, contentValue, horizontalOptions: horizontalOptions);
        }

        /// <summary>
        /// Render section title.
        /// </summary>
        /// <typeparam name="TViewModel">View model to use.</typeparam>
        /// <param name="stackContainer">Container to use.</param>
        /// <param name="expressionLabel">Expression for the label.</param>
        /// <param name="expressionIsVisible">Expression for the is visible.</param>
        /// <param name="labelConverter">Converter to use.</param>
        /// <param name="addTwoPoint">Add ':' at the end.</param>
        /// <param name="labelValue">Label value</param>
        /// <param name="isVisibleConverter">Is visible converter.</param>
        /// <param name="cornerRadius">Corner radius.</param>
        public virtual void RenderSectionTitle<TViewModel>(
            StackLayout stackContainer,
            Expression<Func<TViewModel, object>> expressionLabel = null,
            Expression<Func<TViewModel, object>> expressionIsVisible = null,
            IValueConverter labelConverter = null,
            IValueConverter isVisibleConverter = null,
            bool addTwoPoint = false,
            string labelValue = null,
            float cornerRadius = 10)
        {
            Grid gridLayout = new Grid()
            {
                RowSpacing = 0,
                ColumnSpacing = 0,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.Start,
                MinimumHeightRequest = 0,
                MinimumWidthRequest = 0
            };

            ShapeView shapeSectionBackground = new ShapeView()
            {
                CornerRadius = cornerRadius,
                Color = ThemeManager.CommonResourcesBase.Accent,
                ShapeType = ShapeType.Box
            };

            gridLayout.Children.Add(shapeSectionBackground);

            StackLayout stackSection = new StackLayout()
            {
                Style = ThemeManager.ApplicationStyles.FormSectionContainerStyle,
                BackgroundColor = Color.Transparent,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.Center,
                Spacing = 0,
                Orientation = StackOrientation.Horizontal
            };

            ExtendedLabel labelsection = new ExtendedLabel()
            {
                Style = ThemeManager.ApplicationStyles.FormExtendedLabelStyle,
                TextColor = ThemeManager.CommonResourcesBase.TextColorSection,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.Center
            };

            if (!string.IsNullOrEmpty(labelValue))
            {
                if (addTwoPoint)
                {
                    labelsection.Text = string.Format(" {0}:", labelValue);
                }
                else
                {
                    labelsection.Text = string.Format(" {0}", labelValue);
                }
            }
            else if (expressionLabel != null)
            {
                if (addTwoPoint)
                {
                    labelsection.SetBinding<TViewModel>(ExtendedLabel.TextProperty, expressionLabel, stringFormat: " {0}:", converter: labelConverter);
                }
                else
                {
                    labelsection.SetBinding<TViewModel>(ExtendedLabel.TextProperty, expressionLabel, stringFormat: " {0}", converter: labelConverter);
                }
            }

            if (expressionIsVisible != null)
            {
                gridLayout.SetBinding<TViewModel>(StackLayout.IsVisibleProperty, expressionIsVisible, converter: isVisibleConverter);
                shapeSectionBackground.SetBinding<TViewModel>(ShapeView.IsVisibleProperty, expressionIsVisible, converter: isVisibleConverter);
            }

            RenderSpace(stackSection, 10, 10);
            stackSection.Children.Add(labelsection);
            gridLayout.Children.Add(stackSection);
            stackContainer.Children.Add(gridLayout);
        }

        /// <summary>
        /// Render space.
        /// </summary>
        /// <param name="container">Containder to use.</param>
        /// <param name="heightRequest">Height to use.</param>
        /// <param name="widthRequest">Width to use.</param>
        public virtual void RenderSpace(Layout<View> container, double heightRequest = 5, double widthRequest = 5)
        {
            if (container != null)
            {
                container.Children.Add(new BoxView
                {
                    Color = Color.Transparent,
                    HeightRequest = heightRequest,
                    WidthRequest = widthRequest,
                    VerticalOptions = LayoutOptions.Start,
                    HorizontalOptions = LayoutOptions.Start
                });
            }
        }

        /// <summary>
        /// Rotate animation.
        /// </summary>
        /// <param name="view">View to use.</param>
        /// <returns>Task to await.</returns>
        public virtual async Task RotateAnimationAsync(View view)
        {
            if (view != null)
            {
                await view.RotateTo(360, easing: Easing.Linear);
                await view.RotateTo(0, 0);
            }
        }
    }
}