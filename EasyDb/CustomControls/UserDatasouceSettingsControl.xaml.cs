using EasyDb.ViewModel.DataSource.Items;

namespace EasyDb.CustomControls
{
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Controls;

    using EasyDb.Annotations;
    using EasyDb.CustomControls.DatasourceSettings;
    using EasyDb.Model;

    using EDb.Interfaces;

    /// <summary>
    /// Interaction logic for UserDatasouceSettingsControl.xaml
    /// </summary>
    public partial class UserDatasouceSettingsControl : UserControl, INotifyPropertyChanged
    {
        /// <summary>
        /// Defines the SelectedObjectProperty
        /// </summary>
        public static readonly DependencyProperty SelectedObjectProperty = DependencyProperty.Register(
            "SelectedObject",
            typeof(EdbSourceOption),
            typeof(UserDatasouceSettingsControl),
            new PropertyMetadata(null, SetObjectPropsDisplay));

        /// <summary>
        /// Defines the _sourceOptions
        /// </summary>
        private IList<DatasourceOption> _sourceOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserDatasouceSettingsControl"/> class.
        /// </summary>
        public UserDatasouceSettingsControl()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Defines the PropertyChanged
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the SelectedObject
        /// </summary>
        public EdbSourceOptionProxy SelectedObject
        {
            get => (EdbSourceOptionProxy)this.GetValue(SelectedObjectProperty);

            set => this.SetValue(SelectedObjectProperty, value);
        }

        /// <summary>
        /// Gets or sets the SourceOptions
        /// </summary>
        public IList<DatasourceOption> SourceOptions
        {
            get => this._sourceOptions;
            set
            {
                this._sourceOptions = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// The FormatDatasourceOptions
        /// </summary>
        /// <param name="valueObject">The valueObject<see cref="EdbSourceOption"/></param>
        /// <param name="resourceDictionary">The resourceDictionary<see cref="IDictionary"/></param>
        /// <returns>The <see cref="IList{DatasourceOption}"/></returns>
        public static IList<DatasourceOption> FormatDatasourceOptions(
            EdbSourceOption valueObject,
            IDictionary resourceDictionary)
        {
            var optionDefinitions = valueObject.ToOptionDefinition();
            var res = new List<DatasourceOption>();
            foreach (var prop in optionDefinitions.Properties)
            {
                var optionName = resourceDictionary[prop.ResourcePropertyKey] as string
                                 ?? prop.DefaultPropertyName;
                res.Add(new DatasourceOption(prop) { OptionName = optionName });
            }

            return res;
        }

        /// <summary>
        /// The OnPropertyChanged
        /// </summary>
        /// <param name="propertyName">The propertyName<see cref="string"/></param>
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// The SetObjectPropsDisplay
        /// </summary>
        /// <param name="d">The d<see cref="DependencyObject"/></param>
        /// <param name="e">The e<see cref="DependencyPropertyChangedEventArgs"/></param>
        private static void SetObjectPropsDisplay(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var depObject = (UserDatasouceSettingsControl)d;
            if (e.NewValue != null)
            {
                depObject.SourceOptions = FormatDatasourceOptions(
                    e.NewValue as EdbSourceOption,
                    Application.Current.Resources);
            }
        }
    }
}