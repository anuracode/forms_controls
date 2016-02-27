// <copyright file="ITraceService.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anuracode.Forms.Controls
{
    /// <summary>
    /// Trace interface.
    /// </summary>
    public interface ITraceService
    {
        /// <summary>
        /// Trace message.
        /// </summary>
        /// <param name="message">Message or key to register.</param>
        /// <param name="errorException">Exception to register.</param>
        /// <param name="eventParameters">Paramters to register.</param>
        void TraceError(string message, System.Exception errorException, params KeyValuePair<string, object>[] eventParameters);
    }
}
