﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
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
            loadXmlFile();
        }

        public void loadXmlFile() {
            try
            {
                while (xmlDocument.IsReadOnly)
                {
                }
                xmlDocument.Load(path);
            }
            catch (Exception e)
            {
                // DANGER do not uncomment for your safety :')
                Console.WriteLine(e.Message + " " + e.StackTrace);
                loadXmlFile();
            }
            
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
