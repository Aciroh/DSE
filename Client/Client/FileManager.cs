using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Client
{
    internal class FileManager
    {
        private String path;
        XmlDocument xmlDocument = new XmlDocument();

        public FileManager(string path)
        {
            this.path = path;
            xmlDocument.Load(path);
            //xmlDocument.LoadXml(path);
        }

        public void PrintNodeAttribute(String targetNode, int attributeIndex)
        {
            
            String nodePath = "psatsim/" + targetNode;
            XmlNode node = xmlDocument.SelectSingleNode(nodePath);

            //XmlNodeList nodeAttribute = xmlDocument.GetElementsByTagName(targetNode);
            String nodeAttribute = node.Attributes[attributeIndex].InnerText;
            Console.WriteLine(nodeAttribute);
        }

        public void UpdateAttribute(int attributeIndex, String newValue, String targetNode)
        {
            String nodePath = "psatsim/" + targetNode;

            XmlNode node = xmlDocument.SelectSingleNode(nodePath);
   
            node.Attributes[attributeIndex].Value = newValue;

            xmlDocument.Save(path);
        }

        public String ReadAttribute(int attributeIndex, String targetNode)
        {
            String nodePath = "psatsim_results/" + targetNode;

            XmlNode node = xmlDocument.SelectSingleNode(nodePath);

            return node.Attributes[attributeIndex].Value;
        }


    }
}
