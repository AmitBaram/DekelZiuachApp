using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace DekelApp.Services
{
    public class FileService : IFileService
    {
        public async Task SaveAsync<T>(T data, string filePath)
        {
            var directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var options = new JsonSerializerOptions 
            { 
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
            var json = JsonSerializer.Serialize(data, options);
            await File.WriteAllTextAsync(filePath, json);
        }

        public async Task CopyShapefileAsync(string sourceBaseFilePath, string destinationDirectory)
        {
            if (string.IsNullOrEmpty(sourceBaseFilePath) || !File.Exists(sourceBaseFilePath)) return;

            if (!Directory.Exists(destinationDirectory))
            {
                Directory.CreateDirectory(destinationDirectory);
            }

            var directory = Path.GetDirectoryName(sourceBaseFilePath) ?? string.Empty;
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(sourceBaseFilePath);

            // Find all files with the same name (regardless of extension) in the same directory
            var relatedFiles = Directory.GetFiles(directory, fileNameWithoutExtension + ".*");

            foreach (var file in relatedFiles)
            {
                var destFile = Path.Combine(destinationDirectory, Path.GetFileName(file));
                await Task.Run(() => File.Copy(file, destFile, true));
            }
        }
    }
}
