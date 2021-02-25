# EzMidi

Get MIDI events with a single line of code.

## About

This library uses the [NAudio.Midi](https://github.com/naudio/NAudio) to catch MIDI events from MIDI devices. The purpose is to provide an extremely simple interface for receiving MIDI input which makes this tool extremely useful for automation and purposes other than music production. If you need more advanced MIDI features you will need to use another library.

Only receiving MIDI input is included. Sending MIDI events might be included in the future.

## How to use

#### Install package from Nuget

```
install
```

#### Setup callback

```C#
using EzMidi;
public class ExampleCode {
    public static void Main(string[] args) {
        MidiListener.StartListening(MidiListenerSettings.Default, OnMidiEvent);
        Console.ReadLine();
    }

    public static void OnMidiEvent(MidiEvent e) {
        Console.WriteLine(e.ToPrettyString());
    }
}
```

### Documentation

[This document](https://github.com/porrasm/EzMidi/blob/main/documentation.md) explains the usage in more detail.