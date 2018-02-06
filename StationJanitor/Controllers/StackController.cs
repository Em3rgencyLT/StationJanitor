using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandAndConquer.CLI.Attributes;
using System.Diagnostics;
using System.Xml;

namespace StationJanitor.Controllers
{
    [CliController("stack", "Allows the filling of Fabricators etc.")]
    class StackController
    {
        [CliCommand("Ingots", "Fills all printers with 10kg of Reagents")]
        public static void MaxIngots(string pathToWorldXml)
        {

            Console.WriteLine("Setting all ingots to 500 items in stack");

            XmlDocument World = WorldReader.ReadWorld(pathToWorldXml);
            XmlNode ThingsRoot = World.GetElementsByTagName("Things")[0];

            _MaxIngots(ThingsRoot.ChildNodes, "500");

            WorldReader.SaveWorld(pathToWorldXml, World);

        }

        private static void _MaxIngots(XmlNodeList Things, string StackSize)
        {
            // Ingots to 500

            foreach (XmlNode Thing in Things)
            {

                if (Thing.SelectSingleNode("PrefabName").InnerText.EndsWith("Ingot")) 
                {

                    Thing.SelectSingleNode("Quantity").InnerText = StackSize;

                }

            }

        }

    }

}
