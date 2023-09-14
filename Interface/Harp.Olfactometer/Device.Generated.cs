using Bonsai;
using Bonsai.Harp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Xml.Serialization;

namespace Harp.Olfactometer
{
    /// <summary>
    /// Generates events and processes commands for the Olfactometer device connected
    /// at the specified serial port.
    /// </summary>
    [Combinator(MethodName = nameof(Generate))]
    [WorkflowElementCategory(ElementCategory.Source)]
    [Description("Generates events and processes commands for the Olfactometer device.")]
    public partial class Device : Bonsai.Harp.Device, INamedElement
    {
        /// <summary>
        /// Represents the unique identity class of the <see cref="Olfactometer"/> device.
        /// This field is constant.
        /// </summary>
        public const int WhoAmI = 1140;

        /// <summary>
        /// Initializes a new instance of the <see cref="Device"/> class.
        /// </summary>
        public Device() : base(WhoAmI) { }

        string INamedElement.Name => nameof(Olfactometer);

        /// <summary>
        /// Gets a read-only mapping from address to register type.
        /// </summary>
        public static new IReadOnlyDictionary<int, Type> RegisterMap { get; } = new Dictionary<int, Type>
            (Bonsai.Harp.Device.RegisterMap.ToDictionary(entry => entry.Key, entry => entry.Value))
        {
            { 32, typeof(EnableFlow) },
            { 33, typeof(FlowmeterAnalogOutputs) },
            { 34, typeof(DI0State) },
            { 35, typeof(Channel0UserCalibration) },
            { 36, typeof(Channel1UserCalibration) },
            { 37, typeof(Channel2UserCalibration) },
            { 38, typeof(Channel3UserCalibration) },
            { 39, typeof(Channel4UserCalibration) },
            { 40, typeof(Channel3UserCalibrationAux) },
            { 41, typeof(UserCalibrationEnable) },
            { 42, typeof(Channel0FlowTarget) },
            { 43, typeof(Channel1FlowTarget) },
            { 44, typeof(Channel2FlowTarget) },
            { 45, typeof(Channel3FlowTarget) },
            { 46, typeof(Channel4FlowTarget) },
            { 47, typeof(Channel0FlowReal) },
            { 48, typeof(Channel1FlowReal) },
            { 49, typeof(Channel2FlowReal) },
            { 50, typeof(Channel3FlowReal) },
            { 51, typeof(Channel4FlowReal) },
            { 57, typeof(Channel0DutyCycle) },
            { 58, typeof(Channel1DutyCycle) },
            { 59, typeof(Channel2DutyCycle) },
            { 60, typeof(Channel3DutyCycle) },
            { 61, typeof(Channel4DutyCycle) },
            { 62, typeof(DigitalOutputSet) },
            { 63, typeof(DigitalOutputClear) },
            { 64, typeof(DigitalOutputToggle) },
            { 65, typeof(DigitalOutputState) },
            { 66, typeof(EnableValvesPulse) },
            { 67, typeof(ValvesSet) },
            { 68, typeof(ValvesClear) },
            { 69, typeof(ValvesToggle) },
            { 70, typeof(ValvesState) },
            { 71, typeof(PulseValve0) },
            { 72, typeof(PulseValve1) },
            { 73, typeof(PulseValve2) },
            { 74, typeof(PulseValve3) },
            { 75, typeof(PulseEndvalve0) },
            { 76, typeof(PulseEndvalve1) },
            { 77, typeof(PulseDummyvalve) },
            { 78, typeof(DO0Sync) },
            { 79, typeof(DO1Sync) },
            { 80, typeof(DI0Trigger) },
            { 81, typeof(MimicValve0) },
            { 82, typeof(MimicValve1) },
            { 83, typeof(MimicValve2) },
            { 84, typeof(MimicValve3) },
            { 85, typeof(MimicEndvalve0) },
            { 86, typeof(MimicEndvalve1) },
            { 87, typeof(MimicDummyvalve) },
            { 88, typeof(EnableExternalControlValves) },
            { 89, typeof(Channel3Range) },
            { 93, typeof(EnableEvents) }
        };
    }

    /// <summary>
    /// Represents an operator that groups the sequence of <see cref="Olfactometer"/>" messages by register type.
    /// </summary>
    [Description("Groups the sequence of Olfactometer messages by register type.")]
    public partial class GroupByRegister : Combinator<HarpMessage, IGroupedObservable<Type, HarpMessage>>
    {
        /// <summary>
        /// Groups an observable sequence of <see cref="Olfactometer"/> messages
        /// by register type.
        /// </summary>
        /// <param name="source">The sequence of Harp device messages.</param>
        /// <returns>
        /// A sequence of observable groups, each of which corresponds to a unique
        /// <see cref="Olfactometer"/> register.
        /// </returns>
        public override IObservable<IGroupedObservable<Type, HarpMessage>> Process(IObservable<HarpMessage> source)
        {
            return source.GroupBy(message => Device.RegisterMap[message.Address]);
        }
    }

    /// <summary>
    /// Represents an operator that filters register-specific messages
    /// reported by the <see cref="Olfactometer"/> device.
    /// </summary>
    /// <seealso cref="EnableFlow"/>
    /// <seealso cref="FlowmeterAnalogOutputs"/>
    /// <seealso cref="DI0State"/>
    /// <seealso cref="Channel0UserCalibration"/>
    /// <seealso cref="Channel1UserCalibration"/>
    /// <seealso cref="Channel2UserCalibration"/>
    /// <seealso cref="Channel3UserCalibration"/>
    /// <seealso cref="Channel4UserCalibration"/>
    /// <seealso cref="Channel3UserCalibrationAux"/>
    /// <seealso cref="UserCalibrationEnable"/>
    /// <seealso cref="Channel0FlowTarget"/>
    /// <seealso cref="Channel1FlowTarget"/>
    /// <seealso cref="Channel2FlowTarget"/>
    /// <seealso cref="Channel3FlowTarget"/>
    /// <seealso cref="Channel4FlowTarget"/>
    /// <seealso cref="Channel0FlowReal"/>
    /// <seealso cref="Channel1FlowReal"/>
    /// <seealso cref="Channel2FlowReal"/>
    /// <seealso cref="Channel3FlowReal"/>
    /// <seealso cref="Channel4FlowReal"/>
    /// <seealso cref="Channel0DutyCycle"/>
    /// <seealso cref="Channel1DutyCycle"/>
    /// <seealso cref="Channel2DutyCycle"/>
    /// <seealso cref="Channel3DutyCycle"/>
    /// <seealso cref="Channel4DutyCycle"/>
    /// <seealso cref="DigitalOutputSet"/>
    /// <seealso cref="DigitalOutputClear"/>
    /// <seealso cref="DigitalOutputToggle"/>
    /// <seealso cref="DigitalOutputState"/>
    /// <seealso cref="EnableValvesPulse"/>
    /// <seealso cref="ValvesSet"/>
    /// <seealso cref="ValvesClear"/>
    /// <seealso cref="ValvesToggle"/>
    /// <seealso cref="ValvesState"/>
    /// <seealso cref="PulseValve0"/>
    /// <seealso cref="PulseValve1"/>
    /// <seealso cref="PulseValve2"/>
    /// <seealso cref="PulseValve3"/>
    /// <seealso cref="PulseEndvalve0"/>
    /// <seealso cref="PulseEndvalve1"/>
    /// <seealso cref="PulseDummyvalve"/>
    /// <seealso cref="DO0Sync"/>
    /// <seealso cref="DO1Sync"/>
    /// <seealso cref="DI0Trigger"/>
    /// <seealso cref="MimicValve0"/>
    /// <seealso cref="MimicValve1"/>
    /// <seealso cref="MimicValve2"/>
    /// <seealso cref="MimicValve3"/>
    /// <seealso cref="MimicEndvalve0"/>
    /// <seealso cref="MimicEndvalve1"/>
    /// <seealso cref="MimicDummyvalve"/>
    /// <seealso cref="EnableExternalControlValves"/>
    /// <seealso cref="Channel3Range"/>
    /// <seealso cref="EnableEvents"/>
    [XmlInclude(typeof(EnableFlow))]
    [XmlInclude(typeof(FlowmeterAnalogOutputs))]
    [XmlInclude(typeof(DI0State))]
    [XmlInclude(typeof(Channel0UserCalibration))]
    [XmlInclude(typeof(Channel1UserCalibration))]
    [XmlInclude(typeof(Channel2UserCalibration))]
    [XmlInclude(typeof(Channel3UserCalibration))]
    [XmlInclude(typeof(Channel4UserCalibration))]
    [XmlInclude(typeof(Channel3UserCalibrationAux))]
    [XmlInclude(typeof(UserCalibrationEnable))]
    [XmlInclude(typeof(Channel0FlowTarget))]
    [XmlInclude(typeof(Channel1FlowTarget))]
    [XmlInclude(typeof(Channel2FlowTarget))]
    [XmlInclude(typeof(Channel3FlowTarget))]
    [XmlInclude(typeof(Channel4FlowTarget))]
    [XmlInclude(typeof(Channel0FlowReal))]
    [XmlInclude(typeof(Channel1FlowReal))]
    [XmlInclude(typeof(Channel2FlowReal))]
    [XmlInclude(typeof(Channel3FlowReal))]
    [XmlInclude(typeof(Channel4FlowReal))]
    [XmlInclude(typeof(Channel0DutyCycle))]
    [XmlInclude(typeof(Channel1DutyCycle))]
    [XmlInclude(typeof(Channel2DutyCycle))]
    [XmlInclude(typeof(Channel3DutyCycle))]
    [XmlInclude(typeof(Channel4DutyCycle))]
    [XmlInclude(typeof(DigitalOutputSet))]
    [XmlInclude(typeof(DigitalOutputClear))]
    [XmlInclude(typeof(DigitalOutputToggle))]
    [XmlInclude(typeof(DigitalOutputState))]
    [XmlInclude(typeof(EnableValvesPulse))]
    [XmlInclude(typeof(ValvesSet))]
    [XmlInclude(typeof(ValvesClear))]
    [XmlInclude(typeof(ValvesToggle))]
    [XmlInclude(typeof(ValvesState))]
    [XmlInclude(typeof(PulseValve0))]
    [XmlInclude(typeof(PulseValve1))]
    [XmlInclude(typeof(PulseValve2))]
    [XmlInclude(typeof(PulseValve3))]
    [XmlInclude(typeof(PulseEndvalve0))]
    [XmlInclude(typeof(PulseEndvalve1))]
    [XmlInclude(typeof(PulseDummyvalve))]
    [XmlInclude(typeof(DO0Sync))]
    [XmlInclude(typeof(DO1Sync))]
    [XmlInclude(typeof(DI0Trigger))]
    [XmlInclude(typeof(MimicValve0))]
    [XmlInclude(typeof(MimicValve1))]
    [XmlInclude(typeof(MimicValve2))]
    [XmlInclude(typeof(MimicValve3))]
    [XmlInclude(typeof(MimicEndvalve0))]
    [XmlInclude(typeof(MimicEndvalve1))]
    [XmlInclude(typeof(MimicDummyvalve))]
    [XmlInclude(typeof(EnableExternalControlValves))]
    [XmlInclude(typeof(Channel3Range))]
    [XmlInclude(typeof(EnableEvents))]
    [Description("Filters register-specific messages reported by the Olfactometer device.")]
    public class FilterRegister : FilterRegisterBuilder, INamedElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FilterRegister"/> class.
        /// </summary>
        public FilterRegister()
        {
            Register = new EnableFlow();
        }

        string INamedElement.Name
        {
            get => $"{nameof(Olfactometer)}.{GetElementDisplayName(Register)}";
        }
    }

    /// <summary>
    /// Represents an operator which filters and selects specific messages
    /// reported by the Olfactometer device.
    /// </summary>
    /// <seealso cref="EnableFlow"/>
    /// <seealso cref="FlowmeterAnalogOutputs"/>
    /// <seealso cref="DI0State"/>
    /// <seealso cref="Channel0UserCalibration"/>
    /// <seealso cref="Channel1UserCalibration"/>
    /// <seealso cref="Channel2UserCalibration"/>
    /// <seealso cref="Channel3UserCalibration"/>
    /// <seealso cref="Channel4UserCalibration"/>
    /// <seealso cref="Channel3UserCalibrationAux"/>
    /// <seealso cref="UserCalibrationEnable"/>
    /// <seealso cref="Channel0FlowTarget"/>
    /// <seealso cref="Channel1FlowTarget"/>
    /// <seealso cref="Channel2FlowTarget"/>
    /// <seealso cref="Channel3FlowTarget"/>
    /// <seealso cref="Channel4FlowTarget"/>
    /// <seealso cref="Channel0FlowReal"/>
    /// <seealso cref="Channel1FlowReal"/>
    /// <seealso cref="Channel2FlowReal"/>
    /// <seealso cref="Channel3FlowReal"/>
    /// <seealso cref="Channel4FlowReal"/>
    /// <seealso cref="Channel0DutyCycle"/>
    /// <seealso cref="Channel1DutyCycle"/>
    /// <seealso cref="Channel2DutyCycle"/>
    /// <seealso cref="Channel3DutyCycle"/>
    /// <seealso cref="Channel4DutyCycle"/>
    /// <seealso cref="DigitalOutputSet"/>
    /// <seealso cref="DigitalOutputClear"/>
    /// <seealso cref="DigitalOutputToggle"/>
    /// <seealso cref="DigitalOutputState"/>
    /// <seealso cref="EnableValvesPulse"/>
    /// <seealso cref="ValvesSet"/>
    /// <seealso cref="ValvesClear"/>
    /// <seealso cref="ValvesToggle"/>
    /// <seealso cref="ValvesState"/>
    /// <seealso cref="PulseValve0"/>
    /// <seealso cref="PulseValve1"/>
    /// <seealso cref="PulseValve2"/>
    /// <seealso cref="PulseValve3"/>
    /// <seealso cref="PulseEndvalve0"/>
    /// <seealso cref="PulseEndvalve1"/>
    /// <seealso cref="PulseDummyvalve"/>
    /// <seealso cref="DO0Sync"/>
    /// <seealso cref="DO1Sync"/>
    /// <seealso cref="DI0Trigger"/>
    /// <seealso cref="MimicValve0"/>
    /// <seealso cref="MimicValve1"/>
    /// <seealso cref="MimicValve2"/>
    /// <seealso cref="MimicValve3"/>
    /// <seealso cref="MimicEndvalve0"/>
    /// <seealso cref="MimicEndvalve1"/>
    /// <seealso cref="MimicDummyvalve"/>
    /// <seealso cref="EnableExternalControlValves"/>
    /// <seealso cref="Channel3Range"/>
    /// <seealso cref="EnableEvents"/>
    [XmlInclude(typeof(EnableFlow))]
    [XmlInclude(typeof(FlowmeterAnalogOutputs))]
    [XmlInclude(typeof(DI0State))]
    [XmlInclude(typeof(Channel0UserCalibration))]
    [XmlInclude(typeof(Channel1UserCalibration))]
    [XmlInclude(typeof(Channel2UserCalibration))]
    [XmlInclude(typeof(Channel3UserCalibration))]
    [XmlInclude(typeof(Channel4UserCalibration))]
    [XmlInclude(typeof(Channel3UserCalibrationAux))]
    [XmlInclude(typeof(UserCalibrationEnable))]
    [XmlInclude(typeof(Channel0FlowTarget))]
    [XmlInclude(typeof(Channel1FlowTarget))]
    [XmlInclude(typeof(Channel2FlowTarget))]
    [XmlInclude(typeof(Channel3FlowTarget))]
    [XmlInclude(typeof(Channel4FlowTarget))]
    [XmlInclude(typeof(Channel0FlowReal))]
    [XmlInclude(typeof(Channel1FlowReal))]
    [XmlInclude(typeof(Channel2FlowReal))]
    [XmlInclude(typeof(Channel3FlowReal))]
    [XmlInclude(typeof(Channel4FlowReal))]
    [XmlInclude(typeof(Channel0DutyCycle))]
    [XmlInclude(typeof(Channel1DutyCycle))]
    [XmlInclude(typeof(Channel2DutyCycle))]
    [XmlInclude(typeof(Channel3DutyCycle))]
    [XmlInclude(typeof(Channel4DutyCycle))]
    [XmlInclude(typeof(DigitalOutputSet))]
    [XmlInclude(typeof(DigitalOutputClear))]
    [XmlInclude(typeof(DigitalOutputToggle))]
    [XmlInclude(typeof(DigitalOutputState))]
    [XmlInclude(typeof(EnableValvesPulse))]
    [XmlInclude(typeof(ValvesSet))]
    [XmlInclude(typeof(ValvesClear))]
    [XmlInclude(typeof(ValvesToggle))]
    [XmlInclude(typeof(ValvesState))]
    [XmlInclude(typeof(PulseValve0))]
    [XmlInclude(typeof(PulseValve1))]
    [XmlInclude(typeof(PulseValve2))]
    [XmlInclude(typeof(PulseValve3))]
    [XmlInclude(typeof(PulseEndvalve0))]
    [XmlInclude(typeof(PulseEndvalve1))]
    [XmlInclude(typeof(PulseDummyvalve))]
    [XmlInclude(typeof(DO0Sync))]
    [XmlInclude(typeof(DO1Sync))]
    [XmlInclude(typeof(DI0Trigger))]
    [XmlInclude(typeof(MimicValve0))]
    [XmlInclude(typeof(MimicValve1))]
    [XmlInclude(typeof(MimicValve2))]
    [XmlInclude(typeof(MimicValve3))]
    [XmlInclude(typeof(MimicEndvalve0))]
    [XmlInclude(typeof(MimicEndvalve1))]
    [XmlInclude(typeof(MimicDummyvalve))]
    [XmlInclude(typeof(EnableExternalControlValves))]
    [XmlInclude(typeof(Channel3Range))]
    [XmlInclude(typeof(EnableEvents))]
    [XmlInclude(typeof(TimestampedEnableFlow))]
    [XmlInclude(typeof(TimestampedFlowmeterAnalogOutputs))]
    [XmlInclude(typeof(TimestampedDI0State))]
    [XmlInclude(typeof(TimestampedChannel0UserCalibration))]
    [XmlInclude(typeof(TimestampedChannel1UserCalibration))]
    [XmlInclude(typeof(TimestampedChannel2UserCalibration))]
    [XmlInclude(typeof(TimestampedChannel3UserCalibration))]
    [XmlInclude(typeof(TimestampedChannel4UserCalibration))]
    [XmlInclude(typeof(TimestampedChannel3UserCalibrationAux))]
    [XmlInclude(typeof(TimestampedUserCalibrationEnable))]
    [XmlInclude(typeof(TimestampedChannel0FlowTarget))]
    [XmlInclude(typeof(TimestampedChannel1FlowTarget))]
    [XmlInclude(typeof(TimestampedChannel2FlowTarget))]
    [XmlInclude(typeof(TimestampedChannel3FlowTarget))]
    [XmlInclude(typeof(TimestampedChannel4FlowTarget))]
    [XmlInclude(typeof(TimestampedChannel0FlowReal))]
    [XmlInclude(typeof(TimestampedChannel1FlowReal))]
    [XmlInclude(typeof(TimestampedChannel2FlowReal))]
    [XmlInclude(typeof(TimestampedChannel3FlowReal))]
    [XmlInclude(typeof(TimestampedChannel4FlowReal))]
    [XmlInclude(typeof(TimestampedChannel0DutyCycle))]
    [XmlInclude(typeof(TimestampedChannel1DutyCycle))]
    [XmlInclude(typeof(TimestampedChannel2DutyCycle))]
    [XmlInclude(typeof(TimestampedChannel3DutyCycle))]
    [XmlInclude(typeof(TimestampedChannel4DutyCycle))]
    [XmlInclude(typeof(TimestampedDigitalOutputSet))]
    [XmlInclude(typeof(TimestampedDigitalOutputClear))]
    [XmlInclude(typeof(TimestampedDigitalOutputToggle))]
    [XmlInclude(typeof(TimestampedDigitalOutputState))]
    [XmlInclude(typeof(TimestampedEnableValvesPulse))]
    [XmlInclude(typeof(TimestampedValvesSet))]
    [XmlInclude(typeof(TimestampedValvesClear))]
    [XmlInclude(typeof(TimestampedValvesToggle))]
    [XmlInclude(typeof(TimestampedValvesState))]
    [XmlInclude(typeof(TimestampedPulseValve0))]
    [XmlInclude(typeof(TimestampedPulseValve1))]
    [XmlInclude(typeof(TimestampedPulseValve2))]
    [XmlInclude(typeof(TimestampedPulseValve3))]
    [XmlInclude(typeof(TimestampedPulseEndvalve0))]
    [XmlInclude(typeof(TimestampedPulseEndvalve1))]
    [XmlInclude(typeof(TimestampedPulseDummyvalve))]
    [XmlInclude(typeof(TimestampedDO0Sync))]
    [XmlInclude(typeof(TimestampedDO1Sync))]
    [XmlInclude(typeof(TimestampedDI0Trigger))]
    [XmlInclude(typeof(TimestampedMimicValve0))]
    [XmlInclude(typeof(TimestampedMimicValve1))]
    [XmlInclude(typeof(TimestampedMimicValve2))]
    [XmlInclude(typeof(TimestampedMimicValve3))]
    [XmlInclude(typeof(TimestampedMimicEndvalve0))]
    [XmlInclude(typeof(TimestampedMimicEndvalve1))]
    [XmlInclude(typeof(TimestampedMimicDummyvalve))]
    [XmlInclude(typeof(TimestampedEnableExternalControlValves))]
    [XmlInclude(typeof(TimestampedChannel3Range))]
    [XmlInclude(typeof(TimestampedEnableEvents))]
    [Description("Filters and selects specific messages reported by the Olfactometer device.")]
    public partial class Parse : ParseBuilder, INamedElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Parse"/> class.
        /// </summary>
        public Parse()
        {
            Register = new EnableFlow();
        }

        string INamedElement.Name => $"{nameof(Olfactometer)}.{GetElementDisplayName(Register)}";
    }

    /// <summary>
    /// Represents an operator which formats a sequence of values as specific
    /// Olfactometer register messages.
    /// </summary>
    /// <seealso cref="EnableFlow"/>
    /// <seealso cref="FlowmeterAnalogOutputs"/>
    /// <seealso cref="DI0State"/>
    /// <seealso cref="Channel0UserCalibration"/>
    /// <seealso cref="Channel1UserCalibration"/>
    /// <seealso cref="Channel2UserCalibration"/>
    /// <seealso cref="Channel3UserCalibration"/>
    /// <seealso cref="Channel4UserCalibration"/>
    /// <seealso cref="Channel3UserCalibrationAux"/>
    /// <seealso cref="UserCalibrationEnable"/>
    /// <seealso cref="Channel0FlowTarget"/>
    /// <seealso cref="Channel1FlowTarget"/>
    /// <seealso cref="Channel2FlowTarget"/>
    /// <seealso cref="Channel3FlowTarget"/>
    /// <seealso cref="Channel4FlowTarget"/>
    /// <seealso cref="Channel0FlowReal"/>
    /// <seealso cref="Channel1FlowReal"/>
    /// <seealso cref="Channel2FlowReal"/>
    /// <seealso cref="Channel3FlowReal"/>
    /// <seealso cref="Channel4FlowReal"/>
    /// <seealso cref="Channel0DutyCycle"/>
    /// <seealso cref="Channel1DutyCycle"/>
    /// <seealso cref="Channel2DutyCycle"/>
    /// <seealso cref="Channel3DutyCycle"/>
    /// <seealso cref="Channel4DutyCycle"/>
    /// <seealso cref="DigitalOutputSet"/>
    /// <seealso cref="DigitalOutputClear"/>
    /// <seealso cref="DigitalOutputToggle"/>
    /// <seealso cref="DigitalOutputState"/>
    /// <seealso cref="EnableValvesPulse"/>
    /// <seealso cref="ValvesSet"/>
    /// <seealso cref="ValvesClear"/>
    /// <seealso cref="ValvesToggle"/>
    /// <seealso cref="ValvesState"/>
    /// <seealso cref="PulseValve0"/>
    /// <seealso cref="PulseValve1"/>
    /// <seealso cref="PulseValve2"/>
    /// <seealso cref="PulseValve3"/>
    /// <seealso cref="PulseEndvalve0"/>
    /// <seealso cref="PulseEndvalve1"/>
    /// <seealso cref="PulseDummyvalve"/>
    /// <seealso cref="DO0Sync"/>
    /// <seealso cref="DO1Sync"/>
    /// <seealso cref="DI0Trigger"/>
    /// <seealso cref="MimicValve0"/>
    /// <seealso cref="MimicValve1"/>
    /// <seealso cref="MimicValve2"/>
    /// <seealso cref="MimicValve3"/>
    /// <seealso cref="MimicEndvalve0"/>
    /// <seealso cref="MimicEndvalve1"/>
    /// <seealso cref="MimicDummyvalve"/>
    /// <seealso cref="EnableExternalControlValves"/>
    /// <seealso cref="Channel3Range"/>
    /// <seealso cref="EnableEvents"/>
    [XmlInclude(typeof(EnableFlow))]
    [XmlInclude(typeof(FlowmeterAnalogOutputs))]
    [XmlInclude(typeof(DI0State))]
    [XmlInclude(typeof(Channel0UserCalibration))]
    [XmlInclude(typeof(Channel1UserCalibration))]
    [XmlInclude(typeof(Channel2UserCalibration))]
    [XmlInclude(typeof(Channel3UserCalibration))]
    [XmlInclude(typeof(Channel4UserCalibration))]
    [XmlInclude(typeof(Channel3UserCalibrationAux))]
    [XmlInclude(typeof(UserCalibrationEnable))]
    [XmlInclude(typeof(Channel0FlowTarget))]
    [XmlInclude(typeof(Channel1FlowTarget))]
    [XmlInclude(typeof(Channel2FlowTarget))]
    [XmlInclude(typeof(Channel3FlowTarget))]
    [XmlInclude(typeof(Channel4FlowTarget))]
    [XmlInclude(typeof(Channel0FlowReal))]
    [XmlInclude(typeof(Channel1FlowReal))]
    [XmlInclude(typeof(Channel2FlowReal))]
    [XmlInclude(typeof(Channel3FlowReal))]
    [XmlInclude(typeof(Channel4FlowReal))]
    [XmlInclude(typeof(Channel0DutyCycle))]
    [XmlInclude(typeof(Channel1DutyCycle))]
    [XmlInclude(typeof(Channel2DutyCycle))]
    [XmlInclude(typeof(Channel3DutyCycle))]
    [XmlInclude(typeof(Channel4DutyCycle))]
    [XmlInclude(typeof(DigitalOutputSet))]
    [XmlInclude(typeof(DigitalOutputClear))]
    [XmlInclude(typeof(DigitalOutputToggle))]
    [XmlInclude(typeof(DigitalOutputState))]
    [XmlInclude(typeof(EnableValvesPulse))]
    [XmlInclude(typeof(ValvesSet))]
    [XmlInclude(typeof(ValvesClear))]
    [XmlInclude(typeof(ValvesToggle))]
    [XmlInclude(typeof(ValvesState))]
    [XmlInclude(typeof(PulseValve0))]
    [XmlInclude(typeof(PulseValve1))]
    [XmlInclude(typeof(PulseValve2))]
    [XmlInclude(typeof(PulseValve3))]
    [XmlInclude(typeof(PulseEndvalve0))]
    [XmlInclude(typeof(PulseEndvalve1))]
    [XmlInclude(typeof(PulseDummyvalve))]
    [XmlInclude(typeof(DO0Sync))]
    [XmlInclude(typeof(DO1Sync))]
    [XmlInclude(typeof(DI0Trigger))]
    [XmlInclude(typeof(MimicValve0))]
    [XmlInclude(typeof(MimicValve1))]
    [XmlInclude(typeof(MimicValve2))]
    [XmlInclude(typeof(MimicValve3))]
    [XmlInclude(typeof(MimicEndvalve0))]
    [XmlInclude(typeof(MimicEndvalve1))]
    [XmlInclude(typeof(MimicDummyvalve))]
    [XmlInclude(typeof(EnableExternalControlValves))]
    [XmlInclude(typeof(Channel3Range))]
    [XmlInclude(typeof(EnableEvents))]
    [Description("Formats a sequence of values as specific Olfactometer register messages.")]
    public partial class Format : FormatBuilder, INamedElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Format"/> class.
        /// </summary>
        public Format()
        {
            Register = new EnableFlow();
        }

        string INamedElement.Name => $"{nameof(Olfactometer)}.{GetElementDisplayName(Register)}";
    }

    /// <summary>
    /// Represents a register that write any value above zero to start the flowmeter and zero to stop.
    /// </summary>
    [Description("Write any value above zero to start the flowmeter and zero to stop.")]
    public partial class EnableFlow
    {
        /// <summary>
        /// Represents the address of the <see cref="EnableFlow"/> register. This field is constant.
        /// </summary>
        public const int Address = 32;

        /// <summary>
        /// Represents the payload type of the <see cref="EnableFlow"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="EnableFlow"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="EnableFlow"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static EnableFlag GetPayload(HarpMessage message)
        {
            return (EnableFlag)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="EnableFlow"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<EnableFlag> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((EnableFlag)payload.Value, payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="EnableFlow"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="EnableFlow"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, EnableFlag value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="EnableFlow"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="EnableFlow"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, EnableFlag value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// EnableFlow register.
    /// </summary>
    /// <seealso cref="EnableFlow"/>
    [Description("Filters and selects timestamped messages from the EnableFlow register.")]
    public partial class TimestampedEnableFlow
    {
        /// <summary>
        /// Represents the address of the <see cref="EnableFlow"/> register. This field is constant.
        /// </summary>
        public const int Address = EnableFlow.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="EnableFlow"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<EnableFlag> GetPayload(HarpMessage message)
        {
            return EnableFlow.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that value of the flowmeters analog outups.
    /// </summary>
    [Description("Value of the flowmeters analog outups.")]
    public partial class FlowmeterAnalogOutputs
    {
        /// <summary>
        /// Represents the address of the <see cref="FlowmeterAnalogOutputs"/> register. This field is constant.
        /// </summary>
        public const int Address = 33;

        /// <summary>
        /// Represents the payload type of the <see cref="FlowmeterAnalogOutputs"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.S16;

        /// <summary>
        /// Represents the length of the <see cref="FlowmeterAnalogOutputs"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 5;

        /// <summary>
        /// Returns the payload data for <see cref="FlowmeterAnalogOutputs"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static short[] GetPayload(HarpMessage message)
        {
            return message.GetPayloadArray<short>();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="FlowmeterAnalogOutputs"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<short[]> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadArray<short>();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="FlowmeterAnalogOutputs"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="FlowmeterAnalogOutputs"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, short[] value)
        {
            return HarpMessage.FromInt16(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="FlowmeterAnalogOutputs"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="FlowmeterAnalogOutputs"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, short[] value)
        {
            return HarpMessage.FromInt16(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// FlowmeterAnalogOutputs register.
    /// </summary>
    /// <seealso cref="FlowmeterAnalogOutputs"/>
    [Description("Filters and selects timestamped messages from the FlowmeterAnalogOutputs register.")]
    public partial class TimestampedFlowmeterAnalogOutputs
    {
        /// <summary>
        /// Represents the address of the <see cref="FlowmeterAnalogOutputs"/> register. This field is constant.
        /// </summary>
        public const int Address = FlowmeterAnalogOutputs.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="FlowmeterAnalogOutputs"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<short[]> GetPayload(HarpMessage message)
        {
            return FlowmeterAnalogOutputs.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that state of the digital input pin 0.
    /// </summary>
    [Description("State of the digital input pin 0.")]
    public partial class DI0State
    {
        /// <summary>
        /// Represents the address of the <see cref="DI0State"/> register. This field is constant.
        /// </summary>
        public const int Address = 34;

        /// <summary>
        /// Represents the payload type of the <see cref="DI0State"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="DI0State"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="DI0State"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static DigitalState GetPayload(HarpMessage message)
        {
            return (DigitalState)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="DI0State"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<DigitalState> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((DigitalState)payload.Value, payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="DI0State"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="DI0State"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, DigitalState value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="DI0State"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="DI0State"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, DigitalState value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// DI0State register.
    /// </summary>
    /// <seealso cref="DI0State"/>
    [Description("Filters and selects timestamped messages from the DI0State register.")]
    public partial class TimestampedDI0State
    {
        /// <summary>
        /// Represents the address of the <see cref="DI0State"/> register. This field is constant.
        /// </summary>
        public const int Address = DI0State.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="DI0State"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<DigitalState> GetPayload(HarpMessage message)
        {
            return DI0State.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that values of calibration for channel 0 - flowmeter 0 [x0,x1, ..., x10] [x= ADC raw value for 0-100 ml/min, step 10].
    /// </summary>
    [Description("Values of calibration for channel 0 - flowmeter 0 [x0,x1, ..., x10] [x= ADC raw value for 0-100 ml/min, step 10].")]
    public partial class Channel0UserCalibration
    {
        /// <summary>
        /// Represents the address of the <see cref="Channel0UserCalibration"/> register. This field is constant.
        /// </summary>
        public const int Address = 35;

        /// <summary>
        /// Represents the payload type of the <see cref="Channel0UserCalibration"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U16;

        /// <summary>
        /// Represents the length of the <see cref="Channel0UserCalibration"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 11;

        /// <summary>
        /// Returns the payload data for <see cref="Channel0UserCalibration"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static ushort[] GetPayload(HarpMessage message)
        {
            return message.GetPayloadArray<ushort>();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="Channel0UserCalibration"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort[]> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadArray<ushort>();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="Channel0UserCalibration"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Channel0UserCalibration"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, ushort[] value)
        {
            return HarpMessage.FromUInt16(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="Channel0UserCalibration"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Channel0UserCalibration"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, ushort[] value)
        {
            return HarpMessage.FromUInt16(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// Channel0UserCalibration register.
    /// </summary>
    /// <seealso cref="Channel0UserCalibration"/>
    [Description("Filters and selects timestamped messages from the Channel0UserCalibration register.")]
    public partial class TimestampedChannel0UserCalibration
    {
        /// <summary>
        /// Represents the address of the <see cref="Channel0UserCalibration"/> register. This field is constant.
        /// </summary>
        public const int Address = Channel0UserCalibration.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="Channel0UserCalibration"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort[]> GetPayload(HarpMessage message)
        {
            return Channel0UserCalibration.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that values of calibration for channel 1 - flowmeter 1 [x0,x1, ..., x10] [x= ADC raw value for 0-100 ml/min, step 10].
    /// </summary>
    [Description("Values of calibration for channel 1 - flowmeter 1 [x0,x1, ..., x10] [x= ADC raw value for 0-100 ml/min, step 10].")]
    public partial class Channel1UserCalibration
    {
        /// <summary>
        /// Represents the address of the <see cref="Channel1UserCalibration"/> register. This field is constant.
        /// </summary>
        public const int Address = 36;

        /// <summary>
        /// Represents the payload type of the <see cref="Channel1UserCalibration"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U16;

        /// <summary>
        /// Represents the length of the <see cref="Channel1UserCalibration"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 11;

        /// <summary>
        /// Returns the payload data for <see cref="Channel1UserCalibration"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static ushort[] GetPayload(HarpMessage message)
        {
            return message.GetPayloadArray<ushort>();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="Channel1UserCalibration"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort[]> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadArray<ushort>();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="Channel1UserCalibration"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Channel1UserCalibration"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, ushort[] value)
        {
            return HarpMessage.FromUInt16(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="Channel1UserCalibration"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Channel1UserCalibration"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, ushort[] value)
        {
            return HarpMessage.FromUInt16(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// Channel1UserCalibration register.
    /// </summary>
    /// <seealso cref="Channel1UserCalibration"/>
    [Description("Filters and selects timestamped messages from the Channel1UserCalibration register.")]
    public partial class TimestampedChannel1UserCalibration
    {
        /// <summary>
        /// Represents the address of the <see cref="Channel1UserCalibration"/> register. This field is constant.
        /// </summary>
        public const int Address = Channel1UserCalibration.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="Channel1UserCalibration"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort[]> GetPayload(HarpMessage message)
        {
            return Channel1UserCalibration.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that values of calibration for channel 2 - flowmeter 2 [x0,x1, ..., x10] [x= ADC raw value for 0-100 ml/min, step 10].
    /// </summary>
    [Description("Values of calibration for channel 2 - flowmeter 2 [x0,x1, ..., x10] [x= ADC raw value for 0-100 ml/min, step 10].")]
    public partial class Channel2UserCalibration
    {
        /// <summary>
        /// Represents the address of the <see cref="Channel2UserCalibration"/> register. This field is constant.
        /// </summary>
        public const int Address = 37;

        /// <summary>
        /// Represents the payload type of the <see cref="Channel2UserCalibration"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U16;

        /// <summary>
        /// Represents the length of the <see cref="Channel2UserCalibration"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 11;

        /// <summary>
        /// Returns the payload data for <see cref="Channel2UserCalibration"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static ushort[] GetPayload(HarpMessage message)
        {
            return message.GetPayloadArray<ushort>();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="Channel2UserCalibration"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort[]> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadArray<ushort>();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="Channel2UserCalibration"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Channel2UserCalibration"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, ushort[] value)
        {
            return HarpMessage.FromUInt16(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="Channel2UserCalibration"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Channel2UserCalibration"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, ushort[] value)
        {
            return HarpMessage.FromUInt16(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// Channel2UserCalibration register.
    /// </summary>
    /// <seealso cref="Channel2UserCalibration"/>
    [Description("Filters and selects timestamped messages from the Channel2UserCalibration register.")]
    public partial class TimestampedChannel2UserCalibration
    {
        /// <summary>
        /// Represents the address of the <see cref="Channel2UserCalibration"/> register. This field is constant.
        /// </summary>
        public const int Address = Channel2UserCalibration.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="Channel2UserCalibration"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort[]> GetPayload(HarpMessage message)
        {
            return Channel2UserCalibration.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that values of calibration for channel 3 - flowmeter 3 [x0,x1, ..., x10] [x= ADC raw value for 0-100 ml/min, step 10].
    /// </summary>
    [Description("Values of calibration for channel 3 - flowmeter 3 [x0,x1, ..., x10] [x= ADC raw value for 0-100 ml/min, step 10].")]
    public partial class Channel3UserCalibration
    {
        /// <summary>
        /// Represents the address of the <see cref="Channel3UserCalibration"/> register. This field is constant.
        /// </summary>
        public const int Address = 38;

        /// <summary>
        /// Represents the payload type of the <see cref="Channel3UserCalibration"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U16;

        /// <summary>
        /// Represents the length of the <see cref="Channel3UserCalibration"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 11;

        /// <summary>
        /// Returns the payload data for <see cref="Channel3UserCalibration"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static ushort[] GetPayload(HarpMessage message)
        {
            return message.GetPayloadArray<ushort>();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="Channel3UserCalibration"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort[]> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadArray<ushort>();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="Channel3UserCalibration"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Channel3UserCalibration"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, ushort[] value)
        {
            return HarpMessage.FromUInt16(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="Channel3UserCalibration"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Channel3UserCalibration"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, ushort[] value)
        {
            return HarpMessage.FromUInt16(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// Channel3UserCalibration register.
    /// </summary>
    /// <seealso cref="Channel3UserCalibration"/>
    [Description("Filters and selects timestamped messages from the Channel3UserCalibration register.")]
    public partial class TimestampedChannel3UserCalibration
    {
        /// <summary>
        /// Represents the address of the <see cref="Channel3UserCalibration"/> register. This field is constant.
        /// </summary>
        public const int Address = Channel3UserCalibration.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="Channel3UserCalibration"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort[]> GetPayload(HarpMessage message)
        {
            return Channel3UserCalibration.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that values of calibration for channel 4 - flowmeter 4 [x0,x1, ..., x10] [x= ADC raw value for 0-1000 ml/min, step 100].
    /// </summary>
    [Description("Values of calibration for channel 4 - flowmeter 4 [x0,x1, ..., x10] [x= ADC raw value for 0-1000 ml/min, step 100].")]
    public partial class Channel4UserCalibration
    {
        /// <summary>
        /// Represents the address of the <see cref="Channel4UserCalibration"/> register. This field is constant.
        /// </summary>
        public const int Address = 39;

        /// <summary>
        /// Represents the payload type of the <see cref="Channel4UserCalibration"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U16;

        /// <summary>
        /// Represents the length of the <see cref="Channel4UserCalibration"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 11;

        /// <summary>
        /// Returns the payload data for <see cref="Channel4UserCalibration"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static ushort[] GetPayload(HarpMessage message)
        {
            return message.GetPayloadArray<ushort>();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="Channel4UserCalibration"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort[]> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadArray<ushort>();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="Channel4UserCalibration"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Channel4UserCalibration"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, ushort[] value)
        {
            return HarpMessage.FromUInt16(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="Channel4UserCalibration"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Channel4UserCalibration"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, ushort[] value)
        {
            return HarpMessage.FromUInt16(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// Channel4UserCalibration register.
    /// </summary>
    /// <seealso cref="Channel4UserCalibration"/>
    [Description("Filters and selects timestamped messages from the Channel4UserCalibration register.")]
    public partial class TimestampedChannel4UserCalibration
    {
        /// <summary>
        /// Represents the address of the <see cref="Channel4UserCalibration"/> register. This field is constant.
        /// </summary>
        public const int Address = Channel4UserCalibration.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="Channel4UserCalibration"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort[]> GetPayload(HarpMessage message)
        {
            return Channel4UserCalibration.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that values of calibration for channel 3 - flowmeter 3 [x0,x1, ..., x10] [x= ADC raw value for 0-1000 ml/min, step 100].
    /// </summary>
    [Description("Values of calibration for channel 3 - flowmeter 3 [x0,x1, ..., x10] [x= ADC raw value for 0-1000 ml/min, step 100].")]
    public partial class Channel3UserCalibrationAux
    {
        /// <summary>
        /// Represents the address of the <see cref="Channel3UserCalibrationAux"/> register. This field is constant.
        /// </summary>
        public const int Address = 40;

        /// <summary>
        /// Represents the payload type of the <see cref="Channel3UserCalibrationAux"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U16;

        /// <summary>
        /// Represents the length of the <see cref="Channel3UserCalibrationAux"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 11;

        /// <summary>
        /// Returns the payload data for <see cref="Channel3UserCalibrationAux"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static ushort[] GetPayload(HarpMessage message)
        {
            return message.GetPayloadArray<ushort>();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="Channel3UserCalibrationAux"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort[]> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadArray<ushort>();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="Channel3UserCalibrationAux"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Channel3UserCalibrationAux"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, ushort[] value)
        {
            return HarpMessage.FromUInt16(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="Channel3UserCalibrationAux"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Channel3UserCalibrationAux"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, ushort[] value)
        {
            return HarpMessage.FromUInt16(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// Channel3UserCalibrationAux register.
    /// </summary>
    /// <seealso cref="Channel3UserCalibrationAux"/>
    [Description("Filters and selects timestamped messages from the Channel3UserCalibrationAux register.")]
    public partial class TimestampedChannel3UserCalibrationAux
    {
        /// <summary>
        /// Represents the address of the <see cref="Channel3UserCalibrationAux"/> register. This field is constant.
        /// </summary>
        public const int Address = Channel3UserCalibrationAux.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="Channel3UserCalibrationAux"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort[]> GetPayload(HarpMessage message)
        {
            return Channel3UserCalibrationAux.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that override the factory calibration values, replacing with CHX_USER_CALIBRATION.
    /// </summary>
    [Description("Override the factory calibration values, replacing with CHX_USER_CALIBRATION.")]
    public partial class UserCalibrationEnable
    {
        /// <summary>
        /// Represents the address of the <see cref="UserCalibrationEnable"/> register. This field is constant.
        /// </summary>
        public const int Address = 41;

        /// <summary>
        /// Represents the payload type of the <see cref="UserCalibrationEnable"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="UserCalibrationEnable"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="UserCalibrationEnable"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static byte GetPayload(HarpMessage message)
        {
            return message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="UserCalibrationEnable"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<byte> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadByte();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="UserCalibrationEnable"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="UserCalibrationEnable"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, byte value)
        {
            return HarpMessage.FromByte(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="UserCalibrationEnable"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="UserCalibrationEnable"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, byte value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// UserCalibrationEnable register.
    /// </summary>
    /// <seealso cref="UserCalibrationEnable"/>
    [Description("Filters and selects timestamped messages from the UserCalibrationEnable register.")]
    public partial class TimestampedUserCalibrationEnable
    {
        /// <summary>
        /// Represents the address of the <see cref="UserCalibrationEnable"/> register. This field is constant.
        /// </summary>
        public const int Address = UserCalibrationEnable.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="UserCalibrationEnable"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<byte> GetPayload(HarpMessage message)
        {
            return UserCalibrationEnable.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that flow value set for channel 0 - flowmeter 0 [ml/min].
    /// </summary>
    [Description("Flow value set for channel 0 - flowmeter 0 [ml/min].")]
    public partial class Channel0FlowTarget
    {
        /// <summary>
        /// Represents the address of the <see cref="Channel0FlowTarget"/> register. This field is constant.
        /// </summary>
        public const int Address = 42;

        /// <summary>
        /// Represents the payload type of the <see cref="Channel0FlowTarget"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.Float;

        /// <summary>
        /// Represents the length of the <see cref="Channel0FlowTarget"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="Channel0FlowTarget"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static float GetPayload(HarpMessage message)
        {
            return message.GetPayloadSingle();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="Channel0FlowTarget"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<float> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadSingle();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="Channel0FlowTarget"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Channel0FlowTarget"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, float value)
        {
            return HarpMessage.FromSingle(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="Channel0FlowTarget"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Channel0FlowTarget"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, float value)
        {
            return HarpMessage.FromSingle(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// Channel0FlowTarget register.
    /// </summary>
    /// <seealso cref="Channel0FlowTarget"/>
    [Description("Filters and selects timestamped messages from the Channel0FlowTarget register.")]
    public partial class TimestampedChannel0FlowTarget
    {
        /// <summary>
        /// Represents the address of the <see cref="Channel0FlowTarget"/> register. This field is constant.
        /// </summary>
        public const int Address = Channel0FlowTarget.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="Channel0FlowTarget"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<float> GetPayload(HarpMessage message)
        {
            return Channel0FlowTarget.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that flow value set for channel 1 - flowmeter 1 [ml/min].
    /// </summary>
    [Description("Flow value set for channel 1 - flowmeter 1 [ml/min].")]
    public partial class Channel1FlowTarget
    {
        /// <summary>
        /// Represents the address of the <see cref="Channel1FlowTarget"/> register. This field is constant.
        /// </summary>
        public const int Address = 43;

        /// <summary>
        /// Represents the payload type of the <see cref="Channel1FlowTarget"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.Float;

        /// <summary>
        /// Represents the length of the <see cref="Channel1FlowTarget"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="Channel1FlowTarget"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static float GetPayload(HarpMessage message)
        {
            return message.GetPayloadSingle();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="Channel1FlowTarget"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<float> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadSingle();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="Channel1FlowTarget"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Channel1FlowTarget"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, float value)
        {
            return HarpMessage.FromSingle(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="Channel1FlowTarget"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Channel1FlowTarget"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, float value)
        {
            return HarpMessage.FromSingle(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// Channel1FlowTarget register.
    /// </summary>
    /// <seealso cref="Channel1FlowTarget"/>
    [Description("Filters and selects timestamped messages from the Channel1FlowTarget register.")]
    public partial class TimestampedChannel1FlowTarget
    {
        /// <summary>
        /// Represents the address of the <see cref="Channel1FlowTarget"/> register. This field is constant.
        /// </summary>
        public const int Address = Channel1FlowTarget.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="Channel1FlowTarget"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<float> GetPayload(HarpMessage message)
        {
            return Channel1FlowTarget.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that flow value set for channel 2 - flowmeter 2 [ml/min].
    /// </summary>
    [Description("Flow value set for channel 2 - flowmeter 2 [ml/min].")]
    public partial class Channel2FlowTarget
    {
        /// <summary>
        /// Represents the address of the <see cref="Channel2FlowTarget"/> register. This field is constant.
        /// </summary>
        public const int Address = 44;

        /// <summary>
        /// Represents the payload type of the <see cref="Channel2FlowTarget"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.Float;

        /// <summary>
        /// Represents the length of the <see cref="Channel2FlowTarget"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="Channel2FlowTarget"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static float GetPayload(HarpMessage message)
        {
            return message.GetPayloadSingle();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="Channel2FlowTarget"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<float> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadSingle();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="Channel2FlowTarget"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Channel2FlowTarget"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, float value)
        {
            return HarpMessage.FromSingle(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="Channel2FlowTarget"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Channel2FlowTarget"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, float value)
        {
            return HarpMessage.FromSingle(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// Channel2FlowTarget register.
    /// </summary>
    /// <seealso cref="Channel2FlowTarget"/>
    [Description("Filters and selects timestamped messages from the Channel2FlowTarget register.")]
    public partial class TimestampedChannel2FlowTarget
    {
        /// <summary>
        /// Represents the address of the <see cref="Channel2FlowTarget"/> register. This field is constant.
        /// </summary>
        public const int Address = Channel2FlowTarget.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="Channel2FlowTarget"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<float> GetPayload(HarpMessage message)
        {
            return Channel2FlowTarget.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that flow value set for channel 3 - flowmeter 3 [ml/min].
    /// </summary>
    [Description("Flow value set for channel 3 - flowmeter 3 [ml/min].")]
    public partial class Channel3FlowTarget
    {
        /// <summary>
        /// Represents the address of the <see cref="Channel3FlowTarget"/> register. This field is constant.
        /// </summary>
        public const int Address = 45;

        /// <summary>
        /// Represents the payload type of the <see cref="Channel3FlowTarget"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.Float;

        /// <summary>
        /// Represents the length of the <see cref="Channel3FlowTarget"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="Channel3FlowTarget"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static float GetPayload(HarpMessage message)
        {
            return message.GetPayloadSingle();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="Channel3FlowTarget"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<float> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadSingle();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="Channel3FlowTarget"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Channel3FlowTarget"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, float value)
        {
            return HarpMessage.FromSingle(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="Channel3FlowTarget"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Channel3FlowTarget"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, float value)
        {
            return HarpMessage.FromSingle(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// Channel3FlowTarget register.
    /// </summary>
    /// <seealso cref="Channel3FlowTarget"/>
    [Description("Filters and selects timestamped messages from the Channel3FlowTarget register.")]
    public partial class TimestampedChannel3FlowTarget
    {
        /// <summary>
        /// Represents the address of the <see cref="Channel3FlowTarget"/> register. This field is constant.
        /// </summary>
        public const int Address = Channel3FlowTarget.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="Channel3FlowTarget"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<float> GetPayload(HarpMessage message)
        {
            return Channel3FlowTarget.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that flow value set for channel 4 - flowmeter 4 [ml/min].
    /// </summary>
    [Description("Flow value set for channel 4 - flowmeter 4 [ml/min].")]
    public partial class Channel4FlowTarget
    {
        /// <summary>
        /// Represents the address of the <see cref="Channel4FlowTarget"/> register. This field is constant.
        /// </summary>
        public const int Address = 46;

        /// <summary>
        /// Represents the payload type of the <see cref="Channel4FlowTarget"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.Float;

        /// <summary>
        /// Represents the length of the <see cref="Channel4FlowTarget"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="Channel4FlowTarget"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static float GetPayload(HarpMessage message)
        {
            return message.GetPayloadSingle();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="Channel4FlowTarget"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<float> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadSingle();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="Channel4FlowTarget"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Channel4FlowTarget"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, float value)
        {
            return HarpMessage.FromSingle(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="Channel4FlowTarget"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Channel4FlowTarget"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, float value)
        {
            return HarpMessage.FromSingle(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// Channel4FlowTarget register.
    /// </summary>
    /// <seealso cref="Channel4FlowTarget"/>
    [Description("Filters and selects timestamped messages from the Channel4FlowTarget register.")]
    public partial class TimestampedChannel4FlowTarget
    {
        /// <summary>
        /// Represents the address of the <see cref="Channel4FlowTarget"/> register. This field is constant.
        /// </summary>
        public const int Address = Channel4FlowTarget.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="Channel4FlowTarget"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<float> GetPayload(HarpMessage message)
        {
            return Channel4FlowTarget.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that flow value read from channel 0 - flowmeter 0 [ml/min].
    /// </summary>
    [Description("Flow value read from channel 0 - flowmeter 0 [ml/min].")]
    public partial class Channel0FlowReal
    {
        /// <summary>
        /// Represents the address of the <see cref="Channel0FlowReal"/> register. This field is constant.
        /// </summary>
        public const int Address = 47;

        /// <summary>
        /// Represents the payload type of the <see cref="Channel0FlowReal"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.Float;

        /// <summary>
        /// Represents the length of the <see cref="Channel0FlowReal"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="Channel0FlowReal"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static float GetPayload(HarpMessage message)
        {
            return message.GetPayloadSingle();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="Channel0FlowReal"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<float> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadSingle();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="Channel0FlowReal"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Channel0FlowReal"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, float value)
        {
            return HarpMessage.FromSingle(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="Channel0FlowReal"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Channel0FlowReal"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, float value)
        {
            return HarpMessage.FromSingle(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// Channel0FlowReal register.
    /// </summary>
    /// <seealso cref="Channel0FlowReal"/>
    [Description("Filters and selects timestamped messages from the Channel0FlowReal register.")]
    public partial class TimestampedChannel0FlowReal
    {
        /// <summary>
        /// Represents the address of the <see cref="Channel0FlowReal"/> register. This field is constant.
        /// </summary>
        public const int Address = Channel0FlowReal.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="Channel0FlowReal"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<float> GetPayload(HarpMessage message)
        {
            return Channel0FlowReal.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that flow value read from channel 1 - flowmeter 1 [ml/min].
    /// </summary>
    [Description("Flow value read from channel 1 - flowmeter 1 [ml/min].")]
    public partial class Channel1FlowReal
    {
        /// <summary>
        /// Represents the address of the <see cref="Channel1FlowReal"/> register. This field is constant.
        /// </summary>
        public const int Address = 48;

        /// <summary>
        /// Represents the payload type of the <see cref="Channel1FlowReal"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.Float;

        /// <summary>
        /// Represents the length of the <see cref="Channel1FlowReal"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="Channel1FlowReal"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static float GetPayload(HarpMessage message)
        {
            return message.GetPayloadSingle();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="Channel1FlowReal"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<float> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadSingle();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="Channel1FlowReal"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Channel1FlowReal"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, float value)
        {
            return HarpMessage.FromSingle(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="Channel1FlowReal"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Channel1FlowReal"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, float value)
        {
            return HarpMessage.FromSingle(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// Channel1FlowReal register.
    /// </summary>
    /// <seealso cref="Channel1FlowReal"/>
    [Description("Filters and selects timestamped messages from the Channel1FlowReal register.")]
    public partial class TimestampedChannel1FlowReal
    {
        /// <summary>
        /// Represents the address of the <see cref="Channel1FlowReal"/> register. This field is constant.
        /// </summary>
        public const int Address = Channel1FlowReal.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="Channel1FlowReal"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<float> GetPayload(HarpMessage message)
        {
            return Channel1FlowReal.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that flow value read from channel 2 - flowmeter 2 [ml/min].
    /// </summary>
    [Description("Flow value read from channel 2 - flowmeter 2 [ml/min].")]
    public partial class Channel2FlowReal
    {
        /// <summary>
        /// Represents the address of the <see cref="Channel2FlowReal"/> register. This field is constant.
        /// </summary>
        public const int Address = 49;

        /// <summary>
        /// Represents the payload type of the <see cref="Channel2FlowReal"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.Float;

        /// <summary>
        /// Represents the length of the <see cref="Channel2FlowReal"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="Channel2FlowReal"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static float GetPayload(HarpMessage message)
        {
            return message.GetPayloadSingle();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="Channel2FlowReal"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<float> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadSingle();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="Channel2FlowReal"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Channel2FlowReal"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, float value)
        {
            return HarpMessage.FromSingle(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="Channel2FlowReal"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Channel2FlowReal"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, float value)
        {
            return HarpMessage.FromSingle(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// Channel2FlowReal register.
    /// </summary>
    /// <seealso cref="Channel2FlowReal"/>
    [Description("Filters and selects timestamped messages from the Channel2FlowReal register.")]
    public partial class TimestampedChannel2FlowReal
    {
        /// <summary>
        /// Represents the address of the <see cref="Channel2FlowReal"/> register. This field is constant.
        /// </summary>
        public const int Address = Channel2FlowReal.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="Channel2FlowReal"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<float> GetPayload(HarpMessage message)
        {
            return Channel2FlowReal.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that flow value read from channel 3 - flowmeter 3 [ml/min].
    /// </summary>
    [Description("Flow value read from channel 3 - flowmeter 3 [ml/min].")]
    public partial class Channel3FlowReal
    {
        /// <summary>
        /// Represents the address of the <see cref="Channel3FlowReal"/> register. This field is constant.
        /// </summary>
        public const int Address = 50;

        /// <summary>
        /// Represents the payload type of the <see cref="Channel3FlowReal"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.Float;

        /// <summary>
        /// Represents the length of the <see cref="Channel3FlowReal"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="Channel3FlowReal"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static float GetPayload(HarpMessage message)
        {
            return message.GetPayloadSingle();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="Channel3FlowReal"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<float> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadSingle();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="Channel3FlowReal"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Channel3FlowReal"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, float value)
        {
            return HarpMessage.FromSingle(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="Channel3FlowReal"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Channel3FlowReal"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, float value)
        {
            return HarpMessage.FromSingle(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// Channel3FlowReal register.
    /// </summary>
    /// <seealso cref="Channel3FlowReal"/>
    [Description("Filters and selects timestamped messages from the Channel3FlowReal register.")]
    public partial class TimestampedChannel3FlowReal
    {
        /// <summary>
        /// Represents the address of the <see cref="Channel3FlowReal"/> register. This field is constant.
        /// </summary>
        public const int Address = Channel3FlowReal.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="Channel3FlowReal"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<float> GetPayload(HarpMessage message)
        {
            return Channel3FlowReal.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that flow value read from channel 4 - flowmeter 4 [ml/min].
    /// </summary>
    [Description("Flow value read from channel 4 - flowmeter 4 [ml/min].")]
    public partial class Channel4FlowReal
    {
        /// <summary>
        /// Represents the address of the <see cref="Channel4FlowReal"/> register. This field is constant.
        /// </summary>
        public const int Address = 51;

        /// <summary>
        /// Represents the payload type of the <see cref="Channel4FlowReal"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.Float;

        /// <summary>
        /// Represents the length of the <see cref="Channel4FlowReal"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="Channel4FlowReal"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static float GetPayload(HarpMessage message)
        {
            return message.GetPayloadSingle();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="Channel4FlowReal"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<float> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadSingle();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="Channel4FlowReal"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Channel4FlowReal"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, float value)
        {
            return HarpMessage.FromSingle(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="Channel4FlowReal"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Channel4FlowReal"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, float value)
        {
            return HarpMessage.FromSingle(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// Channel4FlowReal register.
    /// </summary>
    /// <seealso cref="Channel4FlowReal"/>
    [Description("Filters and selects timestamped messages from the Channel4FlowReal register.")]
    public partial class TimestampedChannel4FlowReal
    {
        /// <summary>
        /// Represents the address of the <see cref="Channel4FlowReal"/> register. This field is constant.
        /// </summary>
        public const int Address = Channel4FlowReal.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="Channel4FlowReal"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<float> GetPayload(HarpMessage message)
        {
            return Channel4FlowReal.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that duty cycle for proportional valve 0 [%].
    /// </summary>
    [Description("Duty cycle for proportional valve 0 [%].")]
    public partial class Channel0DutyCycle
    {
        /// <summary>
        /// Represents the address of the <see cref="Channel0DutyCycle"/> register. This field is constant.
        /// </summary>
        public const int Address = 57;

        /// <summary>
        /// Represents the payload type of the <see cref="Channel0DutyCycle"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.Float;

        /// <summary>
        /// Represents the length of the <see cref="Channel0DutyCycle"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="Channel0DutyCycle"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static float GetPayload(HarpMessage message)
        {
            return message.GetPayloadSingle();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="Channel0DutyCycle"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<float> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadSingle();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="Channel0DutyCycle"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Channel0DutyCycle"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, float value)
        {
            return HarpMessage.FromSingle(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="Channel0DutyCycle"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Channel0DutyCycle"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, float value)
        {
            return HarpMessage.FromSingle(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// Channel0DutyCycle register.
    /// </summary>
    /// <seealso cref="Channel0DutyCycle"/>
    [Description("Filters and selects timestamped messages from the Channel0DutyCycle register.")]
    public partial class TimestampedChannel0DutyCycle
    {
        /// <summary>
        /// Represents the address of the <see cref="Channel0DutyCycle"/> register. This field is constant.
        /// </summary>
        public const int Address = Channel0DutyCycle.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="Channel0DutyCycle"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<float> GetPayload(HarpMessage message)
        {
            return Channel0DutyCycle.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that duty cycle for proportional valve 1 [%].
    /// </summary>
    [Description("Duty cycle for proportional valve 1 [%].")]
    public partial class Channel1DutyCycle
    {
        /// <summary>
        /// Represents the address of the <see cref="Channel1DutyCycle"/> register. This field is constant.
        /// </summary>
        public const int Address = 58;

        /// <summary>
        /// Represents the payload type of the <see cref="Channel1DutyCycle"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.Float;

        /// <summary>
        /// Represents the length of the <see cref="Channel1DutyCycle"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="Channel1DutyCycle"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static float GetPayload(HarpMessage message)
        {
            return message.GetPayloadSingle();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="Channel1DutyCycle"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<float> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadSingle();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="Channel1DutyCycle"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Channel1DutyCycle"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, float value)
        {
            return HarpMessage.FromSingle(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="Channel1DutyCycle"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Channel1DutyCycle"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, float value)
        {
            return HarpMessage.FromSingle(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// Channel1DutyCycle register.
    /// </summary>
    /// <seealso cref="Channel1DutyCycle"/>
    [Description("Filters and selects timestamped messages from the Channel1DutyCycle register.")]
    public partial class TimestampedChannel1DutyCycle
    {
        /// <summary>
        /// Represents the address of the <see cref="Channel1DutyCycle"/> register. This field is constant.
        /// </summary>
        public const int Address = Channel1DutyCycle.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="Channel1DutyCycle"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<float> GetPayload(HarpMessage message)
        {
            return Channel1DutyCycle.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that duty cycle for proportional valve 2 [%].
    /// </summary>
    [Description("Duty cycle for proportional valve 2 [%].")]
    public partial class Channel2DutyCycle
    {
        /// <summary>
        /// Represents the address of the <see cref="Channel2DutyCycle"/> register. This field is constant.
        /// </summary>
        public const int Address = 59;

        /// <summary>
        /// Represents the payload type of the <see cref="Channel2DutyCycle"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.Float;

        /// <summary>
        /// Represents the length of the <see cref="Channel2DutyCycle"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="Channel2DutyCycle"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static float GetPayload(HarpMessage message)
        {
            return message.GetPayloadSingle();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="Channel2DutyCycle"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<float> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadSingle();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="Channel2DutyCycle"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Channel2DutyCycle"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, float value)
        {
            return HarpMessage.FromSingle(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="Channel2DutyCycle"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Channel2DutyCycle"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, float value)
        {
            return HarpMessage.FromSingle(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// Channel2DutyCycle register.
    /// </summary>
    /// <seealso cref="Channel2DutyCycle"/>
    [Description("Filters and selects timestamped messages from the Channel2DutyCycle register.")]
    public partial class TimestampedChannel2DutyCycle
    {
        /// <summary>
        /// Represents the address of the <see cref="Channel2DutyCycle"/> register. This field is constant.
        /// </summary>
        public const int Address = Channel2DutyCycle.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="Channel2DutyCycle"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<float> GetPayload(HarpMessage message)
        {
            return Channel2DutyCycle.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that duty cycle for proportional valve 3 [%].
    /// </summary>
    [Description("Duty cycle for proportional valve 3 [%].")]
    public partial class Channel3DutyCycle
    {
        /// <summary>
        /// Represents the address of the <see cref="Channel3DutyCycle"/> register. This field is constant.
        /// </summary>
        public const int Address = 60;

        /// <summary>
        /// Represents the payload type of the <see cref="Channel3DutyCycle"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.Float;

        /// <summary>
        /// Represents the length of the <see cref="Channel3DutyCycle"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="Channel3DutyCycle"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static float GetPayload(HarpMessage message)
        {
            return message.GetPayloadSingle();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="Channel3DutyCycle"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<float> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadSingle();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="Channel3DutyCycle"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Channel3DutyCycle"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, float value)
        {
            return HarpMessage.FromSingle(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="Channel3DutyCycle"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Channel3DutyCycle"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, float value)
        {
            return HarpMessage.FromSingle(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// Channel3DutyCycle register.
    /// </summary>
    /// <seealso cref="Channel3DutyCycle"/>
    [Description("Filters and selects timestamped messages from the Channel3DutyCycle register.")]
    public partial class TimestampedChannel3DutyCycle
    {
        /// <summary>
        /// Represents the address of the <see cref="Channel3DutyCycle"/> register. This field is constant.
        /// </summary>
        public const int Address = Channel3DutyCycle.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="Channel3DutyCycle"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<float> GetPayload(HarpMessage message)
        {
            return Channel3DutyCycle.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that duty cycle for proportional valve 4 [%].
    /// </summary>
    [Description("Duty cycle for proportional valve 4 [%].")]
    public partial class Channel4DutyCycle
    {
        /// <summary>
        /// Represents the address of the <see cref="Channel4DutyCycle"/> register. This field is constant.
        /// </summary>
        public const int Address = 61;

        /// <summary>
        /// Represents the payload type of the <see cref="Channel4DutyCycle"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.Float;

        /// <summary>
        /// Represents the length of the <see cref="Channel4DutyCycle"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="Channel4DutyCycle"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static float GetPayload(HarpMessage message)
        {
            return message.GetPayloadSingle();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="Channel4DutyCycle"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<float> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadSingle();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="Channel4DutyCycle"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Channel4DutyCycle"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, float value)
        {
            return HarpMessage.FromSingle(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="Channel4DutyCycle"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Channel4DutyCycle"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, float value)
        {
            return HarpMessage.FromSingle(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// Channel4DutyCycle register.
    /// </summary>
    /// <seealso cref="Channel4DutyCycle"/>
    [Description("Filters and selects timestamped messages from the Channel4DutyCycle register.")]
    public partial class TimestampedChannel4DutyCycle
    {
        /// <summary>
        /// Represents the address of the <see cref="Channel4DutyCycle"/> register. This field is constant.
        /// </summary>
        public const int Address = Channel4DutyCycle.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="Channel4DutyCycle"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<float> GetPayload(HarpMessage message)
        {
            return Channel4DutyCycle.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that set the correspondent output.
    /// </summary>
    [Description("Set the correspondent output.")]
    public partial class DigitalOutputSet
    {
        /// <summary>
        /// Represents the address of the <see cref="DigitalOutputSet"/> register. This field is constant.
        /// </summary>
        public const int Address = 62;

        /// <summary>
        /// Represents the payload type of the <see cref="DigitalOutputSet"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="DigitalOutputSet"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="DigitalOutputSet"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static DigitalOutputs GetPayload(HarpMessage message)
        {
            return (DigitalOutputs)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="DigitalOutputSet"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<DigitalOutputs> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((DigitalOutputs)payload.Value, payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="DigitalOutputSet"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="DigitalOutputSet"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, DigitalOutputs value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="DigitalOutputSet"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="DigitalOutputSet"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, DigitalOutputs value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// DigitalOutputSet register.
    /// </summary>
    /// <seealso cref="DigitalOutputSet"/>
    [Description("Filters and selects timestamped messages from the DigitalOutputSet register.")]
    public partial class TimestampedDigitalOutputSet
    {
        /// <summary>
        /// Represents the address of the <see cref="DigitalOutputSet"/> register. This field is constant.
        /// </summary>
        public const int Address = DigitalOutputSet.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="DigitalOutputSet"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<DigitalOutputs> GetPayload(HarpMessage message)
        {
            return DigitalOutputSet.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that clear the correspondent output.
    /// </summary>
    [Description("Clear the correspondent output.")]
    public partial class DigitalOutputClear
    {
        /// <summary>
        /// Represents the address of the <see cref="DigitalOutputClear"/> register. This field is constant.
        /// </summary>
        public const int Address = 63;

        /// <summary>
        /// Represents the payload type of the <see cref="DigitalOutputClear"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="DigitalOutputClear"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="DigitalOutputClear"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static DigitalOutputs GetPayload(HarpMessage message)
        {
            return (DigitalOutputs)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="DigitalOutputClear"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<DigitalOutputs> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((DigitalOutputs)payload.Value, payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="DigitalOutputClear"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="DigitalOutputClear"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, DigitalOutputs value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="DigitalOutputClear"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="DigitalOutputClear"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, DigitalOutputs value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// DigitalOutputClear register.
    /// </summary>
    /// <seealso cref="DigitalOutputClear"/>
    [Description("Filters and selects timestamped messages from the DigitalOutputClear register.")]
    public partial class TimestampedDigitalOutputClear
    {
        /// <summary>
        /// Represents the address of the <see cref="DigitalOutputClear"/> register. This field is constant.
        /// </summary>
        public const int Address = DigitalOutputClear.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="DigitalOutputClear"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<DigitalOutputs> GetPayload(HarpMessage message)
        {
            return DigitalOutputClear.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that toggle the correspondent output.
    /// </summary>
    [Description("Toggle the correspondent output.")]
    public partial class DigitalOutputToggle
    {
        /// <summary>
        /// Represents the address of the <see cref="DigitalOutputToggle"/> register. This field is constant.
        /// </summary>
        public const int Address = 64;

        /// <summary>
        /// Represents the payload type of the <see cref="DigitalOutputToggle"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="DigitalOutputToggle"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="DigitalOutputToggle"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static DigitalOutputs GetPayload(HarpMessage message)
        {
            return (DigitalOutputs)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="DigitalOutputToggle"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<DigitalOutputs> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((DigitalOutputs)payload.Value, payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="DigitalOutputToggle"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="DigitalOutputToggle"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, DigitalOutputs value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="DigitalOutputToggle"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="DigitalOutputToggle"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, DigitalOutputs value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// DigitalOutputToggle register.
    /// </summary>
    /// <seealso cref="DigitalOutputToggle"/>
    [Description("Filters and selects timestamped messages from the DigitalOutputToggle register.")]
    public partial class TimestampedDigitalOutputToggle
    {
        /// <summary>
        /// Represents the address of the <see cref="DigitalOutputToggle"/> register. This field is constant.
        /// </summary>
        public const int Address = DigitalOutputToggle.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="DigitalOutputToggle"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<DigitalOutputs> GetPayload(HarpMessage message)
        {
            return DigitalOutputToggle.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that control the correspondent output.
    /// </summary>
    [Description("Control the correspondent output.")]
    public partial class DigitalOutputState
    {
        /// <summary>
        /// Represents the address of the <see cref="DigitalOutputState"/> register. This field is constant.
        /// </summary>
        public const int Address = 65;

        /// <summary>
        /// Represents the payload type of the <see cref="DigitalOutputState"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="DigitalOutputState"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="DigitalOutputState"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static DigitalOutputs GetPayload(HarpMessage message)
        {
            return (DigitalOutputs)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="DigitalOutputState"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<DigitalOutputs> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((DigitalOutputs)payload.Value, payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="DigitalOutputState"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="DigitalOutputState"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, DigitalOutputs value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="DigitalOutputState"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="DigitalOutputState"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, DigitalOutputs value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// DigitalOutputState register.
    /// </summary>
    /// <seealso cref="DigitalOutputState"/>
    [Description("Filters and selects timestamped messages from the DigitalOutputState register.")]
    public partial class TimestampedDigitalOutputState
    {
        /// <summary>
        /// Represents the address of the <see cref="DigitalOutputState"/> register. This field is constant.
        /// </summary>
        public const int Address = DigitalOutputState.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="DigitalOutputState"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<DigitalOutputs> GetPayload(HarpMessage message)
        {
            return DigitalOutputState.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that enable pulse mode for valves.
    /// </summary>
    [Description("Enable pulse mode for valves.")]
    public partial class EnableValvesPulse
    {
        /// <summary>
        /// Represents the address of the <see cref="EnableValvesPulse"/> register. This field is constant.
        /// </summary>
        public const int Address = 66;

        /// <summary>
        /// Represents the payload type of the <see cref="EnableValvesPulse"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="EnableValvesPulse"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="EnableValvesPulse"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static Valves GetPayload(HarpMessage message)
        {
            return (Valves)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="EnableValvesPulse"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<Valves> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((Valves)payload.Value, payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="EnableValvesPulse"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="EnableValvesPulse"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, Valves value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="EnableValvesPulse"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="EnableValvesPulse"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, Valves value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// EnableValvesPulse register.
    /// </summary>
    /// <seealso cref="EnableValvesPulse"/>
    [Description("Filters and selects timestamped messages from the EnableValvesPulse register.")]
    public partial class TimestampedEnableValvesPulse
    {
        /// <summary>
        /// Represents the address of the <see cref="EnableValvesPulse"/> register. This field is constant.
        /// </summary>
        public const int Address = EnableValvesPulse.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="EnableValvesPulse"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<Valves> GetPayload(HarpMessage message)
        {
            return EnableValvesPulse.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that set the correspondent valve.
    /// </summary>
    [Description("Set the correspondent valve.")]
    public partial class ValvesSet
    {
        /// <summary>
        /// Represents the address of the <see cref="ValvesSet"/> register. This field is constant.
        /// </summary>
        public const int Address = 67;

        /// <summary>
        /// Represents the payload type of the <see cref="ValvesSet"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="ValvesSet"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="ValvesSet"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static Valves GetPayload(HarpMessage message)
        {
            return (Valves)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="ValvesSet"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<Valves> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((Valves)payload.Value, payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="ValvesSet"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="ValvesSet"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, Valves value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="ValvesSet"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="ValvesSet"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, Valves value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// ValvesSet register.
    /// </summary>
    /// <seealso cref="ValvesSet"/>
    [Description("Filters and selects timestamped messages from the ValvesSet register.")]
    public partial class TimestampedValvesSet
    {
        /// <summary>
        /// Represents the address of the <see cref="ValvesSet"/> register. This field is constant.
        /// </summary>
        public const int Address = ValvesSet.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="ValvesSet"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<Valves> GetPayload(HarpMessage message)
        {
            return ValvesSet.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that clear the correspondent valve.
    /// </summary>
    [Description("Clear the correspondent valve.")]
    public partial class ValvesClear
    {
        /// <summary>
        /// Represents the address of the <see cref="ValvesClear"/> register. This field is constant.
        /// </summary>
        public const int Address = 68;

        /// <summary>
        /// Represents the payload type of the <see cref="ValvesClear"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="ValvesClear"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="ValvesClear"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static Valves GetPayload(HarpMessage message)
        {
            return (Valves)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="ValvesClear"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<Valves> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((Valves)payload.Value, payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="ValvesClear"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="ValvesClear"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, Valves value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="ValvesClear"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="ValvesClear"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, Valves value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// ValvesClear register.
    /// </summary>
    /// <seealso cref="ValvesClear"/>
    [Description("Filters and selects timestamped messages from the ValvesClear register.")]
    public partial class TimestampedValvesClear
    {
        /// <summary>
        /// Represents the address of the <see cref="ValvesClear"/> register. This field is constant.
        /// </summary>
        public const int Address = ValvesClear.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="ValvesClear"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<Valves> GetPayload(HarpMessage message)
        {
            return ValvesClear.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that toggle the correspondent valve.
    /// </summary>
    [Description("Toggle the correspondent valve.")]
    public partial class ValvesToggle
    {
        /// <summary>
        /// Represents the address of the <see cref="ValvesToggle"/> register. This field is constant.
        /// </summary>
        public const int Address = 69;

        /// <summary>
        /// Represents the payload type of the <see cref="ValvesToggle"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="ValvesToggle"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="ValvesToggle"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static Valves GetPayload(HarpMessage message)
        {
            return (Valves)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="ValvesToggle"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<Valves> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((Valves)payload.Value, payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="ValvesToggle"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="ValvesToggle"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, Valves value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="ValvesToggle"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="ValvesToggle"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, Valves value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// ValvesToggle register.
    /// </summary>
    /// <seealso cref="ValvesToggle"/>
    [Description("Filters and selects timestamped messages from the ValvesToggle register.")]
    public partial class TimestampedValvesToggle
    {
        /// <summary>
        /// Represents the address of the <see cref="ValvesToggle"/> register. This field is constant.
        /// </summary>
        public const int Address = ValvesToggle.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="ValvesToggle"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<Valves> GetPayload(HarpMessage message)
        {
            return ValvesToggle.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that control the correspondent valve.
    /// </summary>
    [Description("Control the correspondent valve.")]
    public partial class ValvesState
    {
        /// <summary>
        /// Represents the address of the <see cref="ValvesState"/> register. This field is constant.
        /// </summary>
        public const int Address = 70;

        /// <summary>
        /// Represents the payload type of the <see cref="ValvesState"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="ValvesState"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="ValvesState"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static Valves GetPayload(HarpMessage message)
        {
            return (Valves)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="ValvesState"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<Valves> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((Valves)payload.Value, payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="ValvesState"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="ValvesState"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, Valves value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="ValvesState"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="ValvesState"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, Valves value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// ValvesState register.
    /// </summary>
    /// <seealso cref="ValvesState"/>
    [Description("Filters and selects timestamped messages from the ValvesState register.")]
    public partial class TimestampedValvesState
    {
        /// <summary>
        /// Represents the address of the <see cref="ValvesState"/> register. This field is constant.
        /// </summary>
        public const int Address = ValvesState.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="ValvesState"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<Valves> GetPayload(HarpMessage message)
        {
            return ValvesState.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that valve 0 pulse duration [1:65535] ms.
    /// </summary>
    [Description("Valve 0 pulse duration [1:65535] ms.")]
    public partial class PulseValve0
    {
        /// <summary>
        /// Represents the address of the <see cref="PulseValve0"/> register. This field is constant.
        /// </summary>
        public const int Address = 71;

        /// <summary>
        /// Represents the payload type of the <see cref="PulseValve0"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U16;

        /// <summary>
        /// Represents the length of the <see cref="PulseValve0"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="PulseValve0"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static ushort GetPayload(HarpMessage message)
        {
            return message.GetPayloadUInt16();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="PulseValve0"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadUInt16();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="PulseValve0"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="PulseValve0"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, ushort value)
        {
            return HarpMessage.FromUInt16(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="PulseValve0"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="PulseValve0"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, ushort value)
        {
            return HarpMessage.FromUInt16(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// PulseValve0 register.
    /// </summary>
    /// <seealso cref="PulseValve0"/>
    [Description("Filters and selects timestamped messages from the PulseValve0 register.")]
    public partial class TimestampedPulseValve0
    {
        /// <summary>
        /// Represents the address of the <see cref="PulseValve0"/> register. This field is constant.
        /// </summary>
        public const int Address = PulseValve0.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="PulseValve0"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort> GetPayload(HarpMessage message)
        {
            return PulseValve0.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that valve 1 pulse duration [1:65535] ms.
    /// </summary>
    [Description("Valve 1 pulse duration [1:65535] ms.")]
    public partial class PulseValve1
    {
        /// <summary>
        /// Represents the address of the <see cref="PulseValve1"/> register. This field is constant.
        /// </summary>
        public const int Address = 72;

        /// <summary>
        /// Represents the payload type of the <see cref="PulseValve1"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U16;

        /// <summary>
        /// Represents the length of the <see cref="PulseValve1"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="PulseValve1"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static ushort GetPayload(HarpMessage message)
        {
            return message.GetPayloadUInt16();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="PulseValve1"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadUInt16();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="PulseValve1"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="PulseValve1"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, ushort value)
        {
            return HarpMessage.FromUInt16(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="PulseValve1"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="PulseValve1"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, ushort value)
        {
            return HarpMessage.FromUInt16(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// PulseValve1 register.
    /// </summary>
    /// <seealso cref="PulseValve1"/>
    [Description("Filters and selects timestamped messages from the PulseValve1 register.")]
    public partial class TimestampedPulseValve1
    {
        /// <summary>
        /// Represents the address of the <see cref="PulseValve1"/> register. This field is constant.
        /// </summary>
        public const int Address = PulseValve1.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="PulseValve1"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort> GetPayload(HarpMessage message)
        {
            return PulseValve1.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that valve 2 pulse duration [1:65535] ms.
    /// </summary>
    [Description("Valve 2 pulse duration [1:65535] ms.")]
    public partial class PulseValve2
    {
        /// <summary>
        /// Represents the address of the <see cref="PulseValve2"/> register. This field is constant.
        /// </summary>
        public const int Address = 73;

        /// <summary>
        /// Represents the payload type of the <see cref="PulseValve2"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U16;

        /// <summary>
        /// Represents the length of the <see cref="PulseValve2"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="PulseValve2"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static ushort GetPayload(HarpMessage message)
        {
            return message.GetPayloadUInt16();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="PulseValve2"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadUInt16();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="PulseValve2"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="PulseValve2"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, ushort value)
        {
            return HarpMessage.FromUInt16(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="PulseValve2"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="PulseValve2"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, ushort value)
        {
            return HarpMessage.FromUInt16(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// PulseValve2 register.
    /// </summary>
    /// <seealso cref="PulseValve2"/>
    [Description("Filters and selects timestamped messages from the PulseValve2 register.")]
    public partial class TimestampedPulseValve2
    {
        /// <summary>
        /// Represents the address of the <see cref="PulseValve2"/> register. This field is constant.
        /// </summary>
        public const int Address = PulseValve2.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="PulseValve2"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort> GetPayload(HarpMessage message)
        {
            return PulseValve2.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that valve 3 pulse duration [1:65535] ms.
    /// </summary>
    [Description("Valve 3 pulse duration [1:65535] ms.")]
    public partial class PulseValve3
    {
        /// <summary>
        /// Represents the address of the <see cref="PulseValve3"/> register. This field is constant.
        /// </summary>
        public const int Address = 74;

        /// <summary>
        /// Represents the payload type of the <see cref="PulseValve3"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U16;

        /// <summary>
        /// Represents the length of the <see cref="PulseValve3"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="PulseValve3"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static ushort GetPayload(HarpMessage message)
        {
            return message.GetPayloadUInt16();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="PulseValve3"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadUInt16();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="PulseValve3"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="PulseValve3"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, ushort value)
        {
            return HarpMessage.FromUInt16(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="PulseValve3"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="PulseValve3"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, ushort value)
        {
            return HarpMessage.FromUInt16(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// PulseValve3 register.
    /// </summary>
    /// <seealso cref="PulseValve3"/>
    [Description("Filters and selects timestamped messages from the PulseValve3 register.")]
    public partial class TimestampedPulseValve3
    {
        /// <summary>
        /// Represents the address of the <see cref="PulseValve3"/> register. This field is constant.
        /// </summary>
        public const int Address = PulseValve3.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="PulseValve3"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort> GetPayload(HarpMessage message)
        {
            return PulseValve3.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that end valve 0 pulse duration [1:65535] ms.
    /// </summary>
    [Description("End valve 0 pulse duration [1:65535] ms.")]
    public partial class PulseEndvalve0
    {
        /// <summary>
        /// Represents the address of the <see cref="PulseEndvalve0"/> register. This field is constant.
        /// </summary>
        public const int Address = 75;

        /// <summary>
        /// Represents the payload type of the <see cref="PulseEndvalve0"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U16;

        /// <summary>
        /// Represents the length of the <see cref="PulseEndvalve0"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="PulseEndvalve0"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static ushort GetPayload(HarpMessage message)
        {
            return message.GetPayloadUInt16();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="PulseEndvalve0"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadUInt16();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="PulseEndvalve0"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="PulseEndvalve0"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, ushort value)
        {
            return HarpMessage.FromUInt16(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="PulseEndvalve0"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="PulseEndvalve0"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, ushort value)
        {
            return HarpMessage.FromUInt16(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// PulseEndvalve0 register.
    /// </summary>
    /// <seealso cref="PulseEndvalve0"/>
    [Description("Filters and selects timestamped messages from the PulseEndvalve0 register.")]
    public partial class TimestampedPulseEndvalve0
    {
        /// <summary>
        /// Represents the address of the <see cref="PulseEndvalve0"/> register. This field is constant.
        /// </summary>
        public const int Address = PulseEndvalve0.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="PulseEndvalve0"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort> GetPayload(HarpMessage message)
        {
            return PulseEndvalve0.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that end valve 1 pulse duration [1:65535] ms.
    /// </summary>
    [Description("End valve 1 pulse duration [1:65535] ms.")]
    public partial class PulseEndvalve1
    {
        /// <summary>
        /// Represents the address of the <see cref="PulseEndvalve1"/> register. This field is constant.
        /// </summary>
        public const int Address = 76;

        /// <summary>
        /// Represents the payload type of the <see cref="PulseEndvalve1"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U16;

        /// <summary>
        /// Represents the length of the <see cref="PulseEndvalve1"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="PulseEndvalve1"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static ushort GetPayload(HarpMessage message)
        {
            return message.GetPayloadUInt16();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="PulseEndvalve1"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadUInt16();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="PulseEndvalve1"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="PulseEndvalve1"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, ushort value)
        {
            return HarpMessage.FromUInt16(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="PulseEndvalve1"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="PulseEndvalve1"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, ushort value)
        {
            return HarpMessage.FromUInt16(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// PulseEndvalve1 register.
    /// </summary>
    /// <seealso cref="PulseEndvalve1"/>
    [Description("Filters and selects timestamped messages from the PulseEndvalve1 register.")]
    public partial class TimestampedPulseEndvalve1
    {
        /// <summary>
        /// Represents the address of the <see cref="PulseEndvalve1"/> register. This field is constant.
        /// </summary>
        public const int Address = PulseEndvalve1.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="PulseEndvalve1"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort> GetPayload(HarpMessage message)
        {
            return PulseEndvalve1.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that dummy valve pulse duration [1:65535] ms.
    /// </summary>
    [Description("Dummy valve pulse duration [1:65535] ms.")]
    public partial class PulseDummyvalve
    {
        /// <summary>
        /// Represents the address of the <see cref="PulseDummyvalve"/> register. This field is constant.
        /// </summary>
        public const int Address = 77;

        /// <summary>
        /// Represents the payload type of the <see cref="PulseDummyvalve"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U16;

        /// <summary>
        /// Represents the length of the <see cref="PulseDummyvalve"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="PulseDummyvalve"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static ushort GetPayload(HarpMessage message)
        {
            return message.GetPayloadUInt16();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="PulseDummyvalve"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadUInt16();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="PulseDummyvalve"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="PulseDummyvalve"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, ushort value)
        {
            return HarpMessage.FromUInt16(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="PulseDummyvalve"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="PulseDummyvalve"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, ushort value)
        {
            return HarpMessage.FromUInt16(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// PulseDummyvalve register.
    /// </summary>
    /// <seealso cref="PulseDummyvalve"/>
    [Description("Filters and selects timestamped messages from the PulseDummyvalve register.")]
    public partial class TimestampedPulseDummyvalve
    {
        /// <summary>
        /// Represents the address of the <see cref="PulseDummyvalve"/> register. This field is constant.
        /// </summary>
        public const int Address = PulseDummyvalve.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="PulseDummyvalve"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort> GetPayload(HarpMessage message)
        {
            return PulseDummyvalve.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that configuration of the digital output 0 (DOUT0).
    /// </summary>
    [Description("Configuration of the digital output 0 (DOUT0).")]
    public partial class DO0Sync
    {
        /// <summary>
        /// Represents the address of the <see cref="DO0Sync"/> register. This field is constant.
        /// </summary>
        public const int Address = 78;

        /// <summary>
        /// Represents the payload type of the <see cref="DO0Sync"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="DO0Sync"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="DO0Sync"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static DO0SyncConfig GetPayload(HarpMessage message)
        {
            return (DO0SyncConfig)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="DO0Sync"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<DO0SyncConfig> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((DO0SyncConfig)payload.Value, payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="DO0Sync"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="DO0Sync"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, DO0SyncConfig value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="DO0Sync"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="DO0Sync"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, DO0SyncConfig value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// DO0Sync register.
    /// </summary>
    /// <seealso cref="DO0Sync"/>
    [Description("Filters and selects timestamped messages from the DO0Sync register.")]
    public partial class TimestampedDO0Sync
    {
        /// <summary>
        /// Represents the address of the <see cref="DO0Sync"/> register. This field is constant.
        /// </summary>
        public const int Address = DO0Sync.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="DO0Sync"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<DO0SyncConfig> GetPayload(HarpMessage message)
        {
            return DO0Sync.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that configuration of the digital output 1 (DOUT1).
    /// </summary>
    [Description("Configuration of the digital output 1 (DOUT1).")]
    public partial class DO1Sync
    {
        /// <summary>
        /// Represents the address of the <see cref="DO1Sync"/> register. This field is constant.
        /// </summary>
        public const int Address = 79;

        /// <summary>
        /// Represents the payload type of the <see cref="DO1Sync"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="DO1Sync"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="DO1Sync"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static DO1SyncConfig GetPayload(HarpMessage message)
        {
            return (DO1SyncConfig)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="DO1Sync"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<DO1SyncConfig> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((DO1SyncConfig)payload.Value, payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="DO1Sync"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="DO1Sync"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, DO1SyncConfig value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="DO1Sync"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="DO1Sync"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, DO1SyncConfig value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// DO1Sync register.
    /// </summary>
    /// <seealso cref="DO1Sync"/>
    [Description("Filters and selects timestamped messages from the DO1Sync register.")]
    public partial class TimestampedDO1Sync
    {
        /// <summary>
        /// Represents the address of the <see cref="DO1Sync"/> register. This field is constant.
        /// </summary>
        public const int Address = DO1Sync.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="DO1Sync"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<DO1SyncConfig> GetPayload(HarpMessage message)
        {
            return DO1Sync.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that configuration of the digital input pin 0 (DIN0).
    /// </summary>
    [Description("Configuration of the digital input pin 0 (DIN0).")]
    public partial class DI0Trigger
    {
        /// <summary>
        /// Represents the address of the <see cref="DI0Trigger"/> register. This field is constant.
        /// </summary>
        public const int Address = 80;

        /// <summary>
        /// Represents the payload type of the <see cref="DI0Trigger"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="DI0Trigger"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="DI0Trigger"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static DI0TriggerConfig GetPayload(HarpMessage message)
        {
            return (DI0TriggerConfig)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="DI0Trigger"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<DI0TriggerConfig> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((DI0TriggerConfig)payload.Value, payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="DI0Trigger"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="DI0Trigger"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, DI0TriggerConfig value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="DI0Trigger"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="DI0Trigger"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, DI0TriggerConfig value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// DI0Trigger register.
    /// </summary>
    /// <seealso cref="DI0Trigger"/>
    [Description("Filters and selects timestamped messages from the DI0Trigger register.")]
    public partial class TimestampedDI0Trigger
    {
        /// <summary>
        /// Represents the address of the <see cref="DI0Trigger"/> register. This field is constant.
        /// </summary>
        public const int Address = DI0Trigger.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="DI0Trigger"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<DI0TriggerConfig> GetPayload(HarpMessage message)
        {
            return DI0Trigger.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that mimic valve 0.
    /// </summary>
    [Description("Mimic valve 0.")]
    public partial class MimicValve0
    {
        /// <summary>
        /// Represents the address of the <see cref="MimicValve0"/> register. This field is constant.
        /// </summary>
        public const int Address = 81;

        /// <summary>
        /// Represents the payload type of the <see cref="MimicValve0"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="MimicValve0"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="MimicValve0"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static MimicOuputs GetPayload(HarpMessage message)
        {
            return (MimicOuputs)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="MimicValve0"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<MimicOuputs> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((MimicOuputs)payload.Value, payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="MimicValve0"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="MimicValve0"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, MimicOuputs value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="MimicValve0"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="MimicValve0"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, MimicOuputs value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// MimicValve0 register.
    /// </summary>
    /// <seealso cref="MimicValve0"/>
    [Description("Filters and selects timestamped messages from the MimicValve0 register.")]
    public partial class TimestampedMimicValve0
    {
        /// <summary>
        /// Represents the address of the <see cref="MimicValve0"/> register. This field is constant.
        /// </summary>
        public const int Address = MimicValve0.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="MimicValve0"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<MimicOuputs> GetPayload(HarpMessage message)
        {
            return MimicValve0.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that mimic valve 1.
    /// </summary>
    [Description("Mimic valve 1.")]
    public partial class MimicValve1
    {
        /// <summary>
        /// Represents the address of the <see cref="MimicValve1"/> register. This field is constant.
        /// </summary>
        public const int Address = 82;

        /// <summary>
        /// Represents the payload type of the <see cref="MimicValve1"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="MimicValve1"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="MimicValve1"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static MimicOuputs GetPayload(HarpMessage message)
        {
            return (MimicOuputs)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="MimicValve1"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<MimicOuputs> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((MimicOuputs)payload.Value, payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="MimicValve1"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="MimicValve1"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, MimicOuputs value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="MimicValve1"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="MimicValve1"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, MimicOuputs value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// MimicValve1 register.
    /// </summary>
    /// <seealso cref="MimicValve1"/>
    [Description("Filters and selects timestamped messages from the MimicValve1 register.")]
    public partial class TimestampedMimicValve1
    {
        /// <summary>
        /// Represents the address of the <see cref="MimicValve1"/> register. This field is constant.
        /// </summary>
        public const int Address = MimicValve1.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="MimicValve1"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<MimicOuputs> GetPayload(HarpMessage message)
        {
            return MimicValve1.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that mimic valve 2.
    /// </summary>
    [Description("Mimic valve 2.")]
    public partial class MimicValve2
    {
        /// <summary>
        /// Represents the address of the <see cref="MimicValve2"/> register. This field is constant.
        /// </summary>
        public const int Address = 83;

        /// <summary>
        /// Represents the payload type of the <see cref="MimicValve2"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="MimicValve2"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="MimicValve2"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static MimicOuputs GetPayload(HarpMessage message)
        {
            return (MimicOuputs)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="MimicValve2"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<MimicOuputs> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((MimicOuputs)payload.Value, payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="MimicValve2"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="MimicValve2"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, MimicOuputs value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="MimicValve2"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="MimicValve2"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, MimicOuputs value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// MimicValve2 register.
    /// </summary>
    /// <seealso cref="MimicValve2"/>
    [Description("Filters and selects timestamped messages from the MimicValve2 register.")]
    public partial class TimestampedMimicValve2
    {
        /// <summary>
        /// Represents the address of the <see cref="MimicValve2"/> register. This field is constant.
        /// </summary>
        public const int Address = MimicValve2.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="MimicValve2"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<MimicOuputs> GetPayload(HarpMessage message)
        {
            return MimicValve2.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that mimic valve 3.
    /// </summary>
    [Description("Mimic valve 3.")]
    public partial class MimicValve3
    {
        /// <summary>
        /// Represents the address of the <see cref="MimicValve3"/> register. This field is constant.
        /// </summary>
        public const int Address = 84;

        /// <summary>
        /// Represents the payload type of the <see cref="MimicValve3"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="MimicValve3"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="MimicValve3"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static MimicOuputs GetPayload(HarpMessage message)
        {
            return (MimicOuputs)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="MimicValve3"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<MimicOuputs> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((MimicOuputs)payload.Value, payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="MimicValve3"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="MimicValve3"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, MimicOuputs value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="MimicValve3"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="MimicValve3"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, MimicOuputs value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// MimicValve3 register.
    /// </summary>
    /// <seealso cref="MimicValve3"/>
    [Description("Filters and selects timestamped messages from the MimicValve3 register.")]
    public partial class TimestampedMimicValve3
    {
        /// <summary>
        /// Represents the address of the <see cref="MimicValve3"/> register. This field is constant.
        /// </summary>
        public const int Address = MimicValve3.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="MimicValve3"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<MimicOuputs> GetPayload(HarpMessage message)
        {
            return MimicValve3.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that mimic end valve 0.
    /// </summary>
    [Description("Mimic end valve 0.")]
    public partial class MimicEndvalve0
    {
        /// <summary>
        /// Represents the address of the <see cref="MimicEndvalve0"/> register. This field is constant.
        /// </summary>
        public const int Address = 85;

        /// <summary>
        /// Represents the payload type of the <see cref="MimicEndvalve0"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="MimicEndvalve0"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="MimicEndvalve0"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static MimicOuputs GetPayload(HarpMessage message)
        {
            return (MimicOuputs)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="MimicEndvalve0"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<MimicOuputs> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((MimicOuputs)payload.Value, payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="MimicEndvalve0"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="MimicEndvalve0"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, MimicOuputs value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="MimicEndvalve0"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="MimicEndvalve0"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, MimicOuputs value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// MimicEndvalve0 register.
    /// </summary>
    /// <seealso cref="MimicEndvalve0"/>
    [Description("Filters and selects timestamped messages from the MimicEndvalve0 register.")]
    public partial class TimestampedMimicEndvalve0
    {
        /// <summary>
        /// Represents the address of the <see cref="MimicEndvalve0"/> register. This field is constant.
        /// </summary>
        public const int Address = MimicEndvalve0.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="MimicEndvalve0"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<MimicOuputs> GetPayload(HarpMessage message)
        {
            return MimicEndvalve0.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that mimic end valve 1.
    /// </summary>
    [Description("Mimic end valve 1.")]
    public partial class MimicEndvalve1
    {
        /// <summary>
        /// Represents the address of the <see cref="MimicEndvalve1"/> register. This field is constant.
        /// </summary>
        public const int Address = 86;

        /// <summary>
        /// Represents the payload type of the <see cref="MimicEndvalve1"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="MimicEndvalve1"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="MimicEndvalve1"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static MimicOuputs GetPayload(HarpMessage message)
        {
            return (MimicOuputs)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="MimicEndvalve1"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<MimicOuputs> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((MimicOuputs)payload.Value, payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="MimicEndvalve1"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="MimicEndvalve1"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, MimicOuputs value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="MimicEndvalve1"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="MimicEndvalve1"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, MimicOuputs value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// MimicEndvalve1 register.
    /// </summary>
    /// <seealso cref="MimicEndvalve1"/>
    [Description("Filters and selects timestamped messages from the MimicEndvalve1 register.")]
    public partial class TimestampedMimicEndvalve1
    {
        /// <summary>
        /// Represents the address of the <see cref="MimicEndvalve1"/> register. This field is constant.
        /// </summary>
        public const int Address = MimicEndvalve1.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="MimicEndvalve1"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<MimicOuputs> GetPayload(HarpMessage message)
        {
            return MimicEndvalve1.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that mimic dummy valve.
    /// </summary>
    [Description("Mimic dummy valve.")]
    public partial class MimicDummyvalve
    {
        /// <summary>
        /// Represents the address of the <see cref="MimicDummyvalve"/> register. This field is constant.
        /// </summary>
        public const int Address = 87;

        /// <summary>
        /// Represents the payload type of the <see cref="MimicDummyvalve"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="MimicDummyvalve"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="MimicDummyvalve"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static MimicOuputs GetPayload(HarpMessage message)
        {
            return (MimicOuputs)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="MimicDummyvalve"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<MimicOuputs> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((MimicOuputs)payload.Value, payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="MimicDummyvalve"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="MimicDummyvalve"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, MimicOuputs value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="MimicDummyvalve"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="MimicDummyvalve"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, MimicOuputs value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// MimicDummyvalve register.
    /// </summary>
    /// <seealso cref="MimicDummyvalve"/>
    [Description("Filters and selects timestamped messages from the MimicDummyvalve register.")]
    public partial class TimestampedMimicDummyvalve
    {
        /// <summary>
        /// Represents the address of the <see cref="MimicDummyvalve"/> register. This field is constant.
        /// </summary>
        public const int Address = MimicDummyvalve.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="MimicDummyvalve"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<MimicOuputs> GetPayload(HarpMessage message)
        {
            return MimicDummyvalve.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that enable the valves control via the external screw terminals.
    /// </summary>
    [Description("Enable the valves control via the external screw terminals.")]
    public partial class EnableExternalControlValves
    {
        /// <summary>
        /// Represents the address of the <see cref="EnableExternalControlValves"/> register. This field is constant.
        /// </summary>
        public const int Address = 88;

        /// <summary>
        /// Represents the payload type of the <see cref="EnableExternalControlValves"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="EnableExternalControlValves"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="EnableExternalControlValves"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static EnableFlag GetPayload(HarpMessage message)
        {
            return (EnableFlag)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="EnableExternalControlValves"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<EnableFlag> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((EnableFlag)payload.Value, payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="EnableExternalControlValves"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="EnableExternalControlValves"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, EnableFlag value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="EnableExternalControlValves"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="EnableExternalControlValves"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, EnableFlag value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// EnableExternalControlValves register.
    /// </summary>
    /// <seealso cref="EnableExternalControlValves"/>
    [Description("Filters and selects timestamped messages from the EnableExternalControlValves register.")]
    public partial class TimestampedEnableExternalControlValves
    {
        /// <summary>
        /// Represents the address of the <see cref="EnableExternalControlValves"/> register. This field is constant.
        /// </summary>
        public const int Address = EnableExternalControlValves.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="EnableExternalControlValves"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<EnableFlag> GetPayload(HarpMessage message)
        {
            return EnableExternalControlValves.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that selects the flow range for the channel 3 (0-100ml/min or 0-1000ml/min).
    /// </summary>
    [Description("Selects the flow range for the channel 3 (0-100ml/min or 0-1000ml/min).")]
    public partial class Channel3Range
    {
        /// <summary>
        /// Represents the address of the <see cref="Channel3Range"/> register. This field is constant.
        /// </summary>
        public const int Address = 89;

        /// <summary>
        /// Represents the payload type of the <see cref="Channel3Range"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="Channel3Range"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="Channel3Range"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static Channel3RangeConfig GetPayload(HarpMessage message)
        {
            return (Channel3RangeConfig)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="Channel3Range"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<Channel3RangeConfig> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((Channel3RangeConfig)payload.Value, payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="Channel3Range"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Channel3Range"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, Channel3RangeConfig value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="Channel3Range"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Channel3Range"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, Channel3RangeConfig value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// Channel3Range register.
    /// </summary>
    /// <seealso cref="Channel3Range"/>
    [Description("Filters and selects timestamped messages from the Channel3Range register.")]
    public partial class TimestampedChannel3Range
    {
        /// <summary>
        /// Represents the address of the <see cref="Channel3Range"/> register. This field is constant.
        /// </summary>
        public const int Address = Channel3Range.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="Channel3Range"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<Channel3RangeConfig> GetPayload(HarpMessage message)
        {
            return Channel3Range.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that specifies the active events in the device.
    /// </summary>
    [Description("Specifies the active events in the device.")]
    public partial class EnableEvents
    {
        /// <summary>
        /// Represents the address of the <see cref="EnableEvents"/> register. This field is constant.
        /// </summary>
        public const int Address = 93;

        /// <summary>
        /// Represents the payload type of the <see cref="EnableEvents"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="EnableEvents"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="EnableEvents"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static OlfactometerEvents GetPayload(HarpMessage message)
        {
            return (OlfactometerEvents)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="EnableEvents"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<OlfactometerEvents> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((OlfactometerEvents)payload.Value, payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="EnableEvents"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="EnableEvents"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, OlfactometerEvents value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="EnableEvents"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="EnableEvents"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, OlfactometerEvents value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// EnableEvents register.
    /// </summary>
    /// <seealso cref="EnableEvents"/>
    [Description("Filters and selects timestamped messages from the EnableEvents register.")]
    public partial class TimestampedEnableEvents
    {
        /// <summary>
        /// Represents the address of the <see cref="EnableEvents"/> register. This field is constant.
        /// </summary>
        public const int Address = EnableEvents.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="EnableEvents"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<OlfactometerEvents> GetPayload(HarpMessage message)
        {
            return EnableEvents.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents an operator which creates standard message payloads for the
    /// Olfactometer device.
    /// </summary>
    /// <seealso cref="CreateEnableFlowPayload"/>
    /// <seealso cref="CreateFlowmeterAnalogOutputsPayload"/>
    /// <seealso cref="CreateDI0StatePayload"/>
    /// <seealso cref="CreateChannel0UserCalibrationPayload"/>
    /// <seealso cref="CreateChannel1UserCalibrationPayload"/>
    /// <seealso cref="CreateChannel2UserCalibrationPayload"/>
    /// <seealso cref="CreateChannel3UserCalibrationPayload"/>
    /// <seealso cref="CreateChannel4UserCalibrationPayload"/>
    /// <seealso cref="CreateChannel3UserCalibrationAuxPayload"/>
    /// <seealso cref="CreateUserCalibrationEnablePayload"/>
    /// <seealso cref="CreateChannel0FlowTargetPayload"/>
    /// <seealso cref="CreateChannel1FlowTargetPayload"/>
    /// <seealso cref="CreateChannel2FlowTargetPayload"/>
    /// <seealso cref="CreateChannel3FlowTargetPayload"/>
    /// <seealso cref="CreateChannel4FlowTargetPayload"/>
    /// <seealso cref="CreateChannel0FlowRealPayload"/>
    /// <seealso cref="CreateChannel1FlowRealPayload"/>
    /// <seealso cref="CreateChannel2FlowRealPayload"/>
    /// <seealso cref="CreateChannel3FlowRealPayload"/>
    /// <seealso cref="CreateChannel4FlowRealPayload"/>
    /// <seealso cref="CreateChannel0DutyCyclePayload"/>
    /// <seealso cref="CreateChannel1DutyCyclePayload"/>
    /// <seealso cref="CreateChannel2DutyCyclePayload"/>
    /// <seealso cref="CreateChannel3DutyCyclePayload"/>
    /// <seealso cref="CreateChannel4DutyCyclePayload"/>
    /// <seealso cref="CreateDigitalOutputSetPayload"/>
    /// <seealso cref="CreateDigitalOutputClearPayload"/>
    /// <seealso cref="CreateDigitalOutputTogglePayload"/>
    /// <seealso cref="CreateDigitalOutputStatePayload"/>
    /// <seealso cref="CreateEnableValvesPulsePayload"/>
    /// <seealso cref="CreateValvesSetPayload"/>
    /// <seealso cref="CreateValvesClearPayload"/>
    /// <seealso cref="CreateValvesTogglePayload"/>
    /// <seealso cref="CreateValvesStatePayload"/>
    /// <seealso cref="CreatePulseValve0Payload"/>
    /// <seealso cref="CreatePulseValve1Payload"/>
    /// <seealso cref="CreatePulseValve2Payload"/>
    /// <seealso cref="CreatePulseValve3Payload"/>
    /// <seealso cref="CreatePulseEndvalve0Payload"/>
    /// <seealso cref="CreatePulseEndvalve1Payload"/>
    /// <seealso cref="CreatePulseDummyvalvePayload"/>
    /// <seealso cref="CreateDO0SyncPayload"/>
    /// <seealso cref="CreateDO1SyncPayload"/>
    /// <seealso cref="CreateDI0TriggerPayload"/>
    /// <seealso cref="CreateMimicValve0Payload"/>
    /// <seealso cref="CreateMimicValve1Payload"/>
    /// <seealso cref="CreateMimicValve2Payload"/>
    /// <seealso cref="CreateMimicValve3Payload"/>
    /// <seealso cref="CreateMimicEndvalve0Payload"/>
    /// <seealso cref="CreateMimicEndvalve1Payload"/>
    /// <seealso cref="CreateMimicDummyvalvePayload"/>
    /// <seealso cref="CreateEnableExternalControlValvesPayload"/>
    /// <seealso cref="CreateChannel3RangePayload"/>
    /// <seealso cref="CreateEnableEventsPayload"/>
    [XmlInclude(typeof(CreateEnableFlowPayload))]
    [XmlInclude(typeof(CreateFlowmeterAnalogOutputsPayload))]
    [XmlInclude(typeof(CreateDI0StatePayload))]
    [XmlInclude(typeof(CreateChannel0UserCalibrationPayload))]
    [XmlInclude(typeof(CreateChannel1UserCalibrationPayload))]
    [XmlInclude(typeof(CreateChannel2UserCalibrationPayload))]
    [XmlInclude(typeof(CreateChannel3UserCalibrationPayload))]
    [XmlInclude(typeof(CreateChannel4UserCalibrationPayload))]
    [XmlInclude(typeof(CreateChannel3UserCalibrationAuxPayload))]
    [XmlInclude(typeof(CreateUserCalibrationEnablePayload))]
    [XmlInclude(typeof(CreateChannel0FlowTargetPayload))]
    [XmlInclude(typeof(CreateChannel1FlowTargetPayload))]
    [XmlInclude(typeof(CreateChannel2FlowTargetPayload))]
    [XmlInclude(typeof(CreateChannel3FlowTargetPayload))]
    [XmlInclude(typeof(CreateChannel4FlowTargetPayload))]
    [XmlInclude(typeof(CreateChannel0FlowRealPayload))]
    [XmlInclude(typeof(CreateChannel1FlowRealPayload))]
    [XmlInclude(typeof(CreateChannel2FlowRealPayload))]
    [XmlInclude(typeof(CreateChannel3FlowRealPayload))]
    [XmlInclude(typeof(CreateChannel4FlowRealPayload))]
    [XmlInclude(typeof(CreateChannel0DutyCyclePayload))]
    [XmlInclude(typeof(CreateChannel1DutyCyclePayload))]
    [XmlInclude(typeof(CreateChannel2DutyCyclePayload))]
    [XmlInclude(typeof(CreateChannel3DutyCyclePayload))]
    [XmlInclude(typeof(CreateChannel4DutyCyclePayload))]
    [XmlInclude(typeof(CreateDigitalOutputSetPayload))]
    [XmlInclude(typeof(CreateDigitalOutputClearPayload))]
    [XmlInclude(typeof(CreateDigitalOutputTogglePayload))]
    [XmlInclude(typeof(CreateDigitalOutputStatePayload))]
    [XmlInclude(typeof(CreateEnableValvesPulsePayload))]
    [XmlInclude(typeof(CreateValvesSetPayload))]
    [XmlInclude(typeof(CreateValvesClearPayload))]
    [XmlInclude(typeof(CreateValvesTogglePayload))]
    [XmlInclude(typeof(CreateValvesStatePayload))]
    [XmlInclude(typeof(CreatePulseValve0Payload))]
    [XmlInclude(typeof(CreatePulseValve1Payload))]
    [XmlInclude(typeof(CreatePulseValve2Payload))]
    [XmlInclude(typeof(CreatePulseValve3Payload))]
    [XmlInclude(typeof(CreatePulseEndvalve0Payload))]
    [XmlInclude(typeof(CreatePulseEndvalve1Payload))]
    [XmlInclude(typeof(CreatePulseDummyvalvePayload))]
    [XmlInclude(typeof(CreateDO0SyncPayload))]
    [XmlInclude(typeof(CreateDO1SyncPayload))]
    [XmlInclude(typeof(CreateDI0TriggerPayload))]
    [XmlInclude(typeof(CreateMimicValve0Payload))]
    [XmlInclude(typeof(CreateMimicValve1Payload))]
    [XmlInclude(typeof(CreateMimicValve2Payload))]
    [XmlInclude(typeof(CreateMimicValve3Payload))]
    [XmlInclude(typeof(CreateMimicEndvalve0Payload))]
    [XmlInclude(typeof(CreateMimicEndvalve1Payload))]
    [XmlInclude(typeof(CreateMimicDummyvalvePayload))]
    [XmlInclude(typeof(CreateEnableExternalControlValvesPayload))]
    [XmlInclude(typeof(CreateChannel3RangePayload))]
    [XmlInclude(typeof(CreateEnableEventsPayload))]
    [XmlInclude(typeof(CreateTimestampedEnableFlowPayload))]
    [XmlInclude(typeof(CreateTimestampedFlowmeterAnalogOutputsPayload))]
    [XmlInclude(typeof(CreateTimestampedDI0StatePayload))]
    [XmlInclude(typeof(CreateTimestampedChannel0UserCalibrationPayload))]
    [XmlInclude(typeof(CreateTimestampedChannel1UserCalibrationPayload))]
    [XmlInclude(typeof(CreateTimestampedChannel2UserCalibrationPayload))]
    [XmlInclude(typeof(CreateTimestampedChannel3UserCalibrationPayload))]
    [XmlInclude(typeof(CreateTimestampedChannel4UserCalibrationPayload))]
    [XmlInclude(typeof(CreateTimestampedChannel3UserCalibrationAuxPayload))]
    [XmlInclude(typeof(CreateTimestampedUserCalibrationEnablePayload))]
    [XmlInclude(typeof(CreateTimestampedChannel0FlowTargetPayload))]
    [XmlInclude(typeof(CreateTimestampedChannel1FlowTargetPayload))]
    [XmlInclude(typeof(CreateTimestampedChannel2FlowTargetPayload))]
    [XmlInclude(typeof(CreateTimestampedChannel3FlowTargetPayload))]
    [XmlInclude(typeof(CreateTimestampedChannel4FlowTargetPayload))]
    [XmlInclude(typeof(CreateTimestampedChannel0FlowRealPayload))]
    [XmlInclude(typeof(CreateTimestampedChannel1FlowRealPayload))]
    [XmlInclude(typeof(CreateTimestampedChannel2FlowRealPayload))]
    [XmlInclude(typeof(CreateTimestampedChannel3FlowRealPayload))]
    [XmlInclude(typeof(CreateTimestampedChannel4FlowRealPayload))]
    [XmlInclude(typeof(CreateTimestampedChannel0DutyCyclePayload))]
    [XmlInclude(typeof(CreateTimestampedChannel1DutyCyclePayload))]
    [XmlInclude(typeof(CreateTimestampedChannel2DutyCyclePayload))]
    [XmlInclude(typeof(CreateTimestampedChannel3DutyCyclePayload))]
    [XmlInclude(typeof(CreateTimestampedChannel4DutyCyclePayload))]
    [XmlInclude(typeof(CreateTimestampedDigitalOutputSetPayload))]
    [XmlInclude(typeof(CreateTimestampedDigitalOutputClearPayload))]
    [XmlInclude(typeof(CreateTimestampedDigitalOutputTogglePayload))]
    [XmlInclude(typeof(CreateTimestampedDigitalOutputStatePayload))]
    [XmlInclude(typeof(CreateTimestampedEnableValvesPulsePayload))]
    [XmlInclude(typeof(CreateTimestampedValvesSetPayload))]
    [XmlInclude(typeof(CreateTimestampedValvesClearPayload))]
    [XmlInclude(typeof(CreateTimestampedValvesTogglePayload))]
    [XmlInclude(typeof(CreateTimestampedValvesStatePayload))]
    [XmlInclude(typeof(CreateTimestampedPulseValve0Payload))]
    [XmlInclude(typeof(CreateTimestampedPulseValve1Payload))]
    [XmlInclude(typeof(CreateTimestampedPulseValve2Payload))]
    [XmlInclude(typeof(CreateTimestampedPulseValve3Payload))]
    [XmlInclude(typeof(CreateTimestampedPulseEndvalve0Payload))]
    [XmlInclude(typeof(CreateTimestampedPulseEndvalve1Payload))]
    [XmlInclude(typeof(CreateTimestampedPulseDummyvalvePayload))]
    [XmlInclude(typeof(CreateTimestampedDO0SyncPayload))]
    [XmlInclude(typeof(CreateTimestampedDO1SyncPayload))]
    [XmlInclude(typeof(CreateTimestampedDI0TriggerPayload))]
    [XmlInclude(typeof(CreateTimestampedMimicValve0Payload))]
    [XmlInclude(typeof(CreateTimestampedMimicValve1Payload))]
    [XmlInclude(typeof(CreateTimestampedMimicValve2Payload))]
    [XmlInclude(typeof(CreateTimestampedMimicValve3Payload))]
    [XmlInclude(typeof(CreateTimestampedMimicEndvalve0Payload))]
    [XmlInclude(typeof(CreateTimestampedMimicEndvalve1Payload))]
    [XmlInclude(typeof(CreateTimestampedMimicDummyvalvePayload))]
    [XmlInclude(typeof(CreateTimestampedEnableExternalControlValvesPayload))]
    [XmlInclude(typeof(CreateTimestampedChannel3RangePayload))]
    [XmlInclude(typeof(CreateTimestampedEnableEventsPayload))]
    [Description("Creates standard message payloads for the Olfactometer device.")]
    public partial class CreateMessage : CreateMessageBuilder, INamedElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateMessage"/> class.
        /// </summary>
        public CreateMessage()
        {
            Payload = new CreateEnableFlowPayload();
        }

        string INamedElement.Name => $"{nameof(Olfactometer)}.{GetElementDisplayName(Payload)}";
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that write any value above zero to start the flowmeter and zero to stop.
    /// </summary>
    [DisplayName("EnableFlowPayload")]
    [Description("Creates a message payload that write any value above zero to start the flowmeter and zero to stop.")]
    public partial class CreateEnableFlowPayload
    {
        /// <summary>
        /// Gets or sets the value that write any value above zero to start the flowmeter and zero to stop.
        /// </summary>
        [Description("The value that write any value above zero to start the flowmeter and zero to stop.")]
        public EnableFlag EnableFlow { get; set; }

        /// <summary>
        /// Creates a message payload for the EnableFlow register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public EnableFlag GetPayload()
        {
            return EnableFlow;
        }

        /// <summary>
        /// Creates a message that write any value above zero to start the flowmeter and zero to stop.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the EnableFlow register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.EnableFlow.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that write any value above zero to start the flowmeter and zero to stop.
    /// </summary>
    [DisplayName("TimestampedEnableFlowPayload")]
    [Description("Creates a timestamped message payload that write any value above zero to start the flowmeter and zero to stop.")]
    public partial class CreateTimestampedEnableFlowPayload : CreateEnableFlowPayload
    {
        /// <summary>
        /// Creates a timestamped message that write any value above zero to start the flowmeter and zero to stop.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the EnableFlow register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.EnableFlow.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that value of the flowmeters analog outups.
    /// </summary>
    [DisplayName("FlowmeterAnalogOutputsPayload")]
    [Description("Creates a message payload that value of the flowmeters analog outups.")]
    public partial class CreateFlowmeterAnalogOutputsPayload
    {
        /// <summary>
        /// Gets or sets the value that value of the flowmeters analog outups.
        /// </summary>
        [Description("The value that value of the flowmeters analog outups.")]
        public short[] FlowmeterAnalogOutputs { get; set; }

        /// <summary>
        /// Creates a message payload for the FlowmeterAnalogOutputs register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public short[] GetPayload()
        {
            return FlowmeterAnalogOutputs;
        }

        /// <summary>
        /// Creates a message that value of the flowmeters analog outups.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the FlowmeterAnalogOutputs register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.FlowmeterAnalogOutputs.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that value of the flowmeters analog outups.
    /// </summary>
    [DisplayName("TimestampedFlowmeterAnalogOutputsPayload")]
    [Description("Creates a timestamped message payload that value of the flowmeters analog outups.")]
    public partial class CreateTimestampedFlowmeterAnalogOutputsPayload : CreateFlowmeterAnalogOutputsPayload
    {
        /// <summary>
        /// Creates a timestamped message that value of the flowmeters analog outups.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the FlowmeterAnalogOutputs register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.FlowmeterAnalogOutputs.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that state of the digital input pin 0.
    /// </summary>
    [DisplayName("DI0StatePayload")]
    [Description("Creates a message payload that state of the digital input pin 0.")]
    public partial class CreateDI0StatePayload
    {
        /// <summary>
        /// Gets or sets the value that state of the digital input pin 0.
        /// </summary>
        [Description("The value that state of the digital input pin 0.")]
        public DigitalState DI0State { get; set; }

        /// <summary>
        /// Creates a message payload for the DI0State register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public DigitalState GetPayload()
        {
            return DI0State;
        }

        /// <summary>
        /// Creates a message that state of the digital input pin 0.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the DI0State register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.DI0State.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that state of the digital input pin 0.
    /// </summary>
    [DisplayName("TimestampedDI0StatePayload")]
    [Description("Creates a timestamped message payload that state of the digital input pin 0.")]
    public partial class CreateTimestampedDI0StatePayload : CreateDI0StatePayload
    {
        /// <summary>
        /// Creates a timestamped message that state of the digital input pin 0.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the DI0State register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.DI0State.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that values of calibration for channel 0 - flowmeter 0 [x0,x1, ..., x10] [x= ADC raw value for 0-100 ml/min, step 10].
    /// </summary>
    [DisplayName("Channel0UserCalibrationPayload")]
    [Description("Creates a message payload that values of calibration for channel 0 - flowmeter 0 [x0,x1, ..., x10] [x= ADC raw value for 0-100 ml/min, step 10].")]
    public partial class CreateChannel0UserCalibrationPayload
    {
        /// <summary>
        /// Gets or sets the value that values of calibration for channel 0 - flowmeter 0 [x0,x1, ..., x10] [x= ADC raw value for 0-100 ml/min, step 10].
        /// </summary>
        [Description("The value that values of calibration for channel 0 - flowmeter 0 [x0,x1, ..., x10] [x= ADC raw value for 0-100 ml/min, step 10].")]
        public ushort[] Channel0UserCalibration { get; set; }

        /// <summary>
        /// Creates a message payload for the Channel0UserCalibration register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public ushort[] GetPayload()
        {
            return Channel0UserCalibration;
        }

        /// <summary>
        /// Creates a message that values of calibration for channel 0 - flowmeter 0 [x0,x1, ..., x10] [x= ADC raw value for 0-100 ml/min, step 10].
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the Channel0UserCalibration register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.Channel0UserCalibration.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that values of calibration for channel 0 - flowmeter 0 [x0,x1, ..., x10] [x= ADC raw value for 0-100 ml/min, step 10].
    /// </summary>
    [DisplayName("TimestampedChannel0UserCalibrationPayload")]
    [Description("Creates a timestamped message payload that values of calibration for channel 0 - flowmeter 0 [x0,x1, ..., x10] [x= ADC raw value for 0-100 ml/min, step 10].")]
    public partial class CreateTimestampedChannel0UserCalibrationPayload : CreateChannel0UserCalibrationPayload
    {
        /// <summary>
        /// Creates a timestamped message that values of calibration for channel 0 - flowmeter 0 [x0,x1, ..., x10] [x= ADC raw value for 0-100 ml/min, step 10].
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the Channel0UserCalibration register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.Channel0UserCalibration.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that values of calibration for channel 1 - flowmeter 1 [x0,x1, ..., x10] [x= ADC raw value for 0-100 ml/min, step 10].
    /// </summary>
    [DisplayName("Channel1UserCalibrationPayload")]
    [Description("Creates a message payload that values of calibration for channel 1 - flowmeter 1 [x0,x1, ..., x10] [x= ADC raw value for 0-100 ml/min, step 10].")]
    public partial class CreateChannel1UserCalibrationPayload
    {
        /// <summary>
        /// Gets or sets the value that values of calibration for channel 1 - flowmeter 1 [x0,x1, ..., x10] [x= ADC raw value for 0-100 ml/min, step 10].
        /// </summary>
        [Description("The value that values of calibration for channel 1 - flowmeter 1 [x0,x1, ..., x10] [x= ADC raw value for 0-100 ml/min, step 10].")]
        public ushort[] Channel1UserCalibration { get; set; }

        /// <summary>
        /// Creates a message payload for the Channel1UserCalibration register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public ushort[] GetPayload()
        {
            return Channel1UserCalibration;
        }

        /// <summary>
        /// Creates a message that values of calibration for channel 1 - flowmeter 1 [x0,x1, ..., x10] [x= ADC raw value for 0-100 ml/min, step 10].
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the Channel1UserCalibration register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.Channel1UserCalibration.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that values of calibration for channel 1 - flowmeter 1 [x0,x1, ..., x10] [x= ADC raw value for 0-100 ml/min, step 10].
    /// </summary>
    [DisplayName("TimestampedChannel1UserCalibrationPayload")]
    [Description("Creates a timestamped message payload that values of calibration for channel 1 - flowmeter 1 [x0,x1, ..., x10] [x= ADC raw value for 0-100 ml/min, step 10].")]
    public partial class CreateTimestampedChannel1UserCalibrationPayload : CreateChannel1UserCalibrationPayload
    {
        /// <summary>
        /// Creates a timestamped message that values of calibration for channel 1 - flowmeter 1 [x0,x1, ..., x10] [x= ADC raw value for 0-100 ml/min, step 10].
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the Channel1UserCalibration register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.Channel1UserCalibration.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that values of calibration for channel 2 - flowmeter 2 [x0,x1, ..., x10] [x= ADC raw value for 0-100 ml/min, step 10].
    /// </summary>
    [DisplayName("Channel2UserCalibrationPayload")]
    [Description("Creates a message payload that values of calibration for channel 2 - flowmeter 2 [x0,x1, ..., x10] [x= ADC raw value for 0-100 ml/min, step 10].")]
    public partial class CreateChannel2UserCalibrationPayload
    {
        /// <summary>
        /// Gets or sets the value that values of calibration for channel 2 - flowmeter 2 [x0,x1, ..., x10] [x= ADC raw value for 0-100 ml/min, step 10].
        /// </summary>
        [Description("The value that values of calibration for channel 2 - flowmeter 2 [x0,x1, ..., x10] [x= ADC raw value for 0-100 ml/min, step 10].")]
        public ushort[] Channel2UserCalibration { get; set; }

        /// <summary>
        /// Creates a message payload for the Channel2UserCalibration register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public ushort[] GetPayload()
        {
            return Channel2UserCalibration;
        }

        /// <summary>
        /// Creates a message that values of calibration for channel 2 - flowmeter 2 [x0,x1, ..., x10] [x= ADC raw value for 0-100 ml/min, step 10].
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the Channel2UserCalibration register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.Channel2UserCalibration.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that values of calibration for channel 2 - flowmeter 2 [x0,x1, ..., x10] [x= ADC raw value for 0-100 ml/min, step 10].
    /// </summary>
    [DisplayName("TimestampedChannel2UserCalibrationPayload")]
    [Description("Creates a timestamped message payload that values of calibration for channel 2 - flowmeter 2 [x0,x1, ..., x10] [x= ADC raw value for 0-100 ml/min, step 10].")]
    public partial class CreateTimestampedChannel2UserCalibrationPayload : CreateChannel2UserCalibrationPayload
    {
        /// <summary>
        /// Creates a timestamped message that values of calibration for channel 2 - flowmeter 2 [x0,x1, ..., x10] [x= ADC raw value for 0-100 ml/min, step 10].
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the Channel2UserCalibration register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.Channel2UserCalibration.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that values of calibration for channel 3 - flowmeter 3 [x0,x1, ..., x10] [x= ADC raw value for 0-100 ml/min, step 10].
    /// </summary>
    [DisplayName("Channel3UserCalibrationPayload")]
    [Description("Creates a message payload that values of calibration for channel 3 - flowmeter 3 [x0,x1, ..., x10] [x= ADC raw value for 0-100 ml/min, step 10].")]
    public partial class CreateChannel3UserCalibrationPayload
    {
        /// <summary>
        /// Gets or sets the value that values of calibration for channel 3 - flowmeter 3 [x0,x1, ..., x10] [x= ADC raw value for 0-100 ml/min, step 10].
        /// </summary>
        [Description("The value that values of calibration for channel 3 - flowmeter 3 [x0,x1, ..., x10] [x= ADC raw value for 0-100 ml/min, step 10].")]
        public ushort[] Channel3UserCalibration { get; set; }

        /// <summary>
        /// Creates a message payload for the Channel3UserCalibration register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public ushort[] GetPayload()
        {
            return Channel3UserCalibration;
        }

        /// <summary>
        /// Creates a message that values of calibration for channel 3 - flowmeter 3 [x0,x1, ..., x10] [x= ADC raw value for 0-100 ml/min, step 10].
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the Channel3UserCalibration register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.Channel3UserCalibration.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that values of calibration for channel 3 - flowmeter 3 [x0,x1, ..., x10] [x= ADC raw value for 0-100 ml/min, step 10].
    /// </summary>
    [DisplayName("TimestampedChannel3UserCalibrationPayload")]
    [Description("Creates a timestamped message payload that values of calibration for channel 3 - flowmeter 3 [x0,x1, ..., x10] [x= ADC raw value for 0-100 ml/min, step 10].")]
    public partial class CreateTimestampedChannel3UserCalibrationPayload : CreateChannel3UserCalibrationPayload
    {
        /// <summary>
        /// Creates a timestamped message that values of calibration for channel 3 - flowmeter 3 [x0,x1, ..., x10] [x= ADC raw value for 0-100 ml/min, step 10].
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the Channel3UserCalibration register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.Channel3UserCalibration.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that values of calibration for channel 4 - flowmeter 4 [x0,x1, ..., x10] [x= ADC raw value for 0-1000 ml/min, step 100].
    /// </summary>
    [DisplayName("Channel4UserCalibrationPayload")]
    [Description("Creates a message payload that values of calibration for channel 4 - flowmeter 4 [x0,x1, ..., x10] [x= ADC raw value for 0-1000 ml/min, step 100].")]
    public partial class CreateChannel4UserCalibrationPayload
    {
        /// <summary>
        /// Gets or sets the value that values of calibration for channel 4 - flowmeter 4 [x0,x1, ..., x10] [x= ADC raw value for 0-1000 ml/min, step 100].
        /// </summary>
        [Description("The value that values of calibration for channel 4 - flowmeter 4 [x0,x1, ..., x10] [x= ADC raw value for 0-1000 ml/min, step 100].")]
        public ushort[] Channel4UserCalibration { get; set; }

        /// <summary>
        /// Creates a message payload for the Channel4UserCalibration register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public ushort[] GetPayload()
        {
            return Channel4UserCalibration;
        }

        /// <summary>
        /// Creates a message that values of calibration for channel 4 - flowmeter 4 [x0,x1, ..., x10] [x= ADC raw value for 0-1000 ml/min, step 100].
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the Channel4UserCalibration register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.Channel4UserCalibration.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that values of calibration for channel 4 - flowmeter 4 [x0,x1, ..., x10] [x= ADC raw value for 0-1000 ml/min, step 100].
    /// </summary>
    [DisplayName("TimestampedChannel4UserCalibrationPayload")]
    [Description("Creates a timestamped message payload that values of calibration for channel 4 - flowmeter 4 [x0,x1, ..., x10] [x= ADC raw value for 0-1000 ml/min, step 100].")]
    public partial class CreateTimestampedChannel4UserCalibrationPayload : CreateChannel4UserCalibrationPayload
    {
        /// <summary>
        /// Creates a timestamped message that values of calibration for channel 4 - flowmeter 4 [x0,x1, ..., x10] [x= ADC raw value for 0-1000 ml/min, step 100].
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the Channel4UserCalibration register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.Channel4UserCalibration.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that values of calibration for channel 3 - flowmeter 3 [x0,x1, ..., x10] [x= ADC raw value for 0-1000 ml/min, step 100].
    /// </summary>
    [DisplayName("Channel3UserCalibrationAuxPayload")]
    [Description("Creates a message payload that values of calibration for channel 3 - flowmeter 3 [x0,x1, ..., x10] [x= ADC raw value for 0-1000 ml/min, step 100].")]
    public partial class CreateChannel3UserCalibrationAuxPayload
    {
        /// <summary>
        /// Gets or sets the value that values of calibration for channel 3 - flowmeter 3 [x0,x1, ..., x10] [x= ADC raw value for 0-1000 ml/min, step 100].
        /// </summary>
        [Description("The value that values of calibration for channel 3 - flowmeter 3 [x0,x1, ..., x10] [x= ADC raw value for 0-1000 ml/min, step 100].")]
        public ushort[] Channel3UserCalibrationAux { get; set; }

        /// <summary>
        /// Creates a message payload for the Channel3UserCalibrationAux register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public ushort[] GetPayload()
        {
            return Channel3UserCalibrationAux;
        }

        /// <summary>
        /// Creates a message that values of calibration for channel 3 - flowmeter 3 [x0,x1, ..., x10] [x= ADC raw value for 0-1000 ml/min, step 100].
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the Channel3UserCalibrationAux register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.Channel3UserCalibrationAux.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that values of calibration for channel 3 - flowmeter 3 [x0,x1, ..., x10] [x= ADC raw value for 0-1000 ml/min, step 100].
    /// </summary>
    [DisplayName("TimestampedChannel3UserCalibrationAuxPayload")]
    [Description("Creates a timestamped message payload that values of calibration for channel 3 - flowmeter 3 [x0,x1, ..., x10] [x= ADC raw value for 0-1000 ml/min, step 100].")]
    public partial class CreateTimestampedChannel3UserCalibrationAuxPayload : CreateChannel3UserCalibrationAuxPayload
    {
        /// <summary>
        /// Creates a timestamped message that values of calibration for channel 3 - flowmeter 3 [x0,x1, ..., x10] [x= ADC raw value for 0-1000 ml/min, step 100].
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the Channel3UserCalibrationAux register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.Channel3UserCalibrationAux.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that override the factory calibration values, replacing with CHX_USER_CALIBRATION.
    /// </summary>
    [DisplayName("UserCalibrationEnablePayload")]
    [Description("Creates a message payload that override the factory calibration values, replacing with CHX_USER_CALIBRATION.")]
    public partial class CreateUserCalibrationEnablePayload
    {
        /// <summary>
        /// Gets or sets the value that override the factory calibration values, replacing with CHX_USER_CALIBRATION.
        /// </summary>
        [Description("The value that override the factory calibration values, replacing with CHX_USER_CALIBRATION.")]
        public byte UserCalibrationEnable { get; set; }

        /// <summary>
        /// Creates a message payload for the UserCalibrationEnable register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public byte GetPayload()
        {
            return UserCalibrationEnable;
        }

        /// <summary>
        /// Creates a message that override the factory calibration values, replacing with CHX_USER_CALIBRATION.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the UserCalibrationEnable register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.UserCalibrationEnable.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that override the factory calibration values, replacing with CHX_USER_CALIBRATION.
    /// </summary>
    [DisplayName("TimestampedUserCalibrationEnablePayload")]
    [Description("Creates a timestamped message payload that override the factory calibration values, replacing with CHX_USER_CALIBRATION.")]
    public partial class CreateTimestampedUserCalibrationEnablePayload : CreateUserCalibrationEnablePayload
    {
        /// <summary>
        /// Creates a timestamped message that override the factory calibration values, replacing with CHX_USER_CALIBRATION.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the UserCalibrationEnable register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.UserCalibrationEnable.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that flow value set for channel 0 - flowmeter 0 [ml/min].
    /// </summary>
    [DisplayName("Channel0FlowTargetPayload")]
    [Description("Creates a message payload that flow value set for channel 0 - flowmeter 0 [ml/min].")]
    public partial class CreateChannel0FlowTargetPayload
    {
        /// <summary>
        /// Gets or sets the value that flow value set for channel 0 - flowmeter 0 [ml/min].
        /// </summary>
        [Description("The value that flow value set for channel 0 - flowmeter 0 [ml/min].")]
        public float Channel0FlowTarget { get; set; }

        /// <summary>
        /// Creates a message payload for the Channel0FlowTarget register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public float GetPayload()
        {
            return Channel0FlowTarget;
        }

        /// <summary>
        /// Creates a message that flow value set for channel 0 - flowmeter 0 [ml/min].
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the Channel0FlowTarget register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.Channel0FlowTarget.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that flow value set for channel 0 - flowmeter 0 [ml/min].
    /// </summary>
    [DisplayName("TimestampedChannel0FlowTargetPayload")]
    [Description("Creates a timestamped message payload that flow value set for channel 0 - flowmeter 0 [ml/min].")]
    public partial class CreateTimestampedChannel0FlowTargetPayload : CreateChannel0FlowTargetPayload
    {
        /// <summary>
        /// Creates a timestamped message that flow value set for channel 0 - flowmeter 0 [ml/min].
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the Channel0FlowTarget register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.Channel0FlowTarget.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that flow value set for channel 1 - flowmeter 1 [ml/min].
    /// </summary>
    [DisplayName("Channel1FlowTargetPayload")]
    [Description("Creates a message payload that flow value set for channel 1 - flowmeter 1 [ml/min].")]
    public partial class CreateChannel1FlowTargetPayload
    {
        /// <summary>
        /// Gets or sets the value that flow value set for channel 1 - flowmeter 1 [ml/min].
        /// </summary>
        [Description("The value that flow value set for channel 1 - flowmeter 1 [ml/min].")]
        public float Channel1FlowTarget { get; set; }

        /// <summary>
        /// Creates a message payload for the Channel1FlowTarget register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public float GetPayload()
        {
            return Channel1FlowTarget;
        }

        /// <summary>
        /// Creates a message that flow value set for channel 1 - flowmeter 1 [ml/min].
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the Channel1FlowTarget register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.Channel1FlowTarget.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that flow value set for channel 1 - flowmeter 1 [ml/min].
    /// </summary>
    [DisplayName("TimestampedChannel1FlowTargetPayload")]
    [Description("Creates a timestamped message payload that flow value set for channel 1 - flowmeter 1 [ml/min].")]
    public partial class CreateTimestampedChannel1FlowTargetPayload : CreateChannel1FlowTargetPayload
    {
        /// <summary>
        /// Creates a timestamped message that flow value set for channel 1 - flowmeter 1 [ml/min].
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the Channel1FlowTarget register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.Channel1FlowTarget.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that flow value set for channel 2 - flowmeter 2 [ml/min].
    /// </summary>
    [DisplayName("Channel2FlowTargetPayload")]
    [Description("Creates a message payload that flow value set for channel 2 - flowmeter 2 [ml/min].")]
    public partial class CreateChannel2FlowTargetPayload
    {
        /// <summary>
        /// Gets or sets the value that flow value set for channel 2 - flowmeter 2 [ml/min].
        /// </summary>
        [Description("The value that flow value set for channel 2 - flowmeter 2 [ml/min].")]
        public float Channel2FlowTarget { get; set; }

        /// <summary>
        /// Creates a message payload for the Channel2FlowTarget register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public float GetPayload()
        {
            return Channel2FlowTarget;
        }

        /// <summary>
        /// Creates a message that flow value set for channel 2 - flowmeter 2 [ml/min].
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the Channel2FlowTarget register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.Channel2FlowTarget.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that flow value set for channel 2 - flowmeter 2 [ml/min].
    /// </summary>
    [DisplayName("TimestampedChannel2FlowTargetPayload")]
    [Description("Creates a timestamped message payload that flow value set for channel 2 - flowmeter 2 [ml/min].")]
    public partial class CreateTimestampedChannel2FlowTargetPayload : CreateChannel2FlowTargetPayload
    {
        /// <summary>
        /// Creates a timestamped message that flow value set for channel 2 - flowmeter 2 [ml/min].
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the Channel2FlowTarget register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.Channel2FlowTarget.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that flow value set for channel 3 - flowmeter 3 [ml/min].
    /// </summary>
    [DisplayName("Channel3FlowTargetPayload")]
    [Description("Creates a message payload that flow value set for channel 3 - flowmeter 3 [ml/min].")]
    public partial class CreateChannel3FlowTargetPayload
    {
        /// <summary>
        /// Gets or sets the value that flow value set for channel 3 - flowmeter 3 [ml/min].
        /// </summary>
        [Description("The value that flow value set for channel 3 - flowmeter 3 [ml/min].")]
        public float Channel3FlowTarget { get; set; }

        /// <summary>
        /// Creates a message payload for the Channel3FlowTarget register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public float GetPayload()
        {
            return Channel3FlowTarget;
        }

        /// <summary>
        /// Creates a message that flow value set for channel 3 - flowmeter 3 [ml/min].
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the Channel3FlowTarget register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.Channel3FlowTarget.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that flow value set for channel 3 - flowmeter 3 [ml/min].
    /// </summary>
    [DisplayName("TimestampedChannel3FlowTargetPayload")]
    [Description("Creates a timestamped message payload that flow value set for channel 3 - flowmeter 3 [ml/min].")]
    public partial class CreateTimestampedChannel3FlowTargetPayload : CreateChannel3FlowTargetPayload
    {
        /// <summary>
        /// Creates a timestamped message that flow value set for channel 3 - flowmeter 3 [ml/min].
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the Channel3FlowTarget register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.Channel3FlowTarget.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that flow value set for channel 4 - flowmeter 4 [ml/min].
    /// </summary>
    [DisplayName("Channel4FlowTargetPayload")]
    [Description("Creates a message payload that flow value set for channel 4 - flowmeter 4 [ml/min].")]
    public partial class CreateChannel4FlowTargetPayload
    {
        /// <summary>
        /// Gets or sets the value that flow value set for channel 4 - flowmeter 4 [ml/min].
        /// </summary>
        [Description("The value that flow value set for channel 4 - flowmeter 4 [ml/min].")]
        public float Channel4FlowTarget { get; set; }

        /// <summary>
        /// Creates a message payload for the Channel4FlowTarget register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public float GetPayload()
        {
            return Channel4FlowTarget;
        }

        /// <summary>
        /// Creates a message that flow value set for channel 4 - flowmeter 4 [ml/min].
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the Channel4FlowTarget register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.Channel4FlowTarget.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that flow value set for channel 4 - flowmeter 4 [ml/min].
    /// </summary>
    [DisplayName("TimestampedChannel4FlowTargetPayload")]
    [Description("Creates a timestamped message payload that flow value set for channel 4 - flowmeter 4 [ml/min].")]
    public partial class CreateTimestampedChannel4FlowTargetPayload : CreateChannel4FlowTargetPayload
    {
        /// <summary>
        /// Creates a timestamped message that flow value set for channel 4 - flowmeter 4 [ml/min].
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the Channel4FlowTarget register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.Channel4FlowTarget.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that flow value read from channel 0 - flowmeter 0 [ml/min].
    /// </summary>
    [DisplayName("Channel0FlowRealPayload")]
    [Description("Creates a message payload that flow value read from channel 0 - flowmeter 0 [ml/min].")]
    public partial class CreateChannel0FlowRealPayload
    {
        /// <summary>
        /// Gets or sets the value that flow value read from channel 0 - flowmeter 0 [ml/min].
        /// </summary>
        [Description("The value that flow value read from channel 0 - flowmeter 0 [ml/min].")]
        public float Channel0FlowReal { get; set; }

        /// <summary>
        /// Creates a message payload for the Channel0FlowReal register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public float GetPayload()
        {
            return Channel0FlowReal;
        }

        /// <summary>
        /// Creates a message that flow value read from channel 0 - flowmeter 0 [ml/min].
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the Channel0FlowReal register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.Channel0FlowReal.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that flow value read from channel 0 - flowmeter 0 [ml/min].
    /// </summary>
    [DisplayName("TimestampedChannel0FlowRealPayload")]
    [Description("Creates a timestamped message payload that flow value read from channel 0 - flowmeter 0 [ml/min].")]
    public partial class CreateTimestampedChannel0FlowRealPayload : CreateChannel0FlowRealPayload
    {
        /// <summary>
        /// Creates a timestamped message that flow value read from channel 0 - flowmeter 0 [ml/min].
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the Channel0FlowReal register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.Channel0FlowReal.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that flow value read from channel 1 - flowmeter 1 [ml/min].
    /// </summary>
    [DisplayName("Channel1FlowRealPayload")]
    [Description("Creates a message payload that flow value read from channel 1 - flowmeter 1 [ml/min].")]
    public partial class CreateChannel1FlowRealPayload
    {
        /// <summary>
        /// Gets or sets the value that flow value read from channel 1 - flowmeter 1 [ml/min].
        /// </summary>
        [Description("The value that flow value read from channel 1 - flowmeter 1 [ml/min].")]
        public float Channel1FlowReal { get; set; }

        /// <summary>
        /// Creates a message payload for the Channel1FlowReal register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public float GetPayload()
        {
            return Channel1FlowReal;
        }

        /// <summary>
        /// Creates a message that flow value read from channel 1 - flowmeter 1 [ml/min].
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the Channel1FlowReal register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.Channel1FlowReal.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that flow value read from channel 1 - flowmeter 1 [ml/min].
    /// </summary>
    [DisplayName("TimestampedChannel1FlowRealPayload")]
    [Description("Creates a timestamped message payload that flow value read from channel 1 - flowmeter 1 [ml/min].")]
    public partial class CreateTimestampedChannel1FlowRealPayload : CreateChannel1FlowRealPayload
    {
        /// <summary>
        /// Creates a timestamped message that flow value read from channel 1 - flowmeter 1 [ml/min].
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the Channel1FlowReal register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.Channel1FlowReal.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that flow value read from channel 2 - flowmeter 2 [ml/min].
    /// </summary>
    [DisplayName("Channel2FlowRealPayload")]
    [Description("Creates a message payload that flow value read from channel 2 - flowmeter 2 [ml/min].")]
    public partial class CreateChannel2FlowRealPayload
    {
        /// <summary>
        /// Gets or sets the value that flow value read from channel 2 - flowmeter 2 [ml/min].
        /// </summary>
        [Description("The value that flow value read from channel 2 - flowmeter 2 [ml/min].")]
        public float Channel2FlowReal { get; set; }

        /// <summary>
        /// Creates a message payload for the Channel2FlowReal register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public float GetPayload()
        {
            return Channel2FlowReal;
        }

        /// <summary>
        /// Creates a message that flow value read from channel 2 - flowmeter 2 [ml/min].
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the Channel2FlowReal register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.Channel2FlowReal.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that flow value read from channel 2 - flowmeter 2 [ml/min].
    /// </summary>
    [DisplayName("TimestampedChannel2FlowRealPayload")]
    [Description("Creates a timestamped message payload that flow value read from channel 2 - flowmeter 2 [ml/min].")]
    public partial class CreateTimestampedChannel2FlowRealPayload : CreateChannel2FlowRealPayload
    {
        /// <summary>
        /// Creates a timestamped message that flow value read from channel 2 - flowmeter 2 [ml/min].
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the Channel2FlowReal register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.Channel2FlowReal.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that flow value read from channel 3 - flowmeter 3 [ml/min].
    /// </summary>
    [DisplayName("Channel3FlowRealPayload")]
    [Description("Creates a message payload that flow value read from channel 3 - flowmeter 3 [ml/min].")]
    public partial class CreateChannel3FlowRealPayload
    {
        /// <summary>
        /// Gets or sets the value that flow value read from channel 3 - flowmeter 3 [ml/min].
        /// </summary>
        [Description("The value that flow value read from channel 3 - flowmeter 3 [ml/min].")]
        public float Channel3FlowReal { get; set; }

        /// <summary>
        /// Creates a message payload for the Channel3FlowReal register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public float GetPayload()
        {
            return Channel3FlowReal;
        }

        /// <summary>
        /// Creates a message that flow value read from channel 3 - flowmeter 3 [ml/min].
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the Channel3FlowReal register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.Channel3FlowReal.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that flow value read from channel 3 - flowmeter 3 [ml/min].
    /// </summary>
    [DisplayName("TimestampedChannel3FlowRealPayload")]
    [Description("Creates a timestamped message payload that flow value read from channel 3 - flowmeter 3 [ml/min].")]
    public partial class CreateTimestampedChannel3FlowRealPayload : CreateChannel3FlowRealPayload
    {
        /// <summary>
        /// Creates a timestamped message that flow value read from channel 3 - flowmeter 3 [ml/min].
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the Channel3FlowReal register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.Channel3FlowReal.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that flow value read from channel 4 - flowmeter 4 [ml/min].
    /// </summary>
    [DisplayName("Channel4FlowRealPayload")]
    [Description("Creates a message payload that flow value read from channel 4 - flowmeter 4 [ml/min].")]
    public partial class CreateChannel4FlowRealPayload
    {
        /// <summary>
        /// Gets or sets the value that flow value read from channel 4 - flowmeter 4 [ml/min].
        /// </summary>
        [Description("The value that flow value read from channel 4 - flowmeter 4 [ml/min].")]
        public float Channel4FlowReal { get; set; }

        /// <summary>
        /// Creates a message payload for the Channel4FlowReal register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public float GetPayload()
        {
            return Channel4FlowReal;
        }

        /// <summary>
        /// Creates a message that flow value read from channel 4 - flowmeter 4 [ml/min].
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the Channel4FlowReal register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.Channel4FlowReal.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that flow value read from channel 4 - flowmeter 4 [ml/min].
    /// </summary>
    [DisplayName("TimestampedChannel4FlowRealPayload")]
    [Description("Creates a timestamped message payload that flow value read from channel 4 - flowmeter 4 [ml/min].")]
    public partial class CreateTimestampedChannel4FlowRealPayload : CreateChannel4FlowRealPayload
    {
        /// <summary>
        /// Creates a timestamped message that flow value read from channel 4 - flowmeter 4 [ml/min].
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the Channel4FlowReal register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.Channel4FlowReal.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that duty cycle for proportional valve 0 [%].
    /// </summary>
    [DisplayName("Channel0DutyCyclePayload")]
    [Description("Creates a message payload that duty cycle for proportional valve 0 [%].")]
    public partial class CreateChannel0DutyCyclePayload
    {
        /// <summary>
        /// Gets or sets the value that duty cycle for proportional valve 0 [%].
        /// </summary>
        [Description("The value that duty cycle for proportional valve 0 [%].")]
        public float Channel0DutyCycle { get; set; }

        /// <summary>
        /// Creates a message payload for the Channel0DutyCycle register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public float GetPayload()
        {
            return Channel0DutyCycle;
        }

        /// <summary>
        /// Creates a message that duty cycle for proportional valve 0 [%].
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the Channel0DutyCycle register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.Channel0DutyCycle.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that duty cycle for proportional valve 0 [%].
    /// </summary>
    [DisplayName("TimestampedChannel0DutyCyclePayload")]
    [Description("Creates a timestamped message payload that duty cycle for proportional valve 0 [%].")]
    public partial class CreateTimestampedChannel0DutyCyclePayload : CreateChannel0DutyCyclePayload
    {
        /// <summary>
        /// Creates a timestamped message that duty cycle for proportional valve 0 [%].
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the Channel0DutyCycle register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.Channel0DutyCycle.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that duty cycle for proportional valve 1 [%].
    /// </summary>
    [DisplayName("Channel1DutyCyclePayload")]
    [Description("Creates a message payload that duty cycle for proportional valve 1 [%].")]
    public partial class CreateChannel1DutyCyclePayload
    {
        /// <summary>
        /// Gets or sets the value that duty cycle for proportional valve 1 [%].
        /// </summary>
        [Description("The value that duty cycle for proportional valve 1 [%].")]
        public float Channel1DutyCycle { get; set; }

        /// <summary>
        /// Creates a message payload for the Channel1DutyCycle register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public float GetPayload()
        {
            return Channel1DutyCycle;
        }

        /// <summary>
        /// Creates a message that duty cycle for proportional valve 1 [%].
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the Channel1DutyCycle register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.Channel1DutyCycle.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that duty cycle for proportional valve 1 [%].
    /// </summary>
    [DisplayName("TimestampedChannel1DutyCyclePayload")]
    [Description("Creates a timestamped message payload that duty cycle for proportional valve 1 [%].")]
    public partial class CreateTimestampedChannel1DutyCyclePayload : CreateChannel1DutyCyclePayload
    {
        /// <summary>
        /// Creates a timestamped message that duty cycle for proportional valve 1 [%].
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the Channel1DutyCycle register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.Channel1DutyCycle.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that duty cycle for proportional valve 2 [%].
    /// </summary>
    [DisplayName("Channel2DutyCyclePayload")]
    [Description("Creates a message payload that duty cycle for proportional valve 2 [%].")]
    public partial class CreateChannel2DutyCyclePayload
    {
        /// <summary>
        /// Gets or sets the value that duty cycle for proportional valve 2 [%].
        /// </summary>
        [Description("The value that duty cycle for proportional valve 2 [%].")]
        public float Channel2DutyCycle { get; set; }

        /// <summary>
        /// Creates a message payload for the Channel2DutyCycle register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public float GetPayload()
        {
            return Channel2DutyCycle;
        }

        /// <summary>
        /// Creates a message that duty cycle for proportional valve 2 [%].
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the Channel2DutyCycle register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.Channel2DutyCycle.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that duty cycle for proportional valve 2 [%].
    /// </summary>
    [DisplayName("TimestampedChannel2DutyCyclePayload")]
    [Description("Creates a timestamped message payload that duty cycle for proportional valve 2 [%].")]
    public partial class CreateTimestampedChannel2DutyCyclePayload : CreateChannel2DutyCyclePayload
    {
        /// <summary>
        /// Creates a timestamped message that duty cycle for proportional valve 2 [%].
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the Channel2DutyCycle register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.Channel2DutyCycle.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that duty cycle for proportional valve 3 [%].
    /// </summary>
    [DisplayName("Channel3DutyCyclePayload")]
    [Description("Creates a message payload that duty cycle for proportional valve 3 [%].")]
    public partial class CreateChannel3DutyCyclePayload
    {
        /// <summary>
        /// Gets or sets the value that duty cycle for proportional valve 3 [%].
        /// </summary>
        [Description("The value that duty cycle for proportional valve 3 [%].")]
        public float Channel3DutyCycle { get; set; }

        /// <summary>
        /// Creates a message payload for the Channel3DutyCycle register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public float GetPayload()
        {
            return Channel3DutyCycle;
        }

        /// <summary>
        /// Creates a message that duty cycle for proportional valve 3 [%].
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the Channel3DutyCycle register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.Channel3DutyCycle.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that duty cycle for proportional valve 3 [%].
    /// </summary>
    [DisplayName("TimestampedChannel3DutyCyclePayload")]
    [Description("Creates a timestamped message payload that duty cycle for proportional valve 3 [%].")]
    public partial class CreateTimestampedChannel3DutyCyclePayload : CreateChannel3DutyCyclePayload
    {
        /// <summary>
        /// Creates a timestamped message that duty cycle for proportional valve 3 [%].
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the Channel3DutyCycle register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.Channel3DutyCycle.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that duty cycle for proportional valve 4 [%].
    /// </summary>
    [DisplayName("Channel4DutyCyclePayload")]
    [Description("Creates a message payload that duty cycle for proportional valve 4 [%].")]
    public partial class CreateChannel4DutyCyclePayload
    {
        /// <summary>
        /// Gets or sets the value that duty cycle for proportional valve 4 [%].
        /// </summary>
        [Description("The value that duty cycle for proportional valve 4 [%].")]
        public float Channel4DutyCycle { get; set; }

        /// <summary>
        /// Creates a message payload for the Channel4DutyCycle register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public float GetPayload()
        {
            return Channel4DutyCycle;
        }

        /// <summary>
        /// Creates a message that duty cycle for proportional valve 4 [%].
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the Channel4DutyCycle register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.Channel4DutyCycle.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that duty cycle for proportional valve 4 [%].
    /// </summary>
    [DisplayName("TimestampedChannel4DutyCyclePayload")]
    [Description("Creates a timestamped message payload that duty cycle for proportional valve 4 [%].")]
    public partial class CreateTimestampedChannel4DutyCyclePayload : CreateChannel4DutyCyclePayload
    {
        /// <summary>
        /// Creates a timestamped message that duty cycle for proportional valve 4 [%].
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the Channel4DutyCycle register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.Channel4DutyCycle.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that set the correspondent output.
    /// </summary>
    [DisplayName("DigitalOutputSetPayload")]
    [Description("Creates a message payload that set the correspondent output.")]
    public partial class CreateDigitalOutputSetPayload
    {
        /// <summary>
        /// Gets or sets the value that set the correspondent output.
        /// </summary>
        [Description("The value that set the correspondent output.")]
        public DigitalOutputs DigitalOutputSet { get; set; }

        /// <summary>
        /// Creates a message payload for the DigitalOutputSet register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public DigitalOutputs GetPayload()
        {
            return DigitalOutputSet;
        }

        /// <summary>
        /// Creates a message that set the correspondent output.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the DigitalOutputSet register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.DigitalOutputSet.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that set the correspondent output.
    /// </summary>
    [DisplayName("TimestampedDigitalOutputSetPayload")]
    [Description("Creates a timestamped message payload that set the correspondent output.")]
    public partial class CreateTimestampedDigitalOutputSetPayload : CreateDigitalOutputSetPayload
    {
        /// <summary>
        /// Creates a timestamped message that set the correspondent output.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the DigitalOutputSet register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.DigitalOutputSet.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that clear the correspondent output.
    /// </summary>
    [DisplayName("DigitalOutputClearPayload")]
    [Description("Creates a message payload that clear the correspondent output.")]
    public partial class CreateDigitalOutputClearPayload
    {
        /// <summary>
        /// Gets or sets the value that clear the correspondent output.
        /// </summary>
        [Description("The value that clear the correspondent output.")]
        public DigitalOutputs DigitalOutputClear { get; set; }

        /// <summary>
        /// Creates a message payload for the DigitalOutputClear register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public DigitalOutputs GetPayload()
        {
            return DigitalOutputClear;
        }

        /// <summary>
        /// Creates a message that clear the correspondent output.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the DigitalOutputClear register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.DigitalOutputClear.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that clear the correspondent output.
    /// </summary>
    [DisplayName("TimestampedDigitalOutputClearPayload")]
    [Description("Creates a timestamped message payload that clear the correspondent output.")]
    public partial class CreateTimestampedDigitalOutputClearPayload : CreateDigitalOutputClearPayload
    {
        /// <summary>
        /// Creates a timestamped message that clear the correspondent output.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the DigitalOutputClear register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.DigitalOutputClear.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that toggle the correspondent output.
    /// </summary>
    [DisplayName("DigitalOutputTogglePayload")]
    [Description("Creates a message payload that toggle the correspondent output.")]
    public partial class CreateDigitalOutputTogglePayload
    {
        /// <summary>
        /// Gets or sets the value that toggle the correspondent output.
        /// </summary>
        [Description("The value that toggle the correspondent output.")]
        public DigitalOutputs DigitalOutputToggle { get; set; }

        /// <summary>
        /// Creates a message payload for the DigitalOutputToggle register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public DigitalOutputs GetPayload()
        {
            return DigitalOutputToggle;
        }

        /// <summary>
        /// Creates a message that toggle the correspondent output.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the DigitalOutputToggle register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.DigitalOutputToggle.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that toggle the correspondent output.
    /// </summary>
    [DisplayName("TimestampedDigitalOutputTogglePayload")]
    [Description("Creates a timestamped message payload that toggle the correspondent output.")]
    public partial class CreateTimestampedDigitalOutputTogglePayload : CreateDigitalOutputTogglePayload
    {
        /// <summary>
        /// Creates a timestamped message that toggle the correspondent output.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the DigitalOutputToggle register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.DigitalOutputToggle.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that control the correspondent output.
    /// </summary>
    [DisplayName("DigitalOutputStatePayload")]
    [Description("Creates a message payload that control the correspondent output.")]
    public partial class CreateDigitalOutputStatePayload
    {
        /// <summary>
        /// Gets or sets the value that control the correspondent output.
        /// </summary>
        [Description("The value that control the correspondent output.")]
        public DigitalOutputs DigitalOutputState { get; set; }

        /// <summary>
        /// Creates a message payload for the DigitalOutputState register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public DigitalOutputs GetPayload()
        {
            return DigitalOutputState;
        }

        /// <summary>
        /// Creates a message that control the correspondent output.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the DigitalOutputState register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.DigitalOutputState.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that control the correspondent output.
    /// </summary>
    [DisplayName("TimestampedDigitalOutputStatePayload")]
    [Description("Creates a timestamped message payload that control the correspondent output.")]
    public partial class CreateTimestampedDigitalOutputStatePayload : CreateDigitalOutputStatePayload
    {
        /// <summary>
        /// Creates a timestamped message that control the correspondent output.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the DigitalOutputState register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.DigitalOutputState.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that enable pulse mode for valves.
    /// </summary>
    [DisplayName("EnableValvesPulsePayload")]
    [Description("Creates a message payload that enable pulse mode for valves.")]
    public partial class CreateEnableValvesPulsePayload
    {
        /// <summary>
        /// Gets or sets the value that enable pulse mode for valves.
        /// </summary>
        [Description("The value that enable pulse mode for valves.")]
        public Valves EnableValvesPulse { get; set; }

        /// <summary>
        /// Creates a message payload for the EnableValvesPulse register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public Valves GetPayload()
        {
            return EnableValvesPulse;
        }

        /// <summary>
        /// Creates a message that enable pulse mode for valves.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the EnableValvesPulse register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.EnableValvesPulse.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that enable pulse mode for valves.
    /// </summary>
    [DisplayName("TimestampedEnableValvesPulsePayload")]
    [Description("Creates a timestamped message payload that enable pulse mode for valves.")]
    public partial class CreateTimestampedEnableValvesPulsePayload : CreateEnableValvesPulsePayload
    {
        /// <summary>
        /// Creates a timestamped message that enable pulse mode for valves.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the EnableValvesPulse register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.EnableValvesPulse.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that set the correspondent valve.
    /// </summary>
    [DisplayName("ValvesSetPayload")]
    [Description("Creates a message payload that set the correspondent valve.")]
    public partial class CreateValvesSetPayload
    {
        /// <summary>
        /// Gets or sets the value that set the correspondent valve.
        /// </summary>
        [Description("The value that set the correspondent valve.")]
        public Valves ValvesSet { get; set; }

        /// <summary>
        /// Creates a message payload for the ValvesSet register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public Valves GetPayload()
        {
            return ValvesSet;
        }

        /// <summary>
        /// Creates a message that set the correspondent valve.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the ValvesSet register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.ValvesSet.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that set the correspondent valve.
    /// </summary>
    [DisplayName("TimestampedValvesSetPayload")]
    [Description("Creates a timestamped message payload that set the correspondent valve.")]
    public partial class CreateTimestampedValvesSetPayload : CreateValvesSetPayload
    {
        /// <summary>
        /// Creates a timestamped message that set the correspondent valve.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the ValvesSet register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.ValvesSet.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that clear the correspondent valve.
    /// </summary>
    [DisplayName("ValvesClearPayload")]
    [Description("Creates a message payload that clear the correspondent valve.")]
    public partial class CreateValvesClearPayload
    {
        /// <summary>
        /// Gets or sets the value that clear the correspondent valve.
        /// </summary>
        [Description("The value that clear the correspondent valve.")]
        public Valves ValvesClear { get; set; }

        /// <summary>
        /// Creates a message payload for the ValvesClear register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public Valves GetPayload()
        {
            return ValvesClear;
        }

        /// <summary>
        /// Creates a message that clear the correspondent valve.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the ValvesClear register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.ValvesClear.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that clear the correspondent valve.
    /// </summary>
    [DisplayName("TimestampedValvesClearPayload")]
    [Description("Creates a timestamped message payload that clear the correspondent valve.")]
    public partial class CreateTimestampedValvesClearPayload : CreateValvesClearPayload
    {
        /// <summary>
        /// Creates a timestamped message that clear the correspondent valve.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the ValvesClear register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.ValvesClear.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that toggle the correspondent valve.
    /// </summary>
    [DisplayName("ValvesTogglePayload")]
    [Description("Creates a message payload that toggle the correspondent valve.")]
    public partial class CreateValvesTogglePayload
    {
        /// <summary>
        /// Gets or sets the value that toggle the correspondent valve.
        /// </summary>
        [Description("The value that toggle the correspondent valve.")]
        public Valves ValvesToggle { get; set; }

        /// <summary>
        /// Creates a message payload for the ValvesToggle register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public Valves GetPayload()
        {
            return ValvesToggle;
        }

        /// <summary>
        /// Creates a message that toggle the correspondent valve.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the ValvesToggle register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.ValvesToggle.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that toggle the correspondent valve.
    /// </summary>
    [DisplayName("TimestampedValvesTogglePayload")]
    [Description("Creates a timestamped message payload that toggle the correspondent valve.")]
    public partial class CreateTimestampedValvesTogglePayload : CreateValvesTogglePayload
    {
        /// <summary>
        /// Creates a timestamped message that toggle the correspondent valve.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the ValvesToggle register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.ValvesToggle.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that control the correspondent valve.
    /// </summary>
    [DisplayName("ValvesStatePayload")]
    [Description("Creates a message payload that control the correspondent valve.")]
    public partial class CreateValvesStatePayload
    {
        /// <summary>
        /// Gets or sets the value that control the correspondent valve.
        /// </summary>
        [Description("The value that control the correspondent valve.")]
        public Valves ValvesState { get; set; }

        /// <summary>
        /// Creates a message payload for the ValvesState register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public Valves GetPayload()
        {
            return ValvesState;
        }

        /// <summary>
        /// Creates a message that control the correspondent valve.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the ValvesState register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.ValvesState.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that control the correspondent valve.
    /// </summary>
    [DisplayName("TimestampedValvesStatePayload")]
    [Description("Creates a timestamped message payload that control the correspondent valve.")]
    public partial class CreateTimestampedValvesStatePayload : CreateValvesStatePayload
    {
        /// <summary>
        /// Creates a timestamped message that control the correspondent valve.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the ValvesState register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.ValvesState.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that valve 0 pulse duration [1:65535] ms.
    /// </summary>
    [DisplayName("PulseValve0Payload")]
    [Description("Creates a message payload that valve 0 pulse duration [1:65535] ms.")]
    public partial class CreatePulseValve0Payload
    {
        /// <summary>
        /// Gets or sets the value that valve 0 pulse duration [1:65535] ms.
        /// </summary>
        [Description("The value that valve 0 pulse duration [1:65535] ms.")]
        public ushort PulseValve0 { get; set; }

        /// <summary>
        /// Creates a message payload for the PulseValve0 register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public ushort GetPayload()
        {
            return PulseValve0;
        }

        /// <summary>
        /// Creates a message that valve 0 pulse duration [1:65535] ms.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the PulseValve0 register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.PulseValve0.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that valve 0 pulse duration [1:65535] ms.
    /// </summary>
    [DisplayName("TimestampedPulseValve0Payload")]
    [Description("Creates a timestamped message payload that valve 0 pulse duration [1:65535] ms.")]
    public partial class CreateTimestampedPulseValve0Payload : CreatePulseValve0Payload
    {
        /// <summary>
        /// Creates a timestamped message that valve 0 pulse duration [1:65535] ms.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the PulseValve0 register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.PulseValve0.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that valve 1 pulse duration [1:65535] ms.
    /// </summary>
    [DisplayName("PulseValve1Payload")]
    [Description("Creates a message payload that valve 1 pulse duration [1:65535] ms.")]
    public partial class CreatePulseValve1Payload
    {
        /// <summary>
        /// Gets or sets the value that valve 1 pulse duration [1:65535] ms.
        /// </summary>
        [Description("The value that valve 1 pulse duration [1:65535] ms.")]
        public ushort PulseValve1 { get; set; }

        /// <summary>
        /// Creates a message payload for the PulseValve1 register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public ushort GetPayload()
        {
            return PulseValve1;
        }

        /// <summary>
        /// Creates a message that valve 1 pulse duration [1:65535] ms.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the PulseValve1 register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.PulseValve1.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that valve 1 pulse duration [1:65535] ms.
    /// </summary>
    [DisplayName("TimestampedPulseValve1Payload")]
    [Description("Creates a timestamped message payload that valve 1 pulse duration [1:65535] ms.")]
    public partial class CreateTimestampedPulseValve1Payload : CreatePulseValve1Payload
    {
        /// <summary>
        /// Creates a timestamped message that valve 1 pulse duration [1:65535] ms.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the PulseValve1 register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.PulseValve1.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that valve 2 pulse duration [1:65535] ms.
    /// </summary>
    [DisplayName("PulseValve2Payload")]
    [Description("Creates a message payload that valve 2 pulse duration [1:65535] ms.")]
    public partial class CreatePulseValve2Payload
    {
        /// <summary>
        /// Gets or sets the value that valve 2 pulse duration [1:65535] ms.
        /// </summary>
        [Description("The value that valve 2 pulse duration [1:65535] ms.")]
        public ushort PulseValve2 { get; set; }

        /// <summary>
        /// Creates a message payload for the PulseValve2 register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public ushort GetPayload()
        {
            return PulseValve2;
        }

        /// <summary>
        /// Creates a message that valve 2 pulse duration [1:65535] ms.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the PulseValve2 register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.PulseValve2.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that valve 2 pulse duration [1:65535] ms.
    /// </summary>
    [DisplayName("TimestampedPulseValve2Payload")]
    [Description("Creates a timestamped message payload that valve 2 pulse duration [1:65535] ms.")]
    public partial class CreateTimestampedPulseValve2Payload : CreatePulseValve2Payload
    {
        /// <summary>
        /// Creates a timestamped message that valve 2 pulse duration [1:65535] ms.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the PulseValve2 register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.PulseValve2.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that valve 3 pulse duration [1:65535] ms.
    /// </summary>
    [DisplayName("PulseValve3Payload")]
    [Description("Creates a message payload that valve 3 pulse duration [1:65535] ms.")]
    public partial class CreatePulseValve3Payload
    {
        /// <summary>
        /// Gets or sets the value that valve 3 pulse duration [1:65535] ms.
        /// </summary>
        [Description("The value that valve 3 pulse duration [1:65535] ms.")]
        public ushort PulseValve3 { get; set; }

        /// <summary>
        /// Creates a message payload for the PulseValve3 register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public ushort GetPayload()
        {
            return PulseValve3;
        }

        /// <summary>
        /// Creates a message that valve 3 pulse duration [1:65535] ms.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the PulseValve3 register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.PulseValve3.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that valve 3 pulse duration [1:65535] ms.
    /// </summary>
    [DisplayName("TimestampedPulseValve3Payload")]
    [Description("Creates a timestamped message payload that valve 3 pulse duration [1:65535] ms.")]
    public partial class CreateTimestampedPulseValve3Payload : CreatePulseValve3Payload
    {
        /// <summary>
        /// Creates a timestamped message that valve 3 pulse duration [1:65535] ms.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the PulseValve3 register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.PulseValve3.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that end valve 0 pulse duration [1:65535] ms.
    /// </summary>
    [DisplayName("PulseEndvalve0Payload")]
    [Description("Creates a message payload that end valve 0 pulse duration [1:65535] ms.")]
    public partial class CreatePulseEndvalve0Payload
    {
        /// <summary>
        /// Gets or sets the value that end valve 0 pulse duration [1:65535] ms.
        /// </summary>
        [Description("The value that end valve 0 pulse duration [1:65535] ms.")]
        public ushort PulseEndvalve0 { get; set; }

        /// <summary>
        /// Creates a message payload for the PulseEndvalve0 register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public ushort GetPayload()
        {
            return PulseEndvalve0;
        }

        /// <summary>
        /// Creates a message that end valve 0 pulse duration [1:65535] ms.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the PulseEndvalve0 register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.PulseEndvalve0.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that end valve 0 pulse duration [1:65535] ms.
    /// </summary>
    [DisplayName("TimestampedPulseEndvalve0Payload")]
    [Description("Creates a timestamped message payload that end valve 0 pulse duration [1:65535] ms.")]
    public partial class CreateTimestampedPulseEndvalve0Payload : CreatePulseEndvalve0Payload
    {
        /// <summary>
        /// Creates a timestamped message that end valve 0 pulse duration [1:65535] ms.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the PulseEndvalve0 register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.PulseEndvalve0.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that end valve 1 pulse duration [1:65535] ms.
    /// </summary>
    [DisplayName("PulseEndvalve1Payload")]
    [Description("Creates a message payload that end valve 1 pulse duration [1:65535] ms.")]
    public partial class CreatePulseEndvalve1Payload
    {
        /// <summary>
        /// Gets or sets the value that end valve 1 pulse duration [1:65535] ms.
        /// </summary>
        [Description("The value that end valve 1 pulse duration [1:65535] ms.")]
        public ushort PulseEndvalve1 { get; set; }

        /// <summary>
        /// Creates a message payload for the PulseEndvalve1 register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public ushort GetPayload()
        {
            return PulseEndvalve1;
        }

        /// <summary>
        /// Creates a message that end valve 1 pulse duration [1:65535] ms.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the PulseEndvalve1 register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.PulseEndvalve1.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that end valve 1 pulse duration [1:65535] ms.
    /// </summary>
    [DisplayName("TimestampedPulseEndvalve1Payload")]
    [Description("Creates a timestamped message payload that end valve 1 pulse duration [1:65535] ms.")]
    public partial class CreateTimestampedPulseEndvalve1Payload : CreatePulseEndvalve1Payload
    {
        /// <summary>
        /// Creates a timestamped message that end valve 1 pulse duration [1:65535] ms.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the PulseEndvalve1 register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.PulseEndvalve1.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that dummy valve pulse duration [1:65535] ms.
    /// </summary>
    [DisplayName("PulseDummyvalvePayload")]
    [Description("Creates a message payload that dummy valve pulse duration [1:65535] ms.")]
    public partial class CreatePulseDummyvalvePayload
    {
        /// <summary>
        /// Gets or sets the value that dummy valve pulse duration [1:65535] ms.
        /// </summary>
        [Description("The value that dummy valve pulse duration [1:65535] ms.")]
        public ushort PulseDummyvalve { get; set; }

        /// <summary>
        /// Creates a message payload for the PulseDummyvalve register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public ushort GetPayload()
        {
            return PulseDummyvalve;
        }

        /// <summary>
        /// Creates a message that dummy valve pulse duration [1:65535] ms.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the PulseDummyvalve register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.PulseDummyvalve.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that dummy valve pulse duration [1:65535] ms.
    /// </summary>
    [DisplayName("TimestampedPulseDummyvalvePayload")]
    [Description("Creates a timestamped message payload that dummy valve pulse duration [1:65535] ms.")]
    public partial class CreateTimestampedPulseDummyvalvePayload : CreatePulseDummyvalvePayload
    {
        /// <summary>
        /// Creates a timestamped message that dummy valve pulse duration [1:65535] ms.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the PulseDummyvalve register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.PulseDummyvalve.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that configuration of the digital output 0 (DOUT0).
    /// </summary>
    [DisplayName("DO0SyncPayload")]
    [Description("Creates a message payload that configuration of the digital output 0 (DOUT0).")]
    public partial class CreateDO0SyncPayload
    {
        /// <summary>
        /// Gets or sets the value that configuration of the digital output 0 (DOUT0).
        /// </summary>
        [Description("The value that configuration of the digital output 0 (DOUT0).")]
        public DO0SyncConfig DO0Sync { get; set; }

        /// <summary>
        /// Creates a message payload for the DO0Sync register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public DO0SyncConfig GetPayload()
        {
            return DO0Sync;
        }

        /// <summary>
        /// Creates a message that configuration of the digital output 0 (DOUT0).
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the DO0Sync register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.DO0Sync.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that configuration of the digital output 0 (DOUT0).
    /// </summary>
    [DisplayName("TimestampedDO0SyncPayload")]
    [Description("Creates a timestamped message payload that configuration of the digital output 0 (DOUT0).")]
    public partial class CreateTimestampedDO0SyncPayload : CreateDO0SyncPayload
    {
        /// <summary>
        /// Creates a timestamped message that configuration of the digital output 0 (DOUT0).
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the DO0Sync register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.DO0Sync.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that configuration of the digital output 1 (DOUT1).
    /// </summary>
    [DisplayName("DO1SyncPayload")]
    [Description("Creates a message payload that configuration of the digital output 1 (DOUT1).")]
    public partial class CreateDO1SyncPayload
    {
        /// <summary>
        /// Gets or sets the value that configuration of the digital output 1 (DOUT1).
        /// </summary>
        [Description("The value that configuration of the digital output 1 (DOUT1).")]
        public DO1SyncConfig DO1Sync { get; set; }

        /// <summary>
        /// Creates a message payload for the DO1Sync register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public DO1SyncConfig GetPayload()
        {
            return DO1Sync;
        }

        /// <summary>
        /// Creates a message that configuration of the digital output 1 (DOUT1).
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the DO1Sync register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.DO1Sync.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that configuration of the digital output 1 (DOUT1).
    /// </summary>
    [DisplayName("TimestampedDO1SyncPayload")]
    [Description("Creates a timestamped message payload that configuration of the digital output 1 (DOUT1).")]
    public partial class CreateTimestampedDO1SyncPayload : CreateDO1SyncPayload
    {
        /// <summary>
        /// Creates a timestamped message that configuration of the digital output 1 (DOUT1).
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the DO1Sync register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.DO1Sync.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that configuration of the digital input pin 0 (DIN0).
    /// </summary>
    [DisplayName("DI0TriggerPayload")]
    [Description("Creates a message payload that configuration of the digital input pin 0 (DIN0).")]
    public partial class CreateDI0TriggerPayload
    {
        /// <summary>
        /// Gets or sets the value that configuration of the digital input pin 0 (DIN0).
        /// </summary>
        [Description("The value that configuration of the digital input pin 0 (DIN0).")]
        public DI0TriggerConfig DI0Trigger { get; set; }

        /// <summary>
        /// Creates a message payload for the DI0Trigger register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public DI0TriggerConfig GetPayload()
        {
            return DI0Trigger;
        }

        /// <summary>
        /// Creates a message that configuration of the digital input pin 0 (DIN0).
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the DI0Trigger register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.DI0Trigger.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that configuration of the digital input pin 0 (DIN0).
    /// </summary>
    [DisplayName("TimestampedDI0TriggerPayload")]
    [Description("Creates a timestamped message payload that configuration of the digital input pin 0 (DIN0).")]
    public partial class CreateTimestampedDI0TriggerPayload : CreateDI0TriggerPayload
    {
        /// <summary>
        /// Creates a timestamped message that configuration of the digital input pin 0 (DIN0).
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the DI0Trigger register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.DI0Trigger.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that mimic valve 0.
    /// </summary>
    [DisplayName("MimicValve0Payload")]
    [Description("Creates a message payload that mimic valve 0.")]
    public partial class CreateMimicValve0Payload
    {
        /// <summary>
        /// Gets or sets the value that mimic valve 0.
        /// </summary>
        [Description("The value that mimic valve 0.")]
        public MimicOuputs MimicValve0 { get; set; }

        /// <summary>
        /// Creates a message payload for the MimicValve0 register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public MimicOuputs GetPayload()
        {
            return MimicValve0;
        }

        /// <summary>
        /// Creates a message that mimic valve 0.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the MimicValve0 register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.MimicValve0.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that mimic valve 0.
    /// </summary>
    [DisplayName("TimestampedMimicValve0Payload")]
    [Description("Creates a timestamped message payload that mimic valve 0.")]
    public partial class CreateTimestampedMimicValve0Payload : CreateMimicValve0Payload
    {
        /// <summary>
        /// Creates a timestamped message that mimic valve 0.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the MimicValve0 register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.MimicValve0.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that mimic valve 1.
    /// </summary>
    [DisplayName("MimicValve1Payload")]
    [Description("Creates a message payload that mimic valve 1.")]
    public partial class CreateMimicValve1Payload
    {
        /// <summary>
        /// Gets or sets the value that mimic valve 1.
        /// </summary>
        [Description("The value that mimic valve 1.")]
        public MimicOuputs MimicValve1 { get; set; }

        /// <summary>
        /// Creates a message payload for the MimicValve1 register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public MimicOuputs GetPayload()
        {
            return MimicValve1;
        }

        /// <summary>
        /// Creates a message that mimic valve 1.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the MimicValve1 register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.MimicValve1.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that mimic valve 1.
    /// </summary>
    [DisplayName("TimestampedMimicValve1Payload")]
    [Description("Creates a timestamped message payload that mimic valve 1.")]
    public partial class CreateTimestampedMimicValve1Payload : CreateMimicValve1Payload
    {
        /// <summary>
        /// Creates a timestamped message that mimic valve 1.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the MimicValve1 register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.MimicValve1.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that mimic valve 2.
    /// </summary>
    [DisplayName("MimicValve2Payload")]
    [Description("Creates a message payload that mimic valve 2.")]
    public partial class CreateMimicValve2Payload
    {
        /// <summary>
        /// Gets or sets the value that mimic valve 2.
        /// </summary>
        [Description("The value that mimic valve 2.")]
        public MimicOuputs MimicValve2 { get; set; }

        /// <summary>
        /// Creates a message payload for the MimicValve2 register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public MimicOuputs GetPayload()
        {
            return MimicValve2;
        }

        /// <summary>
        /// Creates a message that mimic valve 2.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the MimicValve2 register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.MimicValve2.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that mimic valve 2.
    /// </summary>
    [DisplayName("TimestampedMimicValve2Payload")]
    [Description("Creates a timestamped message payload that mimic valve 2.")]
    public partial class CreateTimestampedMimicValve2Payload : CreateMimicValve2Payload
    {
        /// <summary>
        /// Creates a timestamped message that mimic valve 2.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the MimicValve2 register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.MimicValve2.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that mimic valve 3.
    /// </summary>
    [DisplayName("MimicValve3Payload")]
    [Description("Creates a message payload that mimic valve 3.")]
    public partial class CreateMimicValve3Payload
    {
        /// <summary>
        /// Gets or sets the value that mimic valve 3.
        /// </summary>
        [Description("The value that mimic valve 3.")]
        public MimicOuputs MimicValve3 { get; set; }

        /// <summary>
        /// Creates a message payload for the MimicValve3 register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public MimicOuputs GetPayload()
        {
            return MimicValve3;
        }

        /// <summary>
        /// Creates a message that mimic valve 3.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the MimicValve3 register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.MimicValve3.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that mimic valve 3.
    /// </summary>
    [DisplayName("TimestampedMimicValve3Payload")]
    [Description("Creates a timestamped message payload that mimic valve 3.")]
    public partial class CreateTimestampedMimicValve3Payload : CreateMimicValve3Payload
    {
        /// <summary>
        /// Creates a timestamped message that mimic valve 3.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the MimicValve3 register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.MimicValve3.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that mimic end valve 0.
    /// </summary>
    [DisplayName("MimicEndvalve0Payload")]
    [Description("Creates a message payload that mimic end valve 0.")]
    public partial class CreateMimicEndvalve0Payload
    {
        /// <summary>
        /// Gets or sets the value that mimic end valve 0.
        /// </summary>
        [Description("The value that mimic end valve 0.")]
        public MimicOuputs MimicEndvalve0 { get; set; }

        /// <summary>
        /// Creates a message payload for the MimicEndvalve0 register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public MimicOuputs GetPayload()
        {
            return MimicEndvalve0;
        }

        /// <summary>
        /// Creates a message that mimic end valve 0.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the MimicEndvalve0 register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.MimicEndvalve0.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that mimic end valve 0.
    /// </summary>
    [DisplayName("TimestampedMimicEndvalve0Payload")]
    [Description("Creates a timestamped message payload that mimic end valve 0.")]
    public partial class CreateTimestampedMimicEndvalve0Payload : CreateMimicEndvalve0Payload
    {
        /// <summary>
        /// Creates a timestamped message that mimic end valve 0.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the MimicEndvalve0 register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.MimicEndvalve0.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that mimic end valve 1.
    /// </summary>
    [DisplayName("MimicEndvalve1Payload")]
    [Description("Creates a message payload that mimic end valve 1.")]
    public partial class CreateMimicEndvalve1Payload
    {
        /// <summary>
        /// Gets or sets the value that mimic end valve 1.
        /// </summary>
        [Description("The value that mimic end valve 1.")]
        public MimicOuputs MimicEndvalve1 { get; set; }

        /// <summary>
        /// Creates a message payload for the MimicEndvalve1 register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public MimicOuputs GetPayload()
        {
            return MimicEndvalve1;
        }

        /// <summary>
        /// Creates a message that mimic end valve 1.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the MimicEndvalve1 register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.MimicEndvalve1.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that mimic end valve 1.
    /// </summary>
    [DisplayName("TimestampedMimicEndvalve1Payload")]
    [Description("Creates a timestamped message payload that mimic end valve 1.")]
    public partial class CreateTimestampedMimicEndvalve1Payload : CreateMimicEndvalve1Payload
    {
        /// <summary>
        /// Creates a timestamped message that mimic end valve 1.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the MimicEndvalve1 register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.MimicEndvalve1.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that mimic dummy valve.
    /// </summary>
    [DisplayName("MimicDummyvalvePayload")]
    [Description("Creates a message payload that mimic dummy valve.")]
    public partial class CreateMimicDummyvalvePayload
    {
        /// <summary>
        /// Gets or sets the value that mimic dummy valve.
        /// </summary>
        [Description("The value that mimic dummy valve.")]
        public MimicOuputs MimicDummyvalve { get; set; }

        /// <summary>
        /// Creates a message payload for the MimicDummyvalve register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public MimicOuputs GetPayload()
        {
            return MimicDummyvalve;
        }

        /// <summary>
        /// Creates a message that mimic dummy valve.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the MimicDummyvalve register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.MimicDummyvalve.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that mimic dummy valve.
    /// </summary>
    [DisplayName("TimestampedMimicDummyvalvePayload")]
    [Description("Creates a timestamped message payload that mimic dummy valve.")]
    public partial class CreateTimestampedMimicDummyvalvePayload : CreateMimicDummyvalvePayload
    {
        /// <summary>
        /// Creates a timestamped message that mimic dummy valve.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the MimicDummyvalve register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.MimicDummyvalve.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that enable the valves control via the external screw terminals.
    /// </summary>
    [DisplayName("EnableExternalControlValvesPayload")]
    [Description("Creates a message payload that enable the valves control via the external screw terminals.")]
    public partial class CreateEnableExternalControlValvesPayload
    {
        /// <summary>
        /// Gets or sets the value that enable the valves control via the external screw terminals.
        /// </summary>
        [Description("The value that enable the valves control via the external screw terminals.")]
        public EnableFlag EnableExternalControlValves { get; set; }

        /// <summary>
        /// Creates a message payload for the EnableExternalControlValves register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public EnableFlag GetPayload()
        {
            return EnableExternalControlValves;
        }

        /// <summary>
        /// Creates a message that enable the valves control via the external screw terminals.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the EnableExternalControlValves register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.EnableExternalControlValves.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that enable the valves control via the external screw terminals.
    /// </summary>
    [DisplayName("TimestampedEnableExternalControlValvesPayload")]
    [Description("Creates a timestamped message payload that enable the valves control via the external screw terminals.")]
    public partial class CreateTimestampedEnableExternalControlValvesPayload : CreateEnableExternalControlValvesPayload
    {
        /// <summary>
        /// Creates a timestamped message that enable the valves control via the external screw terminals.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the EnableExternalControlValves register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.EnableExternalControlValves.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that selects the flow range for the channel 3 (0-100ml/min or 0-1000ml/min).
    /// </summary>
    [DisplayName("Channel3RangePayload")]
    [Description("Creates a message payload that selects the flow range for the channel 3 (0-100ml/min or 0-1000ml/min).")]
    public partial class CreateChannel3RangePayload
    {
        /// <summary>
        /// Gets or sets the value that selects the flow range for the channel 3 (0-100ml/min or 0-1000ml/min).
        /// </summary>
        [Description("The value that selects the flow range for the channel 3 (0-100ml/min or 0-1000ml/min).")]
        public Channel3RangeConfig Channel3Range { get; set; }

        /// <summary>
        /// Creates a message payload for the Channel3Range register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public Channel3RangeConfig GetPayload()
        {
            return Channel3Range;
        }

        /// <summary>
        /// Creates a message that selects the flow range for the channel 3 (0-100ml/min or 0-1000ml/min).
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the Channel3Range register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.Channel3Range.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that selects the flow range for the channel 3 (0-100ml/min or 0-1000ml/min).
    /// </summary>
    [DisplayName("TimestampedChannel3RangePayload")]
    [Description("Creates a timestamped message payload that selects the flow range for the channel 3 (0-100ml/min or 0-1000ml/min).")]
    public partial class CreateTimestampedChannel3RangePayload : CreateChannel3RangePayload
    {
        /// <summary>
        /// Creates a timestamped message that selects the flow range for the channel 3 (0-100ml/min or 0-1000ml/min).
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the Channel3Range register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.Channel3Range.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that specifies the active events in the device.
    /// </summary>
    [DisplayName("EnableEventsPayload")]
    [Description("Creates a message payload that specifies the active events in the device.")]
    public partial class CreateEnableEventsPayload
    {
        /// <summary>
        /// Gets or sets the value that specifies the active events in the device.
        /// </summary>
        [Description("The value that specifies the active events in the device.")]
        public OlfactometerEvents EnableEvents { get; set; }

        /// <summary>
        /// Creates a message payload for the EnableEvents register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public OlfactometerEvents GetPayload()
        {
            return EnableEvents;
        }

        /// <summary>
        /// Creates a message that specifies the active events in the device.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the EnableEvents register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.EnableEvents.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that specifies the active events in the device.
    /// </summary>
    [DisplayName("TimestampedEnableEventsPayload")]
    [Description("Creates a timestamped message payload that specifies the active events in the device.")]
    public partial class CreateTimestampedEnableEventsPayload : CreateEnableEventsPayload
    {
        /// <summary>
        /// Creates a timestamped message that specifies the active events in the device.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the EnableEvents register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.EnableEvents.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Specifies the states of the digital outputs.
    /// </summary>
    [Flags]
    public enum DigitalOutputs : byte
    {
        None = 0x0,
        DO0 = 0x1,
        DO1 = 0x2
    }

    /// <summary>
    /// Specifies the states of the valves.
    /// </summary>
    [Flags]
    public enum Valves : byte
    {
        None = 0x0,
        Valve0 = 0x1,
        Valve1 = 0x2,
        Valve2 = 0x4,
        Valve3 = 0x8,
        EndValve0 = 0x10,
        EndValve1 = 0x20,
        ValveDummy = 0x40
    }

    /// <summary>
    /// The events that can be enabled/disabled.
    /// </summary>
    [Flags]
    public enum OlfactometerEvents : byte
    {
        None = 0x0,
        FlowmeterAnalogOutputs = 0x1,
        DI0Trigger = 0x2,
        ChannelxFlowReal = 0x4
    }

    /// <summary>
    /// The state of a digital pin.
    /// </summary>
    public enum DigitalState : byte
    {
        Low = 0,
        High = 1
    }

    /// <summary>
    /// Available configurations when using DO0 pin to report firmware events.
    /// </summary>
    public enum DO0SyncConfig : byte
    {
        None = 0,
        MimicEnableFlow = 1
    }

    /// <summary>
    /// Available configurations when using DO1 pin to report firmware events.
    /// </summary>
    public enum DO1SyncConfig : byte
    {
        None = 0,
        MimicEnableFlow = 1
    }

    /// <summary>
    /// Specifies the configuration of the digital input 0 (DIN0).
    /// </summary>
    public enum DI0TriggerConfig : byte
    {
        Sync = 0,
        StartOnRisingEdgeStopOnFallingEdge = 1,
        ValveToggle = 2
    }

    /// <summary>
    /// Specifies the target IO on which to mimic the specified register.
    /// </summary>
    public enum MimicOuputs : byte
    {
        None = 0,
        DO0 = 1,
        DO1 = 2
    }

    /// <summary>
    /// Available flow ranges for channel 3 (ml/min).
    /// </summary>
    public enum Channel3RangeConfig : byte
    {
        Flow100 = 0,
        Flow1000 = 1
    }
}
