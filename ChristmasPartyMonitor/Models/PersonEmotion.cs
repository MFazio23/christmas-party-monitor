using System;
using System.Collections.Generic;

namespace ChristmasPartyMonitor.Models
{
    public class PersonEmotion
    {
        public KeyValuePair<string, float> Emotion { get; set; }
        public Guid PersonId { get; set; }
        public string Name { get; set; }
        public string UserData { get; set; }
    }
}