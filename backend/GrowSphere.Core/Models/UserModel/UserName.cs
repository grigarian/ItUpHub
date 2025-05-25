using CSharpFunctionalExtensions;
using GrowSphere.Core;

namespace GrowSphere.Domain.Models.UserModel
{
    public record UserName
    {
        public string Value { get; }

        private UserName(string value)
        {
            Value = value;
        }

        public static Result<UserName, Error> Create(string value)
        {
            if (value.Length > User.USERNAME_MAX_LENGTH)
                return Errors.General.ValueIsInvalid("user_name");
            if (string.IsNullOrWhiteSpace(value))
                return Errors.General.ValueIsInvalid("user_name");

            return new UserName(value);
        }
    }
}
