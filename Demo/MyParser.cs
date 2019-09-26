using OpenStreetMapParser;
using OpenStreetMapParser.Models;
using System;
using System.IO;

namespace Demo
{
    /// <summary>
    /// Parser implementation that adds events
    /// </summary>
    class MyParser : Parser
    {
        readonly Action<OsmNode> nodeRead;
        readonly Action<OsmRelation> relationRead;
        readonly Action<OsmWay> wayRead;

        public MyParser(Stream stream, Action<OsmNode> nodeRead, Action<OsmRelation> relationRead, Action<OsmWay> wayRead) : base(stream)
        {
            this.nodeRead = nodeRead;
            this.relationRead = relationRead;
            this.wayRead = wayRead;
        }

        protected override void NodeRead(OsmNode node)
        {
            nodeRead(node);
        }

        protected override void RelationRead(OsmRelation relation)
        {
            relationRead(relation);
        }

        protected override void WayRead(OsmWay way)
        {
            wayRead(way);
        }
    }
}
