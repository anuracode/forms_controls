// <copyright file="ShapeView.cs" company="">
// All rights reserved.
// </copyright>
// https://github.com/chrispellett/Xamarin-Forms-Shape

using Xamarin.Forms;

namespace Anuracode.Forms.Controls
{
    /// <summary>
    /// Control for showing box or rounded corners.
    /// </summary>
    public class ShapeView : BoxView
    {
        /// <summary>
        /// Corner radius property.
        /// </summary>
        public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create<ShapeView, float>(s => s.CornerRadius, 0f);

        /// <summary>
        /// Padding property.
        /// </summary>
        public static readonly BindableProperty PaddingProperty = BindableProperty.Create<ShapeView, Thickness>(s => s.Padding, default(Thickness));

        /// <summary>
        /// Shape type.
        /// </summary>
        public static readonly BindableProperty ShapeTypeProperty = BindableProperty.Create<ShapeView, ShapeType>(s => s.ShapeType, ShapeType.Box);

        /// <summary>
        /// Shape color.
        /// </summary>
        public static readonly BindableProperty StrokeColorProperty = BindableProperty.Create<ShapeView, Color>(s => s.StrokeColor, Color.Default);

        /// <summary>
        /// Stroke width.
        /// </summary>
        public static readonly BindableProperty StrokeWidthProperty = BindableProperty.Create<ShapeView, float>(s => s.StrokeWidth, 1f);

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ShapeView()
        {
        }

        /// <summary>
        /// Corder radius.
        /// </summary>
        public float CornerRadius
        {
            get
            {
                return (float)GetValue(CornerRadiusProperty);
            }

            set
            {
                if (ShapeType == ShapeType.Box)
                {
                    SetValue(CornerRadiusProperty, value);
                }
            }
        }

        /// <summary>
        /// Padding to use.
        /// </summary>
        public Thickness Padding
        {
            get
            {
                return (Thickness)GetValue(PaddingProperty);
            }

            set
            {
                SetValue(PaddingProperty, value);
            }
        }

        /// <summary>
        /// Shape type.
        /// </summary>
        public Anuracode.Forms.Controls.ShapeType ShapeType
        {
            get
            {
                return (Anuracode.Forms.Controls.ShapeType)GetValue(ShapeTypeProperty);
            }

            set
            {
                SetValue(ShapeTypeProperty, value);
            }
        }

        /// <summary>
        /// Color stroke.
        /// </summary>
        public Color StrokeColor
        {
            get
            {
                return (Color)GetValue(StrokeColorProperty);
            }

            set
            {
                SetValue(StrokeColorProperty, value);
            }
        }

        /// <summary>
        /// Stroke width.
        /// </summary>
        public float StrokeWidth
        {
            get
            {
                return (float)GetValue(StrokeWidthProperty);
            }

            set
            {
                SetValue(StrokeWidthProperty, value);
            }
        }
    }
}