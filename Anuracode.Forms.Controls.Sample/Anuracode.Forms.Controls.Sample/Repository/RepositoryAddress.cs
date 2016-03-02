// <copyright file="RepositoryAddress.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Anuracode.Forms.Controls.Sample.Repository
{
    /// <summary>
    /// Repository for the store items.
    /// </summary>
    public class RepositoryAddress
    {
        /// <summary>
        /// Addresses cahce.
        /// </summary>
        private List<Model.Address> addressesCache = null;

        /// <summary>
        /// Get all the items.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Task to await.</returns>
        public async Task<IEnumerable<Model.Address>> GetAllAsync(System.Threading.CancellationToken cancellationToken)
        {
            await Task.FromResult(0);

            if (addressesCache == null)
            {
                addressesCache = new List<Model.Address>();

                // Generate dummy data.
                for (int i = 0; i < 5; i++)
                {
                    addressesCache.Add(new Model.Address()
                    {
                        Name = string.Format("Name item {0}", i),
                        NickName = string.Format("Nick name item {0}", i),
                        Line1 = string.Format("Address line item {0}", i)
                    });
                }
            }

            return addressesCache;
        }
    }
}