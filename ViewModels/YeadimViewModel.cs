using DekelApp.Models;
using DekelApp.Utils;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace DekelApp.ViewModels
{
    public class YeadimViewModel : BaseViewModel
    {
        public ObservableCollection<YeadimTargetModel> Targets { get; }

        public ICommand AddTargetCommand { get; }
        public ICommand DeleteTargetCommand { get; }

        public YeadimViewModel(ObservableCollection<YeadimTargetModel> targets)
        {
            Targets = targets;
            AddTargetCommand = new RelayCommand(_ => AddTarget());
            DeleteTargetCommand = new RelayCommand(target => DeleteTarget(target));
        }

        private void AddTarget()
        {
            Targets.Add(new YeadimTargetModel() { Name = "New Target", Easting = "0", Northing = "0", Zone = "36N" });
        }

        private void DeleteTarget(object? target)
        {
            if (target is YeadimTargetModel model)
            {
                Targets.Remove(model);
            }
        }
    }
}
