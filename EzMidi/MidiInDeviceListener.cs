using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Midi;

namespace EzMidi {
    /// <summary>
    /// A class used for listening to a single MIDI device
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

        public MidiInDeviceListener(MidiInDevice device) {
            this.Device = device;
            this.input = new MidiIn(device.DeviceNumber);
        }
        ~MidiInDeviceListener() {
            StopListening();
        }

        /// <summary>
        /// Starts listening input on the specified MIDI device
        /// </summary>
        internal void StartListening() {
            input.MessageReceived += OnMidiInputEvent;
            input.Start();
        }

        /// <summary>
        /// Stops listening input on the specified MIDI device
        /// </summary>
        internal void StopListening() {
            if (input != null) {
                input.Stop();
            }
        }

        private void OnMidiInputEvent(object sender, MidiInMessageEventArgs e) {
            MidiEvent m = new MidiEvent(e.RawMessage, Device);
            OnMidiInput?.Invoke(m);
        }

        /// <summary>
        /// Implicitly converts a <see cref=""MidiInput.MidiInDevice/> to a <see cref="MidiInDeviceListener"/>
        /// </summary>
        /// <param name="device">The device info to use</param>
        public static implicit operator MidiInDeviceListener(MidiInDevice device) {
            return new MidiInDeviceListener(device);
        }
    }
}
