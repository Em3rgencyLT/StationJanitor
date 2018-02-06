using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandAndConquer.CLI.Attributes;
using System.Diagnostics;
using System.Xml;

namespace StationJanitor.Controllers

    // Still working on that
{
    [CliController("lockers", "Allows the filling of Lockers ( Work in Progress )")]
    class LockerController
    {
        [CliCommand("Lockers", "Fills all lockers based on their name")]
        public static void FillLockers(string pathToWorldXml)
        {

            Console.WriteLine("Filling Printers with 10kg of Reagents...");

            XmlDocument World = WorldReader.ReadWorld(pathToWorldXml);
            XmlNode ThingsRoot = World.GetElementsByTagName("Things")[0];

            _FillLockers(ThingsRoot.ChildNodes);

            WorldReader.SaveWorld(pathToWorldXml, World);

        }

        private static void _FillLockers(XmlNodeList Things)
        {
            // Lockers

            foreach (XmlNode Thing in Things)
            {

                if (Thing.SelectSingleNode("PrefabName").InnerText == "StructureStorageLocker")
                {

                    string CustomName = Thing.SelectSingleNode("CustomName").InnerText;
                    string ReferenceID = Thing.SelectSingleNode("ReferenceId").InnerText;

                    if (CustomName.StartsWith("Locker|"))

                    {
                        // _RemoveLockerContent(ReferenceID, Things);
                    }

                }

            }

        }

        private static void _RemoveLockerContent(string ID, XmlNodeList Things)
        {

            List<XmlNode> ContentToDelete = new List<XmlNode>();

            foreach (XmlNode Content in Things)
            {
                if(Content.SelectSingleNode("ParentReferenceId").InnerText == ID)
                {
                    ContentToDelete.Add(Content);
                }
            }

            if(ContentToDelete.Count > 0)
            {
                foreach (XmlNode Trash in ContentToDelete)
                {                    
                    Trash.ParentNode.RemoveChild(Trash);
                }
            }
        
        }

    }

}
