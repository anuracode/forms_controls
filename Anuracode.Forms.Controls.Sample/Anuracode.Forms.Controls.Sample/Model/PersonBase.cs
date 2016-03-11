// <copyright file="PersonBase.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using System.Runtime.Serialization;

namespace Anuracode.Forms.Controls.Sample.Model
{
    /// <summary>
    /// Model for a person.
    /// </summary>
    [DataContract]
    [KnownType(typeof(Address))]
    [KnownType(typeof(Client))]
    public partial class PersonBase : BaseEntity
    {
        /// <summary>
        /// Email of the person.
        /// </summary>
        private string email = string.Empty;

        /// <summary>
        /// GUID of the element.
        /// </summary>
        private string id;

        /// <summary>
        /// Name of the entity.
        /// </summary>
        private string name = string.Empty;

        /// <summary>
        /// Nick name.
        /// </summary>
        private string nickName = string.Empty;

        /// <summary>
        /// Value of the role.
        /// </summary>
        private int roleInt;

        /// <summary>
        /// Email of the person.
        /// </summary>
        [DataMember]
        public string Email
        {
            get
            {
                return email;
            }

            set
            {
                ValidateRaiseAndSetIfChanged(ref email, value);
            }
        }

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
        /// Name of the entity.
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
        /// Nick name.
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
    }
}