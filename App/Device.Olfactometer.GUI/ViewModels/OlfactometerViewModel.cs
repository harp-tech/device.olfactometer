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
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Themes.Fluent;
using Bonsai.Harp;
using Device.Olfactometer.GUI.Models;
using MessageBox.Avalonia.Enums;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Helpers;



#endregion

namespace Device.Olfactometer.GUI.ViewModels
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
        
        [Reactive] public bool ShowDarkTheme { get; set; }
        public ReactiveCommand<Unit, Unit> ChangeThemeCommand { get; }
        public ReactiveCommand<Unit, Unit> LoadDeviceInformation { get; }
        public ReactiveCommand<Unit, Unit> ConnectAndGetBaseInfoCommand { get; }
        
        private Harp.Olfactometer.AsyncDevice? _olfactometer;
        private readonly IObserver<HarpMessage> _observer;
        private IDisposable _observable;
        private readonly Subject<HarpMessage> _msgsSubject;
        private DeviceConfiguration configuration;

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
            
            // force initial population of currently connected ports
            LoadUsbInformation();
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

                configuration = new DeviceConfiguration();
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

                //observable.Dispose();

                // generate observable for remaining operations
                // _observable = _olfactometer.Generate(_msgsSubject)
                //     .Subscribe(_observer);
            });
        }
    }
}