// <copyright file="Address.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using System.Runtime.Serialization;
using System.Text;

namespace Anuracode.Forms.Controls.Sample.Model
{
    /// <summary>
    /// Represent an address in the system.
    /// </summary>
    [DataContract]    
    public partial class Address : BaseEntity
    {
        /// <summary>
        /// City to use.
        /// </summary>
        private string city = "Miami";

        /// <summary>
        /// Country for the address.
        /// </summary>
        private string country = "United States";

        /// <summary>
        /// Notes added by the delivery guy.
        /// </summary>
        private string deliveryNotes = string.Empty;

        /// <summary>
        /// External Uri of the zone, used by the providers.
        /// </summary>
        private string externalUri;

        /// <summary>
        /// Extra directions.
        /// </summary>
        private string extraDirections = string.Empty;        

        /// <summary>
        /// GUID of the element.
        /// </summary>
        private string id;

        /// <summary>
        /// True when is the default address.
        /// </summary>
        private bool isDefault;

        /// <summary>
        /// Address info.
        /// </summary>
        private string line1 = string.Empty;

        /// <summary>
        /// Address extra info.
        /// </summary>
        private string line2 = string.Empty;

        /// <summary>
        /// Name of the entity.
        /// </summary>
        private string name = string.Empty;

        /// <summary>
        /// Neighbourhood for the address.
        /// </summary>
        private string neighbourhood;

        /// <summary>
        /// Nickname for the address.
        /// </summary>
        private string nickName;

        /// <summary>
        /// Land phone.
        /// </summary>
        private string phone;

        /// <summary>
        /// State for the address.
        /// </summary>
        private string state = "Miami";

        /// <summary>
        /// Zip code.
        /// </summary>
        private string zipCode;

        /// <summary>
        /// City to use.
        /// </summary>
        [DataMember]
        public string City
        {
            get
            {
                return city;
            }

            set
            {
                ValidateRaiseAndSetIfChanged(ref city, value);
            }
        }

        /// <summary>
        /// Country for the address.
        /// </summary>
        [DataMember]
        public string Country
        {
            get
            {
                return country;
            }

            set
            {
                ValidateRaiseAndSetIfChanged(ref country, value);
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
        /// External Uri of the address, used by the providers.
        /// </summary>
        [DataMember]
        public string ExternalUri
        {
            get
            {
                return externalUri;
            }

            set
            {
                ValidateRaiseAndSetIfChanged(ref externalUri, value);

                OnPropertyChanged(nameof(HasExternalUri));
            }
        }

        /// <summary>
        /// Extra directions.
        /// </summary>
        [DataMember]
        public string ExtraDirections
        {
            get
            {
                return extraDirections;
            }

            set
            {
                ValidateRaiseAndSetIfChanged(ref extraDirections, value);
            }
        }       

        /// <summary>
        /// Flag if it has external uri.
        /// </summary>
        public bool HasExternalUri { get { return !string.IsNullOrWhiteSpace(ExternalUri); } }

        /// <summary>
        /// GUID of the task.
        /// </summary>
        [DataMember]
        public string Id
        {
            get
            {
                return id;
            }

            set
            {
                ValidateRaiseAndSetIfChanged(ref id, value);
            }
        }

        /// <summary>
        /// True when is the default address.
        /// </summary>
        [DataMember]
        public bool IsDefault
        {
            get
            {
                return isDefault;
            }

            set
            {
                ValidateRaiseAndSetIfChanged(ref isDefault, value);
            }
        }

        /// <summary>
        /// Address info.
        /// </summary>
        [DataMember]
        public string Line1
        {
            get
            {
                return line1;
            }

            set
            {
                ValidateRaiseAndSetIfChanged(ref line1, value);
            }
        }

        /// <summary>
        /// Address extra info.
        /// </summary>
        [DataMember]
        public string Line2
        {
            get
            {
                return line2;
            }

            set
            {
                ValidateRaiseAndSetIfChanged(ref line2, value);
            }
        }

        /// <summary>
        /// Name of the reciever.
        /// </summary>
        [DataMember]
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                ValidateRaiseAndSetIfChanged(ref name, value);
            }
        }

        /// <summary>
        /// Neighbourhood for the address.
        /// </summary>
        [DataMember]
        public string Neighbourhood
        {
            get
            {
                return neighbourhood;
            }

            set
            {
                ValidateRaiseAndSetIfChanged(ref neighbourhood, value);
            }
        }

        /// <summary>
        /// Nickname for the address.
        /// </summary>
        [DataMember]
        public string NickName
        {
            get
            {
                return nickName;
            }

            set
            {
                ValidateRaiseAndSetIfChanged(ref nickName, value);
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

        /// <summary>
        /// State for the address.
        /// </summary>
        [DataMember]
        public string State
        {
            get
            {
                return state;
            }

            set
            {
                ValidateRaiseAndSetIfChanged(ref state, value);
            }
        }

        /// <summary>
        /// Zip code.
        /// </summary>
        [DataMember]
        public string ZipCode
        {
            get
            {
                return zipCode;
            }

            set
            {
                ValidateRaiseAndSetIfChanged(ref zipCode, value);
            }
        }

        /// <summary>
        /// Merge data of the address with another.
        /// </summary>
        /// <param name="newAddress">Address to copy from.</param>
        public void Merge(Address newAddress)
        {
            if (newAddress != null)
            {
                NickName = newAddress.NickName;
                Line1 = newAddress.Line1;
                Line2 = newAddress.Line2;
                City = newAddress.City;

                ExternalUri = newAddress.ExternalUri;

                if (!string.IsNullOrWhiteSpace(newAddress.Neighbourhood))
                {
                    Neighbourhood = newAddress.Neighbourhood;
                }

                if (!string.IsNullOrWhiteSpace(newAddress.State))
                {
                    State = newAddress.State;
                }

                if (!string.IsNullOrWhiteSpace(newAddress.Country))
                {
                    Country = newAddress.Country;
                }               
            }
        }

        /// <summary>
        /// String version of the address.
        /// </summary>
        /// <returns>String of the address.</returns>
        public override string ToString()
        {
            StringBuilder sbAddress = new StringBuilder(Line1);

            if (!string.IsNullOrWhiteSpace(Line2))
            {
                if (sbAddress.Length > 0)
                {
                    sbAddress.Append(", ");
                }

                sbAddress.Append(Line2);
            }

            if (!string.IsNullOrWhiteSpace(Neighbourhood))
            {
                if (sbAddress.Length > 0)
                {
                    sbAddress.Append(", ");
                }

                sbAddress.Append(Neighbourhood);
            }

            return sbAddress.ToString();
        }

        /// <summary>
        /// String version of the full address.
        /// </summary>
        /// <returns>String of the address.</returns>
        public string ToStringComplete()
        {
            StringBuilder sbAddress = new StringBuilder(this.ToString());

            if (!string.IsNullOrWhiteSpace(City))
            {
                if (sbAddress.Length > 0)
                {
                    sbAddress.Append(", ");
                }

                sbAddress.Append(City);
            }

            if (!string.IsNullOrWhiteSpace(State))
            {
                if (sbAddress.Length > 0)
                {
                    sbAddress.Append(", ");
                }

                sbAddress.Append(State);
            }

            if (!string.IsNullOrWhiteSpace(Country))
            {
                if (sbAddress.Length > 0)
                {
                    sbAddress.Append(", ");
                }

                sbAddress.Append(Country);
            }

            return sbAddress.ToString();
        }
    }
}
