﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Cci;
using System.IO;
using System.Diagnostics;
using Microsoft.Cci.MutableCodeModel;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Xml;
using System.Text.RegularExpressions;
using System.Globalization;

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
                    if (args.Length < 3 || args.Length % 2 != 1)
                    {
                        return Usage();
                    }

                    return GenerateMetadataAssembly(args[1], args[2], ParseOptions(args, 3));

                case "dump":
                    if (args.Length != 3)
                    {
                        return Usage();
                    }

                    return DumpAssembly(args[1], args[2]);

                case "find":
                    if (args.Length < 3 || args.Length % 2 != 1)
                    {
                        return Usage();
                    }

                    return Find(args[1], args[2], ParseOptions(args, 3));

                case "merge":
                    if (args.Length < 4)
                    {
                        return Usage();
                    }

                    return MergeAssembly(args[1], new List<string>(args.Skip(2)));

                default:
                    return Usage();
            }
        }

        static Dictionary<string, string> ParseOptions(IList<string> source, int index)
        {
            int count = source.Count - index;
            if (count % 2 != 0)
            {
                throw new InvalidOperationException("Options may only be parsed for an even number of strings");
            }

            Dictionary<string, string> ret = new Dictionary<string, string>();
            for (int i = index; i < source.Count; i += 2)
            {
                ret.Add(source[i], source[i + 1]);
            }

            return ret;
        }

        static int Usage()
        {
            //                 Default console width:
            //                 12345678901234567890123456789012345678901234567890123456789012345678901234567890
            Console.WriteLine("Usage: assimilate <command> <arguments>");
            Console.WriteLine("Commands:");
            Console.WriteLine(" meta <existing assembly> <new assembly> [optional parameters]");
            Console.WriteLine("  Generates a metadata assembly from an existing assembly");
            Console.WriteLine("  -loc [standard location of reference assembly]");
            Console.WriteLine("   Desktop20, Desktop30, Desktop35, Compact20, Compact35, Silverlight30, or");
            Console.WriteLine("   Micro40");
            Console.WriteLine(" merge <output assembly> <existing assemblies>");
            Console.WriteLine("  Merges existing metadata assemblies into an output assembly");
            Console.WriteLine(" find <existing assembly> <regex> [optional parameters]");
            Console.WriteLine("  Searches metadata in an existing assembly");
            Console.WriteLine("  -loc [location(s)]");
            Console.WriteLine("   NamespaceReference - entities referencing a matching namespace are found");
            Console.WriteLine("   NamespaceDefinition - matching namespaces are found");
            Console.WriteLine("   TypeReference - entities referencing a matching type are found");
            Console.WriteLine("   TypeDefinition - matching types are found (*)");
            Console.WriteLine("   MemberReference - entities referencing a matching type member are found");
            Console.WriteLine("   MemberDefinition - matching type members are found (*)");
            Console.WriteLine("   Namespace = NamespaceReference,NamespaceDefinition");
            Console.WriteLine("   Type = TypeReference,TypeDefinition");
            Console.WriteLine("   Member = MemberReference,MemberDefinition");
            Console.WriteLine("   Reference = NamespaceReference,TypeReference,MemberReference");
            Console.WriteLine("   Definition = NamespaceDefinition,TypeDefinition,MemberDefinition");
            Console.WriteLine("   All = Reference,Definition");
            Console.WriteLine("  -reopt [regular expression option(s)]");
            Console.WriteLine("   None (*), IgnoreCase, CultureInvariant, etc.");
            Console.WriteLine("  -findformat [formatting option(s)]");
            Console.WriteLine("   Each metadata entity is first converted to a string using these options.");
            Console.WriteLine("   None - remove default formatting");
            Console.WriteLine("   EmptyTypeParameterList - include type parameter lists even if empty (*)");
            Console.WriteLine("   MethodConstraints - include type constraints of generic methods");
            Console.WriteLine("   Modifiers - include modifiers, e.g., \"static\"");
            Console.WriteLine("   ParameterName - include names of parameters");
            Console.WriteLine("   ParameterModifiers - include parameter modifiers, e.g., \"ref\"");
            Console.WriteLine("   ReturnType - include return type in signatures");
            Console.WriteLine("   Signature - include parameter types, and names if necessary");
            Console.WriteLine("   TypeConstraints - include type parameter constraints");
            Console.WriteLine("   TypeParameters - include type parameter names");
            Console.WriteLine("   Visibility - include visibility, e.g., \"public\"");

            Console.WriteLine("   MemberKind - include the entity kind as a prefix, e.g., \"class\"");
            Console.WriteLine("   UseGenericTypeNameSuffix - include the number of type parameters as a");
            Console.WriteLine("     suffix, e.g., \"`1\"");

            Console.WriteLine("   OmitContainingNamespace - exclude containing namespace, for types (*)");
            Console.WriteLine("   OmitContainingType - exclude containing type, for members (*)");
            Console.WriteLine("   OmitCustomModifiers - exclude optional and required modifiers (*)");
            Console.WriteLine("   OmitImplementedInterface - exclude interface name when explicitly impl.");
            Console.WriteLine("   OmitTypeArguments - exclude type argument names");
            Console.WriteLine("   OmitWhiteSpaceAfterListDelimiter - exclude spaces from lists");
            Console.WriteLine("   SupressAttributeSuffix - exclude \"Attribute\" suffix for attributes");

            Console.WriteLine("   ContractNullable - use T? instead of System.Nullable<T> (*)");
            Console.WriteLine("   DocumentationId - unique string, same as XML documentation files");
            Console.WriteLine("     DocumentationId (if specified) should be used with no other format options");
            Console.WriteLine("   EscapeKeyword - use \"@if\" instead of \"if\" (*)");
            Console.WriteLine("   PreserveSpecialNames - do not translate special names, e.g., \".ctor\"");
            Console.WriteLine("   UseReflectionStyleForNestedTypeNames - use \"+\" instead of \".\" for");
            Console.WriteLine("     nested types");
            Console.WriteLine("   UseTypeKeywords - e.g., use \"int\" instead of \"System.Int32\"");

            Console.WriteLine("  -outputformat [formatting option(s)]");
            Console.WriteLine("   Found entities are displayed using these options.");
            Console.WriteLine("   The default is EmptyTypeParameterList,ParameterName,ReturnType,Signature,");
            Console.WriteLine("     MemberKind,OmitCustomModifiers,ContractNullable,EscapeKeyword,");
            Console.WriteLine("     UseTypeKeywords");
            Console.WriteLine("(*) - Default parameter value");
            Console.WriteLine("When passing multiple parameters to an option, separate them with a comma.");

            return -1;
        }

        static int GenerateMetadataAssembly(string originalAssembly, string metadataAssembly, Dictionary<string, string> options)
        {
            string loc = string.Empty;

            foreach (var option in options)
            {
                switch (option.Key)
                {
                    case "-loc":
                        if (string.Equals(option.Value, "Desktop20", StringComparison.InvariantCultureIgnoreCase))
                        {
                            loc = ReferenceAssembliesDirectory.Desktop20Directory;
                            if (loc == string.Empty)
                            {
                                throw new DirectoryNotFoundException("Could not find .NET Framework 2.0 reference assembly directory.");
                            }
                        }
                        else if (string.Equals(option.Value, "Desktop30", StringComparison.InvariantCultureIgnoreCase))
                        {
                            loc = ReferenceAssembliesDirectory.Desktop30Directory;
                            if (loc == string.Empty)
                            {
                                throw new DirectoryNotFoundException("Could not find .NET Framework 3.0 reference assembly directory.");
                            }
                        }
                        else if (string.Equals(option.Value, "Desktop35", StringComparison.InvariantCultureIgnoreCase))
                        {
                            loc = ReferenceAssembliesDirectory.Desktop35Directory;
                            if (loc == string.Empty)
                            {
                                throw new DirectoryNotFoundException("Could not find .NET Framework 3.5 reference assembly directory.");
                            }
                        }
                        else if (string.Equals(option.Value, "Compact20", StringComparison.InvariantCultureIgnoreCase))
                        {
                            loc = ReferenceAssembliesDirectory.Compact20Directory;
                            if (loc == string.Empty)
                            {
                                throw new DirectoryNotFoundException("Could not find .NET Compact Framework 2.0 reference assembly directory.");
                            }
                        }
                        else if (string.Equals(option.Value, "Compact35", StringComparison.InvariantCultureIgnoreCase))
                        {
                            loc = ReferenceAssembliesDirectory.Compact35Directory;
                            if (loc == string.Empty)
                            {
                                throw new DirectoryNotFoundException("Could not find .NET Compact Framework 3.5 reference assembly directory.");
                            }
                        }
                        else if (string.Equals(option.Value, "Silverlight30", StringComparison.InvariantCultureIgnoreCase))
                        {
                            loc = ReferenceAssembliesDirectory.Silverlight30Directory;
                            if (loc == string.Empty)
                            {
                                throw new DirectoryNotFoundException("Could not find Silverlight 3.0 reference assembly directory.");
                            }
                        }
                        else if (string.Equals(option.Value, "Micro40", StringComparison.InvariantCultureIgnoreCase))
                        {
                            loc = ReferenceAssembliesDirectory.Micro40Directory;
                            if (loc == string.Empty)
                            {
                                throw new DirectoryNotFoundException("Could not find .NET Micro Framework 4.0 reference assembly directory.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Could not parse -loc parameter: " + option.Value);
                            return Usage();
                        }
                        break;
                }
            }

            if (loc != string.Empty)
            {
                originalAssembly = Path.Combine(loc, originalAssembly);
            }

            var host = new Host(Platforms.Default);
            var assembly = host.LoadUnitFrom(originalAssembly) as IAssembly;
            if (assembly == null || assembly == Dummy.Module || assembly == Dummy.Assembly)
            {
                throw new InvalidOperationException(originalAssembly + " is not a .NET assembly");
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

        static int DumpAssembly(string assemblyFileName, string xmlFileName)
        {
            var host = new Host(Platforms.Default);

            var assembly = host.LoadUnitFrom(assemblyFileName) as IAssembly;
            if (assembly == null || assembly == Dummy.Module || assembly == Dummy.Assembly)
            {
                throw new InvalidOperationException(assemblyFileName + " is not a .NET assembly");
            }

            using (XmlWriter writer = XmlWriter.Create(xmlFileName, new XmlWriterSettings { Indent = true }))
            {
                assembly = new MetadataMutator(host).Visit(assembly);
                assembly = new DumpMutator(host, writer).Visit(assembly);
            }

            return 0;
        }

        static int Find(string assemblyFileName, string regex, Dictionary<string, string> options)
        {
            var loc = FindTraverser.SearchLocation.TypeDefinition | FindTraverser.SearchLocation.MemberDefinition;
            var reopt = RegexOptions.None;
            var findformat = NameFormattingOptions.EmptyTypeParameterList | NameFormattingOptions.OmitContainingNamespace | NameFormattingOptions.OmitContainingType |
                NameFormattingOptions.OmitCustomModifiers | NameFormattingOptions.ContractNullable | NameFormattingOptions.EscapeKeyword;
            var outputformat = NameFormattingOptions.EmptyTypeParameterList | NameFormattingOptions.ParameterName | NameFormattingOptions.ReturnType | NameFormattingOptions.Signature |
                NameFormattingOptions.MemberKind | NameFormattingOptions.OmitCustomModifiers | NameFormattingOptions.ContractNullable | NameFormattingOptions.EscapeKeyword |
                NameFormattingOptions.UseTypeKeywords;

            foreach (var option in options)
            {
                switch (option.Key)
                {
                    case "-loc":
                        try
                        {
                            loc = (FindTraverser.SearchLocation)Enum.Parse(typeof(FindTraverser.SearchLocation), option.Value, true);
                        }
                        catch
                        {
                            Console.WriteLine("Could not parse -loc parameter: " + option.Value);
                            return Usage();
                        }
                        break;

                    case "-reopt":
                        try
                        {
                            reopt = (RegexOptions)Enum.Parse(typeof(RegexOptions), option.Value, true);
                        }
                        catch
                        {
                            Console.WriteLine("Could not parse -reopt parameter: " + option.Value);
                            return Usage();
                        }
                        break;

                    case "-findformat":
                        try
                        {
                            findformat = (NameFormattingOptions)Enum.Parse(typeof(NameFormattingOptions), option.Value, true);
                        }
                        catch
                        {
                            Console.WriteLine("Could not parse -findformat parameter: " + option.Value);
                            return Usage();
                        }
                        break;

                    case "-outputformat":
                        try
                        {
                            outputformat = (NameFormattingOptions)Enum.Parse(typeof(NameFormattingOptions), option.Value, true);
                        }
                        catch
                        {
                            Console.WriteLine("Could not parse -outputformat parameter: " + option.Value);
                            return Usage();
                        }
                        break;

                    default:
                        Console.WriteLine("Unrecognized parameter: " + option.Key);
                        return Usage();
                }
            }

            var host = new Host(Platforms.Default);

            var assembly = host.LoadUnitFrom(assemblyFileName) as IAssembly;
            if (assembly == null || assembly == Dummy.Module || assembly == Dummy.Assembly)
            {
                throw new InvalidOperationException(assemblyFileName + " is not a .NET assembly");
            }

            new FindTraverser(new Regex(regex, reopt), loc, findformat, outputformat).Visit(assembly);

            return 0;
        }

        static int MergeAssembly(string outputAssemblyName, IList<string> assembliesToMerge)
        {
            var host = new Host(Platforms.Default);
            var baseAssembly = host.LoadUnitFrom(assembliesToMerge[0]) as IAssembly;
            if (baseAssembly == null || baseAssembly == Dummy.Module || baseAssembly == Dummy.Assembly)
            {
                throw new InvalidOperationException(assembliesToMerge[0] + " is not a .NET assembly");
            }

            Assembly mergedAssembly = new MetadataMutator(host).Visit(baseAssembly);
            for (int i = 1; i < assembliesToMerge.Count; ++i)
            {
                var mergeHost = new Host(Platforms.Default, host.NameTable);
                var addedAssembly = mergeHost.LoadUnitFrom(assembliesToMerge[i]) as IAssembly;
                if (addedAssembly == null || addedAssembly == Dummy.Module || addedAssembly == Dummy.Assembly)
                {
                    throw new InvalidOperationException(assembliesToMerge[i] + " is not a .NET assembly");
                }

                mergedAssembly = MergeAssemblies.Run(mergeHost, mergedAssembly, addedAssembly);
            }

            using (var peStream = File.Create(outputAssemblyName))
            {
                PeWriter.WritePeToStream(mergedAssembly, host, peStream);
            }

            // TODO: merge XML documentation, too

            return 0;
        }

        private static IEnumerable<string> ParsePlatform(string option)
        {
            if (string.Equals(option, "None", StringComparison.InvariantCultureIgnoreCase))
            {
                return Platforms.None;
            }
            else if (string.Equals(option, "Default", StringComparison.InvariantCultureIgnoreCase))
            {
                return Platforms.Default;
            }
            else if (string.Equals(option, "Desktop20", StringComparison.InvariantCultureIgnoreCase))
            {
                return Platforms.Desktop20;
            }
            else if (string.Equals(option, "Desktop30", StringComparison.InvariantCultureIgnoreCase))
            {
                return Platforms.Desktop30;
            }
            else if (string.Equals(option, "Desktop35", StringComparison.InvariantCultureIgnoreCase))
            {
                return Platforms.Desktop35;
            }
            else if (string.Equals(option, "Compact20", StringComparison.InvariantCultureIgnoreCase))
            {
                return Platforms.Compact20;
            }
            else if (string.Equals(option, "Compact35", StringComparison.InvariantCultureIgnoreCase))
            {
                return Platforms.Compact35;
            }
            else if (string.Equals(option, "Silverlight30", StringComparison.InvariantCultureIgnoreCase))
            {
                return Platforms.Silverlight30;
            }
            else if (string.Equals(option, "Micro40", StringComparison.InvariantCultureIgnoreCase))
            {
                return Platforms.Micro40;
            }
            else
            {
                throw new ArgumentException("Unknown platform " + option + ".");
            }
        }

        private static string InterpretLocationString(string location)
        {
            if (location.Contains("$(20)"))
            {
                location = location.Replace("$(20)", ReferenceAssembliesDirectory.Desktop20Directory);
            }

            if (location.Contains("$(30)"))
            {
                location = location.Replace("$(30)", ReferenceAssembliesDirectory.Desktop30Directory);
            }

            if (location.Contains("$(35)"))
            {
                location = location.Replace("$(35)", ReferenceAssembliesDirectory.Desktop35Directory);
            }

            if (location.Contains("$(CF20)"))
            {
                location = location.Replace("$(CF20)", ReferenceAssembliesDirectory.Compact20Directory);
            }

            if (location.Contains("$(CF35)"))
            {
                location = location.Replace("$(CF35)", ReferenceAssembliesDirectory.Compact35Directory);
            }

            if (location.Contains("$(SL30)"))
            {
                location = location.Replace("$(SL30)", ReferenceAssembliesDirectory.Silverlight30Directory);
            }

            if (location.Contains("$(u40)"))
            {
                location = location.Replace("$(u40)", ReferenceAssembliesDirectory.Micro40Directory);
            }

            return location;
        }
    }
}
