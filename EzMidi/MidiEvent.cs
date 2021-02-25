using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Midi;

namespace EzMidi {
    public struct MidiEvent {

        /// <summary>
        /// The device which sent the event
        /// </summary>
        public MidiInDevice Device { get; private set; }

        /// <summary>
        /// The type of the event received and the channel it was received on. See <see cref="MidiCommandCode"/>
        /// </summary>
        public byte StatusByte { get; private set; }

        /// <summary>
        /// The target control of this event. Has value between [0, 127] and corresponds to for example a note or a MIDI CC number.
        /// </summary>
        public byte Control { get; private set; }

        /// <summary>
        /// The value which was received on the <see cref="Control"/>. Corresponds to for example note velocity or MIDI CC value.
        /// </summary>
        public byte Value { get; private set; }

        /// <summary>
        /// The command code of this event
        /// </summary>
        public MidiCommandCode CommandCode => (MidiCommandCode)(StatusByte & 0xF1);

        /// <summary>
        /// The channel this event was received on
        /// </summary>
        public byte MidiChannel => (byte)(StatusByte & 0xF);

        /// <summary>
        /// Create a new <see cref="MidiEvent"/> instance from raw MIDI data
        /// </summary>
        /// <param name="rawData">The raw MIDI data</param>
        /// <param name="device">The device which sent this event</param>
        public MidiEvent(int rawData, MidiInDevice device = null) {
            this.Device = device;
            StatusByte = (byte)(rawData & 0xFF);
            Control = (byte)((rawData >> 8) & 0xFF);
            Value = (byte)((rawData >> 16) & 0xFF);
        }

        public override string ToString() {
            return $"MIDI Event: ({Convert.ToString(StatusByte, 2).PadLeft(8, '0')}, {Convert.ToString(Control, 2).PadLeft(8, '0')}, {Convert.ToString(Value, 2).PadLeft(8, '0')})";
        }

        /// <summary>
        /// Converts this MIDI event to a more readable form
        /// </summary>
        /// <param name="castControlToNote">If true the <see cref="Control"/> is cast into a note using <see cref="NoteToString(int)"/></param>
        /// <returns></returns>
        public string ToPrettyString(bool castControlToNote = false) {
            string note = castControlToNote ? MidiNotes.NoteToString(Control) : "" + Control;
            return $"MIDI Event: ({CommandCode} on channel {MidiChannel}, control {note} with value {Value}";
        }
    }
}
