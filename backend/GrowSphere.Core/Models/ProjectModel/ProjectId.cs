namespace GrowSphere.Domain.Models.ProjectModel
{
    public record ProjectId : IComparable<ProjectId>
    {
        public ProjectId(Guid value) => Value = value;

        public Guid Value { get; }

        public static ProjectId NewId() => new(Guid.NewGuid());

        public static ProjectId Empty() => new(Guid.Empty);

        public static ProjectId Create(Guid id) => new(id);

        public int CompareTo(ProjectId? other)
        {
            if (other == null)
                return 1;

            return Value.CompareTo(other.Value);
        }

        public static implicit operator Guid(ProjectId id)
        {
            if (id == null) throw new ArgumentNullException(nameof(id), "ProjectId cannot be null");

            return id.Value;
        }
    }
}
