// <copyright file="AC.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Anuracode.Forms.Controls
{
    /// <summary>
    /// Singletone container.
    /// </summary>
    public static class AC
    {
        /// <summary>
        /// Trace service.
        /// </summary>
        private static ITraceService traceService;

        /// <summary>
        /// Schedule a action to execute.
        /// </summary>
        /// <param name="action">Action to execute.</param>
        public static void ScheduleManaged(Func<Task> action)
        {
            InternalScheduleManaged(action);
        }

        /// <summary>
        /// Schedule a action to execute.
        /// </summary>
        /// <param name="action">Action to execute.</param>
        public static void ScheduleManaged(Action action)
        {
            InternalScheduleManaged(
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
            InternalScheduleManaged(action, delayTime: delayTime);
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

        /// <summary>
        /// Schedule a action to execute.
        /// </summary>
        /// <param name="action">Action to execute.</param>
        /// <param name="useUIThread">True to use the UI thread.</param>
        /// <param name="delayTime">Delay time to execute.</param>
        private static void InternalScheduleManaged(Func<Task> action, bool useUIThread = true, TimeSpan? delayTime = null)
        {
            if (useUIThread)
            {
                Device.BeginInvokeOnMainThread(
                    async () =>
                    {
                        try
                        {
                            if ((delayTime != null) && delayTime.Value.TotalMilliseconds > 0)
                            {
                                await Task.Delay(delayTime.Value);
                            }

                            if (action != null)
                            {
                                await action();
                            }
                        }                       
                        catch (OperationCanceledException opex)
                        {
                            TraceError("Operation cancelled", opex);
                        }
                        catch (Exception ex)
                        {
                            TraceError("Schedule error", ex);
                        }
                    });
            }
            else
            {
                Task.Run(
                    async () =>
                    {
                        try
                        {
                            if ((delayTime != null) && delayTime.Value.TotalMilliseconds > 0)
                            {
                                await Task.Delay(delayTime.Value);
                            }

                            await action();
                        }
                        catch (Exception ex)
                        {
                            TraceError("Schedule error", ex);
                        }
                    });
            }
        }
    }
}