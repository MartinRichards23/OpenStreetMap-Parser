using System;
using System.Collections.Generic;
using System.Text;

namespace OpenStreetMapParser.Models
{
    public class OsmRelation : OsmObject
    {
        public List<OsmMember> Members { get; set; }
    }
}
