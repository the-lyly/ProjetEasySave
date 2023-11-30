using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace EasySave.Model
{
        public class Log
        {
            private const string LogDirectory = "C:\\Users\\acer\\OneDrive\\Bureau\\ici";
            private string LogFilePath => Path.Combine(LogDirectory, $"log_{DateTime.Now:yyyyMMdd}.json");
            private const int LogFileDurationHours = 24;

            private readonly List<LogEntry> logEntries;

            public Log()
            {
                logEntries = LoadLogEntries();
            }

            public void Create_Log(Travail work, long fileSize, double transferDuration)
            {
                var logEntry = new LogEntry
                {
                    Name = work.Name,
                    FileSource = work.SourcePath,
                    FileTarget = work.DestinationPath,
                    FileSize = fileSize,
                    FileTransferTime = transferDuration,
                    Time = DateTime.Now,
                };

                logEntries.Add(logEntry);

                SaveLogEntries();
            }

            private List<LogEntry> LoadLogEntries()
            {
                if (File.Exists(LogFilePath))
                {
                    string jsonContent = File.ReadAllText(LogFilePath);
                    return JsonConvert.DeserializeObject<List<LogEntry>>(jsonContent) ?? new List<LogEntry>();
                }

                return new List<LogEntry>();
            }

            private void SaveLogEntries()
            {

                var entriesInLast24Hours = logEntries
                        .Where(entry => DateTime.Now - entry.Time <= TimeSpan.FromHours(LogFileDurationHours))
                        .ToList();

                string jsonContent = JsonConvert.SerializeObject(entriesInLast24Hours, Formatting.Indented);
                File.WriteAllText(LogFilePath, jsonContent);
            }
        }

    }
