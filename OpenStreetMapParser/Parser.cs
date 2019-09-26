using OpenStreetMapParser.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml;
using System.Xml.Linq;

namespace OpenStreetMapParser
{
    /// <summary>
    /// Parse the OSM data file
    /// </summary>
    public abstract class Parser
    {
        #region Fields

        XmlReader xmlReader;

        #endregion

        protected Parser(Stream stream)
        {
            xmlReader = XmlReader.Create(stream);
        }

        #region Public methods

        /// <summary>
        /// Read the stream
        /// </summary>
        public void Read(CancellationToken token)
        {
            xmlReader.ReadToFollowing("bounds");
            XElement boundsElement = XNode.ReadFrom(xmlReader) as XElement;

            double minLat = boundsElement.GetAttributeDouble("minlat");
            double minLon = boundsElement.GetAttributeDouble("minlon");
            double maxLat = boundsElement.GetAttributeDouble("maxlat");
            double maxLon = boundsElement.GetAttributeDouble("maxlon");

            while (xmlReader.Read())
            {
                token.ThrowIfCancellationRequested();

                if (xmlReader.NodeType == XmlNodeType.Element)
                {
                    if (xmlReader.Name == "node")
                    {
                        XElement nodeElement = XNode.ReadFrom(xmlReader) as XElement;
                        ReadNode(nodeElement);
                    }
                    else if (xmlReader.Name == "way")
                    {
                        XElement wayElement = XNode.ReadFrom(xmlReader) as XElement;
                        ReadWay(wayElement);
                    }
                    else if (xmlReader.Name == "relation")
                    {
                        XElement relationElement = XNode.ReadFrom(xmlReader) as XElement;
                        ReadRelation(relationElement);
                    }
                }
            }
        }

        #endregion

        #region Private methods

        private void ReadNode(XElement el)
        {
            long id = el.GetAttributeLong("id");
            double lat = el.GetAttributeDouble("lat");
            double lon = el.GetAttributeDouble("lon");
            int version = el.GetAttributeInt("version");
            DateTime timestamp = el.GetAttributeDateTime("timestamp", DateTime.MinValue);
            int changeset = el.GetAttributeInt("changeset");

            Dictionary<string, string> tags = new Dictionary<string, string>();

            foreach (var element in el.Nodes().OfType<XElement>().Where(e => e.Name == "tag"))
            {
                string key = element.Attribute("k").Value;
                string value = element.Attribute("v").Value;
                tags.Add(key, value);
            }

            OsmNode node = new OsmNode()
            {
                Id = id,
                Lat = lat,
                Lon = lon,
                Version = version,
                Timestamp = timestamp,
                Changeset = changeset,
                Tags = tags,
            };

            NodeRead(node);
        }

        private void ReadWay(XElement el)
        {
            long id = el.GetAttributeLong("id");
            double lat = el.GetAttributeDouble("lat");
            double lon = el.GetAttributeDouble("lon");
            int version = el.GetAttributeInt("version");
            DateTime timestamp = el.GetAttributeDateTime("timestamp", DateTime.MinValue);
            int changeset = el.GetAttributeInt("changeset");

            Dictionary<string, string> tags = new Dictionary<string, string>();
            List<long> nodes = new List<long>();

            foreach (var element in el.Nodes().OfType<XElement>().Where(e => e.Name == "tag"))
            {
                string key = element.Attribute("k").Value;
                string value = element.Attribute("v").Value;
                tags.Add(key, value);
            }

            foreach (var element in el.Nodes().OfType<XElement>().Where(e => e.Name == "nd"))
            {
                long node = element.GetAttributeLong("ref");
                nodes.Add(node);
            }

            OsmWay way = new OsmWay()
            {
                Id = id,
                Version = version,
                Timestamp = timestamp,
                Changeset = changeset,
                Tags = tags,
                Nodes = nodes,
            };

            WayRead(way);
        }

        private void ReadRelation(XElement el)
        {
            long id = el.GetAttributeLong("id");
            double lat = el.GetAttributeDouble("lat");
            double lon = el.GetAttributeDouble("lon");
            int version = el.GetAttributeInt("version");
            DateTime timestamp = el.GetAttributeDateTime("timestamp", DateTime.MinValue);
            int changeset = el.GetAttributeInt("changeset");

            Dictionary<string, string> tags = new Dictionary<string, string>();
            List<OsmMember> members = new List<OsmMember>();

            foreach (var element in el.Nodes().OfType<XElement>().Where(e => e.Name == "tag"))
            {
                string key = element.Attribute("k").Value;
                string value = element.Attribute("v").Value;
                tags.Add(key, value);
            }

            foreach (var element in el.Nodes().OfType<XElement>().Where(e => e.Name == "member"))
            {
                OsmMember member = new OsmMember()
                {
                    Type = element.Attribute("type")?.Value,
                    Ref = long.Parse(element.Attribute("ref")?.Value),
                    Role = element.Attribute("role")?.Value,
                };

                members.Add(member);
            }

            OsmRelation relation = new OsmRelation()
            {
                Id = id,
                Version = version,
                Timestamp = timestamp,
                Changeset = changeset,
                Tags = tags,
                Members = members,
            };

            RelationRead(relation);
        }

        #endregion

        #region Protected methods

        protected abstract void NodeRead(OsmNode node);

        protected abstract void WayRead(OsmWay way);

        protected abstract void RelationRead(OsmRelation relation);

        #endregion
    }
}
