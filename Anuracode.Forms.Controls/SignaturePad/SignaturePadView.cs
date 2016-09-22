// <copyright file="SignaturePadView.cs" company="">
// All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Anuracode.Forms.Controls
{
    /// <summary>
    /// Control to view and edit a signature.
    /// </summary>
    public class SignaturePadView : View
    {
        /// <summary>
        /// Caption color property.
        /// </summary>
        public static readonly BindableProperty CaptionTextColorProperty = BindablePropertyHelper.Create<SignaturePadView, Color>(nameof(CaptionTextColor), Color.Default);

        /// <summary>
        /// Caption text property.
        /// </summary>
        public static readonly BindableProperty CaptionTextProperty = BindablePropertyHelper.Create<SignaturePadView, string>(nameof(CaptionText), (string)null);

        /// <summary>
        /// Prompt text color.
        /// </summary>
        public static readonly BindableProperty PromptTextColorProperty = BindablePropertyHelper.Create<SignaturePadView, Color>(nameof(PromptTextColor), Color.Default);

        /// <summary>
        /// Prompt text.
        /// </summary>
        public static readonly BindableProperty PromptTextProperty = BindablePropertyHelper.Create<SignaturePadView, string>(nameof(PromptText), (string)null);

        /// <summary>
        /// Signature line color.
        /// </summary>
        public static readonly BindableProperty SignatureLineColorProperty = BindablePropertyHelper.Create<SignaturePadView, Color>(nameof(SignatureLineColor), Color.Default);

        /// <summary>
        /// Storke color.
        /// </summary>
        public static readonly BindableProperty StrokeColorProperty = BindablePropertyHelper.Create<SignaturePadView, Color>(nameof(StrokeColor), Color.Default);

        /// <summary>
        /// Stroke width.
        /// </summary>
        public static readonly BindableProperty StrokeWidthProperty = BindablePropertyHelper.Create<SignaturePadView, float>(nameof(StrokeWidth), (float)0);

        /// <summary>
        /// Clear command.
        /// </summary>
        private Command clearCommand;

        /// <summary>
        /// Caption text.
        /// </summary>
        public string CaptionText
        {
            get
            {
                return (string)this.GetValue(SignaturePadView.CaptionTextProperty);
            }

            set
            {
                this.SetValue(SignaturePadView.CaptionTextProperty, value);
            }
        }

        /// <summary>
        /// Caption text color.
        /// </summary>
        public Color CaptionTextColor
        {
            get
            {
                return (Color)this.GetValue(SignaturePadView.CaptionTextColorProperty);
            }

            set
            {
                this.SetValue(SignaturePadView.CaptionTextColorProperty, value);
            }
        }

        /// <summary>
        /// Clear command.
        /// </summary>
        public Command ClearCommand
        {
            get
            {
                if (clearCommand == null)
                {
                    clearCommand = new Command(
                        () =>
                        {
                            AC.ScheduleManaged(() =>
                            {
                                if (ClearDelegate != null)
                                {
                                    ClearDelegate();
                                }
                            });
                        });
                }

                return clearCommand;
            }
        }

        /// <summary>
        /// Clear delegate.
        /// </summary>
        public Action ClearDelegate { get; set; }

        /// <summary>
        /// Get the draw point.
        /// </summary>
        public Func<Task<string>> GetImageStringBase64Delegate { get; set; }

        /// <summary>
        /// Delegate to check if the pad is blank.
        /// </summary>
        public Func<bool> GetIsBlankDelegate { get; set; }

        /// <summary>
        /// Delegate to get the points in string.
        /// </summary>
        public Func<Task<string>> GetPointsStringDelegate { get; set; }

        /// <summary>
        /// Flag when the pad is blank.
        /// </summary>
        public bool IsBlank
        {
            get
            {
                bool isBlank = true;

                if (GetIsBlankDelegate != null)
                {
                    isBlank = GetIsBlankDelegate();
                }

                return isBlank;
            }
        }

        /// <summary>
        /// Prompt text.
        /// </summary>
        public string PromptText
        {
            get
            {
                return (string)this.GetValue(SignaturePadView.PromptTextProperty);
            }

            set
            {
                this.SetValue(SignaturePadView.PromptTextProperty, value);
            }
        }

        /// <summary>
        /// Propmpt text color.
        /// </summary>
        public Color PromptTextColor
        {
            get
            {
                return (Color)this.GetValue(SignaturePadView.PromptTextColorProperty);
            }

            set
            {
                this.SetValue(SignaturePadView.PromptTextColorProperty, value);
            }
        }

        /// <summary>
        /// Signature line color.
        /// </summary>
        public Color SignatureLineColor
        {
            get
            {
                return (Color)this.GetValue(SignaturePadView.SignatureLineColorProperty);
            }

            set
            {
                this.SetValue(SignaturePadView.SignatureLineColorProperty, value);
            }
        }

        /// <summary>
        /// Stroke color.
        /// </summary>
        public Color StrokeColor
        {
            get
            {
                return (Color)this.GetValue(SignaturePadView.StrokeColorProperty);
            }

            set
            {
                this.SetValue(SignaturePadView.StrokeColorProperty, value);
            }
        }

        /// <summary>
        /// Stroke width.
        /// </summary>
        public float StrokeWidth
        {
            get
            {
                return (float)this.GetValue(SignaturePadView.StrokeWidthProperty);
            }

            set
            {
                this.SetValue(SignaturePadView.StrokeWidthProperty, value);
            }
        }

        /// <summary>
        /// Get the draw point.
        /// </summary>
        public async Task<string> GetImageStringBase64Async()
        {
            string result = null;

            if (GetImageStringBase64Delegate != null)
            {
                result = await GetImageStringBase64Delegate();
            }

            return result;
        }

        /// <summary>
        /// Get the points in string format.
        /// </summary>
        public async Task<string> GetPointsStringAsync()
        {
            string result = null;

            if (GetPointsStringDelegate != null)
            {
                result = await GetPointsStringDelegate();
            }

            return result;
        }

        /// <summary>
        /// Raise is blank changed.
        /// </summary>
        public void RaiseIsBlankChanged()
        {
            AC.ScheduleManaged(
                () =>
                {
                    OnPropertyChanged("IsBlank");
                });
        }
    }
}