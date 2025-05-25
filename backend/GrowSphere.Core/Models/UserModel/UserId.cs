namespace GrowSphere.Domain.Models.UserModel
{
    public record UserId : IComparable<UserId>
    {
        public UserId(Guid value) => Value = value;

        public Guid Value { get; }

        public static UserId NewId() => new(Guid.NewGuid());

        public static UserId Empty() => new(Guid.Empty);

        public static UserId Create(Guid id) => new(id);

        // Возможно не будет работать, надо проверить
        public int CompareTo(UserId? other)
        {
            if ((object)other == null)
            {
                return 1;
            }

            if ((object)this == other)
            {
                return 0;
            }

            return Value.CompareTo(other.Value);
        }

        public static implicit operator Guid(UserId userId)
        {
            if (userId == null) throw new ArgumentNullException("id cannot be empty");

            return userId.Value;
        }

    }
}
