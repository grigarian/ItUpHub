using CSharpFunctionalExtensions;
using GrowSphere.Core;

namespace GrowSphere.Domain.Models.IssueModel
{
    public class IssueStatus : ValueObject
    {
        public static readonly IssueStatus Backlog = new("Backlog");
        public static readonly IssueStatus ToDo = new("ToDo");
        public static readonly IssueStatus InProgress = new("InProgress");
        public static readonly IssueStatus Review = new("Review");
        public static readonly IssueStatus Done = new("Done");
        
        public string Value { get; }

        private IssueStatus(string value)
        {
            Value = value;
        }

        public static Result<IssueStatus, Error> FromString(string value) =>
            value switch
            {
                "Backlog" => Backlog,
                "ToDo" => ToDo,
                "InProgress" => InProgress,
                "Review" => Review,
                "Done" => Done,
                _ => Errors.General.ValueIsInvalid("issue_status")
            };
        
        protected override IEnumerable<IComparable> GetEqualityComponents()
        {
            yield return Value;
        }
        
        public override string ToString() => Value;
    }
}
