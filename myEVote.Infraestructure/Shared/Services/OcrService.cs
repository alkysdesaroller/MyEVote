using System.Text.RegularExpressions;
using Tesseract;


using myEVote.Application.Interfaces.Services;

namespace myEVote.Infraestructure.Shared.Services;

public class OcrService(IWebHostEnvironment webHostEnvironment) : IOcrService
{
    private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;

    public async Task<string> ExtractTextFromImageAsync(string imagePath)
    {
        try
        {
            // Ruta al tessdata (archivos de lenguaje de Tesseract)
            var tessDataPath = Path.Combine(_webHostEnvironment.WebRootPath, "tessdata");

            if (!Directory.Exists(tessDataPath))
            {
                throw new DirectoryNotFoundException($"No se encontró la carpeta tessdata en: {tessDataPath}");
            }

            return await Task.Run(() =>
            {
                using var engine = new TesseractEngine(tessDataPath, "spa", EngineMode.Default);
                using var img = Pix.LoadFromFile(imagePath);
                using var page = engine.Process(img);

                var text = page.GetText();
                return text;
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error en OCR: {ex.Message}");
            throw new Exception($"Error al procesar la imagen con OCR: {ex.Message}", ex);
        }
    }

    public string ExtractCedulaNumber(string ocrText)
    {
        try
        {
            // Patrones comunes para números de cédula en República Dominicana
            // Formato: XXX-XXXXXXX-X 11 dígitos con guiones
            var patterns = new[]
            {
                @"\d{3}-\d{7}-\d{1}", // 001-1234567-8
                @"\d{11}", // 00112345678 
                @"\d{3}\s*\d{7}\s*\d{1}" // Con espacios
            };

            foreach (var pattern in patterns)
            {
                var match = Regex.Match(ocrText, pattern);
                if (match.Success)
                {
                    // Normalizar: quitar espacios y guiones, luego volver a formatear
                    var digits = Regex.Replace(match.Value, @"[^\d]", "");

                    if (digits.Length == 11)
                    {
                        // Formatear como XXX-XXXXXXX-X
                        return $"{digits.Substring(0, 3)}-{digits.Substring(3, 7)}-{digits.Substring(10, 1)}";
                    }
                }
            }

            // Si no se encuentra un patrón válido, buscar cualquier secuencia de 11 dígitos
            var digitsOnly = Regex.Replace(ocrText, @"[^\d]", "");
            if (digitsOnly.Length >= 11)
            {
                var cedulaDigits = digitsOnly.Substring(0, 11);
                return $"{cedulaDigits.Substring(0, 3)}-{cedulaDigits.Substring(3, 7)}-{cedulaDigits.Substring(10, 1)}";
            }

            return string.Empty;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error extrayendo número de cédula: {ex.Message}");
            return string.Empty;
        }
    }
}