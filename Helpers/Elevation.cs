using System;
using System.Security.Principal;
using Microsoft.Win32;

namespace Kil0bitSystemMonitor.Helpers
{
    // CPU temperature is read through the PawnIO kernel driver, whose device only SYSTEM and Administrators may
    // open. The overlay therefore runs elevated while CPU temperature is enabled, and as a normal user otherwise.
    public static class Elevation
    {
        public static bool IsElevated { get; } = CheckElevated();

        private static bool CheckElevated()
        {
            try
            {
                using var identity = WindowsIdentity.GetCurrent();
                // With UAC, an admin's filtered (non-elevated) token has the Administrators group as deny-only
                return new WindowsPrincipal(identity).IsInRole(WindowsBuiltInRole.Administrator);
            }
            catch { return false; }
        }

        // PawnIO's installer registers an uninstall entry; LibreHardwareMonitorLib uses the same check
        public static bool IsPawnIoInstalled()
        {
            try
            {
                using var hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                using var key = hklm.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\PawnIO");
                return key != null;
            }
            catch { return false; }
        }
    }
}
