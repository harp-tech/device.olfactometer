using Bonsai;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using Bonsai.Harp;
using System.Collections.Generic;


namespace Harp.Olfactometer
{
    /// <summary>
    /// Represents an operator that generates a sequence of Harp messages to
    /// configure the odor mixture in the olfactometer as corresponding odor
    /// valves.
    /// The configuration will set the target flow for each channel, assuming
    /// a constant target full flow, target odor flow and percentage of each odor.
    /// </summary>
    [Description("Generates a sequence of Harp messages to configure an odor mixture.")]

    public class ConfigureOdorMix : Source<HarpMessage>
    {
        /// <summary>
        /// Gets or sets the concentration of Channel0.
        /// </summary>
        [Range(0, 1)]
        [Editor(DesignTypes.SliderEditor, DesignTypes.UITypeEditor)]
        [Description("Odor dilution percentage for channel 0.")]
        public float PercentageChannel0 { get; set; } = 0;

        /// <summary>
        /// Gets or sets the concentration of Channel1.
        /// </summary>
        [Range(0, 1)]
        [Editor(DesignTypes.SliderEditor, DesignTypes.UITypeEditor)]
        [Description("Odor dilution percentage for channel 1.")]
        public float PercentageChannel1 { get; set; } = 0;

        /// <summary>
        /// Gets or sets the concentration of Channel2.
        /// </summary>
        [Range(0, 1)]
        [Editor(DesignTypes.SliderEditor, DesignTypes.UITypeEditor)]
        [Description("Odor dilution percentage for channel 2.")]
        public float PercentageChannel2 { get; set; } = 0;

        /// <summary>
        /// Gets or sets the concentration of Channel3.
        /// </summary>
        [Range(0, 1)]
        [Editor(DesignTypes.SliderEditor, DesignTypes.UITypeEditor)]
        [Description("Odor dilution percentage for channel 3. This Value will be ignored if Channel3AsCarrier is set to True.")]
        public float PercentageChannel3 { get; set; } = 0;


        public bool channel3AsCarrier = false;
        /// <summary>
        /// Gets or sets the operation mode of Channel3.
        /// </summary>
        [Description("Specifies if Channel3 should be used as an odor or carrier channel. If True, the flow value value of Channel3 will be set to TargetTotalFlow.")]
        public bool Channel3AsCarrier
        {
            get { return channel3AsCarrier; }
            set
            {
                channel3AsCarrier = value;
                PercentageChannel3 = channel3AsCarrier ? float.NaN : 0;
            }
        }

        /// <summary>
        /// Gets or sets the target flow rate for all odor channels.
        /// </summary>
        [Range(0, 100)]
        [Editor(DesignTypes.SliderEditor, DesignTypes.UITypeEditor)]
        [Description("The target odor flow for each channel, assuming PercentageChannelX = 1.")]
        public int TargetOdorFlow { get; set; } = 100;


        /// <summary>
        /// Gets or sets the target total flow rate.
        /// </summary>
        [Range(0, 100)]
        [Editor(DesignTypes.SliderEditor, DesignTypes.UITypeEditor)]
        [Description("The total target flow rate. This value will be used to calculate the flow rate of the carrier automatically.")]
        public int TargetTotalFlow { get; set; } = 1000;

        private List<HarpMessage> ConstructMessages()
        {
            var adjustedOdorFlow0 = (int)(TargetOdorFlow * PercentageChannel0);
            var adjustedOdorFlow1 = (int)(TargetOdorFlow * PercentageChannel1);
            var adjustedOdorFlow2 = (int)(TargetOdorFlow * PercentageChannel2);
            var adjustedOdorFlow3 = channel3AsCarrier ? 0 : (int)(TargetOdorFlow * PercentageChannel3);
            var carrierFlow = TargetOdorFlow - (adjustedOdorFlow0 + adjustedOdorFlow1 + adjustedOdorFlow2 + adjustedOdorFlow3);

            var channelsTargetFlow = ChannelsTargetFlow.FromPayload(MessageType.Write, new ChannelsTargetFlowPayload(
                adjustedOdorFlow0,
                adjustedOdorFlow1,
                adjustedOdorFlow2,
                channel3AsCarrier ? TargetOdorFlow : adjustedOdorFlow3,
                carrierFlow));

            adjustedOdorFlow3 = channel3AsCarrier ? TargetOdorFlow : adjustedOdorFlow3;
            var odorValveState = OdorValveState.FromPayload(MessageType.Write,
            (
                (adjustedOdorFlow0 > 0 ? OdorValves.Valve0 : OdorValves.None) |
                (adjustedOdorFlow1 > 0 ? OdorValves.Valve1 : OdorValves.None) |
                (adjustedOdorFlow2 > 0 ? OdorValves.Valve2 : OdorValves.None) |
                (adjustedOdorFlow3 > 0 ? OdorValves.Valve3 : OdorValves.None)
            ));

            return new List<HarpMessage> { channelsTargetFlow, odorValveState };
        }

        private List<HarpMessage> ConstructMessages(int odorIndex, double concentration)
        {

            var adjustedOdorFlow0 = 0;
            var adjustedOdorFlow1 = 0;
            var adjustedOdorFlow2 = 0;
            var adjustedOdorFlow3 = 0;

            switch (odorIndex)
            {
                case 0:
                    adjustedOdorFlow0 = (int)(TargetOdorFlow * concentration);
                    break;
                case 1:
                    adjustedOdorFlow1 = (int)(TargetOdorFlow * concentration);
                    break;
                case 2:
                    adjustedOdorFlow2 = (int)(TargetOdorFlow * concentration);
                    break;
                case 3:
                    if (channel3AsCarrier)
                    {
                        throw new Exception("Channel 3 is set as carrier. Cannot set flow for this channel.");
                    }
                    adjustedOdorFlow3 = (int)(TargetOdorFlow * concentration);
                    break;
                default:
                    throw new Exception("Invalid channel number. Must be between 0 and 3.");
            }

            var carrierFlow = TargetOdorFlow - (adjustedOdorFlow0 + adjustedOdorFlow1 + adjustedOdorFlow2 + adjustedOdorFlow3);

            var channelsTargetFlow = ChannelsTargetFlow.FromPayload(MessageType.Write, new ChannelsTargetFlowPayload(
                adjustedOdorFlow0,
                adjustedOdorFlow1,
                adjustedOdorFlow2,
                channel3AsCarrier ? TargetOdorFlow : adjustedOdorFlow3,
                carrierFlow));

            adjustedOdorFlow3 = channel3AsCarrier ? TargetOdorFlow : adjustedOdorFlow3;
            var odorValveState = OdorValveState.FromPayload(MessageType.Write,
            (
                (adjustedOdorFlow0 > 0 ? OdorValves.Valve0 : OdorValves.None) |
                (adjustedOdorFlow1 > 0 ? OdorValves.Valve1 : OdorValves.None) |
                (adjustedOdorFlow2 > 0 ? OdorValves.Valve2 : OdorValves.None) |
                (adjustedOdorFlow3 > 0 ? OdorValves.Valve3 : OdorValves.None)
            ));

            return new List<HarpMessage> { channelsTargetFlow, odorValveState };
        }


        /// <summary>
        /// Generates an observable sequence of Harp messages to configure the
        /// odor mixture whenever the input sequence produces an element.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the <paramref name="source"/> sequence.
        /// </typeparam>
        /// <param name="source">
        /// The sequence containing the notifications used to emit new configuration
        /// messages.
        /// </param>
        /// <returns>
        /// A sequence of <see cref="HarpMessage"/> objects representing the commands
        /// needed to fully configure odor mixture.
        /// </returns>
        public IObservable<HarpMessage> Generate<TSource>(IObservable<TSource> source)
        {
            return source.SelectMany(value => ConstructMessages().ToObservable());
        }

        /// <summary>
        /// Generates an observable sequence of Harp messages to configure the
        /// odor mixture.
        /// </summary>
        /// <returns>
        /// A sequence of <see cref="HarpMessage"/> objects representing the commands
        /// needed to fully configure the PWM feature.
        /// </returns>
        public override IObservable<HarpMessage> Generate()
        {
            return ConstructMessages().ToObservable();
        }

        /// <summary>
        /// Generates an observable sequence of Harp messages to configure the
        /// odor mixture whenever the input sequence produces an element. The
        /// tuple values will be used to target a specific channel index (Item1)
        /// and set its concentration (Item2).
        /// </summary>
        /// <param name="source">
        /// The sequence containing a tuple with the channel index and concentration.
        /// </param>
        /// <returns>
        /// A sequence of <see cref="HarpMessage"/> objects representing the commands
        /// needed to fully configure odor mixture.
        /// </returns>
        public IObservable<HarpMessage> Generate(IObservable<Tuple<int, double>> source)
        {
            return source.SelectMany(value => ConstructMessages(value.Item1, value.Item2));
        }
    }

}
