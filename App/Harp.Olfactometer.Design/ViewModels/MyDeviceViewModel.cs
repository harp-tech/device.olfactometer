using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Bonsai.Harp;
using Harp.Olfactometer.Design.Views;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Harp.Olfactometer.Design.ViewModels;

public class OlfactometerViewModel : ViewModelBase
{
    public string AppVersion { get; set; }
    public ReactiveCommand<Unit, Unit> LoadDeviceInformation { get; }

    #region Connection Information

    [Reactive] public ObservableCollection<string> Ports { get; set; }
    [Reactive] public string SelectedPort { get; set; }
    [Reactive] public bool Connected { get; set; }
    [Reactive] public string ConnectButtonText { get; set; } = "Connect";
    public ReactiveCommand<Unit, Unit> ConnectAndGetBaseInfoCommand { get; }

    #endregion

    #region Operations

    public ReactiveCommand<bool, Unit> SaveConfigurationCommand { get; }
    public ReactiveCommand<Unit, Unit> ResetConfigurationCommand { get; }

    #endregion

    #region Device basic information

    [Reactive] public int DeviceID { get; set; }
    [Reactive] public string DeviceName { get; set; }
    [Reactive] public HarpVersion HardwareVersion { get; set; }
    [Reactive] public HarpVersion FirmwareVersion { get; set; }
    [Reactive] public int SerialNumber { get; set; }

    #endregion

    #region Registers

    [Reactive] public EnableFlag EnableFlow { get; set; }
    [Reactive] public FlowmeterPayload Flowmeter { get; set; }
    [Reactive] public DigitalState DI0State { get; set; }
    [Reactive] public ushort[] Channel0UserCalibration { get; set; }
    [Reactive] public ushort[] Channel1UserCalibration { get; set; }
    [Reactive] public ushort[] Channel2UserCalibration { get; set; }
    [Reactive] public ushort[] Channel3UserCalibration { get; set; }
    [Reactive] public ushort[] Channel4UserCalibration { get; set; }
    [Reactive] public ushort[] Channel3UserCalibrationAux { get; set; }
    [Reactive] public EnableFlag EnableUserCalibration { get; set; }
    [Reactive] public float Channel0TargetFlow { get; set; } = 0f;
    [Reactive] public float Channel1TargetFlow { get; set; } = 0f;
    [Reactive] public float Channel2TargetFlow { get; set; } = 0f;
    [Reactive] public float Channel3TargetFlow { get; set; } = 0f;
    [Reactive] public float Channel4TargetFlow { get; set; } = 0f;
    [Reactive] public ChannelsTargetFlowPayload ChannelsTargetFlow { get; set; }
    [Reactive] public float Channel0ActualFlow { get; set; }
    [Reactive] public float Channel1ActualFlow { get; set; }
    [Reactive] public float Channel2ActualFlow { get; set; }
    [Reactive] public float Channel3ActualFlow { get; set; }
    [Reactive] public float Channel4ActualFlow { get; set; }
    [Reactive] public float Channel0DutyCycle { get; set; }
    [Reactive] public float Channel1DutyCycle { get; set; }
    [Reactive] public float Channel2DutyCycle { get; set; }
    [Reactive] public float Channel3DutyCycle { get; set; }
    [Reactive] public float Channel4DutyCycle { get; set; }
    [Reactive] public DigitalOutputs DigitalOutputSet { get; set; }
    [Reactive] public DigitalOutputs DigitalOutputClear { get; set; }
    [Reactive] public DigitalOutputs DigitalOutputToggle { get; set; }
    [Reactive] public DigitalOutputs DigitalOutputState { get; set; }
    [Reactive] public Valves EnableValvePulse { get; set; }
    [Reactive] public Valves ValveSet { get; set; }
    [Reactive] public Valves ValveClear { get; set; }
    [Reactive] public Valves ValveToggle { get; set; }
    [Reactive] public Valves ValveState { get; set; }
    [Reactive] public OdorValves OdorValveState { get; set; }
    [Reactive] public EndValves EndValveState { get; set; }
    [Reactive] public CheckValves CheckValveState { get; set; }
    [Reactive] public ushort Valve0PulseDuration { get; set; }
    [Reactive] public ushort Valve1PulseDuration { get; set; }
    [Reactive] public ushort Valve2PulseDuration { get; set; }
    [Reactive] public ushort Valve3PulseDuration { get; set; }
    [Reactive] public ushort CheckValve0DelayPulseDuration { get; set; }
    [Reactive] public ushort CheckValve1DelayPulseDuration { get; set; }
    [Reactive] public ushort CheckValve2DelayPulseDuration { get; set; }
    [Reactive] public ushort CheckValve3DelayPulseDuration { get; set; }
    [Reactive] public ushort EndValve0PulseDuration { get; set; }
    [Reactive] public ushort EndValve1PulseDuration { get; set; }
    [Reactive] public DO0SyncConfig DO0Sync { get; set; }
    [Reactive] public DO1SyncConfig DO1Sync { get; set; }
    [Reactive] public DI0TriggerConfig DI0Trigger { get; set; }
    [Reactive] public MimicOutputs MimicValve0 { get; set; }
    [Reactive] public MimicOutputs MimicValve1 { get; set; }
    [Reactive] public MimicOutputs MimicValve2 { get; set; }
    [Reactive] public MimicOutputs MimicValve3 { get; set; }
    [Reactive] public MimicOutputs MimicCheckValve0 { get; set; }
    [Reactive] public MimicOutputs MimicCheckValve1 { get; set; }
    [Reactive] public MimicOutputs MimicCheckValve2 { get; set; }
    [Reactive] public MimicOutputs MimicCheckValve3 { get; set; }
    [Reactive] public MimicOutputs MimicEndValve0 { get; set; }
    [Reactive] public MimicOutputs MimicEndValve1 { get; set; }
    [Reactive] public EnableFlag EnableValveExternalControl { get; set; }
    [Reactive] public Channel3RangeConfig Channel3Range { get; set; }
    [Reactive] public CheckValves EnableCheckValveSync { get; set; }
    [Reactive] public byte TemperatureValue { get; set; }
    [Reactive] public EnableFlag EnableTemperatureCalibration { get; set; }
    [Reactive] public byte TemperatureCalibrationValue { get; set; }
    [Reactive] public OlfactometerEvents EnableEvents { get; set; }

    #endregion

    #region Events Flags

    public bool IsFlowmeterEnabled
    {
        get
        {
            return EnableEvents.HasFlag(OlfactometerEvents.Flowmeter);
        }
        set
        {
            if (value)
            {
                EnableEvents |= OlfactometerEvents.Flowmeter;
            }
            else
            {
                EnableEvents &= ~OlfactometerEvents.Flowmeter;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsFlowmeterEnabled));
            this.RaisePropertyChanged(nameof(EnableEvents));
        }
    }

    public bool IsDI0TriggerEnabled
    {
        get
        {
            return EnableEvents.HasFlag(OlfactometerEvents.DI0Trigger);
        }
        set
        {
            if (value)
            {
                EnableEvents |= OlfactometerEvents.DI0Trigger;
            }
            else
            {
                EnableEvents &= ~OlfactometerEvents.DI0Trigger;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsDI0TriggerEnabled));
            this.RaisePropertyChanged(nameof(EnableEvents));
        }
    }

    public bool IsChannelActualFlowEnabled
    {
        get
        {
            return EnableEvents.HasFlag(OlfactometerEvents.ChannelActualFlow);
        }
        set
        {
            if (value)
            {
                EnableEvents |= OlfactometerEvents.ChannelActualFlow;
            }
            else
            {
                EnableEvents &= ~OlfactometerEvents.ChannelActualFlow;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsChannelActualFlowEnabled));
            this.RaisePropertyChanged(nameof(EnableEvents));
        }
    }

    #endregion

    #region DigitalOutputs_DigitalOutputSet Flags

    public bool IsDO0Enabled_DigitalOutputSet
    {
        get
        {
            return DigitalOutputSet.HasFlag(DigitalOutputs.DO0);
        }
        set
        {
            if (value)
            {
                DigitalOutputSet |= DigitalOutputs.DO0;
            }
            else
            {
                DigitalOutputSet &= ~DigitalOutputs.DO0;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsDO0Enabled_DigitalOutputSet));
            this.RaisePropertyChanged(nameof(DigitalOutputSet));
        }
    }

    public bool IsDO1Enabled_DigitalOutputSet
    {
        get
        {
            return DigitalOutputSet.HasFlag(DigitalOutputs.DO1);
        }
        set
        {
            if (value)
            {
                DigitalOutputSet |= DigitalOutputs.DO1;
            }
            else
            {
                DigitalOutputSet &= ~DigitalOutputs.DO1;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsDO1Enabled_DigitalOutputSet));
            this.RaisePropertyChanged(nameof(DigitalOutputSet));
        }
    }

    #endregion

    #region DigitalOutputs_DigitalOutputClear Flags

    public bool IsDO0Enabled_DigitalOutputClear
    {
        get
        {
            return DigitalOutputClear.HasFlag(DigitalOutputs.DO0);
        }
        set
        {
            if (value)
            {
                DigitalOutputClear |= DigitalOutputs.DO0;
            }
            else
            {
                DigitalOutputClear &= ~DigitalOutputs.DO0;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsDO0Enabled_DigitalOutputClear));
            this.RaisePropertyChanged(nameof(DigitalOutputClear));
        }
    }

    public bool IsDO1Enabled_DigitalOutputClear
    {
        get
        {
            return DigitalOutputClear.HasFlag(DigitalOutputs.DO1);
        }
        set
        {
            if (value)
            {
                DigitalOutputClear |= DigitalOutputs.DO1;
            }
            else
            {
                DigitalOutputClear &= ~DigitalOutputs.DO1;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsDO1Enabled_DigitalOutputClear));
            this.RaisePropertyChanged(nameof(DigitalOutputClear));
        }
    }

    #endregion

    #region DigitalOutputs_DigitalOutputToggle Flags

    public bool IsDO0Enabled_DigitalOutputToggle
    {
        get
        {
            return DigitalOutputToggle.HasFlag(DigitalOutputs.DO0);
        }
        set
        {
            if (value)
            {
                DigitalOutputToggle |= DigitalOutputs.DO0;
            }
            else
            {
                DigitalOutputToggle &= ~DigitalOutputs.DO0;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsDO0Enabled_DigitalOutputToggle));
            this.RaisePropertyChanged(nameof(DigitalOutputToggle));
        }
    }

    public bool IsDO1Enabled_DigitalOutputToggle
    {
        get
        {
            return DigitalOutputToggle.HasFlag(DigitalOutputs.DO1);
        }
        set
        {
            if (value)
            {
                DigitalOutputToggle |= DigitalOutputs.DO1;
            }
            else
            {
                DigitalOutputToggle &= ~DigitalOutputs.DO1;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsDO1Enabled_DigitalOutputToggle));
            this.RaisePropertyChanged(nameof(DigitalOutputToggle));
        }
    }

    #endregion

    #region DigitalOutputs_DigitalOutputState Flags

    public bool IsDO0Enabled_DigitalOutputState
    {
        get
        {
            return DigitalOutputState.HasFlag(DigitalOutputs.DO0);
        }
        set
        {
            if (value)
            {
                DigitalOutputState |= DigitalOutputs.DO0;
            }
            else
            {
                DigitalOutputState &= ~DigitalOutputs.DO0;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsDO0Enabled_DigitalOutputState));
            this.RaisePropertyChanged(nameof(DigitalOutputState));
        }
    }

    public bool IsDO1Enabled_DigitalOutputState
    {
        get
        {
            return DigitalOutputState.HasFlag(DigitalOutputs.DO1);
        }
        set
        {
            if (value)
            {
                DigitalOutputState |= DigitalOutputs.DO1;
            }
            else
            {
                DigitalOutputState &= ~DigitalOutputs.DO1;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsDO1Enabled_DigitalOutputState));
            this.RaisePropertyChanged(nameof(DigitalOutputState));
        }
    }

    #endregion

    #region Valves_EnableValvePulse Flags

    public bool IsValve0Enabled_EnableValvePulse
    {
        get
        {
            return EnableValvePulse.HasFlag(Valves.Valve0);
        }
        set
        {
            if (value)
            {
                EnableValvePulse |= Valves.Valve0;
            }
            else
            {
                EnableValvePulse &= ~Valves.Valve0;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsValve0Enabled_EnableValvePulse));
            this.RaisePropertyChanged(nameof(EnableValvePulse));
        }
    }

    public bool IsValve1Enabled_EnableValvePulse
    {
        get
        {
            return EnableValvePulse.HasFlag(Valves.Valve1);
        }
        set
        {
            if (value)
            {
                EnableValvePulse |= Valves.Valve1;
            }
            else
            {
                EnableValvePulse &= ~Valves.Valve1;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsValve1Enabled_EnableValvePulse));
            this.RaisePropertyChanged(nameof(EnableValvePulse));
        }
    }

    public bool IsValve2Enabled_EnableValvePulse
    {
        get
        {
            return EnableValvePulse.HasFlag(Valves.Valve2);
        }
        set
        {
            if (value)
            {
                EnableValvePulse |= Valves.Valve2;
            }
            else
            {
                EnableValvePulse &= ~Valves.Valve2;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsValve2Enabled_EnableValvePulse));
            this.RaisePropertyChanged(nameof(EnableValvePulse));
        }
    }

    public bool IsValve3Enabled_EnableValvePulse
    {
        get
        {
            return EnableValvePulse.HasFlag(Valves.Valve3);
        }
        set
        {
            if (value)
            {
                EnableValvePulse |= Valves.Valve3;
            }
            else
            {
                EnableValvePulse &= ~Valves.Valve3;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsValve3Enabled_EnableValvePulse));
            this.RaisePropertyChanged(nameof(EnableValvePulse));
        }
    }

    public bool IsEndValve0Enabled_EnableValvePulse
    {
        get
        {
            return EnableValvePulse.HasFlag(Valves.EndValve0);
        }
        set
        {
            if (value)
            {
                EnableValvePulse |= Valves.EndValve0;
            }
            else
            {
                EnableValvePulse &= ~Valves.EndValve0;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsEndValve0Enabled_EnableValvePulse));
            this.RaisePropertyChanged(nameof(EnableValvePulse));
        }
    }

    public bool IsEndValve1Enabled_EnableValvePulse
    {
        get
        {
            return EnableValvePulse.HasFlag(Valves.EndValve1);
        }
        set
        {
            if (value)
            {
                EnableValvePulse |= Valves.EndValve1;
            }
            else
            {
                EnableValvePulse &= ~Valves.EndValve1;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsEndValve1Enabled_EnableValvePulse));
            this.RaisePropertyChanged(nameof(EnableValvePulse));
        }
    }

    public bool IsValveDummyEnabled_EnableValvePulse
    {
        get
        {
            return EnableValvePulse.HasFlag(Valves.ValveDummy);
        }
        set
        {
            if (value)
            {
                EnableValvePulse |= Valves.ValveDummy;
            }
            else
            {
                EnableValvePulse &= ~Valves.ValveDummy;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsValveDummyEnabled_EnableValvePulse));
            this.RaisePropertyChanged(nameof(EnableValvePulse));
        }
    }

    public bool IsCheckValve0Enabled_EnableValvePulse
    {
        get
        {
            return EnableValvePulse.HasFlag(Valves.CheckValve0);
        }
        set
        {
            if (value)
            {
                EnableValvePulse |= Valves.CheckValve0;
            }
            else
            {
                EnableValvePulse &= ~Valves.CheckValve0;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsCheckValve0Enabled_EnableValvePulse));
            this.RaisePropertyChanged(nameof(EnableValvePulse));
        }
    }

    public bool IsCheckValve1Enabled_EnableValvePulse
    {
        get
        {
            return EnableValvePulse.HasFlag(Valves.CheckValve1);
        }
        set
        {
            if (value)
            {
                EnableValvePulse |= Valves.CheckValve1;
            }
            else
            {
                EnableValvePulse &= ~Valves.CheckValve1;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsCheckValve1Enabled_EnableValvePulse));
            this.RaisePropertyChanged(nameof(EnableValvePulse));
        }
    }

    public bool IsCheckValve2Enabled_EnableValvePulse
    {
        get
        {
            return EnableValvePulse.HasFlag(Valves.CheckValve2);
        }
        set
        {
            if (value)
            {
                EnableValvePulse |= Valves.CheckValve2;
            }
            else
            {
                EnableValvePulse &= ~Valves.CheckValve2;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsCheckValve2Enabled_EnableValvePulse));
            this.RaisePropertyChanged(nameof(EnableValvePulse));
        }
    }

    public bool IsCheckValve3Enabled_EnableValvePulse
    {
        get
        {
            return EnableValvePulse.HasFlag(Valves.CheckValve3);
        }
        set
        {
            if (value)
            {
                EnableValvePulse |= Valves.CheckValve3;
            }
            else
            {
                EnableValvePulse &= ~Valves.CheckValve3;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsCheckValve3Enabled_EnableValvePulse));
            this.RaisePropertyChanged(nameof(EnableValvePulse));
        }
    }

    #endregion

    #region Valves_ValveSet Flags

    public bool IsValve0Enabled_ValveSet
    {
        get
        {
            return ValveSet.HasFlag(Valves.Valve0);
        }
        set
        {
            if (value)
            {
                ValveSet |= Valves.Valve0;
            }
            else
            {
                ValveSet &= ~Valves.Valve0;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsValve0Enabled_ValveSet));
            this.RaisePropertyChanged(nameof(ValveSet));
        }
    }

    public bool IsValve1Enabled_ValveSet
    {
        get
        {
            return ValveSet.HasFlag(Valves.Valve1);
        }
        set
        {
            if (value)
            {
                ValveSet |= Valves.Valve1;
            }
            else
            {
                ValveSet &= ~Valves.Valve1;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsValve1Enabled_ValveSet));
            this.RaisePropertyChanged(nameof(ValveSet));
        }
    }

    public bool IsValve2Enabled_ValveSet
    {
        get
        {
            return ValveSet.HasFlag(Valves.Valve2);
        }
        set
        {
            if (value)
            {
                ValveSet |= Valves.Valve2;
            }
            else
            {
                ValveSet &= ~Valves.Valve2;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsValve2Enabled_ValveSet));
            this.RaisePropertyChanged(nameof(ValveSet));
        }
    }

    public bool IsValve3Enabled_ValveSet
    {
        get
        {
            return ValveSet.HasFlag(Valves.Valve3);
        }
        set
        {
            if (value)
            {
                ValveSet |= Valves.Valve3;
            }
            else
            {
                ValveSet &= ~Valves.Valve3;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsValve3Enabled_ValveSet));
            this.RaisePropertyChanged(nameof(ValveSet));
        }
    }

    public bool IsEndValve0Enabled_ValveSet
    {
        get
        {
            return ValveSet.HasFlag(Valves.EndValve0);
        }
        set
        {
            if (value)
            {
                ValveSet |= Valves.EndValve0;
            }
            else
            {
                ValveSet &= ~Valves.EndValve0;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsEndValve0Enabled_ValveSet));
            this.RaisePropertyChanged(nameof(ValveSet));
        }
    }

    public bool IsEndValve1Enabled_ValveSet
    {
        get
        {
            return ValveSet.HasFlag(Valves.EndValve1);
        }
        set
        {
            if (value)
            {
                ValveSet |= Valves.EndValve1;
            }
            else
            {
                ValveSet &= ~Valves.EndValve1;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsEndValve1Enabled_ValveSet));
            this.RaisePropertyChanged(nameof(ValveSet));
        }
    }

    public bool IsValveDummyEnabled_ValveSet
    {
        get
        {
            return ValveSet.HasFlag(Valves.ValveDummy);
        }
        set
        {
            if (value)
            {
                ValveSet |= Valves.ValveDummy;
            }
            else
            {
                ValveSet &= ~Valves.ValveDummy;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsValveDummyEnabled_ValveSet));
            this.RaisePropertyChanged(nameof(ValveSet));
        }
    }

    public bool IsCheckValve0Enabled_ValveSet
    {
        get
        {
            return ValveSet.HasFlag(Valves.CheckValve0);
        }
        set
        {
            if (value)
            {
                ValveSet |= Valves.CheckValve0;
            }
            else
            {
                ValveSet &= ~Valves.CheckValve0;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsCheckValve0Enabled_ValveSet));
            this.RaisePropertyChanged(nameof(ValveSet));
        }
    }

    public bool IsCheckValve1Enabled_ValveSet
    {
        get
        {
            return ValveSet.HasFlag(Valves.CheckValve1);
        }
        set
        {
            if (value)
            {
                ValveSet |= Valves.CheckValve1;
            }
            else
            {
                ValveSet &= ~Valves.CheckValve1;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsCheckValve1Enabled_ValveSet));
            this.RaisePropertyChanged(nameof(ValveSet));
        }
    }

    public bool IsCheckValve2Enabled_ValveSet
    {
        get
        {
            return ValveSet.HasFlag(Valves.CheckValve2);
        }
        set
        {
            if (value)
            {
                ValveSet |= Valves.CheckValve2;
            }
            else
            {
                ValveSet &= ~Valves.CheckValve2;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsCheckValve2Enabled_ValveSet));
            this.RaisePropertyChanged(nameof(ValveSet));
        }
    }

    public bool IsCheckValve3Enabled_ValveSet
    {
        get
        {
            return ValveSet.HasFlag(Valves.CheckValve3);
        }
        set
        {
            if (value)
            {
                ValveSet |= Valves.CheckValve3;
            }
            else
            {
                ValveSet &= ~Valves.CheckValve3;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsCheckValve3Enabled_ValveSet));
            this.RaisePropertyChanged(nameof(ValveSet));
        }
    }

    #endregion

    #region Valves_ValveClear Flags

    public bool IsValve0Enabled_ValveClear
    {
        get
        {
            return ValveClear.HasFlag(Valves.Valve0);
        }
        set
        {
            if (value)
            {
                ValveClear |= Valves.Valve0;
            }
            else
            {
                ValveClear &= ~Valves.Valve0;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsValve0Enabled_ValveClear));
            this.RaisePropertyChanged(nameof(ValveClear));
        }
    }

    public bool IsValve1Enabled_ValveClear
    {
        get
        {
            return ValveClear.HasFlag(Valves.Valve1);
        }
        set
        {
            if (value)
            {
                ValveClear |= Valves.Valve1;
            }
            else
            {
                ValveClear &= ~Valves.Valve1;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsValve1Enabled_ValveClear));
            this.RaisePropertyChanged(nameof(ValveClear));
        }
    }

    public bool IsValve2Enabled_ValveClear
    {
        get
        {
            return ValveClear.HasFlag(Valves.Valve2);
        }
        set
        {
            if (value)
            {
                ValveClear |= Valves.Valve2;
            }
            else
            {
                ValveClear &= ~Valves.Valve2;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsValve2Enabled_ValveClear));
            this.RaisePropertyChanged(nameof(ValveClear));
        }
    }

    public bool IsValve3Enabled_ValveClear
    {
        get
        {
            return ValveClear.HasFlag(Valves.Valve3);
        }
        set
        {
            if (value)
            {
                ValveClear |= Valves.Valve3;
            }
            else
            {
                ValveClear &= ~Valves.Valve3;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsValve3Enabled_ValveClear));
            this.RaisePropertyChanged(nameof(ValveClear));
        }
    }

    public bool IsEndValve0Enabled_ValveClear
    {
        get
        {
            return ValveClear.HasFlag(Valves.EndValve0);
        }
        set
        {
            if (value)
            {
                ValveClear |= Valves.EndValve0;
            }
            else
            {
                ValveClear &= ~Valves.EndValve0;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsEndValve0Enabled_ValveClear));
            this.RaisePropertyChanged(nameof(ValveClear));
        }
    }

    public bool IsEndValve1Enabled_ValveClear
    {
        get
        {
            return ValveClear.HasFlag(Valves.EndValve1);
        }
        set
        {
            if (value)
            {
                ValveClear |= Valves.EndValve1;
            }
            else
            {
                ValveClear &= ~Valves.EndValve1;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsEndValve1Enabled_ValveClear));
            this.RaisePropertyChanged(nameof(ValveClear));
        }
    }

    public bool IsValveDummyEnabled_ValveClear
    {
        get
        {
            return ValveClear.HasFlag(Valves.ValveDummy);
        }
        set
        {
            if (value)
            {
                ValveClear |= Valves.ValveDummy;
            }
            else
            {
                ValveClear &= ~Valves.ValveDummy;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsValveDummyEnabled_ValveClear));
            this.RaisePropertyChanged(nameof(ValveClear));
        }
    }

    public bool IsCheckValve0Enabled_ValveClear
    {
        get
        {
            return ValveClear.HasFlag(Valves.CheckValve0);
        }
        set
        {
            if (value)
            {
                ValveClear |= Valves.CheckValve0;
            }
            else
            {
                ValveClear &= ~Valves.CheckValve0;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsCheckValve0Enabled_ValveClear));
            this.RaisePropertyChanged(nameof(ValveClear));
        }
    }

    public bool IsCheckValve1Enabled_ValveClear
    {
        get
        {
            return ValveClear.HasFlag(Valves.CheckValve1);
        }
        set
        {
            if (value)
            {
                ValveClear |= Valves.CheckValve1;
            }
            else
            {
                ValveClear &= ~Valves.CheckValve1;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsCheckValve1Enabled_ValveClear));
            this.RaisePropertyChanged(nameof(ValveClear));
        }
    }

    public bool IsCheckValve2Enabled_ValveClear
    {
        get
        {
            return ValveClear.HasFlag(Valves.CheckValve2);
        }
        set
        {
            if (value)
            {
                ValveClear |= Valves.CheckValve2;
            }
            else
            {
                ValveClear &= ~Valves.CheckValve2;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsCheckValve2Enabled_ValveClear));
            this.RaisePropertyChanged(nameof(ValveClear));
        }
    }

    public bool IsCheckValve3Enabled_ValveClear
    {
        get
        {
            return ValveClear.HasFlag(Valves.CheckValve3);
        }
        set
        {
            if (value)
            {
                ValveClear |= Valves.CheckValve3;
            }
            else
            {
                ValveClear &= ~Valves.CheckValve3;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsCheckValve3Enabled_ValveClear));
            this.RaisePropertyChanged(nameof(ValveClear));
        }
    }

    #endregion

    #region Valves_ValveToggle Flags

    public bool IsValve0Enabled_ValveToggle
    {
        get
        {
            return ValveToggle.HasFlag(Valves.Valve0);
        }
        set
        {
            if (value)
            {
                ValveToggle |= Valves.Valve0;
            }
            else
            {
                ValveToggle &= ~Valves.Valve0;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsValve0Enabled_ValveToggle));
            this.RaisePropertyChanged(nameof(ValveToggle));
        }
    }

    public bool IsValve1Enabled_ValveToggle
    {
        get
        {
            return ValveToggle.HasFlag(Valves.Valve1);
        }
        set
        {
            if (value)
            {
                ValveToggle |= Valves.Valve1;
            }
            else
            {
                ValveToggle &= ~Valves.Valve1;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsValve1Enabled_ValveToggle));
            this.RaisePropertyChanged(nameof(ValveToggle));
        }
    }

    public bool IsValve2Enabled_ValveToggle
    {
        get
        {
            return ValveToggle.HasFlag(Valves.Valve2);
        }
        set
        {
            if (value)
            {
                ValveToggle |= Valves.Valve2;
            }
            else
            {
                ValveToggle &= ~Valves.Valve2;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsValve2Enabled_ValveToggle));
            this.RaisePropertyChanged(nameof(ValveToggle));
        }
    }

    public bool IsValve3Enabled_ValveToggle
    {
        get
        {
            return ValveToggle.HasFlag(Valves.Valve3);
        }
        set
        {
            if (value)
            {
                ValveToggle |= Valves.Valve3;
            }
            else
            {
                ValveToggle &= ~Valves.Valve3;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsValve3Enabled_ValveToggle));
            this.RaisePropertyChanged(nameof(ValveToggle));
        }
    }

    public bool IsEndValve0Enabled_ValveToggle
    {
        get
        {
            return ValveToggle.HasFlag(Valves.EndValve0);
        }
        set
        {
            if (value)
            {
                ValveToggle |= Valves.EndValve0;
            }
            else
            {
                ValveToggle &= ~Valves.EndValve0;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsEndValve0Enabled_ValveToggle));
            this.RaisePropertyChanged(nameof(ValveToggle));
        }
    }

    public bool IsEndValve1Enabled_ValveToggle
    {
        get
        {
            return ValveToggle.HasFlag(Valves.EndValve1);
        }
        set
        {
            if (value)
            {
                ValveToggle |= Valves.EndValve1;
            }
            else
            {
                ValveToggle &= ~Valves.EndValve1;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsEndValve1Enabled_ValveToggle));
            this.RaisePropertyChanged(nameof(ValveToggle));
        }
    }

    public bool IsValveDummyEnabled_ValveToggle
    {
        get
        {
            return ValveToggle.HasFlag(Valves.ValveDummy);
        }
        set
        {
            if (value)
            {
                ValveToggle |= Valves.ValveDummy;
            }
            else
            {
                ValveToggle &= ~Valves.ValveDummy;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsValveDummyEnabled_ValveToggle));
            this.RaisePropertyChanged(nameof(ValveToggle));
        }
    }

    public bool IsCheckValve0Enabled_ValveToggle
    {
        get
        {
            return ValveToggle.HasFlag(Valves.CheckValve0);
        }
        set
        {
            if (value)
            {
                ValveToggle |= Valves.CheckValve0;
            }
            else
            {
                ValveToggle &= ~Valves.CheckValve0;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsCheckValve0Enabled_ValveToggle));
            this.RaisePropertyChanged(nameof(ValveToggle));
        }
    }

    public bool IsCheckValve1Enabled_ValveToggle
    {
        get
        {
            return ValveToggle.HasFlag(Valves.CheckValve1);
        }
        set
        {
            if (value)
            {
                ValveToggle |= Valves.CheckValve1;
            }
            else
            {
                ValveToggle &= ~Valves.CheckValve1;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsCheckValve1Enabled_ValveToggle));
            this.RaisePropertyChanged(nameof(ValveToggle));
        }
    }

    public bool IsCheckValve2Enabled_ValveToggle
    {
        get
        {
            return ValveToggle.HasFlag(Valves.CheckValve2);
        }
        set
        {
            if (value)
            {
                ValveToggle |= Valves.CheckValve2;
            }
            else
            {
                ValveToggle &= ~Valves.CheckValve2;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsCheckValve2Enabled_ValveToggle));
            this.RaisePropertyChanged(nameof(ValveToggle));
        }
    }

    public bool IsCheckValve3Enabled_ValveToggle
    {
        get
        {
            return ValveToggle.HasFlag(Valves.CheckValve3);
        }
        set
        {
            if (value)
            {
                ValveToggle |= Valves.CheckValve3;
            }
            else
            {
                ValveToggle &= ~Valves.CheckValve3;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsCheckValve3Enabled_ValveToggle));
            this.RaisePropertyChanged(nameof(ValveToggle));
        }
    }

    #endregion

    #region Valves_ValveState Flags

    public bool IsValve0Enabled_ValveState
    {
        get
        {
            return ValveState.HasFlag(Valves.Valve0);
        }
        set
        {
            if (value)
            {
                ValveState |= Valves.Valve0;
            }
            else
            {
                ValveState &= ~Valves.Valve0;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsValve0Enabled_ValveState));
            this.RaisePropertyChanged(nameof(ValveState));
        }
    }

    public bool IsValve1Enabled_ValveState
    {
        get
        {
            return ValveState.HasFlag(Valves.Valve1);
        }
        set
        {
            if (value)
            {
                ValveState |= Valves.Valve1;
            }
            else
            {
                ValveState &= ~Valves.Valve1;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsValve1Enabled_ValveState));
            this.RaisePropertyChanged(nameof(ValveState));
        }
    }

    public bool IsValve2Enabled_ValveState
    {
        get
        {
            return ValveState.HasFlag(Valves.Valve2);
        }
        set
        {
            if (value)
            {
                ValveState |= Valves.Valve2;
            }
            else
            {
                ValveState &= ~Valves.Valve2;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsValve2Enabled_ValveState));
            this.RaisePropertyChanged(nameof(ValveState));
        }
    }

    public bool IsValve3Enabled_ValveState
    {
        get
        {
            return ValveState.HasFlag(Valves.Valve3);
        }
        set
        {
            if (value)
            {
                ValveState |= Valves.Valve3;
            }
            else
            {
                ValveState &= ~Valves.Valve3;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsValve3Enabled_ValveState));
            this.RaisePropertyChanged(nameof(ValveState));
        }
    }

    public bool IsEndValve0Enabled_ValveState
    {
        get
        {
            return ValveState.HasFlag(Valves.EndValve0);
        }
        set
        {
            if (value)
            {
                ValveState |= Valves.EndValve0;
            }
            else
            {
                ValveState &= ~Valves.EndValve0;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsEndValve0Enabled_ValveState));
            this.RaisePropertyChanged(nameof(ValveState));
        }
    }

    public bool IsEndValve1Enabled_ValveState
    {
        get
        {
            return ValveState.HasFlag(Valves.EndValve1);
        }
        set
        {
            if (value)
            {
                ValveState |= Valves.EndValve1;
            }
            else
            {
                ValveState &= ~Valves.EndValve1;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsEndValve1Enabled_ValveState));
            this.RaisePropertyChanged(nameof(ValveState));
        }
    }

    public bool IsValveDummyEnabled_ValveState
    {
        get
        {
            return ValveState.HasFlag(Valves.ValveDummy);
        }
        set
        {
            if (value)
            {
                ValveState |= Valves.ValveDummy;
            }
            else
            {
                ValveState &= ~Valves.ValveDummy;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsValveDummyEnabled_ValveState));
            this.RaisePropertyChanged(nameof(ValveState));
        }
    }

    public bool IsCheckValve0Enabled_ValveState
    {
        get
        {
            return ValveState.HasFlag(Valves.CheckValve0);
        }
        set
        {
            if (value)
            {
                ValveState |= Valves.CheckValve0;
            }
            else
            {
                ValveState &= ~Valves.CheckValve0;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsCheckValve0Enabled_ValveState));
            this.RaisePropertyChanged(nameof(ValveState));
        }
    }

    public bool IsCheckValve1Enabled_ValveState
    {
        get
        {
            return ValveState.HasFlag(Valves.CheckValve1);
        }
        set
        {
            if (value)
            {
                ValveState |= Valves.CheckValve1;
            }
            else
            {
                ValveState &= ~Valves.CheckValve1;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsCheckValve1Enabled_ValveState));
            this.RaisePropertyChanged(nameof(ValveState));
        }
    }

    public bool IsCheckValve2Enabled_ValveState
    {
        get
        {
            return ValveState.HasFlag(Valves.CheckValve2);
        }
        set
        {
            if (value)
            {
                ValveState |= Valves.CheckValve2;
            }
            else
            {
                ValveState &= ~Valves.CheckValve2;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsCheckValve2Enabled_ValveState));
            this.RaisePropertyChanged(nameof(ValveState));
        }
    }

    public bool IsCheckValve3Enabled_ValveState
    {
        get
        {
            return ValveState.HasFlag(Valves.CheckValve3);
        }
        set
        {
            if (value)
            {
                ValveState |= Valves.CheckValve3;
            }
            else
            {
                ValveState &= ~Valves.CheckValve3;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsCheckValve3Enabled_ValveState));
            this.RaisePropertyChanged(nameof(ValveState));
        }
    }

    #endregion

    #region OdorValves_OdorValveState Flags

    public bool IsValve0Enabled_OdorValveState
    {
        get
        {
            return OdorValveState.HasFlag(OdorValves.Valve0);
        }
        set
        {
            if (value)
            {
                OdorValveState |= OdorValves.Valve0;
            }
            else
            {
                OdorValveState &= ~OdorValves.Valve0;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsValve0Enabled_OdorValveState));
            this.RaisePropertyChanged(nameof(OdorValveState));
        }
    }

    public bool IsValve1Enabled_OdorValveState
    {
        get
        {
            return OdorValveState.HasFlag(OdorValves.Valve1);
        }
        set
        {
            if (value)
            {
                OdorValveState |= OdorValves.Valve1;
            }
            else
            {
                OdorValveState &= ~OdorValves.Valve1;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsValve1Enabled_OdorValveState));
            this.RaisePropertyChanged(nameof(OdorValveState));
        }
    }

    public bool IsValve2Enabled_OdorValveState
    {
        get
        {
            return OdorValveState.HasFlag(OdorValves.Valve2);
        }
        set
        {
            if (value)
            {
                OdorValveState |= OdorValves.Valve2;
            }
            else
            {
                OdorValveState &= ~OdorValves.Valve2;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsValve2Enabled_OdorValveState));
            this.RaisePropertyChanged(nameof(OdorValveState));
        }
    }

    public bool IsValve3Enabled_OdorValveState
    {
        get
        {
            return OdorValveState.HasFlag(OdorValves.Valve3);
        }
        set
        {
            if (value)
            {
                OdorValveState |= OdorValves.Valve3;
            }
            else
            {
                OdorValveState &= ~OdorValves.Valve3;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsValve3Enabled_OdorValveState));
            this.RaisePropertyChanged(nameof(OdorValveState));
        }
    }

    #endregion

    #region EndValves_EndValveState Flags

    public bool IsEndValve0Enabled_EndValveState
    {
        get
        {
            return EndValveState.HasFlag(EndValves.EndValve0);
        }
        set
        {
            if (value)
            {
                EndValveState |= EndValves.EndValve0;
            }
            else
            {
                EndValveState &= ~EndValves.EndValve0;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsEndValve0Enabled_EndValveState));
            this.RaisePropertyChanged(nameof(EndValveState));
        }
    }

    public bool IsEndValve1Enabled_EndValveState
    {
        get
        {
            return EndValveState.HasFlag(EndValves.EndValve1);
        }
        set
        {
            if (value)
            {
                EndValveState |= EndValves.EndValve1;
            }
            else
            {
                EndValveState &= ~EndValves.EndValve1;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsEndValve1Enabled_EndValveState));
            this.RaisePropertyChanged(nameof(EndValveState));
        }
    }

    public bool IsValveDummyEnabled_EndValveState
    {
        get
        {
            return EndValveState.HasFlag(EndValves.ValveDummy);
        }
        set
        {
            if (value)
            {
                EndValveState |= EndValves.ValveDummy;
            }
            else
            {
                EndValveState &= ~EndValves.ValveDummy;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsValveDummyEnabled_EndValveState));
            this.RaisePropertyChanged(nameof(EndValveState));
        }
    }

    #endregion

    #region CheckValves_CheckValveState Flags

    public bool IsCheckValve0Enabled_CheckValveState
    {
        get
        {
            return CheckValveState.HasFlag(CheckValves.CheckValve0);
        }
        set
        {
            if (value)
            {
                CheckValveState |= CheckValves.CheckValve0;
            }
            else
            {
                CheckValveState &= ~CheckValves.CheckValve0;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsCheckValve0Enabled_CheckValveState));
            this.RaisePropertyChanged(nameof(CheckValveState));
        }
    }

    public bool IsCheckValve1Enabled_CheckValveState
    {
        get
        {
            return CheckValveState.HasFlag(CheckValves.CheckValve1);
        }
        set
        {
            if (value)
            {
                CheckValveState |= CheckValves.CheckValve1;
            }
            else
            {
                CheckValveState &= ~CheckValves.CheckValve1;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsCheckValve1Enabled_CheckValveState));
            this.RaisePropertyChanged(nameof(CheckValveState));
        }
    }

    public bool IsCheckValve2Enabled_CheckValveState
    {
        get
        {
            return CheckValveState.HasFlag(CheckValves.CheckValve2);
        }
        set
        {
            if (value)
            {
                CheckValveState |= CheckValves.CheckValve2;
            }
            else
            {
                CheckValveState &= ~CheckValves.CheckValve2;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsCheckValve2Enabled_CheckValveState));
            this.RaisePropertyChanged(nameof(CheckValveState));
        }
    }

    public bool IsCheckValve3Enabled_CheckValveState
    {
        get
        {
            return CheckValveState.HasFlag(CheckValves.CheckValve3);
        }
        set
        {
            if (value)
            {
                CheckValveState |= CheckValves.CheckValve3;
            }
            else
            {
                CheckValveState &= ~CheckValves.CheckValve3;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsCheckValve3Enabled_CheckValveState));
            this.RaisePropertyChanged(nameof(CheckValveState));
        }
    }

    #endregion

    #region CheckValves_EnableCheckValveSync Flags

    public bool IsCheckValve0Enabled_EnableCheckValveSync
    {
        get
        {
            return EnableCheckValveSync.HasFlag(CheckValves.CheckValve0);
        }
        set
        {
            if (value)
            {
                EnableCheckValveSync |= CheckValves.CheckValve0;
            }
            else
            {
                EnableCheckValveSync &= ~CheckValves.CheckValve0;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsCheckValve0Enabled_EnableCheckValveSync));
            this.RaisePropertyChanged(nameof(EnableCheckValveSync));
        }
    }

    public bool IsCheckValve1Enabled_EnableCheckValveSync
    {
        get
        {
            return EnableCheckValveSync.HasFlag(CheckValves.CheckValve1);
        }
        set
        {
            if (value)
            {
                EnableCheckValveSync |= CheckValves.CheckValve1;
            }
            else
            {
                EnableCheckValveSync &= ~CheckValves.CheckValve1;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsCheckValve1Enabled_EnableCheckValveSync));
            this.RaisePropertyChanged(nameof(EnableCheckValveSync));
        }
    }

    public bool IsCheckValve2Enabled_EnableCheckValveSync
    {
        get
        {
            return EnableCheckValveSync.HasFlag(CheckValves.CheckValve2);
        }
        set
        {
            if (value)
            {
                EnableCheckValveSync |= CheckValves.CheckValve2;
            }
            else
            {
                EnableCheckValveSync &= ~CheckValves.CheckValve2;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsCheckValve2Enabled_EnableCheckValveSync));
            this.RaisePropertyChanged(nameof(EnableCheckValveSync));
        }
    }

    public bool IsCheckValve3Enabled_EnableCheckValveSync
    {
        get
        {
            return EnableCheckValveSync.HasFlag(CheckValves.CheckValve3);
        }
        set
        {
            if (value)
            {
                EnableCheckValveSync |= CheckValves.CheckValve3;
            }
            else
            {
                EnableCheckValveSync &= ~CheckValves.CheckValve3;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsCheckValve3Enabled_EnableCheckValveSync));
            this.RaisePropertyChanged(nameof(EnableCheckValveSync));
        }
    }

    #endregion

    #region Application State

    [ObservableAsProperty] public bool IsLoadingPorts { get; }
    [ObservableAsProperty] public bool IsConnecting { get; }
    [ObservableAsProperty] public bool IsResetting { get; }
    [ObservableAsProperty] public bool IsSaving { get; }
    [ObservableAsProperty] public bool IsStartingFlow { get; }

    [Reactive] public bool ShowWriteMessages { get; set; }
    [Reactive] public ObservableCollection<string> HarpEvents { get; set; } = new ObservableCollection<string>();
    [Reactive] public ObservableCollection<string> SentMessages { get; set; } = new ObservableCollection<string>();

    public ReactiveCommand<Unit, Unit> ShowAboutCommand { get; private set; }
    public ReactiveCommand<Unit, Unit> ClearMessagesCommand { get; private set; }
    public ReactiveCommand<Unit, Unit> ShowMessagesCommand { get; private set; }
    public ReactiveCommand<Unit, Unit> ToggleFlowCommand { get; }
    public ReactiveCommand<bool, Unit> DO0SetClearCommand { get; }
    public ReactiveCommand<bool, Unit> DO1SetClearCommand { get; }

    [Reactive] public int Channel3MaxValue { get; set; }
    [Reactive] public bool RunningFlow { get; set; }

    [Reactive] public bool ShowChecksFields { get; set; }
    [Reactive] public bool ShowTemperatureValue { get; set; }
    [Reactive] public bool ShowTemperatureFields { get; set; }

    #endregion

    private Harp.Olfactometer.AsyncDevice? _device;
    private IObservable<string> _deviceEventsObservable;
    private IDisposable? _deviceEventsSubscription;
    private IObservable<long> _actualFlowObservable;

    public OlfactometerViewModel()
    {
        var assembly = typeof(OlfactometerViewModel).Assembly;
        var informationVersion = assembly.GetName().Version;
        if (informationVersion != null)
            AppVersion = $"v{informationVersion.Major}.{informationVersion.Minor}.{informationVersion.Build}";

        Ports = new ObservableCollection<string>();

        ClearMessagesCommand = ReactiveCommand.Create(() => { SentMessages.Clear(); });
        ShowMessagesCommand = ReactiveCommand.Create(() => { ShowWriteMessages = !ShowWriteMessages; });

        const int periodInMilliseconds = 100;
        _actualFlowObservable = Observable
            .Interval(TimeSpan.FromMilliseconds(periodInMilliseconds), Scheduler.Default)
            .TakeUntil(this.WhenAnyValue(x => x.EnableFlow).Where(x => x == EnableFlag.Disable));

        LoadDeviceInformation = ReactiveCommand.CreateFromObservable(LoadUsbInformation);
        LoadDeviceInformation.IsExecuting.ToPropertyEx(this, x => x.IsLoadingPorts);
        LoadDeviceInformation.ThrownExceptions.Subscribe(ex =>
            Console.WriteLine($"Error loading device information with exception: {ex.Message}"));
        //Log.Error(ex, "Error loading device information with exception: {Exception}", ex));

        // can connect if there is a selection and also if the new selection is different than the old one
        var canConnect = this.WhenAnyValue(x => x.SelectedPort)
            .Select(selectedPort => !string.IsNullOrEmpty(selectedPort));

        ShowAboutCommand = ReactiveCommand.CreateFromTask(async () =>
                await new About() { DataContext = new AboutViewModel() }.ShowDialog(
                    (Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow));

        ConnectAndGetBaseInfoCommand = ReactiveCommand.CreateFromTask(ConnectAndGetBaseInfo, canConnect);
        ConnectAndGetBaseInfoCommand.IsExecuting.ToPropertyEx(this, x => x.IsConnecting);
        ConnectAndGetBaseInfoCommand.ThrownExceptions.Subscribe(ex =>
            //Log.Error(ex, "Error connecting to device with error: {Exception}", ex));
            Console.WriteLine($"Error connecting to device with error: {ex}"));

        var canChangeConfig = this.WhenAnyValue(x => x.Connected).Select(connected => connected);
        // Handle Save and Reset
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

        ToggleFlowCommand = ReactiveCommand.CreateFromObservable(ToggleFlow, canChangeConfig);
        ToggleFlowCommand.IsExecuting.ToPropertyEx(this, x => x.IsStartingFlow);
        ToggleFlowCommand.ThrownExceptions.Subscribe(ex =>
            //Log.Error(ex, "Error starting protocol with error: {Exception}", ex));
            Console.WriteLine($"Error starting protocol with error: {ex}"));
        
        DO0SetClearCommand = ReactiveCommand.CreateFromObservable<bool, Unit>(DO0SetClear, canChangeConfig);
        DO0SetClearCommand.IsExecuting.ToPropertyEx(this, x => x.IsSaving);
        DO0SetClearCommand.ThrownExceptions.Subscribe(ex =>
            //Log.Error(ex, "Error setting/clearing DO0 with error: {Exception}", ex));
            Console.WriteLine($"Error setting/clearing DO0 with error: {ex}"));
        
        DO1SetClearCommand = ReactiveCommand.CreateFromObservable<bool, Unit>(DO1SetClear, canChangeConfig);
        DO1SetClearCommand.IsExecuting.ToPropertyEx(this, x => x.IsSaving);
        DO1SetClearCommand.ThrownExceptions.Subscribe(ex =>
            //Log.Error(ex, "Error setting/clearing DO1 with error: {Exception}", ex));
            Console.WriteLine($"Error setting/clearing DO1 with error: {ex}"));
        

        this.WhenAnyValue(x => x.HardwareVersion)
            .Subscribe(version =>
            {
                if (version == null)
                    return;

                // show certain fields depending on the hardware version
                ShowChecksFields = version.Major >= 2;
                ShowTemperatureValue = (version.Major, version.Minor) is (2, 0) or (1, 1);
                ShowTemperatureFields = (version.Major, version.Minor) is not ((2, 1) or (1, 0));
            });

        this.WhenAnyValue(x => x.Connected)
            .Subscribe(x => { ConnectButtonText = x ? "Disconnect" : "Connect"; });

        this.WhenAnyValue(x => x.EnableEvents)
            .Subscribe(x =>
            {
                IsFlowmeterEnabled = x.HasFlag(OlfactometerEvents.Flowmeter);
                IsDI0TriggerEnabled = x.HasFlag(OlfactometerEvents.DI0Trigger);
                IsChannelActualFlowEnabled = x.HasFlag(OlfactometerEvents.ChannelActualFlow);
            });

        // handle the events from the device
        // When Connected changes subscribe/unsubscribe the device events.
        this.WhenAnyValue(x => x.Connected)
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(isConnected =>
            {
                if (isConnected && _deviceEventsObservable != null)
                {
                    // Subscribe on the UI thread so that the HarpEvents collection can be updated safely.
                    _deviceEventsSubscription = _deviceEventsObservable
                        .ObserveOn(RxApp.MainThreadScheduler)
                        .Subscribe(
                            msg => HarpEvents.Add(msg.ToString()),
                            ex => Debug.WriteLine($"Error in device events: {ex}")
                        );
                }
                else
                {
                    // Dispose subscription and clear messages.
                    _deviceEventsSubscription?.Dispose();
                    _deviceEventsSubscription = null;
                }
            });

        this.WhenAnyValue(x => x.DigitalOutputSet)
            .Subscribe(x =>
            {
                IsDO0Enabled_DigitalOutputSet = x.HasFlag(DigitalOutputs.DO0);
                IsDO1Enabled_DigitalOutputSet = x.HasFlag(DigitalOutputs.DO1);
            });

        this.WhenAnyValue(x => x.DigitalOutputClear)
            .Subscribe(x =>
            {
                IsDO0Enabled_DigitalOutputClear = x.HasFlag(DigitalOutputs.DO0);
                IsDO1Enabled_DigitalOutputClear = x.HasFlag(DigitalOutputs.DO1);
            });

        this.WhenAnyValue(x => x.DigitalOutputToggle)
            .Subscribe(x =>
            {
                IsDO0Enabled_DigitalOutputToggle = x.HasFlag(DigitalOutputs.DO0);
                IsDO1Enabled_DigitalOutputToggle = x.HasFlag(DigitalOutputs.DO1);
            });

        this.WhenAnyValue(x => x.DigitalOutputState)
            .Subscribe(x =>
            {
                IsDO0Enabled_DigitalOutputState = x.HasFlag(DigitalOutputs.DO0);
                IsDO1Enabled_DigitalOutputState = x.HasFlag(DigitalOutputs.DO1);
            });

        this.WhenAnyValue(x => x.EnableValvePulse)
            .Subscribe(async x =>
            {
                IsValve0Enabled_EnableValvePulse = x.HasFlag(Valves.Valve0);
                IsValve1Enabled_EnableValvePulse = x.HasFlag(Valves.Valve1);
                IsValve2Enabled_EnableValvePulse = x.HasFlag(Valves.Valve2);
                IsValve3Enabled_EnableValvePulse = x.HasFlag(Valves.Valve3);
                IsEndValve0Enabled_EnableValvePulse = x.HasFlag(Valves.EndValve0);
                IsEndValve1Enabled_EnableValvePulse = x.HasFlag(Valves.EndValve1);
                IsValveDummyEnabled_EnableValvePulse = x.HasFlag(Valves.ValveDummy);
                IsCheckValve0Enabled_EnableValvePulse = x.HasFlag(Valves.CheckValve0);
                IsCheckValve1Enabled_EnableValvePulse = x.HasFlag(Valves.CheckValve1);
                IsCheckValve2Enabled_EnableValvePulse = x.HasFlag(Valves.CheckValve2);
                IsCheckValve3Enabled_EnableValvePulse = x.HasFlag(Valves.CheckValve3);

                if (_device != null)
                {
                    await _device.WriteEnableValvePulseAsync(EnableValvePulse);
                }
            });

        this.WhenAnyValue(x => x.ValveSet)
            .Subscribe(x =>
            {
                IsValve0Enabled_ValveSet = x.HasFlag(Valves.Valve0);
                IsValve1Enabled_ValveSet = x.HasFlag(Valves.Valve1);
                IsValve2Enabled_ValveSet = x.HasFlag(Valves.Valve2);
                IsValve3Enabled_ValveSet = x.HasFlag(Valves.Valve3);
                IsEndValve0Enabled_ValveSet = x.HasFlag(Valves.EndValve0);
                IsEndValve1Enabled_ValveSet = x.HasFlag(Valves.EndValve1);
                IsValveDummyEnabled_ValveSet = x.HasFlag(Valves.ValveDummy);
                IsCheckValve0Enabled_ValveSet = x.HasFlag(Valves.CheckValve0);
                IsCheckValve1Enabled_ValveSet = x.HasFlag(Valves.CheckValve1);
                IsCheckValve2Enabled_ValveSet = x.HasFlag(Valves.CheckValve2);
                IsCheckValve3Enabled_ValveSet = x.HasFlag(Valves.CheckValve3);
            });

        this.WhenAnyValue(x => x.ValveClear)
            .Subscribe(x =>
            {
                IsValve0Enabled_ValveClear = x.HasFlag(Valves.Valve0);
                IsValve1Enabled_ValveClear = x.HasFlag(Valves.Valve1);
                IsValve2Enabled_ValveClear = x.HasFlag(Valves.Valve2);
                IsValve3Enabled_ValveClear = x.HasFlag(Valves.Valve3);
                IsEndValve0Enabled_ValveClear = x.HasFlag(Valves.EndValve0);
                IsEndValve1Enabled_ValveClear = x.HasFlag(Valves.EndValve1);
                IsValveDummyEnabled_ValveClear = x.HasFlag(Valves.ValveDummy);
                IsCheckValve0Enabled_ValveClear = x.HasFlag(Valves.CheckValve0);
                IsCheckValve1Enabled_ValveClear = x.HasFlag(Valves.CheckValve1);
                IsCheckValve2Enabled_ValveClear = x.HasFlag(Valves.CheckValve2);
                IsCheckValve3Enabled_ValveClear = x.HasFlag(Valves.CheckValve3);
            });

        this.WhenAnyValue(x => x.ValveToggle)
            .Subscribe(x =>
            {
                IsValve0Enabled_ValveToggle = x.HasFlag(Valves.Valve0);
                IsValve1Enabled_ValveToggle = x.HasFlag(Valves.Valve1);
                IsValve2Enabled_ValveToggle = x.HasFlag(Valves.Valve2);
                IsValve3Enabled_ValveToggle = x.HasFlag(Valves.Valve3);
                IsEndValve0Enabled_ValveToggle = x.HasFlag(Valves.EndValve0);
                IsEndValve1Enabled_ValveToggle = x.HasFlag(Valves.EndValve1);
                IsValveDummyEnabled_ValveToggle = x.HasFlag(Valves.ValveDummy);
                IsCheckValve0Enabled_ValveToggle = x.HasFlag(Valves.CheckValve0);
                IsCheckValve1Enabled_ValveToggle = x.HasFlag(Valves.CheckValve1);
                IsCheckValve2Enabled_ValveToggle = x.HasFlag(Valves.CheckValve2);
                IsCheckValve3Enabled_ValveToggle = x.HasFlag(Valves.CheckValve3);
            });

        this.WhenAnyValue(x => x.ValveState)
            .Subscribe(async x =>
            {
                IsValve0Enabled_ValveState = x.HasFlag(Valves.Valve0);
                IsValve1Enabled_ValveState = x.HasFlag(Valves.Valve1);
                IsValve2Enabled_ValveState = x.HasFlag(Valves.Valve2);
                IsValve3Enabled_ValveState = x.HasFlag(Valves.Valve3);
                IsEndValve0Enabled_ValveState = x.HasFlag(Valves.EndValve0);
                IsEndValve1Enabled_ValveState = x.HasFlag(Valves.EndValve1);
                IsValveDummyEnabled_ValveState = x.HasFlag(Valves.ValveDummy);
                IsCheckValve0Enabled_ValveState = x.HasFlag(Valves.CheckValve0);
                IsCheckValve1Enabled_ValveState = x.HasFlag(Valves.CheckValve1);
                IsCheckValve2Enabled_ValveState = x.HasFlag(Valves.CheckValve2);
                IsCheckValve3Enabled_ValveState = x.HasFlag(Valves.CheckValve3);
            });

        this.WhenAnyValue(x => x.OdorValveState)
            .Subscribe(async x =>
            {
                IsValve0Enabled_OdorValveState = x.HasFlag(OdorValves.Valve0);
                IsValve1Enabled_OdorValveState = x.HasFlag(OdorValves.Valve1);
                IsValve2Enabled_OdorValveState = x.HasFlag(OdorValves.Valve2);
                IsValve3Enabled_OdorValveState = x.HasFlag(OdorValves.Valve3);

                if (_device != null)
                {
                    await _device.WriteOdorValveStateAsync(OdorValveState);
                }
            });

        this.WhenAnyValue(x => x.EndValveState)
            .Subscribe(async x =>
            {
                IsEndValve0Enabled_EndValveState = x.HasFlag(EndValves.EndValve0);
                IsEndValve1Enabled_EndValveState = x.HasFlag(EndValves.EndValve1);
                IsValveDummyEnabled_EndValveState = x.HasFlag(EndValves.ValveDummy);

                if (_device != null)
                {
                    await _device.WriteEndValveStateAsync(EndValveState);
                }
            });

        this.WhenAnyValue(x => x.CheckValveState)
            .Subscribe(async x =>
            {
                IsCheckValve0Enabled_CheckValveState = x.HasFlag(CheckValves.CheckValve0);
                IsCheckValve1Enabled_CheckValveState = x.HasFlag(CheckValves.CheckValve1);
                IsCheckValve2Enabled_CheckValveState = x.HasFlag(CheckValves.CheckValve2);
                IsCheckValve3Enabled_CheckValveState = x.HasFlag(CheckValves.CheckValve3);

                if (_device != null)
                {
                    await _device.WriteCheckValveStateAsync(CheckValveState);
                }
            });

        this.WhenAnyValue(x => x.EnableCheckValveSync)
            .Subscribe(async x =>
            {
                IsCheckValve0Enabled_EnableCheckValveSync = x.HasFlag(CheckValves.CheckValve0);
                IsCheckValve1Enabled_EnableCheckValveSync = x.HasFlag(CheckValves.CheckValve1);
                IsCheckValve2Enabled_EnableCheckValveSync = x.HasFlag(CheckValves.CheckValve2);
                IsCheckValve3Enabled_EnableCheckValveSync = x.HasFlag(CheckValves.CheckValve3);

                if (_device != null)
                {
                    await _device.WriteEnableCheckValveSyncAsync(EnableCheckValveSync);
                }
            });

        this.WhenAnyValue(x => x.EnableFlow)
            .Subscribe(x => RunningFlow = x == EnableFlag.Enable);

        this.WhenAnyValue(x => x.Channel0TargetFlow, x => x.Channel1TargetFlow, x => x.Channel2TargetFlow,
                    x => x.Channel3TargetFlow, x => x.Channel4TargetFlow)
                .Throttle(TimeSpan.FromMilliseconds(200))
                .Subscribe(async x =>
                {
                    if (_device != null)
                    {
                        // This could also be 5 separate calls to WriteChannelXTargetFlowAsync
                        await _device.WriteChannelsTargetFlowAsync(new ChannelsTargetFlowPayload(
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

        this.WhenAnyValue(x => x.Valve0PulseDuration, x => x.Valve1PulseDuration, x => x.Valve2PulseDuration, x => x.Valve3PulseDuration)
            .Throttle(TimeSpan.FromMilliseconds(200))
            .Subscribe(async x =>
            {
                if (_device != null)
                {
                    // This could also be 4 separate calls to WriteValveXPulseDurationAsync
                    await _device.WriteValve0PulseDurationAsync(Valve0PulseDuration);
                    await _device.WriteValve1PulseDurationAsync(Valve1PulseDuration);
                    await _device.WriteValve2PulseDurationAsync(Valve2PulseDuration);
                    await _device.WriteValve3PulseDurationAsync(Valve3PulseDuration);
                }
            });

        this.WhenAnyValue(x => x.EndValve0PulseDuration, x => x.EndValve1PulseDuration)
            .Throttle(TimeSpan.FromMilliseconds(200))
            .Subscribe(async x =>
            {
                if (_device != null)
                {
                    // This could also be 3 separate calls to WriteEndValveXPulseDurationAsync
                    await _device.WriteEndValve0PulseDurationAsync(EndValve0PulseDuration);
                    await _device.WriteEndValve1PulseDurationAsync(EndValve1PulseDuration);
                }
            });

        this.WhenAnyValue(x => x.CheckValve0DelayPulseDuration, x => x.CheckValve1DelayPulseDuration, x => x.CheckValve2DelayPulseDuration, x => x.CheckValve3DelayPulseDuration)
            .Throttle(TimeSpan.FromMilliseconds(200))
            .Subscribe(async x =>
            {
                if (_device != null)
                {
                    // This could also be 4 separate calls to WriteCheckValveXDelayPulseDurationAsync
                    await _device.WriteCheckValve0DelayPulseDurationAsync(CheckValve0DelayPulseDuration);
                    await _device.WriteCheckValve1DelayPulseDurationAsync(CheckValve1DelayPulseDuration);
                    await _device.WriteCheckValve2DelayPulseDurationAsync(CheckValve2DelayPulseDuration);
                    await _device.WriteCheckValve3DelayPulseDurationAsync(CheckValve3DelayPulseDuration);
                }
            });

        this.WhenAnyValue(x => x.Channel3Range).Subscribe(async x =>
        {
            if (_device != null)
                await _device.WriteChannel3RangeAsync(Channel3Range);

            // update Channel3MaxValue
            Channel3MaxValue = Channel3Range == Channel3RangeConfig.FlowRate100 ? 100 : 1000;
        });

        // force initial population of currently connected ports
        LoadUsbInformation();
    }

    private IObservable<Unit> DO0SetClear(bool arg)
    {
        return Observable.StartAsync(async () =>
        {
            if (_device == null)
                throw new Exception("Device not connected");

            IsDO0Enabled_DigitalOutputSet = arg;
            IsDO0Enabled_DigitalOutputClear = !arg;

            await WriteAndLogAsync(
                value => _device.WriteDigitalOutputSetAsync(value),
                DigitalOutputSet,
                "DigitalOutputSet");

            await WriteAndLogAsync(
                value => _device.WriteDigitalOutputClearAsync(value),
                DigitalOutputClear,
                "DigitalOutputClear");
        });
    }

    private IObservable<Unit> DO1SetClear(bool arg)
    {
        return Observable.StartAsync(async () =>
        {
            if (_device == null)
                throw new Exception("Device not connected");

            IsDO1Enabled_DigitalOutputSet = arg;
            IsDO1Enabled_DigitalOutputClear = !arg;

            await WriteAndLogAsync(
                value => _device.WriteDigitalOutputSetAsync(value),
                DigitalOutputSet,
                "DigitalOutputSet");

            await WriteAndLogAsync(
                value => _device.WriteDigitalOutputClearAsync(value),
                DigitalOutputClear,
                "DigitalOutputClear");
        });
    }

    private IObservable<Unit> LoadUsbInformation()
    {
        return Observable.Start(() =>
        {
            var devices = SerialPort.GetPortNames();

            if (OperatingSystem.IsMacOS())
                // except with Bluetooth in the name
                Ports = new ObservableCollection<string>(devices.Where(d => d.Contains("cu.")).Except(devices.Where(d => d.Contains("Bluetooth"))));
            else
                Ports = new ObservableCollection<string>(devices);

            Console.WriteLine("Loaded USB information");
            //Log.Information("Loaded USB information");
        });
    }

    private async Task ConnectAndGetBaseInfo()
    {
        if (string.IsNullOrEmpty(SelectedPort))
            throw new Exception("invalid parameter");

        if (Connected)
        {
            _device?.Dispose();
            _device = null;
            Connected = false;
            SentMessages.Clear();
            return;
        }

        try
        {
            _device = await Harp.Olfactometer.Device.CreateAsync(SelectedPort);
        }
        catch (OperationCanceledException ex)
        {
            Console.WriteLine($"Error connecting to device with error: {ex}");
            //Log.Error(ex, "Error connecting to device with error: {Exception}", ex);
            var messageBoxStandardWindow = MessageBoxManager
                .GetMessageBoxStandard("Unexpected device found",
                    "Timeout when trying to connect to a device. Most likely not an Harp device.",
                    icon: Icon.Error);
            await messageBoxStandardWindow.ShowAsync();
            _device?.Dispose();
            _device = null;
            return;

        }
        catch (HarpException ex)
        {
            Console.WriteLine($"Error connecting to device with error: {ex}");
            //Log.Error(ex, "Error connecting to device with error: {Exception}", ex);

            var messageBoxStandardWindow = MessageBoxManager
                .GetMessageBoxStandard("Unexpected device found",
                    ex.Message,
                    icon: Icon.Error);
            await messageBoxStandardWindow.ShowAsync();

            _device?.Dispose();
            _device = null;
            return;
        }
        catch (UnauthorizedAccessException ex)
        {
            Console.WriteLine($"COM port still in use and most likely not the expected Harp device");
            var messageBoxStandardWindow = MessageBoxManager
                .GetMessageBoxStandard("Unexpected device found",
                    $"COM port still in use and most likely not the expected Harp device.{Environment.NewLine}Specific error: {ex.Message}",
                    icon: Icon.Error);
            await messageBoxStandardWindow.ShowAsync();

            _device?.Dispose();
            _device = null;
            return;
        }

        // Clear the sent messages list
        SentMessages.Clear();

        //Log.Information("Attempting connection to port \'{SelectedPort}\'", SelectedPort);
        Console.WriteLine($"Attempting connection to port \'{SelectedPort}\'");

        DeviceID = await _device.ReadWhoAmIAsync();
        DeviceName = await _device.ReadDeviceNameAsync();
        HardwareVersion = await _device.ReadHardwareVersionAsync();
        FirmwareVersion = await _device.ReadFirmwareVersionAsync();
        try
        {
            // some devices may not have a serial number
            SerialNumber = await _device.ReadSerialNumberAsync();
        }
        catch (HarpException)
        {
            // Device does not have a serial number, simply continue by ignoring the exception
        }

        // show warning if the device's firware.Major version is below 2 and exit
        if (FirmwareVersion != null && FirmwareVersion.Major < 2)
        {
            var messageBoxStandardWindow = MessageBoxManager
                .GetMessageBoxStandard("Unsupported firmware version",
                    $"The connected Olfactometer has an unsupported firmware version (< 2). {Environment.NewLine}Please update the firmware or download version 1.1.0 or below of this application.",
                    icon: Icon.Error);
            await messageBoxStandardWindow.ShowAsync();
            _device.Dispose();
            _device = null;
            return;
        }

        // Read all registers
        EnableFlow = await _device.ReadEnableFlowAsync();
        Flowmeter = await _device.ReadFlowmeterAsync();
        DI0State = await _device.ReadDI0StateAsync();
        Channel0UserCalibration = await _device.ReadChannel0UserCalibrationAsync();
        Channel1UserCalibration = await _device.ReadChannel1UserCalibrationAsync();
        Channel2UserCalibration = await _device.ReadChannel2UserCalibrationAsync();
        Channel3UserCalibration = await _device.ReadChannel3UserCalibrationAsync();
        Channel4UserCalibration = await _device.ReadChannel4UserCalibrationAsync();
        Channel3UserCalibrationAux = await _device.ReadChannel3UserCalibrationAuxAsync();
        EnableUserCalibration = await _device.ReadEnableUserCalibrationAsync();
        Channel0TargetFlow = await _device.ReadChannel0TargetFlowAsync();
        Channel1TargetFlow = await _device.ReadChannel1TargetFlowAsync();
        Channel2TargetFlow = await _device.ReadChannel2TargetFlowAsync();
        Channel3TargetFlow = await _device.ReadChannel3TargetFlowAsync();
        Channel4TargetFlow = await _device.ReadChannel4TargetFlowAsync();
        ChannelsTargetFlow = await _device.ReadChannelsTargetFlowAsync();
        Channel0ActualFlow = await _device.ReadChannel0ActualFlowAsync();
        Channel1ActualFlow = await _device.ReadChannel1ActualFlowAsync();
        Channel2ActualFlow = await _device.ReadChannel2ActualFlowAsync();
        Channel3ActualFlow = await _device.ReadChannel3ActualFlowAsync();
        Channel4ActualFlow = await _device.ReadChannel4ActualFlowAsync();
        Channel0DutyCycle = await _device.ReadChannel0DutyCycleAsync();
        Channel1DutyCycle = await _device.ReadChannel1DutyCycleAsync();
        Channel2DutyCycle = await _device.ReadChannel2DutyCycleAsync();
        Channel3DutyCycle = await _device.ReadChannel3DutyCycleAsync();
        Channel4DutyCycle = await _device.ReadChannel4DutyCycleAsync();
        DigitalOutputSet = await _device.ReadDigitalOutputSetAsync();
        DigitalOutputClear = await _device.ReadDigitalOutputClearAsync();
        DigitalOutputToggle = await _device.ReadDigitalOutputToggleAsync();
        DigitalOutputState = await _device.ReadDigitalOutputStateAsync();
        EnableValvePulse = await _device.ReadEnableValvePulseAsync();
        ValveSet = await _device.ReadValveSetAsync();
        ValveClear = await _device.ReadValveClearAsync();
        ValveToggle = await _device.ReadValveToggleAsync();
        ValveState = await _device.ReadValveStateAsync();
        OdorValveState = await _device.ReadOdorValveStateAsync();
        EndValveState = await _device.ReadEndValveStateAsync();
        CheckValveState = await _device.ReadCheckValveStateAsync();
        Valve0PulseDuration = await _device.ReadValve0PulseDurationAsync();
        Valve1PulseDuration = await _device.ReadValve1PulseDurationAsync();
        Valve2PulseDuration = await _device.ReadValve2PulseDurationAsync();
        Valve3PulseDuration = await _device.ReadValve3PulseDurationAsync();
        CheckValve0DelayPulseDuration = await _device.ReadCheckValve0DelayPulseDurationAsync();
        CheckValve1DelayPulseDuration = await _device.ReadCheckValve1DelayPulseDurationAsync();
        CheckValve2DelayPulseDuration = await _device.ReadCheckValve2DelayPulseDurationAsync();
        CheckValve3DelayPulseDuration = await _device.ReadCheckValve3DelayPulseDurationAsync();
        EndValve0PulseDuration = await _device.ReadEndValve0PulseDurationAsync();
        EndValve1PulseDuration = await _device.ReadEndValve1PulseDurationAsync();
        DO0Sync = await _device.ReadDO0SyncAsync();
        DO1Sync = await _device.ReadDO1SyncAsync();
        DI0Trigger = await _device.ReadDI0TriggerAsync();
        MimicValve0 = await _device.ReadMimicValve0Async();
        MimicValve1 = await _device.ReadMimicValve1Async();
        MimicValve2 = await _device.ReadMimicValve2Async();
        MimicValve3 = await _device.ReadMimicValve3Async();
        MimicCheckValve0 = await _device.ReadMimicCheckValve0Async();
        MimicCheckValve1 = await _device.ReadMimicCheckValve1Async();
        MimicCheckValve2 = await _device.ReadMimicCheckValve2Async();
        MimicCheckValve3 = await _device.ReadMimicCheckValve3Async();
        MimicEndValve0 = await _device.ReadMimicEndValve0Async();
        MimicEndValve1 = await _device.ReadMimicEndValve1Async();
        EnableValveExternalControl = await _device.ReadEnableValveExternalControlAsync();
        Channel3Range = await _device.ReadChannel3RangeAsync();
        EnableCheckValveSync = await _device.ReadEnableCheckValveSyncAsync();
        TemperatureValue = await _device.ReadTemperatureValueAsync();
        EnableTemperatureCalibration = await _device.ReadEnableTemperatureCalibrationAsync();
        TemperatureCalibrationValue = await _device.ReadTemperatureCalibrationValueAsync();
        EnableEvents = await _device.ReadEnableEventsAsync();

        // generate observable for the _deviceSync
        _deviceEventsObservable = GenerateEventMessages();

        Connected = true;

        //Log.Information("Connected to device");
        Console.WriteLine("Connected to device");
    }

    private IObservable<Unit> ToggleFlow()
    {
        return Observable.StartAsync(async () =>
        {
            if (_device == null)
                throw new Exception("Olfactometer is not connected");
            await _device.WriteEnableFlowAsync(EnableFlow == EnableFlag.Enable
                ? EnableFlag.Disable
                : EnableFlag.Enable);
            // update EnableFlow to the actual value
            EnableFlow = await _device.ReadEnableFlowAsync();

            if (EnableFlow == EnableFlag.Enable)
            {
                _actualFlowObservable.Subscribe(async _ =>
                {
                    if (_device != null)
                    {
                        Channel0ActualFlow = await _device.ReadChannel0ActualFlowAsync();
                        Channel1ActualFlow = await _device.ReadChannel1ActualFlowAsync();
                        Channel2ActualFlow = await _device.ReadChannel2ActualFlowAsync();
                        Channel3ActualFlow = await _device.ReadChannel3ActualFlowAsync();
                        Channel4ActualFlow = await _device.ReadChannel4ActualFlowAsync();
                    }
                });
            }
        });
    }

    public IObservable<string> GenerateEventMessages()
    {
        return Observable.Create<string>(async (observer, cancellationToken) =>
        {
            // Loop until cancellation is requested or the device is no longer available.
            while (!cancellationToken.IsCancellationRequested && _device != null)
            {
                // Capture local reference and check for null.
                var device = _device;
                if (device == null)
                {
                    observer.OnCompleted();
                    break;
                }

                try
                {
                    // Check if Flowmeter event is enabled
                    if (IsFlowmeterEnabled)
                    {
                        var result = await device.ReadFlowmeterAsync(cancellationToken);
                        // Update the corresponding property with the result
                        Flowmeter = result;
                        observer.OnNext($"Flowmeter: {result}");
                    }

                    // Check if DI0Trigger event is enabled
                    if (IsDI0TriggerEnabled)
                    {
                        // Update the corresponding property with the result
                        DI0State = await device.ReadDI0StateAsync(cancellationToken);
                        observer.OnNext($"DI0State: {DI0State}");
                    }

                    // Check if ChannelActualFlow event is enabled
                    if (IsChannelActualFlowEnabled)
                    {
                        Channel0ActualFlow = await device.ReadChannel0ActualFlowAsync(cancellationToken);
                        observer.OnNext($"Channel0ActualFlow: {Channel0ActualFlow}");
                        Channel1ActualFlow = await device.ReadChannel1ActualFlowAsync(cancellationToken);
                        observer.OnNext($"Channel1ActualFlow: {Channel1ActualFlow}");
                        Channel2ActualFlow = await device.ReadChannel2ActualFlowAsync(cancellationToken);
                        observer.OnNext($"Channel2ActualFlow: {Channel2ActualFlow}");
                        Channel3ActualFlow = await device.ReadChannel3ActualFlowAsync(cancellationToken);
                        observer.OnNext($"Channel3ActualFlow: {Channel3ActualFlow}");
                        Channel4ActualFlow = await device.ReadChannel4ActualFlowAsync(cancellationToken);
                        observer.OnNext($"Channel4ActualFlow: {Channel4ActualFlow}");
                    }
                    
                    DigitalOutputState = await _device.ReadDigitalOutputStateAsync(cancellationToken);
                    observer.OnNext($"DigitalOutputState: {DigitalOutputState}");

                    // Wait a short while before polling again. Adjust delay as necessary.
                    await Task.Delay(TimeSpan.FromMilliseconds(10), cancellationToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    observer.OnError(ex);
                    break;
                }
            }
            observer.OnCompleted();
            return Disposable.Empty;
        });
    }

    private IObservable<Unit> SaveConfiguration(bool savePermanently)
    {
        return Observable.StartAsync(async () =>
        {
            if (_device == null)
                throw new Exception("You need to connect to the device first");

            await WriteAndLogAsync(
                value => _device.WriteEnableFlowAsync(value),
                EnableFlow,
                "EnableFlow");
            await WriteAndLogAsync(
                value => _device.WriteChannel0UserCalibrationAsync(value),
                Channel0UserCalibration,
                "Channel0UserCalibration");
            await WriteAndLogAsync(
                value => _device.WriteChannel1UserCalibrationAsync(value),
                Channel1UserCalibration,
                "Channel1UserCalibration");
            await WriteAndLogAsync(
                value => _device.WriteChannel2UserCalibrationAsync(value),
                Channel2UserCalibration,
                "Channel2UserCalibration");
            await WriteAndLogAsync(
                value => _device.WriteChannel3UserCalibrationAsync(value),
                Channel3UserCalibration,
                "Channel3UserCalibration");
            await WriteAndLogAsync(
                value => _device.WriteChannel4UserCalibrationAsync(value),
                Channel4UserCalibration,
                "Channel4UserCalibration");
            await WriteAndLogAsync(
                value => _device.WriteChannel3UserCalibrationAuxAsync(value),
                Channel3UserCalibrationAux,
                "Channel3UserCalibrationAux");
            await WriteAndLogAsync(
                value => _device.WriteEnableUserCalibrationAsync(value),
                EnableUserCalibration,
                "EnableUserCalibration");
            await WriteAndLogAsync(
                value => _device.WriteChannel0TargetFlowAsync(value),
                Channel0TargetFlow,
                "Channel0TargetFlow");
            await WriteAndLogAsync(
                value => _device.WriteChannel1TargetFlowAsync(value),
                Channel1TargetFlow,
                "Channel1TargetFlow");
            await WriteAndLogAsync(
                value => _device.WriteChannel2TargetFlowAsync(value),
                Channel2TargetFlow,
                "Channel2TargetFlow");
            await WriteAndLogAsync(
                value => _device.WriteChannel3TargetFlowAsync(value),
                Channel3TargetFlow,
                "Channel3TargetFlow");
            await WriteAndLogAsync(
                value => _device.WriteChannel4TargetFlowAsync(value),
                Channel4TargetFlow,
                "Channel4TargetFlow");
            await WriteAndLogAsync(
                value => _device.WriteChannelsTargetFlowAsync(value),
                ChannelsTargetFlow,
                "ChannelsTargetFlow");
            await WriteAndLogAsync(
                value => _device.WriteChannel0DutyCycleAsync(value),
                Channel0DutyCycle,
                "Channel0DutyCycle");
            await WriteAndLogAsync(
                value => _device.WriteChannel1DutyCycleAsync(value),
                Channel1DutyCycle,
                "Channel1DutyCycle");
            await WriteAndLogAsync(
                value => _device.WriteChannel2DutyCycleAsync(value),
                Channel2DutyCycle,
                "Channel2DutyCycle");
            await WriteAndLogAsync(
                value => _device.WriteChannel3DutyCycleAsync(value),
                Channel3DutyCycle,
                "Channel3DutyCycle");
            await WriteAndLogAsync(
                value => _device.WriteChannel4DutyCycleAsync(value),
                Channel4DutyCycle,
                "Channel4DutyCycle");
            await WriteAndLogAsync(
                value => _device.WriteDigitalOutputSetAsync(value),
                DigitalOutputSet,
                "DigitalOutputSet");
            await WriteAndLogAsync(
                value => _device.WriteDigitalOutputClearAsync(value),
                DigitalOutputClear,
                "DigitalOutputClear");
            await WriteAndLogAsync(
                value => _device.WriteDigitalOutputToggleAsync(value),
                DigitalOutputToggle,
                "DigitalOutputToggle");
            await WriteAndLogAsync(
                value => _device.WriteDigitalOutputStateAsync(value),
                DigitalOutputState,
                "DigitalOutputState");
            await WriteAndLogAsync(
                value => _device.WriteEnableValvePulseAsync(value),
                EnableValvePulse,
                "EnableValvePulse");
            await WriteAndLogAsync(
                value => _device.WriteValveSetAsync(value),
                ValveSet,
                "ValveSet");
            await WriteAndLogAsync(
                value => _device.WriteValveClearAsync(value),
                ValveClear,
                "ValveClear");
            await WriteAndLogAsync(
                value => _device.WriteValveToggleAsync(value),
                ValveToggle,
                "ValveToggle");
            await WriteAndLogAsync(
                value => _device.WriteValveStateAsync(value),
                ValveState,
                "ValveState");
            await WriteAndLogAsync(
                value => _device.WriteOdorValveStateAsync(value),
                OdorValveState,
                "OdorValveState");
            await WriteAndLogAsync(
                value => _device.WriteEndValveStateAsync(value),
                EndValveState,
                "EndValveState");
            await WriteAndLogAsync(
                value => _device.WriteCheckValveStateAsync(value),
                CheckValveState,
                "CheckValveState");
            await WriteAndLogAsync(
                value => _device.WriteValve0PulseDurationAsync(value),
                Valve0PulseDuration,
                "Valve0PulseDuration");
            await WriteAndLogAsync(
                value => _device.WriteValve1PulseDurationAsync(value),
                Valve1PulseDuration,
                "Valve1PulseDuration");
            await WriteAndLogAsync(
                value => _device.WriteValve2PulseDurationAsync(value),
                Valve2PulseDuration,
                "Valve2PulseDuration");
            await WriteAndLogAsync(
                value => _device.WriteValve3PulseDurationAsync(value),
                Valve3PulseDuration,
                "Valve3PulseDuration");
            await WriteAndLogAsync(
                value => _device.WriteCheckValve0DelayPulseDurationAsync(value),
                CheckValve0DelayPulseDuration,
                "CheckValve0DelayPulseDuration");
            await WriteAndLogAsync(
                value => _device.WriteCheckValve1DelayPulseDurationAsync(value),
                CheckValve1DelayPulseDuration,
                "CheckValve1DelayPulseDuration");
            await WriteAndLogAsync(
                value => _device.WriteCheckValve2DelayPulseDurationAsync(value),
                CheckValve2DelayPulseDuration,
                "CheckValve2DelayPulseDuration");
            await WriteAndLogAsync(
                value => _device.WriteCheckValve3DelayPulseDurationAsync(value),
                CheckValve3DelayPulseDuration,
                "CheckValve3DelayPulseDuration");
            await WriteAndLogAsync(
                value => _device.WriteEndValve0PulseDurationAsync(value),
                EndValve0PulseDuration,
                "EndValve0PulseDuration");
            await WriteAndLogAsync(
                value => _device.WriteEndValve1PulseDurationAsync(value),
                EndValve1PulseDuration,
                "EndValve1PulseDuration");
            await WriteAndLogAsync(
                value => _device.WriteDO0SyncAsync(value),
                DO0Sync,
                "DO0Sync");
            await WriteAndLogAsync(
                value => _device.WriteDO1SyncAsync(value),
                DO1Sync,
                "DO1Sync");
            await WriteAndLogAsync(
                value => _device.WriteDI0TriggerAsync(value),
                DI0Trigger,
                "DI0Trigger");
            await WriteAndLogAsync(
                value => _device.WriteMimicValve0Async(value),
                MimicValve0,
                "MimicValve0");
            await WriteAndLogAsync(
                value => _device.WriteMimicValve1Async(value),
                MimicValve1,
                "MimicValve1");
            await WriteAndLogAsync(
                value => _device.WriteMimicValve2Async(value),
                MimicValve2,
                "MimicValve2");
            await WriteAndLogAsync(
                value => _device.WriteMimicValve3Async(value),
                MimicValve3,
                "MimicValve3");
            await WriteAndLogAsync(
                value => _device.WriteMimicCheckValve0Async(value),
                MimicCheckValve0,
                "MimicCheckValve0");
            await WriteAndLogAsync(
                value => _device.WriteMimicCheckValve1Async(value),
                MimicCheckValve1,
                "MimicCheckValve1");
            await WriteAndLogAsync(
                value => _device.WriteMimicCheckValve2Async(value),
                MimicCheckValve2,
                "MimicCheckValve2");
            await WriteAndLogAsync(
                value => _device.WriteMimicCheckValve3Async(value),
                MimicCheckValve3,
                "MimicCheckValve3");
            await WriteAndLogAsync(
                value => _device.WriteMimicEndValve0Async(value),
                MimicEndValve0,
                "MimicEndValve0");
            await WriteAndLogAsync(
                value => _device.WriteMimicEndValve1Async(value),
                MimicEndValve1,
                "MimicEndValve1");
            await WriteAndLogAsync(
                value => _device.WriteEnableValveExternalControlAsync(value),
                EnableValveExternalControl,
                "EnableValveExternalControl");
            await WriteAndLogAsync(
                value => _device.WriteChannel3RangeAsync(value),
                Channel3Range,
                "Channel3Range");
            await WriteAndLogAsync(
                value => _device.WriteEnableCheckValveSyncAsync(value),
                EnableCheckValveSync,
                "EnableCheckValveSync");
            await WriteAndLogAsync(
                value => _device.WriteEnableTemperatureCalibrationAsync(value),
                EnableTemperatureCalibration,
                "EnableTemperatureCalibration");
            await WriteAndLogAsync(
                value => _device.WriteTemperatureCalibrationValueAsync(value),
                TemperatureCalibrationValue,
                "TemperatureCalibrationValue");
            await WriteAndLogAsync(
                value => _device.WriteEnableEventsAsync(value),
                EnableEvents,
                "EnableEvents");

            // Save the configuration to the device permanently
            if (savePermanently)
            {
                await WriteAndLogAsync(
                    value => _device.WriteResetDeviceAsync(value),
                    ResetFlags.Save,
                    "SavePermanently");
            }
        });
    }

    private IObservable<Unit> ResetConfiguration()
    {
        return Observable.StartAsync(async () =>
        {
            if (_device != null)
            {
                await WriteAndLogAsync(
                    value => _device.WriteResetDeviceAsync(value),
                    ResetFlags.RestoreDefault,
                    "ResetDevice");
            }
        });
    }

    private async Task WriteAndLogAsync<T>(Func<T, Task> writeFunc, T value, string registerName)
    {
        if (_device == null)
            throw new Exception("Device is not connected");

        await writeFunc(value);

        // Log the message to the SentMessages collection on the UI thread
        RxApp.MainThreadScheduler.Schedule(() =>
        {
            SentMessages.Add($"{DateTime.Now:HH:mm:ss.fff} - Write {registerName}: {value}");
        });
    }
}
