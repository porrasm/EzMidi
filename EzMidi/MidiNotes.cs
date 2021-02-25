using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EzMidi {
    public static class MidiNotes {

        /// <summary>
        /// Converts a given note value in the range [0, 127] to a string
        /// </summary>
        /// <param name="note"></param>
        /// <returns></returns>
        public static string NoteToString(int note) {
            if (note < 0 || note > 127) {
                throw new Exception("Invalid note");
            }
            int n = note % 12;
            int octave = note / 12 - 1;
            return NoteIndexToString(n) + octave;
        }

        private static string NoteIndexToString(int note) {
            switch (note) {
                case 0:
                    return "C";
                case 1:
                    return "C#";
                case 2:
                    return "D";
                case 3:
                    return "D#";
                case 4:
                    return "E";
                case 5:
                    return "F";
                case 6:
                    return "F#";
                case 7:
                    return "G";
                case 8:
                    return "G#";
                case 9:
                    return "A";
                case 10:
                    return "A#";
                case 11:
                    return "B";
            }
            throw new Exception("Invalid note");
        }

        /// <summary>
        /// Converts a string to a note. Only the sharp (#) sign is allowed.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static byte StringToNote(string s) {
            s = s.ToLower();
            if (!new Regex(@"[a-g]#?-?\d").IsMatch(s)) {
                throw new Exception("Invalid note: (" + s + ") with length: " + s.Length);
            }
            int splitIndex = s[1] == '#' ? 2 : 1;

            int octave = int.Parse(s.Substring(splitIndex));
            string note = s.Substring(0, splitIndex);

            return (byte)((octave + 1) * 12 + StringToNoteIndex(note));
        }

        private static int StringToNoteIndex(string note) {
            switch (note) {
                case "c":
                    return 0;
                case "c#":
                    return 1;
                case "d":
                    return 2;
                case "d#":
                    return 3;
                case "e":
                    return 4;
                case "f":
                    return 5;
                case "f#":
                    return 6;
                case "g":
                    return 7;
                case "g#":
                    return 8;
                case "a":
                    return 9;
                case "a#":
                    return 10;
                case "b":
                    return 11;
                default:
                    throw new Exception("Invalid note: " + note);
            }
        }
    }
}
