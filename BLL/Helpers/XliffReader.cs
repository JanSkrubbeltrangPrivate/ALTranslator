using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using BLL.Models;

namespace BLL.Helpers
{
    public class XliffReader
    {
        public int Count
        {
            get { return CountTransUnits(); }
        }

        private XmlDocument doc;
        public XliffReader()
        {
            doc = new();
        }
        public IEnumerable<TransUnit> GetAllTransUnits()
        {
            List<TransUnit> finalUnits = new();
            var sourceUnits = GetTransUnits();
            if (sourceUnits != null)
                foreach (var sourceUnit in sourceUnits)
                {
                    finalUnits.Add(ConvertTransUnitFromXmlElement((XmlElement)sourceUnit));
                }

            return finalUnits;
        }
        public void LoadDocument(string Filename)
        {
            using (FileStream f = new(Filename, FileMode.Open))
            {
                doc.Load(f);
            }
        }

        public override string ToString()
        {
            using (StringWriter sw = new())
            {
                doc.Save(sw);
                return sw.ToString();
            }
        }

        public TransUnit GetTransUnitByIndex(int index)
        {
            var nodeList = GetTransUnits();
            if (nodeList == null)
                throw (new XmlException("Invalid document, could not find nodes"));

            if (nodeList.Count == 0)
                throw (new XmlException("Node is empty"));

            var node = nodeList.Item(index);
            if (node == null)
                throw (new XmlException($"Could not find node {index}"));

            return ConvertTransUnitFromXmlElement((XmlElement)node);
        }
        private TransUnit ConvertTransUnitFromXmlElement(XmlElement input)
        {
            TransUnit unit = new();
            unit.Id = input.GetAttribute("id");
            foreach (var element in input.ChildNodes)
            {
                if (element is XmlElement)
                {
                    XmlElement element2 = (XmlElement)element;
                    switch (element2.Name)
                    {
                        case "source": 
                            unit.Source = element2.InnerText;
                            break;
                        case "target": 
                            unit.Target = element2.InnerText;
                            break;
                        case "note": 
                            Note n = new();
                            n.From = element2.GetAttribute("from");
                            n.Priority = element2.GetAttribute("priority");
                            n.Value = element2.InnerText;
                            unit.Notes.Add(n);
                            break;

                        default:
                            break;
                    }
                }

            }
            return unit;
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

    }
}