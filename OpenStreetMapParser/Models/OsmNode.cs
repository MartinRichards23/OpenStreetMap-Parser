using System;

namespace OpenStreetMapParser.Models
{
    /// <summary>
    /// OSM node. https://wiki.openstreetmap.org/wiki/Node
    /// </summary>
    public class OsmNode : OsmObject
    {
        public double Lat { get; set; }
        public double Lon { get; set; }
    }
}
