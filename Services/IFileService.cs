using System.Threading.Tasks;

namespace DekelApp.Services
{
    public interface IFileService
    {
        Task SaveAsync<T>(T data, string filePath);
        Task CopyShapefileAsync(string sourceBaseFilePath, string destinationDirectory);
    }
}
