// <copyright file="StoreItemLevelSimpleView.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using Anuracode.Forms.Controls.Extensions;
using Anuracode.Forms.Controls.Views.Extensions;
using Anuracode.Forms.Controls.Sample.Model;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Anuracode.Forms.Controls.Sample.Views
{
    /// <summary>
    /// Level as a simple view.
    /// </summary>
    public class StoreItemLevelSimpleView : SimpleViewBase
    {
        /// <summary>
        /// Separator for the meta.
        /// </summary>
        private const string META_SEPARATOR = "> ";

        /// <summary>
        /// Background custom color.
        /// </summary>
        private Color backgroundCustomColor = Theme.CommonResources.AccentAlternative;

        /// <summary>
        /// Background margin.
        /// </summary>
        private double backgroundMargin = 2.5;

        /// <summary>
        /// Navigate to store level.
        /// </summary>
        private Command<StoreItemLevel> internalNavigateToStoreLevelCommand;

        /// <summary>
        /// Action when navigate.
        /// </summary>
        private ICommand navigateToStoreLevelCommand;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="renderLastLevel">Render only last level.</param>
        /// <param name="renderReadOnly">Render with labels.</param>
        /// <param name="isNavigationMode">True when navigate to previous level to go back.</param>
        /// <param name="renderBackground">True to render the category background.</param>
        public StoreItemLevelSimpleView(bool renderLastLevel, bool renderReadOnly = true, bool isNavigationMode = false, bool renderBackground = false) :
            base(false)
        {
            ContentLayout.IsClippedToBounds = false;

            RenderBackground = renderBackground;
            RenderLastLevel = renderLastLevel;
            RenderReadOnly = renderReadOnly;
            IsNavigationMode = isNavigationMode;

            InitializeView();
        }

        /// <summary>
        /// Background custom color.
        /// </summary>
        public Color BackgroundCustomColor
        {
            get
            {
                return backgroundCustomColor;
            }

            set
            {
                backgroundCustomColor = value;

                if (NavBarBack != null)
                {
                    NavBarBack.Color = value;
                }
            }
        }

        /// <summary>
        /// Background margin.
        /// </summary>
        public double BackgroundMargin
        {
            get
            {
                return backgroundMargin;
            }

            set
            {
                backgroundMargin = value;
            }
        }

        /// <summary>
        /// Size to mode the background to the left.
        /// </summary>
        public double BackgroundTranslateX { get; set; }

        /// <summary>
        /// Is navigation mode.
        /// </summary>
        public bool IsNavigationMode { get; protected set; }

        /// <summary>
        /// Action to navigate back.
        /// </summary>
        public ICommand NavigateBackCommand { get; set; }

        /// <summary>
        /// Action when navigate.
        /// </summary>
        public ICommand NavigateToStoreLevelCommand
        {
            get
            {
                return navigateToStoreLevelCommand;
            }

            set
            {
                navigateToStoreLevelCommand = value;

                InternalNavigateToStoreLevelCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Render last level.
        /// </summary>
        public bool RenderLastLevel { get; protected set; }

        /// <summary>
        /// Render read only.
        /// </summary>
        public bool RenderReadOnly { get; protected set; }

        /// <summary>
        /// Button category.
        /// </summary>
        protected TextContentViewButton ButtonCategory { get; set; }

        /// <summary>
        /// Button department.
        /// </summary>
        protected TextContentViewButton ButtonDepartment { get; set; }

        /// <summary>
        /// Button subcategory.
        /// </summary>
        protected TextContentViewButton ButtonSubcategory { get; set; }

        /// <summary>
        /// Navigate to store level.
        /// </summary>
        protected Command<StoreItemLevel> InternalNavigateToStoreLevelCommand
        {
            get
            {
                if (internalNavigateToStoreLevelCommand == null)
                {
                    internalNavigateToStoreLevelCommand = new Command<StoreItemLevel>(
                        async (subLevel) =>
                        {
                            await Task.FromResult(0);

                            if (IsNavigationMode)
                            {
                                int navigationLevel = 0;

                                StoreItemLevel itemLevel = BindingContext as StoreItemLevel;

                                if (!string.IsNullOrWhiteSpace(itemLevel.Department))
                                {
                                    navigationLevel++;
                                }

                                if (!string.IsNullOrWhiteSpace(itemLevel.Category))
                                {
                                    navigationLevel++;
                                }

                                if (!string.IsNullOrWhiteSpace(itemLevel.Subcategory))
                                {
                                    navigationLevel++;
                                }

                                int subLevelLevel = 0;

                                if (!string.IsNullOrWhiteSpace(subLevel.Department))
                                {
                                    subLevelLevel++;
                                }

                                if (!string.IsNullOrWhiteSpace(subLevel.Category))
                                {
                                    subLevelLevel++;
                                }

                                if (!string.IsNullOrWhiteSpace(subLevel.Subcategory))
                                {
                                    subLevelLevel++;
                                }

                                int netLevel = navigationLevel - subLevelLevel;

                                if (netLevel == 1)
                                {
                                    if ((NavigateBackCommand != null))
                                    {
                                        NavigateBackCommand.ExecuteIfCan();
                                    }
                                }
                                else if (netLevel > 1)
                                {
                                    if ((NavigateToStoreLevelCommand != null) && NavigateToStoreLevelCommand.CanExecute(subLevel))
                                    {
                                        NavigateToStoreLevelCommand.Execute(subLevel);
                                    }
                                }
                            }
                            else
                            {
                                if ((NavigateToStoreLevelCommand != null) && NavigateToStoreLevelCommand.CanExecute(subLevel))
                                {
                                    NavigateToStoreLevelCommand.Execute(subLevel);
                                }
                            }
                        },
                        (subLevel) =>
                        {
                            return (subLevel != null) && (NavigateToStoreLevelCommand != null) && NavigateToStoreLevelCommand.CanExecute(subLevel);
                        });
                }

                return internalNavigateToStoreLevelCommand;
            }
        }

        /// <summary>
        /// label category.
        /// </summary>
        protected ExtendedLabel LabelCategory { get; set; }

        /// <summary>
        /// Separator of the category and subcategory.
        /// </summary>
        protected ExtendedLabel LabelCategorySeparator { get; set; }

        /// <summary>
        /// Label department.
        /// </summary>
        protected ExtendedLabel LabelDepartment { get; set; }

        /// <summary>
        /// Separator of the department and category.
        /// </summary>
        protected ExtendedLabel LabelDepartmentSeparator { get; set; }

        /// <summary>
        /// label category.
        /// </summary>
        protected ExtendedLabel LabelSubcategory { get; set; }

        /// <summary>
        /// Nav bar back.
        /// </summary>
        protected ShapeView NavBarBack { get; set; }

        /// <summary>
        /// Render the category background.
        /// </summary>
        protected bool RenderBackground { get; set; }

        /// <summary>
        /// Button for the last level.
        /// </summary>
        protected GlyphContentViewButton SubLevelButton { get; set; }

        /// <summary>
        /// Add controls to layout.
        /// </summary>
        protected override void AddControlsToLayout()
        {
            AddViewToLayout(NavBarBack);
            AddViewToLayout(SubLevelButton);
            AddViewToLayout(LabelDepartment);
            AddViewToLayout(LabelDepartmentSeparator);
            AddViewToLayout(LabelCategory);
            AddViewToLayout(LabelCategorySeparator);
            AddViewToLayout(LabelSubcategory);
            AddViewToLayout(ButtonDepartment);
            AddViewToLayout(ButtonCategory);
            AddViewToLayout(ButtonSubcategory);
        }

        /// <summary>
        /// Layout children.
        /// </summary>
        /// <param name="x">Top to use.</param>
        /// <param name="y">Left to use.</param>
        /// <param name="width">Width to use.</param>
        /// <param name="height">Height to use.</param>
        protected override void ContentLayout_OnLayoutChildren(double x, double y, double width, double height)
        {
            if (RenderLastLevel)
            {
                if (SubLevelButton != null)
                {
                    var elementSize = SubLevelButton.GetSizeRequest(width, height).Request;

                    double elementLeft = Margin;
                    double elementTop = 0;
                    double elementWidth = elementSize.Width;
                    double elementHeight = elementSize.Height;

                    var elementPosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);

                    SubLevelButton.LayoutUpdate(elementPosition);
                }
            }
            else
            {
                Rectangle departmentPosition = new Rectangle();
                Rectangle departmentSeparatorPosition = new Rectangle();
                Rectangle categoryPosition = new Rectangle();
                Rectangle categorySeparatorPosition = new Rectangle();
                Rectangle subcategoryPosition = new Rectangle();
                Rectangle previousPosition = new Rectangle();

                if (RenderReadOnly)
                {
                    if ((LabelDepartment != null) && LabelDepartment.IsVisible)
                    {
                        var elementSize = LabelDepartment.GetSizeRequest(width, height).Request;

                        double elementLeft = Margin;
                        double elementTop = 0;
                        double elementWidth = elementSize.Width;
                        double elementHeight = elementSize.Height;

                        departmentPosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);

                        LabelDepartment.LayoutUpdate(departmentPosition);
                    }

                    previousPosition = departmentPosition;

                    if ((LabelDepartmentSeparator != null) && LabelDepartmentSeparator.IsVisible)
                    {
                        var elementSize = LabelDepartmentSeparator.GetSizeRequest(width, height).Request;

                        double elementLeft = previousPosition.X + previousPosition.Width;
                        double elementTop = previousPosition.Y;
                        double elementWidth = elementSize.Width;
                        double elementHeight = elementSize.Height;

                        departmentSeparatorPosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);

                        LabelDepartmentSeparator.LayoutUpdate(departmentSeparatorPosition);
                    }

                    previousPosition = departmentSeparatorPosition;

                    if ((LabelCategory != null) && LabelCategory.IsVisible)
                    {
                        var elementSize = LabelCategory.GetSizeRequest(width, height).Request;

                        double elementLeft = previousPosition.X + previousPosition.Width;
                        double elementTop = previousPosition.Y;
                        double elementWidth = elementSize.Width;
                        double elementHeight = elementSize.Height;

                        categoryPosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);

                        LabelCategory.LayoutUpdate(categoryPosition);
                    }

                    previousPosition = categoryPosition;

                    if ((LabelCategorySeparator != null) && LabelCategorySeparator.IsVisible)
                    {
                        var elementSize = LabelCategorySeparator.GetSizeRequest(width, height).Request;

                        double elementLeft = previousPosition.X + previousPosition.Width;
                        double elementTop = previousPosition.Y;
                        double elementWidth = elementSize.Width;
                        double elementHeight = elementSize.Height;

                        categorySeparatorPosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);

                        LabelCategorySeparator.LayoutUpdate(categorySeparatorPosition);
                    }

                    previousPosition = categorySeparatorPosition;

                    if ((LabelSubcategory != null) && LabelSubcategory.IsVisible)
                    {
                        var elementSize = LabelSubcategory.GetSizeRequest(width, height).Request;

                        double elementLeft = previousPosition.X + previousPosition.Width;
                        double elementTop = previousPosition.Y;
                        double elementWidth = elementSize.Width;
                        double elementHeight = elementSize.Height;

                        subcategoryPosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);

                        LabelSubcategory.LayoutUpdate(subcategoryPosition);
                    }
                }
                else
                {
                    if ((ButtonDepartment != null) && ButtonDepartment.IsVisible)
                    {
                        var elementSize = ButtonDepartment.GetSizeRequest(width, height).Request;

                        double elementLeft = Margin;
                        double elementTop = 0;
                        double elementWidth = elementSize.Width;
                        double elementHeight = elementSize.Height;

                        departmentPosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);

                        ButtonDepartment.LayoutUpdate(departmentPosition);
                    }

                    previousPosition = departmentPosition;

                    if ((LabelDepartmentSeparator != null) && LabelDepartmentSeparator.IsVisible)
                    {
                        var elementSize = LabelDepartmentSeparator.GetSizeRequest(width, height).Request;

                        double elementLeft = previousPosition.X + previousPosition.Width;
                        double elementTop = previousPosition.Y;
                        double elementWidth = elementSize.Width;
                        double elementHeight = elementSize.Height;

                        departmentSeparatorPosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);

                        LabelDepartmentSeparator.LayoutUpdate(departmentSeparatorPosition);
                    }

                    previousPosition = departmentSeparatorPosition;

                    if ((ButtonCategory != null) && ButtonCategory.IsVisible)
                    {
                        var elementSize = ButtonCategory.GetSizeRequest(width, height).Request;

                        double elementLeft = previousPosition.X + previousPosition.Width;
                        double elementTop = previousPosition.Y;
                        double elementWidth = elementSize.Width;
                        double elementHeight = elementSize.Height;

                        categoryPosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);

                        ButtonCategory.LayoutUpdate(categoryPosition);
                    }

                    previousPosition = categoryPosition;

                    if ((LabelCategorySeparator != null) && LabelCategorySeparator.IsVisible)
                    {
                        var elementSize = LabelCategorySeparator.GetSizeRequest(width, height).Request;

                        double elementLeft = previousPosition.X + previousPosition.Width;
                        double elementTop = previousPosition.Y;
                        double elementWidth = elementSize.Width;
                        double elementHeight = elementSize.Height;

                        categorySeparatorPosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);

                        LabelCategorySeparator.LayoutUpdate(categorySeparatorPosition);
                    }

                    previousPosition = categorySeparatorPosition;

                    if ((ButtonSubcategory != null) && ButtonSubcategory.IsVisible)
                    {
                        var elementSize = ButtonSubcategory.GetSizeRequest(width, height).Request;

                        double elementLeft = previousPosition.X + previousPosition.Width;
                        double elementTop = previousPosition.Y;
                        double elementWidth = elementSize.Width;
                        double elementHeight = elementSize.Height;

                        subcategoryPosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);

                        ButtonSubcategory.LayoutUpdate(subcategoryPosition);
                    }
                }

                if (NavBarBack != null)
                {
                    double elementLeft = departmentPosition.X - (BackgroundMargin + BackgroundTranslateX);
                    double elementTop = departmentPosition.Y - (BackgroundMargin * 0.5f);
                    double elementHeight = departmentPosition.Height + (BackgroundMargin * 1f);

                    double calculatedWidth = departmentPosition.Width + departmentSeparatorPosition.Width + categoryPosition.Width + categorySeparatorPosition.Width + subcategoryPosition.Width;
                    double elementWidth = calculatedWidth + (BackgroundTranslateX * 2f) + (BackgroundMargin * 2f);

                    var elementPosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);

                    NavBarBack.LayoutUpdate(elementPosition);
                }
            }
        }

        /// <summary>
        /// Mesure the content.
        /// </summary>
        /// <param name="widthConstraint">Width to constarint.</param>
        /// <param name="heightConstraint">Height to constraint.</param>
        /// <returns>Size to use.</returns>
        protected override SizeRequest ContentLayout_OnSizeRequest(double widthConstraint, double heightConstraint)
        {
            Size levelOnlyButton = new Size();
            Size departmentSize = new Size();
            Size categorySize = new Size();
            Size subcategorySize = new Size();
            Size departmentSeparatorSize = new Size();
            Size categorySeparatorSize = new Size();

            if ((SubLevelButton) != null && SubLevelButton.IsVisible)
            {
                levelOnlyButton = SubLevelButton.GetSizeRequest(widthConstraint, heightConstraint).Request;
            }

            if ((LabelDepartment) != null && LabelDepartment.IsVisible)
            {
                departmentSize = LabelDepartment.GetSizeRequest(widthConstraint, heightConstraint).Request;
            }

            if ((LabelCategory) != null && LabelCategory.IsVisible)
            {
                categorySize = LabelCategory.GetSizeRequest(widthConstraint, heightConstraint).Request;
            }

            if ((LabelSubcategory) != null && LabelSubcategory.IsVisible)
            {
                subcategorySize = LabelSubcategory.GetSizeRequest(widthConstraint, heightConstraint).Request;
            }

            if ((ButtonDepartment) != null && ButtonDepartment.IsVisible)
            {
                departmentSize = ButtonDepartment.GetSizeRequest(widthConstraint, heightConstraint).Request;
            }

            if ((ButtonCategory) != null && ButtonCategory.IsVisible)
            {
                categorySize = ButtonCategory.GetSizeRequest(widthConstraint, heightConstraint).Request;
            }

            if ((ButtonSubcategory) != null && ButtonSubcategory.IsVisible)
            {
                subcategorySize = ButtonSubcategory.GetSizeRequest(widthConstraint, heightConstraint).Request;
            }

            if ((LabelDepartmentSeparator) != null && LabelDepartmentSeparator.IsVisible)
            {
                departmentSeparatorSize = LabelDepartmentSeparator.GetSizeRequest(widthConstraint, heightConstraint).Request;
            }

            if ((LabelCategorySeparator) != null && LabelCategorySeparator.IsVisible)
            {
                categorySeparatorSize = LabelCategorySeparator.GetSizeRequest(widthConstraint, heightConstraint).Request;
            }

            double calculatedHeigh = Math.Max(levelOnlyButton.Height, Math.Max(departmentSize.Height, Math.Max(categorySize.Height, Math.Max(subcategorySize.Height, Math.Max(departmentSeparatorSize.Height, categorySeparatorSize.Height)))));
            double calculatedWidth = levelOnlyButton.Width + departmentSize.Width + categorySize.Width + subcategorySize.Width + departmentSeparatorSize.Width + categorySeparatorSize.Width + (Margin * 2f);

            return new SizeRequest(new Size(calculatedWidth, calculatedHeigh), new Size(calculatedWidth, calculatedHeigh));
        }

        /// <summary>
        /// Instance elements.
        /// </summary>
        protected override void InternalInitializeView()
        {
            if (RenderBackground)
            {
                NavBarBack = new ShapeView()
                {
                    Color = BackgroundCustomColor,
                    ShapeType = ShapeType.Box,
                    CornerRadius = 15
                };
            }

            if (RenderLastLevel)
            {
                SubLevelButton = new GlyphContentViewButton(true, true, ImageOrientation.ImageToLeft)
                {
                    Style = Theme.ApplicationStyles.TextOnlyContentButtonStyle,
                    MinimumWidthRequest = 120,
                    TextColor = Theme.CommonResources.Accent,
                    GlyphTextColor = Theme.CommonResources.Accent,
                    GlyphText = Theme.CommonResources.GlyphTextCategoryBullet,
                    MarginElements = 5,
                    MarginBorders = 10
                };
            }
            else
            {
                LabelCategorySeparator = new ExtendedLabel()
                {
                    Text = META_SEPARATOR,
                    Style = Theme.ApplicationStyles.DetailValueExtendedLabelStyle,
                    HorizontalOptions = LayoutOptions.Start,
                    VerticalOptions = LayoutOptions.Start,
                    HorizontalTextAlignment = TextAlignment.Start,
                    VerticalTextAlignment = TextAlignment.Center,
                    TextColor = Theme.CommonResources.TextColorSection
                };

                LabelDepartmentSeparator = new ExtendedLabel()
                {
                    Text = META_SEPARATOR,
                    Style = Theme.ApplicationStyles.DetailValueExtendedLabelStyle,
                    HorizontalOptions = LayoutOptions.Start,
                    VerticalOptions = LayoutOptions.Start,
                    HorizontalTextAlignment = TextAlignment.Start,
                    VerticalTextAlignment = TextAlignment.Center,
                    TextColor = Theme.CommonResources.TextColorSection
                };

                if (RenderReadOnly)
                {
                    LabelDepartment = new ExtendedLabel()
                    {
                        Style = Theme.ApplicationStyles.DetailValueExtendedLabelStyle,
                        HorizontalOptions = LayoutOptions.Start,
                        VerticalOptions = LayoutOptions.Start,
                        HorizontalTextAlignment = TextAlignment.Start,
                        VerticalTextAlignment = TextAlignment.Center,
                        TextColor = Theme.CommonResources.TextColorSection
                    };

                    LabelCategory = new ExtendedLabel()
                    {
                        Style = Theme.ApplicationStyles.DetailValueExtendedLabelStyle,
                        HorizontalOptions = LayoutOptions.Start,
                        VerticalOptions = LayoutOptions.Start,
                        HorizontalTextAlignment = TextAlignment.Start,
                        VerticalTextAlignment = TextAlignment.Center,
                        TextColor = Theme.CommonResources.TextColorSection
                    };

                    LabelSubcategory = new ExtendedLabel()
                    {
                        Style = Theme.ApplicationStyles.DetailValueExtendedLabelStyle,
                        HorizontalOptions = LayoutOptions.Start,
                        VerticalOptions = LayoutOptions.Start,
                        HorizontalTextAlignment = TextAlignment.Start,
                        VerticalTextAlignment = TextAlignment.Center,
                        TextColor = Theme.CommonResources.TextColorSection
                    };
                }
                else
                {
                    ButtonDepartment = new TextContentViewButton()
                    {
                        Style = Theme.ApplicationStyles.TextOnlyContentButtonStyle,
                        Command = InternalNavigateToStoreLevelCommand,
                        TextColor = Theme.CommonResources.TextColorSection,
                        HorizontalOptions = LayoutOptions.Start,
                        VerticalOptions = LayoutOptions.Start
                    };

                    ButtonCategory = new TextContentViewButton()
                    {
                        Style = Theme.ApplicationStyles.TextOnlyContentButtonStyle,
                        Command = InternalNavigateToStoreLevelCommand,
                        TextColor = Theme.CommonResources.TextColorSection,
                        HorizontalOptions = LayoutOptions.Start,
                        VerticalOptions = LayoutOptions.Start
                    };

                    ButtonSubcategory = new TextContentViewButton()
                    {
                        Style = Theme.ApplicationStyles.TextOnlyContentButtonStyle,
                        Command = InternalNavigateToStoreLevelCommand,
                        TextColor = Theme.CommonResources.TextColorSection,
                        HorizontalOptions = LayoutOptions.Start,
                        VerticalOptions = LayoutOptions.Start
                    };
                }
            }
        }

        /// <summary>
        /// When binding context changes.
        /// </summary>
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            AC.ScheduleManaged(
                () =>
                {
                    UpdateButtonLevels(BindingContext as StoreItemLevel);

                    InternalNavigateToStoreLevelCommand.RaiseCanExecuteChanged();
                });
        }

        /// <summary>
        /// Setup binding.
        /// </summary>
        protected override void SetupBindings()
        {
            if (SubLevelButton != null)
            {
                SubLevelButton.Command = InternalNavigateToStoreLevelCommand;
            }
        }

        /// <summary>
        /// Update button.
        /// </summary>
        /// <param name="level">Level to use.</param>
        protected void UpdateButtonLevels(StoreItemLevel level)
        {
            if (SubLevelButton != null)
            {
                SubLevelButton.CommandParameter = level;

                SubLevelButton.Text = Theme.CommonResources.StoreItemLevelToLowerLevelStringConverter.Convert(level, typeof(string), null, System.Globalization.CultureInfo.CurrentUICulture) as string;
            }

            if (ButtonDepartment != null)
            {
                ButtonDepartment.IsVisible = (level != null) && !string.IsNullOrWhiteSpace(level.Department);

                if (ButtonDepartment.IsVisible)
                {
                    ButtonDepartment.Text = level.Department;
                    ButtonDepartment.CommandParameter = new StoreItemLevel()
                    {
                        Department = level.Department
                    };
                }
            }

            if (ButtonCategory != null)
            {
                ButtonCategory.IsVisible = (level != null) && !string.IsNullOrWhiteSpace(level.Department) && !string.IsNullOrWhiteSpace(level.Category);

                if (ButtonCategory.IsVisible)
                {
                    ButtonCategory.Text = level.Category;
                    ButtonCategory.CommandParameter = new StoreItemLevel()
                    {
                        Department = level.Department,
                        Category = level.Category
                    };
                }

                if (LabelDepartmentSeparator != null)
                {
                    LabelDepartmentSeparator.IsVisible = ButtonCategory.IsVisible;
                }
            }

            if (ButtonSubcategory != null)
            {
                ButtonSubcategory.IsVisible = (level != null) && !string.IsNullOrWhiteSpace(level.Department) && !string.IsNullOrWhiteSpace(level.Category) && !string.IsNullOrWhiteSpace(level.Subcategory);

                if (ButtonCategory.IsVisible)
                {
                    ButtonSubcategory.Text = level.Subcategory;
                    ButtonSubcategory.CommandParameter = new StoreItemLevel()
                    {
                        Department = level.Department,
                        Category = level.Category,
                        Subcategory = level.Subcategory
                    };
                }

                if (LabelCategorySeparator != null)
                {
                    LabelCategorySeparator.IsVisible = ButtonSubcategory.IsVisible;
                }
            }

            if (LabelDepartment != null)
            {
                LabelDepartment.IsVisible = (level != null) && !string.IsNullOrWhiteSpace(level.Department);

                if (LabelDepartment.IsVisible)
                {
                    LabelDepartment.Text = level.Department;
                }
            }

            if (LabelCategory != null)
            {
                LabelCategory.IsVisible = (level != null) && !string.IsNullOrWhiteSpace(level.Department) && !string.IsNullOrWhiteSpace(level.Category);

                if (LabelCategory.IsVisible)
                {
                    LabelCategory.Text = level.Category;
                }

                if (LabelDepartmentSeparator != null)
                {
                    LabelDepartmentSeparator.IsVisible = LabelCategory.IsVisible;
                }
            }

            if (LabelSubcategory != null)
            {
                LabelSubcategory.IsVisible = (level != null) && !string.IsNullOrWhiteSpace(level.Department) && !string.IsNullOrWhiteSpace(level.Category) && !string.IsNullOrWhiteSpace(level.Subcategory);

                if (LabelSubcategory.IsVisible)
                {
                    LabelSubcategory.Text = level.Subcategory;
                }

                if (LabelCategorySeparator != null)
                {
                    LabelCategorySeparator.IsVisible = LabelSubcategory.IsVisible;
                }
            }
        }
    }
}