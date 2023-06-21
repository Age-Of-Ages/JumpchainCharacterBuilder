using System;
using System.IO;
using System.Linq;

namespace JumpchainCharacterBuilder
{
    public static class FileAccess
    {
        /// <summary>
        /// Check whether a file exists before performing any operations.
        /// </summary>
        /// <param name="filePath">
        /// Represents the full file path of the save file.
        /// </param>
        /// <returns></returns>
        public static bool CheckFileExists(string filePath)
        {
            return File.Exists(filePath);
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
        /// Checks if the stated subdirectory exists and creates it if it doesn't exist.
        /// </summary>
        /// <param name="subDirectory">Represents the subdirectory to check for.</param>
        public static void CheckSubdirectoryExists(string subDirectory)
        {
            DirectoryInfo directory = new(Environment.CurrentDirectory);

            if (!Directory.Exists(Path.Combine(directory.FullName, subDirectory)))
            {
                directory.CreateSubdirectory(subDirectory);
            }

        }
    }
}
