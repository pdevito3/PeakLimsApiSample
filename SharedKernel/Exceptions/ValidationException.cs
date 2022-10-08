namespace SharedKernel.Exceptions
{
    using FluentValidation.Results;
    using System.Collections.Generic;
    using System.Linq;
    using System;

    public class ValidationException : Exception
    {
        public ValidationException()
            : base("One or more validation failures have occurred.")
        {
            Errors = new Dictionary<string, string[]>();
        }

        public ValidationException(IEnumerable<ValidationFailure> failures)
            : this()
        {
            Errors = failures
                .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
        }

        public ValidationException(ValidationFailure failure)
            : this()
        {
            Errors = new Dictionary<string, string[]>
            {
                [failure.PropertyName] = new[] { failure.ErrorMessage }
            };
        }

        public ValidationException(string errorPropertyName, string errorMessage)
            : this()
        {
            Errors = new Dictionary<string, string[]>
            {
                [errorPropertyName] = new[] { errorMessage }
            };
        }

        public IDictionary<string, string[]> Errors { get; }
    }
}