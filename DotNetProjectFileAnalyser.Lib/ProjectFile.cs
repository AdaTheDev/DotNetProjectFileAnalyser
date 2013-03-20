using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace DotNetProjectFileAnalyser.Lib
{
    /// <summary>
    /// Represents a .csproj file.
    /// </summary>
    public class ProjectFile
    {
        private readonly string _fileName;
        private string _buildOutputDirectoryRelative;
        private string _buildOutputDirectoryAbsolute;
        private readonly List<ProjectReference> _projectReferences;
        private readonly string _projectFileDirectory;

        public ProjectFile(string fileName)
        {
            _fileName = fileName;
            _projectReferences = new List<ProjectReference>();
            _projectFileDirectory = Path.GetDirectoryName(fileName);
        }

        public string FileName { get { return _fileName; } }
        public string BuildOutputDirectoryRelative { get { return _buildOutputDirectoryRelative; } }
        public string BuildOutputDirectoryAbsolute { get { return _buildOutputDirectoryAbsolute; } }

        public ReadOnlyCollection<ProjectReference> ProjectReferences
        {
            get { return _projectReferences.AsReadOnly(); }
        }

        public void AddProjectReference(ProjectReference reference)
        {
            _projectReferences.Add(reference);
        }

        public void SetBuildOutputDirectory(string buildOutputDirectory)
        {
            _buildOutputDirectoryRelative = buildOutputDirectory;
            if (!buildOutputDirectory.StartsWith(".") && !buildOutputDirectory.StartsWith(@"\"))
                buildOutputDirectory = @"\" + buildOutputDirectory;
            _buildOutputDirectoryAbsolute = Path.GetFullPath(_projectFileDirectory + buildOutputDirectory);
        }
    }
}
