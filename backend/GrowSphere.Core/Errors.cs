using GrowSphere.Core;

namespace GrowSphere.Domain
{
    public static class Errors
    {
        public static class General
        {
            public static Error ValueIsInvalid(string? name = null)
            {
                var label = name ?? "value";
                return Error.Validatioin("value.is.invalid", $"{label} is invalid");
            }

            public static Error NotFound(Guid? id = null)
            {
                var forId = id == null ? "" : $" for id '{id}'";
                return Error.NotFound("record.not.found", $"record not found{forId}");
            }

            public static Error ValueIsRequired(string? name = null)
            {
                var label = name == null ? " " : " " + name + " ";
                return Error.Validatioin("lenght.is.invalid", $"invalid{label}lenght");
            }

            public static Error Unauthorized()
            {
                return Error.Unauthorized("unauthorized", "unauthorized");
            }
        }

        public static class UserError
        {
            public static Error FailedLogin(string? email = null)
            {
                var forEmail = email == null ? "" : $" for id '{email}'";
                return Error.NotFound("failed.to.login", $"failed.to.login{forEmail}");
            }

            public static Error InvalidPassword()
            {
                return Error.Validatioin("invalid.password", $"invalid.password");
            }

            public static Error InvalidEmail(string? email = null)
            {
                var forEmail = email ?? ".can.not.be.empty";
                return Error.Validatioin("invalid.email", $"invalid.email{forEmail}");
            }
        }

        public static class FileError
        {
            public static Error EmptyFile()
            {
                return Error.Validatioin("file.empty", "File is empty or not selected");
            }

            public static Error InvalidType(string? allowedTypes = null)
            {
                var typesMessage = allowedTypes == null 
                    ? "Allowed types: images (JPEG, PNG, etc.)" 
                    : $"Allowed types: {allowedTypes}";
                return Error.Validatioin("file.invalid.type", $"Invalid file type. {typesMessage}");
            }

            public static Error TooLarge(long? maxSizeBytes = null)
            {
                var sizeMessage = maxSizeBytes == null
                    ? "File is too large"
                    : $"Max file size: {maxSizeBytes / (1024 * 1024)}MB";
                return Error.Validatioin("file.too.large", sizeMessage);
            }

            public static Error UploadFailed(string? reason = null)
            {
                var reasonMessage = reason == null ? "" : $" Reason: {reason}";
                return Error.Failure("file.upload.failed", $"File upload failed.{reasonMessage}");
            }

            public static Error NotFound(string? path = null)
            {
                var pathMessage = path == null ? "" : $" Path: {path}";
                return Error.NotFound("file.not.found", $"File not found.{pathMessage}");
            }

            public static Error CorruptedData()
            {
                return Error.Validatioin("file.corrupted", "File data is corrupted");
            }

            public static Error UnsupportedOperation()
            {
                return Error.Failure("file.unsupported.operation", "Unsupported file operation");
            }
        }
    }
}
