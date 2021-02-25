using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Midi;

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
        public void StartListening() {
            input.MessageReceived += OnMidiInputEvent;
            input.Start();
        }

        /// <summary>
        /// Stops listening input on the specified MIDI device. Do not call if you are using <see cref="MidiListener"/>.
        /// </summary>
        public void StopListening() {
            if (input != null) {
                input.Stop();
            }
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
