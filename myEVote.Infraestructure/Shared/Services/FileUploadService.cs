using myEVote.Application.Interfaces.Services;

namespace myEVote.Infraestructure.Shared.Services;

public class FileUploadService(IWebHostEnvironment webHostEnvironment) : IFileUploadService
{
    private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;

    public async Task<string> UploadFileAsync(IFormFile file, string folder)
    {
        try
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("El archivo está vacío o es nulo");

            // Crear carpeta si no existe
            var uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", folder);
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            // Generar nombre único para el archivo
            var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
            var filePath = Path.Combine(uploadPath, fileName);

            // Guardar archivo
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Retornar ruta relativa
            return $"/uploads/{folder}/{fileName}";
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error subiendo archivo: {ex.Message}");
            throw new Exception($"Error al subir el archivo: {ex.Message}", ex);
        }
    }

    public void DeleteFile(string filePath)
    {
        try
        {
            if (string.IsNullOrEmpty(filePath))
                return;

            // Convertir ruta relativa a absoluta
            var fullPath = Path.Combine(_webHostEnvironment.WebRootPath, filePath.TrimStart('/'));

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error eliminando archivo: {ex.Message}");
            // No lanzar excepción, solo registrar el error
        }
    }

    public bool FileExists(string filePath)
    {
        try
        {
            if (string.IsNullOrEmpty(filePath))
                return false;

            var fullPath = Path.Combine(_webHostEnvironment.WebRootPath, filePath.TrimStart('/'));
            return File.Exists(fullPath);
        }
        catch
        {
            return false;
        }
    }
}