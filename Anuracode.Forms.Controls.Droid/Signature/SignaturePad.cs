// <copyright file="SignaturePadView.cs" company="">
// All rights reserved.
// </copyright>

using Android.Content;
using Android.Graphics;
using Android.Util;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Anuracode.Forms.Controls.Renderers
{
    /// <summary>
    /// Signature pad view.
    /// </summary>
    public class SignaturePad : RelativeLayout
    {
        /// <summary>
        /// The color of the signature line.
        /// </summary>
        /// <value>The color of the signature line.</value>
        protected Color signatureLineColor;

        /// <summary>
        /// Random seed.
        /// </summary>
        private static Random rndId = new Random();

        /// <summary>
        /// Background color.
        /// </summary>
        private Color backgroundColor;

        /// <summary>
        /// Canvas view.
        /// </summary>
        private SignatureCanvasView canvasView;

        /// <summary>
        /// Context to use.
        /// </summary>
        private Context context;

        /// <summary>
        /// Current points.
        /// </summary>
        private List<System.Drawing.PointF> currentPoints;

        /// <summary>
        /// Used to determine rectangle that needs to be redrawn.
        /// </summary>
        private RectF dirtyRect;

        /// <summary>
        /// Clear image view.
        /// </summary>
        private ClearingImageView imageView;

        /// <summary>
        /// Current path.
        /// </summary>
        private Path internalCurrentPath;

        /// <summary>
        /// Paoint to use.
        /// </summary>
        private Paint internalPaint;

        /// <summary>
        /// Last x.
        /// </summary>
        private float lastX;

        /// <summary>
        /// Last y.
        /// </summary>
        private float lastY;

        /// <summary>
        /// Label sign.
        /// </summary>
        private TextView lblSign;

        /// <summary>
        /// Paths to use.
        /// </summary>
        private List<Path> paths;

        /// <summary>
        /// Points to use.
        /// </summary>
        private List<System.Drawing.PointF[]> points;

        /// <summary>
        /// View signature.
        /// </summary>
        private View signatureLine;

        /// <summary>
        /// Gets or sets the color of the strokes for the signature.
        /// </summary>
        private Color strokeColor;

        /// <summary>
        /// Gets or sets the width in pixels of the strokes for the signature.
        /// </summary>
        private float strokeWidth;

        /// <summary>
        /// Bitmap buffer off by default since memory is a limited resource.
        /// </summary>
        private bool useBitmapBuffer = false;

        /// <summary>
        /// Label for the x.
        /// </summary>
        private TextView xLabel;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Context to use.</param>
        public SignaturePad(Context context)
            : base(context)
        {
            this.context = context;
            Initialize();
        }

        /// <summary>
        /// Constructor with attributes.
        /// </summary>
        /// <param name="context">Context to use.</param>
        /// <param name="attrs">Attributes to use.</param>
        public SignaturePad(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
            this.context = context;
            Initialize();
        }

        /// <summary>
        /// Constructor with attributes.
        /// </summary>
        /// <param name="context">Context to use.</param>
        /// <param name="attrs">Attributes to use.</param>
        /// <param name="defStyle">Style to use.</param>
        public SignaturePad(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        {
            this.context = context;
            Initialize();
        }

        /// <summary>
        /// Background color.
        /// </summary>
        public Color BackgroundColor
        {
            get
            {
                return backgroundColor;
            }

            set
            {
                backgroundColor = value;
                SetBackgroundColor(backgroundColor);
            }
        }

        /// <summary>
        /// Gets the background image view.
        /// </summary>
        /// <value>The background image view.</value>
        public ImageView BackgroundImageView { get; private set; }

        /// <summary>
        /// The caption displayed under the signature line.
        /// </summary>
        /// <remarks>
        /// Text value defaults to 'Sign here.'
        /// </remarks>
        /// <value>The caption.</value>
        public TextView Caption
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
        /// Property to check if its blank.
        /// </summary>
        public bool IsBlank
        {
            get
            {
                return points == null || points.Count() == 0 || !(points.Where(p => p.Any()).Any());
            }
        }

        /// <summary>
        /// Create an array containing all of the points used to draw the signature.  Uses null
        /// to indicate a new line.
        /// </summary>
        public System.Drawing.PointF[] Points
        {
            get
            {
                if (points == null || points.Count() == 0)
                    return new System.Drawing.PointF[0];

                IEnumerable<System.Drawing.PointF> pointsList = points[0];

                for (var i = 1; i < points.Count; i++)
                {
                    pointsList = pointsList.Concat(new[] { System.Drawing.PointF.Empty });
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
        public View SignatureLine
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
        public Color SignatureLineColor
        {
            get
            {
                return signatureLineColor;
            }

            set
            {
                signatureLineColor = value;
                signatureLine.SetBackgroundColor(value);
            }
        }

        /// <summary>
        /// The prompt displayed at the beginning of the signature line.
        /// </summary>
        /// <remarks>
        /// Text value defaults to 'X'.
        /// </remarks>
        /// <value>The signature prompt.</value>
        public TextView SignaturePrompt
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
        /// Gets or sets the color of the strokes for the signature.
        /// </summary>
        public Color StrokeColor
        {
            get
            {
                return strokeColor;
            }

            set
            {
                strokeColor = value;

                if (paint != null)
                    paint.Color = strokeColor;

                if (!IsBlank)
                    DrawStrokes();
            }
        }

        /// <summary>
        /// Gets or sets the width in pixels of the strokes for the signature.
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
                if (paint != null)
                {
                    paint.StrokeWidth = strokeWidth;
                }

                if (!IsBlank)
                {
                    DrawStrokes();
                }
            }
        }

        /// <summary>
        /// Bitmap buffer off by default since memory is a limited resource.
        /// </summary>
        public bool UseBitmapBuffer
        {
            get
            {
                return useBitmapBuffer;
            }

            set
            {
                useBitmapBuffer = value;

                if (useBitmapBuffer)
                {
                    DrawStrokes();
                }
                else
                {
                    imageView.SetImageBitmap(null);
                }
            }
        }

        /// <summary>
        /// Current path.
        /// </summary>
        protected Path currentPath
        {
            get
            {
                return internalCurrentPath;
            }
            set
            {
                canvasView.Path = internalCurrentPath = value;
            }
        }

        /// <summary>
        /// Point to use.
        /// </summary>
        protected Paint paint
        {
            get
            {
                return internalPaint;
            }

            set
            {
                canvasView.Paint = internalPaint = value;
            }
        }

        /// <summary>
        /// Delete the current signature.
        /// </summary>
        public void Clear()
        {
            paths = new List<Path>();
            points = new List<System.Drawing.PointF[]>();
            currentPoints = new List<System.Drawing.PointF>();
            currentPath = new Path();
            imageView.SetImageBitmap(null);
            GC.Collect();

            canvasView.Invalidate();
            Invalidate();
            NotifyIsBlankChanged();
        }

        /// <summary>
        /// Draw canvas.
        /// </summary>
        /// <param name="canvas">Canvas to use.</param>
        public override void Draw(Canvas canvas)
        {
            base.Draw(canvas);

            if (!UseBitmapBuffer)
            {
                //Bitmap not in use: redraw all of the paths.
                DrawStrokesOnCanvas(canvas, this.strokeColor, Color.Transparent, false);
            }
        }

        /// <summary>
        /// Create a UIImage of the currently drawn signature.
        /// </summary>
        /// <param name="shouldCrop">Should crop.</param>
        /// <param name="keepAspectRatio">Keep ratio.</param>
        /// <returns>New bitmap.</returns>
        public Bitmap GetImage(bool shouldCrop = true, bool keepAspectRatio = true)
        {
            return GetImage(strokeColor, Color.Transparent, new System.Drawing.SizeF(Width, Height), 1, shouldCrop, keepAspectRatio);
        }

        /// <summary>
        /// Create a image of the currently drawn signature.
        /// </summary>
        /// <param name="size">Size to use.</param>
        /// <param name="shouldCrop">Should crop.</param>
        /// <param name="keepAspectRatio">Keep ratio.</param>
        /// <returns>New bitmap.</returns>
        public Bitmap GetImage(System.Drawing.SizeF size, bool shouldCrop = true, bool keepAspectRatio = true)
        {
            return GetImage(strokeColor, Color.Transparent, size, GetScaleFromSize(size, Width, Height), shouldCrop, keepAspectRatio);
        }

        /// <summary>
        /// Create a UIImage of the currently drawn signature.
        /// </summary>
        /// <param name="scale">Scale to use.</param>
        /// <param name="shouldCrop">Should crop.</param>
        /// <param name="keepAspectRatio">Keep ratio.</param>
        /// <returns>New bitmap.</returns>
        public Bitmap GetImage(float scale, bool shouldCrop = true, bool keepAspectRatio = true)
        {
            return GetImage(
                strokeColor,
                Color.Transparent,
                GetSizeFromScale(scale, Width, Height),
                scale,
                shouldCrop,
                keepAspectRatio);
        }

        /// <summary>
        /// Create a UIImage of the currently drawn signature.
        /// </summary>
        /// <param name="strokeColor">Color to use.</param>
        /// <param name="shouldCrop">Should crop.</param>
        /// <param name="keepAspectRatio">Keep ratio.</param>
        /// <returns>New bitmap.</returns>
        public Bitmap GetImage(Color strokeColor, bool shouldCrop = true, bool keepAspectRatio = true)
        {
            return GetImage(
                strokeColor,
                Color.Transparent,
                new System.Drawing.SizeF(Width, Height),
                1,
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
        public Bitmap GetImage(Color strokeColor, System.Drawing.SizeF size, bool shouldCrop = true, bool keepAspectRatio = true)
        {
            return GetImage(
                strokeColor,
                Color.Transparent,
                size,
                GetScaleFromSize(size, Width, Height),
                shouldCrop,
                keepAspectRatio);
        }

        /// <summary>
        /// Create a UIImage of the currently drawn signature.
        /// </summary>
        /// <param name="strokeColor">Color to use.</param>
        /// <param name="scale">Scale to use.</param>
        /// <param name="shouldCrop">Should crop.</param>
        /// <param name="keepAspectRatio">Keep ratio.</param>
        /// <returns>New bitmap.</returns>
        public Bitmap GetImage(Color strokeColor, float scale, bool shouldCrop = true, bool keepAspectRatio = true)
        {
            return GetImage(
                strokeColor,
                Color.Transparent,
                GetSizeFromScale(scale, Width, Height),
                scale,
                shouldCrop,
                keepAspectRatio);
        }

        /// <summary>
        /// Create a UIImage of the currently drawn signature.
        /// </summary>
        /// <param name="strokeColor">Color to use.</param>
        /// <param name="fillColor">Fill color.</param>
        /// <param name="shouldCrop">Should crop.</param>
        /// <param name="keepAspectRatio">Keep ratio.</param>
        /// <returns>New bitmap.</returns>
        public Bitmap GetImage(Color strokeColor, Color fillColor, bool shouldCrop = true, bool keepAspectRatio = true)
        {
            return GetImage(
                strokeColor,
                fillColor,
                new System.Drawing.SizeF(Width, Height),
                1,
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
        public Bitmap GetImage(Color strokeColor, Color fillColor, System.Drawing.SizeF size,
                                bool shouldCrop = true, bool keepAspectRatio = true)
        {
            return GetImage(
                strokeColor,
                fillColor,
                size,
                GetScaleFromSize(size, Width, Height),
                shouldCrop,
                keepAspectRatio);
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
        public Bitmap GetImage(Color strokeColor, Color fillColor, float scale, bool shouldCrop = true, bool keepAspectRatio = true)
        {
            return GetImage(
                strokeColor,
                fillColor,
                GetSizeFromScale(scale, Width, Height),
                scale,
                shouldCrop,
                keepAspectRatio);
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
                System.Drawing.PointF[] line;
                System.Drawing.PointF point;

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
        /// Allow the user to import an array of points to be used to draw a signature in the view, with new
        /// lines indicated by a System.Drawing.PointF.Empty in the array.
        /// </summary>
        /// <param name="loadedPoints">Point to use.</param>
        public void LoadPoints(System.Drawing.PointF[] loadedPoints)
        {
            if (loadedPoints == null || loadedPoints.Count() == 0)
            {
                return;
            }

            var startIndex = 0;
            var emptyIndex = loadedPoints.ToList().IndexOf(System.Drawing.PointF.Empty);

            if (emptyIndex == -1)
            {
                emptyIndex = loadedPoints.Count();
            }

            //Clear any existing paths or points.
            paths = new List<Path>();
            points = new List<System.Drawing.PointF[]>();

            do
            {
                //Create a new path and set the line options
                currentPath = new Path();
                currentPoints = new List<System.Drawing.PointF>();

                //Move to the first point and add that point to the current_points array.
                currentPath.MoveTo(loadedPoints[startIndex].X, loadedPoints[startIndex].Y);
                currentPoints.Add(loadedPoints[startIndex]);

                //Iterate through the array until an empty point (or the end of the array) is reached,
                //adding each point to the current_path and to the current_points array.
                for (var i = startIndex + 1; i < emptyIndex; i++)
                {
                    currentPath.LineTo(loadedPoints[i].X, loadedPoints[i].Y);
                    currentPoints.Add(loadedPoints[i]);
                }

                //Add the current_path and current_points list to their respective Lists before
                //starting on the next line to be drawn.
                paths.Add(currentPath);
                points.Add(currentPoints.ToArray());

                //Obtain the indices for the next line to be drawn.
                startIndex = emptyIndex + 1;
                if (startIndex < loadedPoints.Count() - 1)
                {
                    emptyIndex = loadedPoints.ToList().IndexOf(System.Drawing.PointF.Empty, startIndex);

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

            DrawStrokes();

            //Display the clear button.
            Invalidate();
            NotifyIsBlankChanged();
        }

        /// <summary>
        /// On touch event.
        /// </summary>
        /// <param name="e">Arguments of the event.</param>
        /// <returns>Flag to use.</returns>
        public override bool OnTouchEvent(MotionEvent e)
        {
            float touchX = e.GetX();
            float touchY = e.GetY();

            System.Drawing.PointF touch = new System.Drawing.PointF(touchX, touchY);

            switch (e.Action)
            {
                case MotionEventActions.Down:
                    lastX = touchX;
                    lastY = touchY;

                    //Create a new path and move to the touched point.
                    currentPath = new Path();
                    currentPath.MoveTo(touchX, touchY);

                    //Clear the list of points then add the touched point
                    currentPoints.Clear();
                    currentPoints.Add(touch);

                    return true;

                case MotionEventActions.Move:
                    HandleTouch(e);
                    canvasView.Invalidate(
                        (int)(dirtyRect.Left - 1),
                        (int)(dirtyRect.Top - 1),
                        (int)(dirtyRect.Right + 1),
                        (int)(dirtyRect.Bottom + 1));
                    break;

                case MotionEventActions.Up:
                    HandleTouch(e);
                    currentPath = SmoothedPathWithGranularity(0, out currentPoints);

                    // Add the current path and points to their respective lists.
                    paths.Add(currentPath);
                    points.Add(currentPoints.ToArray());

                    DrawStrokes();
                    canvasView.Invalidate();
                    NotifyIsBlankChanged();
                    break;

                default:
                    return false;
            }

            lastX = touchX;
            lastY = touchY;

            return true;
        }

        /// <summary>
        /// Generate id.
        /// </summary>
        /// <returns>New Id.</returns>
        protected int GenerateId()
        {
            int id;
            for (; ; )
            {
                id = rndId.Next(1, 0x00FFFFFF);
                if (FindViewById<View>(id) != null)
                {
                    continue;
                }
                return id;
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
        /// Draw strokes.
        /// </summary>
        private void DrawStrokes()
        {
            if (UseBitmapBuffer)
            {
                //Get an image of the current signature and display it so that the entire set of paths
                //doesn't have to be redrawn every time.
                this.imageView.SetImageBitmap(this.GetImage(false));
            }
            else
            {
                Invalidate();
            }
        }

        /// <summary>
        /// Draw strokes on canvas.
        /// </summary>
        /// <param name="canvas">Canvas to use.</param>
        /// <param name="strokeColor">Stroke color.</param>
        /// <param name="fillColor">Fill color.</param>
        /// <param name="shouldCrop">Should crop.</param>
        /// <param name="croppedRectangle">Crop rectangle.</param>
        private void DrawStrokesOnCanvas(Canvas canvas, Color strokeColor, Color fillColor, bool shouldCrop, RectF croppedRectangle = null)
        {
            canvas.DrawColor(fillColor);

            paint.Color = strokeColor;

            foreach (var path in paths)
            {
                var tempPath = path;

                if (shouldCrop)
                {
                    tempPath = new Path(path);

                    var translate = new Matrix();
                    translate.SetTranslate(-croppedRectangle.Left, -croppedRectangle.Top);
                    tempPath.Transform(translate);
                }
                canvas.DrawPath(tempPath, paint);

                tempPath = null;
            }

            paint.Color = this.strokeColor;
        }

        /// <summary>
        /// Get cropped rectange.
        /// </summary>
        /// <param name="cachedPoints">Chached points.</param>
        /// <returns>Rectangle for the crop.</returns>
        private RectF GetCroppedRectangle(System.Drawing.PointF[] cachedPoints)
        {
            var xMin = cachedPoints.Where(point => !point.IsEmpty).Min(point => point.X) - strokeWidth / 2;
            var xMax = cachedPoints.Where(point => !point.IsEmpty).Max(point => point.X) + strokeWidth / 2;
            var yMin = cachedPoints.Where(point => !point.IsEmpty).Min(point => point.Y) - strokeWidth / 2;
            var yMax = cachedPoints.Where(point => !point.IsEmpty).Max(point => point.Y) + strokeWidth / 2;

            xMin = Math.Max(xMin, 0);
            xMax = Math.Min(xMax, Width);
            yMin = Math.Max(yMin, 0);
            yMax = Math.Min(yMax, Height);

            return new RectF(xMin, yMin, xMax, yMax);
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
        private Bitmap GetImage(
            Color strokeColor,
            Color fillColor,
            System.Drawing.SizeF size,
            float scale,
            bool shouldCrop = true,
            bool keepAspectRatio = true)
        {
            if (size.Width == 0 || size.Height == 0 || scale <= 0)
            {
                return null;
            }

            float uncroppedScale;
            RectF croppedRectangle = new RectF();

            System.Drawing.PointF[] cachedPoints;

            if (shouldCrop && (cachedPoints = Points).Any())
            {
                croppedRectangle = GetCroppedRectangle(cachedPoints);

                if (croppedRectangle.Left >= 5)
                    croppedRectangle.Left -= 5;
                if (croppedRectangle.Right <= size.Width - 5)
                    croppedRectangle.Right += 5;
                if (croppedRectangle.Top >= 5)
                    croppedRectangle.Top -= 5;
                if (croppedRectangle.Bottom <= size.Height - 5)
                    croppedRectangle.Bottom += 5;

                float scaleX = (croppedRectangle.Right - croppedRectangle.Left) / Width;
                float scaleY = (croppedRectangle.Bottom - croppedRectangle.Top) / Height;
                uncroppedScale = 1 / Math.Max(scaleX, scaleY);
            }
            else
            {
                uncroppedScale = scale;
            }

            Bitmap image;
            if (keepAspectRatio)
            {
                image = Bitmap.CreateBitmap((int)size.Width, (int)size.Height, Bitmap.Config.Argb8888);
            }
            else
            {
                image = Bitmap.CreateBitmap((int)(croppedRectangle.Width() * uncroppedScale), (int)(croppedRectangle.Height() * uncroppedScale), Bitmap.Config.Argb8888);
            }

            Canvas canvas = new Canvas(image);
            canvas.Scale(uncroppedScale, uncroppedScale);

            DrawStrokesOnCanvas(canvas, strokeColor, fillColor, shouldCrop, croppedRectangle);

            return image;
        }

        /// <summary>
        /// Get scale from size.
        /// </summary>
        /// <param name="size">Size to use.</param>
        /// <param name="width">Width to use.</param>
        /// <param name="height">Height to use.</param>
        /// <returns>Scale to use.</returns>
        private float GetScaleFromSize(System.Drawing.SizeF size, float width, float height)
        {
            float scaleX = size.Width / width;
            float scaleY = size.Height / height;

            return Math.Min(scaleX, scaleY);
        }

        /// <summary>
        /// Get size from scale.
        /// </summary>
        /// <param name="scale">Scale to use.</param>
        /// <param name="inWidth">Width to use.</param>
        /// <param name="inHeight">Height to use.</param>
        /// <returns>New size.</returns>
        private System.Drawing.SizeF GetSizeFromScale(float scale, float inWidth, float inHeight)
        {
            float width = inWidth * scale;
            float height = inHeight * scale;

            return new System.Drawing.SizeF(width, height);
        }

        /// <summary>
        /// Iterate through the touch history since the last touch event and add them to the path and points list.
        /// </summary>
        /// <param name="e">Arguments to use.</param>
        private void HandleTouch(MotionEvent e)
        {
            float touchX = e.GetX();
            float touchY = e.GetY();

            System.Drawing.PointF touch = new System.Drawing.PointF(touchX, touchY);

            ResetBounds(touchX, touchY);

            for (var i = 0; i < e.HistorySize; i++)
            {
                float historicalX = e.GetHistoricalX(i);
                float historicalY = e.GetHistoricalY(i);

                System.Drawing.PointF historical = new System.Drawing.PointF(historicalX, historicalY);

                UpdateBounds(historicalX, historicalY);

                currentPath.LineTo(historicalX, historicalY);
                currentPoints.Add(historical);
            }

            currentPath.LineTo(touchX, touchY);
            currentPoints.Add(touch);
        }

        /// <summary>
        /// Initialize view.
        /// </summary>
        private void Initialize()
        {
            BackgroundColor = Color.Black;
            strokeColor = Color.White;
            StrokeWidth = 2f;

            canvasView = new SignatureCanvasView(this.context);
            canvasView.LayoutParameters = new RelativeLayout.LayoutParams(RelativeLayout.LayoutParams.FillParent, RelativeLayout.LayoutParams.FillParent);

            // Set the attributes for painting the lines on the screen.
            paint = new Paint();
            paint.Color = strokeColor;
            paint.StrokeWidth = StrokeWidth;
            paint.SetStyle(Paint.Style.Stroke);
            paint.StrokeJoin = Paint.Join.Round;
            paint.StrokeCap = Paint.Cap.Round;
            paint.AntiAlias = true;

            #region Add Subviews

            RelativeLayout.LayoutParams layout;

            BackgroundImageView = new ImageView(this.context);
            BackgroundImageView.Id = GenerateId();
            AddView(BackgroundImageView);

            // Add an image that covers the entire signature view, used to display already drawn
            // elements instead of having to redraw them every time the user touches the screen.
            imageView = new ClearingImageView(context);
            imageView.SetBackgroundColor(Color.Transparent);
            imageView.LayoutParameters = new RelativeLayout.LayoutParams(RelativeLayout.LayoutParams.FillParent, RelativeLayout.LayoutParams.FillParent);
            AddView(imageView);

            lblSign = new TextView(context);
            lblSign.Id = GenerateId();
            lblSign.SetIncludeFontPadding(true);
            lblSign.Text = "Sign Here";
            layout = new RelativeLayout.LayoutParams(RelativeLayout.LayoutParams.WrapContent, RelativeLayout.LayoutParams.WrapContent);
            layout.AlignWithParent = true;
            layout.BottomMargin = 6;
            layout.AddRule(LayoutRules.AlignBottom);
            layout.AddRule(LayoutRules.CenterHorizontal);
            lblSign.LayoutParameters = layout;
            lblSign.SetPadding(0, 0, 0, 6);
            AddView(lblSign);

            // Display the base line for the user to sign on.
            signatureLine = new View(context);
            signatureLine.Id = GenerateId();
            signatureLine.SetBackgroundColor(Color.Gray);
            layout = new RelativeLayout.LayoutParams(RelativeLayout.LayoutParams.MatchParent, 1);
            layout.SetMargins(10, 0, 10, 5);
            layout.AddRule(LayoutRules.Above, lblSign.Id);
            signatureLine.LayoutParameters = layout;
            AddView(signatureLine);

            // Display the X on the left hand side of the line where the user signs.
            xLabel = new TextView(context);
            xLabel.Id = GenerateId();
            xLabel.Text = "X";
            xLabel.SetTypeface(null, TypefaceStyle.Bold);
            layout = new RelativeLayout.LayoutParams(RelativeLayout.LayoutParams.WrapContent, RelativeLayout.LayoutParams.WrapContent);
            layout.LeftMargin = 11;
            layout.AddRule(LayoutRules.Above, signatureLine.Id);
            xLabel.LayoutParameters = layout;
            AddView(xLabel);

            AddView(canvasView);

            #endregion Add Subviews

            paths = new List<Path>();
            points = new List<System.Drawing.PointF[]>();
            currentPoints = new List<System.Drawing.PointF>();

            dirtyRect = new RectF();
        }

        /// <summary>
        /// Set the bounds for the rectangle that will need to be redrawn to show the drawn path.
        /// </summary>
        /// <param name="touchX">Touch X.</param>
        /// <param name="touchY">Touch Y.</param>
        private void ResetBounds(float touchX, float touchY)
        {
            if (touchX < lastX)
            {
                dirtyRect.Left = touchX;
            }

            if (touchX > lastX)
            {
                dirtyRect.Right = touchX;
            }

            if (touchY < lastY)
            {
                dirtyRect.Top = touchY;
            }

            if (touchY > lastY)
            {
                dirtyRect.Bottom = touchY;
            }
        }

        /// <summary>
        /// Smooth points.
        /// </summary>
        /// <param name="granularity">Granularity to use.</param>
        /// <param name="smoothedPoints">Points smoothed.</param>
        /// <returns>Path to use.</returns>
        private Path SmoothedPathWithGranularity(int granularity, out List<System.Drawing.PointF> smoothedPoints)
        {
            List<System.Drawing.PointF> pointsArray = currentPoints;
            smoothedPoints = new List<System.Drawing.PointF>();

            //Not enough points to smooth effectively, so return the original path and points.
            if (pointsArray.Count < 4)
            {
                smoothedPoints = pointsArray;
                return currentPath;
            }

            //Create a new bezier path to hold the smoothed path.
            Path smoothedPath = new Path();

            //Duplicate the first and last points as control points.
            pointsArray.Insert(0, pointsArray[0]);
            pointsArray.Add(pointsArray[pointsArray.Count - 1]);

            //Add the first point
            smoothedPath.MoveTo(pointsArray[0].X, pointsArray[0].Y);
            smoothedPoints.Add(pointsArray[0]);

            for (var index = 1; index < pointsArray.Count - 2; index++)
            {
                System.Drawing.PointF p0 = pointsArray[index - 1];
                System.Drawing.PointF p1 = pointsArray[index];
                System.Drawing.PointF p2 = pointsArray[index + 1];
                System.Drawing.PointF p3 = pointsArray[index + 2];

                //Add n points starting at p1 + dx/dy up until p2 using Catmull-Rom splines
                for (var i = 1; i < granularity; i++)
                {
                    float t = (float)i * (1f / (float)granularity);
                    float tt = t * t;
                    float ttt = tt * t;

                    //Intermediate point
                    System.Drawing.PointF mid = new System.Drawing.PointF();
                    mid.X = 0.5f * (2f * p1.X + (p2.X - p0.X) * t +
                                    (2f * p0.X - 5f * p1.X + 4f * p2.X - p3.X) * tt +
                                    (3f * p1.X - p0.X - 3f * p2.X + p3.X) * ttt);
                    mid.Y = 0.5f * (2 * p1.Y + (p2.Y - p0.Y) * t +
                                    (2 * p0.Y - 5 * p1.Y + 4 * p2.Y - p3.Y) * tt +
                                    (3 * p1.Y - p0.Y - 3 * p2.Y + p3.Y) * ttt);

                    smoothedPath.LineTo(mid.X, mid.Y);
                    smoothedPoints.Add(mid);
                }

                //Add p2
                smoothedPath.LineTo(p2.X, p2.Y);
                smoothedPoints.Add(p2);
            }

            //Add the last point
            System.Drawing.PointF last = pointsArray[pointsArray.Count - 1];
            smoothedPath.LineTo(last.X, last.Y);
            smoothedPoints.Add(last);

            return smoothedPath;
        }

        /// <summary>
        /// Update the bounds for the rectangle to be redrawn if necessary for the given point.
        /// </summary>
        /// <param name="touchX">Touch X.</param>
        /// <param name="touchY">Touch Y.</param>
        private void UpdateBounds(float touchX, float touchY)
        {
            if (touchX < dirtyRect.Left)
            {
                dirtyRect.Left = touchX;
            }
            else if (touchX > dirtyRect.Right)
            {
                dirtyRect.Right = touchX;
            }

            if (touchY < dirtyRect.Top)
            {
                dirtyRect.Top = touchY;
            }
            else if (touchY > dirtyRect.Bottom)
            {
                dirtyRect.Bottom = touchY;
            }
        }
    }
}