using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Midi;

namespace EzMidi {
    /// <summary>
    /// A wrapper class used to retrieve MIDI input devices
    /// </summary>
    public static class MidiDevices {

        /// <summary>
        /// Returns a list of all active MIDI input devices
        /// </summary>
        /// <returns></returns>
        public static List<MidiInDevice> GetActiveMidiInputs() {
            List<MidiInDevice> inputs = new List<MidiInDevice>();
            for (int i = 0; i < MidiIn.NumberOfDevices; i++) {
                inputs.Add(new MidiInDevice(i, MidiIn.DeviceInfo(i)));
            }
            return inputs;
        }

        /// <summary>
        /// Returns a list of all active MIDI input devices
        /// </summary>
        /// <returns></returns>
        public static List<MidiInDevice> GetActiveMidiInputs(MidiListenerFilter filter) {
            List<MidiInDevice> inputs = new List<MidiInDevice>();
            for (int i = 0; i < MidiIn.NumberOfDevices; i++) {
                MidiInDevice device = new MidiInDevice(i, MidiIn.DeviceInfo(i));

                if (filter.UseAll
                    || filter.DeviceNumberFilters.Contains(i)
                    || filter.DeviceIDFilters.Contains(device.InputInfo.ProductId)
                    || filter.ManufactureFilters.Contains(device.InputInfo.Manufacturer)
                    || filter.DeviceNameFilters.Contains(device.InputInfo.ProductName)) {
                    inputs.Add(device);
                }
            }
            return inputs;
        }

        /// <summary>
        /// Collects a device with a matching name
        /// </summary>
        /// <param name="match"></param>
        /// <param name="device"></param>
        /// <returns></returns>
        public static bool GetDeviceByName(string match, out MidiInDevice device) {
            match = match.ToLower();

            foreach (MidiInDevice d in GetActiveMidiInputs()) {
                Console.WriteLine(d.InputInfo.ProductName);
                if (d.InputInfo.ProductName.ToLower().Contains(match)) {
                    device = d;
                    return true;
                }
            }

            device = default;
            return false;
        }

        /// <summary>
        /// Collects a device with a matching device number
        /// </summary>
        /// <param name="deviceNumber"></param>
        /// <param name="device"></param>
        /// <returns></returns>
        public static bool GetDeviceByNumber(int deviceNumber, out MidiInDevice device) {
            if (deviceNumber >= MidiIn.NumberOfDevices) {
                device = default;
                return false;
            }
            device = new MidiInDevice(deviceNumber, MidiIn.DeviceInfo(deviceNumber));
            return true;
        }



    }
}
