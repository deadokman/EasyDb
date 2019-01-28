namespace EasyDb.CustomControls
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Controls;

    using EasyDb.Annotations;
    using EasyDb.CustomControls.DatasourceSettings;

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
        public EdbSourceOption SelectedObject
        {
            get => (EdbSourceOption)this.GetValue(SelectedObjectProperty);

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
        /// <param name="objectType">The objectType<see cref="Type"/></param>
        /// <param name="resourceDictionary">The resourceDictionary<see cref="IDictionary"/></param>
        /// <returns>The <see cref="IList{DatasourceOption}"/></returns>
        public static IList<DatasourceOption> FormatDatasourceOptions(
            EdbSourceOption valueObject,
            Type objectType,
            IDictionary resourceDictionary)
        {
            if (valueObject == null)
            {
                throw new Exception("Options object sholud be implemented from EdbSourceOption");
            }

            valueObject.SetThrowExceptionOnInvalidate(true);
            var res = new List<DatasourceOption>();
            var props = objectType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Select(
                    p => new
                             {
                                 propInfo = p,
                                 attributes = p.GetCustomAttributes<OptionDisplayNameAttribute>().FirstOrDefault()
                             }).Where(pair => pair.attributes != null);
            foreach (var prop in props)
            {
                var optionName = resourceDictionary[prop.attributes.ResourceNameKey] as string
                                 ?? prop.attributes.AlternativeName;

                // Check password attribute
                var isPasswordOption = prop.propInfo.GetCustomAttributes<PasswordFieldAttribute>().Any();
                res.Add(
                    new DatasourceOption(valueObject, prop.propInfo)
                        {
                            IsReadOnly =
                                !prop.propInfo.CanWrite || !prop.propInfo.GetSetMethod(true).IsPublic,
                            OptionEditType = isPasswordOption ? "PasswordTextBox" : prop.propInfo.PropertyType.FullName,
                            OptionName = optionName
                        });
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
            var valueObjectType = e.NewValue.GetType();
            var depObject = (UserDatasouceSettingsControl)d;
            depObject.SourceOptions = FormatDatasourceOptions(
                e.NewValue as EdbSourceOption,
                valueObjectType,
                Application.Current.Resources);
        }
    }
}