namespace GrowSphere.Domain.Models.NotificationModel;

public class NotificationId : IComparable<NotificationId>
{
    public NotificationId(Guid value) => Value = value;

    public Guid Value { get; }

    public static NotificationId NewId() => new(Guid.NewGuid());

    public static NotificationId Empty() => new(Guid.Empty);

    public static NotificationId Create(Guid id) => new(id);

    public int CompareTo(NotificationId? other)
    {
        if (other == null)
            return 1;

        return Value.CompareTo(other.Value);
    }

    public static implicit operator Guid(NotificationId id)
    {
        if (id == null) throw new ArgumentNullException(nameof(id), "NotificationId cannot be null");

        return id.Value;
    }
}