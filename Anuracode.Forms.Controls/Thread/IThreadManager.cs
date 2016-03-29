// <copyright file="IThreadManager.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using System;
using System.Threading.Tasks;

namespace Anuracode.Forms.Controls.Thread
{
    /// <summary>
    /// Threath manager interface.
    /// </summary>
    public interface IThreadManager
    {
        /// <summary>
        /// Schedule a action to execute.
        /// </summary>
        /// <param name="action">Action to execute.</param>
        /// <param name="useUIThread">True to use the UI thread.</param>
        /// <param name="delayTime">Delay time to execute.</param>
        void ScheduleManagedFull(Func<Task> action, bool useUIThread = true, TimeSpan? delayTime = null);
    }
}