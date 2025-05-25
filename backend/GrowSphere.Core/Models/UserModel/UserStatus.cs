using CSharpFunctionalExtensions;
using GrowSphere.Core;

namespace GrowSphere.Domain.Models.UserModel
{
    public record UserStatus
    {
        public static readonly UserStatus Online = new(nameof(Online));
        public static readonly UserStatus Offline = new(nameof(Offline));

        private static readonly UserStatus[] _all = { Online, Offline };

        public string Value { get; }

        private UserStatus(string value)
        {
            Value = value;
        }

        public static Result<UserStatus, Error> Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return Errors.General.ValueIsRequired("user_status");

            if (_all.Any(l => l.Value.ToLower() == value.Trim().ToLower()) == false)
                return Errors.General.ValueIsInvalid("user_status");

            return new UserStatus(value);
        }

    }

   
}
