using NAudio.Midi;
using System;

namespace EzMidi {
    /// <summary>
    /// A class used for listening to a single MIDI device. Do not use this class if you are using <see cref="MidiListener"/>
    /// </summary>
    public class MidiInDeviceListener {

        #region fields
        private MidiIn input;

        /// <summary>
        /// The device assigned to this listener.
        /// </summary>
        public MidiInDevice Device { get; private set; }

        /// <summary>
        /// A callback which can be used to get MIDI events from this device
        /// </summary>
        public MidiListener.OnMidiEvent OnMidiInput { get; set; }
        #endregion

        /// <summary>
        /// Creates a new <see cref="MidiInDeviceListener"/> listener instance
        /// </summary>
        /// <param name="device"></param>
        public MidiInDeviceListener(MidiInDevice device) {
            this.Device = device;
            this.input = new MidiIn(device.DeviceNumber);
        }
        /// <summary>
        /// Stops the listening process before garbage collection
        /// </summary>
        ~MidiInDeviceListener() {
            StopListening();
        }

        /// <summary>
        /// Starts listening input on the specified MIDI device. Do not call if you are using <see cref="MidiListener"/>.
        /// </summary>
        public bool StartListening() {
            try {
                input.MessageReceived += OnMidiInputEvent;
                input.Start();
                return true;
            } catch (Exception e) {
                Console.WriteLine("Error listening to MIDI device: " + e);
                return false;
            }
        }

        /// <summary>
        /// Stops listening input on the specified MIDI device. Do not call if you are using <see cref="MidiListener"/>.
        /// </summary>
        public bool StopListening() {
            try {
                if (input != null) {
                    input.Stop();
                    input.Close();
                    input.Dispose();
                    return true;
                }
            } catch (Exception e) {
                Console.WriteLine("Error closing MIDI device listener: " + e);
            }

            return false;
        }

        private void OnMidiInputEvent(object sender, MidiInMessageEventArgs e) {
            MidiEvent m = new MidiEvent(e.RawMessage, Device);
            OnMidiInput?.Invoke(m);
        }

        /// <summary>
        /// Converts a <see cref="MidiInDevice"/> to a <see cref="MidiInDeviceListener"/> implicitly
        /// </summary>
        /// <param name="device"></param>
        public static implicit operator MidiInDeviceListener(MidiInDevice device) {
            return new MidiInDeviceListener(device);
        }
    }
}
