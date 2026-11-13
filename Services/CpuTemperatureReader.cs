using System;
using System.Linq;
using LibreHardwareMonitor.Hardware;

namespace Kil0bitSystemMonitor.Services
{
    // Reads the CPU package/die temperature with LibreHardwareMonitorLib, which talks to an installed PawnIO
    // driver (e.g. the one FanControl or LibreHardwareMonitor installs). Only the CPU group is enabled, so no
    // other hardware (disks, SMBus, embedded controllers) is probed. Requires an elevated process.
    public sealed class CpuTemperatureReader : IDisposable
    {
        // Most representative sensor first: AMD Zen reports Tctl/Tdie, Intel reports the package temperature
        private static readonly string[] PreferredSensors = { "Core (Tctl/Tdie)", "CPU Package", "Core (Tctl)", "Core (Tdie)", "Core Max" };

        private readonly Computer _computer;
        private readonly IHardware? _cpu;
        private ISensor? _sensor;

        public CpuTemperatureReader()
        {
            _computer = new Computer { IsCpuEnabled = true };
            _computer.Open();
            _cpu = _computer.Hardware.FirstOrDefault(h => h.HardwareType == HardwareType.Cpu);
        }

        // °C, or -1 when no plausible reading is available
        public float Read()
        {
            if (_cpu == null) return -1;
            _cpu.Update();
            if (_sensor?.Value == null) _sensor = PickSensor(_cpu);
            float? value = _sensor?.Value;
            return value is > 0 and < 150 ? value.Value : -1;
        }

        private static ISensor? PickSensor(IHardware cpu)
        {
            var temperatures = cpu.Sensors.Where(s => s.SensorType == SensorType.Temperature && s.Value != null).ToList();
            foreach (var name in PreferredSensors)
            {
                var match = temperatures.FirstOrDefault(s => s.Name == name);
                if (match != null) return match;
            }
            return temperatures.OrderByDescending(s => s.Value).FirstOrDefault();
        }

        public void Dispose()
        {
            try { _computer.Close(); } catch { }
        }
    }
}
