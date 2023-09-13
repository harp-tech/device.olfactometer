using Bonsai.Harp;
using System.Threading.Tasks;

namespace Harp.Olfactometer
{
    /// <inheritdoc/>
    public partial class Device
    {
        /// <summary>
        /// Initializes a new instance of the asynchronous API to configure and interface
        /// with Olfactometer devices on the specified serial port.
        /// </summary>
        /// <param name="portName">
        /// The name of the serial port used to communicate with the Harp device.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous initialization operation. The value of
        /// the <see cref="Task{TResult}.Result"/> parameter contains a new instance of
        /// the <see cref="AsyncDevice"/> class.
        /// </returns>
        public static async Task<AsyncDevice> CreateAsync(string portName)
        {
            var device = new AsyncDevice(portName);
            var whoAmI = await device.ReadWhoAmIAsync();
            if (whoAmI != Device.WhoAmI)
            {
                var errorMessage = string.Format(
                    "The device ID {1} on {0} was unexpected. Check whether a Olfactometer device is connected to the specified serial port.",
                    portName, whoAmI);
                throw new HarpException(errorMessage);
            }

            return device;
        }
    }

    /// <summary>
    /// Represents an asynchronous API to configure and interface with Olfactometer devices.
    /// </summary>
    public partial class AsyncDevice : Bonsai.Harp.AsyncDevice
    {
        internal AsyncDevice(string portName)
            : base(portName)
        {
        }

        /// <summary>
        /// Asynchronously reads the contents of the EnableFlow register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<EnableFlag> ReadEnableFlowAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(EnableFlow.Address));
            return EnableFlow.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the EnableFlow register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<EnableFlag>> ReadTimestampedEnableFlowAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(EnableFlow.Address));
            return EnableFlow.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the EnableFlow register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteEnableFlowAsync(EnableFlag value)
        {
            var request = EnableFlow.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the FlowmeterAnalogOutputs register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<short[]> ReadFlowmeterAnalogOutputsAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadInt16(FlowmeterAnalogOutputs.Address));
            return FlowmeterAnalogOutputs.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the FlowmeterAnalogOutputs register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<short[]>> ReadTimestampedFlowmeterAnalogOutputsAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadInt16(FlowmeterAnalogOutputs.Address));
            return FlowmeterAnalogOutputs.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the contents of the DI0State register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<DigitalState> ReadDI0StateAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(DI0State.Address));
            return DI0State.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the DI0State register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<DigitalState>> ReadTimestampedDI0StateAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(DI0State.Address));
            return DI0State.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the contents of the Channel0UserCalibration register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<ushort[]> ReadChannel0UserCalibrationAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt16(Channel0UserCalibration.Address));
            return Channel0UserCalibration.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the Channel0UserCalibration register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<ushort[]>> ReadTimestampedChannel0UserCalibrationAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt16(Channel0UserCalibration.Address));
            return Channel0UserCalibration.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the Channel0UserCalibration register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteChannel0UserCalibrationAsync(ushort[] value)
        {
            var request = Channel0UserCalibration.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the Channel1UserCalibration register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<ushort[]> ReadChannel1UserCalibrationAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt16(Channel1UserCalibration.Address));
            return Channel1UserCalibration.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the Channel1UserCalibration register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<ushort[]>> ReadTimestampedChannel1UserCalibrationAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt16(Channel1UserCalibration.Address));
            return Channel1UserCalibration.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the Channel1UserCalibration register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteChannel1UserCalibrationAsync(ushort[] value)
        {
            var request = Channel1UserCalibration.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the Channel2UserCalibration register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<ushort[]> ReadChannel2UserCalibrationAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt16(Channel2UserCalibration.Address));
            return Channel2UserCalibration.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the Channel2UserCalibration register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<ushort[]>> ReadTimestampedChannel2UserCalibrationAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt16(Channel2UserCalibration.Address));
            return Channel2UserCalibration.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the Channel2UserCalibration register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteChannel2UserCalibrationAsync(ushort[] value)
        {
            var request = Channel2UserCalibration.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the Channel3UserCalibration register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<ushort[]> ReadChannel3UserCalibrationAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt16(Channel3UserCalibration.Address));
            return Channel3UserCalibration.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the Channel3UserCalibration register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<ushort[]>> ReadTimestampedChannel3UserCalibrationAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt16(Channel3UserCalibration.Address));
            return Channel3UserCalibration.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the Channel3UserCalibration register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteChannel3UserCalibrationAsync(ushort[] value)
        {
            var request = Channel3UserCalibration.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the Channel4UserCalibration register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<ushort[]> ReadChannel4UserCalibrationAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt16(Channel4UserCalibration.Address));
            return Channel4UserCalibration.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the Channel4UserCalibration register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<ushort[]>> ReadTimestampedChannel4UserCalibrationAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt16(Channel4UserCalibration.Address));
            return Channel4UserCalibration.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the Channel4UserCalibration register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteChannel4UserCalibrationAsync(ushort[] value)
        {
            var request = Channel4UserCalibration.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the Channel3UserCalibrationAux register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<ushort[]> ReadChannel3UserCalibrationAuxAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt16(Channel3UserCalibrationAux.Address));
            return Channel3UserCalibrationAux.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the Channel3UserCalibrationAux register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<ushort[]>> ReadTimestampedChannel3UserCalibrationAuxAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt16(Channel3UserCalibrationAux.Address));
            return Channel3UserCalibrationAux.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the Channel3UserCalibrationAux register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteChannel3UserCalibrationAuxAsync(ushort[] value)
        {
            var request = Channel3UserCalibrationAux.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the UserCalibrationEnable register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<byte> ReadUserCalibrationEnableAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(UserCalibrationEnable.Address));
            return UserCalibrationEnable.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the UserCalibrationEnable register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<byte>> ReadTimestampedUserCalibrationEnableAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(UserCalibrationEnable.Address));
            return UserCalibrationEnable.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the UserCalibrationEnable register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteUserCalibrationEnableAsync(byte value)
        {
            var request = UserCalibrationEnable.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the Channel0FlowTarget register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<float> ReadChannel0FlowTargetAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadSingle(Channel0FlowTarget.Address));
            return Channel0FlowTarget.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the Channel0FlowTarget register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<float>> ReadTimestampedChannel0FlowTargetAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadSingle(Channel0FlowTarget.Address));
            return Channel0FlowTarget.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the Channel0FlowTarget register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteChannel0FlowTargetAsync(float value)
        {
            var request = Channel0FlowTarget.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the Channel1FlowTarget register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<float> ReadChannel1FlowTargetAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadSingle(Channel1FlowTarget.Address));
            return Channel1FlowTarget.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the Channel1FlowTarget register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<float>> ReadTimestampedChannel1FlowTargetAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadSingle(Channel1FlowTarget.Address));
            return Channel1FlowTarget.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the Channel1FlowTarget register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteChannel1FlowTargetAsync(float value)
        {
            var request = Channel1FlowTarget.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the Channel2FlowTarget register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<float> ReadChannel2FlowTargetAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadSingle(Channel2FlowTarget.Address));
            return Channel2FlowTarget.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the Channel2FlowTarget register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<float>> ReadTimestampedChannel2FlowTargetAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadSingle(Channel2FlowTarget.Address));
            return Channel2FlowTarget.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the Channel2FlowTarget register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteChannel2FlowTargetAsync(float value)
        {
            var request = Channel2FlowTarget.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the Channel3FlowTarget register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<float> ReadChannel3FlowTargetAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadSingle(Channel3FlowTarget.Address));
            return Channel3FlowTarget.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the Channel3FlowTarget register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<float>> ReadTimestampedChannel3FlowTargetAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadSingle(Channel3FlowTarget.Address));
            return Channel3FlowTarget.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the Channel3FlowTarget register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteChannel3FlowTargetAsync(float value)
        {
            var request = Channel3FlowTarget.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the Channel4FlowTarget register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<float> ReadChannel4FlowTargetAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadSingle(Channel4FlowTarget.Address));
            return Channel4FlowTarget.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the Channel4FlowTarget register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<float>> ReadTimestampedChannel4FlowTargetAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadSingle(Channel4FlowTarget.Address));
            return Channel4FlowTarget.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the Channel4FlowTarget register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteChannel4FlowTargetAsync(float value)
        {
            var request = Channel4FlowTarget.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the Channel0FlowReal register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<float> ReadChannel0FlowRealAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadSingle(Channel0FlowReal.Address));
            return Channel0FlowReal.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the Channel0FlowReal register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<float>> ReadTimestampedChannel0FlowRealAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadSingle(Channel0FlowReal.Address));
            return Channel0FlowReal.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the contents of the Channel1FlowReal register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<float> ReadChannel1FlowRealAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadSingle(Channel1FlowReal.Address));
            return Channel1FlowReal.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the Channel1FlowReal register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<float>> ReadTimestampedChannel1FlowRealAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadSingle(Channel1FlowReal.Address));
            return Channel1FlowReal.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the contents of the Channel2FlowReal register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<float> ReadChannel2FlowRealAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadSingle(Channel2FlowReal.Address));
            return Channel2FlowReal.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the Channel2FlowReal register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<float>> ReadTimestampedChannel2FlowRealAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadSingle(Channel2FlowReal.Address));
            return Channel2FlowReal.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the contents of the Channel3FlowReal register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<float> ReadChannel3FlowRealAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadSingle(Channel3FlowReal.Address));
            return Channel3FlowReal.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the Channel3FlowReal register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<float>> ReadTimestampedChannel3FlowRealAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadSingle(Channel3FlowReal.Address));
            return Channel3FlowReal.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the contents of the Channel4FlowReal register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<float> ReadChannel4FlowRealAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadSingle(Channel4FlowReal.Address));
            return Channel4FlowReal.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the Channel4FlowReal register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<float>> ReadTimestampedChannel4FlowRealAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadSingle(Channel4FlowReal.Address));
            return Channel4FlowReal.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the contents of the Channel0DutyCycle register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<float> ReadChannel0DutyCycleAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadSingle(Channel0DutyCycle.Address));
            return Channel0DutyCycle.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the Channel0DutyCycle register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<float>> ReadTimestampedChannel0DutyCycleAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadSingle(Channel0DutyCycle.Address));
            return Channel0DutyCycle.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the Channel0DutyCycle register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteChannel0DutyCycleAsync(float value)
        {
            var request = Channel0DutyCycle.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the Channel1DutyCycle register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<float> ReadChannel1DutyCycleAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadSingle(Channel1DutyCycle.Address));
            return Channel1DutyCycle.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the Channel1DutyCycle register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<float>> ReadTimestampedChannel1DutyCycleAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadSingle(Channel1DutyCycle.Address));
            return Channel1DutyCycle.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the Channel1DutyCycle register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteChannel1DutyCycleAsync(float value)
        {
            var request = Channel1DutyCycle.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the Channel2DutyCycle register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<float> ReadChannel2DutyCycleAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadSingle(Channel2DutyCycle.Address));
            return Channel2DutyCycle.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the Channel2DutyCycle register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<float>> ReadTimestampedChannel2DutyCycleAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadSingle(Channel2DutyCycle.Address));
            return Channel2DutyCycle.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the Channel2DutyCycle register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteChannel2DutyCycleAsync(float value)
        {
            var request = Channel2DutyCycle.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the Channel3DutyCycle register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<float> ReadChannel3DutyCycleAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadSingle(Channel3DutyCycle.Address));
            return Channel3DutyCycle.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the Channel3DutyCycle register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<float>> ReadTimestampedChannel3DutyCycleAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadSingle(Channel3DutyCycle.Address));
            return Channel3DutyCycle.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the Channel3DutyCycle register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteChannel3DutyCycleAsync(float value)
        {
            var request = Channel3DutyCycle.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the Channel4DutyCycle register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<float> ReadChannel4DutyCycleAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadSingle(Channel4DutyCycle.Address));
            return Channel4DutyCycle.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the Channel4DutyCycle register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<float>> ReadTimestampedChannel4DutyCycleAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadSingle(Channel4DutyCycle.Address));
            return Channel4DutyCycle.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the Channel4DutyCycle register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteChannel4DutyCycleAsync(float value)
        {
            var request = Channel4DutyCycle.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the DigitalOutputSet register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<DigitalOutputs> ReadDigitalOutputSetAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(DigitalOutputSet.Address));
            return DigitalOutputSet.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the DigitalOutputSet register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<DigitalOutputs>> ReadTimestampedDigitalOutputSetAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(DigitalOutputSet.Address));
            return DigitalOutputSet.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the DigitalOutputSet register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteDigitalOutputSetAsync(DigitalOutputs value)
        {
            var request = DigitalOutputSet.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the DigitalOutputClear register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<DigitalOutputs> ReadDigitalOutputClearAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(DigitalOutputClear.Address));
            return DigitalOutputClear.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the DigitalOutputClear register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<DigitalOutputs>> ReadTimestampedDigitalOutputClearAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(DigitalOutputClear.Address));
            return DigitalOutputClear.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the DigitalOutputClear register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteDigitalOutputClearAsync(DigitalOutputs value)
        {
            var request = DigitalOutputClear.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the DigitalOutputToggle register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<DigitalOutputs> ReadDigitalOutputToggleAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(DigitalOutputToggle.Address));
            return DigitalOutputToggle.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the DigitalOutputToggle register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<DigitalOutputs>> ReadTimestampedDigitalOutputToggleAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(DigitalOutputToggle.Address));
            return DigitalOutputToggle.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the DigitalOutputToggle register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteDigitalOutputToggleAsync(DigitalOutputs value)
        {
            var request = DigitalOutputToggle.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the DigitalOutputState register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<DigitalOutputs> ReadDigitalOutputStateAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(DigitalOutputState.Address));
            return DigitalOutputState.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the DigitalOutputState register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<DigitalOutputs>> ReadTimestampedDigitalOutputStateAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(DigitalOutputState.Address));
            return DigitalOutputState.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the DigitalOutputState register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteDigitalOutputStateAsync(DigitalOutputs value)
        {
            var request = DigitalOutputState.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the EnableValvesPulse register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<Valves> ReadEnableValvesPulseAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(EnableValvesPulse.Address));
            return EnableValvesPulse.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the EnableValvesPulse register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<Valves>> ReadTimestampedEnableValvesPulseAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(EnableValvesPulse.Address));
            return EnableValvesPulse.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the EnableValvesPulse register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteEnableValvesPulseAsync(Valves value)
        {
            var request = EnableValvesPulse.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the ValvesSet register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<Valves> ReadValvesSetAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(ValvesSet.Address));
            return ValvesSet.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the ValvesSet register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<Valves>> ReadTimestampedValvesSetAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(ValvesSet.Address));
            return ValvesSet.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the ValvesSet register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteValvesSetAsync(Valves value)
        {
            var request = ValvesSet.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the ValvesClear register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<Valves> ReadValvesClearAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(ValvesClear.Address));
            return ValvesClear.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the ValvesClear register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<Valves>> ReadTimestampedValvesClearAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(ValvesClear.Address));
            return ValvesClear.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the ValvesClear register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteValvesClearAsync(Valves value)
        {
            var request = ValvesClear.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the ValvesToggle register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<Valves> ReadValvesToggleAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(ValvesToggle.Address));
            return ValvesToggle.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the ValvesToggle register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<Valves>> ReadTimestampedValvesToggleAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(ValvesToggle.Address));
            return ValvesToggle.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the ValvesToggle register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteValvesToggleAsync(Valves value)
        {
            var request = ValvesToggle.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the ValvesState register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<Valves> ReadValvesStateAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(ValvesState.Address));
            return ValvesState.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the ValvesState register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<Valves>> ReadTimestampedValvesStateAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(ValvesState.Address));
            return ValvesState.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the ValvesState register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteValvesStateAsync(Valves value)
        {
            var request = ValvesState.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the PulseValve0 register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<ushort> ReadPulseValve0Async()
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt16(PulseValve0.Address));
            return PulseValve0.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the PulseValve0 register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<ushort>> ReadTimestampedPulseValve0Async()
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt16(PulseValve0.Address));
            return PulseValve0.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the PulseValve0 register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WritePulseValve0Async(ushort value)
        {
            var request = PulseValve0.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the PulseValve1 register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<ushort> ReadPulseValve1Async()
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt16(PulseValve1.Address));
            return PulseValve1.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the PulseValve1 register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<ushort>> ReadTimestampedPulseValve1Async()
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt16(PulseValve1.Address));
            return PulseValve1.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the PulseValve1 register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WritePulseValve1Async(ushort value)
        {
            var request = PulseValve1.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the PulseValve2 register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<ushort> ReadPulseValve2Async()
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt16(PulseValve2.Address));
            return PulseValve2.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the PulseValve2 register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<ushort>> ReadTimestampedPulseValve2Async()
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt16(PulseValve2.Address));
            return PulseValve2.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the PulseValve2 register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WritePulseValve2Async(ushort value)
        {
            var request = PulseValve2.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the PulseValve3 register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<ushort> ReadPulseValve3Async()
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt16(PulseValve3.Address));
            return PulseValve3.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the PulseValve3 register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<ushort>> ReadTimestampedPulseValve3Async()
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt16(PulseValve3.Address));
            return PulseValve3.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the PulseValve3 register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WritePulseValve3Async(ushort value)
        {
            var request = PulseValve3.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the PulseEndvalve0 register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<ushort> ReadPulseEndvalve0Async()
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt16(PulseEndvalve0.Address));
            return PulseEndvalve0.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the PulseEndvalve0 register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<ushort>> ReadTimestampedPulseEndvalve0Async()
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt16(PulseEndvalve0.Address));
            return PulseEndvalve0.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the PulseEndvalve0 register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WritePulseEndvalve0Async(ushort value)
        {
            var request = PulseEndvalve0.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the PulseEndvalve1 register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<ushort> ReadPulseEndvalve1Async()
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt16(PulseEndvalve1.Address));
            return PulseEndvalve1.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the PulseEndvalve1 register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<ushort>> ReadTimestampedPulseEndvalve1Async()
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt16(PulseEndvalve1.Address));
            return PulseEndvalve1.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the PulseEndvalve1 register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WritePulseEndvalve1Async(ushort value)
        {
            var request = PulseEndvalve1.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the PulseDummyvalve register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<ushort> ReadPulseDummyvalveAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt16(PulseDummyvalve.Address));
            return PulseDummyvalve.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the PulseDummyvalve register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<ushort>> ReadTimestampedPulseDummyvalveAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt16(PulseDummyvalve.Address));
            return PulseDummyvalve.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the PulseDummyvalve register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WritePulseDummyvalveAsync(ushort value)
        {
            var request = PulseDummyvalve.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the DO0Sync register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<DO0SyncConfig> ReadDO0SyncAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(DO0Sync.Address));
            return DO0Sync.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the DO0Sync register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<DO0SyncConfig>> ReadTimestampedDO0SyncAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(DO0Sync.Address));
            return DO0Sync.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the DO0Sync register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteDO0SyncAsync(DO0SyncConfig value)
        {
            var request = DO0Sync.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the DO1Sync register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<DO1SyncConfig> ReadDO1SyncAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(DO1Sync.Address));
            return DO1Sync.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the DO1Sync register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<DO1SyncConfig>> ReadTimestampedDO1SyncAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(DO1Sync.Address));
            return DO1Sync.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the DO1Sync register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteDO1SyncAsync(DO1SyncConfig value)
        {
            var request = DO1Sync.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the DI0Trigger register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<DI0TriggerConfig> ReadDI0TriggerAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(DI0Trigger.Address));
            return DI0Trigger.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the DI0Trigger register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<DI0TriggerConfig>> ReadTimestampedDI0TriggerAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(DI0Trigger.Address));
            return DI0Trigger.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the DI0Trigger register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteDI0TriggerAsync(DI0TriggerConfig value)
        {
            var request = DI0Trigger.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the MimicValve0 register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<MimicOuputs> ReadMimicValve0Async()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(MimicValve0.Address));
            return MimicValve0.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the MimicValve0 register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<MimicOuputs>> ReadTimestampedMimicValve0Async()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(MimicValve0.Address));
            return MimicValve0.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the MimicValve0 register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteMimicValve0Async(MimicOuputs value)
        {
            var request = MimicValve0.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the MimicValve1 register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<MimicOuputs> ReadMimicValve1Async()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(MimicValve1.Address));
            return MimicValve1.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the MimicValve1 register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<MimicOuputs>> ReadTimestampedMimicValve1Async()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(MimicValve1.Address));
            return MimicValve1.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the MimicValve1 register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteMimicValve1Async(MimicOuputs value)
        {
            var request = MimicValve1.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the MimicValve2 register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<MimicOuputs> ReadMimicValve2Async()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(MimicValve2.Address));
            return MimicValve2.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the MimicValve2 register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<MimicOuputs>> ReadTimestampedMimicValve2Async()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(MimicValve2.Address));
            return MimicValve2.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the MimicValve2 register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteMimicValve2Async(MimicOuputs value)
        {
            var request = MimicValve2.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the MimicValve3 register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<MimicOuputs> ReadMimicValve3Async()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(MimicValve3.Address));
            return MimicValve3.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the MimicValve3 register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<MimicOuputs>> ReadTimestampedMimicValve3Async()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(MimicValve3.Address));
            return MimicValve3.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the MimicValve3 register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteMimicValve3Async(MimicOuputs value)
        {
            var request = MimicValve3.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the MimicEndvalve0 register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<MimicOuputs> ReadMimicEndvalve0Async()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(MimicEndvalve0.Address));
            return MimicEndvalve0.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the MimicEndvalve0 register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<MimicOuputs>> ReadTimestampedMimicEndvalve0Async()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(MimicEndvalve0.Address));
            return MimicEndvalve0.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the MimicEndvalve0 register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteMimicEndvalve0Async(MimicOuputs value)
        {
            var request = MimicEndvalve0.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the MimicEndvalve1 register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<MimicOuputs> ReadMimicEndvalve1Async()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(MimicEndvalve1.Address));
            return MimicEndvalve1.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the MimicEndvalve1 register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<MimicOuputs>> ReadTimestampedMimicEndvalve1Async()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(MimicEndvalve1.Address));
            return MimicEndvalve1.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the MimicEndvalve1 register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteMimicEndvalve1Async(MimicOuputs value)
        {
            var request = MimicEndvalve1.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the MimicDummyvalve register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<MimicOuputs> ReadMimicDummyvalveAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(MimicDummyvalve.Address));
            return MimicDummyvalve.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the MimicDummyvalve register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<MimicOuputs>> ReadTimestampedMimicDummyvalveAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(MimicDummyvalve.Address));
            return MimicDummyvalve.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the MimicDummyvalve register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteMimicDummyvalveAsync(MimicOuputs value)
        {
            var request = MimicDummyvalve.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the EnableExternalControlValves register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<EnableFlag> ReadEnableExternalControlValvesAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(EnableExternalControlValves.Address));
            return EnableExternalControlValves.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the EnableExternalControlValves register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<EnableFlag>> ReadTimestampedEnableExternalControlValvesAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(EnableExternalControlValves.Address));
            return EnableExternalControlValves.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the EnableExternalControlValves register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteEnableExternalControlValvesAsync(EnableFlag value)
        {
            var request = EnableExternalControlValves.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the Channel3Range register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<Channel3RangeConfig> ReadChannel3RangeAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(Channel3Range.Address));
            return Channel3Range.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the Channel3Range register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<Channel3RangeConfig>> ReadTimestampedChannel3RangeAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(Channel3Range.Address));
            return Channel3Range.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the Channel3Range register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteChannel3RangeAsync(Channel3RangeConfig value)
        {
            var request = Channel3Range.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the EnableEvents register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<OlfactometerEvents> ReadEnableEventsAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(EnableEvents.Address));
            return EnableEvents.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the EnableEvents register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<OlfactometerEvents>> ReadTimestampedEnableEventsAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(EnableEvents.Address));
            return EnableEvents.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the EnableEvents register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteEnableEventsAsync(OlfactometerEvents value)
        {
            var request = EnableEvents.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }
    }
}
