using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageViewer.Service.File
{
    public class XMLFileService
    {
        public void CreateXMLFile(string filePath, string content)
        {
           var xml = new System.Xml.Linq.XDocument(
                new System.Xml.Linq.XElement("Root",
                    new System.Xml.Linq.XElement("Content", content)
                )
            );
            xml.Save(filePath);
        }
    }
}
