using Microsoft.AspNetCore.Http;

namespace myEVote.Application.Interfaces.Services;

public interface IFileUploadService
{
    Task<string> UploadFileAsync(IFormFile file, string folder);
    void DeleteFile(string filePath);
    bool FileExists(string filePath);
}