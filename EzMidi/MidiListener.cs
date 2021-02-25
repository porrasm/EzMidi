using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzMidi {
    public static class MidiListener {

        #region fields
        /// <summary>
        /// A callback delegate for receiving MIDI events
        /// </summary>
        /// <param name="e">The received MIDI event</param>
        public delegate void OnMidiEvent(MidiEvent e);

        /// <summary>
        /// True if the listeners are active
        /// </summary>
        public static bool IsListening { get; private set; }

        private static List<MidiInDeviceListener> devicesToListen;

        private static MidiListenerSettings settings;
        private static int runIndex;

        /// <summary>
        /// The MIDI event callback
        /// </summary>
        public static OnMidiEvent MidiEventCallback { get; set; }
        #endregion

        static MidiListener() {
            devicesToListen = new List<MidiInDeviceListener>();
        }

        #region management
        /// <summary>
        /// Starts listening MIDI input on all of the devices. If <see cref="IsListening"/> is true, this does nothing.
        /// </summary>
        public static void StartListening(MidiListenerSettings settings) {
            if (IsListening) {
                return;
            }
            IsListening = true;
            MidiListener.settings = settings;

            UpdateLoop(++runIndex);

            foreach (MidiInDeviceListener device in devicesToListen) {
                device.StartListening();
                device.OnMidiInput += OnMidiEventHandler;
            }
        }

        /// <summary>
        /// Stops listening MIDI input
        /// </summary>
        public static void StopListening() {
            if (!IsListening) {
                return;
            }

            runIndex++;
            foreach (MidiInDeviceListener device in devicesToListen) {
                device.StopListening();
                device.OnMidiInput -= OnMidiEventHandler;
            }
            devicesToListen.Clear();
        }

        private static async void UpdateLoop(int currentIndex) {
            if (settings.UpdateLoopTimeout == 0) {
                return;
            }
            while (currentIndex == runIndex) {
                UpdateAllDevices();
                await Task.Delay((int)settings.UpdateLoopTimeout);
            }
        }

        private static void UpdateAllDevices() {
            foreach (MidiInDeviceListener device in devicesToListen) {
                device.StopListening();
            }
            devicesToListen.Clear();

            foreach (MidiInDevice input in MidiDevices.GetActiveMidiInputs(settings.Filters)) {
                devicesToListen.Add(input);
            }
        }
        #endregion

        #region input 
        private static void OnMidiEventHandler(MidiEvent e) {
            try {
                Console.WriteLine(e.ToPrettyString(true));
                MidiEventCallback?.Invoke(e);
            } catch (Exception ex) {
                Console.WriteLine("Error on MIDI event callback: " + e);
            }
        }
        #endregion
    }
}
