Overview
------
App to find all .csproj files within a directory (and all subdirectories) and parse out some info
into an output file.

Output:
-------
The following info is extracted:
1) Build output directory (relative and absolute) for the configuration/platform specified (e.g. Debug AnyCpu).
   Useful if you want find which projects you need to change to build to a central/common build directory.
2) Llist of all Project Reference dependencies (as opposed to assembly references). 
   Useful if you want to find the projects that have Project References so you can switch them to assembly references

The results are written to a file in the folder containing the .exe 

Usage
-----
DotNetProjectFileAnalyser.exe {RootDirectory} {Configuration} {Path}

{RootDirectory} = start directory to trawl for .csproj files (including subdirectories)
{Configuration} = as defined in VS, e.g. Debug, Release
{Platform} = as defined in VS, e.g. AnyCpu

Example
-------
DotNetProjectFileAnalyser.exe "C:\src\" "Debug" "AnyCpu"
