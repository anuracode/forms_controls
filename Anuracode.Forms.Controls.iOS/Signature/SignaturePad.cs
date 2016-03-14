// <copyright file="SignaturePadView.cs" company="Timothy Risi">
// All rights reserved.
// </copyright>
//
// SignaturePadView.cs: UIView subclass for MonoTouch to allow users to draw their signature on the device
// 		     to be captured as an image or vector.
//
// Author:
//   Timothy Risi (timothy.risi@gmail.com)
//
// Copyright (C) 2012 Timothy Risi

using CoreGraphics;
using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UIKit;

namespace Anuracode.Forms.Controls.Renderers
{
    /// <summary>
    /// Signature pad view.
    /// </summary>
    [Register("SignaturePad")]
    public class SignaturePad : UIView
    {
        /// <summary>
        /// Current path.
        /// </summary>
        private UIBezierPath currentPath;

        /// <summary>
        /// Current point.
        /// </summary>
        private List<CGPoint> currentPoints;

        /// <summary>
        /// Image view.
        /// </summary>
        private UIImageView imageView;

        /// <summary>
        /// lablel sign.
        /// </summary>
        private UILabel lblSign;

        /// <summary>
        /// Used to determine rectangle that needs to be redrawn.
        /// </summary>
        private nfloat minX, minY, maxX, maxY;

        /// <summary>
        /// List of paths.
        /// </summary>
        private List<UIBezierPath> paths = new List<UIBezierPath>();

        /// <summary>
        /// List of point.
        /// </summary>
        private List<CGPoint[]> points;

        /// <summary>
        /// Label signature.
        /// </summary>
        private UIView signatureLine;

        /// <summary>
        /// Stroke color.
        /// </summary>
        private UIColor strokeColor;

        /// <summary>
        /// Stroke width.
        /// </summary>
        private float strokeWidth;

        /// <summary>
        /// Label X.
        /// </summary>
        private UILabel xLabel;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public SignaturePad()
        {
            Initialize();
        }

        /// <summary>
        /// Constructor with coder.
        /// </summary>
        /// <param name="coder">Coder to use.</param>
        public SignaturePad(NSCoder coder)
            : base(coder)
        {
            Initialize();
        }

        /// <summary>
        /// Constructor with pointer.
        /// </summary>
        /// <param name="ptr">Pointer to use.</param>
        public SignaturePad(IntPtr ptr)
            : base(ptr)
        {
            Initialize();
        }

        /// <summary>
        /// Constructor with frame.
        /// </summary>
        /// <param name="frame">Frame to use.</param>
        public SignaturePad(CGRect frame)
        {
            Frame = frame;
            Initialize();
        }

        /// <summary>
        ///  An image view that may be used as a watermark or as a texture
        ///  for the signature pad.
        /// </summary>
        /// <value>The background image view.</value>
        public UIImageView BackgroundImageView { get; private set; }

        /// <summary>
        /// The caption displayed under the signature line.
        /// </summary>
        /// <remarks>
        /// Text value defaults to 'Sign here.'
        /// </remarks>
        /// <value>The caption.</value>
        public UILabel Caption
        {
            get
            {
                return lblSign;
            }

            set
            {
                lblSign = value;
            }
        }

        /// <summary>
        /// Flag when is blank.
        /// </summary>
        public bool IsBlank
        {
            get
            {
                return points == null || points.Count() == 0 || !(points.Where(p => p.Any()).Any());
            }
        }

        /// <summary>
        /// Create an array containing all of the points used to draw the signature.  Uses CGPoint.Empty
        /// to indicate a new line.
        /// </summary>
        public CGPoint[] Points
        {
            get
            {
                if (points == null || points.Count() == 0)
                {
                    return new CGPoint[0];
                }

                IEnumerable<CGPoint> pointsList = points[0];

                for (var i = 1; i < points.Count; i++)
                {
                    pointsList = pointsList.Concat(new[] { CGPoint.Empty });
                    pointsList = pointsList.Concat(points[i]);
                }

                return pointsList.ToArray();
            }
        }

        /// <summary>
        /// Delegate when the is blank changed.
        /// </summary>
        public Action RaiseIsBlankChangedDelegate { get; set; }

        /// <summary>
        /// Gets the horizontal line that goes in the lower part of the pad.
        /// </summary>
        /// <value>The signature line.</value>
        public UIView SignatureLine
        {
            get
            {
                return signatureLine;
            }
        }

        /// <summary>
        /// The color of the signature line.
        /// </summary>
        /// <value>The color of the signature line.</value>
        public UIColor SignatureLineColor
        {
            get
            {
                return signatureLine.BackgroundColor;
            }

            set
            {
                signatureLine.BackgroundColor = value;
            }
        }

        /// <summary>
        /// The prompt displayed at the beginning of the signature line.
        /// </summary>
        /// <remarks>
        /// Text value defaults to 'X'.
        /// </remarks>
        /// <value>The signature prompt.</value>
        public UILabel SignaturePrompt
        {
            get
            {
                return xLabel;
            }

            set
            {
                xLabel = value;
            }
        }

        /// <summary>
        /// Stroke color.
        /// </summary>
        public UIColor StrokeColor
        {
            get
            {
                return strokeColor;
            }

            set
            {
                strokeColor = value ?? strokeColor;
                if (!IsBlank)
                {
                    LoadNewImage();
                }
            }
        }

        /// <summary>
        /// Stroke width.
        /// </summary>
        public float StrokeWidth
        {
            get
            {
                return strokeWidth;
            }

            set
            {
                strokeWidth = value;
                if (!IsBlank)
                {
                    LoadNewImage();
                }
            }
        }

        /// <summary>
        /// Bitmap buffer off by default since memory is a limited resource.
        /// </summary>
        public bool UseBitmapBuffer { get; set; }

        /// <summary>
        /// Delete the current signature.
        /// </summary>
        public void Clear()
        {
            paths = new List<UIBezierPath>();
            currentPath = UIBezierPath.Create();
            points = new List<CGPoint[]>();
            currentPoints.Clear();

            if (UseBitmapBuffer)
            {
                if (imageView.Image != null)
                {
                    imageView.Image.Dispose();
                }

                imageView.Image = null;
            }

            SetNeedsDisplay();
            NotifyIsBlankChanged();
        }

        /// <summary>
        /// Draw view.
        /// </summary>
        /// <param name="rect">Rect to use.</param>
        public override void Draw(CGRect rect)
        {
            strokeColor.SetStroke();

            if (!UseBitmapBuffer)
            {
                UIBezierPath drawPath = null;
                for (int i = 0; i < paths.Count; i++)
                {
                    drawPath = paths[i];

                    if (drawPath != null)
                    {
                        drawPath.Stroke();
                    }
                }
            }

            if (currentPath == null || currentPath.Empty)
            {
                return;
            }

            currentPath.Stroke();
        }

        /// <summary>
        /// Create a image of the currently drawn signature.
        /// </summary>
        /// <param name="shouldCrop">Should crop.</param>
        /// <param name="keepAspectRatio">Keep ratio.</param>
        /// <returns>New bitmap.</returns>
        public UIImage GetImage(bool shouldCrop = true, bool keepAspectRatio = true)
        {
            return GetImage(strokeColor,
                UIColor.Clear,
                GetSizeFromScale(UIScreen.MainScreen.Scale, Bounds),
                UIScreen.MainScreen.Scale,
                shouldCrop,
                keepAspectRatio);
        }

        /// <summary>
        /// Create a image of the currently drawn signature.
        /// </summary>
        /// <param name="size">Size to use.</param>
        /// <param name="shouldCrop">Should crop.</param>
        /// <param name="keepAspectRatio">Keep ratio.</param>
        /// <returns>New bitmap.</returns>
        public UIImage GetImage(CGSize size, bool shouldCrop = true, bool keepAspectRatio = true)
        {
            return GetImage(strokeColor, UIColor.Clear, size, GetScaleFromSize(size, Bounds), shouldCrop, keepAspectRatio);
        }

        /// <summary>
        /// Create a UIImage of the currently drawn signature.
        /// </summary>
        /// <param name="scale">Scale to use.</param>
        /// <param name="shouldCrop">Should crop.</param>
        /// <param name="keepAspectRatio">Keep ratio.</param>
        /// <returns>New bitmap.</returns>
        public UIImage GetImage(nfloat scale, bool shouldCrop = true, bool keepAspectRatio = true)
        {
            return GetImage(strokeColor, UIColor.Clear, GetSizeFromScale(scale, Bounds), scale, shouldCrop, keepAspectRatio);
        }

        /// <summary>
        /// Create a UIImage of the currently drawn signature.
        /// </summary>
        /// <param name="strokeColor">Color to use.</param>
        /// <param name="shouldCrop">Should crop.</param>
        /// <param name="keepAspectRatio">Keep ratio.</param>
        /// <returns>New bitmap.</returns>
        public UIImage GetImage(UIColor strokeColor, bool shouldCrop = true, bool keepAspectRatio = true)
        {
            return GetImage(
                strokeColor,
                UIColor.Clear,
                GetSizeFromScale(UIScreen.MainScreen.Scale, Bounds),
                UIScreen.MainScreen.Scale,
                shouldCrop,
                keepAspectRatio);
        }

        /// <summary>
        /// Create a UIImage of the currently drawn signature.
        /// </summary>
        /// <param name="strokeColor">Color to use.</param>
        /// <param name="size">Size to use.</param>
        /// <param name="shouldCrop">Should crop.</param>
        /// <param name="keepAspectRatio">Keep ratio.</param>
        /// <returns>New bitmap.</returns>
        public UIImage GetImage(UIColor strokeColor, CGSize size, bool shouldCrop = true, bool keepAspectRatio = true)
        {
            return GetImage(strokeColor, UIColor.Clear, size, GetScaleFromSize(size, Bounds), shouldCrop, keepAspectRatio);
        }

        /// <summary>
        /// Create a UIImage of the currently drawn signature.
        /// </summary>
        /// <param name="strokeColor">Color to use.</param>
        /// <param name="scale">Scale to use.</param>
        /// <param name="shouldCrop">Should crop.</param>
        /// <param name="keepAspectRatio">Keep ratio.</param>
        /// <returns>New bitmap.</returns>
        public UIImage GetImage(UIColor strokeColor, nfloat scale, bool shouldCrop = true, bool keepAspectRatio = true)
        {
            return GetImage(strokeColor, UIColor.Clear, GetSizeFromScale(scale, Bounds), scale, shouldCrop, keepAspectRatio);
        }

        /// <summary>
        /// Create a UIImage of the currently drawn signature.
        /// </summary>
        /// <param name="strokeColor">Color to use.</param>
        /// <param name="fillColor">Fill color.</param>
        /// <param name="shouldCrop">Should crop.</param>
        /// <param name="keepAspectRatio">Keep ratio.</param>
        /// <returns>New bitmap.</returns>
        public UIImage GetImage(UIColor strokeColor, UIColor fillColor, bool shouldCrop = true, bool keepAspectRatio = true)
        {
            return GetImage(
                strokeColor,
                fillColor,
                GetSizeFromScale(UIScreen.MainScreen.Scale, Bounds),
                UIScreen.MainScreen.Scale,
                shouldCrop,
                keepAspectRatio);
        }

        /// <summary>
        /// Create a UIImage of the currently drawn signature.
        /// </summary>
        /// <param name="strokeColor">Color to use.</param>
        /// <param name="fillColor">Fill color.</param>
        /// <param name="size">Size to use.</param>
        /// <param name="shouldCrop">Should crop.</param>
        /// <param name="keepAspectRatio">Keep ratio.</param>
        /// <returns>New bitmap.</returns>
        public UIImage GetImage(UIColor strokeColor, UIColor fillColor, CGSize size, bool shouldCrop = true, bool keepAspectRatio = true)
        {
            return GetImage(strokeColor, fillColor, size, GetScaleFromSize(size, Bounds), shouldCrop, keepAspectRatio);
        }

        /// <summary>
        /// Create a UIImage of the currently drawn signature.
        /// </summary>
        /// <param name="strokeColor">Color to use.</param>
        /// <param name="fillColor">Fill color.</param>
        /// <param name="scale">Scale to use.</param>
        /// <param name="shouldCrop">Should crop.</param>
        /// <param name="keepAspectRatio">Keep ratio.</param>
        /// <returns>New bitmap.</returns>
        public UIImage GetImage(UIColor strokeColor, UIColor fillColor, nfloat scale, bool shouldCrop = true, bool keepAspectRatio = true)
        {
            return GetImage(strokeColor, fillColor, GetSizeFromScale(scale, Bounds), scale, shouldCrop, keepAspectRatio);
        }

        /// <summary>
        /// Get the points in string format.
        /// </summary>
        /// <returns>String with the point.</returns>
        public async Task<string> GetPointsStringAsync()
        {
            await Task.FromResult(0);
            string result = null;

            List<List<DrawPoint>> linesList = new List<List<DrawPoint>>();

            // The first child is always the current drawing path, and should be ignored.
            if ((points != null) && (points.Count > 1))
            {
                List<DrawPoint> pointLine;
                CGPoint[] line;
                CGPoint point;

                for (int i = 0; i < points.Count; i++)
                {
                    line = points[i];

                    if ((line != null) && (line.Length > 0))
                    {
                        pointLine = new List<DrawPoint>();
                        for (int j = 0; j < line.Length; j++)
                        {
                            point = line[j];
                            pointLine.Add(new DrawPoint(point.X, point.Y));
                        }

                        linesList.Add(pointLine);
                    }
                }

                result = await SignatureSerializer.SerializeAsync(linesList);
            }

            return result;
        }

        /// <summary>
        /// Layout views.
        /// </summary>
        public override void LayoutSubviews()
        {
            lblSign.SizeToFit();
            xLabel.SizeToFit();

            imageView.Frame = new CGRect(0, 0, Bounds.Width, Bounds.Height);

            lblSign.Frame = new CGRect(
                (Bounds.Width / 2) - (lblSign.Frame.Width / 2),
                Bounds.Height - lblSign.Frame.Height - 3,
                lblSign.Frame.Width,
                lblSign.Frame.Height);

            signatureLine.Frame = new CGRect(10, Bounds.Height - signatureLine.Frame.Height - 5 - lblSign.Frame.Height, Bounds.Width - 20, 1);

            xLabel.Frame = new CGRect(
                10,
                Bounds.Height - xLabel.Frame.Height - signatureLine.Frame.Height - 2 - lblSign.Frame.Height,
                xLabel.Frame.Width,
                xLabel.Frame.Height);
        }

        /// <summary>
        /// Allow the user to import an array of points to be used to draw a signature in the view, with new
        /// lines indicated by a CGPoint.Empty in the array.
        /// </summary>
        /// <param name="loadedPoints">Point to use.</param>
        public void LoadPoints(CGPoint[] loadedPoints)
        {
            if (loadedPoints == null || loadedPoints.Count() == 0)
                return;

            var startIndex = 0;
            var emptyIndex = loadedPoints.ToList().IndexOf(CGPoint.Empty);

            if (emptyIndex == -1)
            {
                emptyIndex = loadedPoints.Count();
            }

            //Clear any existing paths or points.
            paths = new List<UIBezierPath>();
            points = new List<CGPoint[]>();

            do
            {
                // Create a new path and set the line options
                currentPath = UIBezierPath.Create();
                currentPath.LineWidth = StrokeWidth;
                currentPath.LineJoinStyle = CGLineJoin.Round;

                currentPoints = new List<CGPoint>();

                // Move to the first point and add that point to the current_points array.
                currentPath.MoveTo(loadedPoints[startIndex]);
                currentPoints.Add(loadedPoints[startIndex]);

                // Iterate through the array until an empty point (or the end of the array) is reached,
                // adding each point to the current_path and to the current_points array.
                for (var i = startIndex + 1; i < emptyIndex; i++)
                {
                    currentPath.AddLineTo(loadedPoints[i]);
                    currentPoints.Add(loadedPoints[i]);
                }

                // Add the current_path and current_points list to their respective Lists before
                // starting on the next line to be drawn.
                paths.Add(currentPath);
                points.Add(currentPoints.ToArray());

                // Obtain the indices for the next line to be drawn.
                startIndex = emptyIndex + 1;
                if (startIndex < loadedPoints.Count() - 1)
                {
                    emptyIndex = loadedPoints.ToList().IndexOf(CGPoint.Empty, startIndex);

                    if (emptyIndex == -1)
                    {
                        emptyIndex = loadedPoints.Count();
                    }
                }
                else
                {
                    emptyIndex = startIndex;
                }
            } while (startIndex < emptyIndex);

            LoadNewImage();

            // Display the clear button.
            SetNeedsDisplay();
            NotifyIsBlankChanged();
        }

        /// <summary>
        /// Begin touches.
        /// </summary>
        /// <param name="touches">Touches to use.</param>
        /// <param name="evt">Event to use.</param>
        public override void TouchesBegan(Foundation.NSSet touches, UIEvent evt)
        {
            if (currentPath != null)
            {
                // Obtain the smoothed path and the points array for that path.
                currentPath = SmoothedPathWithGranularity(0, out currentPoints);

                // Add the smoothed path and points array to their Lists.
                paths.Add(currentPath);
                points.Add(currentPoints.ToArray());

                LoadNewImage();
                SetNeedsDisplay();
                NotifyIsBlankChanged();

                currentPath = null;
                currentPoints.Clear();
            }

            // Create a new path and set the options.
            currentPath = UIBezierPath.Create();
            currentPath.LineWidth = StrokeWidth;
            currentPath.LineJoinStyle = CGLineJoin.Round;

            currentPoints.Clear();

            UITouch touch = touches.AnyObject as UITouch;

            // Obtain the location of the touch, move the path to that position and add it to the
            // current_points array.
            CGPoint touchLocation = touch.LocationInView(this);
            currentPath.MoveTo(touchLocation);
            currentPoints.Add(touchLocation);

            ResetBounds(touchLocation);
        }

        /// <summary>
        /// Touches ended.
        /// </summary>
        /// <param name="touches">Touches to use.</param>
        /// <param name="evt">Event to use.</param>
        public override void TouchesEnded(NSSet touches, UIEvent evt)
        {
            UITouch touch = touches.AnyObject as UITouch;

            // Obtain the location of the touch and add it to the current path and current_points array.
            CGPoint touchLocation = touch.LocationInView(this);
            currentPath.AddLineTo(touchLocation);
            currentPoints.Add(touchLocation);

            // Obtain the smoothed path and the points array for that path.
            currentPath = SmoothedPathWithGranularity(0, out currentPoints);

            // Add the smoothed path and points array to their Lists.
            paths.Add(currentPath);
            points.Add(currentPoints.ToArray());

            LoadNewImage();

            UpdateBounds(touchLocation);
            SetNeedsDisplay();
            NotifyIsBlankChanged();

            currentPath = null;
            currentPoints.Clear();
        }

        /// <summary>
        /// Touches moved.
        /// </summary>
        /// <param name="touches">Touches to use.</param>
        /// <param name="evt">Event to use.</param>
        public override void TouchesMoved(Foundation.NSSet touches, UIEvent evt)
        {
            if (currentPath != null)
            {
                UITouch touch = touches.AnyObject as UITouch;

                //Obtain the location of the touch and add it to the current path and current_points array.
                CGPoint touchLocation = touch.LocationInView(this);
                currentPath.AddLineTo(touchLocation);
                currentPoints.Add(touchLocation);

                UpdateBounds(touchLocation);
                SetNeedsDisplayInRect(new CGRect(minX, minY, (nfloat)Math.Abs(maxX - minX), (nfloat)Math.Abs(maxY - minY)));
            }
        }

        /// <summary>
        /// Notify the is blank changed.
        /// </summary>
        protected void NotifyIsBlankChanged()
        {
            if (RaiseIsBlankChangedDelegate != null)
            {
                RaiseIsBlankChangedDelegate();
            }
        }

        /// <summary>
        /// Get cropped rectange.
        /// </summary>
        /// <param name="cachedPoints">Chached points.</param>
        /// <returns>Rectangle for the crop.</returns>
        private CGRect GetCroppedRectangle(CGPoint[] cachedPoints)
        {
            var xMin = cachedPoints.Where(point => !point.IsEmpty).Min(point => point.X) - strokeWidth / 2;
            var xMax = cachedPoints.Where(point => !point.IsEmpty).Max(point => point.X) + strokeWidth / 2;
            var yMin = cachedPoints.Where(point => !point.IsEmpty).Min(point => point.Y) - strokeWidth / 2;
            var yMax = cachedPoints.Where(point => !point.IsEmpty).Max(point => point.Y) + strokeWidth / 2;

            xMin = (nfloat)Math.Max(xMin, 0);
            xMax = (nfloat)Math.Min(xMax, Bounds.Width);
            yMin = (nfloat)Math.Max(yMin, 0);
            yMax = (nfloat)Math.Min(yMax, Bounds.Height);

            return new CGRect(xMin, yMin, xMax - xMin, yMax - yMin);
        }

        /// <summary>
        /// Create a UIImage of the currently drawn signature.
        /// </summary>
        /// <param name="strokeColor">Color to use.</param>
        /// <param name="fillColor">Fill color.</param>
        /// <param name="size">Size to use.</param>
        /// <param name="scale">Scale to use.</param>
        /// <param name="shouldCrop">Should crop.</param>
        /// <param name="keepAspectRatio">Keep ratio.</param>
        /// <returns>New bitmap.</returns>
        private UIImage GetImage(UIColor strokeColor, UIColor fillColor, CGSize size, nfloat scale, bool shouldCrop = true, bool keepAspectRatio = true)
        {
            if (size.Width == 0 || size.Height == 0 || scale <= 0 || strokeColor == null || fillColor == null)
            {
                return null;
            }

            nfloat uncroppedScale;
            CGRect croppedRectangle = new CGRect();

            CGPoint[] cachedPoints;

            if (shouldCrop && (cachedPoints = Points).Any())
            {
                croppedRectangle = GetCroppedRectangle(cachedPoints);
                croppedRectangle.Width /= scale;
                croppedRectangle.Height /= scale;
                if (croppedRectangle.X >= 5)
                {
                    croppedRectangle.X -= 5;
                    croppedRectangle.Width += 5;
                }

                if (croppedRectangle.Y >= 5)
                {
                    croppedRectangle.Y -= 5;
                    croppedRectangle.Height += 5;
                }

                if (croppedRectangle.X + croppedRectangle.Width <= size.Width - 5)
                {
                    croppedRectangle.Width += 5;
                }

                if (croppedRectangle.Y + croppedRectangle.Height <= size.Height - 5)
                {
                    croppedRectangle.Height += 5;
                }

                nfloat scaleX = croppedRectangle.Width / Bounds.Width;
                nfloat scaleY = croppedRectangle.Height / Bounds.Height;
                uncroppedScale = 1 / (nfloat)Math.Max(scaleX, scaleY);
            }
            else
            {
                uncroppedScale = scale;
            }

            // Make sure the image is scaled to the screen resolution in case of Retina display.
            if (keepAspectRatio)
            {
                UIGraphics.BeginImageContext(size);
            }
            else
            {
                UIGraphics.BeginImageContext(new CGSize(croppedRectangle.Width * uncroppedScale, croppedRectangle.Height * uncroppedScale));
            }

            // Create context and set the desired options
            CGContext context = UIGraphics.GetCurrentContext();
            context.SetFillColor(fillColor.CGColor);
            context.FillRect(new CGRect(0, 0, size.Width, size.Height));
            context.SetStrokeColor(strokeColor.CGColor);
            context.SetLineWidth(StrokeWidth);
            context.SetLineCap(CGLineCap.Round);
            context.SetLineJoin(CGLineJoin.Round);
            context.ScaleCTM(uncroppedScale, uncroppedScale);

            // Obtain all drawn paths from the array
            foreach (var bezierPath in paths)
            {
                var tempPath = (UIBezierPath)bezierPath.Copy();
                if (shouldCrop)
                {
                    tempPath.ApplyTransform(CGAffineTransform.MakeTranslation(-croppedRectangle.X, -croppedRectangle.Y));
                }

                CGPath path = tempPath.CGPath;
                context.AddPath(path);
                tempPath = null;
            }
            context.StrokePath();

            UIImage image = UIGraphics.GetImageFromCurrentImageContext();

            UIGraphics.EndImageContext();

            return image;
        }

        /// <summary>
        /// Get scale from size.
        /// </summary>
        /// <param name="size">Size to use.</param>
        /// <param name="width">Width to use.</param>
        /// <param name="height">Height to use.</param>
        /// <returns>Scale to use.</returns>
        private nfloat GetScaleFromSize(CGSize size, CGRect rectangle)
        {
            nfloat scaleX = size.Width / rectangle.Width;
            nfloat scaleY = size.Height / rectangle.Height;

            return (nfloat)Math.Min(scaleX, scaleY);
        }

        /// <summary>
        /// Get size from scale.
        /// </summary>
        /// <param name="scale">Scale to use.</param>
        /// <param name="inWidth">Width to use.</param>
        /// <param name="inHeight">Height to use.</param>
        /// <returns>New size.</returns>
        private CGSize GetSizeFromScale(nfloat scale, CGRect rectangle)
        {
            nfloat width = rectangle.Width * scale;
            nfloat height = rectangle.Height * scale;

            return new CGSize(width, height);
        }

        /// <summary>
        /// Initialze view.
        /// </summary>
        private void Initialize()
        {
            BackgroundColor = UIColor.FromRGB(225, 225, 225);
            strokeColor = UIColor.Black;
            StrokeWidth = 2f;

            Layer.ShadowColor = UIColor.Black.CGColor;
            Layer.ShadowOffset = new CGSize(2, 2);
            Layer.ShadowOpacity = 1f;
            Layer.ShadowRadius = 2f;

            BackgroundImageView = new UIImageView();
            AddSubview(BackgroundImageView);

            // Add an image that covers the entire signature view, used to display already drawn
            // elements instead of having to redraw them every time the user touches the screen.
            imageView = new UIImageView();
            AddSubview(imageView);

            lblSign = new UILabel();
            lblSign.Text = "Sign here.";
            lblSign.Font = UIFont.BoldSystemFontOfSize(11f);
            lblSign.BackgroundColor = UIColor.Clear;
            lblSign.TextColor = UIColor.Gray;
            AddSubview(lblSign);

            // Display the base line for the user to sign on.
            signatureLine = new UIView();
            signatureLine.BackgroundColor = UIColor.Gray;
            AddSubview(signatureLine);

            // Display the X on the left hand side of the line where the user signs.
            xLabel = new UILabel();
            xLabel.Text = "X";
            xLabel.Font = UIFont.BoldSystemFontOfSize(20f);
            xLabel.BackgroundColor = UIColor.Clear;
            xLabel.TextColor = UIColor.Gray;
            AddSubview(xLabel);

            paths = new List<UIBezierPath>();
            points = new List<CGPoint[]>();
            currentPoints = new List<CGPoint>();
        }

        /// <summary>
        /// Load new image.
        /// </summary>
        private void LoadNewImage()
        {
            if (UseBitmapBuffer)
            {
                if (imageView.Image != null)
                {
                    imageView.Image.Dispose();
                }

                imageView.Image = GetImage(false);
            }
        }

        /// <summary>
        /// Set the bounds for the rectangle that will need to be redrawn to show the drawn path.
        /// </summary>
        /// <param name="point">Point to use.</param>
        private void ResetBounds(CGPoint point)
        {
            minX = point.X - 1;
            maxX = point.X + 1;
            minY = point.Y - 1;
            maxY = point.Y + 1;
        }

        /// <summary>
        /// Obtain a smoothed path with the specified granularity from the current path using Catmull-Rom spline.
        /// Implemented using a modified version of the code in the solution at
        /// http://stackoverflow.com/questions/8702696/drawing-smooth-curves-methods-needed.
        /// Also outputs a List of the points corresponding to the smoothed path.
        ///
        /// </summary>
        /// <param name="granularity">Granularity to use.</param>
        /// <param name="smoothedPoints">Smoothed points.</param>
        /// <returns>Path to use.</returns>
        private UIBezierPath SmoothedPathWithGranularity(int granularity, out List<CGPoint> smoothedPoints)
        {
            List<CGPoint> pointsArray = currentPoints;
            smoothedPoints = new List<CGPoint>();

            // Not enough points to smooth effectively, so return the original path and points.
            if (pointsArray.Count < 4)
            {
                smoothedPoints = pointsArray;
                return currentPath;
            }

            // Create a new bezier path to hold the smoothed path.
            UIBezierPath smoothedPath = UIBezierPath.Create();
            smoothedPath.LineWidth = StrokeWidth;
            smoothedPath.LineJoinStyle = CGLineJoin.Round;

            // Duplicate the first and last points as control points.
            pointsArray.Insert(0, pointsArray[0]);
            pointsArray.Add(pointsArray[pointsArray.Count - 1]);

            // Add the first point
            smoothedPath.MoveTo(pointsArray[0]);
            smoothedPoints.Add(pointsArray[0]);

            for (var index = 1; index < pointsArray.Count - 2; index++)
            {
                CGPoint p0 = pointsArray[index - 1];
                CGPoint p1 = pointsArray[index];
                CGPoint p2 = pointsArray[index + 1];
                CGPoint p3 = pointsArray[index + 2];

                // Add n points starting at p1 + dx/dy up until p2 using Catmull-Rom splines
                for (var i = 1; i < granularity; i++)
                {
                    float t = (float)i * (1f / (float)granularity);
                    float tt = t * t;
                    float ttt = tt * t;

                    // Intermediate point
                    CGPoint mid = default(CGPoint);
                    mid.X = 0.5f * (2f * p1.X + (p2.X - p0.X) * t +
                        (2f * p0.X - 5f * p1.X + 4f * p2.X - p3.X) * tt +
                        (3f * p1.X - p0.X - 3f * p2.X + p3.X) * ttt);
                    mid.Y = 0.5f * (2 * p1.Y + (p2.Y - p0.Y) * t +
                        (2 * p0.Y - 5 * p1.Y + 4 * p2.Y - p3.Y) * tt +
                        (3 * p1.Y - p0.Y - 3 * p2.Y + p3.Y) * ttt);

                    smoothedPath.AddLineTo(mid);
                    smoothedPoints.Add(mid);
                }

                // Add p2
                smoothedPath.AddLineTo(p2);
                smoothedPoints.Add(p2);
            }

            // Add the last point
            smoothedPath.AddLineTo(pointsArray[pointsArray.Count - 1]);
            smoothedPoints.Add(pointsArray[pointsArray.Count - 1]);

            return smoothedPath;
        }

        /// <summary>
        /// Update the bounds for the rectangle to be redrawn if necessary for the given point.
        /// </summary>
        /// <param name="point">Point to use.</param>
        private void UpdateBounds(CGPoint point)
        {
            if (point.X < minX + 1)
            {
                minX = point.X - 1;
            }
            if (point.X > maxX - 1)
            {
                maxX = point.X + 1;
            }
            if (point.Y < minY + 1)
            {
                minY = point.Y - 1;
            }
            if (point.Y > maxY - 1)
            {
                maxY = point.Y + 1;
            }
        }
    }
}