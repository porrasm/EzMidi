using NAudio.Midi;
using System;
using System.Collections.Generic;

namespace EzMidi {
    /// <summary>
    /// A wrapper class used to retrieve MIDI input devices
    /// </summary>
    public static class MidiDevices {
        /// <summary>
        /// Returns a list of all active MIDI input devices
        /// </summary>
        /// <returns></returns>
        public static List<MidiInDevice> GetActiveMidiInputs(MidiListenerFilter filter) {
            List<MidiInDevice> inputs = new List<MidiInDevice>();
            for (int i = 0; i < MidiIn.NumberOfDevices; i++) {
                try {
                    MidiInDevice device = new MidiInDevice(i, MidiIn.DeviceInfo(i));

                    if (filter.UseAll
                        || filter.DeviceNumberFilters.Contains(i)
                        || filter.DeviceIDFilters.Contains(device.InputInfo.ProductId)
                        || filter.ManufactureFilters.Contains(device.InputInfo.Manufacturer)
                        || filter.DeviceNameFilters.Contains(device.InputInfo.ProductName?.ToLower())) {
                        inputs.Add(device);
                    }
                } catch (Exception e) {
                    Console.WriteLine("Could not create MIDI device: " + e);
                }
            }
            return inputs;
        }
    }
}
