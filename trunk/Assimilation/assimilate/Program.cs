using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Cci;
using System.IO;
using System.Diagnostics;
using Microsoft.Cci.MutableCodeModel;
using System.Xml.Linq;
using System.Xml.XPath;

namespace assimilate
{
    class Program
    {
        static int Main(string[] args)
        {
            if (Debugger.IsAttached)
            {
                return ProgramMain(args);
            }
            else
            {
                try
                {
                    return ProgramMain(args);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: [" + ex.GetType().Name + "] " + ex.Message + ex.StackTrace);
                    return -1;
                }
            }
        }

        static int ProgramMain(string[] args)
        {
            if (args == null || args.Length == 0)
            {
                return Usage();
            }

            switch (args[0])
            {
                case "meta":
                    if (args.Length != 3)
                    {
                        return Usage();
                    }

                    return GenerateMetadataAssembly(args[1], args[2]);

                default:
                    return Usage();
            }
        }

        static int Usage()
        {
            Console.WriteLine("Usage: assimilate <command> <arguments>");
            Console.WriteLine("Commands:");
            Console.WriteLine("  meta <existing assembly> <new assembly>");
            Console.WriteLine("    Generates a metadata assembly from an existing assembly");
            return -1;
        }

        static int GenerateMetadataAssembly(string originalAssembly, string metadataAssembly)
        {
            var host = new PeReader.DefaultHost();

            var assembly = host.LoadUnitFrom(originalAssembly) as IAssembly;
            if (assembly == null || assembly == Dummy.Module || assembly == Dummy.Assembly)
            {
                Console.WriteLine(originalAssembly + " is not a .NET assembly");
            }

            assembly = StripToMetadata.Run(host, assembly);
            using (var peStream = File.Create(metadataAssembly))
            {
                PeWriter.WritePeToStream(assembly, host, peStream);
            }

            string xmlFileName = Path.ChangeExtension(originalAssembly, "xml");
            string newXmlFileName = Path.ChangeExtension(metadataAssembly, "xml");
            if (File.Exists(xmlFileName))
            {
                RedirectToXmlDoc(xmlFileName, newXmlFileName);
            }
            else
            {
                string originalAssemblyDirectory = Path.GetDirectoryName(originalAssembly);
                string originalAssemblyFileName = Path.GetFileName(originalAssembly);
                foreach (var subdir in Directory.GetDirectories(originalAssemblyDirectory))
                {
                    string localizedXmlFileName = Path.ChangeExtension(Path.Combine(subdir, originalAssemblyFileName), "xml");
                    if (File.Exists(localizedXmlFileName))
                    {
                        RedirectToXmlDoc(localizedXmlFileName, newXmlFileName);
                        break;
                    }
                }
            }

            return 0;
        }

        private static void RedirectToXmlDoc(string xmlFileName, string newXmlFileName)
        {
            // Visual Studio (as of 2008) cannot redirect through a redirected xml doc file, so we just have to copy it if it is already redirected
            if (XDocument.Load(xmlFileName).Root.Attributes("redirect").FirstOrDefault() == null)
            {
                new XElement("doc", new XAttribute("redirect", Path.GetFullPath(xmlFileName))).Save(newXmlFileName);
            }
            else
            {
                File.Copy(xmlFileName, newXmlFileName, true);
            }
        }
    }
}
