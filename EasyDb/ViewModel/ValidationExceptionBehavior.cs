namespace EasyDb.ViewModel
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Interactivity;

    using EDb.Interfaces.Validation;

    /// <summary>
    /// A simple behavior that can transfer the number of validation error with exceptions
    /// to a ViewModel which supports the INotifyValidationException interface
    /// </summary>
    public class ValidationExceptionBehavior : Behavior<FrameworkElement>
    {
        /// <summary>
        /// Defines the validationExceptionCount
        /// </summary>
        private int validationExceptionCount;

        /// <summary>
        /// The CreateInstanceCore
        /// </summary>
        /// <returns>The <see cref="Freezable"/></returns>
        protected override Freezable CreateInstanceCore()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The OnAttached
        /// </summary>
        protected override void OnAttached()
        {
            AssociatedObject.AddHandler(
                Validation.ErrorEvent,
                new EventHandler<ValidationErrorEventArgs>(OnValidationError));
        }

        /// <summary>
        /// The OnValidationError
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="ValidationErrorEventArgs"/></param>
        private void OnValidationError(object sender, ValidationErrorEventArgs e)
        {
            // we want to count only the validation error with an exception
            // other error are handled by using the attribute on the properties
            if (e.Error.Exception == null)
            {
                return;
            }

            if (e.Action == ValidationErrorEventAction.Added)
            {
                validationExceptionCount++;
            }
            else
            {
                validationExceptionCount--;
            }

            if (AssociatedObject.DataContext is IValidationExceptionHandler)
            {
                // transfer the information back to the viewmodel
                var viewModel = (IValidationExceptionHandler)AssociatedObject.DataContext;
                viewModel.ValidationExceptionsChanged(validationExceptionCount);
            }
        }
    }
}