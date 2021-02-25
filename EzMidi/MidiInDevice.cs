using NAudio.Midi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzMidi {
    /// <summary>
    /// A wrapper class containing MIDI device information
    /// </summary>
    public class MidiInDevice {

        /// <summary>
        /// MIDI device index
        /// </summary>
        public int DeviceNumber { get; private set; }

        /// <summary>
        /// MIDI device info
        /// </summary>
        public MidiInCapabilities InputInfo { get; private set; }

        /// <summary>
        /// Creates a new <see cref="MidiInDevice"/> instance
        /// </summary>
        /// <param name="deviceNumber"></param>
        /// <param name="inputInfo"></param>
        public MidiInDevice(int deviceNumber, MidiInCapabilities inputInfo) {
            DeviceNumber = deviceNumber;
            InputInfo = inputInfo;
        }

        public override bool Equals(object obj) {
            return obj is MidiInDevice device &&
                   DeviceNumber == device.DeviceNumber;
        }

        public override int GetHashCode() {
            return 1015454754 + DeviceNumber.GetHashCode();
        }
    }
}
