﻿using JumpchainCharacterBuilder.Model;
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
        /// Backup existing save file before overwriting.
        /// </summary>
        /// <param name="filePath">
        /// Represents the full file path of the save file.
        /// </param>
        public static void BackupFile(string filePath)
        {
            FileAccess.CheckSubdirectoryExists("Backups");

            string fileName = Path.GetFileNameWithoutExtension(filePath);
            DirectoryInfo directory = new(Path.Combine(Environment.CurrentDirectory, "Backups"));

            for (int i = 1; i <= 10; i++)
            {
                string currentFile = Path.Combine(directory.FullName, $"{fileName} ({i}).xml");

                if (!FileAccess.CheckFileExists(currentFile))
                {
                    File.Copy(filePath, currentFile, true);

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

            if (FileAccess.CheckFileExists(filePath))
            {
                BackupFile(filePath);
            }

            using XmlWriter writer = XmlWriter.Create(filePath, settings);
            ser.WriteObject(writer, saveFile);
            writer.Close();
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
            using XmlReader reader = XmlReader.Create(filePath, settings);
            SaveFile newSave = ser.ReadObject(reader) as SaveFile ?? throw new ArgumentNullException();
            reader.Close();
            return newSave;

        }
    }
}
