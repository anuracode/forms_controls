// <copyright file="RenderersRegistration.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using Xamarin.Forms;

[assembly: ExportRenderer(typeof(Anuracode.Forms.Controls.ShapeView), typeof(Anuracode.Forms.Controls.Sample.iOS.Renderers.ShapeRenderer))]
[assembly: ExportRenderer(typeof(Anuracode.Forms.Controls.ExtendedLabel), typeof(Anuracode.Forms.Controls.Sample.iOS.Renderers.ExtendedLabelRenderer))]
[assembly: ExportRenderer(typeof(Anuracode.Forms.Controls.ExtendedImage), typeof(Anuracode.Forms.Controls.Sample.iOS.Renderers.ManagedImageRenderer))]
[assembly: ExportRenderer(typeof(Anuracode.Forms.Controls.ExtendedEntry), typeof(Anuracode.Forms.Controls.Sample.iOS.Renderers.ExtendedEntryRenderer))]
[assembly: ExportRenderer(typeof(Anuracode.Forms.Controls.SignaturePadView), typeof(Anuracode.Forms.Controls.Sample.iOS.Renderers.SignaturePadViewRenderer))]
[assembly: ExportRenderer(typeof(Anuracode.Forms.Controls.CircleImage), typeof(Anuracode.Forms.Controls.Sample.iOS.Renderers.ImageCircleRenderer))]

namespace Anuracode.Forms.Controls.Sample.iOS.Renderers
{
    /// <summary>
    /// Renderer proxy.
    /// </summary>
    public class ExtendedEntryRenderer : Anuracode.Forms.Controls.Renderers.ExtendedEntryRenderer { }

    /// <summary>
    /// Renderer proxy.
    /// </summary>
    public class ExtendedLabelRenderer : Anuracode.Forms.Controls.Renderers.ExtendedLabelRenderer { }

    /// <summary>
    /// Renderer proxy.
    /// </summary>
    public class ImageCircleRenderer : Anuracode.Forms.Controls.Renderers.ImageCircleRenderer { }

    /// <summary>
    /// Renderer proxy.
    /// </summary>
    public class ManagedImageRenderer : Anuracode.Forms.Controls.Renderers.ExtendedImageRenderer { }

    /// <summary>
    /// Renderer proxy.
    /// </summary>
    public class ShapeRenderer : Anuracode.Forms.Controls.Renderers.ShapeRenderer { }

    /// <summary>
    /// Renderer proxy.
    /// </summary>
    public class SignaturePadViewRenderer : Anuracode.Forms.Controls.Renderers.SignaturePadRenderer { }
}