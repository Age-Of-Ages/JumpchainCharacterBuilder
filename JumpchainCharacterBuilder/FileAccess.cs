using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
