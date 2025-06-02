using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using Avalonia.ReactiveUI;
using Harp.Olfactometer.Design.ViewModels;
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

        this.WhenActivated(disposables =>
        {
            if (ViewModel != null)
            {
                ViewModel.ShowOpenFileDialog.RegisterHandler(async interaction => await DoShowOpenFileDialog(interaction)).DisposeWith(disposables);
                ViewModel.ShowSaveFileDialog.RegisterHandler(async interaction => await DoShowSaveFileDialog(interaction)).DisposeWith(disposables);
            }
        });
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private async Task DoShowOpenFileDialog(IInteractionContext<Unit, string?> interaction)
    {
        var storageProvider = this.StorageProvider;
        if (storageProvider == null)
        {
            interaction.SetOutput(null);
            return;
        }

        var files = await storageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            AllowMultiple = false
        });

        interaction.SetOutput(files.FirstOrDefault()?.TryGetLocalPath());
    }

    private async Task DoShowSaveFileDialog(IInteractionContext<Unit, string?> interaction)
    {
        var storageProvider = this.StorageProvider;
        if (storageProvider == null)
        {
            interaction.SetOutput(null);
            return;
        }

        var files = await storageProvider.SaveFilePickerAsync(new FilePickerSaveOptions());

        interaction.SetOutput(files?.TryGetLocalPath());
    }
}
