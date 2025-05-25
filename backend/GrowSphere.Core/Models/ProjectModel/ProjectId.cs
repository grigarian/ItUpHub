namespace GrowSphere.Domain.Models.ProjectModel
{
    public record ProjectId : IComparable<ProjectId>
    {
        public ProjectId(Guid value) => Value = value;

        public Guid Value { get; }

        public static ProjectId NewId() => new(Guid.NewGuid());

        public static ProjectId Empty() => new(Guid.Empty);

        public static ProjectId Create(Guid id) => new(id);

        // Возможно не будет работать, надо проверить
        public int CompareTo(ProjectId? other)
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

        public static implicit operator Guid(ProjectId id)
        {
            if (id == null) throw new ArgumentNullException("id cannot be empty");

            return id.Value;
        }
    }
}
