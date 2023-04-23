using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Xml;

using System.Runtime.Serialization.Formatters.Binary;

namespace xml_si_serializare
{
    internal class Program
    {
        public static void binaryDeserialization()
        {
            FileStream fileStream = new FileStream("info.dat", FileMode.Open, FileAccess.Read);
            BinaryFormatter formatter = new BinaryFormatter();
            List<Entity> entities = (List<Entity>)formatter.Deserialize(fileStream);
            foreach(Entity entity in entities)
            {
                Console.WriteLine(entity.Id + " " + entity.Name);
            }
        }
        public static void binarySerialization(List<Entity> entities)
        {
            FileStream sw = new FileStream("info.dat", FileMode.Create, FileAccess.Write);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(sw, entities);
            sw.Close();
        }
        public static void xmlReader()
        {
            StreamReader sr = new StreamReader("info.xml");
            string str = sr.ReadToEnd();
            sr.Close();

            XmlReader xmlReader = XmlReader.Create(new StringReader(str));
            while(xmlReader.Read())
            {
                if (xmlReader.NodeType == XmlNodeType.Element)
                {
                    if (xmlReader.Name == "Entity")
                    {
                        Console.Write(xmlReader["ID"] + " ");
                        xmlReader.Read();
                        Console.WriteLine(xmlReader.Value);
                    }
                }
            }
            xmlReader.Close();
        }

        public static void xmlConverter(List<Entity> entities)
        {
            MemoryStream memStream = new MemoryStream();
            XmlTextWriter xmlTextWriter = new XmlTextWriter(memStream, Encoding.UTF8);
            xmlTextWriter.Formatting = Formatting.Indented;

            xmlTextWriter.WriteStartDocument();
            xmlTextWriter.WriteStartElement("Entities");
            foreach(Entity entity in entities)
            {
                xmlTextWriter.WriteStartElement("Entity");
                    xmlTextWriter.WriteStartAttribute("ID");
                        xmlTextWriter.WriteValue(entity.Id);
                    xmlTextWriter.WriteEndAttribute();

                            xmlTextWriter.WriteValue(entity.Name);
                xmlTextWriter.WriteEndElement();
            }
            xmlTextWriter.WriteEndElement();
            xmlTextWriter.WriteEndDocument();
            xmlTextWriter.Close(); // <-- importan!

            string xmlString = Encoding.UTF8.GetString(memStream.ToArray());
            memStream.Close(); // <-- importan!

            StreamWriter sw = new StreamWriter("info.xml");
            sw.WriteLine(xmlString);
            sw.Close(); // <-- importan!
        }

        static void Main(string[] args)
        {
            Entity entity_1 = new Entity(1, "Entity one");
            Entity entity_2 = new Entity(2, "Entity two");
            Entity entity_3 = new Entity(3, "Entity three");
            Entity entity_4 = new Entity(4, "Entity four");
            Entity entity_5 = new Entity(5, "Entity five");

            List<Entity> entities = new List<Entity>() 
            {entity_1, entity_2, entity_3, entity_4, entity_5};

            //xmlConverter(entities);
            //xmlReader();
            //binarySerialization(entities);
            //binaryDeserialization();
        }
    }
}