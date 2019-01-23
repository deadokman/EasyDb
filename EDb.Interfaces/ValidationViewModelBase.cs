using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Xml.Serialization;
using EDb.Interfaces.Annotations;
using EDb.Interfaces.Validation;

namespace EDb.Interfaces
{
    public class ValidationViewModelBase : IDataErrorInfo, IValidationExceptionHandler, INotifyPropertyChanged
    {
        private readonly Dictionary<string, Tuple<PropertyInfo, ValidationAttribute[]>> _validators;

        private int _validationExceptionCount;
        private bool _isValid;
        private bool _throwOninvalidate;

        public void SetThrowExceptionOnInvalidate(bool throwErrors)
        {
            _throwOninvalidate = throwErrors;
        }

        public string this[string columnName]
        {
            get
            {
                Tuple<PropertyInfo, ValidationAttribute[]> validators;
                if (_validators.TryGetValue(columnName, out validators))
                {
                    var errors = validators.Item2
                        .Select(v => new { isValid = v.IsValid(validators.Item1.GetValue(this)), errm = v.ErrorMessage, errmrk = v.ErrorMessageResourceName })
                        .Where(p => !p.isValid).Select(p => p.errm ?? Application.Current.FindResource(p.errmrk) as string ?? "<Invalidate>");
                    return string.Join(Environment.NewLine, errors);
                }

                return string.Empty;
            }
        }

        [XmlIgnore]
        public string Error
        {
            get
            {
                var errors = _validators.SelectMany(tuple => tuple.Value.Item2
                    .Select(v => new { isValid = v.IsValid(tuple.Value.Item1.GetValue(this)), errm = v.ErrorMessage, errmrk =  v.ErrorMessageResourceName })
                    .Where(p => !p.isValid).Select(p => p.errm ?? (p.errmrk == null ? "<Invalidate>" : Application.Current.FindResource(p.errmrk) as string)));
                return string.Join(Environment.NewLine, errors);
            }
        }

        /// <summary>
        /// Gets the number of properties which have a validation attribute and are currently valid
        /// </summary>
        [XmlIgnore]
        public int ValidPropertiesCount
        {
            get
            {
                return _validators.SelectMany(tuple => tuple.Value.Item2
                    .Select(v => v.IsValid(tuple.Value.Item1.GetValue(this)))).Count(v => v) - _validationExceptionCount;
            }
        }

        public void ValidationExceptionsChanged(int count)
        {
            _validationExceptionCount = count;
            OnPropertyChanged("ValidPropertiesCount");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        [DebuggerHidden]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        { 
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            PropertyChangedCompleted(propertyName);
        }

        public ValidationViewModelBase()
        {
            // Initialize propInfo for derived type
            _validators = this.GetType().GetProperties()
                .Select(p => new { propInfo = p, Attrs = p.GetCustomAttributes<ValidationAttribute>() })
                .Where(pair => pair.Attrs.Any())
                .ToDictionary(p => p.propInfo.Name, p => new Tuple<PropertyInfo, ValidationAttribute[]>(p.propInfo, p.Attrs.ToArray()));
        }

        [XmlIgnore]
        public bool IsValid
        {
            get
            {
                return _isValid;
            }
            protected set
            {
                _isValid = value;
                this.OnPropertyChanged("IsValid");
            }
        }

        /// <summary>
        /// Gets the number of properties which have a validation attribute
        /// </summary>
        public int TotalPropertiesWithValidationCount => _validators.Count();

        [DebuggerHidden]
        protected void PropertyChangedCompleted(string propertyName)
        {
            // test prevent infinite loop while settings IsValid 
            // (which causes an PropertyChanged to be raised)
            if (propertyName != "IsValid")
            {
                // update the isValid status
                if (string.IsNullOrEmpty(this.Error) && this.ValidPropertiesCount == this.TotalPropertiesWithValidationCount)
                {
                    this.IsValid = true;
                }
                else
                {
                    this.IsValid = false;
                    var error = this[propertyName];
                    if (_throwOninvalidate && !String.IsNullOrEmpty(error))
                    {
                        throw new Exception(error);
                    }
                }
            }
        }
    }
}
