# Third-party notices

Kil0bit System Monitor is MIT-licensed. The optional CPU temperature feature uses the components below. They are distributed unmodified inside the application binary, and each remains under its own license.

| Component | Version | License | Source |
|---|---|---|---|
| LibreHardwareMonitorLib | 0.9.6 | MPL-2.0 | https://github.com/LibreHardwareMonitor/LibreHardwareMonitor |
| DiskInfoToolkit (LibreHardwareMonitorLib dependency) | 1.1.2 | MPL-2.0 | https://github.com/Blacktempel/DiskInfoToolkit |
| RAMSPDToolkit-NDD (LibreHardwareMonitorLib dependency) | 1.4.2 | MPL-2.0 | https://github.com/Blacktempel/RAMSPDToolkit |
| HidSharp (LibreHardwareMonitorLib dependency) | 2.6.4 | Apache-2.0 | https://www.zer7.com/software/hidsharp |

The source code of the MPL-2.0 components is available at the repositories listed above, as MPL-2.0 section 3.2 requires.

LibreHardwareMonitorLib embeds signed PawnIO modules (https://github.com/namazso/PawnIO.Modules). The PawnIO driver itself is **not** distributed with this application. CPU temperature works only when the user has installed it separately, for example from https://pawnio.eu or through FanControl or LibreHardwareMonitor.
