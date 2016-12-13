using System;
using System.Collections.Generic;

namespace ChristmasPartyMonitor.Models
{
    public class PersonLoadResult
    {
        public IList<Guid> PersistedFaceIds = new List<Guid>();
        public string Name { get; set; }

        public PersonLoadResult(string name)
        {
            this.Name = name;
        }

        public void AddPersistedFaceId(Guid persistedFaceId) => PersistedFaceIds.Add(persistedFaceId);
    }
}