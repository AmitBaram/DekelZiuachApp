using DekelApp.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DekelApp.Services
{
    public interface IGeoCellService
    {
        List<string> CalculateGeoCells(ObservableCollection<TichumAreaModel> tichumAreas);
    }
}
