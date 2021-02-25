using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using EzMidi;

namespace EzMidiTests {
    [TestClass]
    public class MidiNotesTest {
        [TestMethod]
        public void TestAll() {
            for (int i = 0; i < 128; i++) {
                string noteString = MidiNotes.NoteToString(i);

                Console.WriteLine($"{i} = {noteString}");

                int note = MidiNotes.StringToNote(noteString);
                Assert.AreEqual(i, note);
            }
        }
    }
}
