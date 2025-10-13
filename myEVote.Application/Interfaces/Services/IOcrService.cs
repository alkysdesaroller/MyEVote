namespace myEVote.Application.Interfaces.Services;

public interface IOcrService
{
    Task<string> ExtractTextFromImageAsync(string imagePath);
    string ExtractCedulaNumber(string ocrText);
}