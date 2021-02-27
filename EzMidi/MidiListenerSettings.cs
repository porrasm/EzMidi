namespace EzMidi {
    /// <summary>
    /// A struct containing settings about the listener
    /// </summary>
    public struct MidiListenerSettings {
        /// <summary>
        /// How many milliseconds to wait before updating the device list. Set to 0 to disable automatic polling.
        /// </summary>
        public uint UpdateLoopTimeout { get; set; }

        /// <summary>
        /// The MIDI device filter options
        /// </summary>
        public MidiListenerFilter Filters { get; set; }

        /// <summary>
        /// The default listener settings. Devices are updated every 30 seconds and all devices are used.
        /// </summary>
        public static MidiListenerSettings Default {
            get {
                MidiListenerSettings set = new MidiListenerSettings();
                set.UpdateLoopTimeout = 30000;
                set.Filters = MidiListenerFilter.UseAllDevices();
                return set;
            }
        }
    }
}
