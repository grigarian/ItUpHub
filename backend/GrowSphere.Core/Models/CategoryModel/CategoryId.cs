namespace GrowSphere.Domain.Models.CategoryModel
{
    public class CategoryId : IComparable<CategoryId>
    {
        public CategoryId(Guid value) => Value = value;

        public Guid Value { get; }

        public static CategoryId NewId() => new(Guid.NewGuid());

        public static CategoryId Empty() => new(Guid.Empty);

        public static CategoryId Create(Guid id) => new(id);

        public int CompareTo(CategoryId? other)
        {
            if (other == null)
                return 1;

            return Value.CompareTo(other.Value);
        }

        public static implicit operator Guid(CategoryId id)
        {
            if (id == null) throw new ArgumentNullException(nameof(id), "CategoryId cannot be null");

            return id.Value;
        }
    }
}
