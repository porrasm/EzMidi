using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzMidi {
    /// <summary>
    /// Contains a collection of filters to use with <see cref="MidiListener"/>
    /// </summary>
    public class MidiListenerFilter {

        /// <summary>
        /// Use all devices, ignoring all filter options
        /// </summary>
        public bool UseAll = false;

        /// <summary>
        /// Use all devices with a matching name (device name and filter are lowercased and the deviceName.Contains(filter) is used)
        /// </summary>
        public List<string> DeviceNameFilters { get; private set; }

        /// <summary>
        /// Use all devics with matchign IDs
        /// </summary>
        public List<int> DeviceIDFilters { get; private set; }

        /// <summary>
        /// Use all devices with matching device numbers
        /// </summary>
        public List<int> DeviceNumberFilters { get; private set; }

        /// <summary>
        /// Use all devices from matching manufacturers
        /// </summary>
        public List<NAudio.Manufacturers> ManufactureFilters { get; private set; }

        #region constructors
        /// <summary>
        /// Creates a new <see cref="MidiListenerFilter"/> instance with no filter options
        /// </summary>
        public MidiListenerFilter() {
            ResetFilters();
        }

        /// <summary>
        /// Creates a new <see cref="MidiListenerFilter"/> instance with <see cref="UseAll"/> set to true
        /// </summary>
        public static MidiListenerFilter UseAllDevices() {
            MidiListenerFilter filter = new MidiListenerFilter();
            filter.UseAll = true;
            return filter;
        }

        /// <summary>
        /// Resets all filters
        /// </summary>
        public void ResetFilters() {
            UseAll = false;
            DeviceNameFilters = new List<string>();
            DeviceIDFilters = new List<int>();
            DeviceNumberFilters = new List<int>();
            ManufactureFilters = new List<NAudio.Manufacturers>();
        }
        #endregion
    }
}
