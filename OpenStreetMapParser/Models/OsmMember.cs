using System;
using System.Collections.Generic;
using System.Text;

namespace OpenStreetMapParser.Models
{
    public class OsmMember
    {
        public string Type { get; set; }
        public long Ref { get; set; }
        public string Role { get; set; }
    }
}
