using CSharpFunctionalExtensions;
using GrowSphere.Core;

namespace GrowSphere.Domain.Models.UserModel
{
    public record Level
    {
        public static readonly Level Low = new(nameof(Low));
        public static readonly Level Medium = new(nameof(Medium));
        public static readonly Level Hight = new(nameof(Hight));

        private static readonly Level[] _all = { Low, Medium, Hight };

        public string Value { get; }

        private Level(string value)
        {
            Value = value;
        }

        public static Result<Level, Error> Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return Errors.General.ValueIsRequired("level");

            if (_all.Any(l => l.Value.ToLower() == value.Trim().ToLower()) == false)
                return Errors.General.ValueIsInvalid("level");

            return new Level(value);
        }
    }
}
