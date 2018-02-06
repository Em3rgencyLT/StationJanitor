using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using CommandAndConquer.CLI.Attributes;
using System.Diagnostics;

namespace StationJanitor.Controllers
{
    [CliController("run", "The main (and currently only) command namespace ( Commands: removeTrash )")]
    public static class MainController

    {

        //Don't remove these
        private enum Untouchables
        {
            DynamicCrate,
            Character // Addes this so that the Characters remain intact
        }

        [CliCommand("removeTrash", "Removes all objects lying on the ground")]
        public static void MethodExample(string pathToWorldXml)

        {

            string originalXmlPath = pathToWorldXml + "\\world.xml";
            if (!File.Exists(originalXmlPath))
            {
                Console.WriteLine("Could not find world.xml at " + pathToWorldXml);
                return;
            }

            string backupXmlPath = originalXmlPath + ".original";
            Console.WriteLine("Making backup of original file to " + backupXmlPath);
            File.Copy(originalXmlPath, backupXmlPath, true);

            XmlDocument world = new XmlDocument();
            world.Load(originalXmlPath);

            XmlNode thingsRoot = world.GetElementsByTagName("Things")[0];

            //No things! We're done here.
            if (!thingsRoot.HasChildNodes)
            {
                Console.WriteLine("Could not find any trash on this world.");
                return;
            }

            XmlNodeList things = thingsRoot.ChildNodes;
            List<XmlNode> removeList = new List<XmlNode>();
            List<string> PrefabNames = new List<string>();

            Console.WriteLine("Assembling trash list...");

            // This will remove dead Characters from the top
            // _RemoveDeadCharacters(things, ref removeList);

            //"0" id as that is what is assigned for things on the ground. Then if that object has children, they will be recursivelly removed.
            RemoveChildrenRecursive("0", things, ref removeList, ref PrefabNames);

            int removed = 0;
            foreach (XmlNode trash in removeList)
            {
                Console.WriteLine("Removing " + trash.SelectSingleNode("PrefabName").InnerText);
                trash.ParentNode.RemoveChild(trash);
                removed++;
            }

            if (Debugger.IsAttached) {

                foreach (string PrefabName in PrefabNames)
                {
                    Debug.WriteLine(String.Format("PrefabName: {0}", PrefabName));
                }

            }

            world.Save(originalXmlPath);

            Console.WriteLine("Completed! Removed " + removed.ToString() + " trash.");
            return;
        }

        private static void RemoveChildrenRecursive(string recursiveReferenceId, XmlNodeList things, ref List<XmlNode> removeList, ref List<string> PrefabNames)
        {

            foreach (XmlNode thing in things)
            {
                XmlNode prefab = thing.SelectSingleNode("PrefabName");

                if (!PrefabNames.Contains(thing.SelectSingleNode("PrefabName").InnerText))
                {
                    PrefabNames.Add(thing.SelectSingleNode("PrefabName").InnerText);

                }

                //Skip things we don't want removed
                if (Enum.IsDefined(typeof(Untouchables), prefab.InnerText))
                {
                    continue;
                }

                XmlNode referenceId = thing.SelectSingleNode("ReferenceId");
                XmlNode parentReferenceId = thing.SelectSingleNode("ParentReferenceId");
                XmlNode parentSlotId = thing.SelectSingleNode("ParentSlotId");

                //Probably a structure or something
                if (parentSlotId == null)
                {
                    continue;
                }

                if (parentReferenceId.InnerText == recursiveReferenceId)
                {
                    removeList.Add(thing);
                    RemoveChildrenRecursive(referenceId.InnerText, things, ref removeList, ref PrefabNames);
                }
            }
        }

        private static void _RemoveDeadCharacters(XmlNodeList Things, ref List<XmlNode> ThingsToRemove)
        {

            foreach (XmlNode Thing in Things)
            {
                if(Thing.SelectSingleNode("PrefabName").InnerText == "Character" && !(Thing.SelectSingleNode("State").InnerText == "Alive")) {
                    ThingsToRemove.Add(Thing);
                }
            }

        }

    }

}