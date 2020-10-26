using System;
using System.IO;

namespace pathfinder
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // Create a new instance of the class findPath and pass the string argument from console
            Path findPath = new Path(args[0]);
            // Create string path with the argument passed from console and the extension .csv
            string path = @args[0] + ".csn";
            // Initialise a new instance of the StreamWriter class for the specified path string
            StreamWriter streamWriter = new StreamWriter(path);
            // We write the string with the path into the .csn file
            streamWriter.Write(findPath.SolutionString);
            // Closes the current StreamWriter object.
            streamWriter.Close();
            // Terminates the application
            Environment.Exit(0);
        }
    }
}