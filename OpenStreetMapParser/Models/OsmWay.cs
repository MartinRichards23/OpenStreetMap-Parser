using System;
using System.Collections.Generic;
using System.Text;

namespace OpenStreetMapParser.Models
{
    public class OsmWay : OsmObject
    {
        public List<long> Nodes { get; set; }
    }
}
