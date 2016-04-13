// <copyright file="SignaturePad.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace Anuracode.Forms.Controls.Renderers
{
    /// <summary>
    /// Signature pad view.
    /// </summary>
    public sealed partial class SignaturePad : UserControl
    {
        /// <summary>
        /// Flag for content loaded.
        /// </summary>
        private bool _contentLoaded;

        /// <summary>
        /// Border for the signature.
        /// </summary>
        private Border borderSignature;

        /// <summary>
        /// Current path.
        /// </summary>
        private Polyline currentPath;

        /// <summary>
        /// Canvas to use.
        /// </summary>
        private Canvas inkPresenter;

        /// <summary>
        /// Last x.
        /// </summary>
        private double lastX;

        /// <summary>
        /// Last y.
        /// </summary>
        private double lastY;

        /// <summary>
        /// Layout to use.
        /// </summary>
        private Grid LayoutRoot;

        /// <summary>
        /// Solor for the stroke.
        /// </summary>
        private Brush strokeColor;

        /// <summary>
        /// Storke thickness.
        /// </summary>
        private double strokeThickness = 1;

        /// <summary>
        /// Text for the caption.
        /// </summary>
        private TextBlock textBlockCaption;

        /// <summary>
        /// Text for the prompt.
        /// </summary>
        private TextBlock textBlockPrompt;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public SignaturePad()
        {
            this.InitializeComponent();

            inkPresenter.Clip = new RectangleGeometry()
            {
                Rect = new Rect(0, 0, inkPresenter.ActualWidth, inkPresenter.ActualHeight)
            };

            inkPresenter.SizeChanged += (object sender, Windows.UI.Xaml.SizeChangedEventArgs e) =>
            {
                inkPresenter.Clip = new RectangleGeometry()
                {
                    Rect = new Rect(0, 0, e.NewSize.Width, e.NewSize.Height)
                };
            };

            inkPresenter.Children.Add(CurrentPath);

            inkPresenter.ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY;
            inkPresenter.ManipulationStarted += InkPresenter_ManipulationStarted;
            inkPresenter.ManipulationCompleted += InkPresenter_ManipulationCompleted;
            inkPresenter.ManipulationDelta += InkPresenter_ManipulationDelta;
        }

        /// <summary>
        /// Border for the signature.
        /// </summary>
        public Border BorderSignature
        {
            get
            {
                return borderSignature;
            }
        }

        /// <summary>
        /// Current path.
        /// </summary>
        public Polyline CurrentPath
        {
            get
            {
                if (currentPath == null)
                {
                    currentPath = new Polyline()
                    {
                        Stroke = StrokeColor,
                        StrokeThickness = this.StrokeThickness
                    };
                }

                return currentPath;
            }
        }

        /// <summary>
        /// Canvas for the ink.
        /// </summary>
        public Canvas InkPresenter
        {
            get
            {
                return inkPresenter;
            }
        }

        /// <summary>
        /// Property to check if its blank.
        /// </summary>
        public bool IsBlank
        {
            get
            {
                return (inkPresenter != null) && (inkPresenter.Children.Count == 1) && (CurrentPath.Points.Count == 0);
            }
        }

        /// <summary>
        /// Delegate when the is blank changed.
        /// </summary>
        public Action RaiseIsBlankChangedDelegate { get; set; }

        /// <summary>
        /// Solor for the stroke.
        /// </summary>
        public Brush StrokeColor
        {
            get
            {
                if (strokeColor == null)
                {
                    strokeColor = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Colors.Black);
                }

                return strokeColor;
            }
            set
            {
                strokeColor = value;

                if (CurrentPath != null)
                {
                    CurrentPath.Stroke = value;
                }
            }
        }

        /// <summary>
        /// Stroke thickness.
        /// </summary>
        public double StrokeThickness
        {
            get
            {
                return strokeThickness;
            }

            set
            {
                strokeThickness = value;

                if (CurrentPath != null)
                {
                    CurrentPath.StrokeThickness = value;
                }
            }
        }

        /// <summary>
        /// Text block for the caption.
        /// </summary>
        public TextBlock TextBlockCaption
        {
            get
            {
                return textBlockCaption;
            }
        }

        /// <summary>
        /// Text block for the prompt.
        /// </summary>
        public TextBlock TextBlockPrompt
        {
            get
            {
                return textBlockPrompt;
            }
        }

        /// <summary>
        /// Delete the current signature.
        /// </summary>
        public void Clear()
        {
            CurrentPath.Points.Clear();

            inkPresenter.Children.Clear();
            inkPresenter.Children.Add(CurrentPath);

            NotifyIsBlankChanged();
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
            if ((inkPresenter != null) && (inkPresenter.Children.Count > 1))
            {
                List<DrawPoint> pointLine;
                Polyline line;
                Point point;

                for (int i = 0; i < inkPresenter.Children.Count; i++)
                {
                    line = inkPresenter.Children[i] as Polyline;

                    if ((line != null) && (line.Points.Count > 0))
                    {
                        pointLine = new List<DrawPoint>();
                        for (int j = 0; j < line.Points.Count; j++)
                        {
                            point = line.Points[j];
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
        /// InitializeComponent()
        /// </summary>
        public void InitializeComponent()
        {
            if (_contentLoaded)
            {
                return;
            }

            _contentLoaded = true;

            LayoutRoot = new Grid();
            inkPresenter = new Canvas()
            {
                VerticalAlignment = VerticalAlignment.Stretch,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Background = new SolidColorBrush(Colors.Transparent),
                Opacity = 1
            };

            LayoutRoot.Children.Add(InkPresenter);

            textBlockPrompt = new TextBlock()
            {
                Text = "X",
                Margin = new Thickness(20, 0, 0, 25),
                Foreground = new SolidColorBrush(Colors.Gray),
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Left,
            };

            LayoutRoot.Children.Add(textBlockPrompt);

            borderSignature = new Border()
            {
                Height = 2,
                BorderThickness = new Thickness(2),
                Margin = new Thickness(20, 0, 20, 25),
                BorderBrush = new SolidColorBrush(Colors.Gray),
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Stretch
            };

            LayoutRoot.Children.Add(borderSignature);

            textBlockCaption = new TextBlock()
            {
                Text = "",
                Margin = new Thickness(20, 0, 0, 5),
                Foreground = new SolidColorBrush(Colors.Gray),
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Center,
            };

            LayoutRoot.Children.Add(textBlockCaption);

            Content = LayoutRoot;
        }

        /// <summary>
        /// Get the points in string format.
        /// </summary>
        /// <param name="pointsSerialized">Point serialized.</param>
        public async Task LoadPointsStringAsync(string pointsSerialized)
        {
            await Task.FromResult(0);

            Clear();

            List<List<DrawPoint>> linesList = await SignatureSerializer.DeserializeAsync(pointsSerialized);
            List<DrawPoint> pointsLine;
            Polyline newLine;
            DrawPoint drawPoint;

            if (linesList != null)
            {
                for (int i = 0; i < linesList.Count; i++)
                {
                    pointsLine = linesList[i];

                    if (pointsLine != null)
                    {
                        newLine = new Polyline();
                        newLine.Stroke = StrokeColor;
                        newLine.StrokeThickness = StrokeThickness;

                        for (int j = 0; j < pointsLine.Count; j++)
                        {
                            drawPoint = pointsLine[j];
                            newLine.Points.Add(new Point(drawPoint.X, drawPoint.Y));
                        }

                        inkPresenter.Children.Add(newLine);
                    }
                }
            }

            NotifyIsBlankChanged();
        }

        /// <summary>
        /// Manipulation completed.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Arguments of the event.</param>
        private void InkPresenter_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            double touchX = e.Position.X;
            double touchY = e.Position.Y;
            lastX = touchX;
            lastY = touchY;

            // Create a new path and move to the touched point.
            if (CurrentPath != null)
            {
                CurrentPath.Points.Add(new Point(touchX, touchY));
            }

            Polyline newPath = SmoothedPathWithGranularity(0, CurrentPath.Points);

            newPath.Stroke = StrokeColor;
            newPath.StrokeThickness = StrokeThickness;

            inkPresenter.Children.Add(newPath);

            CurrentPath.Points.Clear();

            NotifyIsBlankChanged();
        }

        /// <summary>
        /// Manipulation delta.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Arguments of the event.</param>
        private void InkPresenter_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            double touchX = e.Position.X;
            double touchY = e.Position.Y;
            lastX = touchX;
            lastY = touchY;

            // Create a new path and move to the touched point.
            if (CurrentPath != null)
            {
                CurrentPath.Points.Add(new Point(touchX, touchY));
            }
        }

        /// <summary>
        /// Manipulation started.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Arguments of the event.</param>
        private void InkPresenter_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            double touchX = e.Position.X;
            double touchY = e.Position.Y;
            lastX = touchX;
            lastY = touchY;

            // Create a new path and move to the touched point.
            CurrentPath.Points.Clear();
            CurrentPath.Points.Add(new Point(touchX, touchY));
        }

        /// <summary>
        /// Notify the is blank changed.
        /// </summary>
        private void NotifyIsBlankChanged()
        {
            if (RaiseIsBlankChangedDelegate != null)
            {
                RaiseIsBlankChangedDelegate();
            }
        }

        /// <summary>
        /// Smooth points.
        /// </summary>
        /// <param name="granularity">Granularity to use.</param>
        /// <param name="pointsArray">Points to smooth.</param>
        /// <returns>Path to use.</returns>
        private Polyline SmoothedPathWithGranularity(int granularity, PointCollection pointsOriginal)
        {
            List<Point> pointsArray = new List<Point>();
            Polyline smoothedPoints = new Polyline();

            // Not enough points to smooth effectively, so return the original path and points.
            if (pointsOriginal.Count < 4)
            {
                for (int i = 0; i < pointsOriginal.Count; i++)
                {
                    smoothedPoints.Points.Add(pointsOriginal[i]);
                }

                return smoothedPoints;
            }

            pointsArray.Add(pointsOriginal[0]);

            for (var index = 1; index < pointsOriginal.Count - 2; index++)
            {
                var p0 = pointsOriginal[index - 1];
                var p1 = pointsOriginal[index];
                var p2 = pointsOriginal[index + 1];
                var p3 = pointsOriginal[index + 2];

                //Add n points starting at p1 + dx/dy up until p2 using Catmull-Rom splines
                for (var i = 1; i < granularity; i++)
                {
                    float t = (float)i * (1f / (float)granularity);
                    float tt = t * t;
                    float ttt = tt * t;

                    //Intermediate point
                    Point mid = new Point();
                    mid.X = 0.5f * (2f * p1.X + (p2.X - p0.X) * t +
                                    (2f * p0.X - 5f * p1.X + 4f * p2.X - p3.X) * tt +
                                    (3f * p1.X - p0.X - 3f * p2.X + p3.X) * ttt);
                    mid.Y = 0.5f * (2 * p1.Y + (p2.Y - p0.Y) * t +
                                    (2 * p0.Y - 5 * p1.Y + 4 * p2.Y - p3.Y) * tt +
                                    (3 * p1.Y - p0.Y - 3 * p2.Y + p3.Y) * ttt);

                    pointsArray.Add(mid);
                }

                pointsArray.Add(p2);
            }

            // Add the last point
            pointsArray.Add(pointsOriginal[pointsOriginal.Count - 1]);

            for (int i = 0; i < pointsArray.Count; i++)
            {
                smoothedPoints.Points.Add(pointsArray[i]);
            }

            return smoothedPoints;
        }
    }
}