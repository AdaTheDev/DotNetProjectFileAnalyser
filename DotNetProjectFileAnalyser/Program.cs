using System;
using DotNetProjectFileAnalyser.Lib;

namespace DotNetProjectFileAnalyser
{
    /// <summary>
    /// Finds all .csproj files within the supplied directory (and all subdirectories)
    /// and analyses them to extract out some info:
    /// - Build output directory (relative and absolute) - for the build/platform specified(e.g. Debug AnyCpu)
    /// - list of all Project References (as opposed to assembly references)       
    ///
    /// Syntax:
    ///     DotNetProfileFileAnalyser {RootDirectory} {Configuration} {Platform}
    /// </summary>
    /// <example>    
    /// DotNetProjectFileAnalyser "C:\dev\" "Debug" "AnyCpu"
    /// </example>
    class Program
    {
        static void Main(string[] args)
        {
            if (args == null || args.Length != 3)
            {
                Console.WriteLine("Error - missing args. Syntax:");
                Console.WriteLine("DotNetProjectFileAnalyster.exe {RootDirectory} {Configuration} {Platform}");
                Console.WriteLine("Params:");
                Console.WriteLine("{RootDirectory} = start directory to trawl for .csproj files (including subdirectories)");
                Console.WriteLine("{Configuration} = as defined in VS, e.g. Debug, Release");
                Console.WriteLine("{Platform} = as defined in VS, e.g. AnyCpu");
            }
            else
            {
                var analyser = new ProjectFileAnalyser(args[0], args[1], args[2]);
                string outputFile = analyser.Analyse();
                Console.WriteLine("Analysis complete.");
                Console.WriteLine("See output file: ");
                Console.WriteLine(outputFile);
            }
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
