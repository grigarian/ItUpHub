namespace GrowSphere.Domain.Models.UserModel
{
    public record UserId : IComparable<UserId>
    {
        public UserId(Guid value) => Value = value;

        public Guid Value { get; }

        public static UserId NewId() => new(Guid.NewGuid());

        public static UserId Empty() => new(Guid.Empty);

        public static UserId Create(Guid id) => new(id);

        public int CompareTo(UserId? other)
        {
            if (other == null)
                return 1;

            return Value.CompareTo(other.Value);
        }

        public static implicit operator Guid(UserId userId)
        {
            if (userId == null) throw new ArgumentNullException(nameof(userId), "UserId cannot be null");

            return userId.Value;
        }

    }
}
