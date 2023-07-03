namespace Olfactometer.Design.ViewModels
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