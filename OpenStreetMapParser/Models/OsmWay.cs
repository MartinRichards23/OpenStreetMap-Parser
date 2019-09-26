using System;
using System.Collections.Generic;
using System.Text;

namespace OpenStreetMapParser.Models
{
    /// <summary>
    /// OSM way. https://wiki.openstreetmap.org/wiki/Way
    /// </summary>
    public class OsmWay : OsmObject
    {
        public List<long> Nodes { get; set; }
    }
}
