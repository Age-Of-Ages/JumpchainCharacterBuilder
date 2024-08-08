using JumpchainCharacterBuilder.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace JumpchainCharacterBuilder
{
    public static partial class CfgAccess
    {
        public static void WriteCfgFile(AppSettingsModel appSettings)
        {
            string filePath = Path.Combine(Environment.CurrentDirectory, "Configuration.cfg");
            List<string> settings = [];
            Dictionary<string, string> currentSettings = new()
            {
                {"WeightFormat", appSettings.WeightFormat.ToString() },
                {"HeightFormat", appSettings.HeightFormat.ToString() },
                {"Theme", appSettings.Theme },
                {"CanResizeWindow", appSettings.CanResizeWindow.ToString() },
                {"ConfirmSaveOnClose", appSettings.ConfirmSaveOnClose.ToString() },
                {"SpellCheckEnabled", appSettings.SpellCheckEnabled.ToString() }
            };

            settings.Add("# Application settings");
            settings.Add("# Lines beginning with '#' and blank lines will be ignored.");
            settings.Add("");

            foreach (KeyValuePair<string, string> setting in currentSettings)
            {
                settings.Add(setting.Key + " = " + setting.Value);
            }

            File.WriteAllLines(filePath, settings);
        }

        public static void ReadCfgFile(AppSettingsModel appSettings)
        {
            string filePath = Path.Combine(Environment.CurrentDirectory, "Configuration.cfg");

            if (!FileAccess.CheckFileExists(filePath))
            {
                TxtAccess.WriteLog(
                [
                    "Configuration file not found: Regenerating file with default settings."
                ]);

                using FileStream temp = File.Create(filePath);

                WriteCfgFile(appSettings);
            }
            else
            {
                List<string> settings = File.ReadLines(filePath).ToList();
                Dictionary<string, string> settingsDictionary = new()
                {
                    {"HeightFormat", "FeetInches" },
                    {"WeightFormat", "Pounds" },
                    {"Theme", "Light" },
                    {"CanResizeWindow", "False" },
                    {"ConfirmSaveOnClose", "True" },
                    {"SpellCheckEnabled", "True" }
                };
                string[] splitString;
                string settingKey;
                string settingValue;

                foreach (string line in settings)
                {
                    if (CfgSplitRegex().IsMatch(line) && line[0] != '#' && line != "")
                    {
                        splitString = line.Split(" = ");

                        settingKey = splitString[0];
                        settingValue = splitString[1];

                        if (settingsDictionary.ContainsKey(settingKey))
                        {
                            settingsDictionary[settingKey] = settingValue;
                        }
                    }

                }


                if (Enum.TryParse(settingsDictionary["HeightFormat"], true, out AppSettingsModel.HeightFormats heightFormat))
                {
                    appSettings.HeightFormat = heightFormat;
                }
                else
                {
                    appSettings.HeightFormat = AppSettingsModel.HeightFormats.FeetInches;
                }

                if (Enum.TryParse(settingsDictionary["WeightFormat"], true, out AppSettingsModel.WeightFormats weightFormat))
                {
                    appSettings.WeightFormat = weightFormat;
                }
                else
                {
                    appSettings.WeightFormat = AppSettingsModel.WeightFormats.Pounds;
                }

                appSettings.Theme = settingsDictionary["Theme"];

                if (settingsDictionary["CanResizeWindow"] == "True")
                {
                    appSettings.CanResizeWindow = true;
                }
                else
                {
                    appSettings.CanResizeWindow = false;
                }

                if (settingsDictionary["ConfirmSaveOnClose"] == "True")
                {
                    appSettings.ConfirmSaveOnClose = true;
                }
                else
                {
                    appSettings.ConfirmSaveOnClose = false;
                }

                if (settingsDictionary["SpellCheckEnabled"] == "True")
                {
                    appSettings.SpellCheckEnabled = true;
                }
                else
                {
                    appSettings.SpellCheckEnabled = false;
                }
            }
        }

        public static string ReadSingleSetting(string settingSelection)
        {
            string filePath = Path.Combine(Environment.CurrentDirectory, "Configuration.cfg");
            string result = "Error";

            if (!FileAccess.CheckFileExists(filePath))
            {
                TxtAccess.WriteLog(
                [
                    "Configuration file not found: Regenerating file with default settings."
                ]);

                using FileStream temp = File.Create(filePath);

                AppSettingsModel appSettings = new();

                WriteCfgFile(appSettings);
            }

            List<string> settings = File.ReadLines(filePath).ToList();
            Dictionary<string, string> settingsDictionary = new()
            {
                {"HeightFormat", "FeetInches" },
                {"WeightFormat", "Pounds" },
                {"Theme", "Light" },
                {"CanResizeWindow", "False" },
                {"ConfirmSaveOnClose", "True" },
                {"SpellCheckEnabled", "True" }
            };
            string[] splitString;
            string settingKey;
            string settingValue;

            foreach (string line in settings)
            {
                if (CfgSplitRegex().IsMatch(line) && line[0] != '#' && line != "")
                {
                    splitString = line.Split(" = ");

                    settingKey = splitString[0];
                    settingValue = splitString[1];

                    if (settingsDictionary.ContainsKey(settingKey))
                    {
                        settingsDictionary[settingKey] = settingValue;
                    }
                }

            }

            if (settingsDictionary.TryGetValue(settingSelection, out string? value))
            {
                result = value;
            }

            return result;
        }

        [GeneratedRegex(".+ = .+")]
        private static partial Regex CfgSplitRegex();
    }
}
