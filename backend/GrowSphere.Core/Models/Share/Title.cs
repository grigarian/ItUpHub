using CSharpFunctionalExtensions;
using GrowSphere.Core;

namespace GrowSphere.Domain.Models.Share
{
    public record Title
    {
        public const int TITLE_MAX_LENGHT = 100;
        
        public string Value { get; }

        private Title(string value)
        {
            Value = value;
        }

        public static Result<Title, Error> Create(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                return Errors.General.ValueIsRequired("title");
            if (title.Length > TITLE_MAX_LENGHT)
                return Errors.General.ValueIsInvalid("title");

            return new Title(title);
        }
    }
}
