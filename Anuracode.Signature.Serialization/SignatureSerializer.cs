// <copyright file="SignatureSerializer.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

namespace Anuracode.Signature.Serialization
{
    /// <summary>
    /// Serialize a signature.
    /// </summary>
    public static class SignatureSerializer
    {
        /// <summary>
        /// Deserialize the points.
        /// </summary>
        /// <param name="pointsSerialized">String with points serialized.</param>
        /// <returns>List of points.</returns>
        public static async Task<List<List<DrawPoint>>> DeserializeAsync(string pointsSerialized)
        {
            await Task.FromResult(0);
            List<List<DrawPoint>> result = new List<List<DrawPoint>>();

            double x, y;
            List<DrawPoint> newLine;
            string[] values, points;
            NumberStyles style = NumberStyles.AllowDecimalPoint | NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingSign | NumberStyles.AllowLeadingWhite;

            if (!string.IsNullOrWhiteSpace(pointsSerialized))
            {
                var polilines = pointsSerialized.Split(new string[] { "P" }, StringSplitOptions.RemoveEmptyEntries);

                if (polilines != null)
                {
                    for (int i = 0; i < polilines.Length; i++)
                    {
                        points = polilines[i].Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

                        if (points != null)
                        {
                            newLine = new List<DrawPoint>();

                            for (int j = 0; j < points.Length; j++)
                            {
                                values = points[j].Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                                if ((values != null) && (values.Length == 2))
                                {
                                    if (double.TryParse(values[0], style, CultureInfo.InvariantCulture, out x) && double.TryParse(values[1], style, CultureInfo.InvariantCulture, out y))
                                    {
                                        newLine.Add(new DrawPoint(x, y));
                                    }
                                }
                            }

                            result.Add(newLine);
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Get the points in string format.
        /// </summary>
        /// <param name="linesList">List of point for the lines.</param>
        /// <returns>String with the point.</returns>
        public static async Task<string> SerializeAsync(List<List<DrawPoint>> linesList)
        {
            await Task.FromResult(0);
            string result = null;
                        
            if ((linesList != null) && (linesList.Count > 1))
            {
                double translateX = 0;
                double translateY = 0;

                StringBuilder sbPoints = new StringBuilder();
                IList<DrawPoint> line;
                DrawPoint point;

                double margin = 5;
                double minX = double.MaxValue;
                double minY = double.MaxValue;

                // find max and min values.
                for (int i = 0; i < linesList.Count; i++)
                {
                    line = linesList[i];

                    if ((line != null) && (line.Count > 0))
                    {
                        for (int j = 0; j < line.Count; j++)
                        {
                            point = line[j];
                            minX = Math.Min(minX, point.X);
                            minY = Math.Min(minY, point.Y);
                        }
                    }
                }

                if (minX > margin)
                {
                    translateX = -(minX - margin);
                }

                if (minY > margin)
                {
                    translateY = -(minY - margin);
                }

                for (int i = 0; i < linesList.Count; i++)
                {
                    line = linesList[i];

                    if ((line != null) && (line.Count > 0))
                    {
                        sbPoints.Append("P");
                        for (int j = 0; j < line.Count; j++)
                        {
                            point = line[j];
                            sbPoints.AppendFormat(CultureInfo.InvariantCulture, "{0:####0.##}", point.X + translateX);
                            sbPoints.Append(":");
                            sbPoints.AppendFormat(CultureInfo.InvariantCulture, "{0:####0.##}", point.Y + translateY);
                            sbPoints.Append(";");
                        }
                    }
                }

                result = sbPoints.ToString();
            }

            return result;
        }
    }
}