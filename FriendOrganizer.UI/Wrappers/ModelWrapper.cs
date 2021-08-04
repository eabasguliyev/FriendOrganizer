using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace FriendOrganizer.UI.Wrappers
{
    public class ModelWrapper<T>: NotifyDataErrorInfoBase where T : class
    {
        public T Model { get; }

        public ModelWrapper(T model)
        {
            Model = model;
        }

        public TValue GetValue<TValue>([CallerMemberName] string propertyName = null)
        {
            return (TValue)typeof(T).GetProperty(propertyName).GetValue(Model);
        }

        public void SetValue<TValue>(TValue value, [CallerMemberName] string propertyName = null)
        {
            typeof(T).GetProperty(propertyName).SetValue(Model, value);
            OnPropertyChanged();
            ValidatePropertyInternal(propertyName, value);
        }


        private void ValidatePropertyInternal(string propertyName, object currentValue)
        {
            ClearErrors(propertyName);

            ValidateDataAnnotations(propertyName, currentValue);

            ValidateCustomErrors(propertyName);
        }

        private void ValidateDataAnnotations(string propertyName, object currentValue)
        {
            var validationContext = new ValidationContext(Model)
            {
                MemberName = propertyName
            };

            var results = new List<ValidationResult>();

            Validator.TryValidateProperty(currentValue, validationContext, results);

            foreach (var result in results)
            {
                AddError(propertyName, result.ErrorMessage);
            }
        }

        private void ValidateCustomErrors(string propertyName)
        {
            var errors = ValidateProperty(propertyName);

            foreach (var error in errors)
            {
                AddError(propertyName, error);
            }
        }

        protected virtual IEnumerable<string> ValidateProperty(string propertyName)
        {
            return null;
        }
    }
}