namespace GrowSphere.Domain.Models.IssueModel
{
    public class IssueId : IComparable<IssueId>
    {
        public IssueId(Guid value) => Value = value;

        public Guid Value { get; }

        public static IssueId NewId() => new(Guid.NewGuid());

        public static IssueId Empty() => new(Guid.Empty);

        public static IssueId Create(Guid id) => new(id);

        // Возможно не будет работать, надо проверить
        public int CompareTo(IssueId? other)
        {
            if (other == null)
            {
                return 1;
            }

            if ((object)this == other)
            {
                return 0;
            }

            return Value.CompareTo(other.Value);
        }

        public static implicit operator Guid(IssueId id)
        {
            if (id == null) throw new ArgumentNullException("id cannot be empty");

            return id.Value;
        }
    }
}
