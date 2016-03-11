// <copyright file="Client.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Anuracode.Forms.Controls.Sample.Model
{
    /// <summary>
    /// Model for the client.
    /// </summary>
    [DataContract]
    [KnownType(typeof(Address))]
    [KnownType(typeof(PersonBase))]
    public partial class Client : PersonBase
    {
        /// <summary>
        /// Collection of addresses.
        /// </summary>
        private List<Address> addresses;

        /// <summary>
        /// Notes added by the delivery guy.
        /// </summary>
        private string deliveryNotes = string.Empty;

        /// <summary>
        /// Is verified.
        /// </summary>
        private bool isVerified;

        /// <summary>
        /// Mobile phone.
        /// </summary>
        private string mobilePhone;

        /// <summary>
        /// Land phone.
        /// </summary>
        private string phone;

        /// <summary>
        /// Collection of addresses.
        /// </summary>
        [DataMember]
        public List<Address> Addresses
        {
            get
            {
                return addresses;
            }

            set
            {
                addresses = value;
            }
        }

        /// <summary>
        /// Notes added by the delivery guy.
        /// </summary>
        [DataMember]
        public string DeliveryNotes
        {
            get
            {
                return deliveryNotes;
            }

            set
            {
                ValidateRaiseAndSetIfChanged(ref deliveryNotes, value);
            }
        }

        /// <summary>
        /// Is verified.
        /// </summary>
        [DataMember]
        public bool IsVerified
        {
            get
            {
                return isVerified;
            }

            set
            {
                ValidateRaiseAndSetIfChanged(ref isVerified, value);
            }
        }

        /// <summary>
        /// Mobile phone.
        /// </summary>
        [DataMember]
        public string MobilePhone
        {
            get
            {
                return mobilePhone;
            }

            set
            {
                ValidateRaiseAndSetIfChanged(ref mobilePhone, value);
            }
        }

        /// <summary>
        /// Land phone.
        /// </summary>
        [DataMember]
        public string Phone
        {
            get
            {
                return phone;
            }

            set
            {
                ValidateRaiseAndSetIfChanged(ref phone, value);
            }
        }
    }
}