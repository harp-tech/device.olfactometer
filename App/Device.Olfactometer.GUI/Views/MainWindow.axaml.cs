using Avalonia.Controls;
using Avalonia.ReactiveUI;
using Device.Olfactometer.GUI.ViewModels;

namespace Device.Olfactometer.GUI.Views;

public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
    public MainWindow()
    {
        InitializeComponent();
    }
}