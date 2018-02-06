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
    [CliController("fill", "Allows the filling of Fabricators and Printers ( Commands: All )")]
    class ReagentController
    {
        //Don't remove these
        private enum Untouchables
        {
            DynamicCrate
        }

        [CliCommand("All", "Fills all Fabricators and Printers with 10kg of Reagents")]
        public static void FillPrinters(string pathToWorldXml)
        {

            Console.WriteLine("Filling Printers with 10kg of Reagents...");

            XmlDocument World = WorldReader.ReadWorld(pathToWorldXml);
            XmlNode ThingsRoot = World.GetElementsByTagName("Things")[0];


            _FillPrinters(ThingsRoot.ChildNodes, 10000);

            WorldReader.SaveWorld(pathToWorldXml, World);

        }

        private static void _FillPrinters(XmlNodeList Things, int Quantity)
        {

            List<string> Printers = new List<string> {
                "StructureElectronicsPrinter",
                "StructureFabricator",
                "StructureHydraulicPipeBender",
                "StructureAutolathe",
                "StructureToolManufactory"
            };

            foreach (XmlNode Thing in Things)
            {

                if (Printers.Contains(Thing.SelectSingleNode("PrefabName").InnerText)) {

                    Debug.WriteLine(Thing.SelectSingleNode("PrefabName").InnerText);

                    XmlNode Reagents = Thing.SelectSingleNode("Reagents");
                    Reagents.RemoveAll();
                    _AddAllReagents(Reagents, Quantity);

                }

            }

        }

        private static void _AddAllReagents(XmlNode Parent, int Quantity)
        {

            List<string> Reagents = new List<string> {
                "Reagents.Iron",
                "Reagents.Copper",
                "Reagents.Gold",
                "Reagents.Silver",
                "Reagents.Steel",
                "Reagents.Nickel",
                "Reagents.Electrum",
                "Reagents.Invar",
                "Reagents.Constatan",
                "Reagents.Lead",
                "Reagents.Solder",
                "Reagents.Silicon"
            };

            foreach (string Reagent in Reagents)
            {
                _AddSingleReagent(Parent, Reagent, Quantity);
            }


        }


        private static void _AddSingleReagent(XmlNode Parent, string ReagentTypeName, int Quantity)
        {

            XmlNode NewReagent = Parent.OwnerDocument.CreateNode("element", "Reagent", "");

            XmlNode NewTypeName = NewReagent.OwnerDocument.CreateNode("element", "TypeName", "");
            NewTypeName.InnerText = string.Format("{0}", ReagentTypeName);

            NewReagent.AppendChild(NewTypeName);

            XmlNode NewQuantity = NewReagent.OwnerDocument.CreateNode("element", "Quanitity", "");
            NewQuantity.InnerText = string.Format("{0}", Quantity);

            NewReagent.AppendChild(NewQuantity);

            Parent.AppendChild(NewReagent);

        }

    }

}
