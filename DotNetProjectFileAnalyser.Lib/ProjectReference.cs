namespace DotNetProjectFileAnalyser.Lib
{
    /// <summary>
    /// A project reference dependency that a .csproj has 
    /// </summary>
    public class ProjectReference
    {
        private readonly string _referencedProjectFile;

        public ProjectReference(string projectFileReference)
        {
            _referencedProjectFile = projectFileReference;
        }

        public string ReferencedProjectFile
        {
            get { return _referencedProjectFile; }
        }        
    }
}