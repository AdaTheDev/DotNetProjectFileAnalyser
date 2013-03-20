using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace DotNetProjectFileAnalyser.Lib
{
    /// <summary>
    /// Analyses .csproj files, extracting build output directory (relative/absolute) and any project references.
    /// </summary>
    public sealed class ProjectFileAnalyser
    {
        private readonly string _directory;
        private readonly string _buildConfiguration;
        private readonly string _buildPlatform;

        public ProjectFileAnalyser(string directory, string buildConfiguration, string buildPlatform)
        {
            if (string.IsNullOrWhiteSpace(directory)) throw new ArgumentException("Directory must be supplied", "directory");
            if (string.IsNullOrWhiteSpace(buildConfiguration)) throw new ArgumentException("Build Configuration must be supplied", "buildConfiguration");
            if (string.IsNullOrWhiteSpace(buildPlatform)) throw new ArgumentException("Build Platform must be supplied", "buildPlatform");
            if (!Directory.Exists(directory))            
                throw new DirectoryNotFoundException("Directory does not exist");
            
            _directory = directory;
            _buildConfiguration = buildConfiguration.ToLowerInvariant();
            _buildPlatform = buildPlatform.ToLowerInvariant();
        }

        public string Analyse()
        {
            using (var outputFile = new OutputFileWriter())
            {
                foreach (var projectFile in Directory.GetFiles(_directory, "*.csproj", SearchOption.AllDirectories))
                {
                    var file = ProcessFile(projectFile);
                    outputFile.WriteToFile(file);
                }

                return outputFile.FileName;
            }
        }

        private ProjectFile ProcessFile(string file)
        {
            var projectFile = new ProjectFile(file);

            var projectFileDoc = XDocument.Load(file);
            XNamespace @namespace = "";
            if (projectFileDoc.Root != null)            
                @namespace = projectFileDoc.Root.Name.Namespace;

            ParseBuildDirectory(projectFileDoc, @namespace, projectFile);
            ParseProjectReferences(projectFileDoc, @namespace, projectFile);
            
            return projectFile;
        }

        private void ParseBuildDirectory(XDocument projectFileDoc, XNamespace @namespace, ProjectFile projectFile)
        {
            var node = (from g in projectFileDoc.Descendants(@namespace + "PropertyGroup")
                        let config = g.Attribute("Condition")
                        where
                            config != null &&
                            config.Value.ToLowerInvariant() ==
                            " '$(configuration)|$(platform)' == '" + _buildConfiguration + "|" + _buildPlatform + "' "
                        select g.Element(@namespace + "OutputPath")).FirstOrDefault();
            
            if (node != null)
            {
                projectFile.SetBuildOutputDirectory(node.Value);
            }
        }

        private static void ParseProjectReferences(XDocument projectFileDoc, XNamespace @namespace, ProjectFile projectFile)
        {
            foreach (var referenceNode in projectFileDoc.Descendants(@namespace + "ProjectReference"))
            {
                projectFile.AddProjectReference(new ProjectReference(referenceNode.Attribute("Include").Value));
            }
        }
    }
}
