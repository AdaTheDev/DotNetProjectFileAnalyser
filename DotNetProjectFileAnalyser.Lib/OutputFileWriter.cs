using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DotNetProjectFileAnalyser.Lib
{
    /// <summary>
    /// Writes info extracted from parsing .csproj files, into a output file.
    /// </summary>
    internal sealed class OutputFileWriter : IDisposable
    {
        private readonly string _fileName;
        private StreamWriter _writer;

        public OutputFileWriter()
        {
            _fileName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) +
                        @"\DotNetProjectFileAnalyserOutput_" + DateTime.Now.ToString("yyyyMMdd_HHmmssfff") +
                        ".txt";
        }

        public string FileName
        {
            get { return _fileName; }
        }

        public void WriteToFile(ProjectFile projectFile)
        {
            if (_writer == null)
                _writer = File.CreateText(_fileName);

            _writer.WriteLine("----------------------------------------------------");
            _writer.WriteLine("Project: " + projectFile.FileName);

            WriteBuildOutputDirectorySection(projectFile);
            WriteProjectReferencesSection(projectFile);                      
        }

        private void WriteBuildOutputDirectorySection(ProjectFile projectFile)
        {
            _writer.WriteLine("\tBuild Output (relative): " + projectFile.BuildOutputDirectoryRelative);
            _writer.WriteLine("\tBuild Output (absolute): " + projectFile.BuildOutputDirectoryAbsolute);
        }

        private void WriteProjectReferencesSection(ProjectFile projectFile)
        {
            _writer.Write("\tProject References: ");
            if (projectFile.ProjectReferences.Any())
            {
                _writer.WriteLine("");
                foreach (var reference in projectFile.ProjectReferences)
                {
                    _writer.WriteLine("\t\t" + reference.ReferencedProjectFile);
                }
            }
            else
            {
                _writer.WriteLine("None Found");
            }
        }

        public void Dispose()
        {
            if (_writer != null)           
                _writer.Dispose();            
        }
    }
}