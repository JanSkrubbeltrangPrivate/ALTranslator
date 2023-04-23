using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using BLL.Models;

namespace BLL.Helpers
{
    public class XliffWriter
    {
        public int Count
        {
            get { return CountTransUnits(); }
        }

        private XmlDocument doc;
        private XmlElement? group;
        private XmlElement? transUnit;

        public XliffWriter()
        {
            doc = new();
        }

        public void AddTransUnitToXliff(TransUnit transUnit)
        {
            NewTransUnit(transUnit.Id);
            AddTransUnitSource(transUnit.Source);
            AddTransUnitTarget(transUnit.Target);
            foreach (var note in transUnit.Notes)
            {
                AddTransUnitNote(note.From, note.Priority, note.Value);
            }
            AddTransUnitToGroup();
        }
        public void NewDocument(string targetLanguage = "en-US", string sourceLanguage = "en-US")
        {
            doc = new();
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", null, null);
            doc.AppendChild(docNode);

            XmlElement rootNode = doc.CreateElement("xliff");

            rootNode.Attributes.Append(CreateAttribute("version", "1.2", doc));
            rootNode.Attributes.Append(CreateAttribute("xmlns", "urn:oasis:names:tc:xliff:document:1.2", doc));
            rootNode.Attributes.Append(CreateAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance", doc));
            rootNode.Attributes.Append(CreateAttribute("xsi:schemaLocation", "http://www.w3.org/2001/XMLSchema-instance", "urn:oasis:names:tc:xliff:document:1.2 xliff-core-1.2-transitional.xsd", doc));
            doc.AppendChild(rootNode);

            XmlElement fileNode = doc.CreateElement("file");
            fileNode.Attributes.Append(CreateAttribute("datatype", "xml", doc));
            fileNode.Attributes.Append(CreateAttribute("source-language", sourceLanguage, doc));
            fileNode.Attributes.Append(CreateAttribute("target-language", targetLanguage, doc));
            //fileNode.Attributes.Append(CreateAttribute("orginal", "Base Application", doc));
            rootNode.AppendChild(fileNode);

            XmlElement bodyNode = doc.CreateElement("body");
            fileNode.AppendChild(bodyNode);

            group = doc.CreateElement("group");
            group.Attributes.Append(CreateAttribute("id", "body", doc));
            bodyNode.AppendChild(group);
        }

        public void SaveDocument(string filename)
        {
            doc.Save(filename);
        }

        public override string ToString()
        {
            using (StringWriter sw = new())
            {
                doc.Save(sw);
                return sw.ToString();
            }
        }

        private XmlNodeList? GetTransUnits()
        {
            return doc.SelectNodes("//ns:xliff/ns:file/ns:body/ns:group/ns:trans-unit", getNameSpaceManager());
        }
        
        private XmlNamespaceManager getNameSpaceManager()
        {
            XmlNamespaceManager n = new(doc.NameTable);
            n.AddNamespace("ns", "urn:oasis:names:tc:xliff:document:1.2");
            return n;
        }
        
        private int CountTransUnits()
        {
            var nodeList = GetTransUnits();
            if (nodeList != null)
                return nodeList.Count;
            else
                return 0;
        }
        
        private XmlAttribute CreateAttribute(string name, string value, XmlDocument Owner)
        {
            XmlAttribute attribute = Owner.CreateAttribute(name);
            attribute.Value = value;
            return attribute;
        }

        private XmlAttribute CreateAttribute(string name, string nameSpace, string value, XmlDocument Owner)
        {
            XmlAttribute attribute = Owner.CreateAttribute(name, nameSpace);
            attribute.Value = value;
            return attribute;
        }

        private void NewTransUnit(string Id)
        {
            transUnit = doc.CreateElement("trans-unit");
            transUnit.Attributes.Append(CreateAttribute("id", Id, doc));
            transUnit.Attributes.Append(CreateAttribute("maxwidth", "0", doc));
            transUnit.Attributes.Append(CreateAttribute("size-unit", "char", doc));
            transUnit.Attributes.Append(CreateAttribute("translate", "yes", doc));
            transUnit.Attributes.Append(CreateAttribute("xml:space", "preserve", doc));
        }

        private void AddTransUnitSource(string source)
        {
            if (transUnit == null)
                return;

            XmlElement node = doc.CreateElement("source");
            node.InnerText = source;
            transUnit.AppendChild(node);
        }

        private void AddTransUnitTarget(string target)
        {
            if (transUnit == null)
                return;

            XmlElement node = doc.CreateElement("target");
            node.InnerText = target;
            node.Attributes.Append(CreateAttribute("state", "translated", doc));
            transUnit.AppendChild(node);
        }

        private void AddTransUnitNote(string from, string priority, string note)
        {
            if (transUnit == null)
                return;

            XmlElement element = doc.CreateElement("note");
            element.InnerText = note;
            element.Attributes.Append(CreateAttribute("from", from, doc));
            element.Attributes.Append(CreateAttribute("annotates", "general", doc));
            element.Attributes.Append(CreateAttribute("priority", priority, doc));
            transUnit.AppendChild(element);
        }

        private void AddTransUnitToGroup()
        {
            if (group != null && transUnit != null)
                group.AppendChild(transUnit);
        }
    }
}