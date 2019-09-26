using System;
using System.Collections.Generic;
using System.Text;

namespace OpenStreetMapParser.Models
{
    /// <summary>
    /// OSM relation. https://wiki.openstreetmap.org/wiki/Relation
    /// </summary>
    public class OsmRelation : OsmObject
    {
        public List<OsmMember> Members { get; set; }
    }
}
