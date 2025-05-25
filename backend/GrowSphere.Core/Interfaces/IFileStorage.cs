namespace GrowSphere.Domain.Interfaces;

public interface IFileStorage
{
    Task<FileUploadResult> UploadFileAsync(
        Stream fileStream, 
        string filePath, 
        string contentType);
    
    Task DeleteFileAsync(string filePath);
    Task<Stream> GetFileAsync(string filePath);
}

public record FileUploadResult(string Path);