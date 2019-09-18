using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace OpenStreetMapParser
{
    internal static class ExternsionMethods
    {
        public static double GetAttributeDouble(this XElement element, string name, double defaultVal = 0)
        {
            XAttribute attribute = element.Attribute(name);

            if (attribute == null)
                return defaultVal;

            if (double.TryParse(attribute.Value, out var result))
                return result;

            return defaultVal;
        }

        public static long GetAttributeLong(this XElement element, string name, long defaultVal = 0)
        {
            XAttribute attribute = element.Attribute(name);

            if (attribute == null)
                return defaultVal;

            if (long.TryParse(attribute.Value, out var result))
                return result;

            return defaultVal;
        }

        public static int GetAttributeInt(this XElement element, string name, int defaultVal = 0)
        {
            XAttribute attribute = element.Attribute(name);

            if (attribute == null)
                return defaultVal;

            if (int.TryParse(attribute.Value, out var result))
                return result;

            return defaultVal;
        }
        
        public static DateTime GetAttributeDateTime(this XElement element, string name, DateTime defaultVal)
        {
            XAttribute attribute = element.Attribute(name);

            if (attribute == null)
                return defaultVal;

            if (DateTime.TryParse(attribute.Value, out var result))
                return result;

            return defaultVal;
        }
    }
}
