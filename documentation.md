# Documentation

This document will contain a quick explanation of how the different classes work.

## MidiListener

The MidiListener class is the main class used to receive MIDI events. You can pass a settings object and a callback to the `StartListening` method. The callback parameter is optional and it can also be set by using the `MidiEventCallback` field. 

## MidiListenerSettings

A struct which has two fields: 

- `int UpdateLoopTimeout` can be used to define how often (in milliseconds) the MIDI devices are updated. Set to 0 to disable device polling. 
- `MidiListenerFilter Filter` which can be used to filter the MIDI devices.

## MidiListenerFilter

A class used to filter MIDI devices. It has the following fields:

- `bool UseAll` When true all devices are used.
- `List<string> DeviceNameFilters` All devices with matching names will be used
- `List<int> DeviceIDFilters` All devices with matching IDs will be used
- `List<int> DeviceNumberFilters` All devices with matching device index numbers will be used
- `List<NAduio.Manufacturers> ManufactureFilters` All devices with matching manufacturers will be used

These conditions will be "OR"ed.

## MidiDevices

This class can be used to retrieve information about connected MIDI devices. Refer to the XML documentation.

## MidiNotes

Tranforms notes from integers to string and from strings to integers. Refer to the XML documentation.

## MidiEvent

Contains the incoming MIDI data and the device which sent it. Has the following fields:

- `MidiInDevice Device`
- `byte StatusByte` Refer to external sources about the MIDI format for more information.
- `byte Control` The corresponding control of the event (e.g. the note or a MIDI CC control)
- `byte Value` The value of the event (e.g. note velocity or MIDI CC value)
- `NAudio.Midi.MidiCommandCode CommandCode` the command code. Refer to NAudio documentation.
- `byte MidiChannel` The channel the event was received on.

### MidiEventFilter

Can be used to filter certain type of MIDI messages based on the above fields.

## Other classes

The other classes are self explanatory.