using System.Reactive;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Olfactometer.Design.Views;
using ReactiveUI;

namespace Harp.Olfactometer.Design.ViewModels;

public class AboutViewModel : ViewModelBase
{
    public ReactiveCommand<Unit, Unit> GenerateEepromCommand { get; }

    public AboutViewModel()
    {
        GenerateEepromCommand = ReactiveCommand.CreateFromTask(async () =>
                await new EepromGenerationWindow() { DataContext = new EepromGenerationViewModel() }.ShowDialog(
                    (Application.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime).MainWindow));

    }
}
