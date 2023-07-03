using System;
using System.Diagnostics;
using System.Reactive.Concurrency;
using ReactiveUI;
//using Serilog;

namespace Device.Olfactometer.GUI
{
    public class MyCustomObservableExceptionHandler : IObserver<Exception>
    {
        public void OnNext(Exception value)
        {
            if (Debugger.IsAttached) Debugger.Break();

            //Log.Information(value, "{@Type} - {@Message}", value.GetType().ToString(), value.Message);

            RxApp.MainThreadScheduler.Schedule(() => { throw value; });
        }

        public void OnError(Exception error)
        {
            if (Debugger.IsAttached) Debugger.Break();

            //Log.Error(error, "{@Type} - {@Message}", error.GetType().ToString(), error.Message);

            RxApp.MainThreadScheduler.Schedule(() => { throw error; });
        }

        public void OnCompleted()
        {
            if (Debugger.IsAttached) Debugger.Break();
            RxApp.MainThreadScheduler.Schedule(() => { throw new NotImplementedException(); });
        }
    }
}
