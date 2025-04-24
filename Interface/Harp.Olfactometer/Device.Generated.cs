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
            { 33, typeof(Flowmeter) },
            { 34, typeof(DI0State) },
            { 35, typeof(Channel0UserCalibration) },
            { 36, typeof(Channel1UserCalibration) },
            { 37, typeof(Channel2UserCalibration) },
            { 38, typeof(Channel3UserCalibration) },
            { 39, typeof(Channel4UserCalibration) },
            { 40, typeof(Channel3UserCalibrationAux) },
            { 41, typeof(UserCalibrationEnable) },
            { 42, typeof(Channel0TargetFlow) },
            { 43, typeof(Channel1TargetFlow) },
            { 44, typeof(Channel2TargetFlow) },
            { 45, typeof(Channel3TargetFlow) },
            { 46, typeof(Channel4TargetFlow) },
            { 47, typeof(ChannelsTargetFlow) },
            { 48, typeof(Channel0ActualFlow) },
            { 49, typeof(Channel1ActualFlow) },
            { 50, typeof(Channel2ActualFlow) },
            { 51, typeof(Channel3ActualFlow) },
            { 52, typeof(Channel4ActualFlow) },
            { 53, typeof(Channel0Frequency) },
            { 54, typeof(Channel1Frequency) },
            { 55, typeof(Channel2Frequency) },
            { 56, typeof(Channel3Frequency) },
            { 57, typeof(Channel4Frequency) },
            { 58, typeof(Channel0DutyCycle) },
            { 59, typeof(Channel1DutyCycle) },
            { 60, typeof(Channel2DutyCycle) },
            { 61, typeof(Channel3DutyCycle) },
            { 62, typeof(Channel4DutyCycle) },
            { 63, typeof(DigitalOutputSet) },
            { 64, typeof(DigitalOutputClear) },
            { 65, typeof(DigitalOutputToggle) },
            { 66, typeof(DigitalOutputState) },
            { 67, typeof(EnableValvePulse) },
            { 68, typeof(ValveSet) },
            { 69, typeof(ValveClear) },
            { 70, typeof(ValveToggle) },
            { 71, typeof(ValveState) },
            { 72, typeof(OdorValveState) },
            { 73, typeof(EndValveState) },
            { 74, typeof(CheckValveState) },
            { 75, typeof(Valve0PulseDuration) },
            { 76, typeof(Valve1PulseDuration) },
            { 77, typeof(Valve2PulseDuration) },
            { 78, typeof(Valve3PulseDuration) },
            { 79, typeof(CheckValve0DelayPulseDuration) },
            { 80, typeof(CheckValve1DelayPulseDuration) },
            { 81, typeof(CheckValve2DelayPulseDuration) },
            { 82, typeof(CheckValve3DelayPulseDuration) },
            { 83, typeof(EndValve0PulseDuration) },
            { 84, typeof(EndValve1PulseDuration) },
            { 85, typeof(PulseDummyValve) },
            { 86, typeof(DO0Sync) },
            { 87, typeof(DO1Sync) },
            { 88, typeof(DI0Trigger) },
            { 89, typeof(MimicValve0) },
            { 90, typeof(MimicValve1) },
            { 91, typeof(MimicValve2) },
            { 92, typeof(MimicValve3) },
            { 93, typeof(MimicCheckValve0) },
            { 94, typeof(MimicCheckValve1) },
            { 95, typeof(MimicCheckValve2) },
            { 96, typeof(MimicCheckValve3) },
            { 97, typeof(MimicEndValve0) },
            { 98, typeof(MimicEndValve1) },
            { 99, typeof(MimicDummyValve) },
            { 100, typeof(EnableValveExternalControl) },
            { 101, typeof(Channel3Range) },
            { 102, typeof(EnableCheckValveSync) },
            { 103, typeof(TemperatureValue) },
            { 104, typeof(EnableTemperatureCalibration) },
            { 105, typeof(TemperatureCalibrationValue) },
            { 106, typeof(EnableEvents) }
        };

        /// <summary>
        /// Gets the contents of the metadata file describing the <see cref="Olfactometer"/>
        /// device registers.
        /// </summary>
        public static readonly string Metadata = GetDeviceMetadata();

        static string GetDeviceMetadata()
        {
            var deviceType = typeof(Device);
            using var metadataStream = deviceType.Assembly.GetManifestResourceStream($"{deviceType.Namespace}.device.yml");
            using var streamReader = new System.IO.StreamReader(metadataStream);
            return streamReader.ReadToEnd();
        }
    }

    /// <summary>
    /// Represents an operator that returns the contents of the metadata file
    /// describing the <see cref="Olfactometer"/> device registers.
    /// </summary>
    [Description("Returns the contents of the metadata file describing the Olfactometer device registers.")]
    public partial class GetMetadata : Source<string>
    {
        /// <summary>
        /// Returns an observable sequence with the contents of the metadata file
        /// describing the <see cref="Olfactometer"/> device registers.
        /// </summary>
        /// <returns>
        /// A sequence with a single <see cref="string"/> object representing the
        /// contents of the metadata file.
        /// </returns>
        public override IObservable<string> Generate()
        {
            return Observable.Return(Device.Metadata);
        }
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
    /// <seealso cref="Flowmeter"/>
    /// <seealso cref="DI0State"/>
    /// <seealso cref="Channel0UserCalibration"/>
    /// <seealso cref="Channel1UserCalibration"/>
    /// <seealso cref="Channel2UserCalibration"/>
    /// <seealso cref="Channel3UserCalibration"/>
    /// <seealso cref="Channel4UserCalibration"/>
    /// <seealso cref="Channel3UserCalibrationAux"/>
    /// <seealso cref="UserCalibrationEnable"/>
    /// <seealso cref="Channel0TargetFlow"/>
    /// <seealso cref="Channel1TargetFlow"/>
    /// <seealso cref="Channel2TargetFlow"/>
    /// <seealso cref="Channel3TargetFlow"/>
    /// <seealso cref="Channel4TargetFlow"/>
    /// <seealso cref="ChannelsTargetFlow"/>
    /// <seealso cref="Channel0ActualFlow"/>
    /// <seealso cref="Channel1ActualFlow"/>
    /// <seealso cref="Channel2ActualFlow"/>
    /// <seealso cref="Channel3ActualFlow"/>
    /// <seealso cref="Channel4ActualFlow"/>
    /// <seealso cref="Channel0DutyCycle"/>
    /// <seealso cref="Channel1DutyCycle"/>
    /// <seealso cref="Channel2DutyCycle"/>
    /// <seealso cref="Channel3DutyCycle"/>
    /// <seealso cref="Channel4DutyCycle"/>
    /// <seealso cref="DigitalOutputSet"/>
    /// <seealso cref="DigitalOutputClear"/>
    /// <seealso cref="DigitalOutputToggle"/>
    /// <seealso cref="DigitalOutputState"/>
    /// <seealso cref="EnableValvePulse"/>
    /// <seealso cref="ValveSet"/>
    /// <seealso cref="ValveClear"/>
    /// <seealso cref="ValveToggle"/>
    /// <seealso cref="ValveState"/>
    /// <seealso cref="OdorValveState"/>
    /// <seealso cref="EndValveState"/>
    /// <seealso cref="CheckValveState"/>
    /// <seealso cref="Valve0PulseDuration"/>
    /// <seealso cref="Valve1PulseDuration"/>
    /// <seealso cref="Valve2PulseDuration"/>
    /// <seealso cref="Valve3PulseDuration"/>
    /// <seealso cref="CheckValve0DelayPulseDuration"/>
    /// <seealso cref="CheckValve1DelayPulseDuration"/>
    /// <seealso cref="CheckValve2DelayPulseDuration"/>
    /// <seealso cref="CheckValve3DelayPulseDuration"/>
    /// <seealso cref="EndValve0PulseDuration"/>
    /// <seealso cref="EndValve1PulseDuration"/>
    /// <seealso cref="DO0Sync"/>
    /// <seealso cref="DO1Sync"/>
    /// <seealso cref="DI0Trigger"/>
    /// <seealso cref="MimicValve0"/>
    /// <seealso cref="MimicValve1"/>
    /// <seealso cref="MimicValve2"/>
    /// <seealso cref="MimicValve3"/>
    /// <seealso cref="MimicCheckValve0"/>
    /// <seealso cref="MimicCheckValve1"/>
    /// <seealso cref="MimicCheckValve2"/>
    /// <seealso cref="MimicCheckValve3"/>
    /// <seealso cref="MimicEndValve0"/>
    /// <seealso cref="MimicEndValve1"/>
    /// <seealso cref="EnableValveExternalControl"/>
    /// <seealso cref="Channel3Range"/>
    /// <seealso cref="EnableCheckValveSync"/>
    /// <seealso cref="TemperatureValue"/>
    /// <seealso cref="EnableTemperatureCalibration"/>
    /// <seealso cref="TemperatureCalibrationValue"/>
    /// <seealso cref="EnableEvents"/>
    [XmlInclude(typeof(EnableFlow))]
    [XmlInclude(typeof(Flowmeter))]
    [XmlInclude(typeof(DI0State))]
    [XmlInclude(typeof(Channel0UserCalibration))]
    [XmlInclude(typeof(Channel1UserCalibration))]
    [XmlInclude(typeof(Channel2UserCalibration))]
    [XmlInclude(typeof(Channel3UserCalibration))]
    [XmlInclude(typeof(Channel4UserCalibration))]
    [XmlInclude(typeof(Channel3UserCalibrationAux))]
    [XmlInclude(typeof(UserCalibrationEnable))]
    [XmlInclude(typeof(Channel0TargetFlow))]
    [XmlInclude(typeof(Channel1TargetFlow))]
    [XmlInclude(typeof(Channel2TargetFlow))]
    [XmlInclude(typeof(Channel3TargetFlow))]
    [XmlInclude(typeof(Channel4TargetFlow))]
    [XmlInclude(typeof(ChannelsTargetFlow))]
    [XmlInclude(typeof(Channel0ActualFlow))]
    [XmlInclude(typeof(Channel1ActualFlow))]
    [XmlInclude(typeof(Channel2ActualFlow))]
    [XmlInclude(typeof(Channel3ActualFlow))]
    [XmlInclude(typeof(Channel4ActualFlow))]
    [XmlInclude(typeof(Channel0DutyCycle))]
    [XmlInclude(typeof(Channel1DutyCycle))]
    [XmlInclude(typeof(Channel2DutyCycle))]
    [XmlInclude(typeof(Channel3DutyCycle))]
    [XmlInclude(typeof(Channel4DutyCycle))]
    [XmlInclude(typeof(DigitalOutputSet))]
    [XmlInclude(typeof(DigitalOutputClear))]
    [XmlInclude(typeof(DigitalOutputToggle))]
    [XmlInclude(typeof(DigitalOutputState))]
    [XmlInclude(typeof(EnableValvePulse))]
    [XmlInclude(typeof(ValveSet))]
    [XmlInclude(typeof(ValveClear))]
    [XmlInclude(typeof(ValveToggle))]
    [XmlInclude(typeof(ValveState))]
    [XmlInclude(typeof(OdorValveState))]
    [XmlInclude(typeof(EndValveState))]
    [XmlInclude(typeof(CheckValveState))]
    [XmlInclude(typeof(Valve0PulseDuration))]
    [XmlInclude(typeof(Valve1PulseDuration))]
    [XmlInclude(typeof(Valve2PulseDuration))]
    [XmlInclude(typeof(Valve3PulseDuration))]
    [XmlInclude(typeof(CheckValve0DelayPulseDuration))]
    [XmlInclude(typeof(CheckValve1DelayPulseDuration))]
    [XmlInclude(typeof(CheckValve2DelayPulseDuration))]
    [XmlInclude(typeof(CheckValve3DelayPulseDuration))]
    [XmlInclude(typeof(EndValve0PulseDuration))]
    [XmlInclude(typeof(EndValve1PulseDuration))]
    [XmlInclude(typeof(DO0Sync))]
    [XmlInclude(typeof(DO1Sync))]
    [XmlInclude(typeof(DI0Trigger))]
    [XmlInclude(typeof(MimicValve0))]
    [XmlInclude(typeof(MimicValve1))]
    [XmlInclude(typeof(MimicValve2))]
    [XmlInclude(typeof(MimicValve3))]
    [XmlInclude(typeof(MimicCheckValve0))]
    [XmlInclude(typeof(MimicCheckValve1))]
    [XmlInclude(typeof(MimicCheckValve2))]
    [XmlInclude(typeof(MimicCheckValve3))]
    [XmlInclude(typeof(MimicEndValve0))]
    [XmlInclude(typeof(MimicEndValve1))]
    [XmlInclude(typeof(EnableValveExternalControl))]
    [XmlInclude(typeof(Channel3Range))]
    [XmlInclude(typeof(EnableCheckValveSync))]
    [XmlInclude(typeof(TemperatureValue))]
    [XmlInclude(typeof(EnableTemperatureCalibration))]
    [XmlInclude(typeof(TemperatureCalibrationValue))]
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
    /// <seealso cref="Flowmeter"/>
    /// <seealso cref="DI0State"/>
    /// <seealso cref="Channel0UserCalibration"/>
    /// <seealso cref="Channel1UserCalibration"/>
    /// <seealso cref="Channel2UserCalibration"/>
    /// <seealso cref="Channel3UserCalibration"/>
    /// <seealso cref="Channel4UserCalibration"/>
    /// <seealso cref="Channel3UserCalibrationAux"/>
    /// <seealso cref="UserCalibrationEnable"/>
    /// <seealso cref="Channel0TargetFlow"/>
    /// <seealso cref="Channel1TargetFlow"/>
    /// <seealso cref="Channel2TargetFlow"/>
    /// <seealso cref="Channel3TargetFlow"/>
    /// <seealso cref="Channel4TargetFlow"/>
    /// <seealso cref="ChannelsTargetFlow"/>
    /// <seealso cref="Channel0ActualFlow"/>
    /// <seealso cref="Channel1ActualFlow"/>
    /// <seealso cref="Channel2ActualFlow"/>
    /// <seealso cref="Channel3ActualFlow"/>
    /// <seealso cref="Channel4ActualFlow"/>
    /// <seealso cref="Channel0DutyCycle"/>
    /// <seealso cref="Channel1DutyCycle"/>
    /// <seealso cref="Channel2DutyCycle"/>
    /// <seealso cref="Channel3DutyCycle"/>
    /// <seealso cref="Channel4DutyCycle"/>
    /// <seealso cref="DigitalOutputSet"/>
    /// <seealso cref="DigitalOutputClear"/>
    /// <seealso cref="DigitalOutputToggle"/>
    /// <seealso cref="DigitalOutputState"/>
    /// <seealso cref="EnableValvePulse"/>
    /// <seealso cref="ValveSet"/>
    /// <seealso cref="ValveClear"/>
    /// <seealso cref="ValveToggle"/>
    /// <seealso cref="ValveState"/>
    /// <seealso cref="OdorValveState"/>
    /// <seealso cref="EndValveState"/>
    /// <seealso cref="CheckValveState"/>
    /// <seealso cref="Valve0PulseDuration"/>
    /// <seealso cref="Valve1PulseDuration"/>
    /// <seealso cref="Valve2PulseDuration"/>
    /// <seealso cref="Valve3PulseDuration"/>
    /// <seealso cref="CheckValve0DelayPulseDuration"/>
    /// <seealso cref="CheckValve1DelayPulseDuration"/>
    /// <seealso cref="CheckValve2DelayPulseDuration"/>
    /// <seealso cref="CheckValve3DelayPulseDuration"/>
    /// <seealso cref="EndValve0PulseDuration"/>
    /// <seealso cref="EndValve1PulseDuration"/>
    /// <seealso cref="DO0Sync"/>
    /// <seealso cref="DO1Sync"/>
    /// <seealso cref="DI0Trigger"/>
    /// <seealso cref="MimicValve0"/>
    /// <seealso cref="MimicValve1"/>
    /// <seealso cref="MimicValve2"/>
    /// <seealso cref="MimicValve3"/>
    /// <seealso cref="MimicCheckValve0"/>
    /// <seealso cref="MimicCheckValve1"/>
    /// <seealso cref="MimicCheckValve2"/>
    /// <seealso cref="MimicCheckValve3"/>
    /// <seealso cref="MimicEndValve0"/>
    /// <seealso cref="MimicEndValve1"/>
    /// <seealso cref="EnableValveExternalControl"/>
    /// <seealso cref="Channel3Range"/>
    /// <seealso cref="EnableCheckValveSync"/>
    /// <seealso cref="TemperatureValue"/>
    /// <seealso cref="EnableTemperatureCalibration"/>
    /// <seealso cref="TemperatureCalibrationValue"/>
    /// <seealso cref="EnableEvents"/>
    [XmlInclude(typeof(EnableFlow))]
    [XmlInclude(typeof(Flowmeter))]
    [XmlInclude(typeof(DI0State))]
    [XmlInclude(typeof(Channel0UserCalibration))]
    [XmlInclude(typeof(Channel1UserCalibration))]
    [XmlInclude(typeof(Channel2UserCalibration))]
    [XmlInclude(typeof(Channel3UserCalibration))]
    [XmlInclude(typeof(Channel4UserCalibration))]
    [XmlInclude(typeof(Channel3UserCalibrationAux))]
    [XmlInclude(typeof(UserCalibrationEnable))]
    [XmlInclude(typeof(Channel0TargetFlow))]
    [XmlInclude(typeof(Channel1TargetFlow))]
    [XmlInclude(typeof(Channel2TargetFlow))]
    [XmlInclude(typeof(Channel3TargetFlow))]
    [XmlInclude(typeof(Channel4TargetFlow))]
    [XmlInclude(typeof(ChannelsTargetFlow))]
    [XmlInclude(typeof(Channel0ActualFlow))]
    [XmlInclude(typeof(Channel1ActualFlow))]
    [XmlInclude(typeof(Channel2ActualFlow))]
    [XmlInclude(typeof(Channel3ActualFlow))]
    [XmlInclude(typeof(Channel4ActualFlow))]
    [XmlInclude(typeof(Channel0DutyCycle))]
    [XmlInclude(typeof(Channel1DutyCycle))]
    [XmlInclude(typeof(Channel2DutyCycle))]
    [XmlInclude(typeof(Channel3DutyCycle))]
    [XmlInclude(typeof(Channel4DutyCycle))]
    [XmlInclude(typeof(DigitalOutputSet))]
    [XmlInclude(typeof(DigitalOutputClear))]
    [XmlInclude(typeof(DigitalOutputToggle))]
    [XmlInclude(typeof(DigitalOutputState))]
    [XmlInclude(typeof(EnableValvePulse))]
    [XmlInclude(typeof(ValveSet))]
    [XmlInclude(typeof(ValveClear))]
    [XmlInclude(typeof(ValveToggle))]
    [XmlInclude(typeof(ValveState))]
    [XmlInclude(typeof(OdorValveState))]
    [XmlInclude(typeof(EndValveState))]
    [XmlInclude(typeof(CheckValveState))]
    [XmlInclude(typeof(Valve0PulseDuration))]
    [XmlInclude(typeof(Valve1PulseDuration))]
    [XmlInclude(typeof(Valve2PulseDuration))]
    [XmlInclude(typeof(Valve3PulseDuration))]
    [XmlInclude(typeof(CheckValve0DelayPulseDuration))]
    [XmlInclude(typeof(CheckValve1DelayPulseDuration))]
    [XmlInclude(typeof(CheckValve2DelayPulseDuration))]
    [XmlInclude(typeof(CheckValve3DelayPulseDuration))]
    [XmlInclude(typeof(EndValve0PulseDuration))]
    [XmlInclude(typeof(EndValve1PulseDuration))]
    [XmlInclude(typeof(DO0Sync))]
    [XmlInclude(typeof(DO1Sync))]
    [XmlInclude(typeof(DI0Trigger))]
    [XmlInclude(typeof(MimicValve0))]
    [XmlInclude(typeof(MimicValve1))]
    [XmlInclude(typeof(MimicValve2))]
    [XmlInclude(typeof(MimicValve3))]
    [XmlInclude(typeof(MimicCheckValve0))]
    [XmlInclude(typeof(MimicCheckValve1))]
    [XmlInclude(typeof(MimicCheckValve2))]
    [XmlInclude(typeof(MimicCheckValve3))]
    [XmlInclude(typeof(MimicEndValve0))]
    [XmlInclude(typeof(MimicEndValve1))]
    [XmlInclude(typeof(EnableValveExternalControl))]
    [XmlInclude(typeof(Channel3Range))]
    [XmlInclude(typeof(EnableCheckValveSync))]
    [XmlInclude(typeof(TemperatureValue))]
    [XmlInclude(typeof(EnableTemperatureCalibration))]
    [XmlInclude(typeof(TemperatureCalibrationValue))]
    [XmlInclude(typeof(EnableEvents))]
    [XmlInclude(typeof(TimestampedEnableFlow))]
    [XmlInclude(typeof(TimestampedFlowmeter))]
    [XmlInclude(typeof(TimestampedDI0State))]
    [XmlInclude(typeof(TimestampedChannel0UserCalibration))]
    [XmlInclude(typeof(TimestampedChannel1UserCalibration))]
    [XmlInclude(typeof(TimestampedChannel2UserCalibration))]
    [XmlInclude(typeof(TimestampedChannel3UserCalibration))]
    [XmlInclude(typeof(TimestampedChannel4UserCalibration))]
    [XmlInclude(typeof(TimestampedChannel3UserCalibrationAux))]
    [XmlInclude(typeof(TimestampedUserCalibrationEnable))]
    [XmlInclude(typeof(TimestampedChannel0TargetFlow))]
    [XmlInclude(typeof(TimestampedChannel1TargetFlow))]
    [XmlInclude(typeof(TimestampedChannel2TargetFlow))]
    [XmlInclude(typeof(TimestampedChannel3TargetFlow))]
    [XmlInclude(typeof(TimestampedChannel4TargetFlow))]
    [XmlInclude(typeof(TimestampedChannelsTargetFlow))]
    [XmlInclude(typeof(TimestampedChannel0ActualFlow))]
    [XmlInclude(typeof(TimestampedChannel1ActualFlow))]
    [XmlInclude(typeof(TimestampedChannel2ActualFlow))]
    [XmlInclude(typeof(TimestampedChannel3ActualFlow))]
    [XmlInclude(typeof(TimestampedChannel4ActualFlow))]
    [XmlInclude(typeof(TimestampedChannel0DutyCycle))]
    [XmlInclude(typeof(TimestampedChannel1DutyCycle))]
    [XmlInclude(typeof(TimestampedChannel2DutyCycle))]
    [XmlInclude(typeof(TimestampedChannel3DutyCycle))]
    [XmlInclude(typeof(TimestampedChannel4DutyCycle))]
    [XmlInclude(typeof(TimestampedDigitalOutputSet))]
    [XmlInclude(typeof(TimestampedDigitalOutputClear))]
    [XmlInclude(typeof(TimestampedDigitalOutputToggle))]
    [XmlInclude(typeof(TimestampedDigitalOutputState))]
    [XmlInclude(typeof(TimestampedEnableValvePulse))]
    [XmlInclude(typeof(TimestampedValveSet))]
    [XmlInclude(typeof(TimestampedValveClear))]
    [XmlInclude(typeof(TimestampedValveToggle))]
    [XmlInclude(typeof(TimestampedValveState))]
    [XmlInclude(typeof(TimestampedOdorValveState))]
    [XmlInclude(typeof(TimestampedEndValveState))]
    [XmlInclude(typeof(TimestampedCheckValveState))]
    [XmlInclude(typeof(TimestampedValve0PulseDuration))]
    [XmlInclude(typeof(TimestampedValve1PulseDuration))]
    [XmlInclude(typeof(TimestampedValve2PulseDuration))]
    [XmlInclude(typeof(TimestampedValve3PulseDuration))]
    [XmlInclude(typeof(TimestampedCheckValve0DelayPulseDuration))]
    [XmlInclude(typeof(TimestampedCheckValve1DelayPulseDuration))]
    [XmlInclude(typeof(TimestampedCheckValve2DelayPulseDuration))]
    [XmlInclude(typeof(TimestampedCheckValve3DelayPulseDuration))]
    [XmlInclude(typeof(TimestampedEndValve0PulseDuration))]
    [XmlInclude(typeof(TimestampedEndValve1PulseDuration))]
    [XmlInclude(typeof(TimestampedDO0Sync))]
    [XmlInclude(typeof(TimestampedDO1Sync))]
    [XmlInclude(typeof(TimestampedDI0Trigger))]
    [XmlInclude(typeof(TimestampedMimicValve0))]
    [XmlInclude(typeof(TimestampedMimicValve1))]
    [XmlInclude(typeof(TimestampedMimicValve2))]
    [XmlInclude(typeof(TimestampedMimicValve3))]
    [XmlInclude(typeof(TimestampedMimicCheckValve0))]
    [XmlInclude(typeof(TimestampedMimicCheckValve1))]
    [XmlInclude(typeof(TimestampedMimicCheckValve2))]
    [XmlInclude(typeof(TimestampedMimicCheckValve3))]
    [XmlInclude(typeof(TimestampedMimicEndValve0))]
    [XmlInclude(typeof(TimestampedMimicEndValve1))]
    [XmlInclude(typeof(TimestampedEnableValveExternalControl))]
    [XmlInclude(typeof(TimestampedChannel3Range))]
    [XmlInclude(typeof(TimestampedEnableCheckValveSync))]
    [XmlInclude(typeof(TimestampedTemperatureValue))]
    [XmlInclude(typeof(TimestampedEnableTemperatureCalibration))]
    [XmlInclude(typeof(TimestampedTemperatureCalibrationValue))]
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
    /// <seealso cref="Flowmeter"/>
    /// <seealso cref="DI0State"/>
    /// <seealso cref="Channel0UserCalibration"/>
    /// <seealso cref="Channel1UserCalibration"/>
    /// <seealso cref="Channel2UserCalibration"/>
    /// <seealso cref="Channel3UserCalibration"/>
    /// <seealso cref="Channel4UserCalibration"/>
    /// <seealso cref="Channel3UserCalibrationAux"/>
    /// <seealso cref="UserCalibrationEnable"/>
    /// <seealso cref="Channel0TargetFlow"/>
    /// <seealso cref="Channel1TargetFlow"/>
    /// <seealso cref="Channel2TargetFlow"/>
    /// <seealso cref="Channel3TargetFlow"/>
    /// <seealso cref="Channel4TargetFlow"/>
    /// <seealso cref="ChannelsTargetFlow"/>
    /// <seealso cref="Channel0ActualFlow"/>
    /// <seealso cref="Channel1ActualFlow"/>
    /// <seealso cref="Channel2ActualFlow"/>
    /// <seealso cref="Channel3ActualFlow"/>
    /// <seealso cref="Channel4ActualFlow"/>
    /// <seealso cref="Channel0DutyCycle"/>
    /// <seealso cref="Channel1DutyCycle"/>
    /// <seealso cref="Channel2DutyCycle"/>
    /// <seealso cref="Channel3DutyCycle"/>
    /// <seealso cref="Channel4DutyCycle"/>
    /// <seealso cref="DigitalOutputSet"/>
    /// <seealso cref="DigitalOutputClear"/>
    /// <seealso cref="DigitalOutputToggle"/>
    /// <seealso cref="DigitalOutputState"/>
    /// <seealso cref="EnableValvePulse"/>
    /// <seealso cref="ValveSet"/>
    /// <seealso cref="ValveClear"/>
    /// <seealso cref="ValveToggle"/>
    /// <seealso cref="ValveState"/>
    /// <seealso cref="OdorValveState"/>
    /// <seealso cref="EndValveState"/>
    /// <seealso cref="CheckValveState"/>
    /// <seealso cref="Valve0PulseDuration"/>
    /// <seealso cref="Valve1PulseDuration"/>
    /// <seealso cref="Valve2PulseDuration"/>
    /// <seealso cref="Valve3PulseDuration"/>
    /// <seealso cref="CheckValve0DelayPulseDuration"/>
    /// <seealso cref="CheckValve1DelayPulseDuration"/>
    /// <seealso cref="CheckValve2DelayPulseDuration"/>
    /// <seealso cref="CheckValve3DelayPulseDuration"/>
    /// <seealso cref="EndValve0PulseDuration"/>
    /// <seealso cref="EndValve1PulseDuration"/>
    /// <seealso cref="DO0Sync"/>
    /// <seealso cref="DO1Sync"/>
    /// <seealso cref="DI0Trigger"/>
    /// <seealso cref="MimicValve0"/>
    /// <seealso cref="MimicValve1"/>
    /// <seealso cref="MimicValve2"/>
    /// <seealso cref="MimicValve3"/>
    /// <seealso cref="MimicCheckValve0"/>
    /// <seealso cref="MimicCheckValve1"/>
    /// <seealso cref="MimicCheckValve2"/>
    /// <seealso cref="MimicCheckValve3"/>
    /// <seealso cref="MimicEndValve0"/>
    /// <seealso cref="MimicEndValve1"/>
    /// <seealso cref="EnableValveExternalControl"/>
    /// <seealso cref="Channel3Range"/>
    /// <seealso cref="EnableCheckValveSync"/>
    /// <seealso cref="TemperatureValue"/>
    /// <seealso cref="EnableTemperatureCalibration"/>
    /// <seealso cref="TemperatureCalibrationValue"/>
    /// <seealso cref="EnableEvents"/>
    [XmlInclude(typeof(EnableFlow))]
    [XmlInclude(typeof(Flowmeter))]
    [XmlInclude(typeof(DI0State))]
    [XmlInclude(typeof(Channel0UserCalibration))]
    [XmlInclude(typeof(Channel1UserCalibration))]
    [XmlInclude(typeof(Channel2UserCalibration))]
    [XmlInclude(typeof(Channel3UserCalibration))]
    [XmlInclude(typeof(Channel4UserCalibration))]
    [XmlInclude(typeof(Channel3UserCalibrationAux))]
    [XmlInclude(typeof(UserCalibrationEnable))]
    [XmlInclude(typeof(Channel0TargetFlow))]
    [XmlInclude(typeof(Channel1TargetFlow))]
    [XmlInclude(typeof(Channel2TargetFlow))]
    [XmlInclude(typeof(Channel3TargetFlow))]
    [XmlInclude(typeof(Channel4TargetFlow))]
    [XmlInclude(typeof(ChannelsTargetFlow))]
    [XmlInclude(typeof(Channel0ActualFlow))]
    [XmlInclude(typeof(Channel1ActualFlow))]
    [XmlInclude(typeof(Channel2ActualFlow))]
    [XmlInclude(typeof(Channel3ActualFlow))]
    [XmlInclude(typeof(Channel4ActualFlow))]
    [XmlInclude(typeof(Channel0DutyCycle))]
    [XmlInclude(typeof(Channel1DutyCycle))]
    [XmlInclude(typeof(Channel2DutyCycle))]
    [XmlInclude(typeof(Channel3DutyCycle))]
    [XmlInclude(typeof(Channel4DutyCycle))]
    [XmlInclude(typeof(DigitalOutputSet))]
    [XmlInclude(typeof(DigitalOutputClear))]
    [XmlInclude(typeof(DigitalOutputToggle))]
    [XmlInclude(typeof(DigitalOutputState))]
    [XmlInclude(typeof(EnableValvePulse))]
    [XmlInclude(typeof(ValveSet))]
    [XmlInclude(typeof(ValveClear))]
    [XmlInclude(typeof(ValveToggle))]
    [XmlInclude(typeof(ValveState))]
    [XmlInclude(typeof(OdorValveState))]
    [XmlInclude(typeof(EndValveState))]
    [XmlInclude(typeof(CheckValveState))]
    [XmlInclude(typeof(Valve0PulseDuration))]
    [XmlInclude(typeof(Valve1PulseDuration))]
    [XmlInclude(typeof(Valve2PulseDuration))]
    [XmlInclude(typeof(Valve3PulseDuration))]
    [XmlInclude(typeof(CheckValve0DelayPulseDuration))]
    [XmlInclude(typeof(CheckValve1DelayPulseDuration))]
    [XmlInclude(typeof(CheckValve2DelayPulseDuration))]
    [XmlInclude(typeof(CheckValve3DelayPulseDuration))]
    [XmlInclude(typeof(EndValve0PulseDuration))]
    [XmlInclude(typeof(EndValve1PulseDuration))]
    [XmlInclude(typeof(DO0Sync))]
    [XmlInclude(typeof(DO1Sync))]
    [XmlInclude(typeof(DI0Trigger))]
    [XmlInclude(typeof(MimicValve0))]
    [XmlInclude(typeof(MimicValve1))]
    [XmlInclude(typeof(MimicValve2))]
    [XmlInclude(typeof(MimicValve3))]
    [XmlInclude(typeof(MimicCheckValve0))]
    [XmlInclude(typeof(MimicCheckValve1))]
    [XmlInclude(typeof(MimicCheckValve2))]
    [XmlInclude(typeof(MimicCheckValve3))]
    [XmlInclude(typeof(MimicEndValve0))]
    [XmlInclude(typeof(MimicEndValve1))]
    [XmlInclude(typeof(EnableValveExternalControl))]
    [XmlInclude(typeof(Channel3Range))]
    [XmlInclude(typeof(EnableCheckValveSync))]
    [XmlInclude(typeof(TemperatureValue))]
    [XmlInclude(typeof(EnableTemperatureCalibration))]
    [XmlInclude(typeof(TemperatureCalibrationValue))]
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
    /// Represents a register that starts or stops the flow in all channels.
    /// </summary>
    [Description("Starts or stops the flow in all channels.")]
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
    /// Represents a register that value of single ADC read from all flowmeter channels.
    /// </summary>
    [Description("Value of single ADC read from all flowmeter channels.")]
    public partial class Flowmeter
    {
        /// <summary>
        /// Represents the address of the <see cref="Flowmeter"/> register. This field is constant.
        /// </summary>
        public const int Address = 33;

        /// <summary>
        /// Represents the payload type of the <see cref="Flowmeter"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.S16;

        /// <summary>
        /// Represents the length of the <see cref="Flowmeter"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 5;

        static FlowmeterPayload ParsePayload(short[] payload)
        {
            FlowmeterPayload result;
            result.Channel0 = payload[0];
            result.Channel1 = payload[1];
            result.Channel2 = payload[2];
            result.Channel3 = payload[3];
            result.Channel4 = payload[4];
            return result;
        }

        static short[] FormatPayload(FlowmeterPayload value)
        {
            short[] result;
            result = new short[5];
            result[0] = value.Channel0;
            result[1] = value.Channel1;
            result[2] = value.Channel2;
            result[3] = value.Channel3;
            result[4] = value.Channel4;
            return result;
        }

        /// <summary>
        /// Returns the payload data for <see cref="Flowmeter"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static FlowmeterPayload GetPayload(HarpMessage message)
        {
            return ParsePayload(message.GetPayloadArray<short>());
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="Flowmeter"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<FlowmeterPayload> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadArray<short>();
            return Timestamped.Create(ParsePayload(payload.Value), payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="Flowmeter"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Flowmeter"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, FlowmeterPayload value)
        {
            return HarpMessage.FromInt16(Address, messageType, FormatPayload(value));
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="Flowmeter"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Flowmeter"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, FlowmeterPayload value)
        {
            return HarpMessage.FromInt16(Address, timestamp, messageType, FormatPayload(value));
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// Flowmeter register.
    /// </summary>
    /// <seealso cref="Flowmeter"/>
    [Description("Filters and selects timestamped messages from the Flowmeter register.")]
    public partial class TimestampedFlowmeter
    {
        /// <summary>
        /// Represents the address of the <see cref="Flowmeter"/> register. This field is constant.
        /// </summary>
        public const int Address = Flowmeter.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="Flowmeter"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<FlowmeterPayload> GetPayload(HarpMessage message)
        {
            return Flowmeter.GetTimestampedPayload(message);
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
    /// Represents a register that calibration values for a single channel [x0,...xn], where x= ADC raw value for 0:10:100 ml/min.
    /// </summary>
    [Description("Calibration values for a single channel [x0,...xn], where x= ADC raw value for 0:10:100 ml/min.")]
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
    /// Represents a register that calibration values for a single channel [x0,...xn], where x= ADC raw value for 0:10:100 ml/min.
    /// </summary>
    [Description("Calibration values for a single channel [x0,...xn], where x= ADC raw value for 0:10:100 ml/min.")]
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
    /// Represents a register that calibration values for a single channel [x0,...xn], where x= ADC raw value for 0:10:100 ml/min.
    /// </summary>
    [Description("Calibration values for a single channel [x0,...xn], where x= ADC raw value for 0:10:100 ml/min.")]
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
    /// Represents a register that calibration values for a single channel [x0,...xn], where x= ADC raw value for 0:10:100 ml/min.
    /// </summary>
    [Description("Calibration values for a single channel [x0,...xn], where x= ADC raw value for 0:10:100 ml/min.")]
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
    /// Represents a register that calibration values specific for channel 4 [x0,...xn], where x= ADC raw value for 0:100:1000 ml/min.
    /// </summary>
    [Description("Calibration values specific for channel 4 [x0,...xn], where x= ADC raw value for 0:100:1000 ml/min.")]
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
    /// Represents a register that calibration values specific for channel 3 if Channel3RangeConfig = FlowRate1000. [x0,...xn], where x= ADC raw value for 0:100:1000 ml/min.
    /// </summary>
    [Description("Calibration values specific for channel 3 if Channel3RangeConfig = FlowRate1000. [x0,...xn], where x= ADC raw value for 0:100:1000 ml/min.")]
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
        public static EnableFlag GetPayload(HarpMessage message)
        {
            return (EnableFlag)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="UserCalibrationEnable"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<EnableFlag> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((EnableFlag)payload.Value, payload.Seconds);
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
        public static HarpMessage FromPayload(MessageType messageType, EnableFlag value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
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
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, EnableFlag value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
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
        public static Timestamped<EnableFlag> GetPayload(HarpMessage message)
        {
            return UserCalibrationEnable.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that sets the flow-rate rate for channel 0 [ml/min].
    /// </summary>
    [Description("Sets the flow-rate rate for channel 0 [ml/min].")]
    public partial class Channel0TargetFlow
    {
        /// <summary>
        /// Represents the address of the <see cref="Channel0TargetFlow"/> register. This field is constant.
        /// </summary>
        public const int Address = 42;

        /// <summary>
        /// Represents the payload type of the <see cref="Channel0TargetFlow"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.Float;

        /// <summary>
        /// Represents the length of the <see cref="Channel0TargetFlow"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="Channel0TargetFlow"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static float GetPayload(HarpMessage message)
        {
            return message.GetPayloadSingle();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="Channel0TargetFlow"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<float> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadSingle();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="Channel0TargetFlow"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Channel0TargetFlow"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, float value)
        {
            return HarpMessage.FromSingle(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="Channel0TargetFlow"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Channel0TargetFlow"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, float value)
        {
            return HarpMessage.FromSingle(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// Channel0TargetFlow register.
    /// </summary>
    /// <seealso cref="Channel0TargetFlow"/>
    [Description("Filters and selects timestamped messages from the Channel0TargetFlow register.")]
    public partial class TimestampedChannel0TargetFlow
    {
        /// <summary>
        /// Represents the address of the <see cref="Channel0TargetFlow"/> register. This field is constant.
        /// </summary>
        public const int Address = Channel0TargetFlow.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="Channel0TargetFlow"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<float> GetPayload(HarpMessage message)
        {
            return Channel0TargetFlow.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that sets the flow-rate rate for channel 1 [ml/min].
    /// </summary>
    [Description("Sets the flow-rate rate for channel 1 [ml/min].")]
    public partial class Channel1TargetFlow
    {
        /// <summary>
        /// Represents the address of the <see cref="Channel1TargetFlow"/> register. This field is constant.
        /// </summary>
        public const int Address = 43;

        /// <summary>
        /// Represents the payload type of the <see cref="Channel1TargetFlow"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.Float;

        /// <summary>
        /// Represents the length of the <see cref="Channel1TargetFlow"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="Channel1TargetFlow"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static float GetPayload(HarpMessage message)
        {
            return message.GetPayloadSingle();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="Channel1TargetFlow"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<float> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadSingle();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="Channel1TargetFlow"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Channel1TargetFlow"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, float value)
        {
            return HarpMessage.FromSingle(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="Channel1TargetFlow"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Channel1TargetFlow"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, float value)
        {
            return HarpMessage.FromSingle(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// Channel1TargetFlow register.
    /// </summary>
    /// <seealso cref="Channel1TargetFlow"/>
    [Description("Filters and selects timestamped messages from the Channel1TargetFlow register.")]
    public partial class TimestampedChannel1TargetFlow
    {
        /// <summary>
        /// Represents the address of the <see cref="Channel1TargetFlow"/> register. This field is constant.
        /// </summary>
        public const int Address = Channel1TargetFlow.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="Channel1TargetFlow"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<float> GetPayload(HarpMessage message)
        {
            return Channel1TargetFlow.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that sets the flow-rate rate for channel 2 [ml/min].
    /// </summary>
    [Description("Sets the flow-rate rate for channel 2 [ml/min].")]
    public partial class Channel2TargetFlow
    {
        /// <summary>
        /// Represents the address of the <see cref="Channel2TargetFlow"/> register. This field is constant.
        /// </summary>
        public const int Address = 44;

        /// <summary>
        /// Represents the payload type of the <see cref="Channel2TargetFlow"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.Float;

        /// <summary>
        /// Represents the length of the <see cref="Channel2TargetFlow"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="Channel2TargetFlow"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static float GetPayload(HarpMessage message)
        {
            return message.GetPayloadSingle();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="Channel2TargetFlow"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<float> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadSingle();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="Channel2TargetFlow"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Channel2TargetFlow"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, float value)
        {
            return HarpMessage.FromSingle(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="Channel2TargetFlow"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Channel2TargetFlow"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, float value)
        {
            return HarpMessage.FromSingle(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// Channel2TargetFlow register.
    /// </summary>
    /// <seealso cref="Channel2TargetFlow"/>
    [Description("Filters and selects timestamped messages from the Channel2TargetFlow register.")]
    public partial class TimestampedChannel2TargetFlow
    {
        /// <summary>
        /// Represents the address of the <see cref="Channel2TargetFlow"/> register. This field is constant.
        /// </summary>
        public const int Address = Channel2TargetFlow.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="Channel2TargetFlow"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<float> GetPayload(HarpMessage message)
        {
            return Channel2TargetFlow.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that sets the flow-rate rate for channel 3 [ml/min].
    /// </summary>
    [Description("Sets the flow-rate rate for channel 3 [ml/min].")]
    public partial class Channel3TargetFlow
    {
        /// <summary>
        /// Represents the address of the <see cref="Channel3TargetFlow"/> register. This field is constant.
        /// </summary>
        public const int Address = 45;

        /// <summary>
        /// Represents the payload type of the <see cref="Channel3TargetFlow"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.Float;

        /// <summary>
        /// Represents the length of the <see cref="Channel3TargetFlow"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="Channel3TargetFlow"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static float GetPayload(HarpMessage message)
        {
            return message.GetPayloadSingle();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="Channel3TargetFlow"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<float> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadSingle();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="Channel3TargetFlow"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Channel3TargetFlow"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, float value)
        {
            return HarpMessage.FromSingle(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="Channel3TargetFlow"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Channel3TargetFlow"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, float value)
        {
            return HarpMessage.FromSingle(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// Channel3TargetFlow register.
    /// </summary>
    /// <seealso cref="Channel3TargetFlow"/>
    [Description("Filters and selects timestamped messages from the Channel3TargetFlow register.")]
    public partial class TimestampedChannel3TargetFlow
    {
        /// <summary>
        /// Represents the address of the <see cref="Channel3TargetFlow"/> register. This field is constant.
        /// </summary>
        public const int Address = Channel3TargetFlow.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="Channel3TargetFlow"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<float> GetPayload(HarpMessage message)
        {
            return Channel3TargetFlow.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that sets the flow-rate rate for channel 4 [ml/min].
    /// </summary>
    [Description("Sets the flow-rate rate for channel 4 [ml/min].")]
    public partial class Channel4TargetFlow
    {
        /// <summary>
        /// Represents the address of the <see cref="Channel4TargetFlow"/> register. This field is constant.
        /// </summary>
        public const int Address = 46;

        /// <summary>
        /// Represents the payload type of the <see cref="Channel4TargetFlow"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.Float;

        /// <summary>
        /// Represents the length of the <see cref="Channel4TargetFlow"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="Channel4TargetFlow"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static float GetPayload(HarpMessage message)
        {
            return message.GetPayloadSingle();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="Channel4TargetFlow"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<float> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadSingle();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="Channel4TargetFlow"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Channel4TargetFlow"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, float value)
        {
            return HarpMessage.FromSingle(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="Channel4TargetFlow"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Channel4TargetFlow"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, float value)
        {
            return HarpMessage.FromSingle(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// Channel4TargetFlow register.
    /// </summary>
    /// <seealso cref="Channel4TargetFlow"/>
    [Description("Filters and selects timestamped messages from the Channel4TargetFlow register.")]
    public partial class TimestampedChannel4TargetFlow
    {
        /// <summary>
        /// Represents the address of the <see cref="Channel4TargetFlow"/> register. This field is constant.
        /// </summary>
        public const int Address = Channel4TargetFlow.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="Channel4TargetFlow"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<float> GetPayload(HarpMessage message)
        {
            return Channel4TargetFlow.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that sets the flow-rate rate for all channels [ml/min].
    /// </summary>
    [Description("Sets the flow-rate rate for all channels [ml/min].")]
    public partial class ChannelsTargetFlow
    {
        /// <summary>
        /// Represents the address of the <see cref="ChannelsTargetFlow"/> register. This field is constant.
        /// </summary>
        public const int Address = 47;

        /// <summary>
        /// Represents the payload type of the <see cref="ChannelsTargetFlow"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.Float;

        /// <summary>
        /// Represents the length of the <see cref="ChannelsTargetFlow"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 5;

        static ChannelsTargetFlowPayload ParsePayload(float[] payload)
        {
            ChannelsTargetFlowPayload result;
            result.Channel0 = payload[0];
            result.Channel1 = payload[1];
            result.Channel2 = payload[2];
            result.Channel3 = payload[3];
            result.Channel4 = payload[4];
            return result;
        }

        static float[] FormatPayload(ChannelsTargetFlowPayload value)
        {
            float[] result;
            result = new float[5];
            result[0] = value.Channel0;
            result[1] = value.Channel1;
            result[2] = value.Channel2;
            result[3] = value.Channel3;
            result[4] = value.Channel4;
            return result;
        }

        /// <summary>
        /// Returns the payload data for <see cref="ChannelsTargetFlow"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static ChannelsTargetFlowPayload GetPayload(HarpMessage message)
        {
            return ParsePayload(message.GetPayloadArray<float>());
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="ChannelsTargetFlow"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ChannelsTargetFlowPayload> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadArray<float>();
            return Timestamped.Create(ParsePayload(payload.Value), payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="ChannelsTargetFlow"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="ChannelsTargetFlow"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, ChannelsTargetFlowPayload value)
        {
            return HarpMessage.FromSingle(Address, messageType, FormatPayload(value));
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="ChannelsTargetFlow"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="ChannelsTargetFlow"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, ChannelsTargetFlowPayload value)
        {
            return HarpMessage.FromSingle(Address, timestamp, messageType, FormatPayload(value));
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// ChannelsTargetFlow register.
    /// </summary>
    /// <seealso cref="ChannelsTargetFlow"/>
    [Description("Filters and selects timestamped messages from the ChannelsTargetFlow register.")]
    public partial class TimestampedChannelsTargetFlow
    {
        /// <summary>
        /// Represents the address of the <see cref="ChannelsTargetFlow"/> register. This field is constant.
        /// </summary>
        public const int Address = ChannelsTargetFlow.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="ChannelsTargetFlow"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ChannelsTargetFlowPayload> GetPayload(HarpMessage message)
        {
            return ChannelsTargetFlow.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that actual flow-rate read for channel 0 - flowmeter 0 [ml/min].
    /// </summary>
    [Description("Actual flow-rate read for channel 0 - flowmeter 0 [ml/min].")]
    public partial class Channel0ActualFlow
    {
        /// <summary>
        /// Represents the address of the <see cref="Channel0ActualFlow"/> register. This field is constant.
        /// </summary>
        public const int Address = 48;

        /// <summary>
        /// Represents the payload type of the <see cref="Channel0ActualFlow"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.Float;

        /// <summary>
        /// Represents the length of the <see cref="Channel0ActualFlow"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="Channel0ActualFlow"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static float GetPayload(HarpMessage message)
        {
            return message.GetPayloadSingle();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="Channel0ActualFlow"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<float> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadSingle();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="Channel0ActualFlow"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Channel0ActualFlow"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, float value)
        {
            return HarpMessage.FromSingle(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="Channel0ActualFlow"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Channel0ActualFlow"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, float value)
        {
            return HarpMessage.FromSingle(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// Channel0ActualFlow register.
    /// </summary>
    /// <seealso cref="Channel0ActualFlow"/>
    [Description("Filters and selects timestamped messages from the Channel0ActualFlow register.")]
    public partial class TimestampedChannel0ActualFlow
    {
        /// <summary>
        /// Represents the address of the <see cref="Channel0ActualFlow"/> register. This field is constant.
        /// </summary>
        public const int Address = Channel0ActualFlow.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="Channel0ActualFlow"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<float> GetPayload(HarpMessage message)
        {
            return Channel0ActualFlow.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that actual flow-rate read for channel 1 - flowmeter 1 [ml/min].
    /// </summary>
    [Description("Actual flow-rate read for channel 1 - flowmeter 1 [ml/min].")]
    public partial class Channel1ActualFlow
    {
        /// <summary>
        /// Represents the address of the <see cref="Channel1ActualFlow"/> register. This field is constant.
        /// </summary>
        public const int Address = 49;

        /// <summary>
        /// Represents the payload type of the <see cref="Channel1ActualFlow"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.Float;

        /// <summary>
        /// Represents the length of the <see cref="Channel1ActualFlow"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="Channel1ActualFlow"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static float GetPayload(HarpMessage message)
        {
            return message.GetPayloadSingle();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="Channel1ActualFlow"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<float> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadSingle();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="Channel1ActualFlow"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Channel1ActualFlow"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, float value)
        {
            return HarpMessage.FromSingle(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="Channel1ActualFlow"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Channel1ActualFlow"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, float value)
        {
            return HarpMessage.FromSingle(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// Channel1ActualFlow register.
    /// </summary>
    /// <seealso cref="Channel1ActualFlow"/>
    [Description("Filters and selects timestamped messages from the Channel1ActualFlow register.")]
    public partial class TimestampedChannel1ActualFlow
    {
        /// <summary>
        /// Represents the address of the <see cref="Channel1ActualFlow"/> register. This field is constant.
        /// </summary>
        public const int Address = Channel1ActualFlow.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="Channel1ActualFlow"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<float> GetPayload(HarpMessage message)
        {
            return Channel1ActualFlow.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that actual flow-rate read for channel 2 - flowmeter 2 [ml/min].
    /// </summary>
    [Description("Actual flow-rate read for channel 2 - flowmeter 2 [ml/min].")]
    public partial class Channel2ActualFlow
    {
        /// <summary>
        /// Represents the address of the <see cref="Channel2ActualFlow"/> register. This field is constant.
        /// </summary>
        public const int Address = 50;

        /// <summary>
        /// Represents the payload type of the <see cref="Channel2ActualFlow"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.Float;

        /// <summary>
        /// Represents the length of the <see cref="Channel2ActualFlow"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="Channel2ActualFlow"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static float GetPayload(HarpMessage message)
        {
            return message.GetPayloadSingle();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="Channel2ActualFlow"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<float> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadSingle();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="Channel2ActualFlow"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Channel2ActualFlow"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, float value)
        {
            return HarpMessage.FromSingle(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="Channel2ActualFlow"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Channel2ActualFlow"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, float value)
        {
            return HarpMessage.FromSingle(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// Channel2ActualFlow register.
    /// </summary>
    /// <seealso cref="Channel2ActualFlow"/>
    [Description("Filters and selects timestamped messages from the Channel2ActualFlow register.")]
    public partial class TimestampedChannel2ActualFlow
    {
        /// <summary>
        /// Represents the address of the <see cref="Channel2ActualFlow"/> register. This field is constant.
        /// </summary>
        public const int Address = Channel2ActualFlow.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="Channel2ActualFlow"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<float> GetPayload(HarpMessage message)
        {
            return Channel2ActualFlow.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that actual flow-rate read for channel 3 - flowmeter 3 [ml/min].
    /// </summary>
    [Description("Actual flow-rate read for channel 3 - flowmeter 3 [ml/min].")]
    public partial class Channel3ActualFlow
    {
        /// <summary>
        /// Represents the address of the <see cref="Channel3ActualFlow"/> register. This field is constant.
        /// </summary>
        public const int Address = 51;

        /// <summary>
        /// Represents the payload type of the <see cref="Channel3ActualFlow"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.Float;

        /// <summary>
        /// Represents the length of the <see cref="Channel3ActualFlow"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="Channel3ActualFlow"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static float GetPayload(HarpMessage message)
        {
            return message.GetPayloadSingle();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="Channel3ActualFlow"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<float> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadSingle();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="Channel3ActualFlow"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Channel3ActualFlow"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, float value)
        {
            return HarpMessage.FromSingle(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="Channel3ActualFlow"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Channel3ActualFlow"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, float value)
        {
            return HarpMessage.FromSingle(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// Channel3ActualFlow register.
    /// </summary>
    /// <seealso cref="Channel3ActualFlow"/>
    [Description("Filters and selects timestamped messages from the Channel3ActualFlow register.")]
    public partial class TimestampedChannel3ActualFlow
    {
        /// <summary>
        /// Represents the address of the <see cref="Channel3ActualFlow"/> register. This field is constant.
        /// </summary>
        public const int Address = Channel3ActualFlow.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="Channel3ActualFlow"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<float> GetPayload(HarpMessage message)
        {
            return Channel3ActualFlow.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that actual flow-rate read for channel 4 - flowmeter 4 [ml/min].
    /// </summary>
    [Description("Actual flow-rate read for channel 4 - flowmeter 4 [ml/min].")]
    public partial class Channel4ActualFlow
    {
        /// <summary>
        /// Represents the address of the <see cref="Channel4ActualFlow"/> register. This field is constant.
        /// </summary>
        public const int Address = 52;

        /// <summary>
        /// Represents the payload type of the <see cref="Channel4ActualFlow"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.Float;

        /// <summary>
        /// Represents the length of the <see cref="Channel4ActualFlow"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="Channel4ActualFlow"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static float GetPayload(HarpMessage message)
        {
            return message.GetPayloadSingle();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="Channel4ActualFlow"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<float> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadSingle();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="Channel4ActualFlow"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Channel4ActualFlow"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, float value)
        {
            return HarpMessage.FromSingle(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="Channel4ActualFlow"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Channel4ActualFlow"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, float value)
        {
            return HarpMessage.FromSingle(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// Channel4ActualFlow register.
    /// </summary>
    /// <seealso cref="Channel4ActualFlow"/>
    [Description("Filters and selects timestamped messages from the Channel4ActualFlow register.")]
    public partial class TimestampedChannel4ActualFlow
    {
        /// <summary>
        /// Represents the address of the <see cref="Channel4ActualFlow"/> register. This field is constant.
        /// </summary>
        public const int Address = Channel4ActualFlow.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="Channel4ActualFlow"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<float> GetPayload(HarpMessage message)
        {
            return Channel4ActualFlow.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that switching frequency for proportional valve 0 - DO NOT CHANGE [Hz].
    /// </summary>
    [Description("Switching frequency for proportional valve 0 - DO NOT CHANGE [Hz].")]
    internal partial class Channel0Frequency
    {
        /// <summary>
        /// Represents the address of the <see cref="Channel0Frequency"/> register. This field is constant.
        /// </summary>
        public const int Address = 53;

        /// <summary>
        /// Represents the payload type of the <see cref="Channel0Frequency"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U16;

        /// <summary>
        /// Represents the length of the <see cref="Channel0Frequency"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;
    }

    /// <summary>
    /// Represents a register that switching frequency for proportional valve 1 - DO NOT CHANGE [Hz].
    /// </summary>
    [Description("Switching frequency for proportional valve 1 - DO NOT CHANGE [Hz].")]
    internal partial class Channel1Frequency
    {
        /// <summary>
        /// Represents the address of the <see cref="Channel1Frequency"/> register. This field is constant.
        /// </summary>
        public const int Address = 54;

        /// <summary>
        /// Represents the payload type of the <see cref="Channel1Frequency"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U16;

        /// <summary>
        /// Represents the length of the <see cref="Channel1Frequency"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;
    }

    /// <summary>
    /// Represents a register that switching frequency for proportional valve 2 - DO NOT CHANGE [Hz].
    /// </summary>
    [Description("Switching frequency for proportional valve 2 - DO NOT CHANGE [Hz].")]
    internal partial class Channel2Frequency
    {
        /// <summary>
        /// Represents the address of the <see cref="Channel2Frequency"/> register. This field is constant.
        /// </summary>
        public const int Address = 55;

        /// <summary>
        /// Represents the payload type of the <see cref="Channel2Frequency"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U16;

        /// <summary>
        /// Represents the length of the <see cref="Channel2Frequency"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;
    }

    /// <summary>
    /// Represents a register that switching frequency for proportional valve 3 - DO NOT CHANGE [Hz].
    /// </summary>
    [Description("Switching frequency for proportional valve 3 - DO NOT CHANGE [Hz].")]
    internal partial class Channel3Frequency
    {
        /// <summary>
        /// Represents the address of the <see cref="Channel3Frequency"/> register. This field is constant.
        /// </summary>
        public const int Address = 56;

        /// <summary>
        /// Represents the payload type of the <see cref="Channel3Frequency"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U16;

        /// <summary>
        /// Represents the length of the <see cref="Channel3Frequency"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;
    }

    /// <summary>
    /// Represents a register that switching frequency for proportional valve 4 - DO NOT CHANGE [Hz].
    /// </summary>
    [Description("Switching frequency for proportional valve 4 - DO NOT CHANGE [Hz].")]
    internal partial class Channel4Frequency
    {
        /// <summary>
        /// Represents the address of the <see cref="Channel4Frequency"/> register. This field is constant.
        /// </summary>
        public const int Address = 57;

        /// <summary>
        /// Represents the payload type of the <see cref="Channel4Frequency"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U16;

        /// <summary>
        /// Represents the length of the <see cref="Channel4Frequency"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;
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
        public const int Address = 58;

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
        public const int Address = 59;

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
        public const int Address = 60;

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
        public const int Address = 61;

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
        public const int Address = 62;

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
    /// Represents a register that set the specified digital output lines.
    /// </summary>
    [Description("Set the specified digital output lines.")]
    public partial class DigitalOutputSet
    {
        /// <summary>
        /// Represents the address of the <see cref="DigitalOutputSet"/> register. This field is constant.
        /// </summary>
        public const int Address = 63;

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
    /// Represents a register that clears the specified digital output lines.
    /// </summary>
    [Description("Clears the specified digital output lines.")]
    public partial class DigitalOutputClear
    {
        /// <summary>
        /// Represents the address of the <see cref="DigitalOutputClear"/> register. This field is constant.
        /// </summary>
        public const int Address = 64;

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
    /// Represents a register that toggles the specified digital output lines.
    /// </summary>
    [Description("Toggles the specified digital output lines.")]
    public partial class DigitalOutputToggle
    {
        /// <summary>
        /// Represents the address of the <see cref="DigitalOutputToggle"/> register. This field is constant.
        /// </summary>
        public const int Address = 65;

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
    /// Represents a register that write the state of all digital output lines.
    /// </summary>
    [Description("Write the state of all digital output lines.")]
    public partial class DigitalOutputState
    {
        /// <summary>
        /// Represents the address of the <see cref="DigitalOutputState"/> register. This field is constant.
        /// </summary>
        public const int Address = 66;

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
    public partial class EnableValvePulse
    {
        /// <summary>
        /// Represents the address of the <see cref="EnableValvePulse"/> register. This field is constant.
        /// </summary>
        public const int Address = 67;

        /// <summary>
        /// Represents the payload type of the <see cref="EnableValvePulse"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U16;

        /// <summary>
        /// Represents the length of the <see cref="EnableValvePulse"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="EnableValvePulse"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static Valves GetPayload(HarpMessage message)
        {
            return (Valves)message.GetPayloadUInt16();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="EnableValvePulse"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<Valves> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadUInt16();
            return Timestamped.Create((Valves)payload.Value, payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="EnableValvePulse"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="EnableValvePulse"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, Valves value)
        {
            return HarpMessage.FromUInt16(Address, messageType, (ushort)value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="EnableValvePulse"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="EnableValvePulse"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, Valves value)
        {
            return HarpMessage.FromUInt16(Address, timestamp, messageType, (ushort)value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// EnableValvePulse register.
    /// </summary>
    /// <seealso cref="EnableValvePulse"/>
    [Description("Filters and selects timestamped messages from the EnableValvePulse register.")]
    public partial class TimestampedEnableValvePulse
    {
        /// <summary>
        /// Represents the address of the <see cref="EnableValvePulse"/> register. This field is constant.
        /// </summary>
        public const int Address = EnableValvePulse.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="EnableValvePulse"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<Valves> GetPayload(HarpMessage message)
        {
            return EnableValvePulse.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that set the specified valve output lines.
    /// </summary>
    [Description("Set the specified valve output lines.")]
    public partial class ValveSet
    {
        /// <summary>
        /// Represents the address of the <see cref="ValveSet"/> register. This field is constant.
        /// </summary>
        public const int Address = 68;

        /// <summary>
        /// Represents the payload type of the <see cref="ValveSet"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U16;

        /// <summary>
        /// Represents the length of the <see cref="ValveSet"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="ValveSet"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static Valves GetPayload(HarpMessage message)
        {
            return (Valves)message.GetPayloadUInt16();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="ValveSet"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<Valves> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadUInt16();
            return Timestamped.Create((Valves)payload.Value, payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="ValveSet"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="ValveSet"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, Valves value)
        {
            return HarpMessage.FromUInt16(Address, messageType, (ushort)value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="ValveSet"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="ValveSet"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, Valves value)
        {
            return HarpMessage.FromUInt16(Address, timestamp, messageType, (ushort)value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// ValveSet register.
    /// </summary>
    /// <seealso cref="ValveSet"/>
    [Description("Filters and selects timestamped messages from the ValveSet register.")]
    public partial class TimestampedValveSet
    {
        /// <summary>
        /// Represents the address of the <see cref="ValveSet"/> register. This field is constant.
        /// </summary>
        public const int Address = ValveSet.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="ValveSet"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<Valves> GetPayload(HarpMessage message)
        {
            return ValveSet.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that clears the specified valve output lines.
    /// </summary>
    [Description("Clears the specified valve output lines.")]
    public partial class ValveClear
    {
        /// <summary>
        /// Represents the address of the <see cref="ValveClear"/> register. This field is constant.
        /// </summary>
        public const int Address = 69;

        /// <summary>
        /// Represents the payload type of the <see cref="ValveClear"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U16;

        /// <summary>
        /// Represents the length of the <see cref="ValveClear"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="ValveClear"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static Valves GetPayload(HarpMessage message)
        {
            return (Valves)message.GetPayloadUInt16();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="ValveClear"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<Valves> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadUInt16();
            return Timestamped.Create((Valves)payload.Value, payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="ValveClear"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="ValveClear"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, Valves value)
        {
            return HarpMessage.FromUInt16(Address, messageType, (ushort)value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="ValveClear"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="ValveClear"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, Valves value)
        {
            return HarpMessage.FromUInt16(Address, timestamp, messageType, (ushort)value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// ValveClear register.
    /// </summary>
    /// <seealso cref="ValveClear"/>
    [Description("Filters and selects timestamped messages from the ValveClear register.")]
    public partial class TimestampedValveClear
    {
        /// <summary>
        /// Represents the address of the <see cref="ValveClear"/> register. This field is constant.
        /// </summary>
        public const int Address = ValveClear.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="ValveClear"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<Valves> GetPayload(HarpMessage message)
        {
            return ValveClear.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that toggles the specified valve output lines.
    /// </summary>
    [Description("Toggles the specified valve output lines.")]
    public partial class ValveToggle
    {
        /// <summary>
        /// Represents the address of the <see cref="ValveToggle"/> register. This field is constant.
        /// </summary>
        public const int Address = 70;

        /// <summary>
        /// Represents the payload type of the <see cref="ValveToggle"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U16;

        /// <summary>
        /// Represents the length of the <see cref="ValveToggle"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="ValveToggle"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static Valves GetPayload(HarpMessage message)
        {
            return (Valves)message.GetPayloadUInt16();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="ValveToggle"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<Valves> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadUInt16();
            return Timestamped.Create((Valves)payload.Value, payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="ValveToggle"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="ValveToggle"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, Valves value)
        {
            return HarpMessage.FromUInt16(Address, messageType, (ushort)value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="ValveToggle"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="ValveToggle"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, Valves value)
        {
            return HarpMessage.FromUInt16(Address, timestamp, messageType, (ushort)value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// ValveToggle register.
    /// </summary>
    /// <seealso cref="ValveToggle"/>
    [Description("Filters and selects timestamped messages from the ValveToggle register.")]
    public partial class TimestampedValveToggle
    {
        /// <summary>
        /// Represents the address of the <see cref="ValveToggle"/> register. This field is constant.
        /// </summary>
        public const int Address = ValveToggle.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="ValveToggle"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<Valves> GetPayload(HarpMessage message)
        {
            return ValveToggle.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that controls the specified valve output lines.
    /// </summary>
    [Description("Controls the specified valve output lines.")]
    public partial class ValveState
    {
        /// <summary>
        /// Represents the address of the <see cref="ValveState"/> register. This field is constant.
        /// </summary>
        public const int Address = 71;

        /// <summary>
        /// Represents the payload type of the <see cref="ValveState"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U16;

        /// <summary>
        /// Represents the length of the <see cref="ValveState"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="ValveState"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static Valves GetPayload(HarpMessage message)
        {
            return (Valves)message.GetPayloadUInt16();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="ValveState"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<Valves> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadUInt16();
            return Timestamped.Create((Valves)payload.Value, payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="ValveState"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="ValveState"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, Valves value)
        {
            return HarpMessage.FromUInt16(Address, messageType, (ushort)value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="ValveState"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="ValveState"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, Valves value)
        {
            return HarpMessage.FromUInt16(Address, timestamp, messageType, (ushort)value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// ValveState register.
    /// </summary>
    /// <seealso cref="ValveState"/>
    [Description("Filters and selects timestamped messages from the ValveState register.")]
    public partial class TimestampedValveState
    {
        /// <summary>
        /// Represents the address of the <see cref="ValveState"/> register. This field is constant.
        /// </summary>
        public const int Address = ValveState.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="ValveState"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<Valves> GetPayload(HarpMessage message)
        {
            return ValveState.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that write the state of all odor valve output lines.
    /// </summary>
    [Description("Write the state of all odor valve output lines.")]
    public partial class OdorValveState
    {
        /// <summary>
        /// Represents the address of the <see cref="OdorValveState"/> register. This field is constant.
        /// </summary>
        public const int Address = 72;

        /// <summary>
        /// Represents the payload type of the <see cref="OdorValveState"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="OdorValveState"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="OdorValveState"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static OdorValves GetPayload(HarpMessage message)
        {
            return (OdorValves)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="OdorValveState"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<OdorValves> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((OdorValves)payload.Value, payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="OdorValveState"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="OdorValveState"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, OdorValves value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="OdorValveState"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="OdorValveState"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, OdorValves value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// OdorValveState register.
    /// </summary>
    /// <seealso cref="OdorValveState"/>
    [Description("Filters and selects timestamped messages from the OdorValveState register.")]
    public partial class TimestampedOdorValveState
    {
        /// <summary>
        /// Represents the address of the <see cref="OdorValveState"/> register. This field is constant.
        /// </summary>
        public const int Address = OdorValveState.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="OdorValveState"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<OdorValves> GetPayload(HarpMessage message)
        {
            return OdorValveState.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that write the state of all end valve output lines.
    /// </summary>
    [Description("Write the state of all end valve output lines.")]
    public partial class EndValveState
    {
        /// <summary>
        /// Represents the address of the <see cref="EndValveState"/> register. This field is constant.
        /// </summary>
        public const int Address = 73;

        /// <summary>
        /// Represents the payload type of the <see cref="EndValveState"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="EndValveState"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="EndValveState"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static EndValves GetPayload(HarpMessage message)
        {
            return (EndValves)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="EndValveState"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<EndValves> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((EndValves)payload.Value, payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="EndValveState"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="EndValveState"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, EndValves value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="EndValveState"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="EndValveState"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, EndValves value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// EndValveState register.
    /// </summary>
    /// <seealso cref="EndValveState"/>
    [Description("Filters and selects timestamped messages from the EndValveState register.")]
    public partial class TimestampedEndValveState
    {
        /// <summary>
        /// Represents the address of the <see cref="EndValveState"/> register. This field is constant.
        /// </summary>
        public const int Address = EndValveState.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="EndValveState"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<EndValves> GetPayload(HarpMessage message)
        {
            return EndValveState.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that write the state of all check valve output lines.
    /// </summary>
    [Description("Write the state of all check valve output lines.")]
    public partial class CheckValveState
    {
        /// <summary>
        /// Represents the address of the <see cref="CheckValveState"/> register. This field is constant.
        /// </summary>
        public const int Address = 74;

        /// <summary>
        /// Represents the payload type of the <see cref="CheckValveState"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U16;

        /// <summary>
        /// Represents the length of the <see cref="CheckValveState"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="CheckValveState"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static CheckValves GetPayload(HarpMessage message)
        {
            return (CheckValves)message.GetPayloadUInt16();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="CheckValveState"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<CheckValves> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadUInt16();
            return Timestamped.Create((CheckValves)payload.Value, payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="CheckValveState"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="CheckValveState"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, CheckValves value)
        {
            return HarpMessage.FromUInt16(Address, messageType, (ushort)value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="CheckValveState"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="CheckValveState"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, CheckValves value)
        {
            return HarpMessage.FromUInt16(Address, timestamp, messageType, (ushort)value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// CheckValveState register.
    /// </summary>
    /// <seealso cref="CheckValveState"/>
    [Description("Filters and selects timestamped messages from the CheckValveState register.")]
    public partial class TimestampedCheckValveState
    {
        /// <summary>
        /// Represents the address of the <see cref="CheckValveState"/> register. This field is constant.
        /// </summary>
        public const int Address = CheckValveState.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="CheckValveState"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<CheckValves> GetPayload(HarpMessage message)
        {
            return CheckValveState.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that sets the pulse duration for Valve0.
    /// </summary>
    [Description("Sets the pulse duration for Valve0.")]
    public partial class Valve0PulseDuration
    {
        /// <summary>
        /// Represents the address of the <see cref="Valve0PulseDuration"/> register. This field is constant.
        /// </summary>
        public const int Address = 75;

        /// <summary>
        /// Represents the payload type of the <see cref="Valve0PulseDuration"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U16;

        /// <summary>
        /// Represents the length of the <see cref="Valve0PulseDuration"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="Valve0PulseDuration"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static ushort GetPayload(HarpMessage message)
        {
            return message.GetPayloadUInt16();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="Valve0PulseDuration"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadUInt16();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="Valve0PulseDuration"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Valve0PulseDuration"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, ushort value)
        {
            return HarpMessage.FromUInt16(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="Valve0PulseDuration"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Valve0PulseDuration"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, ushort value)
        {
            return HarpMessage.FromUInt16(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// Valve0PulseDuration register.
    /// </summary>
    /// <seealso cref="Valve0PulseDuration"/>
    [Description("Filters and selects timestamped messages from the Valve0PulseDuration register.")]
    public partial class TimestampedValve0PulseDuration
    {
        /// <summary>
        /// Represents the address of the <see cref="Valve0PulseDuration"/> register. This field is constant.
        /// </summary>
        public const int Address = Valve0PulseDuration.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="Valve0PulseDuration"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort> GetPayload(HarpMessage message)
        {
            return Valve0PulseDuration.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that sets the pulse duration for Valve1.
    /// </summary>
    [Description("Sets the pulse duration for Valve1.")]
    public partial class Valve1PulseDuration
    {
        /// <summary>
        /// Represents the address of the <see cref="Valve1PulseDuration"/> register. This field is constant.
        /// </summary>
        public const int Address = 76;

        /// <summary>
        /// Represents the payload type of the <see cref="Valve1PulseDuration"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U16;

        /// <summary>
        /// Represents the length of the <see cref="Valve1PulseDuration"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="Valve1PulseDuration"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static ushort GetPayload(HarpMessage message)
        {
            return message.GetPayloadUInt16();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="Valve1PulseDuration"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadUInt16();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="Valve1PulseDuration"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Valve1PulseDuration"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, ushort value)
        {
            return HarpMessage.FromUInt16(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="Valve1PulseDuration"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Valve1PulseDuration"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, ushort value)
        {
            return HarpMessage.FromUInt16(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// Valve1PulseDuration register.
    /// </summary>
    /// <seealso cref="Valve1PulseDuration"/>
    [Description("Filters and selects timestamped messages from the Valve1PulseDuration register.")]
    public partial class TimestampedValve1PulseDuration
    {
        /// <summary>
        /// Represents the address of the <see cref="Valve1PulseDuration"/> register. This field is constant.
        /// </summary>
        public const int Address = Valve1PulseDuration.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="Valve1PulseDuration"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort> GetPayload(HarpMessage message)
        {
            return Valve1PulseDuration.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that sets the pulse duration for Valve2.
    /// </summary>
    [Description("Sets the pulse duration for Valve2.")]
    public partial class Valve2PulseDuration
    {
        /// <summary>
        /// Represents the address of the <see cref="Valve2PulseDuration"/> register. This field is constant.
        /// </summary>
        public const int Address = 77;

        /// <summary>
        /// Represents the payload type of the <see cref="Valve2PulseDuration"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U16;

        /// <summary>
        /// Represents the length of the <see cref="Valve2PulseDuration"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="Valve2PulseDuration"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static ushort GetPayload(HarpMessage message)
        {
            return message.GetPayloadUInt16();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="Valve2PulseDuration"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadUInt16();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="Valve2PulseDuration"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Valve2PulseDuration"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, ushort value)
        {
            return HarpMessage.FromUInt16(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="Valve2PulseDuration"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Valve2PulseDuration"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, ushort value)
        {
            return HarpMessage.FromUInt16(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// Valve2PulseDuration register.
    /// </summary>
    /// <seealso cref="Valve2PulseDuration"/>
    [Description("Filters and selects timestamped messages from the Valve2PulseDuration register.")]
    public partial class TimestampedValve2PulseDuration
    {
        /// <summary>
        /// Represents the address of the <see cref="Valve2PulseDuration"/> register. This field is constant.
        /// </summary>
        public const int Address = Valve2PulseDuration.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="Valve2PulseDuration"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort> GetPayload(HarpMessage message)
        {
            return Valve2PulseDuration.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that sets the pulse duration for Valve3.
    /// </summary>
    [Description("Sets the pulse duration for Valve3.")]
    public partial class Valve3PulseDuration
    {
        /// <summary>
        /// Represents the address of the <see cref="Valve3PulseDuration"/> register. This field is constant.
        /// </summary>
        public const int Address = 78;

        /// <summary>
        /// Represents the payload type of the <see cref="Valve3PulseDuration"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U16;

        /// <summary>
        /// Represents the length of the <see cref="Valve3PulseDuration"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="Valve3PulseDuration"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static ushort GetPayload(HarpMessage message)
        {
            return message.GetPayloadUInt16();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="Valve3PulseDuration"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadUInt16();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="Valve3PulseDuration"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Valve3PulseDuration"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, ushort value)
        {
            return HarpMessage.FromUInt16(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="Valve3PulseDuration"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Valve3PulseDuration"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, ushort value)
        {
            return HarpMessage.FromUInt16(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// Valve3PulseDuration register.
    /// </summary>
    /// <seealso cref="Valve3PulseDuration"/>
    [Description("Filters and selects timestamped messages from the Valve3PulseDuration register.")]
    public partial class TimestampedValve3PulseDuration
    {
        /// <summary>
        /// Represents the address of the <see cref="Valve3PulseDuration"/> register. This field is constant.
        /// </summary>
        public const int Address = Valve3PulseDuration.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="Valve3PulseDuration"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort> GetPayload(HarpMessage message)
        {
            return Valve3PulseDuration.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that sets the delay when CheckValvesConfig is Sync. Otherwise, sets the pulse duration for Check Valve0.
    /// </summary>
    [Description("Sets the delay when CheckValvesConfig is Sync. Otherwise, sets the pulse duration for Check Valve0.")]
    public partial class CheckValve0DelayPulseDuration
    {
        /// <summary>
        /// Represents the address of the <see cref="CheckValve0DelayPulseDuration"/> register. This field is constant.
        /// </summary>
        public const int Address = 79;

        /// <summary>
        /// Represents the payload type of the <see cref="CheckValve0DelayPulseDuration"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U16;

        /// <summary>
        /// Represents the length of the <see cref="CheckValve0DelayPulseDuration"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="CheckValve0DelayPulseDuration"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static ushort GetPayload(HarpMessage message)
        {
            return message.GetPayloadUInt16();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="CheckValve0DelayPulseDuration"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadUInt16();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="CheckValve0DelayPulseDuration"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="CheckValve0DelayPulseDuration"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, ushort value)
        {
            return HarpMessage.FromUInt16(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="CheckValve0DelayPulseDuration"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="CheckValve0DelayPulseDuration"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, ushort value)
        {
            return HarpMessage.FromUInt16(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// CheckValve0DelayPulseDuration register.
    /// </summary>
    /// <seealso cref="CheckValve0DelayPulseDuration"/>
    [Description("Filters and selects timestamped messages from the CheckValve0DelayPulseDuration register.")]
    public partial class TimestampedCheckValve0DelayPulseDuration
    {
        /// <summary>
        /// Represents the address of the <see cref="CheckValve0DelayPulseDuration"/> register. This field is constant.
        /// </summary>
        public const int Address = CheckValve0DelayPulseDuration.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="CheckValve0DelayPulseDuration"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort> GetPayload(HarpMessage message)
        {
            return CheckValve0DelayPulseDuration.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that sets the delay when CheckValvesConfig is Sync. Otherwise, sets the pulse duration for Check Valve1.
    /// </summary>
    [Description("Sets the delay when CheckValvesConfig is Sync. Otherwise, sets the pulse duration for Check Valve1.")]
    public partial class CheckValve1DelayPulseDuration
    {
        /// <summary>
        /// Represents the address of the <see cref="CheckValve1DelayPulseDuration"/> register. This field is constant.
        /// </summary>
        public const int Address = 80;

        /// <summary>
        /// Represents the payload type of the <see cref="CheckValve1DelayPulseDuration"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U16;

        /// <summary>
        /// Represents the length of the <see cref="CheckValve1DelayPulseDuration"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="CheckValve1DelayPulseDuration"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static ushort GetPayload(HarpMessage message)
        {
            return message.GetPayloadUInt16();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="CheckValve1DelayPulseDuration"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadUInt16();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="CheckValve1DelayPulseDuration"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="CheckValve1DelayPulseDuration"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, ushort value)
        {
            return HarpMessage.FromUInt16(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="CheckValve1DelayPulseDuration"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="CheckValve1DelayPulseDuration"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, ushort value)
        {
            return HarpMessage.FromUInt16(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// CheckValve1DelayPulseDuration register.
    /// </summary>
    /// <seealso cref="CheckValve1DelayPulseDuration"/>
    [Description("Filters and selects timestamped messages from the CheckValve1DelayPulseDuration register.")]
    public partial class TimestampedCheckValve1DelayPulseDuration
    {
        /// <summary>
        /// Represents the address of the <see cref="CheckValve1DelayPulseDuration"/> register. This field is constant.
        /// </summary>
        public const int Address = CheckValve1DelayPulseDuration.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="CheckValve1DelayPulseDuration"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort> GetPayload(HarpMessage message)
        {
            return CheckValve1DelayPulseDuration.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that sets the delay when CheckValvesConfig is Sync. Otherwise, sets the pulse duration for Check Valve2.
    /// </summary>
    [Description("Sets the delay when CheckValvesConfig is Sync. Otherwise, sets the pulse duration for Check Valve2.")]
    public partial class CheckValve2DelayPulseDuration
    {
        /// <summary>
        /// Represents the address of the <see cref="CheckValve2DelayPulseDuration"/> register. This field is constant.
        /// </summary>
        public const int Address = 81;

        /// <summary>
        /// Represents the payload type of the <see cref="CheckValve2DelayPulseDuration"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U16;

        /// <summary>
        /// Represents the length of the <see cref="CheckValve2DelayPulseDuration"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="CheckValve2DelayPulseDuration"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static ushort GetPayload(HarpMessage message)
        {
            return message.GetPayloadUInt16();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="CheckValve2DelayPulseDuration"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadUInt16();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="CheckValve2DelayPulseDuration"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="CheckValve2DelayPulseDuration"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, ushort value)
        {
            return HarpMessage.FromUInt16(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="CheckValve2DelayPulseDuration"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="CheckValve2DelayPulseDuration"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, ushort value)
        {
            return HarpMessage.FromUInt16(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// CheckValve2DelayPulseDuration register.
    /// </summary>
    /// <seealso cref="CheckValve2DelayPulseDuration"/>
    [Description("Filters and selects timestamped messages from the CheckValve2DelayPulseDuration register.")]
    public partial class TimestampedCheckValve2DelayPulseDuration
    {
        /// <summary>
        /// Represents the address of the <see cref="CheckValve2DelayPulseDuration"/> register. This field is constant.
        /// </summary>
        public const int Address = CheckValve2DelayPulseDuration.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="CheckValve2DelayPulseDuration"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort> GetPayload(HarpMessage message)
        {
            return CheckValve2DelayPulseDuration.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that sets the delay when CheckValvesConfig is Sync. Otherwise, sets the pulse duration for Check Valve3.
    /// </summary>
    [Description("Sets the delay when CheckValvesConfig is Sync. Otherwise, sets the pulse duration for Check Valve3.")]
    public partial class CheckValve3DelayPulseDuration
    {
        /// <summary>
        /// Represents the address of the <see cref="CheckValve3DelayPulseDuration"/> register. This field is constant.
        /// </summary>
        public const int Address = 82;

        /// <summary>
        /// Represents the payload type of the <see cref="CheckValve3DelayPulseDuration"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U16;

        /// <summary>
        /// Represents the length of the <see cref="CheckValve3DelayPulseDuration"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="CheckValve3DelayPulseDuration"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static ushort GetPayload(HarpMessage message)
        {
            return message.GetPayloadUInt16();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="CheckValve3DelayPulseDuration"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadUInt16();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="CheckValve3DelayPulseDuration"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="CheckValve3DelayPulseDuration"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, ushort value)
        {
            return HarpMessage.FromUInt16(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="CheckValve3DelayPulseDuration"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="CheckValve3DelayPulseDuration"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, ushort value)
        {
            return HarpMessage.FromUInt16(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// CheckValve3DelayPulseDuration register.
    /// </summary>
    /// <seealso cref="CheckValve3DelayPulseDuration"/>
    [Description("Filters and selects timestamped messages from the CheckValve3DelayPulseDuration register.")]
    public partial class TimestampedCheckValve3DelayPulseDuration
    {
        /// <summary>
        /// Represents the address of the <see cref="CheckValve3DelayPulseDuration"/> register. This field is constant.
        /// </summary>
        public const int Address = CheckValve3DelayPulseDuration.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="CheckValve3DelayPulseDuration"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort> GetPayload(HarpMessage message)
        {
            return CheckValve3DelayPulseDuration.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that sets the pulse duration for EndValve0.
    /// </summary>
    [Description("Sets the pulse duration for EndValve0.")]
    public partial class EndValve0PulseDuration
    {
        /// <summary>
        /// Represents the address of the <see cref="EndValve0PulseDuration"/> register. This field is constant.
        /// </summary>
        public const int Address = 83;

        /// <summary>
        /// Represents the payload type of the <see cref="EndValve0PulseDuration"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U16;

        /// <summary>
        /// Represents the length of the <see cref="EndValve0PulseDuration"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="EndValve0PulseDuration"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static ushort GetPayload(HarpMessage message)
        {
            return message.GetPayloadUInt16();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="EndValve0PulseDuration"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadUInt16();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="EndValve0PulseDuration"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="EndValve0PulseDuration"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, ushort value)
        {
            return HarpMessage.FromUInt16(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="EndValve0PulseDuration"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="EndValve0PulseDuration"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, ushort value)
        {
            return HarpMessage.FromUInt16(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// EndValve0PulseDuration register.
    /// </summary>
    /// <seealso cref="EndValve0PulseDuration"/>
    [Description("Filters and selects timestamped messages from the EndValve0PulseDuration register.")]
    public partial class TimestampedEndValve0PulseDuration
    {
        /// <summary>
        /// Represents the address of the <see cref="EndValve0PulseDuration"/> register. This field is constant.
        /// </summary>
        public const int Address = EndValve0PulseDuration.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="EndValve0PulseDuration"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort> GetPayload(HarpMessage message)
        {
            return EndValve0PulseDuration.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that sets the pulse duration for EndValve1.
    /// </summary>
    [Description("Sets the pulse duration for EndValve1.")]
    public partial class EndValve1PulseDuration
    {
        /// <summary>
        /// Represents the address of the <see cref="EndValve1PulseDuration"/> register. This field is constant.
        /// </summary>
        public const int Address = 84;

        /// <summary>
        /// Represents the payload type of the <see cref="EndValve1PulseDuration"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U16;

        /// <summary>
        /// Represents the length of the <see cref="EndValve1PulseDuration"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="EndValve1PulseDuration"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static ushort GetPayload(HarpMessage message)
        {
            return message.GetPayloadUInt16();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="EndValve1PulseDuration"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadUInt16();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="EndValve1PulseDuration"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="EndValve1PulseDuration"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, ushort value)
        {
            return HarpMessage.FromUInt16(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="EndValve1PulseDuration"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="EndValve1PulseDuration"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, ushort value)
        {
            return HarpMessage.FromUInt16(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// EndValve1PulseDuration register.
    /// </summary>
    /// <seealso cref="EndValve1PulseDuration"/>
    [Description("Filters and selects timestamped messages from the EndValve1PulseDuration register.")]
    public partial class TimestampedEndValve1PulseDuration
    {
        /// <summary>
        /// Represents the address of the <see cref="EndValve1PulseDuration"/> register. This field is constant.
        /// </summary>
        public const int Address = EndValve1PulseDuration.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="EndValve1PulseDuration"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort> GetPayload(HarpMessage message)
        {
            return EndValve1PulseDuration.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that dummy valve pulse duration [1:65535] ms.
    /// </summary>
    [Description("Dummy valve pulse duration [1:65535] ms.")]
    internal partial class PulseDummyValve
    {
        /// <summary>
        /// Represents the address of the <see cref="PulseDummyValve"/> register. This field is constant.
        /// </summary>
        public const int Address = 85;

        /// <summary>
        /// Represents the payload type of the <see cref="PulseDummyValve"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U16;

        /// <summary>
        /// Represents the length of the <see cref="PulseDummyValve"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;
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
        public const int Address = 86;

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
        public const int Address = 87;

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
        public const int Address = 88;

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
    /// Represents a register that mimic Valve0.
    /// </summary>
    [Description("Mimic Valve0.")]
    public partial class MimicValve0
    {
        /// <summary>
        /// Represents the address of the <see cref="MimicValve0"/> register. This field is constant.
        /// </summary>
        public const int Address = 89;

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
        public static MimicOutputs GetPayload(HarpMessage message)
        {
            return (MimicOutputs)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="MimicValve0"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<MimicOutputs> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((MimicOutputs)payload.Value, payload.Seconds);
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
        public static HarpMessage FromPayload(MessageType messageType, MimicOutputs value)
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
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, MimicOutputs value)
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
        public static Timestamped<MimicOutputs> GetPayload(HarpMessage message)
        {
            return MimicValve0.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that mimic Valve1.
    /// </summary>
    [Description("Mimic Valve1.")]
    public partial class MimicValve1
    {
        /// <summary>
        /// Represents the address of the <see cref="MimicValve1"/> register. This field is constant.
        /// </summary>
        public const int Address = 90;

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
        public static MimicOutputs GetPayload(HarpMessage message)
        {
            return (MimicOutputs)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="MimicValve1"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<MimicOutputs> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((MimicOutputs)payload.Value, payload.Seconds);
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
        public static HarpMessage FromPayload(MessageType messageType, MimicOutputs value)
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
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, MimicOutputs value)
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
        public static Timestamped<MimicOutputs> GetPayload(HarpMessage message)
        {
            return MimicValve1.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that mimic Valve2.
    /// </summary>
    [Description("Mimic Valve2.")]
    public partial class MimicValve2
    {
        /// <summary>
        /// Represents the address of the <see cref="MimicValve2"/> register. This field is constant.
        /// </summary>
        public const int Address = 91;

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
        public static MimicOutputs GetPayload(HarpMessage message)
        {
            return (MimicOutputs)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="MimicValve2"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<MimicOutputs> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((MimicOutputs)payload.Value, payload.Seconds);
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
        public static HarpMessage FromPayload(MessageType messageType, MimicOutputs value)
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
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, MimicOutputs value)
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
        public static Timestamped<MimicOutputs> GetPayload(HarpMessage message)
        {
            return MimicValve2.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that mimic Valve3.
    /// </summary>
    [Description("Mimic Valve3.")]
    public partial class MimicValve3
    {
        /// <summary>
        /// Represents the address of the <see cref="MimicValve3"/> register. This field is constant.
        /// </summary>
        public const int Address = 92;

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
        public static MimicOutputs GetPayload(HarpMessage message)
        {
            return (MimicOutputs)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="MimicValve3"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<MimicOutputs> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((MimicOutputs)payload.Value, payload.Seconds);
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
        public static HarpMessage FromPayload(MessageType messageType, MimicOutputs value)
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
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, MimicOutputs value)
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
        public static Timestamped<MimicOutputs> GetPayload(HarpMessage message)
        {
            return MimicValve3.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that mimic Check Valve0.
    /// </summary>
    [Description("Mimic Check Valve0.")]
    public partial class MimicCheckValve0
    {
        /// <summary>
        /// Represents the address of the <see cref="MimicCheckValve0"/> register. This field is constant.
        /// </summary>
        public const int Address = 93;

        /// <summary>
        /// Represents the payload type of the <see cref="MimicCheckValve0"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="MimicCheckValve0"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="MimicCheckValve0"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static MimicOutputs GetPayload(HarpMessage message)
        {
            return (MimicOutputs)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="MimicCheckValve0"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<MimicOutputs> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((MimicOutputs)payload.Value, payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="MimicCheckValve0"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="MimicCheckValve0"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, MimicOutputs value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="MimicCheckValve0"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="MimicCheckValve0"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, MimicOutputs value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// MimicCheckValve0 register.
    /// </summary>
    /// <seealso cref="MimicCheckValve0"/>
    [Description("Filters and selects timestamped messages from the MimicCheckValve0 register.")]
    public partial class TimestampedMimicCheckValve0
    {
        /// <summary>
        /// Represents the address of the <see cref="MimicCheckValve0"/> register. This field is constant.
        /// </summary>
        public const int Address = MimicCheckValve0.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="MimicCheckValve0"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<MimicOutputs> GetPayload(HarpMessage message)
        {
            return MimicCheckValve0.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that mimic Check Valve1.
    /// </summary>
    [Description("Mimic Check Valve1.")]
    public partial class MimicCheckValve1
    {
        /// <summary>
        /// Represents the address of the <see cref="MimicCheckValve1"/> register. This field is constant.
        /// </summary>
        public const int Address = 94;

        /// <summary>
        /// Represents the payload type of the <see cref="MimicCheckValve1"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="MimicCheckValve1"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="MimicCheckValve1"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static MimicOutputs GetPayload(HarpMessage message)
        {
            return (MimicOutputs)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="MimicCheckValve1"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<MimicOutputs> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((MimicOutputs)payload.Value, payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="MimicCheckValve1"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="MimicCheckValve1"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, MimicOutputs value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="MimicCheckValve1"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="MimicCheckValve1"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, MimicOutputs value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// MimicCheckValve1 register.
    /// </summary>
    /// <seealso cref="MimicCheckValve1"/>
    [Description("Filters and selects timestamped messages from the MimicCheckValve1 register.")]
    public partial class TimestampedMimicCheckValve1
    {
        /// <summary>
        /// Represents the address of the <see cref="MimicCheckValve1"/> register. This field is constant.
        /// </summary>
        public const int Address = MimicCheckValve1.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="MimicCheckValve1"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<MimicOutputs> GetPayload(HarpMessage message)
        {
            return MimicCheckValve1.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that mimic Check Valve2.
    /// </summary>
    [Description("Mimic Check Valve2.")]
    public partial class MimicCheckValve2
    {
        /// <summary>
        /// Represents the address of the <see cref="MimicCheckValve2"/> register. This field is constant.
        /// </summary>
        public const int Address = 95;

        /// <summary>
        /// Represents the payload type of the <see cref="MimicCheckValve2"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="MimicCheckValve2"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="MimicCheckValve2"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static MimicOutputs GetPayload(HarpMessage message)
        {
            return (MimicOutputs)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="MimicCheckValve2"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<MimicOutputs> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((MimicOutputs)payload.Value, payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="MimicCheckValve2"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="MimicCheckValve2"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, MimicOutputs value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="MimicCheckValve2"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="MimicCheckValve2"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, MimicOutputs value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// MimicCheckValve2 register.
    /// </summary>
    /// <seealso cref="MimicCheckValve2"/>
    [Description("Filters and selects timestamped messages from the MimicCheckValve2 register.")]
    public partial class TimestampedMimicCheckValve2
    {
        /// <summary>
        /// Represents the address of the <see cref="MimicCheckValve2"/> register. This field is constant.
        /// </summary>
        public const int Address = MimicCheckValve2.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="MimicCheckValve2"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<MimicOutputs> GetPayload(HarpMessage message)
        {
            return MimicCheckValve2.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that mimic Check Valve3.
    /// </summary>
    [Description("Mimic Check Valve3.")]
    public partial class MimicCheckValve3
    {
        /// <summary>
        /// Represents the address of the <see cref="MimicCheckValve3"/> register. This field is constant.
        /// </summary>
        public const int Address = 96;

        /// <summary>
        /// Represents the payload type of the <see cref="MimicCheckValve3"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="MimicCheckValve3"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="MimicCheckValve3"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static MimicOutputs GetPayload(HarpMessage message)
        {
            return (MimicOutputs)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="MimicCheckValve3"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<MimicOutputs> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((MimicOutputs)payload.Value, payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="MimicCheckValve3"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="MimicCheckValve3"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, MimicOutputs value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="MimicCheckValve3"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="MimicCheckValve3"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, MimicOutputs value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// MimicCheckValve3 register.
    /// </summary>
    /// <seealso cref="MimicCheckValve3"/>
    [Description("Filters and selects timestamped messages from the MimicCheckValve3 register.")]
    public partial class TimestampedMimicCheckValve3
    {
        /// <summary>
        /// Represents the address of the <see cref="MimicCheckValve3"/> register. This field is constant.
        /// </summary>
        public const int Address = MimicCheckValve3.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="MimicCheckValve3"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<MimicOutputs> GetPayload(HarpMessage message)
        {
            return MimicCheckValve3.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that mimic EndValve0.
    /// </summary>
    [Description("Mimic EndValve0.")]
    public partial class MimicEndValve0
    {
        /// <summary>
        /// Represents the address of the <see cref="MimicEndValve0"/> register. This field is constant.
        /// </summary>
        public const int Address = 97;

        /// <summary>
        /// Represents the payload type of the <see cref="MimicEndValve0"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="MimicEndValve0"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="MimicEndValve0"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static MimicOutputs GetPayload(HarpMessage message)
        {
            return (MimicOutputs)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="MimicEndValve0"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<MimicOutputs> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((MimicOutputs)payload.Value, payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="MimicEndValve0"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="MimicEndValve0"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, MimicOutputs value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="MimicEndValve0"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="MimicEndValve0"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, MimicOutputs value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// MimicEndValve0 register.
    /// </summary>
    /// <seealso cref="MimicEndValve0"/>
    [Description("Filters and selects timestamped messages from the MimicEndValve0 register.")]
    public partial class TimestampedMimicEndValve0
    {
        /// <summary>
        /// Represents the address of the <see cref="MimicEndValve0"/> register. This field is constant.
        /// </summary>
        public const int Address = MimicEndValve0.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="MimicEndValve0"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<MimicOutputs> GetPayload(HarpMessage message)
        {
            return MimicEndValve0.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that mimic EndValve1.
    /// </summary>
    [Description("Mimic EndValve1.")]
    public partial class MimicEndValve1
    {
        /// <summary>
        /// Represents the address of the <see cref="MimicEndValve1"/> register. This field is constant.
        /// </summary>
        public const int Address = 98;

        /// <summary>
        /// Represents the payload type of the <see cref="MimicEndValve1"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="MimicEndValve1"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="MimicEndValve1"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static MimicOutputs GetPayload(HarpMessage message)
        {
            return (MimicOutputs)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="MimicEndValve1"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<MimicOutputs> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((MimicOutputs)payload.Value, payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="MimicEndValve1"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="MimicEndValve1"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, MimicOutputs value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="MimicEndValve1"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="MimicEndValve1"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, MimicOutputs value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// MimicEndValve1 register.
    /// </summary>
    /// <seealso cref="MimicEndValve1"/>
    [Description("Filters and selects timestamped messages from the MimicEndValve1 register.")]
    public partial class TimestampedMimicEndValve1
    {
        /// <summary>
        /// Represents the address of the <see cref="MimicEndValve1"/> register. This field is constant.
        /// </summary>
        public const int Address = MimicEndValve1.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="MimicEndValve1"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<MimicOutputs> GetPayload(HarpMessage message)
        {
            return MimicEndValve1.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that mimic DummyValve.
    /// </summary>
    [Description("Mimic DummyValve.")]
    internal partial class MimicDummyValve
    {
        /// <summary>
        /// Represents the address of the <see cref="MimicDummyValve"/> register. This field is constant.
        /// </summary>
        public const int Address = 99;

        /// <summary>
        /// Represents the payload type of the <see cref="MimicDummyValve"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="MimicDummyValve"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;
    }

    /// <summary>
    /// Represents a register that enable the valves control via low-level IO screw terminals.
    /// </summary>
    [Description("Enable the valves control via low-level IO screw terminals.")]
    public partial class EnableValveExternalControl
    {
        /// <summary>
        /// Represents the address of the <see cref="EnableValveExternalControl"/> register. This field is constant.
        /// </summary>
        public const int Address = 100;

        /// <summary>
        /// Represents the payload type of the <see cref="EnableValveExternalControl"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="EnableValveExternalControl"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="EnableValveExternalControl"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static EnableFlag GetPayload(HarpMessage message)
        {
            return (EnableFlag)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="EnableValveExternalControl"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<EnableFlag> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((EnableFlag)payload.Value, payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="EnableValveExternalControl"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="EnableValveExternalControl"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, EnableFlag value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="EnableValveExternalControl"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="EnableValveExternalControl"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, EnableFlag value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// EnableValveExternalControl register.
    /// </summary>
    /// <seealso cref="EnableValveExternalControl"/>
    [Description("Filters and selects timestamped messages from the EnableValveExternalControl register.")]
    public partial class TimestampedEnableValveExternalControl
    {
        /// <summary>
        /// Represents the address of the <see cref="EnableValveExternalControl"/> register. This field is constant.
        /// </summary>
        public const int Address = EnableValveExternalControl.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="EnableValveExternalControl"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<EnableFlag> GetPayload(HarpMessage message)
        {
            return EnableValveExternalControl.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that selects the flow range for the channel 3.
    /// </summary>
    [Description("Selects the flow range for the channel 3.")]
    public partial class Channel3Range
    {
        /// <summary>
        /// Represents the address of the <see cref="Channel3Range"/> register. This field is constant.
        /// </summary>
        public const int Address = 101;

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
    /// Represents a register that enable the check valve to sync with the respective odor valve.
    /// </summary>
    [Description("Enable the check valve to sync with the respective odor valve.")]
    public partial class EnableCheckValveSync
    {
        /// <summary>
        /// Represents the address of the <see cref="EnableCheckValveSync"/> register. This field is constant.
        /// </summary>
        public const int Address = 102;

        /// <summary>
        /// Represents the payload type of the <see cref="EnableCheckValveSync"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U16;

        /// <summary>
        /// Represents the length of the <see cref="EnableCheckValveSync"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="EnableCheckValveSync"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static CheckValves GetPayload(HarpMessage message)
        {
            return (CheckValves)message.GetPayloadUInt16();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="EnableCheckValveSync"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<CheckValves> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadUInt16();
            return Timestamped.Create((CheckValves)payload.Value, payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="EnableCheckValveSync"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="EnableCheckValveSync"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, CheckValves value)
        {
            return HarpMessage.FromUInt16(Address, messageType, (ushort)value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="EnableCheckValveSync"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="EnableCheckValveSync"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, CheckValves value)
        {
            return HarpMessage.FromUInt16(Address, timestamp, messageType, (ushort)value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// EnableCheckValveSync register.
    /// </summary>
    /// <seealso cref="EnableCheckValveSync"/>
    [Description("Filters and selects timestamped messages from the EnableCheckValveSync register.")]
    public partial class TimestampedEnableCheckValveSync
    {
        /// <summary>
        /// Represents the address of the <see cref="EnableCheckValveSync"/> register. This field is constant.
        /// </summary>
        public const int Address = EnableCheckValveSync.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="EnableCheckValveSync"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<CheckValves> GetPayload(HarpMessage message)
        {
            return EnableCheckValveSync.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that temperature sensor reading value.
    /// </summary>
    [Description("Temperature sensor reading value.")]
    public partial class TemperatureValue
    {
        /// <summary>
        /// Represents the address of the <see cref="TemperatureValue"/> register. This field is constant.
        /// </summary>
        public const int Address = 103;

        /// <summary>
        /// Represents the payload type of the <see cref="TemperatureValue"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="TemperatureValue"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="TemperatureValue"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static byte GetPayload(HarpMessage message)
        {
            return message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="TemperatureValue"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<byte> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadByte();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="TemperatureValue"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="TemperatureValue"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, byte value)
        {
            return HarpMessage.FromByte(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="TemperatureValue"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="TemperatureValue"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, byte value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// TemperatureValue register.
    /// </summary>
    /// <seealso cref="TemperatureValue"/>
    [Description("Filters and selects timestamped messages from the TemperatureValue register.")]
    public partial class TimestampedTemperatureValue
    {
        /// <summary>
        /// Represents the address of the <see cref="TemperatureValue"/> register. This field is constant.
        /// </summary>
        public const int Address = TemperatureValue.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="TemperatureValue"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<byte> GetPayload(HarpMessage message)
        {
            return TemperatureValue.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that enable flow adjustment based on the temperature calibration.
    /// </summary>
    [Description("Enable flow adjustment based on the temperature calibration.")]
    public partial class EnableTemperatureCalibration
    {
        /// <summary>
        /// Represents the address of the <see cref="EnableTemperatureCalibration"/> register. This field is constant.
        /// </summary>
        public const int Address = 104;

        /// <summary>
        /// Represents the payload type of the <see cref="EnableTemperatureCalibration"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="EnableTemperatureCalibration"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="EnableTemperatureCalibration"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static EnableFlag GetPayload(HarpMessage message)
        {
            return (EnableFlag)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="EnableTemperatureCalibration"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<EnableFlag> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((EnableFlag)payload.Value, payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="EnableTemperatureCalibration"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="EnableTemperatureCalibration"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, EnableFlag value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="EnableTemperatureCalibration"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="EnableTemperatureCalibration"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, EnableFlag value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// EnableTemperatureCalibration register.
    /// </summary>
    /// <seealso cref="EnableTemperatureCalibration"/>
    [Description("Filters and selects timestamped messages from the EnableTemperatureCalibration register.")]
    public partial class TimestampedEnableTemperatureCalibration
    {
        /// <summary>
        /// Represents the address of the <see cref="EnableTemperatureCalibration"/> register. This field is constant.
        /// </summary>
        public const int Address = EnableTemperatureCalibration.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="EnableTemperatureCalibration"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<EnableFlag> GetPayload(HarpMessage message)
        {
            return EnableTemperatureCalibration.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that temperature value measured during the device calibration.
    /// </summary>
    [Description("Temperature value measured during the device calibration.")]
    public partial class TemperatureCalibrationValue
    {
        /// <summary>
        /// Represents the address of the <see cref="TemperatureCalibrationValue"/> register. This field is constant.
        /// </summary>
        public const int Address = 105;

        /// <summary>
        /// Represents the payload type of the <see cref="TemperatureCalibrationValue"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="TemperatureCalibrationValue"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="TemperatureCalibrationValue"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static byte GetPayload(HarpMessage message)
        {
            return message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="TemperatureCalibrationValue"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<byte> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadByte();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="TemperatureCalibrationValue"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="TemperatureCalibrationValue"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, byte value)
        {
            return HarpMessage.FromByte(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="TemperatureCalibrationValue"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="TemperatureCalibrationValue"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, byte value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// TemperatureCalibrationValue register.
    /// </summary>
    /// <seealso cref="TemperatureCalibrationValue"/>
    [Description("Filters and selects timestamped messages from the TemperatureCalibrationValue register.")]
    public partial class TimestampedTemperatureCalibrationValue
    {
        /// <summary>
        /// Represents the address of the <see cref="TemperatureCalibrationValue"/> register. This field is constant.
        /// </summary>
        public const int Address = TemperatureCalibrationValue.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="TemperatureCalibrationValue"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<byte> GetPayload(HarpMessage message)
        {
            return TemperatureCalibrationValue.GetTimestampedPayload(message);
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
        public const int Address = 106;

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
    /// <seealso cref="CreateFlowmeterPayload"/>
    /// <seealso cref="CreateDI0StatePayload"/>
    /// <seealso cref="CreateChannel0UserCalibrationPayload"/>
    /// <seealso cref="CreateChannel1UserCalibrationPayload"/>
    /// <seealso cref="CreateChannel2UserCalibrationPayload"/>
    /// <seealso cref="CreateChannel3UserCalibrationPayload"/>
    /// <seealso cref="CreateChannel4UserCalibrationPayload"/>
    /// <seealso cref="CreateChannel3UserCalibrationAuxPayload"/>
    /// <seealso cref="CreateUserCalibrationEnablePayload"/>
    /// <seealso cref="CreateChannel0TargetFlowPayload"/>
    /// <seealso cref="CreateChannel1TargetFlowPayload"/>
    /// <seealso cref="CreateChannel2TargetFlowPayload"/>
    /// <seealso cref="CreateChannel3TargetFlowPayload"/>
    /// <seealso cref="CreateChannel4TargetFlowPayload"/>
    /// <seealso cref="CreateChannelsTargetFlowPayload"/>
    /// <seealso cref="CreateChannel0ActualFlowPayload"/>
    /// <seealso cref="CreateChannel1ActualFlowPayload"/>
    /// <seealso cref="CreateChannel2ActualFlowPayload"/>
    /// <seealso cref="CreateChannel3ActualFlowPayload"/>
    /// <seealso cref="CreateChannel4ActualFlowPayload"/>
    /// <seealso cref="CreateChannel0DutyCyclePayload"/>
    /// <seealso cref="CreateChannel1DutyCyclePayload"/>
    /// <seealso cref="CreateChannel2DutyCyclePayload"/>
    /// <seealso cref="CreateChannel3DutyCyclePayload"/>
    /// <seealso cref="CreateChannel4DutyCyclePayload"/>
    /// <seealso cref="CreateDigitalOutputSetPayload"/>
    /// <seealso cref="CreateDigitalOutputClearPayload"/>
    /// <seealso cref="CreateDigitalOutputTogglePayload"/>
    /// <seealso cref="CreateDigitalOutputStatePayload"/>
    /// <seealso cref="CreateEnableValvePulsePayload"/>
    /// <seealso cref="CreateValveSetPayload"/>
    /// <seealso cref="CreateValveClearPayload"/>
    /// <seealso cref="CreateValveTogglePayload"/>
    /// <seealso cref="CreateValveStatePayload"/>
    /// <seealso cref="CreateOdorValveStatePayload"/>
    /// <seealso cref="CreateEndValveStatePayload"/>
    /// <seealso cref="CreateCheckValveStatePayload"/>
    /// <seealso cref="CreateValve0PulseDurationPayload"/>
    /// <seealso cref="CreateValve1PulseDurationPayload"/>
    /// <seealso cref="CreateValve2PulseDurationPayload"/>
    /// <seealso cref="CreateValve3PulseDurationPayload"/>
    /// <seealso cref="CreateCheckValve0DelayPulseDurationPayload"/>
    /// <seealso cref="CreateCheckValve1DelayPulseDurationPayload"/>
    /// <seealso cref="CreateCheckValve2DelayPulseDurationPayload"/>
    /// <seealso cref="CreateCheckValve3DelayPulseDurationPayload"/>
    /// <seealso cref="CreateEndValve0PulseDurationPayload"/>
    /// <seealso cref="CreateEndValve1PulseDurationPayload"/>
    /// <seealso cref="CreateDO0SyncPayload"/>
    /// <seealso cref="CreateDO1SyncPayload"/>
    /// <seealso cref="CreateDI0TriggerPayload"/>
    /// <seealso cref="CreateMimicValve0Payload"/>
    /// <seealso cref="CreateMimicValve1Payload"/>
    /// <seealso cref="CreateMimicValve2Payload"/>
    /// <seealso cref="CreateMimicValve3Payload"/>
    /// <seealso cref="CreateMimicCheckValve0Payload"/>
    /// <seealso cref="CreateMimicCheckValve1Payload"/>
    /// <seealso cref="CreateMimicCheckValve2Payload"/>
    /// <seealso cref="CreateMimicCheckValve3Payload"/>
    /// <seealso cref="CreateMimicEndValve0Payload"/>
    /// <seealso cref="CreateMimicEndValve1Payload"/>
    /// <seealso cref="CreateEnableValveExternalControlPayload"/>
    /// <seealso cref="CreateChannel3RangePayload"/>
    /// <seealso cref="CreateEnableCheckValveSyncPayload"/>
    /// <seealso cref="CreateTemperatureValuePayload"/>
    /// <seealso cref="CreateEnableTemperatureCalibrationPayload"/>
    /// <seealso cref="CreateTemperatureCalibrationValuePayload"/>
    /// <seealso cref="CreateEnableEventsPayload"/>
    [XmlInclude(typeof(CreateEnableFlowPayload))]
    [XmlInclude(typeof(CreateFlowmeterPayload))]
    [XmlInclude(typeof(CreateDI0StatePayload))]
    [XmlInclude(typeof(CreateChannel0UserCalibrationPayload))]
    [XmlInclude(typeof(CreateChannel1UserCalibrationPayload))]
    [XmlInclude(typeof(CreateChannel2UserCalibrationPayload))]
    [XmlInclude(typeof(CreateChannel3UserCalibrationPayload))]
    [XmlInclude(typeof(CreateChannel4UserCalibrationPayload))]
    [XmlInclude(typeof(CreateChannel3UserCalibrationAuxPayload))]
    [XmlInclude(typeof(CreateUserCalibrationEnablePayload))]
    [XmlInclude(typeof(CreateChannel0TargetFlowPayload))]
    [XmlInclude(typeof(CreateChannel1TargetFlowPayload))]
    [XmlInclude(typeof(CreateChannel2TargetFlowPayload))]
    [XmlInclude(typeof(CreateChannel3TargetFlowPayload))]
    [XmlInclude(typeof(CreateChannel4TargetFlowPayload))]
    [XmlInclude(typeof(CreateChannelsTargetFlowPayload))]
    [XmlInclude(typeof(CreateChannel0ActualFlowPayload))]
    [XmlInclude(typeof(CreateChannel1ActualFlowPayload))]
    [XmlInclude(typeof(CreateChannel2ActualFlowPayload))]
    [XmlInclude(typeof(CreateChannel3ActualFlowPayload))]
    [XmlInclude(typeof(CreateChannel4ActualFlowPayload))]
    [XmlInclude(typeof(CreateChannel0DutyCyclePayload))]
    [XmlInclude(typeof(CreateChannel1DutyCyclePayload))]
    [XmlInclude(typeof(CreateChannel2DutyCyclePayload))]
    [XmlInclude(typeof(CreateChannel3DutyCyclePayload))]
    [XmlInclude(typeof(CreateChannel4DutyCyclePayload))]
    [XmlInclude(typeof(CreateDigitalOutputSetPayload))]
    [XmlInclude(typeof(CreateDigitalOutputClearPayload))]
    [XmlInclude(typeof(CreateDigitalOutputTogglePayload))]
    [XmlInclude(typeof(CreateDigitalOutputStatePayload))]
    [XmlInclude(typeof(CreateEnableValvePulsePayload))]
    [XmlInclude(typeof(CreateValveSetPayload))]
    [XmlInclude(typeof(CreateValveClearPayload))]
    [XmlInclude(typeof(CreateValveTogglePayload))]
    [XmlInclude(typeof(CreateValveStatePayload))]
    [XmlInclude(typeof(CreateOdorValveStatePayload))]
    [XmlInclude(typeof(CreateEndValveStatePayload))]
    [XmlInclude(typeof(CreateCheckValveStatePayload))]
    [XmlInclude(typeof(CreateValve0PulseDurationPayload))]
    [XmlInclude(typeof(CreateValve1PulseDurationPayload))]
    [XmlInclude(typeof(CreateValve2PulseDurationPayload))]
    [XmlInclude(typeof(CreateValve3PulseDurationPayload))]
    [XmlInclude(typeof(CreateCheckValve0DelayPulseDurationPayload))]
    [XmlInclude(typeof(CreateCheckValve1DelayPulseDurationPayload))]
    [XmlInclude(typeof(CreateCheckValve2DelayPulseDurationPayload))]
    [XmlInclude(typeof(CreateCheckValve3DelayPulseDurationPayload))]
    [XmlInclude(typeof(CreateEndValve0PulseDurationPayload))]
    [XmlInclude(typeof(CreateEndValve1PulseDurationPayload))]
    [XmlInclude(typeof(CreateDO0SyncPayload))]
    [XmlInclude(typeof(CreateDO1SyncPayload))]
    [XmlInclude(typeof(CreateDI0TriggerPayload))]
    [XmlInclude(typeof(CreateMimicValve0Payload))]
    [XmlInclude(typeof(CreateMimicValve1Payload))]
    [XmlInclude(typeof(CreateMimicValve2Payload))]
    [XmlInclude(typeof(CreateMimicValve3Payload))]
    [XmlInclude(typeof(CreateMimicCheckValve0Payload))]
    [XmlInclude(typeof(CreateMimicCheckValve1Payload))]
    [XmlInclude(typeof(CreateMimicCheckValve2Payload))]
    [XmlInclude(typeof(CreateMimicCheckValve3Payload))]
    [XmlInclude(typeof(CreateMimicEndValve0Payload))]
    [XmlInclude(typeof(CreateMimicEndValve1Payload))]
    [XmlInclude(typeof(CreateEnableValveExternalControlPayload))]
    [XmlInclude(typeof(CreateChannel3RangePayload))]
    [XmlInclude(typeof(CreateEnableCheckValveSyncPayload))]
    [XmlInclude(typeof(CreateTemperatureValuePayload))]
    [XmlInclude(typeof(CreateEnableTemperatureCalibrationPayload))]
    [XmlInclude(typeof(CreateTemperatureCalibrationValuePayload))]
    [XmlInclude(typeof(CreateEnableEventsPayload))]
    [XmlInclude(typeof(CreateTimestampedEnableFlowPayload))]
    [XmlInclude(typeof(CreateTimestampedFlowmeterPayload))]
    [XmlInclude(typeof(CreateTimestampedDI0StatePayload))]
    [XmlInclude(typeof(CreateTimestampedChannel0UserCalibrationPayload))]
    [XmlInclude(typeof(CreateTimestampedChannel1UserCalibrationPayload))]
    [XmlInclude(typeof(CreateTimestampedChannel2UserCalibrationPayload))]
    [XmlInclude(typeof(CreateTimestampedChannel3UserCalibrationPayload))]
    [XmlInclude(typeof(CreateTimestampedChannel4UserCalibrationPayload))]
    [XmlInclude(typeof(CreateTimestampedChannel3UserCalibrationAuxPayload))]
    [XmlInclude(typeof(CreateTimestampedUserCalibrationEnablePayload))]
    [XmlInclude(typeof(CreateTimestampedChannel0TargetFlowPayload))]
    [XmlInclude(typeof(CreateTimestampedChannel1TargetFlowPayload))]
    [XmlInclude(typeof(CreateTimestampedChannel2TargetFlowPayload))]
    [XmlInclude(typeof(CreateTimestampedChannel3TargetFlowPayload))]
    [XmlInclude(typeof(CreateTimestampedChannel4TargetFlowPayload))]
    [XmlInclude(typeof(CreateTimestampedChannelsTargetFlowPayload))]
    [XmlInclude(typeof(CreateTimestampedChannel0ActualFlowPayload))]
    [XmlInclude(typeof(CreateTimestampedChannel1ActualFlowPayload))]
    [XmlInclude(typeof(CreateTimestampedChannel2ActualFlowPayload))]
    [XmlInclude(typeof(CreateTimestampedChannel3ActualFlowPayload))]
    [XmlInclude(typeof(CreateTimestampedChannel4ActualFlowPayload))]
    [XmlInclude(typeof(CreateTimestampedChannel0DutyCyclePayload))]
    [XmlInclude(typeof(CreateTimestampedChannel1DutyCyclePayload))]
    [XmlInclude(typeof(CreateTimestampedChannel2DutyCyclePayload))]
    [XmlInclude(typeof(CreateTimestampedChannel3DutyCyclePayload))]
    [XmlInclude(typeof(CreateTimestampedChannel4DutyCyclePayload))]
    [XmlInclude(typeof(CreateTimestampedDigitalOutputSetPayload))]
    [XmlInclude(typeof(CreateTimestampedDigitalOutputClearPayload))]
    [XmlInclude(typeof(CreateTimestampedDigitalOutputTogglePayload))]
    [XmlInclude(typeof(CreateTimestampedDigitalOutputStatePayload))]
    [XmlInclude(typeof(CreateTimestampedEnableValvePulsePayload))]
    [XmlInclude(typeof(CreateTimestampedValveSetPayload))]
    [XmlInclude(typeof(CreateTimestampedValveClearPayload))]
    [XmlInclude(typeof(CreateTimestampedValveTogglePayload))]
    [XmlInclude(typeof(CreateTimestampedValveStatePayload))]
    [XmlInclude(typeof(CreateTimestampedOdorValveStatePayload))]
    [XmlInclude(typeof(CreateTimestampedEndValveStatePayload))]
    [XmlInclude(typeof(CreateTimestampedCheckValveStatePayload))]
    [XmlInclude(typeof(CreateTimestampedValve0PulseDurationPayload))]
    [XmlInclude(typeof(CreateTimestampedValve1PulseDurationPayload))]
    [XmlInclude(typeof(CreateTimestampedValve2PulseDurationPayload))]
    [XmlInclude(typeof(CreateTimestampedValve3PulseDurationPayload))]
    [XmlInclude(typeof(CreateTimestampedCheckValve0DelayPulseDurationPayload))]
    [XmlInclude(typeof(CreateTimestampedCheckValve1DelayPulseDurationPayload))]
    [XmlInclude(typeof(CreateTimestampedCheckValve2DelayPulseDurationPayload))]
    [XmlInclude(typeof(CreateTimestampedCheckValve3DelayPulseDurationPayload))]
    [XmlInclude(typeof(CreateTimestampedEndValve0PulseDurationPayload))]
    [XmlInclude(typeof(CreateTimestampedEndValve1PulseDurationPayload))]
    [XmlInclude(typeof(CreateTimestampedDO0SyncPayload))]
    [XmlInclude(typeof(CreateTimestampedDO1SyncPayload))]
    [XmlInclude(typeof(CreateTimestampedDI0TriggerPayload))]
    [XmlInclude(typeof(CreateTimestampedMimicValve0Payload))]
    [XmlInclude(typeof(CreateTimestampedMimicValve1Payload))]
    [XmlInclude(typeof(CreateTimestampedMimicValve2Payload))]
    [XmlInclude(typeof(CreateTimestampedMimicValve3Payload))]
    [XmlInclude(typeof(CreateTimestampedMimicCheckValve0Payload))]
    [XmlInclude(typeof(CreateTimestampedMimicCheckValve1Payload))]
    [XmlInclude(typeof(CreateTimestampedMimicCheckValve2Payload))]
    [XmlInclude(typeof(CreateTimestampedMimicCheckValve3Payload))]
    [XmlInclude(typeof(CreateTimestampedMimicEndValve0Payload))]
    [XmlInclude(typeof(CreateTimestampedMimicEndValve1Payload))]
    [XmlInclude(typeof(CreateTimestampedEnableValveExternalControlPayload))]
    [XmlInclude(typeof(CreateTimestampedChannel3RangePayload))]
    [XmlInclude(typeof(CreateTimestampedEnableCheckValveSyncPayload))]
    [XmlInclude(typeof(CreateTimestampedTemperatureValuePayload))]
    [XmlInclude(typeof(CreateTimestampedEnableTemperatureCalibrationPayload))]
    [XmlInclude(typeof(CreateTimestampedTemperatureCalibrationValuePayload))]
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
    /// that starts or stops the flow in all channels.
    /// </summary>
    [DisplayName("EnableFlowPayload")]
    [Description("Creates a message payload that starts or stops the flow in all channels.")]
    public partial class CreateEnableFlowPayload
    {
        /// <summary>
        /// Gets or sets the value that starts or stops the flow in all channels.
        /// </summary>
        [Description("The value that starts or stops the flow in all channels.")]
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
        /// Creates a message that starts or stops the flow in all channels.
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
    /// that starts or stops the flow in all channels.
    /// </summary>
    [DisplayName("TimestampedEnableFlowPayload")]
    [Description("Creates a timestamped message payload that starts or stops the flow in all channels.")]
    public partial class CreateTimestampedEnableFlowPayload : CreateEnableFlowPayload
    {
        /// <summary>
        /// Creates a timestamped message that starts or stops the flow in all channels.
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
    /// that value of single ADC read from all flowmeter channels.
    /// </summary>
    [DisplayName("FlowmeterPayload")]
    [Description("Creates a message payload that value of single ADC read from all flowmeter channels.")]
    public partial class CreateFlowmeterPayload
    {
        /// <summary>
        /// Gets or sets a value to write on payload member Channel0.
        /// </summary>
        [Description("")]
        public short Channel0 { get; set; }

        /// <summary>
        /// Gets or sets a value to write on payload member Channel1.
        /// </summary>
        [Description("")]
        public short Channel1 { get; set; }

        /// <summary>
        /// Gets or sets a value to write on payload member Channel2.
        /// </summary>
        [Description("")]
        public short Channel2 { get; set; }

        /// <summary>
        /// Gets or sets a value to write on payload member Channel3.
        /// </summary>
        [Description("")]
        public short Channel3 { get; set; }

        /// <summary>
        /// Gets or sets a value to write on payload member Channel4.
        /// </summary>
        [Description("")]
        public short Channel4 { get; set; }

        /// <summary>
        /// Creates a message payload for the Flowmeter register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public FlowmeterPayload GetPayload()
        {
            FlowmeterPayload value;
            value.Channel0 = Channel0;
            value.Channel1 = Channel1;
            value.Channel2 = Channel2;
            value.Channel3 = Channel3;
            value.Channel4 = Channel4;
            return value;
        }

        /// <summary>
        /// Creates a message that value of single ADC read from all flowmeter channels.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the Flowmeter register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.Flowmeter.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that value of single ADC read from all flowmeter channels.
    /// </summary>
    [DisplayName("TimestampedFlowmeterPayload")]
    [Description("Creates a timestamped message payload that value of single ADC read from all flowmeter channels.")]
    public partial class CreateTimestampedFlowmeterPayload : CreateFlowmeterPayload
    {
        /// <summary>
        /// Creates a timestamped message that value of single ADC read from all flowmeter channels.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the Flowmeter register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.Flowmeter.FromPayload(timestamp, messageType, GetPayload());
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
    /// that calibration values for a single channel [x0,...xn], where x= ADC raw value for 0:10:100 ml/min.
    /// </summary>
    [DisplayName("Channel0UserCalibrationPayload")]
    [Description("Creates a message payload that calibration values for a single channel [x0,...xn], where x= ADC raw value for 0:10:100 ml/min.")]
    public partial class CreateChannel0UserCalibrationPayload
    {
        /// <summary>
        /// Gets or sets the value that calibration values for a single channel [x0,...xn], where x= ADC raw value for 0:10:100 ml/min.
        /// </summary>
        [Description("The value that calibration values for a single channel [x0,...xn], where x= ADC raw value for 0:10:100 ml/min.")]
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
        /// Creates a message that calibration values for a single channel [x0,...xn], where x= ADC raw value for 0:10:100 ml/min.
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
    /// that calibration values for a single channel [x0,...xn], where x= ADC raw value for 0:10:100 ml/min.
    /// </summary>
    [DisplayName("TimestampedChannel0UserCalibrationPayload")]
    [Description("Creates a timestamped message payload that calibration values for a single channel [x0,...xn], where x= ADC raw value for 0:10:100 ml/min.")]
    public partial class CreateTimestampedChannel0UserCalibrationPayload : CreateChannel0UserCalibrationPayload
    {
        /// <summary>
        /// Creates a timestamped message that calibration values for a single channel [x0,...xn], where x= ADC raw value for 0:10:100 ml/min.
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
    /// that calibration values for a single channel [x0,...xn], where x= ADC raw value for 0:10:100 ml/min.
    /// </summary>
    [DisplayName("Channel1UserCalibrationPayload")]
    [Description("Creates a message payload that calibration values for a single channel [x0,...xn], where x= ADC raw value for 0:10:100 ml/min.")]
    public partial class CreateChannel1UserCalibrationPayload
    {
        /// <summary>
        /// Gets or sets the value that calibration values for a single channel [x0,...xn], where x= ADC raw value for 0:10:100 ml/min.
        /// </summary>
        [Description("The value that calibration values for a single channel [x0,...xn], where x= ADC raw value for 0:10:100 ml/min.")]
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
        /// Creates a message that calibration values for a single channel [x0,...xn], where x= ADC raw value for 0:10:100 ml/min.
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
    /// that calibration values for a single channel [x0,...xn], where x= ADC raw value for 0:10:100 ml/min.
    /// </summary>
    [DisplayName("TimestampedChannel1UserCalibrationPayload")]
    [Description("Creates a timestamped message payload that calibration values for a single channel [x0,...xn], where x= ADC raw value for 0:10:100 ml/min.")]
    public partial class CreateTimestampedChannel1UserCalibrationPayload : CreateChannel1UserCalibrationPayload
    {
        /// <summary>
        /// Creates a timestamped message that calibration values for a single channel [x0,...xn], where x= ADC raw value for 0:10:100 ml/min.
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
    /// that calibration values for a single channel [x0,...xn], where x= ADC raw value for 0:10:100 ml/min.
    /// </summary>
    [DisplayName("Channel2UserCalibrationPayload")]
    [Description("Creates a message payload that calibration values for a single channel [x0,...xn], where x= ADC raw value for 0:10:100 ml/min.")]
    public partial class CreateChannel2UserCalibrationPayload
    {
        /// <summary>
        /// Gets or sets the value that calibration values for a single channel [x0,...xn], where x= ADC raw value for 0:10:100 ml/min.
        /// </summary>
        [Description("The value that calibration values for a single channel [x0,...xn], where x= ADC raw value for 0:10:100 ml/min.")]
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
        /// Creates a message that calibration values for a single channel [x0,...xn], where x= ADC raw value for 0:10:100 ml/min.
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
    /// that calibration values for a single channel [x0,...xn], where x= ADC raw value for 0:10:100 ml/min.
    /// </summary>
    [DisplayName("TimestampedChannel2UserCalibrationPayload")]
    [Description("Creates a timestamped message payload that calibration values for a single channel [x0,...xn], where x= ADC raw value for 0:10:100 ml/min.")]
    public partial class CreateTimestampedChannel2UserCalibrationPayload : CreateChannel2UserCalibrationPayload
    {
        /// <summary>
        /// Creates a timestamped message that calibration values for a single channel [x0,...xn], where x= ADC raw value for 0:10:100 ml/min.
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
    /// that calibration values for a single channel [x0,...xn], where x= ADC raw value for 0:10:100 ml/min.
    /// </summary>
    [DisplayName("Channel3UserCalibrationPayload")]
    [Description("Creates a message payload that calibration values for a single channel [x0,...xn], where x= ADC raw value for 0:10:100 ml/min.")]
    public partial class CreateChannel3UserCalibrationPayload
    {
        /// <summary>
        /// Gets or sets the value that calibration values for a single channel [x0,...xn], where x= ADC raw value for 0:10:100 ml/min.
        /// </summary>
        [Description("The value that calibration values for a single channel [x0,...xn], where x= ADC raw value for 0:10:100 ml/min.")]
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
        /// Creates a message that calibration values for a single channel [x0,...xn], where x= ADC raw value for 0:10:100 ml/min.
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
    /// that calibration values for a single channel [x0,...xn], where x= ADC raw value for 0:10:100 ml/min.
    /// </summary>
    [DisplayName("TimestampedChannel3UserCalibrationPayload")]
    [Description("Creates a timestamped message payload that calibration values for a single channel [x0,...xn], where x= ADC raw value for 0:10:100 ml/min.")]
    public partial class CreateTimestampedChannel3UserCalibrationPayload : CreateChannel3UserCalibrationPayload
    {
        /// <summary>
        /// Creates a timestamped message that calibration values for a single channel [x0,...xn], where x= ADC raw value for 0:10:100 ml/min.
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
    /// that calibration values specific for channel 4 [x0,...xn], where x= ADC raw value for 0:100:1000 ml/min.
    /// </summary>
    [DisplayName("Channel4UserCalibrationPayload")]
    [Description("Creates a message payload that calibration values specific for channel 4 [x0,...xn], where x= ADC raw value for 0:100:1000 ml/min.")]
    public partial class CreateChannel4UserCalibrationPayload
    {
        /// <summary>
        /// Gets or sets the value that calibration values specific for channel 4 [x0,...xn], where x= ADC raw value for 0:100:1000 ml/min.
        /// </summary>
        [Description("The value that calibration values specific for channel 4 [x0,...xn], where x= ADC raw value for 0:100:1000 ml/min.")]
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
        /// Creates a message that calibration values specific for channel 4 [x0,...xn], where x= ADC raw value for 0:100:1000 ml/min.
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
    /// that calibration values specific for channel 4 [x0,...xn], where x= ADC raw value for 0:100:1000 ml/min.
    /// </summary>
    [DisplayName("TimestampedChannel4UserCalibrationPayload")]
    [Description("Creates a timestamped message payload that calibration values specific for channel 4 [x0,...xn], where x= ADC raw value for 0:100:1000 ml/min.")]
    public partial class CreateTimestampedChannel4UserCalibrationPayload : CreateChannel4UserCalibrationPayload
    {
        /// <summary>
        /// Creates a timestamped message that calibration values specific for channel 4 [x0,...xn], where x= ADC raw value for 0:100:1000 ml/min.
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
    /// that calibration values specific for channel 3 if Channel3RangeConfig = FlowRate1000. [x0,...xn], where x= ADC raw value for 0:100:1000 ml/min.
    /// </summary>
    [DisplayName("Channel3UserCalibrationAuxPayload")]
    [Description("Creates a message payload that calibration values specific for channel 3 if Channel3RangeConfig = FlowRate1000. [x0,...xn], where x= ADC raw value for 0:100:1000 ml/min.")]
    public partial class CreateChannel3UserCalibrationAuxPayload
    {
        /// <summary>
        /// Gets or sets the value that calibration values specific for channel 3 if Channel3RangeConfig = FlowRate1000. [x0,...xn], where x= ADC raw value for 0:100:1000 ml/min.
        /// </summary>
        [Description("The value that calibration values specific for channel 3 if Channel3RangeConfig = FlowRate1000. [x0,...xn], where x= ADC raw value for 0:100:1000 ml/min.")]
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
        /// Creates a message that calibration values specific for channel 3 if Channel3RangeConfig = FlowRate1000. [x0,...xn], where x= ADC raw value for 0:100:1000 ml/min.
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
    /// that calibration values specific for channel 3 if Channel3RangeConfig = FlowRate1000. [x0,...xn], where x= ADC raw value for 0:100:1000 ml/min.
    /// </summary>
    [DisplayName("TimestampedChannel3UserCalibrationAuxPayload")]
    [Description("Creates a timestamped message payload that calibration values specific for channel 3 if Channel3RangeConfig = FlowRate1000. [x0,...xn], where x= ADC raw value for 0:100:1000 ml/min.")]
    public partial class CreateTimestampedChannel3UserCalibrationAuxPayload : CreateChannel3UserCalibrationAuxPayload
    {
        /// <summary>
        /// Creates a timestamped message that calibration values specific for channel 3 if Channel3RangeConfig = FlowRate1000. [x0,...xn], where x= ADC raw value for 0:100:1000 ml/min.
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
        public EnableFlag UserCalibrationEnable { get; set; }

        /// <summary>
        /// Creates a message payload for the UserCalibrationEnable register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public EnableFlag GetPayload()
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
    /// that sets the flow-rate rate for channel 0 [ml/min].
    /// </summary>
    [DisplayName("Channel0TargetFlowPayload")]
    [Description("Creates a message payload that sets the flow-rate rate for channel 0 [ml/min].")]
    public partial class CreateChannel0TargetFlowPayload
    {
        /// <summary>
        /// Gets or sets the value that sets the flow-rate rate for channel 0 [ml/min].
        /// </summary>
        [Range(min: 0, max: 100)]
        [Editor(DesignTypes.NumericUpDownEditor, DesignTypes.UITypeEditor)]
        [Description("The value that sets the flow-rate rate for channel 0 [ml/min].")]
        public float Channel0TargetFlow { get; set; } = 0F;

        /// <summary>
        /// Creates a message payload for the Channel0TargetFlow register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public float GetPayload()
        {
            return Channel0TargetFlow;
        }

        /// <summary>
        /// Creates a message that sets the flow-rate rate for channel 0 [ml/min].
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the Channel0TargetFlow register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.Channel0TargetFlow.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that sets the flow-rate rate for channel 0 [ml/min].
    /// </summary>
    [DisplayName("TimestampedChannel0TargetFlowPayload")]
    [Description("Creates a timestamped message payload that sets the flow-rate rate for channel 0 [ml/min].")]
    public partial class CreateTimestampedChannel0TargetFlowPayload : CreateChannel0TargetFlowPayload
    {
        /// <summary>
        /// Creates a timestamped message that sets the flow-rate rate for channel 0 [ml/min].
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the Channel0TargetFlow register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.Channel0TargetFlow.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that sets the flow-rate rate for channel 1 [ml/min].
    /// </summary>
    [DisplayName("Channel1TargetFlowPayload")]
    [Description("Creates a message payload that sets the flow-rate rate for channel 1 [ml/min].")]
    public partial class CreateChannel1TargetFlowPayload
    {
        /// <summary>
        /// Gets or sets the value that sets the flow-rate rate for channel 1 [ml/min].
        /// </summary>
        [Range(min: 0, max: 100)]
        [Editor(DesignTypes.NumericUpDownEditor, DesignTypes.UITypeEditor)]
        [Description("The value that sets the flow-rate rate for channel 1 [ml/min].")]
        public float Channel1TargetFlow { get; set; } = 0F;

        /// <summary>
        /// Creates a message payload for the Channel1TargetFlow register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public float GetPayload()
        {
            return Channel1TargetFlow;
        }

        /// <summary>
        /// Creates a message that sets the flow-rate rate for channel 1 [ml/min].
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the Channel1TargetFlow register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.Channel1TargetFlow.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that sets the flow-rate rate for channel 1 [ml/min].
    /// </summary>
    [DisplayName("TimestampedChannel1TargetFlowPayload")]
    [Description("Creates a timestamped message payload that sets the flow-rate rate for channel 1 [ml/min].")]
    public partial class CreateTimestampedChannel1TargetFlowPayload : CreateChannel1TargetFlowPayload
    {
        /// <summary>
        /// Creates a timestamped message that sets the flow-rate rate for channel 1 [ml/min].
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the Channel1TargetFlow register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.Channel1TargetFlow.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that sets the flow-rate rate for channel 2 [ml/min].
    /// </summary>
    [DisplayName("Channel2TargetFlowPayload")]
    [Description("Creates a message payload that sets the flow-rate rate for channel 2 [ml/min].")]
    public partial class CreateChannel2TargetFlowPayload
    {
        /// <summary>
        /// Gets or sets the value that sets the flow-rate rate for channel 2 [ml/min].
        /// </summary>
        [Range(min: 0, max: 100)]
        [Editor(DesignTypes.NumericUpDownEditor, DesignTypes.UITypeEditor)]
        [Description("The value that sets the flow-rate rate for channel 2 [ml/min].")]
        public float Channel2TargetFlow { get; set; } = 0F;

        /// <summary>
        /// Creates a message payload for the Channel2TargetFlow register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public float GetPayload()
        {
            return Channel2TargetFlow;
        }

        /// <summary>
        /// Creates a message that sets the flow-rate rate for channel 2 [ml/min].
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the Channel2TargetFlow register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.Channel2TargetFlow.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that sets the flow-rate rate for channel 2 [ml/min].
    /// </summary>
    [DisplayName("TimestampedChannel2TargetFlowPayload")]
    [Description("Creates a timestamped message payload that sets the flow-rate rate for channel 2 [ml/min].")]
    public partial class CreateTimestampedChannel2TargetFlowPayload : CreateChannel2TargetFlowPayload
    {
        /// <summary>
        /// Creates a timestamped message that sets the flow-rate rate for channel 2 [ml/min].
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the Channel2TargetFlow register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.Channel2TargetFlow.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that sets the flow-rate rate for channel 3 [ml/min].
    /// </summary>
    [DisplayName("Channel3TargetFlowPayload")]
    [Description("Creates a message payload that sets the flow-rate rate for channel 3 [ml/min].")]
    public partial class CreateChannel3TargetFlowPayload
    {
        /// <summary>
        /// Gets or sets the value that sets the flow-rate rate for channel 3 [ml/min].
        /// </summary>
        [Range(min: 0, max: 1000)]
        [Editor(DesignTypes.NumericUpDownEditor, DesignTypes.UITypeEditor)]
        [Description("The value that sets the flow-rate rate for channel 3 [ml/min].")]
        public float Channel3TargetFlow { get; set; } = 0F;

        /// <summary>
        /// Creates a message payload for the Channel3TargetFlow register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public float GetPayload()
        {
            return Channel3TargetFlow;
        }

        /// <summary>
        /// Creates a message that sets the flow-rate rate for channel 3 [ml/min].
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the Channel3TargetFlow register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.Channel3TargetFlow.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that sets the flow-rate rate for channel 3 [ml/min].
    /// </summary>
    [DisplayName("TimestampedChannel3TargetFlowPayload")]
    [Description("Creates a timestamped message payload that sets the flow-rate rate for channel 3 [ml/min].")]
    public partial class CreateTimestampedChannel3TargetFlowPayload : CreateChannel3TargetFlowPayload
    {
        /// <summary>
        /// Creates a timestamped message that sets the flow-rate rate for channel 3 [ml/min].
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the Channel3TargetFlow register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.Channel3TargetFlow.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that sets the flow-rate rate for channel 4 [ml/min].
    /// </summary>
    [DisplayName("Channel4TargetFlowPayload")]
    [Description("Creates a message payload that sets the flow-rate rate for channel 4 [ml/min].")]
    public partial class CreateChannel4TargetFlowPayload
    {
        /// <summary>
        /// Gets or sets the value that sets the flow-rate rate for channel 4 [ml/min].
        /// </summary>
        [Range(min: 0, max: 1000)]
        [Editor(DesignTypes.NumericUpDownEditor, DesignTypes.UITypeEditor)]
        [Description("The value that sets the flow-rate rate for channel 4 [ml/min].")]
        public float Channel4TargetFlow { get; set; } = 0F;

        /// <summary>
        /// Creates a message payload for the Channel4TargetFlow register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public float GetPayload()
        {
            return Channel4TargetFlow;
        }

        /// <summary>
        /// Creates a message that sets the flow-rate rate for channel 4 [ml/min].
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the Channel4TargetFlow register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.Channel4TargetFlow.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that sets the flow-rate rate for channel 4 [ml/min].
    /// </summary>
    [DisplayName("TimestampedChannel4TargetFlowPayload")]
    [Description("Creates a timestamped message payload that sets the flow-rate rate for channel 4 [ml/min].")]
    public partial class CreateTimestampedChannel4TargetFlowPayload : CreateChannel4TargetFlowPayload
    {
        /// <summary>
        /// Creates a timestamped message that sets the flow-rate rate for channel 4 [ml/min].
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the Channel4TargetFlow register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.Channel4TargetFlow.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that sets the flow-rate rate for all channels [ml/min].
    /// </summary>
    [DisplayName("ChannelsTargetFlowPayload")]
    [Description("Creates a message payload that sets the flow-rate rate for all channels [ml/min].")]
    public partial class CreateChannelsTargetFlowPayload
    {
        /// <summary>
        /// Gets or sets a value to write on payload member Channel0.
        /// </summary>
        [Description("")]
        public float Channel0 { get; set; }

        /// <summary>
        /// Gets or sets a value to write on payload member Channel1.
        /// </summary>
        [Description("")]
        public float Channel1 { get; set; }

        /// <summary>
        /// Gets or sets a value to write on payload member Channel2.
        /// </summary>
        [Description("")]
        public float Channel2 { get; set; }

        /// <summary>
        /// Gets or sets a value to write on payload member Channel3.
        /// </summary>
        [Description("")]
        public float Channel3 { get; set; }

        /// <summary>
        /// Gets or sets a value to write on payload member Channel4.
        /// </summary>
        [Description("")]
        public float Channel4 { get; set; }

        /// <summary>
        /// Creates a message payload for the ChannelsTargetFlow register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public ChannelsTargetFlowPayload GetPayload()
        {
            ChannelsTargetFlowPayload value;
            value.Channel0 = Channel0;
            value.Channel1 = Channel1;
            value.Channel2 = Channel2;
            value.Channel3 = Channel3;
            value.Channel4 = Channel4;
            return value;
        }

        /// <summary>
        /// Creates a message that sets the flow-rate rate for all channels [ml/min].
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the ChannelsTargetFlow register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.ChannelsTargetFlow.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that sets the flow-rate rate for all channels [ml/min].
    /// </summary>
    [DisplayName("TimestampedChannelsTargetFlowPayload")]
    [Description("Creates a timestamped message payload that sets the flow-rate rate for all channels [ml/min].")]
    public partial class CreateTimestampedChannelsTargetFlowPayload : CreateChannelsTargetFlowPayload
    {
        /// <summary>
        /// Creates a timestamped message that sets the flow-rate rate for all channels [ml/min].
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the ChannelsTargetFlow register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.ChannelsTargetFlow.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that actual flow-rate read for channel 0 - flowmeter 0 [ml/min].
    /// </summary>
    [DisplayName("Channel0ActualFlowPayload")]
    [Description("Creates a message payload that actual flow-rate read for channel 0 - flowmeter 0 [ml/min].")]
    public partial class CreateChannel0ActualFlowPayload
    {
        /// <summary>
        /// Gets or sets the value that actual flow-rate read for channel 0 - flowmeter 0 [ml/min].
        /// </summary>
        [Description("The value that actual flow-rate read for channel 0 - flowmeter 0 [ml/min].")]
        public float Channel0ActualFlow { get; set; }

        /// <summary>
        /// Creates a message payload for the Channel0ActualFlow register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public float GetPayload()
        {
            return Channel0ActualFlow;
        }

        /// <summary>
        /// Creates a message that actual flow-rate read for channel 0 - flowmeter 0 [ml/min].
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the Channel0ActualFlow register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.Channel0ActualFlow.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that actual flow-rate read for channel 0 - flowmeter 0 [ml/min].
    /// </summary>
    [DisplayName("TimestampedChannel0ActualFlowPayload")]
    [Description("Creates a timestamped message payload that actual flow-rate read for channel 0 - flowmeter 0 [ml/min].")]
    public partial class CreateTimestampedChannel0ActualFlowPayload : CreateChannel0ActualFlowPayload
    {
        /// <summary>
        /// Creates a timestamped message that actual flow-rate read for channel 0 - flowmeter 0 [ml/min].
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the Channel0ActualFlow register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.Channel0ActualFlow.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that actual flow-rate read for channel 1 - flowmeter 1 [ml/min].
    /// </summary>
    [DisplayName("Channel1ActualFlowPayload")]
    [Description("Creates a message payload that actual flow-rate read for channel 1 - flowmeter 1 [ml/min].")]
    public partial class CreateChannel1ActualFlowPayload
    {
        /// <summary>
        /// Gets or sets the value that actual flow-rate read for channel 1 - flowmeter 1 [ml/min].
        /// </summary>
        [Description("The value that actual flow-rate read for channel 1 - flowmeter 1 [ml/min].")]
        public float Channel1ActualFlow { get; set; }

        /// <summary>
        /// Creates a message payload for the Channel1ActualFlow register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public float GetPayload()
        {
            return Channel1ActualFlow;
        }

        /// <summary>
        /// Creates a message that actual flow-rate read for channel 1 - flowmeter 1 [ml/min].
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the Channel1ActualFlow register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.Channel1ActualFlow.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that actual flow-rate read for channel 1 - flowmeter 1 [ml/min].
    /// </summary>
    [DisplayName("TimestampedChannel1ActualFlowPayload")]
    [Description("Creates a timestamped message payload that actual flow-rate read for channel 1 - flowmeter 1 [ml/min].")]
    public partial class CreateTimestampedChannel1ActualFlowPayload : CreateChannel1ActualFlowPayload
    {
        /// <summary>
        /// Creates a timestamped message that actual flow-rate read for channel 1 - flowmeter 1 [ml/min].
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the Channel1ActualFlow register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.Channel1ActualFlow.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that actual flow-rate read for channel 2 - flowmeter 2 [ml/min].
    /// </summary>
    [DisplayName("Channel2ActualFlowPayload")]
    [Description("Creates a message payload that actual flow-rate read for channel 2 - flowmeter 2 [ml/min].")]
    public partial class CreateChannel2ActualFlowPayload
    {
        /// <summary>
        /// Gets or sets the value that actual flow-rate read for channel 2 - flowmeter 2 [ml/min].
        /// </summary>
        [Description("The value that actual flow-rate read for channel 2 - flowmeter 2 [ml/min].")]
        public float Channel2ActualFlow { get; set; }

        /// <summary>
        /// Creates a message payload for the Channel2ActualFlow register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public float GetPayload()
        {
            return Channel2ActualFlow;
        }

        /// <summary>
        /// Creates a message that actual flow-rate read for channel 2 - flowmeter 2 [ml/min].
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the Channel2ActualFlow register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.Channel2ActualFlow.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that actual flow-rate read for channel 2 - flowmeter 2 [ml/min].
    /// </summary>
    [DisplayName("TimestampedChannel2ActualFlowPayload")]
    [Description("Creates a timestamped message payload that actual flow-rate read for channel 2 - flowmeter 2 [ml/min].")]
    public partial class CreateTimestampedChannel2ActualFlowPayload : CreateChannel2ActualFlowPayload
    {
        /// <summary>
        /// Creates a timestamped message that actual flow-rate read for channel 2 - flowmeter 2 [ml/min].
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the Channel2ActualFlow register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.Channel2ActualFlow.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that actual flow-rate read for channel 3 - flowmeter 3 [ml/min].
    /// </summary>
    [DisplayName("Channel3ActualFlowPayload")]
    [Description("Creates a message payload that actual flow-rate read for channel 3 - flowmeter 3 [ml/min].")]
    public partial class CreateChannel3ActualFlowPayload
    {
        /// <summary>
        /// Gets or sets the value that actual flow-rate read for channel 3 - flowmeter 3 [ml/min].
        /// </summary>
        [Description("The value that actual flow-rate read for channel 3 - flowmeter 3 [ml/min].")]
        public float Channel3ActualFlow { get; set; }

        /// <summary>
        /// Creates a message payload for the Channel3ActualFlow register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public float GetPayload()
        {
            return Channel3ActualFlow;
        }

        /// <summary>
        /// Creates a message that actual flow-rate read for channel 3 - flowmeter 3 [ml/min].
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the Channel3ActualFlow register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.Channel3ActualFlow.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that actual flow-rate read for channel 3 - flowmeter 3 [ml/min].
    /// </summary>
    [DisplayName("TimestampedChannel3ActualFlowPayload")]
    [Description("Creates a timestamped message payload that actual flow-rate read for channel 3 - flowmeter 3 [ml/min].")]
    public partial class CreateTimestampedChannel3ActualFlowPayload : CreateChannel3ActualFlowPayload
    {
        /// <summary>
        /// Creates a timestamped message that actual flow-rate read for channel 3 - flowmeter 3 [ml/min].
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the Channel3ActualFlow register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.Channel3ActualFlow.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that actual flow-rate read for channel 4 - flowmeter 4 [ml/min].
    /// </summary>
    [DisplayName("Channel4ActualFlowPayload")]
    [Description("Creates a message payload that actual flow-rate read for channel 4 - flowmeter 4 [ml/min].")]
    public partial class CreateChannel4ActualFlowPayload
    {
        /// <summary>
        /// Gets or sets the value that actual flow-rate read for channel 4 - flowmeter 4 [ml/min].
        /// </summary>
        [Description("The value that actual flow-rate read for channel 4 - flowmeter 4 [ml/min].")]
        public float Channel4ActualFlow { get; set; }

        /// <summary>
        /// Creates a message payload for the Channel4ActualFlow register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public float GetPayload()
        {
            return Channel4ActualFlow;
        }

        /// <summary>
        /// Creates a message that actual flow-rate read for channel 4 - flowmeter 4 [ml/min].
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the Channel4ActualFlow register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.Channel4ActualFlow.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that actual flow-rate read for channel 4 - flowmeter 4 [ml/min].
    /// </summary>
    [DisplayName("TimestampedChannel4ActualFlowPayload")]
    [Description("Creates a timestamped message payload that actual flow-rate read for channel 4 - flowmeter 4 [ml/min].")]
    public partial class CreateTimestampedChannel4ActualFlowPayload : CreateChannel4ActualFlowPayload
    {
        /// <summary>
        /// Creates a timestamped message that actual flow-rate read for channel 4 - flowmeter 4 [ml/min].
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the Channel4ActualFlow register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.Channel4ActualFlow.FromPayload(timestamp, messageType, GetPayload());
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
        [Range(min: 0.2, max: 99.9)]
        [Editor(DesignTypes.NumericUpDownEditor, DesignTypes.UITypeEditor)]
        [Description("The value that duty cycle for proportional valve 0 [%].")]
        public float Channel0DutyCycle { get; set; } = 0.2F;

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
        [Range(min: 0.2, max: 99.9)]
        [Editor(DesignTypes.NumericUpDownEditor, DesignTypes.UITypeEditor)]
        [Description("The value that duty cycle for proportional valve 1 [%].")]
        public float Channel1DutyCycle { get; set; } = 0.2F;

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
        [Range(min: 0.2, max: 99.9)]
        [Editor(DesignTypes.NumericUpDownEditor, DesignTypes.UITypeEditor)]
        [Description("The value that duty cycle for proportional valve 2 [%].")]
        public float Channel2DutyCycle { get; set; } = 0.2F;

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
        [Range(min: 0.2, max: 99.9)]
        [Editor(DesignTypes.NumericUpDownEditor, DesignTypes.UITypeEditor)]
        [Description("The value that duty cycle for proportional valve 3 [%].")]
        public float Channel3DutyCycle { get; set; } = 0.2F;

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
        [Range(min: 0.2, max: 99.9)]
        [Editor(DesignTypes.NumericUpDownEditor, DesignTypes.UITypeEditor)]
        [Description("The value that duty cycle for proportional valve 4 [%].")]
        public float Channel4DutyCycle { get; set; } = 0.2F;

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
    /// that set the specified digital output lines.
    /// </summary>
    [DisplayName("DigitalOutputSetPayload")]
    [Description("Creates a message payload that set the specified digital output lines.")]
    public partial class CreateDigitalOutputSetPayload
    {
        /// <summary>
        /// Gets or sets the value that set the specified digital output lines.
        /// </summary>
        [Description("The value that set the specified digital output lines.")]
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
        /// Creates a message that set the specified digital output lines.
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
    /// that set the specified digital output lines.
    /// </summary>
    [DisplayName("TimestampedDigitalOutputSetPayload")]
    [Description("Creates a timestamped message payload that set the specified digital output lines.")]
    public partial class CreateTimestampedDigitalOutputSetPayload : CreateDigitalOutputSetPayload
    {
        /// <summary>
        /// Creates a timestamped message that set the specified digital output lines.
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
    /// that clears the specified digital output lines.
    /// </summary>
    [DisplayName("DigitalOutputClearPayload")]
    [Description("Creates a message payload that clears the specified digital output lines.")]
    public partial class CreateDigitalOutputClearPayload
    {
        /// <summary>
        /// Gets or sets the value that clears the specified digital output lines.
        /// </summary>
        [Description("The value that clears the specified digital output lines.")]
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
        /// Creates a message that clears the specified digital output lines.
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
    /// that clears the specified digital output lines.
    /// </summary>
    [DisplayName("TimestampedDigitalOutputClearPayload")]
    [Description("Creates a timestamped message payload that clears the specified digital output lines.")]
    public partial class CreateTimestampedDigitalOutputClearPayload : CreateDigitalOutputClearPayload
    {
        /// <summary>
        /// Creates a timestamped message that clears the specified digital output lines.
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
    /// that toggles the specified digital output lines.
    /// </summary>
    [DisplayName("DigitalOutputTogglePayload")]
    [Description("Creates a message payload that toggles the specified digital output lines.")]
    public partial class CreateDigitalOutputTogglePayload
    {
        /// <summary>
        /// Gets or sets the value that toggles the specified digital output lines.
        /// </summary>
        [Description("The value that toggles the specified digital output lines.")]
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
        /// Creates a message that toggles the specified digital output lines.
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
    /// that toggles the specified digital output lines.
    /// </summary>
    [DisplayName("TimestampedDigitalOutputTogglePayload")]
    [Description("Creates a timestamped message payload that toggles the specified digital output lines.")]
    public partial class CreateTimestampedDigitalOutputTogglePayload : CreateDigitalOutputTogglePayload
    {
        /// <summary>
        /// Creates a timestamped message that toggles the specified digital output lines.
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
    /// that write the state of all digital output lines.
    /// </summary>
    [DisplayName("DigitalOutputStatePayload")]
    [Description("Creates a message payload that write the state of all digital output lines.")]
    public partial class CreateDigitalOutputStatePayload
    {
        /// <summary>
        /// Gets or sets the value that write the state of all digital output lines.
        /// </summary>
        [Description("The value that write the state of all digital output lines.")]
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
        /// Creates a message that write the state of all digital output lines.
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
    /// that write the state of all digital output lines.
    /// </summary>
    [DisplayName("TimestampedDigitalOutputStatePayload")]
    [Description("Creates a timestamped message payload that write the state of all digital output lines.")]
    public partial class CreateTimestampedDigitalOutputStatePayload : CreateDigitalOutputStatePayload
    {
        /// <summary>
        /// Creates a timestamped message that write the state of all digital output lines.
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
    [DisplayName("EnableValvePulsePayload")]
    [Description("Creates a message payload that enable pulse mode for valves.")]
    public partial class CreateEnableValvePulsePayload
    {
        /// <summary>
        /// Gets or sets the value that enable pulse mode for valves.
        /// </summary>
        [Description("The value that enable pulse mode for valves.")]
        public Valves EnableValvePulse { get; set; }

        /// <summary>
        /// Creates a message payload for the EnableValvePulse register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public Valves GetPayload()
        {
            return EnableValvePulse;
        }

        /// <summary>
        /// Creates a message that enable pulse mode for valves.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the EnableValvePulse register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.EnableValvePulse.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that enable pulse mode for valves.
    /// </summary>
    [DisplayName("TimestampedEnableValvePulsePayload")]
    [Description("Creates a timestamped message payload that enable pulse mode for valves.")]
    public partial class CreateTimestampedEnableValvePulsePayload : CreateEnableValvePulsePayload
    {
        /// <summary>
        /// Creates a timestamped message that enable pulse mode for valves.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the EnableValvePulse register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.EnableValvePulse.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that set the specified valve output lines.
    /// </summary>
    [DisplayName("ValveSetPayload")]
    [Description("Creates a message payload that set the specified valve output lines.")]
    public partial class CreateValveSetPayload
    {
        /// <summary>
        /// Gets or sets the value that set the specified valve output lines.
        /// </summary>
        [Description("The value that set the specified valve output lines.")]
        public Valves ValveSet { get; set; }

        /// <summary>
        /// Creates a message payload for the ValveSet register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public Valves GetPayload()
        {
            return ValveSet;
        }

        /// <summary>
        /// Creates a message that set the specified valve output lines.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the ValveSet register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.ValveSet.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that set the specified valve output lines.
    /// </summary>
    [DisplayName("TimestampedValveSetPayload")]
    [Description("Creates a timestamped message payload that set the specified valve output lines.")]
    public partial class CreateTimestampedValveSetPayload : CreateValveSetPayload
    {
        /// <summary>
        /// Creates a timestamped message that set the specified valve output lines.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the ValveSet register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.ValveSet.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that clears the specified valve output lines.
    /// </summary>
    [DisplayName("ValveClearPayload")]
    [Description("Creates a message payload that clears the specified valve output lines.")]
    public partial class CreateValveClearPayload
    {
        /// <summary>
        /// Gets or sets the value that clears the specified valve output lines.
        /// </summary>
        [Description("The value that clears the specified valve output lines.")]
        public Valves ValveClear { get; set; }

        /// <summary>
        /// Creates a message payload for the ValveClear register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public Valves GetPayload()
        {
            return ValveClear;
        }

        /// <summary>
        /// Creates a message that clears the specified valve output lines.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the ValveClear register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.ValveClear.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that clears the specified valve output lines.
    /// </summary>
    [DisplayName("TimestampedValveClearPayload")]
    [Description("Creates a timestamped message payload that clears the specified valve output lines.")]
    public partial class CreateTimestampedValveClearPayload : CreateValveClearPayload
    {
        /// <summary>
        /// Creates a timestamped message that clears the specified valve output lines.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the ValveClear register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.ValveClear.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that toggles the specified valve output lines.
    /// </summary>
    [DisplayName("ValveTogglePayload")]
    [Description("Creates a message payload that toggles the specified valve output lines.")]
    public partial class CreateValveTogglePayload
    {
        /// <summary>
        /// Gets or sets the value that toggles the specified valve output lines.
        /// </summary>
        [Description("The value that toggles the specified valve output lines.")]
        public Valves ValveToggle { get; set; }

        /// <summary>
        /// Creates a message payload for the ValveToggle register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public Valves GetPayload()
        {
            return ValveToggle;
        }

        /// <summary>
        /// Creates a message that toggles the specified valve output lines.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the ValveToggle register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.ValveToggle.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that toggles the specified valve output lines.
    /// </summary>
    [DisplayName("TimestampedValveTogglePayload")]
    [Description("Creates a timestamped message payload that toggles the specified valve output lines.")]
    public partial class CreateTimestampedValveTogglePayload : CreateValveTogglePayload
    {
        /// <summary>
        /// Creates a timestamped message that toggles the specified valve output lines.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the ValveToggle register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.ValveToggle.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that controls the specified valve output lines.
    /// </summary>
    [DisplayName("ValveStatePayload")]
    [Description("Creates a message payload that controls the specified valve output lines.")]
    public partial class CreateValveStatePayload
    {
        /// <summary>
        /// Gets or sets the value that controls the specified valve output lines.
        /// </summary>
        [Description("The value that controls the specified valve output lines.")]
        public Valves ValveState { get; set; }

        /// <summary>
        /// Creates a message payload for the ValveState register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public Valves GetPayload()
        {
            return ValveState;
        }

        /// <summary>
        /// Creates a message that controls the specified valve output lines.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the ValveState register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.ValveState.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that controls the specified valve output lines.
    /// </summary>
    [DisplayName("TimestampedValveStatePayload")]
    [Description("Creates a timestamped message payload that controls the specified valve output lines.")]
    public partial class CreateTimestampedValveStatePayload : CreateValveStatePayload
    {
        /// <summary>
        /// Creates a timestamped message that controls the specified valve output lines.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the ValveState register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.ValveState.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that write the state of all odor valve output lines.
    /// </summary>
    [DisplayName("OdorValveStatePayload")]
    [Description("Creates a message payload that write the state of all odor valve output lines.")]
    public partial class CreateOdorValveStatePayload
    {
        /// <summary>
        /// Gets or sets the value that write the state of all odor valve output lines.
        /// </summary>
        [Description("The value that write the state of all odor valve output lines.")]
        public OdorValves OdorValveState { get; set; }

        /// <summary>
        /// Creates a message payload for the OdorValveState register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public OdorValves GetPayload()
        {
            return OdorValveState;
        }

        /// <summary>
        /// Creates a message that write the state of all odor valve output lines.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the OdorValveState register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.OdorValveState.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that write the state of all odor valve output lines.
    /// </summary>
    [DisplayName("TimestampedOdorValveStatePayload")]
    [Description("Creates a timestamped message payload that write the state of all odor valve output lines.")]
    public partial class CreateTimestampedOdorValveStatePayload : CreateOdorValveStatePayload
    {
        /// <summary>
        /// Creates a timestamped message that write the state of all odor valve output lines.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the OdorValveState register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.OdorValveState.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that write the state of all end valve output lines.
    /// </summary>
    [DisplayName("EndValveStatePayload")]
    [Description("Creates a message payload that write the state of all end valve output lines.")]
    public partial class CreateEndValveStatePayload
    {
        /// <summary>
        /// Gets or sets the value that write the state of all end valve output lines.
        /// </summary>
        [Description("The value that write the state of all end valve output lines.")]
        public EndValves EndValveState { get; set; }

        /// <summary>
        /// Creates a message payload for the EndValveState register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public EndValves GetPayload()
        {
            return EndValveState;
        }

        /// <summary>
        /// Creates a message that write the state of all end valve output lines.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the EndValveState register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.EndValveState.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that write the state of all end valve output lines.
    /// </summary>
    [DisplayName("TimestampedEndValveStatePayload")]
    [Description("Creates a timestamped message payload that write the state of all end valve output lines.")]
    public partial class CreateTimestampedEndValveStatePayload : CreateEndValveStatePayload
    {
        /// <summary>
        /// Creates a timestamped message that write the state of all end valve output lines.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the EndValveState register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.EndValveState.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that write the state of all check valve output lines.
    /// </summary>
    [DisplayName("CheckValveStatePayload")]
    [Description("Creates a message payload that write the state of all check valve output lines.")]
    public partial class CreateCheckValveStatePayload
    {
        /// <summary>
        /// Gets or sets the value that write the state of all check valve output lines.
        /// </summary>
        [Description("The value that write the state of all check valve output lines.")]
        public CheckValves CheckValveState { get; set; }

        /// <summary>
        /// Creates a message payload for the CheckValveState register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public CheckValves GetPayload()
        {
            return CheckValveState;
        }

        /// <summary>
        /// Creates a message that write the state of all check valve output lines.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the CheckValveState register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.CheckValveState.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that write the state of all check valve output lines.
    /// </summary>
    [DisplayName("TimestampedCheckValveStatePayload")]
    [Description("Creates a timestamped message payload that write the state of all check valve output lines.")]
    public partial class CreateTimestampedCheckValveStatePayload : CreateCheckValveStatePayload
    {
        /// <summary>
        /// Creates a timestamped message that write the state of all check valve output lines.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the CheckValveState register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.CheckValveState.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that sets the pulse duration for Valve0.
    /// </summary>
    [DisplayName("Valve0PulseDurationPayload")]
    [Description("Creates a message payload that sets the pulse duration for Valve0.")]
    public partial class CreateValve0PulseDurationPayload
    {
        /// <summary>
        /// Gets or sets the value that sets the pulse duration for Valve0.
        /// </summary>
        [Range(min: 1, max: 65535)]
        [Editor(DesignTypes.NumericUpDownEditor, DesignTypes.UITypeEditor)]
        [Description("The value that sets the pulse duration for Valve0.")]
        public ushort Valve0PulseDuration { get; set; } = 1;

        /// <summary>
        /// Creates a message payload for the Valve0PulseDuration register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public ushort GetPayload()
        {
            return Valve0PulseDuration;
        }

        /// <summary>
        /// Creates a message that sets the pulse duration for Valve0.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the Valve0PulseDuration register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.Valve0PulseDuration.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that sets the pulse duration for Valve0.
    /// </summary>
    [DisplayName("TimestampedValve0PulseDurationPayload")]
    [Description("Creates a timestamped message payload that sets the pulse duration for Valve0.")]
    public partial class CreateTimestampedValve0PulseDurationPayload : CreateValve0PulseDurationPayload
    {
        /// <summary>
        /// Creates a timestamped message that sets the pulse duration for Valve0.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the Valve0PulseDuration register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.Valve0PulseDuration.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that sets the pulse duration for Valve1.
    /// </summary>
    [DisplayName("Valve1PulseDurationPayload")]
    [Description("Creates a message payload that sets the pulse duration for Valve1.")]
    public partial class CreateValve1PulseDurationPayload
    {
        /// <summary>
        /// Gets or sets the value that sets the pulse duration for Valve1.
        /// </summary>
        [Range(min: 1, max: 65535)]
        [Editor(DesignTypes.NumericUpDownEditor, DesignTypes.UITypeEditor)]
        [Description("The value that sets the pulse duration for Valve1.")]
        public ushort Valve1PulseDuration { get; set; } = 1;

        /// <summary>
        /// Creates a message payload for the Valve1PulseDuration register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public ushort GetPayload()
        {
            return Valve1PulseDuration;
        }

        /// <summary>
        /// Creates a message that sets the pulse duration for Valve1.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the Valve1PulseDuration register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.Valve1PulseDuration.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that sets the pulse duration for Valve1.
    /// </summary>
    [DisplayName("TimestampedValve1PulseDurationPayload")]
    [Description("Creates a timestamped message payload that sets the pulse duration for Valve1.")]
    public partial class CreateTimestampedValve1PulseDurationPayload : CreateValve1PulseDurationPayload
    {
        /// <summary>
        /// Creates a timestamped message that sets the pulse duration for Valve1.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the Valve1PulseDuration register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.Valve1PulseDuration.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that sets the pulse duration for Valve2.
    /// </summary>
    [DisplayName("Valve2PulseDurationPayload")]
    [Description("Creates a message payload that sets the pulse duration for Valve2.")]
    public partial class CreateValve2PulseDurationPayload
    {
        /// <summary>
        /// Gets or sets the value that sets the pulse duration for Valve2.
        /// </summary>
        [Range(min: 1, max: 65535)]
        [Editor(DesignTypes.NumericUpDownEditor, DesignTypes.UITypeEditor)]
        [Description("The value that sets the pulse duration for Valve2.")]
        public ushort Valve2PulseDuration { get; set; } = 1;

        /// <summary>
        /// Creates a message payload for the Valve2PulseDuration register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public ushort GetPayload()
        {
            return Valve2PulseDuration;
        }

        /// <summary>
        /// Creates a message that sets the pulse duration for Valve2.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the Valve2PulseDuration register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.Valve2PulseDuration.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that sets the pulse duration for Valve2.
    /// </summary>
    [DisplayName("TimestampedValve2PulseDurationPayload")]
    [Description("Creates a timestamped message payload that sets the pulse duration for Valve2.")]
    public partial class CreateTimestampedValve2PulseDurationPayload : CreateValve2PulseDurationPayload
    {
        /// <summary>
        /// Creates a timestamped message that sets the pulse duration for Valve2.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the Valve2PulseDuration register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.Valve2PulseDuration.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that sets the pulse duration for Valve3.
    /// </summary>
    [DisplayName("Valve3PulseDurationPayload")]
    [Description("Creates a message payload that sets the pulse duration for Valve3.")]
    public partial class CreateValve3PulseDurationPayload
    {
        /// <summary>
        /// Gets or sets the value that sets the pulse duration for Valve3.
        /// </summary>
        [Range(min: 1, max: 65535)]
        [Editor(DesignTypes.NumericUpDownEditor, DesignTypes.UITypeEditor)]
        [Description("The value that sets the pulse duration for Valve3.")]
        public ushort Valve3PulseDuration { get; set; } = 1;

        /// <summary>
        /// Creates a message payload for the Valve3PulseDuration register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public ushort GetPayload()
        {
            return Valve3PulseDuration;
        }

        /// <summary>
        /// Creates a message that sets the pulse duration for Valve3.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the Valve3PulseDuration register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.Valve3PulseDuration.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that sets the pulse duration for Valve3.
    /// </summary>
    [DisplayName("TimestampedValve3PulseDurationPayload")]
    [Description("Creates a timestamped message payload that sets the pulse duration for Valve3.")]
    public partial class CreateTimestampedValve3PulseDurationPayload : CreateValve3PulseDurationPayload
    {
        /// <summary>
        /// Creates a timestamped message that sets the pulse duration for Valve3.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the Valve3PulseDuration register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.Valve3PulseDuration.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that sets the delay when CheckValvesConfig is Sync. Otherwise, sets the pulse duration for Check Valve0.
    /// </summary>
    [DisplayName("CheckValve0DelayPulseDurationPayload")]
    [Description("Creates a message payload that sets the delay when CheckValvesConfig is Sync. Otherwise, sets the pulse duration for Check Valve0.")]
    public partial class CreateCheckValve0DelayPulseDurationPayload
    {
        /// <summary>
        /// Gets or sets the value that sets the delay when CheckValvesConfig is Sync. Otherwise, sets the pulse duration for Check Valve0.
        /// </summary>
        [Range(min: 0, max: 65535)]
        [Editor(DesignTypes.NumericUpDownEditor, DesignTypes.UITypeEditor)]
        [Description("The value that sets the delay when CheckValvesConfig is Sync. Otherwise, sets the pulse duration for Check Valve0.")]
        public ushort CheckValve0DelayPulseDuration { get; set; } = 0;

        /// <summary>
        /// Creates a message payload for the CheckValve0DelayPulseDuration register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public ushort GetPayload()
        {
            return CheckValve0DelayPulseDuration;
        }

        /// <summary>
        /// Creates a message that sets the delay when CheckValvesConfig is Sync. Otherwise, sets the pulse duration for Check Valve0.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the CheckValve0DelayPulseDuration register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.CheckValve0DelayPulseDuration.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that sets the delay when CheckValvesConfig is Sync. Otherwise, sets the pulse duration for Check Valve0.
    /// </summary>
    [DisplayName("TimestampedCheckValve0DelayPulseDurationPayload")]
    [Description("Creates a timestamped message payload that sets the delay when CheckValvesConfig is Sync. Otherwise, sets the pulse duration for Check Valve0.")]
    public partial class CreateTimestampedCheckValve0DelayPulseDurationPayload : CreateCheckValve0DelayPulseDurationPayload
    {
        /// <summary>
        /// Creates a timestamped message that sets the delay when CheckValvesConfig is Sync. Otherwise, sets the pulse duration for Check Valve0.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the CheckValve0DelayPulseDuration register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.CheckValve0DelayPulseDuration.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that sets the delay when CheckValvesConfig is Sync. Otherwise, sets the pulse duration for Check Valve1.
    /// </summary>
    [DisplayName("CheckValve1DelayPulseDurationPayload")]
    [Description("Creates a message payload that sets the delay when CheckValvesConfig is Sync. Otherwise, sets the pulse duration for Check Valve1.")]
    public partial class CreateCheckValve1DelayPulseDurationPayload
    {
        /// <summary>
        /// Gets or sets the value that sets the delay when CheckValvesConfig is Sync. Otherwise, sets the pulse duration for Check Valve1.
        /// </summary>
        [Range(min: 0, max: 65535)]
        [Editor(DesignTypes.NumericUpDownEditor, DesignTypes.UITypeEditor)]
        [Description("The value that sets the delay when CheckValvesConfig is Sync. Otherwise, sets the pulse duration for Check Valve1.")]
        public ushort CheckValve1DelayPulseDuration { get; set; } = 0;

        /// <summary>
        /// Creates a message payload for the CheckValve1DelayPulseDuration register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public ushort GetPayload()
        {
            return CheckValve1DelayPulseDuration;
        }

        /// <summary>
        /// Creates a message that sets the delay when CheckValvesConfig is Sync. Otherwise, sets the pulse duration for Check Valve1.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the CheckValve1DelayPulseDuration register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.CheckValve1DelayPulseDuration.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that sets the delay when CheckValvesConfig is Sync. Otherwise, sets the pulse duration for Check Valve1.
    /// </summary>
    [DisplayName("TimestampedCheckValve1DelayPulseDurationPayload")]
    [Description("Creates a timestamped message payload that sets the delay when CheckValvesConfig is Sync. Otherwise, sets the pulse duration for Check Valve1.")]
    public partial class CreateTimestampedCheckValve1DelayPulseDurationPayload : CreateCheckValve1DelayPulseDurationPayload
    {
        /// <summary>
        /// Creates a timestamped message that sets the delay when CheckValvesConfig is Sync. Otherwise, sets the pulse duration for Check Valve1.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the CheckValve1DelayPulseDuration register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.CheckValve1DelayPulseDuration.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that sets the delay when CheckValvesConfig is Sync. Otherwise, sets the pulse duration for Check Valve2.
    /// </summary>
    [DisplayName("CheckValve2DelayPulseDurationPayload")]
    [Description("Creates a message payload that sets the delay when CheckValvesConfig is Sync. Otherwise, sets the pulse duration for Check Valve2.")]
    public partial class CreateCheckValve2DelayPulseDurationPayload
    {
        /// <summary>
        /// Gets or sets the value that sets the delay when CheckValvesConfig is Sync. Otherwise, sets the pulse duration for Check Valve2.
        /// </summary>
        [Range(min: 0, max: 65535)]
        [Editor(DesignTypes.NumericUpDownEditor, DesignTypes.UITypeEditor)]
        [Description("The value that sets the delay when CheckValvesConfig is Sync. Otherwise, sets the pulse duration for Check Valve2.")]
        public ushort CheckValve2DelayPulseDuration { get; set; } = 0;

        /// <summary>
        /// Creates a message payload for the CheckValve2DelayPulseDuration register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public ushort GetPayload()
        {
            return CheckValve2DelayPulseDuration;
        }

        /// <summary>
        /// Creates a message that sets the delay when CheckValvesConfig is Sync. Otherwise, sets the pulse duration for Check Valve2.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the CheckValve2DelayPulseDuration register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.CheckValve2DelayPulseDuration.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that sets the delay when CheckValvesConfig is Sync. Otherwise, sets the pulse duration for Check Valve2.
    /// </summary>
    [DisplayName("TimestampedCheckValve2DelayPulseDurationPayload")]
    [Description("Creates a timestamped message payload that sets the delay when CheckValvesConfig is Sync. Otherwise, sets the pulse duration for Check Valve2.")]
    public partial class CreateTimestampedCheckValve2DelayPulseDurationPayload : CreateCheckValve2DelayPulseDurationPayload
    {
        /// <summary>
        /// Creates a timestamped message that sets the delay when CheckValvesConfig is Sync. Otherwise, sets the pulse duration for Check Valve2.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the CheckValve2DelayPulseDuration register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.CheckValve2DelayPulseDuration.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that sets the delay when CheckValvesConfig is Sync. Otherwise, sets the pulse duration for Check Valve3.
    /// </summary>
    [DisplayName("CheckValve3DelayPulseDurationPayload")]
    [Description("Creates a message payload that sets the delay when CheckValvesConfig is Sync. Otherwise, sets the pulse duration for Check Valve3.")]
    public partial class CreateCheckValve3DelayPulseDurationPayload
    {
        /// <summary>
        /// Gets or sets the value that sets the delay when CheckValvesConfig is Sync. Otherwise, sets the pulse duration for Check Valve3.
        /// </summary>
        [Range(min: 0, max: 65535)]
        [Editor(DesignTypes.NumericUpDownEditor, DesignTypes.UITypeEditor)]
        [Description("The value that sets the delay when CheckValvesConfig is Sync. Otherwise, sets the pulse duration for Check Valve3.")]
        public ushort CheckValve3DelayPulseDuration { get; set; } = 0;

        /// <summary>
        /// Creates a message payload for the CheckValve3DelayPulseDuration register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public ushort GetPayload()
        {
            return CheckValve3DelayPulseDuration;
        }

        /// <summary>
        /// Creates a message that sets the delay when CheckValvesConfig is Sync. Otherwise, sets the pulse duration for Check Valve3.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the CheckValve3DelayPulseDuration register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.CheckValve3DelayPulseDuration.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that sets the delay when CheckValvesConfig is Sync. Otherwise, sets the pulse duration for Check Valve3.
    /// </summary>
    [DisplayName("TimestampedCheckValve3DelayPulseDurationPayload")]
    [Description("Creates a timestamped message payload that sets the delay when CheckValvesConfig is Sync. Otherwise, sets the pulse duration for Check Valve3.")]
    public partial class CreateTimestampedCheckValve3DelayPulseDurationPayload : CreateCheckValve3DelayPulseDurationPayload
    {
        /// <summary>
        /// Creates a timestamped message that sets the delay when CheckValvesConfig is Sync. Otherwise, sets the pulse duration for Check Valve3.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the CheckValve3DelayPulseDuration register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.CheckValve3DelayPulseDuration.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that sets the pulse duration for EndValve0.
    /// </summary>
    [DisplayName("EndValve0PulseDurationPayload")]
    [Description("Creates a message payload that sets the pulse duration for EndValve0.")]
    public partial class CreateEndValve0PulseDurationPayload
    {
        /// <summary>
        /// Gets or sets the value that sets the pulse duration for EndValve0.
        /// </summary>
        [Range(min: 1, max: 65535)]
        [Editor(DesignTypes.NumericUpDownEditor, DesignTypes.UITypeEditor)]
        [Description("The value that sets the pulse duration for EndValve0.")]
        public ushort EndValve0PulseDuration { get; set; } = 1;

        /// <summary>
        /// Creates a message payload for the EndValve0PulseDuration register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public ushort GetPayload()
        {
            return EndValve0PulseDuration;
        }

        /// <summary>
        /// Creates a message that sets the pulse duration for EndValve0.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the EndValve0PulseDuration register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.EndValve0PulseDuration.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that sets the pulse duration for EndValve0.
    /// </summary>
    [DisplayName("TimestampedEndValve0PulseDurationPayload")]
    [Description("Creates a timestamped message payload that sets the pulse duration for EndValve0.")]
    public partial class CreateTimestampedEndValve0PulseDurationPayload : CreateEndValve0PulseDurationPayload
    {
        /// <summary>
        /// Creates a timestamped message that sets the pulse duration for EndValve0.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the EndValve0PulseDuration register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.EndValve0PulseDuration.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that sets the pulse duration for EndValve1.
    /// </summary>
    [DisplayName("EndValve1PulseDurationPayload")]
    [Description("Creates a message payload that sets the pulse duration for EndValve1.")]
    public partial class CreateEndValve1PulseDurationPayload
    {
        /// <summary>
        /// Gets or sets the value that sets the pulse duration for EndValve1.
        /// </summary>
        [Range(min: 1, max: 65535)]
        [Editor(DesignTypes.NumericUpDownEditor, DesignTypes.UITypeEditor)]
        [Description("The value that sets the pulse duration for EndValve1.")]
        public ushort EndValve1PulseDuration { get; set; } = 1;

        /// <summary>
        /// Creates a message payload for the EndValve1PulseDuration register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public ushort GetPayload()
        {
            return EndValve1PulseDuration;
        }

        /// <summary>
        /// Creates a message that sets the pulse duration for EndValve1.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the EndValve1PulseDuration register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.EndValve1PulseDuration.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that sets the pulse duration for EndValve1.
    /// </summary>
    [DisplayName("TimestampedEndValve1PulseDurationPayload")]
    [Description("Creates a timestamped message payload that sets the pulse duration for EndValve1.")]
    public partial class CreateTimestampedEndValve1PulseDurationPayload : CreateEndValve1PulseDurationPayload
    {
        /// <summary>
        /// Creates a timestamped message that sets the pulse duration for EndValve1.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the EndValve1PulseDuration register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.EndValve1PulseDuration.FromPayload(timestamp, messageType, GetPayload());
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
    /// that mimic Valve0.
    /// </summary>
    [DisplayName("MimicValve0Payload")]
    [Description("Creates a message payload that mimic Valve0.")]
    public partial class CreateMimicValve0Payload
    {
        /// <summary>
        /// Gets or sets the value that mimic Valve0.
        /// </summary>
        [Description("The value that mimic Valve0.")]
        public MimicOutputs MimicValve0 { get; set; }

        /// <summary>
        /// Creates a message payload for the MimicValve0 register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public MimicOutputs GetPayload()
        {
            return MimicValve0;
        }

        /// <summary>
        /// Creates a message that mimic Valve0.
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
    /// that mimic Valve0.
    /// </summary>
    [DisplayName("TimestampedMimicValve0Payload")]
    [Description("Creates a timestamped message payload that mimic Valve0.")]
    public partial class CreateTimestampedMimicValve0Payload : CreateMimicValve0Payload
    {
        /// <summary>
        /// Creates a timestamped message that mimic Valve0.
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
    /// that mimic Valve1.
    /// </summary>
    [DisplayName("MimicValve1Payload")]
    [Description("Creates a message payload that mimic Valve1.")]
    public partial class CreateMimicValve1Payload
    {
        /// <summary>
        /// Gets or sets the value that mimic Valve1.
        /// </summary>
        [Description("The value that mimic Valve1.")]
        public MimicOutputs MimicValve1 { get; set; }

        /// <summary>
        /// Creates a message payload for the MimicValve1 register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public MimicOutputs GetPayload()
        {
            return MimicValve1;
        }

        /// <summary>
        /// Creates a message that mimic Valve1.
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
    /// that mimic Valve1.
    /// </summary>
    [DisplayName("TimestampedMimicValve1Payload")]
    [Description("Creates a timestamped message payload that mimic Valve1.")]
    public partial class CreateTimestampedMimicValve1Payload : CreateMimicValve1Payload
    {
        /// <summary>
        /// Creates a timestamped message that mimic Valve1.
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
    /// that mimic Valve2.
    /// </summary>
    [DisplayName("MimicValve2Payload")]
    [Description("Creates a message payload that mimic Valve2.")]
    public partial class CreateMimicValve2Payload
    {
        /// <summary>
        /// Gets or sets the value that mimic Valve2.
        /// </summary>
        [Description("The value that mimic Valve2.")]
        public MimicOutputs MimicValve2 { get; set; }

        /// <summary>
        /// Creates a message payload for the MimicValve2 register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public MimicOutputs GetPayload()
        {
            return MimicValve2;
        }

        /// <summary>
        /// Creates a message that mimic Valve2.
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
    /// that mimic Valve2.
    /// </summary>
    [DisplayName("TimestampedMimicValve2Payload")]
    [Description("Creates a timestamped message payload that mimic Valve2.")]
    public partial class CreateTimestampedMimicValve2Payload : CreateMimicValve2Payload
    {
        /// <summary>
        /// Creates a timestamped message that mimic Valve2.
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
    /// that mimic Valve3.
    /// </summary>
    [DisplayName("MimicValve3Payload")]
    [Description("Creates a message payload that mimic Valve3.")]
    public partial class CreateMimicValve3Payload
    {
        /// <summary>
        /// Gets or sets the value that mimic Valve3.
        /// </summary>
        [Description("The value that mimic Valve3.")]
        public MimicOutputs MimicValve3 { get; set; }

        /// <summary>
        /// Creates a message payload for the MimicValve3 register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public MimicOutputs GetPayload()
        {
            return MimicValve3;
        }

        /// <summary>
        /// Creates a message that mimic Valve3.
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
    /// that mimic Valve3.
    /// </summary>
    [DisplayName("TimestampedMimicValve3Payload")]
    [Description("Creates a timestamped message payload that mimic Valve3.")]
    public partial class CreateTimestampedMimicValve3Payload : CreateMimicValve3Payload
    {
        /// <summary>
        /// Creates a timestamped message that mimic Valve3.
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
    /// that mimic Check Valve0.
    /// </summary>
    [DisplayName("MimicCheckValve0Payload")]
    [Description("Creates a message payload that mimic Check Valve0.")]
    public partial class CreateMimicCheckValve0Payload
    {
        /// <summary>
        /// Gets or sets the value that mimic Check Valve0.
        /// </summary>
        [Description("The value that mimic Check Valve0.")]
        public MimicOutputs MimicCheckValve0 { get; set; }

        /// <summary>
        /// Creates a message payload for the MimicCheckValve0 register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public MimicOutputs GetPayload()
        {
            return MimicCheckValve0;
        }

        /// <summary>
        /// Creates a message that mimic Check Valve0.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the MimicCheckValve0 register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.MimicCheckValve0.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that mimic Check Valve0.
    /// </summary>
    [DisplayName("TimestampedMimicCheckValve0Payload")]
    [Description("Creates a timestamped message payload that mimic Check Valve0.")]
    public partial class CreateTimestampedMimicCheckValve0Payload : CreateMimicCheckValve0Payload
    {
        /// <summary>
        /// Creates a timestamped message that mimic Check Valve0.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the MimicCheckValve0 register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.MimicCheckValve0.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that mimic Check Valve1.
    /// </summary>
    [DisplayName("MimicCheckValve1Payload")]
    [Description("Creates a message payload that mimic Check Valve1.")]
    public partial class CreateMimicCheckValve1Payload
    {
        /// <summary>
        /// Gets or sets the value that mimic Check Valve1.
        /// </summary>
        [Description("The value that mimic Check Valve1.")]
        public MimicOutputs MimicCheckValve1 { get; set; }

        /// <summary>
        /// Creates a message payload for the MimicCheckValve1 register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public MimicOutputs GetPayload()
        {
            return MimicCheckValve1;
        }

        /// <summary>
        /// Creates a message that mimic Check Valve1.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the MimicCheckValve1 register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.MimicCheckValve1.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that mimic Check Valve1.
    /// </summary>
    [DisplayName("TimestampedMimicCheckValve1Payload")]
    [Description("Creates a timestamped message payload that mimic Check Valve1.")]
    public partial class CreateTimestampedMimicCheckValve1Payload : CreateMimicCheckValve1Payload
    {
        /// <summary>
        /// Creates a timestamped message that mimic Check Valve1.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the MimicCheckValve1 register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.MimicCheckValve1.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that mimic Check Valve2.
    /// </summary>
    [DisplayName("MimicCheckValve2Payload")]
    [Description("Creates a message payload that mimic Check Valve2.")]
    public partial class CreateMimicCheckValve2Payload
    {
        /// <summary>
        /// Gets or sets the value that mimic Check Valve2.
        /// </summary>
        [Description("The value that mimic Check Valve2.")]
        public MimicOutputs MimicCheckValve2 { get; set; }

        /// <summary>
        /// Creates a message payload for the MimicCheckValve2 register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public MimicOutputs GetPayload()
        {
            return MimicCheckValve2;
        }

        /// <summary>
        /// Creates a message that mimic Check Valve2.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the MimicCheckValve2 register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.MimicCheckValve2.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that mimic Check Valve2.
    /// </summary>
    [DisplayName("TimestampedMimicCheckValve2Payload")]
    [Description("Creates a timestamped message payload that mimic Check Valve2.")]
    public partial class CreateTimestampedMimicCheckValve2Payload : CreateMimicCheckValve2Payload
    {
        /// <summary>
        /// Creates a timestamped message that mimic Check Valve2.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the MimicCheckValve2 register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.MimicCheckValve2.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that mimic Check Valve3.
    /// </summary>
    [DisplayName("MimicCheckValve3Payload")]
    [Description("Creates a message payload that mimic Check Valve3.")]
    public partial class CreateMimicCheckValve3Payload
    {
        /// <summary>
        /// Gets or sets the value that mimic Check Valve3.
        /// </summary>
        [Description("The value that mimic Check Valve3.")]
        public MimicOutputs MimicCheckValve3 { get; set; }

        /// <summary>
        /// Creates a message payload for the MimicCheckValve3 register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public MimicOutputs GetPayload()
        {
            return MimicCheckValve3;
        }

        /// <summary>
        /// Creates a message that mimic Check Valve3.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the MimicCheckValve3 register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.MimicCheckValve3.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that mimic Check Valve3.
    /// </summary>
    [DisplayName("TimestampedMimicCheckValve3Payload")]
    [Description("Creates a timestamped message payload that mimic Check Valve3.")]
    public partial class CreateTimestampedMimicCheckValve3Payload : CreateMimicCheckValve3Payload
    {
        /// <summary>
        /// Creates a timestamped message that mimic Check Valve3.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the MimicCheckValve3 register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.MimicCheckValve3.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that mimic EndValve0.
    /// </summary>
    [DisplayName("MimicEndValve0Payload")]
    [Description("Creates a message payload that mimic EndValve0.")]
    public partial class CreateMimicEndValve0Payload
    {
        /// <summary>
        /// Gets or sets the value that mimic EndValve0.
        /// </summary>
        [Description("The value that mimic EndValve0.")]
        public MimicOutputs MimicEndValve0 { get; set; }

        /// <summary>
        /// Creates a message payload for the MimicEndValve0 register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public MimicOutputs GetPayload()
        {
            return MimicEndValve0;
        }

        /// <summary>
        /// Creates a message that mimic EndValve0.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the MimicEndValve0 register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.MimicEndValve0.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that mimic EndValve0.
    /// </summary>
    [DisplayName("TimestampedMimicEndValve0Payload")]
    [Description("Creates a timestamped message payload that mimic EndValve0.")]
    public partial class CreateTimestampedMimicEndValve0Payload : CreateMimicEndValve0Payload
    {
        /// <summary>
        /// Creates a timestamped message that mimic EndValve0.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the MimicEndValve0 register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.MimicEndValve0.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that mimic EndValve1.
    /// </summary>
    [DisplayName("MimicEndValve1Payload")]
    [Description("Creates a message payload that mimic EndValve1.")]
    public partial class CreateMimicEndValve1Payload
    {
        /// <summary>
        /// Gets or sets the value that mimic EndValve1.
        /// </summary>
        [Description("The value that mimic EndValve1.")]
        public MimicOutputs MimicEndValve1 { get; set; }

        /// <summary>
        /// Creates a message payload for the MimicEndValve1 register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public MimicOutputs GetPayload()
        {
            return MimicEndValve1;
        }

        /// <summary>
        /// Creates a message that mimic EndValve1.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the MimicEndValve1 register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.MimicEndValve1.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that mimic EndValve1.
    /// </summary>
    [DisplayName("TimestampedMimicEndValve1Payload")]
    [Description("Creates a timestamped message payload that mimic EndValve1.")]
    public partial class CreateTimestampedMimicEndValve1Payload : CreateMimicEndValve1Payload
    {
        /// <summary>
        /// Creates a timestamped message that mimic EndValve1.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the MimicEndValve1 register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.MimicEndValve1.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that enable the valves control via low-level IO screw terminals.
    /// </summary>
    [DisplayName("EnableValveExternalControlPayload")]
    [Description("Creates a message payload that enable the valves control via low-level IO screw terminals.")]
    public partial class CreateEnableValveExternalControlPayload
    {
        /// <summary>
        /// Gets or sets the value that enable the valves control via low-level IO screw terminals.
        /// </summary>
        [Description("The value that enable the valves control via low-level IO screw terminals.")]
        public EnableFlag EnableValveExternalControl { get; set; }

        /// <summary>
        /// Creates a message payload for the EnableValveExternalControl register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public EnableFlag GetPayload()
        {
            return EnableValveExternalControl;
        }

        /// <summary>
        /// Creates a message that enable the valves control via low-level IO screw terminals.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the EnableValveExternalControl register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.EnableValveExternalControl.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that enable the valves control via low-level IO screw terminals.
    /// </summary>
    [DisplayName("TimestampedEnableValveExternalControlPayload")]
    [Description("Creates a timestamped message payload that enable the valves control via low-level IO screw terminals.")]
    public partial class CreateTimestampedEnableValveExternalControlPayload : CreateEnableValveExternalControlPayload
    {
        /// <summary>
        /// Creates a timestamped message that enable the valves control via low-level IO screw terminals.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the EnableValveExternalControl register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.EnableValveExternalControl.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that selects the flow range for the channel 3.
    /// </summary>
    [DisplayName("Channel3RangePayload")]
    [Description("Creates a message payload that selects the flow range for the channel 3.")]
    public partial class CreateChannel3RangePayload
    {
        /// <summary>
        /// Gets or sets the value that selects the flow range for the channel 3.
        /// </summary>
        [Description("The value that selects the flow range for the channel 3.")]
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
        /// Creates a message that selects the flow range for the channel 3.
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
    /// that selects the flow range for the channel 3.
    /// </summary>
    [DisplayName("TimestampedChannel3RangePayload")]
    [Description("Creates a timestamped message payload that selects the flow range for the channel 3.")]
    public partial class CreateTimestampedChannel3RangePayload : CreateChannel3RangePayload
    {
        /// <summary>
        /// Creates a timestamped message that selects the flow range for the channel 3.
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
    /// that enable the check valve to sync with the respective odor valve.
    /// </summary>
    [DisplayName("EnableCheckValveSyncPayload")]
    [Description("Creates a message payload that enable the check valve to sync with the respective odor valve.")]
    public partial class CreateEnableCheckValveSyncPayload
    {
        /// <summary>
        /// Gets or sets the value that enable the check valve to sync with the respective odor valve.
        /// </summary>
        [Description("The value that enable the check valve to sync with the respective odor valve.")]
        public CheckValves EnableCheckValveSync { get; set; }

        /// <summary>
        /// Creates a message payload for the EnableCheckValveSync register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public CheckValves GetPayload()
        {
            return EnableCheckValveSync;
        }

        /// <summary>
        /// Creates a message that enable the check valve to sync with the respective odor valve.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the EnableCheckValveSync register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.EnableCheckValveSync.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that enable the check valve to sync with the respective odor valve.
    /// </summary>
    [DisplayName("TimestampedEnableCheckValveSyncPayload")]
    [Description("Creates a timestamped message payload that enable the check valve to sync with the respective odor valve.")]
    public partial class CreateTimestampedEnableCheckValveSyncPayload : CreateEnableCheckValveSyncPayload
    {
        /// <summary>
        /// Creates a timestamped message that enable the check valve to sync with the respective odor valve.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the EnableCheckValveSync register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.EnableCheckValveSync.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that temperature sensor reading value.
    /// </summary>
    [DisplayName("TemperatureValuePayload")]
    [Description("Creates a message payload that temperature sensor reading value.")]
    public partial class CreateTemperatureValuePayload
    {
        /// <summary>
        /// Gets or sets the value that temperature sensor reading value.
        /// </summary>
        [Description("The value that temperature sensor reading value.")]
        public byte TemperatureValue { get; set; }

        /// <summary>
        /// Creates a message payload for the TemperatureValue register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public byte GetPayload()
        {
            return TemperatureValue;
        }

        /// <summary>
        /// Creates a message that temperature sensor reading value.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the TemperatureValue register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.TemperatureValue.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that temperature sensor reading value.
    /// </summary>
    [DisplayName("TimestampedTemperatureValuePayload")]
    [Description("Creates a timestamped message payload that temperature sensor reading value.")]
    public partial class CreateTimestampedTemperatureValuePayload : CreateTemperatureValuePayload
    {
        /// <summary>
        /// Creates a timestamped message that temperature sensor reading value.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the TemperatureValue register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.TemperatureValue.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that enable flow adjustment based on the temperature calibration.
    /// </summary>
    [DisplayName("EnableTemperatureCalibrationPayload")]
    [Description("Creates a message payload that enable flow adjustment based on the temperature calibration.")]
    public partial class CreateEnableTemperatureCalibrationPayload
    {
        /// <summary>
        /// Gets or sets the value that enable flow adjustment based on the temperature calibration.
        /// </summary>
        [Description("The value that enable flow adjustment based on the temperature calibration.")]
        public EnableFlag EnableTemperatureCalibration { get; set; }

        /// <summary>
        /// Creates a message payload for the EnableTemperatureCalibration register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public EnableFlag GetPayload()
        {
            return EnableTemperatureCalibration;
        }

        /// <summary>
        /// Creates a message that enable flow adjustment based on the temperature calibration.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the EnableTemperatureCalibration register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.EnableTemperatureCalibration.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that enable flow adjustment based on the temperature calibration.
    /// </summary>
    [DisplayName("TimestampedEnableTemperatureCalibrationPayload")]
    [Description("Creates a timestamped message payload that enable flow adjustment based on the temperature calibration.")]
    public partial class CreateTimestampedEnableTemperatureCalibrationPayload : CreateEnableTemperatureCalibrationPayload
    {
        /// <summary>
        /// Creates a timestamped message that enable flow adjustment based on the temperature calibration.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the EnableTemperatureCalibration register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.EnableTemperatureCalibration.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that temperature value measured during the device calibration.
    /// </summary>
    [DisplayName("TemperatureCalibrationValuePayload")]
    [Description("Creates a message payload that temperature value measured during the device calibration.")]
    public partial class CreateTemperatureCalibrationValuePayload
    {
        /// <summary>
        /// Gets or sets the value that temperature value measured during the device calibration.
        /// </summary>
        [Description("The value that temperature value measured during the device calibration.")]
        public byte TemperatureCalibrationValue { get; set; }

        /// <summary>
        /// Creates a message payload for the TemperatureCalibrationValue register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public byte GetPayload()
        {
            return TemperatureCalibrationValue;
        }

        /// <summary>
        /// Creates a message that temperature value measured during the device calibration.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the TemperatureCalibrationValue register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.Olfactometer.TemperatureCalibrationValue.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that temperature value measured during the device calibration.
    /// </summary>
    [DisplayName("TimestampedTemperatureCalibrationValuePayload")]
    [Description("Creates a timestamped message payload that temperature value measured during the device calibration.")]
    public partial class CreateTimestampedTemperatureCalibrationValuePayload : CreateTemperatureCalibrationValuePayload
    {
        /// <summary>
        /// Creates a timestamped message that temperature value measured during the device calibration.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the TemperatureCalibrationValue register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.Olfactometer.TemperatureCalibrationValue.FromPayload(timestamp, messageType, GetPayload());
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
    /// Represents the payload of the Flowmeter register.
    /// </summary>
    public struct FlowmeterPayload
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FlowmeterPayload"/> structure.
        /// </summary>
        /// <param name="channel0"></param>
        /// <param name="channel1"></param>
        /// <param name="channel2"></param>
        /// <param name="channel3"></param>
        /// <param name="channel4"></param>
        public FlowmeterPayload(
            short channel0,
            short channel1,
            short channel2,
            short channel3,
            short channel4)
        {
            Channel0 = channel0;
            Channel1 = channel1;
            Channel2 = channel2;
            Channel3 = channel3;
            Channel4 = channel4;
        }

        /// <summary>
        /// 
        /// </summary>
        public short Channel0;

        /// <summary>
        /// 
        /// </summary>
        public short Channel1;

        /// <summary>
        /// 
        /// </summary>
        public short Channel2;

        /// <summary>
        /// 
        /// </summary>
        public short Channel3;

        /// <summary>
        /// 
        /// </summary>
        public short Channel4;

        /// <summary>
        /// Returns a <see cref="string"/> that represents the payload of
        /// the Flowmeter register.
        /// </summary>
        /// <returns>
        /// A <see cref="string"/> that represents the payload of the
        /// Flowmeter register.
        /// </returns>
        public override string ToString()
        {
            return "FlowmeterPayload { " +
                "Channel0 = " + Channel0 + ", " +
                "Channel1 = " + Channel1 + ", " +
                "Channel2 = " + Channel2 + ", " +
                "Channel3 = " + Channel3 + ", " +
                "Channel4 = " + Channel4 + " " +
            "}";
        }
    }

    /// <summary>
    /// Represents the payload of the ChannelsTargetFlow register.
    /// </summary>
    public struct ChannelsTargetFlowPayload
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelsTargetFlowPayload"/> structure.
        /// </summary>
        /// <param name="channel0"></param>
        /// <param name="channel1"></param>
        /// <param name="channel2"></param>
        /// <param name="channel3"></param>
        /// <param name="channel4"></param>
        public ChannelsTargetFlowPayload(
            float channel0,
            float channel1,
            float channel2,
            float channel3,
            float channel4)
        {
            Channel0 = channel0;
            Channel1 = channel1;
            Channel2 = channel2;
            Channel3 = channel3;
            Channel4 = channel4;
        }

        /// <summary>
        /// 
        /// </summary>
        public float Channel0;

        /// <summary>
        /// 
        /// </summary>
        public float Channel1;

        /// <summary>
        /// 
        /// </summary>
        public float Channel2;

        /// <summary>
        /// 
        /// </summary>
        public float Channel3;

        /// <summary>
        /// 
        /// </summary>
        public float Channel4;

        /// <summary>
        /// Returns a <see cref="string"/> that represents the payload of
        /// the ChannelsTargetFlow register.
        /// </summary>
        /// <returns>
        /// A <see cref="string"/> that represents the payload of the
        /// ChannelsTargetFlow register.
        /// </returns>
        public override string ToString()
        {
            return "ChannelsTargetFlowPayload { " +
                "Channel0 = " + Channel0 + ", " +
                "Channel1 = " + Channel1 + ", " +
                "Channel2 = " + Channel2 + ", " +
                "Channel3 = " + Channel3 + ", " +
                "Channel4 = " + Channel4 + " " +
            "}";
        }
    }

    /// <summary>
    /// Specifies the state of the digital outputs.
    /// </summary>
    [Flags]
    public enum DigitalOutputs : byte
    {
        None = 0x0,
        DO0 = 0x1,
        DO1 = 0x2
    }

    /// <summary>
    /// Specifies the state of the valves.
    /// </summary>
    [Flags]
    public enum Valves : ushort
    {
        None = 0x0,
        Valve0 = 0x1,
        Valve1 = 0x2,
        Valve2 = 0x4,
        Valve3 = 0x8,
        EndValve0 = 0x10,
        EndValve1 = 0x20,
        ValveDummy = 0x40,
        CheckValve0 = 0x100,
        CheckValve1 = 0x200,
        CheckValve2 = 0x400,
        CheckValve3 = 0x800
    }

    /// <summary>
    /// Specifies the state of the odor valves.
    /// </summary>
    [Flags]
    public enum OdorValves : byte
    {
        None = 0x0,
        Valve0 = 0x1,
        Valve1 = 0x2,
        Valve2 = 0x4,
        Valve3 = 0x8
    }

    /// <summary>
    /// Specifies the state of the check valves.
    /// </summary>
    [Flags]
    public enum CheckValves : ushort
    {
        None = 0x0,
        CheckValve0 = 0x100,
        CheckValve1 = 0x200,
        CheckValve2 = 0x400,
        CheckValve3 = 0x800
    }

    /// <summary>
    /// Specifies the state of the end valves.
    /// </summary>
    [Flags]
    public enum EndValves : byte
    {
        None = 0x0,
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
        Flowmeter = 0x1,
        DI0Trigger = 0x2,
        ChannelActualFlow = 0x4
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
        EnableFlowWhileHigh = 1,
        ValveToggle = 2
    }

    /// <summary>
    /// Specifies the target IO on which to mimic the specified register.
    /// </summary>
    public enum MimicOutputs : byte
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
        FlowRate100 = 0,
        FlowRate1000 = 1
    }
}
