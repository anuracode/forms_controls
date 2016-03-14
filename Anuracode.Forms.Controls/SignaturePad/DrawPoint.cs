// <copyright file="DrawPoint.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

namespace Anuracode.Forms.Controls
{
    /// <summary>
    /// Represents a point.
    /// </summary>
    public struct DrawPoint
    {
        /// <summary>
        /// X coordinate to draw.
        /// </summary>
        private double x;

        /// <summary>
        /// Y coordinate to draw.
        /// </summary>
        private double y;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="x">X to use.</param>
        /// <param name="y">Y to use.</param>
        public DrawPoint(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// X coordinate to draw.
        /// </summary>
        public double X { get { return x; } }

        /// <summary>
        /// Y coordinate to draw.
        /// </summary>
        public double Y { get { return y; } }
    }
}