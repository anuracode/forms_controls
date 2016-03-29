// <copyright file="AC.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using Anuracode.Forms.Controls.Thread;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Anuracode.Forms.Controls
{
    /// <summary>
    /// Singletone container.
    /// </summary>
    public static class AC
    {
        /// <summary>
        /// Thread manager.
        /// </summary>
        private static IThreadManager threadManager;

        /// <summary>
        /// Trace service.
        /// </summary>
        private static ITraceService traceService;

        /// <summary>
        /// Thread manager.
        /// </summary>
        public static IThreadManager ThreadManager
        {
            get
            {
                if (threadManager == null)
                {
                    SetThreadManager(new ThreadManagerBase());
                }

                return threadManager;
            }
        }

        /// <summary>
        /// Schedule a action to execute.
        /// </summary>
        /// <param name="action">Action to execute.</param>
        public static void ScheduleManaged(Func<Task> action)
        {
            ThreadManager.ScheduleManagedFull(action);
        }

        /// <summary>
        /// Schedule a action to execute.
        /// </summary>
        /// <param name="action">Action to execute.</param>
        public static void ScheduleManaged(Action action)
        {
            ThreadManager.ScheduleManagedFull(
                async () =>
                {
                    await Task.FromResult(0);

                    if (action != null)
                    {
                        action();
                    }
                });
        }

        /// <summary>
        /// Schedule a action to execute.
        /// </summary>
        /// <param name="action">Action to execute.</param>
        /// <param name="delayTime">Delay time to execute.</param>
        public static void ScheduleManaged(TimeSpan delayTime, Func<Task> action)
        {
            ThreadManager.ScheduleManagedFull(action, delayTime: delayTime);
        }

        /// <summary>
        /// Set thread manager.
        /// </summary>
        /// <param name="newManager">New manager.</param>
        public static void SetThreadManager(IThreadManager newManager)
        {
            if (newManager != null)
            {
                threadManager = newManager;
            }
        }

        /// <summary>
        /// Update the trace service.
        /// </summary>
        /// <param name="newTraceService"></param>
        public static void SetTraceService(ITraceService newTraceService)
        {
            traceService = newTraceService;
        }

        /// <summary>
        /// Trace message.
        /// </summary>
        /// <param name="message">Message or key to register.</param>
        /// <param name="errorException">Exception to register.</param>
        /// <param name="eventParameters">Paramters to register.</param>
        public static void TraceError(string message, System.Exception errorException, params KeyValuePair<string, object>[] eventParameters)
        {
            if (traceService != null)
            {
                traceService.TraceError(message, errorException, eventParameters);
            }
        }
    }
}