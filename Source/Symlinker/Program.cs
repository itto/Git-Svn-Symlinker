using System;
using System.Diagnostics;
using System.IO;

namespace Symlinker
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length < 3)
            {
                Console.WriteLine("Provide paths in & out");
                Console.ReadLine();
            }
            else
            {
                var inFolder = new DirectoryInfo(args[1]);
                var outFolder = new DirectoryInfo(args[2]);
                var basePath = new DirectoryInfo(args[0]);

                MakeSymlinks(basePath, inFolder, outFolder);
            }
        }

        private static void MakeSymlinks(FileSystemInfo basePath, DirectoryInfo inFolder, FileSystemInfo outFolder)
        {
            using (var sw = File.AppendText(Path.Combine(basePath.FullName, ".git", "info", "exclude")))
            {
                sw.WriteLine();
                sw.WriteLine("#Symlinker added paths");

                foreach (var directory in inFolder.GetDirectories())
                {
                    var destination = Path.Combine(outFolder.FullName, directory.Name);

                    if (Directory.Exists(destination))
                        continue;

                    string linkCommand = $"mklink /J {destination} {directory.FullName}";
                    Console.WriteLine(linkCommand);

                    Process.Start("cmd.exe", $"/C {linkCommand}");

                    var ignorePath = destination.Replace(basePath.FullName, "").Replace(@"\", "/");
                    sw.WriteLine(ignorePath);
                }
            }
        }
    }
}