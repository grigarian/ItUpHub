namespace GrowSphere.Domain.Models.SkillModel
{
    public record SkillId : IComparable<SkillId>
    {
        public SkillId(Guid value) => Value = value;

        public Guid Value { get; }

        public static SkillId NewId() => new(Guid.NewGuid());

        public static SkillId Empty() => new(Guid.Empty);

        public static SkillId Create(Guid id) => new(id);

        // Возможно не будет работать, надо проверить
        public int CompareTo(SkillId? other)
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

        public static implicit operator Guid(SkillId id)
        {
            if (id == null) throw new ArgumentNullException("id cannot be empty");

            return id.Value;
        }
    }
}
