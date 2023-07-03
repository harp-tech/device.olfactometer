#region Usings

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Linq;
using System.Reactive;
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
        
        [Reactive] public ObservableCollection<string> HarpMessages { get; set; }
        
        [ObservableAsProperty] public bool IsLoadingPorts { get; }
        [ObservableAsProperty] public bool IsConnecting { get; }
        
        [Reactive] public string DeviceName { get; set; }
        [Reactive] public int DeviceID { get; set; }
        [Reactive] public HarpVersion HardwareVersion { get; set; }
        [Reactive] public HarpVersion FirmwareVersion { get; set; }
        
        [Reactive] public EnableFlag EnableFlow { get; set; }
        [ObservableAsProperty] public bool IsRunningFlow { get; }
        
        // device fields
        [Reactive] public float Channel0FlowTarget { get; set; }
        [Reactive] public float Channel0FlowReal { get; set; }
        [Reactive] public float Channel1FlowTarget { get; set; }
        [Reactive] public float Channel1FlowReal { get; set; }
        [Reactive] public float Channel2FlowTarget { get; set; }
        [Reactive] public float Channel2FlowReal { get; set; }
        [Reactive] public float Channel3FlowTarget { get; set; }
        [Reactive] public float Channel3FlowReal { get; set; }
        [Reactive] public Channel3RangeConfig Channel3Range { get; set; }
        [Reactive] public List<Channel3RangeConfig> Channel3RangeOptions { get; set; }
        [Reactive] public float Channel4FlowTarget { get; set; }
        [Reactive] public float Channel4FlowReal { get; set; }
        
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
        
        [Reactive] public bool Valve0Toggle { get; set; }
        [Reactive] public bool Valve1Toggle { get; set; }
        [Reactive] public bool Valve2Toggle { get; set; }
        [Reactive] public bool Valve3Toggle { get; set; }
        [Reactive] public bool EndValve0Toggle { get; set; }
        [Reactive] public bool EndValve1Toggle { get; set; }
        
        

        
        
        [Reactive] public bool ShowDarkTheme { get; set; }
        public ReactiveCommand<Unit, Unit> ChangeThemeCommand { get; }
        public ReactiveCommand<Unit, Unit> LoadDeviceInformation { get; }
        public ReactiveCommand<Unit, Unit> ConnectAndGetBaseInfoCommand { get; }
        
        public ReactiveCommand<Unit, Unit> ToggleFlowCommand { get; }
        public ReactiveCommand<Unit, Unit> ShowAboutCommand { get; }
        
        private Harp.Olfactometer.AsyncDevice? _olfactometer;
        private readonly IObserver<HarpMessage> _observer;
        private IDisposable _observable;
        private readonly Subject<HarpMessage> _msgsSubject;

        public OlfactometerViewModel()
        {
            var assembly = typeof(OlfactometerViewModel).Assembly;
            var informationVersion = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                ?.InformationalVersion;
            AppVersion = $"v{informationVersion}";

            Console.WriteLine(
                $"Dotnet version: {System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription}");

            HarpMessages = new ObservableCollection<string>();
            ChangeThemeCommand = ReactiveCommand.Create(ChangeTheme);
            Channel3RangeOptions = Enum.GetValues<Channel3RangeConfig>().ToList();
            Ports = new List<string>();

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
            ToggleFlowCommand.IsExecuting.ToPropertyEx(this, x => x.IsRunningFlow);
            ToggleFlowCommand.ThrownExceptions.Subscribe(ex =>
                //Log.Error(ex, "Error starting protocol with error: {Exception}", ex));
                Console.WriteLine($"Error starting protocol with error: {ex}"));
            
            ShowAboutCommand = ReactiveCommand.CreateFromTask(async () =>
                await new About() { DataContext = new AboutViewModel() }.ShowDialog(
                    (Application.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime).MainWindow));
                
            
            // force initial population of currently connected ports
            LoadUsbInformation();
        }

        private IObservable<Unit> ToggleFlow()
        {
            return Observable.StartAsync(async () =>
            {
                if (_olfactometer == null)
                    throw new Exception("Olfactometer is not connected");
                await _olfactometer.WriteEnableFlowAsync(EnableFlow);
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

                HarpMessages.Clear();

                DeviceID = await _olfactometer.ReadWhoAmIAsync();
                DeviceName = await _olfactometer.ReadDeviceNameAsync();
                HardwareVersion = await _olfactometer.ReadHardwareVersionAsync();
                FirmwareVersion = await _olfactometer.ReadFirmwareVersionAsync();
                
                Connected = true;
                
                // Read all the parameters and update corresponding properties
                Channel0FlowTarget = await _olfactometer.ReadChannel0FlowTargetAsync();
                Channel0FlowReal = await _olfactometer.ReadChannel0FlowRealAsync();
                Channel1FlowTarget = await _olfactometer.ReadChannel1FlowTargetAsync();
                Channel1FlowReal = await _olfactometer.ReadChannel1FlowRealAsync();
                Channel2FlowTarget = await _olfactometer.ReadChannel2FlowTargetAsync();
                Channel2FlowReal = await _olfactometer.ReadChannel2FlowRealAsync();
                Channel3FlowTarget = await _olfactometer.ReadChannel3FlowTargetAsync();
                Channel3FlowReal = await _olfactometer.ReadChannel3FlowRealAsync();
                Channel4FlowTarget = await _olfactometer.ReadChannel4FlowTargetAsync();
                Channel4FlowReal = await _olfactometer.ReadChannel4FlowRealAsync();
                
                PulseValve0 = await _olfactometer.ReadPulseValve0Async();
                PulseValve1 = await _olfactometer.ReadPulseValve1Async();
                PulseValve2 = await _olfactometer.ReadPulseValve2Async();
                PulseValve3 = await _olfactometer.ReadPulseValve3Async();
                PulseEndValve0 = await _olfactometer.ReadPulseEndvalve0Async();
                PulseEndValve1 = await _olfactometer.ReadPulseEndvalve1Async();
                
                // get ValvesState
                var valvesState = await _olfactometer.ReadValvesStateAsync();
                Valve0State = valvesState.HasFlag(Valves.Valve0);
                Valve1State = valvesState.HasFlag(Valves.Valve1);
                Valve2State = valvesState.HasFlag(Valves.Valve2);
                Valve3State = valvesState.HasFlag(Valves.Valve3);
                EndValve0State = valvesState.HasFlag(Valves.EndValve0);
                EndValve1State = valvesState.HasFlag(Valves.EndValve1);
                
                // get ValvesToggle
                var valvesToggle = await _olfactometer.ReadValvesToggleAsync();
                Valve0Toggle = valvesToggle.HasFlag(Valves.Valve0);
                Valve1Toggle = valvesToggle.HasFlag(Valves.Valve1);
                Valve2Toggle = valvesToggle.HasFlag(Valves.Valve2);
                Valve3Toggle = valvesToggle.HasFlag(Valves.Valve3);
                EndValve0Toggle = valvesToggle.HasFlag(Valves.EndValve0);
                EndValve1Toggle = valvesToggle.HasFlag(Valves.EndValve1);

                EnableFlow = await _olfactometer.ReadEnableFlowAsync();

                //observable.Dispose();

                // generate observable for remaining operations
                // _observable = _olfactometer.Generate(_msgsSubject)
                //     .Subscribe(_observer);
            });
        }
    }
}