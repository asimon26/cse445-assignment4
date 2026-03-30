using System;
using System.Xml.Schema;
using System.Xml;
using Newtonsoft.Json;
using System.IO;



/**
 * This template file is created for ASU CSE445 Distributed SW Dev Assignment 4.
 * Please do not modify or delete any existing class/variable/method names. However, you can add more variables and functions.
 * Uploading this file directly will not pass the autograder's compilation check, resulting in a grade of 0.
 * **/


namespace ConsoleApp1
{


    public class Submission
    {
        public static string xmlURL = "Your XML URL";
        public static string xmlErrorURL = "Your Error XML URL";
        public static string xsdURL = "Your XSD URL";

        public static void Main(string[] args)
        {
            string result = Verification(xmlURL, xsdURL);
            Console.WriteLine(result);


            result = Verification(xmlErrorURL, xsdURL);
            Console.WriteLine(result);


            result = Xml2Json(xmlURL);
            Console.WriteLine(result);
        }

        // Q2.1
        public static string Verification(string xmlUrl, string xsdUrl)
        {
            try
            {
                string xsdContent = DownloadContent(xsdUrl);
                XmlSchemaSet schemaSet = new XmlSchemaSet();
                schemaSet.Add(null, XmlReader.Create(new StringReader(xsdContent)));

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.Schemas = schemaSet;
                settings.ValidationType = ValidationType.Schema;

                string errors = "";
                settings.ValidationEventHandler += (sender, e) =>
                {
                    errors += e.Message + "\n";
                };

                string xmlContent = DownloadContent(xmlUrl);
                using (XmlReader reader = XmlReader.Create(new StringReader(xmlContent), settings))
                {
                    while (reader.Read()) { }
                }

                if (string.IsNullOrEmpty(errors))
                {
                    return "No errors are found";
                }
                else
                {
                    return errors.TrimEnd('\n');
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static string Xml2Json(string xmlUrl)
        {
            string xmlContent = DownloadContent(xmlUrl);
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlContent);

            if (doc.DocumentElement.Attributes["xmlns:xsi"] != null)
            {
                doc.DocumentElement.RemoveAttribute("xmlns:xsi");
            }
            if (doc.DocumentElement.Attributes["xsi:noNamespaceSchemaLocation"] != null)
            {
                doc.DocumentElement.RemoveAttribute("xsi:noNamespaceSchemaLocation");
            }

            string jsonText = JsonConvert.SerializeXmlNode(doc.DocumentElement);

            return jsonText;
        }

        private static string DownloadContent(string url)
        {
            using (System.Net.WebClient client = new System.Net.WebClient())
            {
                return client.DownloadString(url);
            }
        }
    }
}