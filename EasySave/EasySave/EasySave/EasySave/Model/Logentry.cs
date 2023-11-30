using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace EasySave.Model
{
    public class LogEntry
    {
        public string Name { get; set; }
        public string FileSource { get; set; }
        public string FileTarget { get; set; }
        public long FileSize { get; set; }
        public double FileTransferTime { get; set; }
        public DateTime Time { get; set; }
    }
}