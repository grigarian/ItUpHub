namespace GrowSphere.Domain.Models.NotificationModel;

public class NotificationId : IComparable<NotificationId>
{
    public NotificationId(Guid value) => Value = value;

    public Guid Value { get; }

    public static NotificationId NewId() => new(Guid.NewGuid());

    public static NotificationId Empty() => new(Guid.Empty);

    public static NotificationId Create(Guid id) => new(id);

    // Возможно не будет работать, надо проверить
    public int CompareTo(NotificationId? other)
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

    public static implicit operator Guid(NotificationId id)
    {
        if (id == null) throw new ArgumentNullException("id cannot be empty");

        return id.Value;
    }
}