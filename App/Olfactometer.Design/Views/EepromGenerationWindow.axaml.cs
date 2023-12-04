using System;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Olfactometer.Design.ViewModels;
using ReactiveUI;

namespace Olfactometer.Design.Views;

public partial class EepromGenerationWindow : ReactiveWindow<EepromGenerationViewModel>
{
    public EepromGenerationWindow()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif

        this.WhenActivated(d => d(ViewModel.ShowOpenFileDialog.RegisterHandler(DoShowOpenFileDialog)));
        this.WhenActivated(d => d(ViewModel.ShowSaveFileDialog.RegisterHandler(DoShowSaveFileDialog)));
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private async Task DoShowOpenFileDialog(InteractionContext<Unit, string?> interaction)
    {
        var dialog = new OpenFileDialog();
        var fileNames = await dialog.ShowAsync(this);
        interaction.SetOutput((fileNames ?? Array.Empty<string>()).FirstOrDefault());
    }

    private async Task DoShowSaveFileDialog(InteractionContext<Unit, string> interaction)
    {
        var dialog = new SaveFileDialog();
        var fileName = await dialog.ShowAsync(this);
        interaction.SetOutput(fileName);
    }
}
