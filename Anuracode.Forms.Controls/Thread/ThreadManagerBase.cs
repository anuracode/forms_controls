// <copyright file="ThreadManagerBase.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Anuracode.Forms.Controls.Thread
{
    /// <summary>
    /// Thread manager.
    /// </summary>
    public class ThreadManagerBase : IThreadManager
    {
        /// <summary>
        /// Schedule a action to execute.
        /// </summary>
        /// <param name="action">Action to execute.</param>
        /// <param name="useUIThread">True to use the UI thread.</param>
        /// <param name="delayTime">Delay time to execute.</param>
        public virtual void ScheduleManagedFull(Func<Task> action, bool useUIThread = true, TimeSpan? delayTime = null)
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
                            AC.TraceError("Operation cancelled", opex);
                        }
                        catch (Exception ex)
                        {
                            AC.TraceError("Schedule error", ex);
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
                            AC.TraceError("Schedule error", ex);
                        }
                    });
            }
        }
    }
}