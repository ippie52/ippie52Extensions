using System;
using System.Collections.Generic;
using System.Xml;

namespace ippie52Extensions
{
    public static class XmlExtensionMethods
    {   
        /// <summary>
        /// Method used to populate the contents of an element
        /// </summary>
        /// <param name="writer">The XmlWriter to write to</param>
        public delegate void XmlElementContents(XmlWriter writer);

        /// <summary>
        /// Method used to write an element to the XmlWriter, and populate the contents
        /// using the contents method
        /// </summary>
        /// <param name="writer">The XmlWriter to write to</param>
        /// <param name="element">The element name to be written</param>
        /// <param name="contents">The method used to populate the contents of the element</param>
        public static void WriteElement(this XmlWriter writer, String element, XmlElementContents contents)
        {
            writer.WriteStartElement(element);
            contents(writer);
            writer.WriteEndElement();
        }

        /// <summary>
        /// Delegate for methods used to read their associated XML data
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public delegate bool XmlReadMethod(XmlReader reader);

        /// <summary>
        /// Method checks that the current element name matches the expected element name,
        /// and then calls an associated method to handle the contents of the current
        /// element
        /// </summary>
        /// <param name="reader">The XmlReader to read from</param>
        /// <param name="expected">The expected element name</param>
        /// <param name="method">The method to call if the names match</param>
        /// <returns>True if the names match and the function returns successfully</returns>
        public static bool ReadElement(this XmlReader reader, String expected, XmlReadMethod method)
        {
            bool result = (reader.NodeType == XmlNodeType.Element) &&
                (expected.Equals(reader.Name, StringComparison.CurrentCultureIgnoreCase));
            if (result)
            {
                result = method(reader);
            }
            return result;
        }

        /// <summary>
        /// Method used to skip to the next element in the XmlReader
        /// </summary>
        /// <param name="reader">The XmlReader object of which to get the next element</param>
        /// <returns>True if no exceptions occured when trying to find the next element</returns>
        public static bool SkipToNextElement(this XmlReader reader)
        {
            bool result = true;
            try { while (reader.Read() && !reader.NodeType.Equals(XmlNodeType.Element)) ; }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }

        /// <summary>
        /// Method used to skip to the next element in the XmlReader
        /// </summary>
        /// <param name="reader">The XmlReader object of which to get the next element</param>
        /// <returns>True if no exceptions occured when trying to find the next element</returns>
        public static bool SkipToNextElement(this XmlReader reader, String elementName)
        {
            bool result = true;
            try 
            { 
                while 
                (
                    reader.Read() && 
                    !reader.NodeType.Equals(XmlNodeType.Element) && 
                    !reader.Name.Equals(elementName)
                ) ; 
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }

        /// <summary>
        /// Method used to get the number stored in a count attribute
        /// </summary>
        /// <param name="reader">The XmlReader to read from</param>
        /// <returns>Number of elements contained</returns>
        public static int GetElementCount(this XmlReader reader)
        {
            int count = 0;
            if (reader.HasAttributes)
            {
                count = Convert.ToInt32(reader.GetAttribute(COUNT_ATT));
            }
            return count;
        }

        /// <summary>
        /// Method used to set the number stored in a count attribute
        /// </summary>
        /// <param name="writer">The XmlWriter to write to</param>
        /// <param name="count">The count to be written</param>
        public static void SetElementCount(this XmlWriter writer, int count)
        {
            writer.WriteAttributeString(COUNT_ATT, count.ToString());
        }

        /// <summary>
        /// Count attribute for XML files
        /// </summary>
        public static readonly String COUNT_ATT = "count";
    }
}
