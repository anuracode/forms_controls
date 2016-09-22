// <copyright file="RepeaterView.cs" company="">
// All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Anuracode.Forms.Controls
{
    /// <summary>
    /// Repeater view.
    /// </summary>
    /// <typeparam name="T">Type of the view.</typeparam>
    public class RepeaterView<T> : StackLayout
        where T : class
    {
        /// <summary>
        /// Definition for <see cref="ItemsSource"/>
        /// </summary>
        /// Element created at 15/11/2014,3:11 PM by Charles
        public static readonly BindableProperty IsUILocableProperty =
            BindablePropertyHelper.Create<RepeaterView<T>, bool>(
                nameof(IsUILocable),
                true,
                propertyChanged: IsUILocableChanged);

        /// <summary>
        /// Definition for <see cref="ItemsSource"/>
        /// </summary>
        /// Element created at 15/11/2014,3:11 PM by Charles
        public static readonly BindableProperty ItemsSourceProperty =
            BindablePropertyHelper.Create<RepeaterView<T>, IEnumerable<T>>(
                nameof(ItemsSource),
                Enumerable.Empty<T>(),
                propertyChanged: ItemsChanged);

        /// <summary>
        /// Definition for <see cref="ItemTemplate"/>
        /// </summary>
        /// Element created at 15/11/2014,3:11 PM by Charles
        public static readonly BindableProperty ItemTemplateProperty =
            BindablePropertyHelper.Create<RepeaterView<T>, DataTemplate>(
                nameof(ItemTemplate),
                default(DataTemplate));

        /// <summary>
        /// Definition for <see cref="TemplateSelector"/>
        /// </summary>
        /// Element created at 15/11/2014,3:12 PM by Charles
        public static readonly BindableProperty TemplateSelectorProperty =
            BindablePropertyHelper.Create<RepeaterView<T>, TemplateSelector>(
                nameof(TemplateSelector),
                default(TemplateSelector));

        /// <summary>
        /// Definition for <see cref="ItemClickCommand"/>
        /// </summary>
        /// Element created at 15/11/2014,3:11 PM by Charles
        public static BindableProperty ItemClickCommandProperty =
            BindablePropertyHelper.Create<RepeaterView<T>, ICommand>(nameof(ItemClickCommand), null);

        /// <summary>
        /// The Collection changed handler
        /// </summary>
        /// Element created at 15/11/2014,3:13 PM by Charles
        private IDisposable _collectionChangedHandle;

        /// <summary>
        /// Is loading flag.
        /// </summary>
        private bool isLoading;

        /// <summary>
        /// Initializes a new instance of the <see cref="RepeaterView{T}"/> class.
        /// </summary>
        /// Element created at 15/11/2014,3:13 PM by Charles
        public RepeaterView()
        {
            MinimumHeightRequest = 20;
            Spacing = 0;
        }

        /// <summary>
        /// Event delegate definition fo the <see cref="ItemCreated"/> event
        /// </summary>
        /// <param name="sender">The sender(this).</param>
        /// <param name="args">The <see cref="RepeaterViewItemAddedEventArgs"/> instance containing the event data.</param>
        /// Element created at 15/11/2014,3:12 PM by Charles
        public delegate void RepeaterViewItemAddedEventHandler(
            object sender,
            RepeaterViewItemAddedEventArgs args);

        /// <summary>Occurs when a view has been created.</summary>
        /// Element created at 15/11/2014,3:13 PM by Charles
        public event RepeaterViewItemAddedEventHandler ItemCreated;

        /// <summary>
        /// Is loading flag.
        /// </summary>
        public bool IsLoading
        {
            get
            {
                return isLoading;
            }

            protected set
            {
                if (isLoading != value)
                {
                    OnPropertyChanging("IsLoading");
                    isLoading = value;
                    OnPropertyChanged("IsLoading");
                }
            }
        }

        /// <summary>
        /// Flag that limits the rendering to the UI lock.
        /// </summary>
        public bool IsUILocable
        {
            get
            {
                return (bool)GetValue(IsUILocableProperty);
            }

            set
            {
                SetValue(IsUILocableProperty, value);
            }
        }

        /// <summary>Gets or sets the item click command.</summary>
        /// <value>The item click command.</value>
        /// Element created at 15/11/2014,3:13 PM by Charles
        public ICommand ItemClickCommand
        {
            get { return (ICommand)this.GetValue(ItemClickCommandProperty); }
            set { SetValue(ItemClickCommandProperty, value); }
        }

        /// <summary>Gets or sets the items source.</summary>
        /// <value>The items source.</value>
        /// Element created at 15/11/2014,3:13 PM by Charles
        public IEnumerable<T> ItemsSource
        {
            get { return (IEnumerable<T>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        /// <summary>
        /// The item template property
        /// This can be used on it's own or in combination with
        /// the <see cref="TemplateSelector"/>
        /// </summary>
        /// Element created at 15/11/2014,3:10 PM by Charles
        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        /// <summary>Gets or sets the template selector.</summary>
        /// <value>The template selector.</value>
        /// Element created at 15/11/2014,3:13 PM by Charles
        public TemplateSelector TemplateSelector
        {
            get { return (TemplateSelector)GetValue(TemplateSelectorProperty); }
            set { SetValue(TemplateSelectorProperty, value); }
        }

        /// <summary>
        /// Select a datatemplate dynamically
        /// Prefer the TemplateSelector then the DataTemplate
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected virtual Task<DataTemplate> GetTemplateFor(Type type)
        {
            TaskCompletionSource<DataTemplate> tc = new TaskCompletionSource<DataTemplate>();

            AC.ScheduleManagedBackground(
                async () =>
                {
                    await Task.FromResult(0);

                    try
                    {
                        DataTemplate retTemplate = null;
                        if (TemplateSelector != null)
                        {
                            retTemplate = TemplateSelector.TemplateFor(type);
                        }

                        tc.TrySetResult(retTemplate ?? ItemTemplate);
                    }
                    catch (Exception ex)
                    {
                        tc.TrySetException(ex);
                    }
                });

            return tc.Task;
        }

        /// <summary>
        /// Gives codebehind a chance to play with the
        /// newly created view object :D
        /// </summary>
        /// <param name="view">The visual view object</param>
        /// <param name="model">The item being added</param>
        protected virtual void NotifyItemAdded(View view, T model)
        {
            if (ItemCreated != null)
            {
                ItemCreated(this, new RepeaterViewItemAddedEventArgs(view, model));
            }
        }

        /// <summary>
        /// Creates a view based on the items type
        /// While we do have T, T could very well be
        /// a common superclass or an interface by
        /// using the items actual type we support
        /// both inheritance based polymorphism
        /// and shape based polymorphism
        ///
        /// </summary>
        /// <param name="item"></param>
        /// <returns>A View that has been initialized with <see cref="item"/> as it's BindingContext</returns>
        /// <exception cref="InvalidVisualObjectException"></exception>Thrown when the matched datatemplate inflates to an object not derived from either
        /// <see cref="Xamarin.Forms.View"/> or <see cref="Xamarin.Forms.ViewCell"/>
        protected virtual async Task<View> ViewFor(T item)
        {
            var template = await GetTemplateFor(item.GetType());

            var content = template.CreateContent();

            if (!(content is View) && !(content is ViewCell))
            {
                throw new ArgumentException(content.GetType().Name);
            }

            var view = (content is View) ? content as View : ((ViewCell)content).View;
            view.BindingContext = item;

            if (ItemClickCommand != null)
            {
                var gestureRecognizer = new TapGestureRecognizer { Command = ItemClickCommand, CommandParameter = item };
                view.GestureRecognizers.Add(gestureRecognizer);
            }
            return view;
        }

        /// <summary>
        /// Validate the change.
        /// </summary>
        /// <param name="bindable">The control</param>
        /// <param name="oldValue">Previous bound collection</param>
        /// <param name="newValue">New bound collection</param>
        private static void IsUILocableChanged(
            BindableObject bindable,
            object oldValue,
            object newValue)
        {
            var control = bindable as RepeaterView<T>;
            if (control == null)
                throw new Exception(
                    "Invalid bindable object passed to ReapterView::IsUILocableChanged expected a ReapterView<T> received a "
                    + bindable.GetType().Name);

            if (control._collectionChangedHandle != null)
            {
                throw new Exception("IsUILocable can only be changed before the item source is assigned");
            }
        }

        /// <summary>
        /// Reset the collection of bound objects
        /// Remove the old collection changed eventhandler (if any)
        /// Create new cells for each new item
        /// </summary>
        /// <param name="bindable">The control</param>
        /// <param name="oldValue">Previous bound collection</param>
        /// <param name="newValue">New bound collection</param>
        private static void ItemsChanged(
            BindableObject bindable,
            object oldValue,
            object newValueObject)
        {
            IEnumerable<T> newValue = newValueObject as IEnumerable<T>;
            var control = bindable as RepeaterView<T>;
            if (control == null)
            {
                throw new Exception("Invalid bindable object passed to ReapterView::ItemsChanged expected a ReapterView<T> received a " + bindable.GetType().Name);
            }

            if (control._collectionChangedHandle != null)
            {
                control._collectionChangedHandle.Dispose();
            }

            control._collectionChangedHandle = new CollectionChangedHandle<View, T>(
                control,
                control.Children,
                newValue,
                control.ViewFor,
                (v, m, i) => control.NotifyItemAdded(v, m),
                isUILockable: control.IsUILocable,
                isLoadingAction: (l) => { control.IsLoading = l; });
        }
    }

    /// <summary>
    /// Argument for the <see cref="RepeaterView{T}.ItemCreated"/> event
    /// </summary>
    /// Element created at 15/11/2014,3:13 PM by Charles
    public class RepeaterViewItemAddedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RepeaterViewItemAddedEventArgs"/> class.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="model">The model.</param>
        /// Element created at 15/11/2014,3:14 PM by Charles
        public RepeaterViewItemAddedEventArgs(View view, object model)
        {
            View = view;
            Model = model;
        }

        /// <summary>Gets or sets the model.</summary>
        /// <value>The original viewmodel.</value>
        /// Element created at 15/11/2014,3:14 PM by Charles
        public object Model { get; set; }

        /// <summary>Gets or sets the view.</summary>
        /// <value>The visual element.</value>
        /// Element created at 15/11/2014,3:14 PM by Charles
        public View View { get; set; }
    }
}