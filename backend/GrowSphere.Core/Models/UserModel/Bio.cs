using CSharpFunctionalExtensions;
using GrowSphere.Core;

namespace GrowSphere.Domain.Models.UserModel
{
    public record Bio
    {
        public const int BIO_MAX_LENGTH = 2000;
        
        private Bio() {}
        
        public string? Value { get; private set; }

        private Bio(string value) => Value = value;

        public static Result<Bio, Error> Create(string value)
        {
            if(value.Length > BIO_MAX_LENGTH)
                return Errors.General.ValueIsInvalid("Bio");
            
            return new Bio(value);
        }
        
        
    }
}
