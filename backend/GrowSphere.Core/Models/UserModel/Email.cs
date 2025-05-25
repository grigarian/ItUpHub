using CSharpFunctionalExtensions;
using GrowSphere.Core;
using System.Text.RegularExpressions;

namespace GrowSphere.Domain.Models.UserModel
{
    public record Email
    {
        public string Value { get; }

        private Email(string value)
        {
            Value = value;
        }

        public static Result<Email, Error> Create(string value)
        {
            if (!Regex.IsMatch(value, @"^[^\s@]+@[^\s@]+\.[^\s@]+$"))
                return Errors.General.ValueIsInvalid("email");
            if (string.IsNullOrWhiteSpace(value))
                return Errors.General.ValueIsInvalid("email");

            return new Email(value);
        }
    }
}
