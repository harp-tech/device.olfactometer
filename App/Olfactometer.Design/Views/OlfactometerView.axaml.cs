using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Olfactometer.Design.ViewModels;

namespace Olfactometer.Design.Views
{
    public class OlfactometerView : ReactiveUserControl<OlfactometerViewModel>
    {
        public OlfactometerView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
