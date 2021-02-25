using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzMidi {
    /// <summary>
    /// Static class used for listening to MIDI devices
    /// </summary>
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
        /// The MIDI event callback. Different delegate from the one that you can give as a parameter to <see cref="StartListening(MidiListenerSettings, OnMidiEvent)"/>
        /// </summary>
        public static OnMidiEvent MidiEventCallback { get; set; }

        /// <summary>
        /// Alternate callback which can be given directly as a parameter.
        /// </summary>
        private static OnMidiEvent quickSetupCallback;
        #endregion

        static MidiListener() {
            devicesToListen = new List<MidiInDeviceListener>();
        }

        #region management
        /// <summary>
        /// Starts listening MIDI input on all of the devices. If <see cref="IsListening"/> is true, this does nothing.
        /// </summary>
        /// <param name="settings">The settings to use</param>
        /// <param name="callback">The callback to use</param>
        public static void StartListening(MidiListenerSettings settings, OnMidiEvent callback = null) {
            if (IsListening) {
                return;
            }
            IsListening = true;
            MidiListener.settings = settings;

            quickSetupCallback = callback;

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

        /// <summary>
        /// Reloads all MIDI devices based on the given filter settings 
        /// </summary>
        public static void UpdateAllDevices() {
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
                quickSetupCallback?.Invoke(e);
                MidiEventCallback?.Invoke(e);
            } catch (Exception ex) {
                Console.WriteLine("Error on MIDI event callback: " + ex);
            }
        }
        #endregion
    }
}
