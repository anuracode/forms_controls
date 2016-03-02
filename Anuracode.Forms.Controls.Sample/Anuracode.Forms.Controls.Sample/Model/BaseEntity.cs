// <copyright file="BaseEntity.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Anuracode.Forms.Controls.Sample.Model
{
    /// <summary>
    /// Base entity.
    /// </summary>
    public class BaseEntity : INotifyPropertyChanged
    {
        /// <summary>
        /// When false, the notifications will be raise.
        /// </summary>
        private bool isEditable = true;

        /// <summary>
        /// Event when property changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// When false, the notifications will be raise.
        /// </summary>
        [IgnoreDataMember]
        public virtual bool IsReadOnly
        {
            get
            {
                return !isEditable;
            }

            set
            {
                isEditable = !value;
            }
        }

        /// <summary>
        /// Validate, set and notify when the property changes.
        /// </summary>
        /// <typeparam name="TRet">Property type.</typeparam>
        /// <param name="backingField">Field to use.</param>
        /// <param name="newValue">Value to check.</param>
        /// <param name="propertyName">Property name.</param>
        public virtual void ValidateRaiseAndSetIfChanged<TRet>(
            ref TRet backingField,
            TRet newValue,
            [CallerMemberName] string propertyName = null)
        {
            Contract.Requires(propertyName != null);

            if (!EqualityComparer<TRet>.Default.Equals(backingField, newValue))
            {
                backingField = newValue;

                if (!IsReadOnly)
                {
                    OnPropertyChanged(propertyName);
                }
            }
        }

        /// <summary>
        /// Notify that a property changed.
        /// </summary>
        /// <param name="propertyName">Name of the property that changed.</param>
        /// [Obsolete("Use OnPropertyChanged with expression.", false)]
        protected void OnPropertyChanged(string propertyName)
        {
            AC.ScheduleManaged(
                async () =>
                {
                    await Task.FromResult(0);
                    RaisePropertyChangedEvent(propertyName);
                });
        }

        /// <summary>
        /// Raise the property change.
        /// </summary>
        /// <param name="whichProperty"></param>
        protected void RaisePropertyChangedEvent(string whichProperty)
        {
            // check for subscription before going multithreaded
            if (PropertyChanged != null)
            {
                var handler = PropertyChanged;

                if (handler != null)
                {
                    handler(this, new PropertyChangedEventArgs(whichProperty));
                }
            }
        }
    }
}