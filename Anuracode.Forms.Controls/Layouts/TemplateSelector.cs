// <copyright file="SimpleViewViewRowCell.cs" company="">
// All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;

namespace Anuracode.Forms.Controls
{
    /// <summary>
    /// Interface to enable DataTemplateCollection to hold
    /// typesafe instances of DataTemplateWrapper
    /// </summary>
    public interface IDataTemplateWrapper
    {
        bool IsDefault { get; set; }

        Type Type { get; }

        DataTemplate WrappedTemplate { get; set; }
    }

    /// <summary>
    /// Collection class of IDataTemplateWrapper
    /// Enables xaml definitions of collections.
    /// </summary>
    public class DataTemplateCollection : ObservableCollection<IDataTemplateWrapper> { }

    /// <summary>
    /// Wrapper for a DataTemplate.
    /// Unfortunately the default constructor for DataTemplate is internal
    /// so I had to wrap the DataTemplate instead of inheriting it.
    /// </summary>
    /// <typeparam name="T">The object type that this DataTemplateWrapper matches</typeparam>
    public class DataTemplateWrapper<T> : BindableObject, IDataTemplateWrapper
    {
        public static readonly BindableProperty IsDefaultProperty = BindablePropertyHelper.Create<DataTemplateWrapper<T>, bool>(nameof(IsDefault), false);
        public static readonly BindableProperty WrappedTemplateProperty = BindablePropertyHelper.Create<DataTemplateWrapper<T>, DataTemplate>(nameof(WrappedTemplate), null);

        public bool IsDefault
        {
            get { return (bool)GetValue(IsDefaultProperty); }
            set { SetValue(IsDefaultProperty, value); }
        }

        public Type Type
        {
            get { return typeof(T); }
        }

        public DataTemplate WrappedTemplate
        {
            get { return (DataTemplate)GetValue(WrappedTemplateProperty); }
            set { SetValue(WrappedTemplateProperty, value); }
        }
    }

    /// <summary>
    /// Template selector.
    /// </summary>
    public class TemplateSelector : BindableObject
    {
        /// <summary>
        /// Property definition for the <see cref="ExceptionOnNoMatch"/> Bindable Property
        /// </summary>
        public static BindableProperty ExceptionOnNoMatchProperty = BindablePropertyHelper.Create<TemplateSelector, bool>(nameof(ExceptionOnNoMatch), true);

        /// <summary>
        /// Property definition for the <see cref="SelectorFunction"/> Bindable Property
        /// </summary>
        public static BindableProperty SelectorFunctionProperty = BindablePropertyHelper.Create<TemplateSelector, Func<Type, DataTemplate>>(nameof(SelectorFunction), null);

        /// <summary>
        /// Property definition for the <see cref="Templates"/> Bindable Property
        /// </summary>
        public static BindableProperty TemplatesProperty = BindablePropertyHelper.Create<TemplateSelector, DataTemplateCollection>(nameof(Templates), default(DataTemplateCollection), BindingMode.OneWay, null, TemplatesChanged);

        /// <summary>
        /// Initialize the TemplateCollections so that each
        /// instance gets it's own collection
        /// </summary>
        public TemplateSelector()
        {
            Templates = new DataTemplateCollection();
        }

        /// <summary>
        /// Bindable property that allows the user to
        /// determine if a <see cref="NoDataTemplateMatchException"/> is thrown when
        /// there is no matching template found
        /// </summary>
        public bool ExceptionOnNoMatch
        {
            get { return (bool)GetValue(ExceptionOnNoMatchProperty); }
            set { SetValue(ExceptionOnNoMatchProperty, value); }
        }

        /// <summary>
        /// A user supplied function of type
        /// <code>Func<typeparamname name="Type"></typeparamname>,<typeparamname name="DataTemplate"></typeparamname></code>
        /// If this function has been supplied it is always called first in the match
        /// process.
        /// </summary>
        public Func<Type, DataTemplate> SelectorFunction
        {
            get { return (Func<Type, DataTemplate>)GetValue(SelectorFunctionProperty); }
            set { SetValue(SelectorFunctionProperty, value); }
        }

        /// <summary>
        /// The collection of DataTemplates
        /// </summary>
        public DataTemplateCollection Templates
        {
            get { return (DataTemplateCollection)GetValue(TemplatesProperty); }
            set { SetValue(TemplatesProperty, value); }
        }

        /// <summary>
        /// Private cache of matched types with datatemplates
        /// The cache is reset on any change to <see cref="Templates"/>
        /// </summary>
        private Dictionary<Type, DataTemplate> Cache { get; set; }

        /// <summary>
        ///  Clears the cache when the set of templates change
        /// </summary>
        /// <param name="bo">Bindable object.</param>
        /// <param name="oldval">Old value.</param>
        /// <param name="newval">New value.</param>
        public static void TemplatesChanged(BindableObject bo, object oldvalObject, object newvalObject)
        {
            DataTemplateCollection oldval = oldvalObject as DataTemplateCollection;
            DataTemplateCollection newval = newvalObject as DataTemplateCollection;

            var ts = bo as TemplateSelector;
            if (ts == null)
            {
                return;
            }

            if (oldval != null)
            {
                oldval.CollectionChanged -= ts.TemplateSetChanged;
            }

            if (newval != null)
            {
                newval.CollectionChanged += ts.TemplateSetChanged;
            }

            ts.Cache = null;
        }

        /// <summary>
        /// Matches a type with a datatemplate
        /// Order of matching=>
        ///     SelectorFunction,
        ///     Cache,
        ///     SpecificTypeMatch,
        ///     InterfaceMatch,
        ///     BaseTypeMatch
        ///     DefaultTempalte
        /// </summary>
        /// <param name="type">Type object type that needs a datatemplate</param>
        /// <returns>The DataTemplate from the WrappedDataTemplates Collection that closest matches
        /// the type paramater.</returns>
        /// <exception cref="NoDataTemplateMatchException"></exception>Thrown if there is no datatemplate that matches the supplied type
        public DataTemplate TemplateFor(Type type)
        {
            var typesExamined = new List<Type>();
            var template = TemplateForImpl(type, typesExamined);
            if (template == null && ExceptionOnNoMatch)
                throw new ArgumentOutOfRangeException(type.Name, typesExamined.ToString());
            return template;
        }

        /// <summary>
        /// Finds a template for the type of the passed in item (<code>item.GetType()</code>)
        /// and creates the content and sets the Binding context of the View
        /// Currently the root of the DataTemplate must be a ViewCell.
        /// </summary>
        /// <param name="item">The item to instantiate a DataTemplate for</param>
        /// <returns>a View with it's binding context set</returns>
        /// <exception cref="InvalidVisualObjectException"></exception>Thrown when the matched datatemplate inflates to an object not derived from either
        /// <see cref="Xamarin.Forms.View"/> or <see cref="Xamarin.Forms.ViewCell"/>
        public View ViewFor(object item)
        {
            var template = TemplateFor(item.GetType());
            var content = template.CreateContent();
            if (!(content is View) && !(content is ViewCell))
                throw new ArgumentException(content.GetType().Name);

            var view = (content is View) ? content as View : ((ViewCell)content).View;
            view.BindingContext = item;
            return view;
        }

        /// <summary>
        /// Interal implementation of <see cref="TemplateFor"/>.
        /// </summary>
        /// <param name="type">The type to match on</param>
        /// <param name="examined">A list of all types examined during the matching process</param>
        /// <returns>A DataTemplate or null</returns>
        private DataTemplate TemplateForImpl(Type type, List<Type> examined)
        {
            if (type == null) return null;//This can happen when we recusively check base types (object.BaseType==null)
            examined.Add(type);
            Contract.Assert(Templates != null, "Templates cannot be null");

            Cache = Cache ?? new Dictionary<Type, DataTemplate>();
            DataTemplate retTemplate = null;

            //Prefer the selector function if present
            //This has been moved before the cache check so that
            //the user supplied function has an opportunity to
            //Make a decision with more information than simply
            //the requested type (perhaps the Ux or Network states...)
            if (SelectorFunction != null) retTemplate = SelectorFunction(type);

            //Happy case we already have the type in our cache
            if (Cache.ContainsKey(type)) return Cache[type];

            //check our list
            retTemplate = Templates.Where(x => x.Type == type).Select(x => x.WrappedTemplate).FirstOrDefault();
            //Check for interfaces
            retTemplate = retTemplate ?? type.GetTypeInfo().ImplementedInterfaces.Select(x => TemplateForImpl(x, examined)).FirstOrDefault();
            //look at base types
            retTemplate = retTemplate ?? TemplateForImpl(type.GetTypeInfo().BaseType, examined);
            //If all else fails try to find a Default Template
            retTemplate = retTemplate ?? Templates.Where(x => x.IsDefault).Select(x => x.WrappedTemplate).FirstOrDefault();

            Cache[type] = retTemplate;
            return retTemplate;
        }

        /// <summary>
        /// Clear the cache on any template set change
        /// If needed this could be optimized to care about the specific
        /// change but I doubt it would be worthwhile.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TemplateSetChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Cache = null;
        }
    }
}