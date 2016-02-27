// <copyright file="SimpleViewViewCell.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Anuracode.Forms.Controls
{
    /// <summary>
    /// Generic for making a fastgrid cell from a simple view.
    /// </summary>
    /// <typeparam name="TView">Type to use.</typeparam>
    public class SimpleViewViewCell<TView> : ViewCell
        where TView : SimpleViewBase, new()
    {
        /// <summary>
        /// Is recylced.
        /// </summary>
        private bool isRecycled;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="renderType">1 if it has delete, other value must be overrided.</param>
        public SimpleViewViewCell(int renderType = 0)
            : base()
        {
            if (InnerView == null)
            {
                InnerView = new TView();
                this.View = InnerView;
            }

            AddContextActions(renderType);
        }

        /// <summary>
        /// Action when delete.
        /// </summary>
        public ICommand ElementDeleteCommand { get; set; }

        /// <summary>
        /// Inner view.
        /// </summary>
        protected TView InnerView { get; set; }

        /// <summary>
        /// The binding is already set.
        /// </summary>
        protected bool IsBindingSet { get; set; }

        /// <summary>
        /// Add context actions.
        /// </summary>
        /// <param name="renderType">Flag for the actions to render.</param>
        protected virtual void AddContextActions(int renderType)
        {
            if (renderType == 1)
            {
                var mitDelete = new MenuItem()
                {
                    Icon = GetDeleteImagePath(),
                    IsDestructive = true,
                    Text = GetDeleteText()
                };

                mitDelete.Clicked += Delete_Clicked;
                ContextActions.Add(mitDelete);
            }
        }

        /// <summary>
        /// Button clicked.
        /// </summary>
        /// <param name="sender">Sender event.</param>
        /// <param name="e">Arguments event.</param>
        protected virtual void Delete_Clicked(object sender, EventArgs e)
        {
            if (ElementDeleteCommand != null)
            {
                AC.ScheduleManaged(
                    () =>
                    {
                        if (ElementDeleteCommand.CanExecute(BindingContext))
                        {
                            ElementDeleteCommand.Execute(BindingContext);
                        }

                        return Task.FromResult(0);
                    });
            }
        }

        /// <summary>
        /// Get the image path for the delete.
        /// </summary>
        /// <returns>Path to use.</returns>
        protected virtual string GetDeleteImagePath()
        {
            return "appbar_delete.png";
        }

        /// <summary>
        /// Get the text for delete.
        /// </summary>
        /// <returns>Path to use.</returns>
        protected virtual string GetDeleteText()
        {
            return "Delete";
        }

        /// <summary>
        /// When binding context changes.
        /// </summary>
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (InnerView != null)
            {
                InnerView.InitializeView();

                InnerView.BindingContext = this.BindingContext;

                if (!IsBindingSet)
                {
                    IsBindingSet = true;
                    InnerView.PrepareBindings();
                }

                InnerView.SetupViewValues(isRecycled);

                if (isRecycled)
                {
                    InnerView.RecycleView();
                }

                if (!isRecycled)
                {
                    isRecycled = true;
                }
            }
        }
    }
}