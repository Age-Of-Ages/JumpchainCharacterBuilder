﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace JumpchainCharacterBuilder
{
    /// <summary>
    /// Controls write access to export to text files.
    /// </summary>
    public static partial class TxtAccess
    {
        /// <summary>
        /// Write export data to a text file.
        /// </summary>
        /// <param name="fileName">The file name to write to.</param>
        /// <param name="exportDirectory">The export subdirectory that the file should be saved in.
        /// Examples are: Builds, Passport, Warehouse, etc.</param>
        /// <param name="lines">The fully formatted data to write.</param>
        public static void WriteExportText(string fileName, string exportDirectory, List<string> lines)
        {
            FileAccess.CheckSubdirectoryExists("Exports");
            FileAccess.CheckSubdirectoryExists(@"Exports\" + exportDirectory);

            fileName = FileNameCleanRegex().Replace(fileName, "");

            File.WriteAllLines(@$"{Environment.CurrentDirectory}\Exports\{exportDirectory}\{fileName}.txt", lines);

        }

        public static void WriteText(string fileName, string directory, List<string> lines)
        {
            FileAccess.CheckSubdirectoryExists(directory);

            fileName = FileNameCleanRegex().Replace(fileName, "");

            File.WriteAllLines(Path.Combine(Environment.CurrentDirectory, directory, fileName), lines);
        }

        public static List<string> ReadText(string filePath)
        {
            List<string> result = [];

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
                using FileStream temp = File.Create(logPath);
            }

            bool fileSizeWithinBounds = FileAccess.CheckFileLength(5000, logPath);

            if (!fileSizeWithinBounds)
            {
                List<string> tempLogs = File.ReadLines(logPath).ToList();

                tempLogs.AddRange(lines);

                tempLogs.RemoveRange(0, lines.Count);

                File.WriteAllLines(logPath, tempLogs);
            }
            else
            {
                File.AppendAllLines(logPath, lines);
            }
        }

        [GeneratedRegex(@"[^0-9a-zA-Z ]+")]
        private static partial Regex FileNameCleanRegex();
    }
}
