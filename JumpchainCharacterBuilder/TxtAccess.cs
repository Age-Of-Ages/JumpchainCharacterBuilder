using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace JumpchainCharacterBuilder
{
    /// <summary>
    /// Controls write access to export text files.
    /// </summary>
    public static class TxtAccess
    {
        /// <summary>
        /// Checks that the Export subdirectory exists and creates it if it doesn't exist.
        /// </summary>
        public static void CheckExportDirectoryExists()
        {
            DirectoryInfo directory = new(Environment.CurrentDirectory);

            if (!Directory.Exists($"{directory} \\Exports"))
            {
                directory.CreateSubdirectory("Exports");
            }

        }

        /// <summary>
        /// Checks that the specific export subdirectory exists and creates it if it doesn't exist.
        /// </summary>
        public static void CheckExportSubdirectoryExists(string subdirectory)
        {
            CheckExportDirectoryExists();

            DirectoryInfo directory = new(Environment.CurrentDirectory);

            if (!Directory.Exists($"{directory} \\Exports\\{subdirectory}"))
            {
                directory.CreateSubdirectory($"Exports\\{subdirectory}");
            }

        }

        /// <summary>
        /// Write export data to a text file.
        /// </summary>
        /// <param name="fileName">The file name to write to.</param>
        /// <param name="exportDirectory">The export subdirectory that the file should be saved in.
        /// Examples are: Builds, Passport, Warehouse, etc.</param>
        /// <param name="lines">The fully formatted data to write.</param>
        public static void WriteText(string fileName, string exportDirectory, List<string> lines)
        {
            CheckExportSubdirectoryExists(exportDirectory);

            fileName = Regex.Replace(fileName, @"[^0-9a-zA-Z ]+", "");

            File.WriteAllLines($"{Environment.CurrentDirectory}\\Exports\\{exportDirectory}\\{fileName}.txt", lines);

        }
    }
}
