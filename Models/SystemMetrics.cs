using System.Windows.Media;
using System.Linq;

namespace Kil0bitSystemMonitor.Models
{
    public class DiskMetric
    {
        public string Name { get; set; } = "";
        public float SpacePercent { get; set; }
        public float ActivityPercent { get; set; }
    }

    public class SystemMetrics
    {
        public float CpuUsage { get; set; }
        public float CpuTemperature { get; set; } = -1; // °C; -1 = unavailable
        public float RamPercent { get; set; }
        public float GpuUsage { get; set; }
        public float GpuTemperature { get; set; }
        public float VramPercent { get; set; }
        public ulong VramUsedBytes { get; set; }
        public ulong VramTotalBytes { get; set; }
        public float NetUpKbps { get; set; }
        public float NetDownKbps { get; set; }
        public string NetUpText { get; set; } = "0 KB/s";
        public string NetDownText { get; set; } = "0 KB/s";
        public float DiskUsage { get; set; }
        public float DiskPercent { get; set; }
        public System.Collections.Generic.List<DiskMetric> Disks { get; set; } = new();
    }

    public class AppConfig : System.ComponentModel.INotifyPropertyChanged
    {
        private bool _showOverlay = true;
        private bool _lockPosition = false;
        private bool _launchOnStartup = false;
        private bool _showCpu = true;
        private bool _showRam = true;
        private bool _showGpu = true;
        private bool _showTemp = true;
        private bool _showVram = false;
        private bool _showCpuTemp = false; // opt-in: needs administrator rights and the PawnIO driver
        private bool _showDisk = true;
        private bool _showDiskSpeed = true;
        private bool _showNetUp = true;
        private bool _showNetDown = true;
        private string _networkAdapter = "Default";
        private string _gpuAdapter = "Default";
        private string _selectedDisks = "All";
        private string _displayStyle = "Text";
        private string _vramDisplayStyle = "Percent";
        private string _fontFamily = "Segoe UI";
        private string _accentColorHex = "#FFFFFF";
        private string _labelColorHex = "#00CCFF";
        private double _x = 100;
        private double _y = 100;
        private bool _hideOnFullscreen = true;
        private bool _stickToTaskbar = true;
        private bool _showBackground = false;
        private string _backgroundColorHex = "#B4141414";
        private double _scaleFactor = 1.0;
        private bool _isTextBold = true;
        private int _columnSpacing = 6;

        private string _theme = "Default";
        private int _updateInterval = 1000;
        private int _gpuIndex = 0;
        private bool _showPods = true;
        private string _podColorHex = "#0FFFFFFF"; 
        private bool _alwaysOnTop = true;

        // Per-section label colors (null = use global LabelColorHex)
        private string? _netLabelColorHex = null;
        private string? _cpuRamLabelColorHex = null;
        private string? _gpuLabelColorHex = null;
        private string? _diskLabelColorHex = null;

        // Per-section metric/accent colors (null = use global AccentColorHex)
        private string? _netAccentColorHex = null;
        private string? _cpuRamAccentColorHex = null;
        private string? _gpuAccentColorHex = null;
        private string? _diskAccentColorHex = null;
        public bool ShowOverlay { get => _showOverlay; set => SetField(ref _showOverlay, value); }
        public bool LockPosition { get => _lockPosition; set => SetField(ref _lockPosition, value); }
        public bool LaunchOnStartup { get => _launchOnStartup; set => SetField(ref _launchOnStartup, value); }

        public bool ShowCpu { get => _showCpu; set => SetField(ref _showCpu, value); }
        public bool ShowRam { get => _showRam; set => SetField(ref _showRam, value); }
        public bool ShowGpu { get => _showGpu; set => SetField(ref _showGpu, value); }
        public bool ShowTemp { get => _showTemp; set => SetField(ref _showTemp, value); }
        public bool ShowVram { get => _showVram; set => SetField(ref _showVram, value); }
        public bool ShowCpuTemp { get => _showCpuTemp; set => SetField(ref _showCpuTemp, value); }
        public bool ShowDisk { get => _showDisk; set => SetField(ref _showDisk, value); }
        public bool ShowDiskSpeed { get => _showDiskSpeed; set => SetField(ref _showDiskSpeed, value); }
        public bool ShowNetUp { get => _showNetUp; set => SetField(ref _showNetUp, value); }
        public bool ShowNetDown { get => _showNetDown; set => SetField(ref _showNetDown, value); }

        public string NetworkAdapter { get => _networkAdapter; set => SetField(ref _networkAdapter, value); }
        public string GpuAdapter { get => _gpuAdapter; set => SetField(ref _gpuAdapter, value); }
        public string SelectedDisks { get => _selectedDisks; set => SetField(ref _selectedDisks, value); }
        public string DisplayStyle { get => _displayStyle; set => SetField(ref _displayStyle, value); }
        public string VramDisplayStyle { get => _vramDisplayStyle; set => SetField(ref _vramDisplayStyle, value); }
        public string FontFamily { get => _fontFamily; set => SetField(ref _fontFamily, value); }

        public string AccentColorHex { get => _accentColorHex; set { if (SetField(ref _accentColorHex, value)) OnPropertyChanged(nameof(AccentColor)); } }
        public string LabelColorHex { get => _labelColorHex; set { if (SetField(ref _labelColorHex, value)) OnPropertyChanged(nameof(LabelColor)); } }
        public string BackgroundColorHex { get => _backgroundColorHex; set { if (SetField(ref _backgroundColorHex, value)) OnPropertyChanged(nameof(BackgroundColor)); } }

        public double ScaleFactor { get => _scaleFactor; set => SetField(ref _scaleFactor, value); }
        public bool IsTextBold { get => _isTextBold; set => SetField(ref _isTextBold, value); }
        public int ColumnSpacing { get => _columnSpacing; set => SetField(ref _columnSpacing, Math.Clamp(value, 0, 20)); }

        public string Theme { get => _theme; set => SetField(ref _theme, value); }
        public int UpdateInterval { get => _updateInterval; set => SetField(ref _updateInterval, value); }
        public int GpuIndex { get => _gpuIndex; set => SetField(ref _gpuIndex, value); }
        public bool ShowPods { get => _showPods; set => SetField(ref _showPods, value); }
        public string PodColorHex { get => _podColorHex; set { if (SetField(ref _podColorHex, value)) OnPropertyChanged(nameof(PodColor)); } }
        public bool AlwaysOnTop { get => _alwaysOnTop; set => SetField(ref _alwaysOnTop, value); }

        // Per-section label colors (null/empty = inherit global LabelColorHex)
        public string? NetLabelColorHex { get => _netLabelColorHex; set => SetField(ref _netLabelColorHex, value); }
        public string? CpuRamLabelColorHex { get => _cpuRamLabelColorHex; set => SetField(ref _cpuRamLabelColorHex, value); }
        public string? GpuLabelColorHex { get => _gpuLabelColorHex; set => SetField(ref _gpuLabelColorHex, value); }
        public string? DiskLabelColorHex { get => _diskLabelColorHex; set => SetField(ref _diskLabelColorHex, value); }

        // Per-section metric/accent colors (null/empty = inherit global AccentColorHex)
        public string? NetAccentColorHex { get => _netAccentColorHex; set => SetField(ref _netAccentColorHex, value); }
        public string? CpuRamAccentColorHex { get => _cpuRamAccentColorHex; set => SetField(ref _cpuRamAccentColorHex, value); }
        public string? GpuAccentColorHex { get => _gpuAccentColorHex; set => SetField(ref _gpuAccentColorHex, value); }
        public string? DiskAccentColorHex { get => _diskAccentColorHex; set => SetField(ref _diskAccentColorHex, value); }

        public double X { get => _x; set => SetField(ref _x, value); }
        public double Y { get => _y; set => SetField(ref _y, value); }
        public bool HideOnFullscreen { get => _hideOnFullscreen; set => SetField(ref _hideOnFullscreen, value); }
        public bool StickToTaskbar { get => _stickToTaskbar; set => SetField(ref _stickToTaskbar, value); }
        public bool ShowBackground { get => _showBackground; set => SetField(ref _showBackground, value); }

        [System.Text.Json.Serialization.JsonIgnore]
        public System.Windows.Media.Color AccentColor { get => HexToColor(AccentColorHex); set => AccentColorHex = ColorToHex(value); }

        [System.Text.Json.Serialization.JsonIgnore]
        public System.Windows.Media.Color LabelColor { get => HexToColor(LabelColorHex); set => LabelColorHex = ColorToHex(value); }

        [System.Text.Json.Serialization.JsonIgnore]
        public System.Windows.Media.Color BackgroundColor { get => HexToColor(BackgroundColorHex); set => BackgroundColorHex = ColorToHex(value); }

        [System.Text.Json.Serialization.JsonIgnore]
        public System.Windows.Media.Color PodColor { get => HexToColor(PodColorHex); set => PodColorHex = ColorToHex(value); }

        private System.Windows.Media.Color HexToColor(string hex)
        {
            try
            {
                hex = hex.TrimStart('#');
                if (hex.Length == 8) // ARGB
                {
                    return System.Windows.Media.Color.FromArgb(
                        byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber),
                        byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber),
                        byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber),
                        byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber));
                }
                if (hex.Length == 6) // RGB
                {
                    return System.Windows.Media.Color.FromRgb(
                        byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber),
                        byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber),
                        byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber));
                }
            }
            catch { }
            return Colors.White;
        }

        private string ColorToHex(System.Windows.Media.Color c) => $"#{c.A:X2}{c.R:X2}{c.G:X2}{c.B:X2}";

        // Assigning the current value raises no PropertyChanged: every notification re-saves the config and some
        // re-initialise telemetry, so no-op writes (e.g. from TwoWay bindings) must not trigger them.
        private bool SetField<T>(ref T field, T value, [System.Runtime.CompilerServices.CallerMemberName] string? name = null)
        {
            if (System.Collections.Generic.EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(name);
            return true;
        }

        public event System.ComponentModel.PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(name));
        }
    }
}
