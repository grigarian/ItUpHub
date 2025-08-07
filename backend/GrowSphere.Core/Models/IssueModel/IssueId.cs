namespace GrowSphere.Domain.Models.IssueModel
{
    public class IssueId : IComparable<IssueId>
    {
        public IssueId(Guid value) => Value = value;

        public Guid Value { get; }

        public static IssueId NewId() => new(Guid.NewGuid());

        public static IssueId Empty() => new(Guid.Empty);

        public static IssueId Create(Guid id) => new(id);

        public int CompareTo(IssueId? other)
        {
            if (other == null)
                return 1;

            return Value.CompareTo(other.Value);
        }

        public static implicit operator Guid(IssueId id)
        {
            if (id == null) throw new ArgumentNullException(nameof(id), "IssueId cannot be null");

            return id.Value;
        }
    }
}
