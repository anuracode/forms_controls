// <copyright file="ICommandExtensions.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Anuracode.Forms.Controls.Extensions
{
    /// <summary>
    /// Extension for ICommand.
    /// </summary>
    public static class ICommandExtensions
    {
        /// <summary>
        /// Can Execute with paramter null.
        /// </summary>
        /// <param name="command">Command to execute.</param>
        public static bool CanExecute(this ICommand command)
        {
            return command.CanExecute(null);
        }

        /// <summary>
        /// Execute with paramter null.
        /// </summary>
        /// <param name="command">Command to execute.</param>
        public static void Execute(this ICommand command)
        {
            command.Execute(null);
        }

        /// <summary>
        /// Validate the can execute, if it can, execute it.
        /// </summary>
        /// <param name="command">Command to execute.</param>
        public static Task ExecuteAsync(this ICommand command)
        {
            ExecuteIfCan(command, null);

            return Task.FromResult(0);
        }

        /// <summary>
        /// Validate the can execute, if it can, execute it.
        /// </summary>
        /// <param name="command">Command to execute.</param>
        /// <param name="parameter">Parameter to use.</param>
        public static Task ExecuteAsync(this ICommand command, object parameter)
        {
            ExecuteIfCan(command, parameter);

            return Task.FromResult(0);
        }

        /// <summary>
        /// Validate the can execute, if it can, execute it.
        /// </summary>
        /// <param name="command">Command to execute.</param>
        /// <param name="parameter">Parameter to use.</param>
        public static void ExecuteIfCan(this ICommand command, object parameter)
        {
            if (command.CanExecute(parameter))
            {
                command.Execute(parameter);
            }
        }

        /// <summary>
        /// Validate the can execute, if it can, execute it.
        /// </summary>
        /// <param name="command">Command to execute.</param>
        public static void ExecuteIfCan(this ICommand command)
        {
            ExecuteIfCan(command, null);
        }

        /// <summary>
        /// Raise can execute changed.
        /// </summary>
        /// <param name="command">Command to use.</param>
        public static void RaiseCanExecuteChanged(this Command command)
        {
            if (command != null)
            {
                command.ChangeCanExecute();
            }
        }
    }
}