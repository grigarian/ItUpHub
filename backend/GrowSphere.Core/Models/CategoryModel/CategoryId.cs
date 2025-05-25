namespace GrowSphere.Domain.Models.CategoryModel
{
    public class CategoryId : IComparable<CategoryId>
    {
        public CategoryId(Guid value) => Value = value;

        public Guid Value { get; }

        public static CategoryId NewId() => new(Guid.NewGuid());

        public static CategoryId Empty() => new(Guid.Empty);

        public static CategoryId Create(Guid id) => new(id);

        // Возможно не будет работать, надо проверить
        public int CompareTo(CategoryId? other)
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

        public static implicit operator Guid(CategoryId id)
        {
            if (id == null) throw new ArgumentNullException("id cannot be empty");

            return id.Value;
        }
    }
}
