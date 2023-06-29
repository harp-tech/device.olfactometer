using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Device.Olfactometer.GUI.ViewModels;

namespace Device.Olfactometer.GUI.Views
{
    public partial class OlfactometerView : ReactiveUserControl<OlfactometerViewModel>
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