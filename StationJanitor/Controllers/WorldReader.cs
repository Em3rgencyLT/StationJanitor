using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;


namespace StationJanitor.Controllers
{
    class WorldReader
    {

        public static XmlDocument ReadWorld(String PathToXML)
        {

            XmlDocument World = new XmlDocument();

            string originalXmlPath = System.IO.Path.Combine(PathToXML, "world.xml");

            if (!File.Exists(originalXmlPath))
            {
                Console.WriteLine("Could not find world.xml at " + PathToXML);
                return null;
            }

            string backupXmlPath = originalXmlPath + String.Format(".{0:yyyyMMddHHmmss}.original", DateTime.Now);

            Console.WriteLine("Making backup of original file to " + backupXmlPath);
            File.Copy(originalXmlPath, backupXmlPath, true);

            World.Load(originalXmlPath);

            if (World.GetElementsByTagName("Things").Count == 0 )
            {
                Console.WriteLine("Could not find any trash on this world.");
                return null;
            }

            return World;

        }

        public static void SaveWorld(String PathToXml, XmlDocument World)        
        {

            string originalXmlPath = System.IO.Path.Combine(PathToXml, "world.xml");
            World.Save(originalXmlPath);


        }


    }

}
