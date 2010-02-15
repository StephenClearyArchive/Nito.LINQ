using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.IO;

namespace assimilate
{
    public static class ReferenceAssembliesDirectory
    {
        private static string programFiles86;

        private static string desktop20Directory;

        private static string desktop30Directory;

        private static string desktop35Directory;

        private static string compact20Directory;

        private static string compact35Directory;

        private static string silverlight30Directory;

        private static string micro40Directory;

        /// <summary>
        /// Gets the 32-bit Program Files directory.
        /// </summary>
        public static string ProgramFiles86
        {
            get
            {
                if (programFiles86 == null)
                {
                    if (IntPtr.Size == 4)
                    {
                        programFiles86 = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
                    }
                    else
                    {
                        programFiles86 = Environment.GetEnvironmentVariable("ProgramFiles(x86)");
                    }
                }

                return programFiles86;
            }
        }

        /// <summary>
        /// Gets the location of the Desktop .NET 2.0 reference assemblies.
        /// </summary>
        public static string Desktop20Directory
        {
            get
            {
                if (desktop20Directory == null)
                {
                    desktop20Directory = GetDesktop20Directory();
                    if (desktop20Directory == null)
                    {
                        throw new InvalidOperationException("Could not find Desktop 2.0 reference assemblies.");
                    }
                }

                return desktop20Directory;
            }
        }

        /// <summary>
        /// Gets the location of the Desktop .NET 3.5 reference assemblies.
        /// </summary>
        public static string Desktop30Directory
        {
            get
            {
                // Uses location documented here: http://blogs.msdn.com/msbuild/archive/2007/04/12/new-reference-assemblies-location.aspx
                if (desktop30Directory == null)
                {
                    desktop30Directory = Path.Combine(ProgramFiles86, @"Reference Assemblies\Microsoft\Framework\v3.0");
                    if (!File.Exists(Path.Combine(desktop30Directory, "WindowsBase.dll")))
                    {
                        throw new InvalidOperationException("Could not find Desktop 3.0 reference assemblies.");
                    }
                }

                return desktop30Directory;
            }
        }

        /// <summary>
        /// Gets the location of the Desktop .NET 3.5 reference assemblies.
        /// </summary>
        public static string Desktop35Directory
        {
            get
            {
                // Uses location documented here: http://blogs.msdn.com/msbuild/archive/2007/04/12/new-reference-assemblies-location.aspx
                if (desktop35Directory == null)
                {
                    desktop35Directory = Path.Combine(ProgramFiles86, @"Reference Assemblies\Microsoft\Framework\v3.5");
                    if (!File.Exists(Path.Combine(desktop35Directory, "System.Core.dll")))
                    {
                        throw new InvalidOperationException("Could not find Desktop 3.5 reference assemblies.");
                    }
                }

                return desktop35Directory;
            }
        }

        /// <summary>
        /// Gets the location of the Compact Framework 2.0 reference assemblies.
        /// </summary>
        public static string Compact20Directory
        {
            get
            {
                if (compact20Directory == null)
                {
                    compact20Directory = Path.Combine(ProgramFiles86, @"Microsoft.NET\SDK\CompactFramework\v2.0\WindowsCE");
                    if (!File.Exists(Path.Combine(compact20Directory, "mscorlib.dll")))
                    {
                        throw new InvalidOperationException("Could not find Compact 2.0 reference assemblies.");
                    }
                }

                return compact20Directory;
            }
        }

        /// <summary>
        /// Gets the location of the Compact Framework 3.5 reference assemblies.
        /// </summary>
        public static string Compact35Directory
        {
            get
            {
                if (compact35Directory == null)
                {
                    compact35Directory = Path.Combine(ProgramFiles86, @"Microsoft.NET\SDK\CompactFramework\v3.5\WindowsCE");
                    if (!File.Exists(Path.Combine(compact35Directory, "mscorlib.dll")))
                    {
                        throw new InvalidOperationException("Could not find Compact 3.5 reference assemblies.");
                    }
                }

                return compact35Directory;
            }
        }

        /// <summary>
        /// Gets the location of the Silverlight 3.0 reference assemblies.
        /// </summary>
        public static string Silverlight30Directory
        {
            get
            {
                if (silverlight30Directory == null)
                {
                    silverlight30Directory = Path.Combine(ProgramFiles86, @"Reference Assemblies\Microsoft\Framework\Silverlight\v3.0");
                    if (!File.Exists(Path.Combine(silverlight30Directory, "mscorlib.dll")))
                    {
                        throw new InvalidOperationException("Could not find Silverlight 3.0 reference assemblies.");
                    }
                }

                return silverlight30Directory;
            }
        }

        /// <summary>
        /// Gets the location of the Micro 4.0 reference assemblies.
        /// </summary>
        public static string Micro40Directory
        {
            get
            {
                if (micro40Directory == null)
                {
                    micro40Directory = Path.Combine(ProgramFiles86, @"Microsoft .NET Micro Framework\v4.0\Assemblies");
                    if (!File.Exists(Path.Combine(micro40Directory, "mscorlib.dll")))
                    {
                        throw new InvalidOperationException("Could not find Micro 4.0 reference assemblies.");
                    }
                }

                return micro40Directory;
            }
        }

        private static string GetDesktop20Directory()
        {
            // Uses the search recommended by: http://blogs.msdn.com/robvi/archive/2004/02/17/75272.aspx
            using (RegistryKey reg = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\.NETFramework"))
            {
                if (reg == null)
                {
                    return null;
                }

                string installRoot = reg.GetValue("InstallRoot") as string;
                if (string.IsNullOrEmpty(installRoot))
                {
                    return null;
                }

                using (RegistryKey policies = reg.OpenSubKey(@"policy\v2.0"))
                {
                    if (policies == null)
                    {
                        return null;
                    }

                    var values = policies.GetValueNames().ToList();
                    values.Sort((x, y) => StringComparer.InvariantCulture.Compare(y, x));
                    foreach (var value in values)
                    {
                        var testDir = Path.Combine(installRoot, "v2.0." + value);
                        if (File.Exists(Path.Combine(testDir, "mscorlib.dll")))
                        {
                            return testDir;
                        }
                    }

                    return null;
                }
            }
        }
    }
}
