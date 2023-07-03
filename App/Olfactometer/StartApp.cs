using Avalonia;
using Olfactometer.Design;

namespace Olfactometer
{
    internal class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
        }
    
        public static AppBuilder BuildAvaloniaApp()
        {
            return StartApp.BuildAvaloniaApp();
        }
    }
}