namespace GrowSphere.Domain.Models.ProjectMessage;

public class ProjectMessageId: IComparable<ProjectMessageId>
{
    public ProjectMessageId(Guid value) => Value = value;

    public Guid Value { get; }

    public static ProjectMessageId NewId() => new(Guid.NewGuid());

    public static ProjectMessageId Empty() => new(Guid.Empty);

    public static ProjectMessageId Create(Guid id) => new(id);

    public int CompareTo(ProjectMessageId? other)
    {
        if (other == null)
            return 1;

        return Value.CompareTo(other.Value);
    }

    public static implicit operator Guid(ProjectMessageId id)
    {
        if (id == null) throw new ArgumentNullException(nameof(id), "ProjectMessageId cannot be null");

        return id.Value;
    }
}