using EzMidi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzMidiTests {
    public class ExampleCode {
        public static void Main(string[] args) {
            MidiListener.StartListening(MidiListenerSettings.Default, OnMidiEvent);
            Console.ReadLine();
        }

        public static void OnMidiEvent(MidiEvent e) {
            Console.WriteLine(e.ToPrettyString());
        }
    }
}
