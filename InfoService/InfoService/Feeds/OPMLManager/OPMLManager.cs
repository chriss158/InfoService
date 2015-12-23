using System;
using System.Data;
using System.Configuration;
using System.Xml;
using System.IO;
using System.Collections.Generic;
using OPMLManager.Data;

namespace OPMLManager
{
    public class OPMLManager
    {
        private string _opmlTitle = string.Empty;
        public string OpmlTitle
        {
            get
            {
                return _opmlTitle;
            }
            set
            {
                _opmlTitle = value;
                Init();
            }
        }
        private XmlElement body;
        private XmlDocument doc;
        private void Init()
        {
            doc = new XmlDocument();
            XmlDeclaration xml = doc.CreateXmlDeclaration("1.0", "", "");
            doc.AppendChild(xml);
            XmlElement opml = doc.CreateElement("opml");
            opml.SetAttribute("version", "1.1");
            doc.AppendChild(opml);


            XmlElement head = doc.CreateElement("head");
            opml.AppendChild(head);
            XmlElement title = doc.CreateElement("title");
            title.InnerText = OpmlTitle;
            head.AppendChild(title);

            body = doc.CreateElement("body");
            opml.AppendChild(body);
        }

        public OPMLManager()
        {
            Init();
        }
        public List<OPMLFeedItem> ParseOPML(string file)
        {
            List<OPMLFeedItem> list = new List<OPMLFeedItem>();
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            try
            {
                doc.Load(file);
                if (doc != null)
                {
                    System.Xml.XmlNodeList outlineList = doc.GetElementsByTagName("outline");
                    foreach (System.Xml.XmlElement elem in outlineList)
                    {

                        if (elem.GetAttribute("type").ToLower() == "rss")
                        {
                            list.Add(new OPMLFeedItem(elem.GetAttribute("title")
                               , elem.GetAttribute("htmlUrl")
                               , elem.GetAttribute("xmlUrl")
                               , elem.GetAttribute("description")));
                        }

                    }
                    return list;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }
        public void AddFeed(string title, string description, string htmlUrl, string xmlUrl)
        {
            XmlElement outline = doc.CreateElement("outline");
            outline.SetAttribute("text", title);
            outline.SetAttribute("description", description);
            outline.SetAttribute("title", title);
            outline.SetAttribute("type", "rss");
            outline.SetAttribute("Version", "RSS");
            outline.SetAttribute("htmlUrl", htmlUrl);
            outline.SetAttribute("xmlUrl", xmlUrl);
            body.AppendChild(outline);
        }
        public void Save(string fileName)
        {
            doc.Save(fileName);
        }
    }
}
