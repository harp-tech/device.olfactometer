using Avalonia.ReactiveUI;
using Olfactometer.Design.ViewModels;

namespace Olfactometer.Design.Views
{
    public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        public MainWindow()
        {
            InitializeComponent();
        }
    }
}