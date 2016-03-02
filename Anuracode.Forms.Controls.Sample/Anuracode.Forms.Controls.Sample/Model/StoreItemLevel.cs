// <copyright file="StoreItemLevel.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using System.Text;

namespace Anuracode.Forms.Controls.Sample.Model
{
    /// <summary>
    /// Level for a store item, use to filter the store.
    /// </summary>
    public class StoreItemLevel
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public StoreItemLevel()
        {
        }

        /// <summary>
        /// Constructor from an item.
        /// </summary>
        /// <param name="item">Item to use.</param>
        public StoreItemLevel(StoreItem item)
        {
            if (item != null)
            {
                Department = item.Department;
                Category = item.Category;
                Subcategory = item.Subcategory;
            }
        }

        /// <summary>
        /// Category for the product.
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Department for the product.
        /// </summary>
        public string Department { get; set; }

        /// <summary>
        /// Subcategory for the product.
        /// </summary>
        public string Subcategory { get; set; }

        /// <summary>
        /// To string value.
        /// </summary>
        /// <returns>String value.</returns>
        public override string ToString()
        {
            return ToString(" - ");
        }

        /// <summary>
        /// To string value.
        /// </summary>
        /// <param name="separator">Separator for the levels.</param>
        /// <returns>String value.</returns>
        public string ToString(string separator)
        {
            StringBuilder sb = new StringBuilder();

            if (!string.IsNullOrEmpty(Department))
            {
                sb.Append(Department);
            }

            if (!string.IsNullOrEmpty(Category))
            {
                if (sb.Length > 0)
                {
                    sb.Append(separator);
                }

                sb.Append(Category);
            }

            if (!string.IsNullOrEmpty(Subcategory))
            {
                if (sb.Length > 0)
                {
                    sb.Append(separator);
                }

                sb.Append(Subcategory);
            }

            return sb.ToString();
        }
    }
}
