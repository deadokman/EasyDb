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

namespace EasyDb.CustomControls
{
    /// <summary>
    /// Interaction logic for UserDatasouceSettingsControl.xaml
    /// </summary>
    public partial class UserDatasouceSettingsControl : UserControl, INotifyPropertyChanged
    {
        public UserDatasouceSettingsControl( )
        {
            InitializeComponent();
        }

        public EdbSourceOption SelectedObject
        {
            get { return (EdbSourceOption)GetValue(SelectedObjectProperty); }
            set { SetValue(SelectedObjectProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedObject.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedObjectProperty =
            DependencyProperty.Register("SelectedObject", typeof(EdbSourceOption), typeof(UserDatasouceSettingsControl), new PropertyMetadata(null, SetObjectPropsDisplay));

        private IList<DatasourceOption> _sourceOptions;


        private static void SetObjectPropsDisplay(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var valueObjectType = e.NewValue.GetType();
            var depObject = (UserDatasouceSettingsControl)d;
            depObject.SourceOptions = FormatDatasourceOptions(e.NewValue as EdbSourceOption, valueObjectType, App.Current.Resources);
        }

        public IList<DatasourceOption> SourceOptions
        {
            get => _sourceOptions;
            set
            {
                _sourceOptions = value;
                OnPropertyChanged();
            }
        }

        public static IList<DatasourceOption> FormatDatasourceOptions(EdbSourceOption valueObject, Type objectType, IDictionary resourceDictionary)
        {
            var res = new List<DatasourceOption>();
            var props = objectType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Select(p => new {propInfo = p, attributes = p.GetCustomAttributes<OptionDisplayNameAttribute>().FirstOrDefault()})
                .Where(pair => pair.attributes != null);
            foreach (var prop in props)
            {
                var optionName = (resourceDictionary[prop.attributes.ResourceNameKey] as string) ?? prop.attributes.AlternativeName;
                res.Add(new DatasourceOption(valueObject, prop.propInfo)
                {
                    IsReadOnly = !prop.propInfo.CanWrite || !prop.propInfo.GetSetMethod(/*nonPublic*/ true).IsPublic,
                    OptionEditType = prop.propInfo.PropertyType.FullName,
                    OptionName = optionName
                });
            }

            return res;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
