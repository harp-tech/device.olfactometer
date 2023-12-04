using System.Runtime.InteropServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Olfactometer.Design.ViewModels;

namespace Olfactometer.Design.Views
{
    public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        public MainWindow()
        {
            InitializeComponent();
            SizeToContent = SizeToContent.WidthAndHeight;

#if DEBUG
            this.AttachDevTools();
#endif
            // check current OS and change window presentation and padding
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                ExtendClientAreaToDecorationsHint = false;
                Padding = new Thickness(0, 0, 0, 0);
            }
        }
    }
}
