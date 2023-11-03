
using System.Runtime.InteropServices;
using System.IO;

namespace Violation_p2.Models;



   
    public static class DllLoader
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr LoadLibrary(string dllToLoad);

        public static void Load()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "libwkhtmltox.dll");
            LoadLibrary(path);
        }
    }


