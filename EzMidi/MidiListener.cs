using System;
using System.Collections.Generic;
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

        /// <summary>
        /// The active list of devices
        /// </summary>
        public static List<MidiInDeviceListener> DevicesToListen { get; private set; }

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
            DevicesToListen = new List<MidiInDeviceListener>();
        }

        #region management
        /// <summary>
        /// Starts listening MIDI input on all of the devices. If <see cref="IsListening"/> is true, this does nothing. Returns false if there was an error starting a MIDI listener.
        /// </summary>
        /// <param name="settings">The settings to use</param>
        /// <param name="callback">The callback to use</param>
        public static bool StartListening(MidiListenerSettings settings, OnMidiEvent callback = null) {
            if (IsListening) {
                return false;
            }

            DevicesToListen.Clear();
            IsListening = true;
            MidiListener.settings = settings;

            quickSetupCallback = callback;

            UpdateAllDevices();
            UpdateLoop(++runIndex);

            bool success = true;

            foreach (MidiInDeviceListener device in DevicesToListen) {
                if (device.StartListening()) {
                    device.OnMidiInput += OnMidiEventHandler;
                } else {
                    success = false;
                }
            }
            return success;
        }

        /// <summary>
        /// Stops listening MIDI input. Returns false if there was a failure closing a MIDI device. If there was an error you can try to manually close to listeners using <see cref="DevicesToListen"/> list.
        /// </summary>
        public static bool StopListening() {
            if (!IsListening) {
                return false;
            }

            bool success = true;

            runIndex++;
            foreach (MidiInDeviceListener device in DevicesToListen) {
                if (device.StopListening()) {
                    device.OnMidiInput -= OnMidiEventHandler;
                } else {
                    success = false;
                }
            }

            return success;
        }

        private static async void UpdateLoop(int currentIndex) {
            if (settings.UpdateLoopTimeout == 0) {
                return;
            }
            await Task.Delay((int)settings.UpdateLoopTimeout);
            while (currentIndex == runIndex) {
                UpdateAllDevices();
                await Task.Delay((int)settings.UpdateLoopTimeout);
            }
        }

        /// <summary>
        /// Reloads all MIDI devices based on the given filter settings 
        /// </summary>
        public static void UpdateAllDevices() {
            foreach (MidiInDeviceListener device in DevicesToListen) {
                device.StopListening();
            }
            DevicesToListen.Clear();

            foreach (MidiInDevice input in MidiDevices.GetActiveMidiInputs(settings.Filters)) {
                DevicesToListen.Add(input);
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
