#region Usings

using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reflection;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Themes.Fluent;
using Bonsai.Harp;
using Harp.Olfactometer;
using MessageBox.Avalonia.Enums;
using Olfactometer.Design.Views;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Helpers;

#endregion

namespace Olfactometer.Design.ViewModels
{
    public class OlfactometerViewModel : ReactiveValidationObject
    {
        public string AppVersion { get; set; }

        [Reactive] public List<string> Ports { get; set; }
        [Reactive] public string SelectedPort { get; set; }
        [Reactive] public bool Connected { get; set; }

        [ObservableAsProperty] public bool IsLoadingPorts { get; }
        [ObservableAsProperty] public bool IsConnecting { get; }
        [ObservableAsProperty] public bool IsResetting { get; }
        [ObservableAsProperty] public bool IsSaving { get; }

        [Reactive] public string DeviceName { get; set; }
        [Reactive] public int DeviceID { get; set; }
        [Reactive] public HarpVersion HardwareVersion { get; set; }
        [Reactive] public HarpVersion FirmwareVersion { get; set; }

        [Reactive] public EnableFlag EnableFlow { get; set; }
        [Reactive] public bool RunningFlow { get; set; }
        [ObservableAsProperty] public bool IsStartingFlow { get; }

        // device fields
        [Reactive] public float Channel0TargetFlow { get; set; }
        [Reactive] public float Channel0ActualFlow { get; set; }
        [Reactive] public float Channel1TargetFlow { get; set; }
        [Reactive] public float Channel1ActualFlow { get; set; }
        [Reactive] public float Channel2TargetFlow { get; set; }
        [Reactive] public float Channel2ActualFlow { get; set; }
        [Reactive] public float Channel3TargetFlow { get; set; }
        [Reactive] public float Channel3ActualFlow { get; set; }
        [Reactive] public int Channel3Range { get; set; }
        [Reactive] public int Channel3MaxValue { get; set; }
        [Reactive] public float Channel4TargetFlow { get; set; }
        [Reactive] public float Channel4ActualFlow { get; set; }

        [Reactive] public ushort PulseValve0 { get; set; }
        [Reactive] public ushort PulseValve1 { get; set; }
        [Reactive] public ushort PulseValve2 { get; set; }
        [Reactive] public ushort PulseValve3 { get; set; }
        [Reactive] public ushort PulseEndValve0 { get; set; }
        [Reactive] public ushort PulseEndValve1 { get; set; }

        [Reactive] public bool Valve0State { get; set; }
        [Reactive] public bool Valve1State { get; set; }
        [Reactive] public bool Valve2State { get; set; }
        [Reactive] public bool Valve3State { get; set; }
        [Reactive] public bool EndValve0State { get; set; }
        [Reactive] public bool EndValve1State { get; set; }

        [Reactive] public bool Valve0Pulse { get; set; }
        [Reactive] public bool Valve1Pulse { get; set; }
        [Reactive] public bool Valve2Pulse { get; set; }
        [Reactive] public bool Valve3Pulse { get; set; }
        [Reactive] public bool EndValve0Pulse { get; set; }
        [Reactive] public bool EndValve1Pulse { get; set; }

        [Reactive] public int DigitalOutput0Config { get; set; }
        [Reactive] public int DigitalOutput1Config { get; set; }
        [Reactive] public int DigitalInput0Config { get; set; }

        [Reactive] public bool FlowmeterEvent { get; set; } = true;
        [Reactive] public bool DI0TriggerEvent { get; set; } = true;
        [Reactive] public bool ChannelActualFlowEvent { get; set; } = true;

        [Reactive] public int ValveExternalControl { get; set; }

        [Reactive] public int MimicValve0 { get; set; }
        [Reactive] public int MimicValve1 { get; set; }
        [Reactive] public int MimicValve2 { get; set; }
        [Reactive] public int MimicValve3 { get; set; }
        [Reactive] public int MimicEndValve0 { get; set; }
        [Reactive] public int MimicEndValve1 { get; set; }

        [Reactive] public bool ShowDarkTheme { get; set; }
        public ReactiveCommand<Unit, Unit> ChangeThemeCommand { get; }
        public ReactiveCommand<Unit, Unit> LoadDeviceInformation { get; }
        public ReactiveCommand<Unit, Unit> ConnectAndGetBaseInfoCommand { get; }
        public ReactiveCommand<Unit, Unit> ToggleFlowCommand { get; }
        public ReactiveCommand<bool, Unit> SaveConfigurationCommand { get; }
        public ReactiveCommand<Unit, Unit> ResetConfigurationCommand { get; }
        public ReactiveCommand<Unit, Unit> ShowAboutCommand { get; }

        private Harp.Olfactometer.AsyncDevice? _olfactometer;
        private readonly IObserver<HarpMessage> _observer;
        private IDisposable _observable;
        private readonly Subject<HarpMessage> _msgsSubject;
        private Valves _valvesPulse;
        private IObservable<long> _actualFlowObservable;

        public OlfactometerViewModel()
        {
            var assembly = typeof(OlfactometerViewModel).Assembly;
            var informationVersion = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                ?.InformationalVersion;
            AppVersion = $"v{informationVersion}";

            Console.WriteLine(
                $"Dotnet version: {System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription}");

            ChangeThemeCommand = ReactiveCommand.Create(ChangeTheme);
            Ports = new List<string>();
            const int periodInMilliseconds = 100;
            _actualFlowObservable = Observable
                .Interval(TimeSpan.FromMilliseconds(periodInMilliseconds), Scheduler.Default)
                .TakeUntil(this.WhenAnyValue(x => x.EnableFlow).Where(x => x == EnableFlag.Disable));

            LoadDeviceInformation = ReactiveCommand.CreateFromObservable(LoadUsbInformation);
            LoadDeviceInformation.IsExecuting.ToPropertyEx(this, x => x.IsLoadingPorts);
            LoadDeviceInformation.ThrownExceptions.Subscribe(ex =>
                //Log.Error(ex, "Error loading device information with exception: {Exception}", ex));
                Console.WriteLine($"Error loading device information with exception: {ex}"));

            // can connect if there is a selection and also if the new selection is different than the old one
            var canConnect = this.WhenAnyValue(x => x.SelectedPort)
                .Select(selectedPort => !string.IsNullOrEmpty(selectedPort));

            ConnectAndGetBaseInfoCommand = ReactiveCommand.CreateFromObservable(ConnectAndGetBaseInfo, canConnect);
            ConnectAndGetBaseInfoCommand.IsExecuting.ToPropertyEx(this, x => x.IsConnecting);
            ConnectAndGetBaseInfoCommand.ThrownExceptions.Subscribe(ex =>
                //Log.Error(ex, "Error connecting to device with error: {Exception}", ex));
                Console.WriteLine($"Error connecting to device with error: {ex}"));

            var canChangeConfig = this.WhenAnyValue(x => x.Connected).Select(connected => connected);
            ToggleFlowCommand = ReactiveCommand.CreateFromObservable(ToggleFlow, canChangeConfig);
            ToggleFlowCommand.IsExecuting.ToPropertyEx(this, x => x.IsStartingFlow);
            ToggleFlowCommand.ThrownExceptions.Subscribe(ex =>
                //Log.Error(ex, "Error starting protocol with error: {Exception}", ex));
                Console.WriteLine($"Error starting protocol with error: {ex}"));

            SaveConfigurationCommand =
                ReactiveCommand.CreateFromObservable<bool, Unit>(SaveConfiguration, canChangeConfig);
            SaveConfigurationCommand.IsExecuting.ToPropertyEx(this, x => x.IsSaving);
            SaveConfigurationCommand.ThrownExceptions.Subscribe(ex =>
                //Log.Error(ex, "Error saving configuration with error: {Exception}", ex));
                Console.WriteLine($"Error saving configuration with error: {ex}"));

            ResetConfigurationCommand = ReactiveCommand.CreateFromObservable(ResetConfiguration, canChangeConfig);
            ResetConfigurationCommand.IsExecuting.ToPropertyEx(this, x => x.IsResetting);
            ResetConfigurationCommand.ThrownExceptions.Subscribe(ex =>
                //Log.Error(ex, "Error resetting device configuration with error: {Exception}", ex));
                Console.WriteLine($"Error resetting device configuration with error: {ex}"));

            ShowAboutCommand = ReactiveCommand.CreateFromTask(async () =>
                await new About() { DataContext = new AboutViewModel() }.ShowDialog(
                    (Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow));

            this.WhenAnyValue(x => x.Channel0TargetFlow, x => x.Channel1TargetFlow, x => x.Channel2TargetFlow,
                    x => x.Channel3TargetFlow, x => x.Channel4TargetFlow)
                .Throttle(TimeSpan.FromMilliseconds(200))
                .Subscribe(async x =>
                {
                    if (_olfactometer != null)
                    {
                        // This could also be 5 separate calls to WriteChannelXTargetFlowAsync
                        await _olfactometer.WriteChannelsTargetFlowAsync(new ChannelsTargetFlowPayload(
                            Channel0TargetFlow, Channel1TargetFlow, Channel2TargetFlow, Channel3TargetFlow,
                            Channel4TargetFlow));

                        // Update actual flow manually to zero when target flow is changed to zero
                        Channel0ActualFlow = Channel0TargetFlow == 0 ? 0 : Channel0ActualFlow;
                        Channel1ActualFlow = Channel1TargetFlow == 0 ? 0 : Channel1ActualFlow;
                        Channel2ActualFlow = Channel2TargetFlow == 0 ? 0 : Channel2ActualFlow;
                        Channel3ActualFlow = Channel3TargetFlow == 0 ? 0 : Channel3ActualFlow;
                        Channel4ActualFlow = Channel4TargetFlow == 0 ? 0 : Channel4ActualFlow;
                    }
                });

            this.WhenAnyValue(x => x.Valve0State, x => x.Valve1State, x => x.Valve2State, x => x.Valve3State,
                    x => x.EndValve0State, x => x.EndValve1State)
                .Subscribe(async x =>
                {
                    if (_olfactometer != null)
                    {
                        await _olfactometer.WriteOdorValveStateAsync(GetCurrentOdorValvesState());
                        await _olfactometer.WriteEndValveStateAsync(GetCurrentEndValvesState());
                    }
                });

            this.WhenAnyValue(x => x.Valve0Pulse, x => x.Valve1Pulse, x => x.Valve2Pulse, x => x.Valve3Pulse,
                    x => x.EndValve0Pulse, x => x.EndValve1Pulse)
                .Subscribe(async x =>
                {
                    if (_olfactometer != null)
                        await _olfactometer.WriteEnableValvePulseAsync(GetCurrentValvesPulse());
                });

            this.WhenAnyValue(x => x.Channel3Range).Subscribe(async x =>
            {
                if (_olfactometer != null)
                    await _olfactometer.WriteChannel3RangeAsync((Channel3RangeConfig)Channel3Range);

                // update Channel3MaxValue
                Channel3MaxValue = Channel3Range == 0 ? 100 : 1000;
            });

            this.WhenAnyValue(x => x.EnableFlow)
                .Subscribe(x => RunningFlow = x == EnableFlag.Enable);

            // force initial population of currently connected ports
            LoadUsbInformation();
        }

        private IObservable<Unit> SaveConfiguration(bool savePermanently)
        {
            return Observable.StartAsync(async () =>
            {
                if (_olfactometer == null)
                    throw new Exception("You need to connect to the device first");

                // TODO: get all configuration values from the UI
                //Events
                OlfactometerEvents events = 0;
                if (FlowmeterEvent)
                    events |= OlfactometerEvents.Flowmeter;
                if (DI0TriggerEvent)
                    events |= OlfactometerEvents.DI0Trigger;
                if (ChannelActualFlowEvent)
                    events |= OlfactometerEvents.ChannelActualFlow;

                if (events != 0)
                    await _olfactometer.WriteEnableEventsAsync(events);

                // Mimic Valves
                await _olfactometer.WriteMimicValve0Async((MimicOutputs)MimicValve0);
                await _olfactometer.WriteMimicValve1Async((MimicOutputs)MimicValve1);
                await _olfactometer.WriteMimicValve2Async((MimicOutputs)MimicValve2);
                await _olfactometer.WriteMimicValve3Async((MimicOutputs)MimicValve3);
                await _olfactometer.WriteMimicEndvalve0Async((MimicOutputs)MimicEndValve0);
                await _olfactometer.WriteMimicEndvalve1Async((MimicOutputs)MimicEndValve1);

                // DO0 Sync, DO1 Sync, DI0 Trigger
                await _olfactometer.WriteDO0SyncAsync((DO0SyncConfig)DigitalOutput0Config);
                await _olfactometer.WriteDO1SyncAsync((DO1SyncConfig)DigitalOutput1Config);
                await _olfactometer.WriteDI0TriggerAsync((DI0TriggerConfig)DigitalInput0Config);

                // External control valves
                await _olfactometer.WriteEnableValveExternalControlAsync((EnableFlag)ValveExternalControl);

                if (savePermanently)
                {
                    await _olfactometer.WriteResetDeviceAsync(ResetFlags.Save);
                }
            });
        }

        private IObservable<Unit> ResetConfiguration()
        {
            return Observable.StartAsync(async () =>
            {
                if (_olfactometer != null)
                {
                    await _olfactometer.WriteResetDeviceAsync(ResetFlags.RestoreDefault);
                }
            });
        }

        private IObservable<Unit> ToggleFlow()
        {
            return Observable.StartAsync(async () =>
            {
                if (_olfactometer == null)
                    throw new Exception("Olfactometer is not connected");
                await _olfactometer.WriteEnableFlowAsync(EnableFlow == EnableFlag.Enable
                    ? EnableFlag.Disable
                    : EnableFlag.Enable);
                // update EnableFlow to the actual value
                EnableFlow = await _olfactometer.ReadEnableFlowAsync();

                if (EnableFlow == EnableFlag.Enable)
                {
                    _actualFlowObservable.Subscribe(async _ =>
                    {
                        if (_olfactometer != null)
                        {
                            Channel0ActualFlow = await _olfactometer.ReadChannel0ActualFlowAsync();
                            Channel1ActualFlow = await _olfactometer.ReadChannel1ActualFlowAsync();
                            Channel2ActualFlow = await _olfactometer.ReadChannel2ActualFlowAsync();
                            Channel3ActualFlow = await _olfactometer.ReadChannel3ActualFlowAsync();
                            Channel4ActualFlow = await _olfactometer.ReadChannel4ActualFlowAsync();
                        }
                    });
                }
            });
        }

        private void ChangeTheme()
        {
            Application.Current.Styles[0] = new FluentTheme(new Uri("avares://ControlCatalog/Styles"))
            {
                Mode = ShowDarkTheme ? FluentThemeMode.Dark : FluentThemeMode.Light
            };
        }

        private IObservable<Unit> LoadUsbInformation()
        {
            return Observable.Start(() =>
            {
                var devices = SerialPort.GetPortNames();

                if (OperatingSystem.IsMacOS())
                    // except with Bluetooth in the name
                    Ports = devices.Where(d => d.Contains("cu.")).Except(devices.Where(d => d.Contains("Bluetooth")))
                        .ToList();
                else
                    Ports = devices.ToList();

                //Log.Information("Loaded USB information");
                Console.WriteLine("Loaded USB information");
            });
        }

        private IObservable<Unit> ConnectAndGetBaseInfo()
        {
            return Observable.StartAsync(async () =>
            {
                if (string.IsNullOrEmpty(SelectedPort))
                    throw new Exception("invalid parameter");

                if (_olfactometer != null)
                {
                    _olfactometer.Dispose();
                    _olfactometer = null;
                    // cleanup variables
                    _observable?.Dispose();
                    _observable = null;
                }

                try
                {
                    _olfactometer = await Harp.Olfactometer.Device.CreateAsync(SelectedPort);
                }
                catch (HarpException ex)
                {
                    //Log.Error(ex, "Error creating device with exception: {Exception}", ex);
                    Console.WriteLine($"Error creating device with exception: {ex}");

                    var messageBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager
                        .GetMessageBoxStandardWindow("Unexpected device found",
                            ex.Message,
                            icon: Icon.Error);
                    await messageBoxStandardWindow.Show();

                    _olfactometer?.Dispose();
                    _olfactometer = null;
                    return;
                }

                //Log.Information("Attempting connection to port \'{SelectedPort}\'", SelectedPort);
                Console.WriteLine($"Attempting connection to port \'{SelectedPort}\'");

                DeviceID = await _olfactometer.ReadWhoAmIAsync();
                DeviceName = await _olfactometer.ReadDeviceNameAsync();
                HardwareVersion = await _olfactometer.ReadHardwareVersionAsync();
                FirmwareVersion = await _olfactometer.ReadFirmwareVersionAsync();

                Connected = true;

                // Read all the parameters and update corresponding properties
                Channel0TargetFlow = await _olfactometer.ReadChannel0TargetFlowAsync();
                Channel0ActualFlow = await _olfactometer.ReadChannel0ActualFlowAsync();
                Channel1TargetFlow = await _olfactometer.ReadChannel1TargetFlowAsync();
                Channel1ActualFlow = await _olfactometer.ReadChannel1ActualFlowAsync();
                Channel2TargetFlow = await _olfactometer.ReadChannel2TargetFlowAsync();
                Channel2ActualFlow = await _olfactometer.ReadChannel2ActualFlowAsync();
                Channel3TargetFlow = await _olfactometer.ReadChannel3TargetFlowAsync();
                Channel3ActualFlow = await _olfactometer.ReadChannel3ActualFlowAsync();
                Channel4TargetFlow = await _olfactometer.ReadChannel4TargetFlowAsync();
                Channel4ActualFlow = await _olfactometer.ReadChannel4ActualFlowAsync();

                PulseValve0 = await _olfactometer.ReadValve0PulseDurationAsync();
                PulseValve1 = await _olfactometer.ReadValve1PulseDurationAsync();
                PulseValve2 = await _olfactometer.ReadValve2PulseDurationAsync();
                PulseValve3 = await _olfactometer.ReadValve3PulseDurationAsync();
                PulseEndValve0 = await _olfactometer.ReadEndValve0PulseDurationAsync();
                PulseEndValve1 = await _olfactometer.ReadEndValve1PulseDurationAsync();

                // get ValvesState (OdorValves and EndValves)
                var odorValvesState = await _olfactometer.ReadOdorValveStateAsync();
                Valve0State = odorValvesState.HasFlag(OdorValves.Valve0);
                Valve1State = odorValvesState.HasFlag(OdorValves.Valve1);
                Valve2State = odorValvesState.HasFlag(OdorValves.Valve2);
                Valve3State = odorValvesState.HasFlag(OdorValves.Valve3);
                var endValvesState = await _olfactometer.ReadEndValveStateAsync();
                EndValve0State = endValvesState.HasFlag(EndValves.EndValve0);
                EndValve1State = endValvesState.HasFlag(EndValves.EndValve1);

                // get ValvesPulse
                var valvesPulse = await _olfactometer.ReadEnableValvePulseAsync();
                Valve0Pulse = valvesPulse.HasFlag(Valves.Valve0);
                Valve1Pulse = valvesPulse.HasFlag(Valves.Valve1);
                Valve2Pulse = valvesPulse.HasFlag(Valves.Valve2);
                Valve3Pulse = valvesPulse.HasFlag(Valves.Valve3);
                EndValve0Pulse = valvesPulse.HasFlag(Valves.EndValve0);
                EndValve1Pulse = valvesPulse.HasFlag(Valves.EndValve1);

                // get default values for DO0_SYNC, DO1_SYNC, DI0_TRIGGER
                var do0Sync = await _olfactometer.ReadDO0SyncAsync();
                DigitalOutput0Config = (int)do0Sync;

                var do1Sync = await _olfactometer.ReadDO1SyncAsync();
                DigitalOutput1Config = (int)do1Sync;

                var di0Trigger = await _olfactometer.ReadDI0TriggerAsync();
                DigitalInput0Config = (int)di0Trigger;

                // External control valves
                ValveExternalControl = (int)await _olfactometer.ReadEnableValveExternalControlAsync();

                // Channel3Range
                Channel3Range = (int)await _olfactometer.ReadChannel3RangeAsync();

                // Events
                var events = await _olfactometer.ReadEnableEventsAsync();
                FlowmeterEvent = events.HasFlag(OlfactometerEvents.Flowmeter);
                DI0TriggerEvent = events.HasFlag(OlfactometerEvents.DI0Trigger);
                ChannelActualFlowEvent = events.HasFlag(OlfactometerEvents.ChannelActualFlow);

                // MimicValves
                MimicValve0 = (int)await _olfactometer.ReadMimicValve0Async();
                MimicValve1 = (int)await _olfactometer.ReadMimicValve1Async();
                MimicValve2 = (int)await _olfactometer.ReadMimicValve2Async();
                MimicValve3 = (int)await _olfactometer.ReadMimicValve3Async();
                MimicEndValve0 = (int)await _olfactometer.ReadMimicEndvalve0Async();
                MimicEndValve1 = (int)await _olfactometer.ReadMimicEndvalve1Async();

                EnableFlow = await _olfactometer.ReadEnableFlowAsync();
            });
        }

        private Valves GetCurrentValvesPulse()
        {
            _valvesPulse = 0;
            if (Valve0Pulse)
                _valvesPulse |= Valves.Valve0;
            if (Valve1Pulse)
                _valvesPulse |= Valves.Valve1;
            if (Valve2Pulse)
                _valvesPulse |= Valves.Valve2;
            if (Valve3Pulse)
                _valvesPulse |= Valves.Valve3;
            if (EndValve0Pulse)
                _valvesPulse |= Valves.EndValve0;
            if (EndValve1Pulse)
                _valvesPulse |= Valves.EndValve1;

            return _valvesPulse;
        }

        private OdorValves GetCurrentOdorValvesState()
        {
            OdorValves odorValvesState = 0;
            if (Valve0State)
                odorValvesState |= OdorValves.Valve0;
            if (Valve1State)
                odorValvesState |= OdorValves.Valve1;
            if (Valve2State)
                odorValvesState |= OdorValves.Valve2;
            if (Valve3State)
                odorValvesState |= OdorValves.Valve3;

            return odorValvesState;
        }

        private EndValves GetCurrentEndValvesState()
        {
            EndValves endValvesState = 0;
            if (EndValve0State)
                endValvesState |= EndValves.EndValve0;
            if (EndValve1State)
                endValvesState |= EndValves.EndValve1;

            return endValvesState;
        }
    }
}
