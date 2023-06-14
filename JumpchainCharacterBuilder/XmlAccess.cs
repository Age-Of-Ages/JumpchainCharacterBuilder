using JumpchainCharacterBuilder.Model;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace JumpchainCharacterBuilder
{
    /// <summary>
    /// Controls read/write access to saved Jumper files.
    /// </summary>
    public static class XmlAccess
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
        /// Checks that the Saves subdirectory exists and creates it if
        /// it doesn't exist.
        /// </summary>
        public static void CheckSavesDirectoryExists()
        {
            DirectoryInfo directory = new(Environment.CurrentDirectory);

            if (!Directory.Exists($"{directory} \\Saves"))
            {
                directory.CreateSubdirectory("Saves");
            }

        }

        /// <summary>
        /// Checks that the Save backups subdirectory exists and creates it if
        /// it doesn't exist.
        /// </summary>
        public static void CheckSaveBackupsDirectoryExists()
        {
            DirectoryInfo directory = new(Environment.CurrentDirectory);

            if (!Directory.Exists($"{directory} \\Backups"))
            {
                directory.CreateSubdirectory("Backups");
            }

        }

        /// <summary>
        /// Backup existing save file before overwriting.
        /// </summary>
        /// <param name="filePath">
        /// Represents the full file path of the save file.
        /// </param>
        public static void BackupFile(string filePath)
        {
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            DirectoryInfo directory = new(Environment.CurrentDirectory + @"\Backups\");

            for (int i = 1; i <= 10; i++)
            {
                if (!CheckFileExists($"{directory}{fileName} ({i}).xml"))
                {
                    File.Copy(filePath, Path.Combine($"{directory}", $"{fileName} ({i}).xml"), true);

                    break;
                }
                else if (i == 10)
                {
                    try
                    {
                        File.Delete(Path.Combine($"{directory}", $"{fileName} (1).xml"));

                        for (int x = 2; x <= 10; x++)
                        {
                            File.Move(Path.Combine($"{directory}", $"{fileName} ({x}).xml"), Path.Combine($"{directory}", $"{fileName} ({x - 1}).xml"));

                            File.Copy(filePath, Path.Combine($"{directory}", $"{fileName} (10).xml"), true);
                        }

                        break;
                    }
                    catch (IOException)
                    {
                        // Failed to create rolling backup.
                    }
                }
            }
        }

        /// <summary>
        /// Write a Jumper save to disk.
        /// </summary>
        /// <param name="filePath">
        /// Represents the full file path of the save file.
        /// </param>
        /// <param name="saveFile">
        /// The save file object that the Jumper's data is stored in.
        /// </param>
        public static void WriteObject(string filePath, SaveFile saveFile)
        {
            DataContractSerializer ser = new(typeof(SaveFile));
            XmlWriterSettings settings = new()
            {
                Indent = true,
                OmitXmlDeclaration = false,
                Encoding = Encoding.UTF8
            };

            if (CheckFileExists(filePath))
            {
                BackupFile(filePath);
            }

            using (XmlWriter writer = XmlWriter.Create(filePath, settings))
            {
                ser.WriteObject(writer, saveFile);
                writer.Close();
            }

        }

        /// <summary>
        /// Loads a Jumper save from disk.
        /// </summary>
        /// <param name="filePath">
        /// The full file path of the loaded file.
        /// </param>
        /// <returns>
        /// Returns the file object of the save data.
        /// </returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static SaveFile ReadObject(string filePath)
        {
            DataContractSerializer ser = new(typeof(SaveFile));
            XmlReaderSettings settings = new()
            {
                DtdProcessing = DtdProcessing.Prohibit,
                XmlResolver = null
            };

            SaveFile newSave = new();

            using (XmlReader reader = XmlReader.Create(filePath, settings))
            {
                newSave = ser.ReadObject(reader) as SaveFile
                          ?? throw new ArgumentNullException();
                reader.Close();
                return newSave;
            }

        }
    }
}
