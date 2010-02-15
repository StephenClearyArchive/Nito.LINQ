using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Cci;

namespace assimilate
{
    public static class Platforms
    {
        public static IEnumerable<string> None
        {
            get
            {
                return Enumerable.Empty<string>();
            }
        }

        public static IEnumerable<string> Default
        {
            get
            {
                yield return Path.GetDirectoryName(Path.GetFullPath(typeof(object).Assembly.GetName().GetLocalPath()));
            }
        }

        public static IEnumerable<string> Desktop20
        {
            get
            {
                yield return ReferenceAssembliesDirectory.Desktop20Directory;
            }
        }

        public static IEnumerable<string> Desktop30
        {
            get
            {
                yield return ReferenceAssembliesDirectory.Desktop30Directory;
                yield return ReferenceAssembliesDirectory.Desktop20Directory;
            }
        }

        public static IEnumerable<string> Desktop35
        {
            get
            {
                yield return ReferenceAssembliesDirectory.Desktop35Directory;
                yield return ReferenceAssembliesDirectory.Desktop30Directory;
                yield return ReferenceAssembliesDirectory.Desktop20Directory;
            }
        }

        public static IEnumerable<string> Compact20
        {
            get
            {
                yield return ReferenceAssembliesDirectory.Compact20Directory;
            }
        }

        public static IEnumerable<string> Compact35
        {
            get
            {
                yield return ReferenceAssembliesDirectory.Compact35Directory;
            }
        }

        public static IEnumerable<string> Silverlight30
        {
            get
            {
                yield return ReferenceAssembliesDirectory.Silverlight30Directory;
            }
        }

        public static IEnumerable<string> Micro40
        {
            get
            {
                yield return ReferenceAssembliesDirectory.Micro40Directory;
            }
        }
    }
}
