using DekelApp.Models;

namespace DekelApp.Services
{
    public interface IValidationService
    {
        bool ValidateAppData(AppData appData, out string errorMessage);
    }
}
