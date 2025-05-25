using CSharpFunctionalExtensions;
using GrowSphere.Core;

namespace GrowSphere.Domain.Models.ProjectModel
{
    public class ProjectStatus
    {
        public static readonly ProjectStatus InProgress = new(nameof(InProgress));
        public static readonly ProjectStatus Complete = new(nameof(Complete));
        public static readonly ProjectStatus Cancelled = new(nameof(Cancelled));

        private static readonly ProjectStatus[] _all = { InProgress, Complete, Cancelled };

        public string Value { get; }

        private ProjectStatus(string value)
        {
            Value = value;
        }

        public static Result<ProjectStatus, Error> Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return Errors.General.ValueIsRequired("project_status");

            if (_all.Any(l => l.Value.ToLower() == value.Trim().ToLower()) == false)
                return Errors.General.ValueIsInvalid("project_status");

            return new ProjectStatus(value);
        }
    }
}
