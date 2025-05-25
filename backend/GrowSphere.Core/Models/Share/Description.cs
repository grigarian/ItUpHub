using CSharpFunctionalExtensions;
using GrowSphere.Core;

namespace GrowSphere.Domain.Models.Share
{
    public record Description
    {
        public const int DESCRIPTION_MAX_LENGHT = 2000;

        public string Value { get; }

        private Description(string value)
        {
            Value = value;
        }

        public static Result<Description, Error> Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return Errors.General.ValueIsRequired("description");
            if (value.Length > DESCRIPTION_MAX_LENGHT)
                return Errors.General.ValueIsInvalid("description");

            return new Description(value);
        }
    }
}
