using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Client
{
    internal class FileManager
    {
        private String path;

        public FileManager(string path)
        {
            this.path = path;
        }

        public void UpdateAttribute(String newValue)
        {
            XmlDocument xmlDocument = new XmlDocument();

            xmlDocument.Load(path);

            XmlNode node = xmlDocument.SelectSingleNode("psatsim/config");
            node.Attributes[0].Value = newValue;

            xmlDocument.Save(path);
        }


    }
}
