using NAudio.Midi;
using System.Collections.Generic;

namespace EzMidi {
    /// <summary>
    /// A filter for <see cref="MidiEvent"/>s which can be used to accept matching events
    /// </summary>
    public class MidiEventFilter {
        #region fields

        /// <summary>
        /// Set of accepted command codes
        /// </summary>
        public HashSet<MidiCommandCode> AcceptedCommandCodes { get; set; }

        /// <summary>
        /// Set of accepted control values
        /// </summary>
        public HashSet<byte> AcceptedControlValues { get; set; }

        /// <summary>
        /// Bitfield of accepted MIDI channels
        /// </summary>
        public ushort AcceptedMidiChannels { get; set; }
        #endregion

        /// <summary>
        /// Creates a new <see cref="MidiEventFilter"/> instance with no filter options
        /// </summary>
        /// <param name="callback"></param>
        public MidiEventFilter() {
            AcceptedCommandCodes = new HashSet<MidiCommandCode>();
            AcceptedControlValues = new HashSet<byte>();
            AcceptedMidiChannels = 0;
        }

        /// <summary>
        /// Returns true if the filters of this instance match the incoming MIDI event
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public bool IsMatch(MidiEvent e) {
            if (AcceptedCommandCodes.Count > 0 && !AcceptedCommandCodes.Contains(e.CommandCode)) {
                return false;
            }
            if (AcceptedMidiChannels != 0 && (e.MidiChannel & AcceptedMidiChannels) == 0) {
                return false;
            }
            if (AcceptedControlValues.Count > 0 && !AcceptedControlValues.Contains(e.Control)) {
                return false;
            }

            return true;
        }

        #region filters
        /// <summary>
        /// Adds a new <see cref="MidiCommandCode"/> filter
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public MidiEventFilter AcceptCommandCode(MidiCommandCode c) {
            AcceptedCommandCodes.Add(c);
            return this;
        }

        /// <summary>
        /// Adds a new MIDI control code filter
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public MidiEventFilter AcceptControl(byte c) {
            AcceptedControlValues.Add(c);
            return this;
        }

        /// <summary>
        /// Sets the accepted MIDI channels. Set 0 to allow all.
        /// </summary>
        /// <param name="channels"></param>
        /// <returns></returns>
        public MidiEventFilter SetMidiChannels(ushort channels) {
            this.AcceptedMidiChannels = channels;
            return this;
        }
        #endregion
    }
}
