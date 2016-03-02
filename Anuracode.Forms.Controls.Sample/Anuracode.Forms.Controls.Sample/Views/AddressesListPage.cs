// <copyright file="AddressesListPage.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using Anuracode.Forms.Controls.Sample.ViewModels;
using Xamarin.Forms;

namespace Anuracode.Forms.Controls.Sample.Views
{
    /// <summary>
    /// Page for the addresses.
    /// </summary>
    public class AddressesListPage : ListBasePagedView<AddressListViewModel, Model.Address>
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="viewModel">View model to use.</param>
        public AddressesListPage(AddressListViewModel viewModel)
            : base(viewModel)
        {
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public AddressesListPage()
            : this(null)
        {
        }

        /// <summary>
        /// Set the properties for the label when there is no elements in the list.
        /// A binding is recomended.
        /// </summary>
        /// <param name="labelToSet">Label to set.</param>
        protected override void BindLabelNoElements(ExtendedLabel labelToSet)
        {
            base.BindLabelNoElements(labelToSet);

            labelToSet.SetBinding<AddressListViewModel>(Label.TextProperty, vm => vm.LocalizationResources.AddressesEmptyText);
        }

        /// <summary>
        /// Set the binding of the list of elements.
        /// </summary>
        /// <param name="ListElementsAll">List to use.</param>
        protected override void ConfigListElementsAll(ListView listElements)
        {
            listElements.ItemTemplate = new DataTemplate(
                () =>
                {
                    return new SimpleViewViewCell<AddressRowView>(1)
                    {
                        ElementDeleteCommand = ViewModel.DeleteItemCommand
                    };
                });
        }
    }
}
