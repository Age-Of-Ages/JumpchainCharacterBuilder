using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace JumpchainCharacterBuilder
{
    /// <summary>
    /// Controls write access to export to text files.
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
        /// Check if a text file is within a certain length.
        /// </summary>
        /// <param name="maxLength">The maximum number of lines allowed.</param>
        /// <param name="filePath">The full path of the file to check.</param>
        /// <returns>True if the file is within the stated length, false if it is not or if an error occurs.</returns>
        public static bool CheckFileLength(int maxLength, string filePath)
        {
            try
            {
                return File.ReadLines(filePath).Count() < maxLength;
            }
            catch (Exception)
            {
                return false;
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

        public static List<string> ReadText(string filePath)
        {
            List<string> result = new();

            if (FileAccess.CheckFileExists(filePath))
            {
                result = File.ReadLines(filePath).ToList();
            }

            return result;
        }

        public static void WriteLog(List<string> lines)
        {
            string logPath = Path.Combine(Environment.CurrentDirectory, "Log.txt");

            if (!FileAccess.CheckFileExists(logPath))
            {
                File.Create(logPath);
            }

            bool fileSizeWithinBounds = CheckFileLength(500, logPath);

            if (!fileSizeWithinBounds)
            {
                List<string> tempLogs = (List<string>)File.ReadLines(logPath);

                tempLogs.AddRange(lines);

                tempLogs.RemoveRange(0, lines.Count);

                File.WriteAllLines(logPath, tempLogs);
            }
            else
            {
                File.AppendAllLines(logPath, lines);
            }
        }
    }
}
