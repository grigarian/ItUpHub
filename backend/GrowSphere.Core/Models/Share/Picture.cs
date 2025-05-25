using CSharpFunctionalExtensions;
using GrowSphere.Core;
using Microsoft.AspNetCore.Http;

namespace GrowSphere.Domain.Models.Share
{
    public record Picture
    {
        public string? Path { get; } = null;
        public string? MimeType { get; } = null;

        public Picture(){}

        private Picture(string path, string mimeType)
        {
            Path = path;
            MimeType = mimeType;
        }

        public static Picture Empty()
        {
            return new Picture();
        } 

        public static Result<Picture, Error> Create(string path, string mimeType)
        {
            if (string.IsNullOrWhiteSpace(path))
                return Errors.FileError.EmptyFile();

            return new Picture(path, mimeType);
        }

    }
}
