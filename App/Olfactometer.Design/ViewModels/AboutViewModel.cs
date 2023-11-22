using System.Reactive;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Themes.Fluent;
using Olfactometer.Design.Views;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Olfactometer.Design.ViewModels
{
    public class AboutViewModel : ReactiveObject
    {
        [Reactive] public bool ShowDarkTheme { get; set; }

        public ReactiveCommand<Unit, Unit> GenerateEepromCommand { get; }

        public AboutViewModel()
        {
            // Get current theme
            ShowDarkTheme = ((FluentTheme)App.Current.Styles[0]).Mode == FluentThemeMode.Dark;

            GenerateEepromCommand = ReactiveCommand.CreateFromTask(async () =>
                await new EepromGenerationWindow() { DataContext = new EepromGenerationViewModel() }.ShowDialog(
                    (Application.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime).MainWindow));
        }
    }
}
