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
using System.Text;
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
        
        private Bonsai.Harp.Device _dev;
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
                if (_dev != null)
                {
                    // cleanup variables
                    _observable?.Dispose();
                    _observable = null;
                }

                _dev = new Bonsai.Harp.Device
                {
                    PortName = SelectedPort,
                    Heartbeat = EnableFlag.Disable,
                    IgnoreErrors = false
                };

                //Log.Information("Attempting connection to port \'{SelectedPort}\'", SelectedPort);
                Console.WriteLine($"Attempting connection to port \'{SelectedPort}\'");

                HarpMessages.Clear();

                await Task.Delay(300);

                var observable = _dev.Generate()
                    .Where(MessageType.Read)
                    .Do(ReadRegister)
                    .Throttle(TimeSpan.FromSeconds(0.2))
                    .Timeout(TimeSpan.FromSeconds(5))
                    .Subscribe(_ => { },
                                // FIXME: ignore here the connection and perhaps simply return?
                                (ex) => { HarpMessages.Add($"Error while sending commands to device:{ex.Message}"); });

                await Task.Delay(300);

                //Log.Information("Connection established with the following return information: {Info}", configuration);
                //Console.WriteLine($"Connection established with the following return information: {configuration}");

                // present messagebox if we are not handling a Pump device
                // NOTE: for testing connection with a pump
                if (configuration.WhoAmI != 1140 && configuration.WhoAmI != 1280 && configuration.WhoAmI != 1296)
                {
                    // when the configuration.WhoAmI is zero, we are dealing with a non-HARP device, so change message accordingly
                    var message = $"Found a HARP device: {configuration.DeviceName} ({configuration.WhoAmI}).\n\nThis GUI is only for the SyringePump HARP device.\n\nPlease select another serial port.";
                    var icon = Icon.Info;
                    if (configuration.WhoAmI == 0)
                    {
                        message =
                            $"Found a non-HARP device.\n\nThis GUI is only for the SyringePump HARP device.\n\nPlease select another serial port.";
                        icon = Icon.Error;
                    }

                    var messageBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager
                        .GetMessageBoxStandardWindow("Unexpected device found",
                            message,
                            icon: icon);
                    await messageBoxStandardWindow.Show();
                    observable.Dispose();
                    return;
                }

                DeviceName = configuration.DeviceName;
                DeviceID = configuration.WhoAmI;

                // convert Hw and Fw version
                HardwareVersion = configuration.HardwareVersion;
                FirmwareVersion = configuration.FirmwareVersion;

                Connected = true;

                observable.Dispose();

                // generate observable for remaining operations
                _observable = _dev.Generate(_msgsSubject)
                    .Subscribe(_observer);
            });
        }
        
        private void ReadRegister(HarpMessage message)
        {
            switch (message.Address)
            {
                case WhoAmI.Address:
                    configuration.WhoAmI = message.GetPayloadUInt16();
                    break;
                case HardwareVersionHigh.Address:
                    configuration.HardwareVersionHigh = message.GetPayloadByte();
                    break;
                case HardwareVersionLow.Address:
                    configuration.HardwareVersionLow = message.GetPayloadByte();
                    break;
                case FirmwareVersionHigh.Address:
                    configuration.FirmwareVersionHigh = message.GetPayloadByte();
                    break;
                case FirmwareVersionLow.Address:
                    configuration.FirmwareVersionLow = message.GetPayloadByte();
                    break;
                case CoreVersionHigh.Address:
                    configuration.CoreVersionHigh = message.GetPayloadByte();
                    break;
                case CoreVersionLow.Address:
                    configuration.CoreVersionLow = message.GetPayloadByte();
                    break;
                case AssemblyVersion.Address:
                    configuration.AssemblyVersion = message.GetPayloadByte();
                    break;
                case DeviceRegisters.TimestampSecond:
                    configuration.Timestamp = message.GetPayloadUInt32();
                    break;
                case DeviceRegisters.DeviceName:
                    var deviceName = nameof(Device);
                    if (!message.Error)
                    {
                        var namePayload = message.GetPayload();
                        deviceName = Encoding.ASCII.GetString(namePayload.Array, namePayload.Offset, namePayload.Count)
                            .Trim('\0');
                    }

                    configuration.DeviceName = deviceName;
                    break;
                case SerialNumber.Address:
                    configuration.SerialNumber = message.GetPayloadUInt16();
                    break;
            }

            // Update UI with the remaining registers
            // if (message.Address >= (int)(PumpRegisters.EnableMotorDriver))
            //     UpdateUI(message);
        }
    }
}