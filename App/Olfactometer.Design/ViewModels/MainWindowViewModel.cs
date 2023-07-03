using Device.Olfactometer.GUI.ViewModels;

namespace Device.Olfactometer.GUI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public OlfactometerViewModel Olfactometer { get; set; }

        public MainWindowViewModel()
        {
            Olfactometer = new OlfactometerViewModel();
        }
    }
}