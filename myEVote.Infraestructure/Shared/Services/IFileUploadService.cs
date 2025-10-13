using Microsoft.AspNetCore.Http;
namespace myEVote.Infraestructure.Shared.Services;

public interface IFileUploadService
{
    Task<string> UploadFileAsync(IFormFile file, string folder);
    void DeleteFile(string filePath);
    bool FileExists(string filePath);
}