using System;
using System.IO;

namespace Kil0bitSystemMonitor.Helpers
{
    // A framework-dependent build stays bound to the shared-runtime folder it started from. A .NET servicing
    // update (e.g. via Microsoft Update) installs the new patch version and deletes the old folder even while
    // this long-running process still uses it. Assemblies that are already loaded keep working, but anything
    // loaded later fails with FileNotFoundException, e.g. while parsing the Settings XAML, which then renders
    // an empty window.
    public static class RuntimeHealth
    {
        public static bool WasRuntimeRemoved()
        {
            try
            {
                foreach (var assembly in new[] { typeof(object).Assembly, typeof(System.Windows.Window).Assembly })
                {
                    // Location is empty for single-file publishes, which carry their own runtime
                    string location = assembly.Location;
                    if (!string.IsNullOrEmpty(location) && !File.Exists(location)) return true;
                }
            }
            catch { }
            return false;
        }
    }
}
