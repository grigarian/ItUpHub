using CSharpFunctionalExtensions;
using GrowSphere.Core;

namespace GrowSphere.Domain.Models.IssueModel
{
    public record IssueStatus
    {
        public static readonly IssueStatus InWork = new(nameof(InWork));
        public static readonly IssueStatus Completed = new(nameof(Completed));
        public static readonly IssueStatus Cancelled = new(nameof(Cancelled));
        public static readonly IssueStatus Expired = new(nameof(Expired));
        public static readonly IssueStatus Wait = new(nameof(Wait));

        private static readonly IssueStatus[] _all = { InWork, Completed, Cancelled, Expired, Wait };

        public string Value { get; }

        private IssueStatus(string value)
        {
            Value = value;
        }

        public static Result<IssueStatus, Error> Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return Errors.General.ValueIsRequired("issue_status");

            if (_all.Any(l => l.Value.ToLower() == value.Trim().ToLower()) == false)
                return Errors.General.ValueIsInvalid("issue_status");

            return new IssueStatus(value);
        }
    }
}
