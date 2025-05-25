using CSharpFunctionalExtensions;
using GrowSphere.Core;
using GrowSphere.Domain.Models.ProjectModel;
using GrowSphere.Domain.Models.Share;

namespace GrowSphere.Domain.Models.CategoryModel
{
    public class Category : Entity<CategoryId>
    {
        private Category(CategoryId id) : base(id) { }
        
        public Title Title { get; private set; }
        
        public List<Project> Projects { get; private set; } = new List<Project>();

        private Category(CategoryId id, Title title) : base(id) 
        {
            
            Title = title;
        }

        public static Result<Category,Error> Create(CategoryId id, Title title)
        {
            return new Category(id, title);
        }
    }
}
