using JumpchainCharacterBuilder.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace JumpchainCharacterBuilder
{
    public static class CfgAccess
    {
        public static void WriteCfgFile(AppSettingsModel appSettings)
        {
            string filePath = Path.Combine(Environment.CurrentDirectory, "Configuration.cfg");
            List<string> settings = new();
            Dictionary<string, string> currentSettings = new()
            {
                {"WeightFormat", appSettings.WeightFormat.ToString() },
                {"HeightFormat", appSettings.HeightFormat.ToString() },
                {"Theme", appSettings.Theme }
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
                TxtAccess.WriteLog(new()
                {
                    "Configuration file not found: Regenerating file with default settings."
                });

                File.Create(filePath);

                WriteCfgFile(appSettings);
            }
            else
            {
                List<string> settings = File.ReadLines(filePath).ToList();
                Dictionary<string, string> settingsDictionary = new()
                {
                    {"HeightFormat", "FeetInches" },
                    {"WeightFormat", "Pounds" },
                    {"Theme", "Light" }
                };
                string[] splitString;
                string settingKey;
                string settingValue;

                foreach (string line in settings)
                {
                    if (Regex.IsMatch(line, @".+ = .+") && line[0] != '#' && line != "")
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
            }
        }
    }
}
