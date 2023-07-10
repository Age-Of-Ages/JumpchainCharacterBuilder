using JumpchainCharacterBuilder.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace JumpchainCharacterBuilder
{
    public static partial class RandomizeListAccess
    {
        public static List<JumpRandomizerList> ReadJumpListFile()
        {
            string filePath = Path.Combine(Environment.CurrentDirectory, "JumpList.txt");
            List<JumpRandomizerList> output = new();

            string categoryTag;
            string[] splitString;
            string jumpName;
            Uri jumpUri;

            if (!FileAccess.CheckFileExists(filePath))
            {
                TxtAccess.WriteLog(new()
                {
                    "Jump List for randomizer not found: Regenerating template file."
                });

                List<JumpRandomizerList> exampleList = new()
                {
                    new()
                    {
                        ListName = "Example List"
                    }
                };

                exampleList.First().ListEntries.Add(new()
                {
                    JumpName = "Example Jump 1"
                });
                exampleList.First().ListEntries.Add(new()
                {
                    JumpName = "Example Jump 2"
                });
                exampleList.First().ListEntries.Add(new()
                {
                    JumpName = "Example Jump 3",
                    JumpWeight = 2
                });
                exampleList.First().ListEntries.Add(new()
                {
                    JumpName = "Example Jump 4"
                });
                exampleList.First().ListEntries.Add(new()
                {
                    JumpName = "Example Jump 5"
                });

                WriteJumpListFile(exampleList);

                return exampleList;
            }
            else
            {
                List<string> tempLines = TxtAccess.ReadText(filePath);
                Regex listTagRegex = JumpListTagRegex();

                foreach (string line in tempLines)
                {
                    if (listTagRegex.IsMatch(line))
                    {
                        categoryTag = listTagRegex.Match(line).Value;

                        output.Add(new()
                        {
                            ListName = categoryTag
                        });
                    }
                    else if (line != "" && line[0] != '#')
                    {
                        splitString = line.Split(" | ");

                        if (splitString.Length < 3)
                        {
                            TxtAccess.WriteLog(new()
                            {
                                $"Entry in JumpList.txt at line {tempLines.IndexOf(line)} is incorrectly formatted or missing one or more required values.",
                                "Skipping and moving on to the next line. Note: Data will be lost if list is saved in this state.",
                                $"Incorrect data line: {line}"
                            });

                            continue;
                        }

                        jumpName = splitString[0];

                        if (!int.TryParse(splitString[1], out int jumpWeight))
                        {
                            TxtAccess.WriteLog(new()
                            {
                                $"Invalid weight value in JumpList.txt at line {tempLines.IndexOf(line)}"
                            });

                            jumpWeight = 0;
                        }

                        if (Uri.IsWellFormedUriString(splitString[2], UriKind.Absolute) && Uri.TryCreate(splitString[2], UriKind.Absolute, out Uri? result) && 
                                                                                        (result.Scheme == Uri.UriSchemeHttp || result.Scheme == Uri.UriSchemeHttps))
                        {
                            jumpUri = new(splitString[2], UriKind.Absolute);
                        }
                        else
                        {
                            jumpUri = new("About:Blank");
                        }

                        output.Last().ListEntries.Add(new()
                        {
                            JumpName = jumpName,
                            JumpWeight = jumpWeight,
                            JumpUri = jumpUri
                        });
                    }
                }

                return output;
            }
        }

        public static void WriteJumpListFile(List<JumpRandomizerList> randomizerJumpLists)
        {
            List<string> lines = new();
            string line;
            string tempPath = Path.Combine(Environment.CurrentDirectory, "JumpList.temp");
            string filePath = Path.Combine(Environment.CurrentDirectory, "JumpList.txt");

            lines.Add("# Lines in this entry that are not blank and that do not start with # will be considered valid lines for the Jump randomizer function.");
            lines.Add("# Entries should be formatted as 'JumpName | Weight | Link'");
            lines.Add("# The weight of a Jump will determine how likely it is to be selected compared to other Jumps.");
            lines.Add("# The third section is optional and may be entered as 'About:Blank' to ignore it.");
            lines.Add("# Each entry must be underneath a section tag, formatted as '[SectionName]'");
            lines.Add("# It is not advised to edit this file manually. Prefer using the application interface to edit Jump lists.");
            lines.Add("");

            foreach (JumpRandomizerList list in randomizerJumpLists)
            {
                lines.Add($"[{list.ListName}]");

                foreach (JumpRandomizerEntry jumpEntry in list.ListEntries)
                {
                    line = jumpEntry.JumpName + " | " + jumpEntry.JumpWeight + " | " + jumpEntry.JumpUri;

                    lines.Add(line);
                }

                lines.Add("");
            }

            File.WriteAllLines(tempPath, lines);

            File.Move(tempPath, filePath, true);
        }

        [GeneratedRegex("(?<=^\\[).+(?=\\])", RegexOptions.Compiled)]
        private static partial Regex JumpListTagRegex();
    }
}
