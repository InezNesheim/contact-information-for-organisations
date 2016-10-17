using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using GalaSoft.MvvmLight;

namespace AltinnDesktopTool.Model
{
    public class ModelBase : ObservableObject, INotifyDataErrorInfo
    {
        public readonly Dictionary<string, ICollection<string>> _validationErrors = new Dictionary<string, ICollection<string>>();

        #region INotifyDataErrorInfo members
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        private void RaiseErrorsChanged(string propertyName)
        {
            if (ErrorsChanged != null)
                ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public bool HasErrors
        {
            get { return _validationErrors.Count > 0; }
        }
        #endregion
        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName) || !_validationErrors.ContainsKey(propertyName))
                return null;

            return _validationErrors[propertyName];
        }


        protected void ValidateModelProperty(object value, string propertyName)
        {
            if (_validationErrors.ContainsKey(propertyName))
                _validationErrors.Remove(propertyName);

            ICollection<ValidationResult> validationResults = new List<ValidationResult>();
            ValidationContext validationContext =
                new ValidationContext(this, null, null) { MemberName = propertyName };
            if (!Validator.TryValidateProperty(value, validationContext, validationResults))
            {
                _validationErrors.Add(propertyName, new List<string>());
                foreach (ValidationResult validationResult in validationResults)
                {
                    _validationErrors[propertyName].Add(validationResult.ErrorMessage);
                }
            }
            RaiseErrorsChanged(propertyName);
        }

        protected void ValidateModel()
        {
            _validationErrors.Clear();
            ICollection<ValidationResult> validationResults = new List<ValidationResult>();
            ValidationContext validationContext = new ValidationContext(this, null, null);
            if (!Validator.TryValidateObject(this, validationContext, validationResults, true))
            {
                foreach (ValidationResult validationResult in validationResults)
                {
                    string property = validationResult.MemberNames.ElementAt(0);
                    if (_validationErrors.ContainsKey(property))
                    {
                        _validationErrors[property].Add(validationResult.ErrorMessage);
                    }
                    else
                    {
                        _validationErrors.Add(property, new List<string> { validationResult.ErrorMessage });
                    }
                }
            }

            /* Raise the ErrorsChanged for all properties explicitly */
            RaiseErrorsChanged("Username");
            RaiseErrorsChanged("Name");
        }
    }
}