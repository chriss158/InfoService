using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Xml;

namespace InfoService.GUIConfiguration
{
    public partial class WeatherForm : Form
    {
        ArrayList cities;
        public class City
        {
            public string Name;
            public string Id;

            public City(string name, string id)
            {
                this.Name = name;
                this.Id = id;
            }

            public override string ToString()
            {
                return Name;
            }
        }
        private string _locationCode;
        public string WeatherLocationCode
        {
            set
            {
                _locationCode = value;
            }
            get
            {
                return _locationCode;
            }
        }
        private string _location;
        public string WeatherLocation
        {
            set
            {
                _location = value;
            }
            get
            {
                return _location;
            }
        }
        public WeatherForm(string Location)
        {
            InitializeComponent();
            _location = Location;
        }
        public ArrayList SearchCity(string searchString)
        {
            ArrayList result = new ArrayList();

            try
            {
                string searchURI = String.Format("http://xoap.weather.com/search/search?where={0}", UrlEncode(searchString));

                // Create the request and fetch the response
                WebRequest request = WebRequest.Create(searchURI);
                try
                {
                    // Use the current user in case an NTLM Proxy or similar is used.
                    // wr.Proxy = WebProxy.GetDefaultProxy();
                    request.Proxy.Credentials = CredentialCache.DefaultCredentials;
                }
                catch (Exception) { }
                WebResponse response = request.GetResponse();

                // Read data from the response stream
                Stream responseStream = response.GetResponseStream();
                Encoding iso8859 = Encoding.GetEncoding("iso-8859-1");
                StreamReader streamReader = new StreamReader(responseStream, iso8859);

                // Fetch information from our stream
                string data = streamReader.ReadToEnd();

                XmlDocument document = new XmlDocument();
                document.LoadXml(data);

                XmlNodeList nodes = document.DocumentElement.SelectNodes("/search/loc");

                if (nodes != null)
                {
                    // Iterate through our results
                    foreach (XmlNode node in nodes)
                    {
                        string name = node.InnerText;
                        string id = node.Attributes["id"].Value;

                        result.Add(new City(name, id));
                    }
                }
            }
            catch (Exception)
            {
                // Failed to perform search
                throw new ApplicationException("Failed to perform city search, make sure you are connected to the internet.");
            }

            return result;
        }

        public string UrlEncode(string instring)
        {
            StringReader strRdr = new StringReader(instring);
            StringWriter strWtr = new StringWriter();
            int charValue = strRdr.Read();
            while (charValue != -1)
            {
                if (((charValue >= 48) && (charValue <= 57)) // 0-9
                    || ((charValue >= 65) && (charValue <= 90)) // A-Z
                    || ((charValue >= 97) && (charValue <= 122))) // a-z
                {
                    strWtr.Write((char)charValue);
                }
                else if (charValue == 32) // Space
                {
                    strWtr.Write("+");
                }
                else
                {
                    strWtr.Write("%{0:x2}", charValue);
                }

                charValue = strRdr.Read();
            }

            return strWtr.ToString();
        }

        private void WeatherForm_Load(object sender, EventArgs e)
        {
            cities = SearchCity(_location);
            foreach (City city in cities)
            {
                lvCities.Items.Add(new ListViewItem(new string[] {city.Name, city.Id}));
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (lvCities.SelectedItems.Count > 0)
            {
                City city = (City)cities[lvCities.SelectedItems[0].Index];
                _locationCode = city.Id;
                _location = city.Name;
                Close();
            }
            
        }
    }
}
