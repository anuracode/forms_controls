// <copyright file="GenericCompare.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using System;
using System.Collections.Generic;

namespace Anuracode.Forms.Controls.Sample.ViewModels
{
    /// <summary>
    /// Generic comparer with expression.
    /// </summary>
    /// <typeparam name="T">Type to use.</typeparam>
    public class GenericCompare<T> : IEqualityComparer<T> where T : class
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="expr"></param>
        public GenericCompare(Func<T, object> expr)
        {
            this.delegateExpression = expr;
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public GenericCompare() :
            this(null)
        {
        }

        /// <summary>
        /// Delegate expression.
        /// </summary>
        private Func<T, object> delegateExpression { get; set; }

        /// <summary>
        /// Equals method.
        /// </summary>
        /// <param name="x">X to compare.</param>
        /// <param name="y">Y to compare.</param>
        /// <returns>Result of the compare.</returns>
        public bool Equals(T x, T y)
        {
            if (delegateExpression == null)
            {
                return x != null && x.Equals(y);
            }
            else
            {
                var first = delegateExpression.Invoke(x);
                var sec = delegateExpression.Invoke(y);

                return (first != null) && first.Equals(sec);
            }
        }

        /// <summary>
        /// Get hash code.
        /// </summary>
        /// <param name="obj">Object to use.</param>
        /// <returns>Hash code.</returns>
        public int GetHashCode(T obj)
        {
            if (delegateExpression == null)
            {
                return obj.GetHashCode();
            }
            else
            {
                return delegateExpression.Invoke(obj).GetHashCode();
            }
        }
    }
}