using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Cci;
using System.IO;
using System.Diagnostics;
using Microsoft.Cci.MutableCodeModel;

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

            var module = host.LoadUnitFrom(originalAssembly) as IModule;
            if (module == null || module == Dummy.Module || module == Dummy.Assembly)
            {
                Console.WriteLine(originalAssembly + " is not a .NET assembly");
            }

            module = StripToMetadata.Run(host, module);
            using (var peStream = File.Create(metadataAssembly))
            {
                PeWriter.WritePeToStream(module, host, peStream);
            }

            return 0;
        }
    }
}
