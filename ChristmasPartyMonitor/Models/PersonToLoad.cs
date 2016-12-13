using System.Collections.Generic;

namespace ChristmasPartyMonitor.Models
{
    public class PersonToLoad
    {
        public string Name { get; set; }
        public string UserData { get; set; }
        public IList<string> Urls { get; set; }
    }
}