using System;
using System.Collections.Generic;
using System.Text;

namespace OpenStreetMapParser.Models
{
    public abstract class OsmObject
    {
        public long Id { get; set; }
        public int Version { get; set; }
        public DateTime Timestamp { get; set; }
        public int Changeset { get; set; }

        public Dictionary<string, string> Tags { get; set; }
    }
}
