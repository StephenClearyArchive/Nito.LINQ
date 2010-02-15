using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Cci;
using System.IO;

namespace assimilate
{
    public sealed class Host : MetadataReaderHost
    {
        private PeReader peReader;

        public Host(IEnumerable<string> referenceDirectories)
            : base(referenceDirectories)
        {
            this.peReader = new PeReader(this);
        }

        public Host(IEnumerable<string> referenceDirectories, INameTable nameTable)
            : base(nameTable)
        {
            this.LibPaths.AddRange(referenceDirectories);
            this.peReader = new PeReader(this);
        }

        public override IUnit LoadUnitFrom(string location)
        {
            IUnit result = this.peReader.OpenModule(
              BinaryDocument.GetBinaryDocumentForFile(location, this));
            this.RegisterAsLatest(result);
            return result;
        }

        /// <summary>
        /// Given the identity of a referenced assembly (but not its location), apply host specific policies for finding the location
        /// of the referenced assembly.
        /// Returns an assembly identity that matches the given referenced assembly identity, but which includes a location.
        /// If the probe failed to find the location of the referenced assembly, the location will be "unknown://location".
        /// </summary>
        /// <param name="referringUnit">The unit that is referencing the assembly. It will have been loaded from somewhere and thus
        /// has a known location, which will typically be probed for the referenced assembly.</param>
        /// <param name="referencedAssembly">The assembly being referenced. This will not have a location since there is no point in probing
        /// for the location of an assembly when you already know its location.</param>
        /// <returns>
        /// An assembly identity that matches the given referenced assembly identity, but which includes a location.
        /// If the probe failed to find the location of the referenced assembly, the location will be "unknown://location".
        /// </returns>
        /// <remarks>
        /// Looks for the referenced assembly first in the same directory as the referring unit, then
        /// in any search paths provided to the constructor, then finally the GAC.
        /// </remarks>
        public override AssemblyIdentity ProbeAssemblyReference(IUnit referringUnit, AssemblyIdentity referencedAssembly)
        {
            // probe for in the same directory as the referring unit
            var referringDir = Path.GetDirectoryName(Path.GetFullPath(referringUnit.Location));
            AssemblyIdentity result = this.Probe(referringDir, referencedAssembly);
            if (result != null) return result;

            // Probe in the libPaths directories
            foreach (string libPath in this.LibPaths)
            {
                result = this.Probe(libPath, referencedAssembly);
                if (result != null) return result;
            }

            // Give up
            return new AssemblyIdentity(referencedAssembly, "unknown://location");
        }

        protected override AssemblyIdentity GetCoreAssemblySymbolicIdentity()
        {
            var coreAssemblyName = typeof(object).Assembly.GetName();

            AssemblyIdentity coreAssembly = new AssemblyIdentity(this.NameTable.GetNameFor(coreAssemblyName.Name), "", coreAssemblyName.Version, coreAssemblyName.GetPublicKeyToken(), string.Empty);

            // Probe in the libPaths directories
            foreach (string libPath in this.LibPaths)
            {
                AssemblyIdentity ret = this.Probe(libPath, coreAssembly);
                if (ret != null) return ret;
            }

            // Give up
            return new AssemblyIdentity(coreAssembly, "unknown://location");
        }

        /// <summary>
        /// Looks in the specified <paramref name="probeDir"/> to see if a file
        /// exists, first with the extension "dll" and then with the extension "exe".
        /// Returns null if not found, otherwise constructs a new AssemblyIdentity
        /// </summary>
        private AssemblyIdentity/*?*/ Probe(string probeDir, AssemblyIdentity referencedAssembly)
        {
            string path = Path.Combine(probeDir, referencedAssembly.Name.Value + ".dll");
            if (File.Exists(path)) return new AssemblyIdentity(referencedAssembly, path);
            path = Path.Combine(probeDir, referencedAssembly.Name.Value + ".exe");
            if (File.Exists(path)) return new AssemblyIdentity(referencedAssembly, path);
            return null;
        }
    }
}
