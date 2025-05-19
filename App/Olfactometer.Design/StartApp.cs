using Avalonia;
using Avalonia.ReactiveUI;
using ReactiveUI;

namespace Olfactometer.Design
{
    public class StartApp
    {
        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
        {
            RxApp.DefaultExceptionHandler = new MyCustomObservableExceptionHandler();

            return AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToTrace()
                .UseReactiveUI();
        }
    }
}
