// <copyright file="TraceService.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using System;
using System.Collections.Generic;
using System.Text;

namespace Anuracode.Forms.Controls.Sample.Windows
{
    /// <summary>
    /// Trace service for the controls.
    /// </summary>
    public class TraceService : ITraceService
    {
        /// <summary>
        /// Trace message.
        /// </summary>
        /// <param name="message">Message or key to register.</param>
        /// <param name="errorException">Exception to register.</param>
        /// <param name="eventParameters">Paramters to register.</param>
        public void TraceError(string message, Exception errorException, params KeyValuePair<string, object>[] eventParameters)
        {
            string level = "Error";

            StringBuilder parametersBuilder = new StringBuilder();

            if (eventParameters != null)
            {
                foreach (var eventParameter in eventParameters)
                {
                    parametersBuilder.AppendFormat("{0}:{1};", eventParameter.Key, eventParameter.Value);
                }
            }

            string formatedMessage = string.Format("{0}: '{1}'; Parameters: {2}", level, message, parametersBuilder.ToString());
        }
    }
}