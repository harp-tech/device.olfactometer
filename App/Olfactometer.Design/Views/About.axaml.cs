using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Olfactometer.Design.ViewModels;

namespace Olfactometer.Design.Views
{
    public partial class About : ReactiveWindow<AboutViewModel>
    {
        public About()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}