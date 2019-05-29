using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace RallyXtreme_launcher
{
    class getDirectory
    {
        public static string getExecutable()
        {
            string directory;
            // 70 characters remove 
            directory = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            int len = (directory.Length - 61);
            System.Console.WriteLine($"#launcherGETEXE# Len = {len}.  Found current process as {directory}");
            string finalDirectory = directory.Substring(0, len);
            

            finalDirectory = finalDirectory + "\\Game\\RallyXtreme\\RallyXtreme\\bin\\DesktopGL\\AnyCPU\\Debug\\RallyXtreme.exe";
            System.Console.WriteLine($"#launcherGETEXE# Truncated dir = {finalDirectory}");
            return finalDirectory;
        }
    }
}
