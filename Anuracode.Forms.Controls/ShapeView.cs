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
        public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create(nameof(CornerRadius), typeof(bool), typeof(ShapeView), defaultValue: 0f);

        /// <summary>
        /// Padding property.
        /// </summary>        
        public static readonly BindableProperty PaddingProperty = BindableProperty.Create(nameof(Padding), typeof(Thickness), typeof(ShapeView), defaultValue: default(Thickness));

        /// <summary>
        /// Shape type.
        /// </summary>
        public static readonly BindableProperty ShapeTypeProperty = BindableProperty.Create(nameof(ShapeType), typeof(Anuracode.Forms.Controls.ShapeType), typeof(ShapeView), defaultValue: Anuracode.Forms.Controls.ShapeType.Box);

        /// <summary>
        /// Shape color.
        /// </summary>
        public static readonly BindableProperty StrokeColorProperty = BindableProperty.Create(nameof(StrokeColor), typeof(Color), typeof(ShapeView), defaultValue: Color.Default);

        /// <summary>
        /// Stroke width.
        /// </summary>
        public static readonly BindableProperty StrokeWidthProperty = BindableProperty.Create(nameof(StrokeWidth), typeof(float), typeof(ShapeView), defaultValue: 1);

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