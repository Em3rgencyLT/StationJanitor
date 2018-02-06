using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using CommandAndConquer.CLI.Attributes;

namespace StationJanitor.Controllers
{
	[CliController("run", "The main (and currently only) command namespace.")]
	public static class MainController
	{
		//Don't remove these
		private enum Untouchables
		{
			DynamicCrate
		}

		[CliCommand("removeTrash", "Removes all objects lying on the ground")]
		public static void MethodExample(string pathToWorldXml)

		{

			string originalXmlPath = pathToWorldXml + "\\world.xml";
			if(!File.Exists(originalXmlPath))
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
			if(!thingsRoot.HasChildNodes)
			{
				Console.WriteLine("Could not find any trash on this world.");
				return;
			}

			XmlNodeList things = thingsRoot.ChildNodes;
			List<XmlNode> removeList = new List<XmlNode>();

			Console.WriteLine("Assembling trash list...");
			//"0" id as that is what is assigned for things on the ground. Then if that object has children, they will be recursivelly removed.
			RemoveChildrenRecursive("0", things, ref removeList);

			int removed = 0;
			foreach (XmlNode trash in removeList)
			{
				Console.WriteLine("Removing " + trash.SelectSingleNode("PrefabName").InnerText);
				trash.ParentNode.RemoveChild(trash);
				removed++;
			}

			world.Save(originalXmlPath);

			Console.WriteLine("Completed! Removed " + removed.ToString() + " trash.");
			return;
		}

		private static void RemoveChildrenRecursive(string recursiveReferenceId, XmlNodeList things, ref List<XmlNode> removeList)
		{
			foreach (XmlNode thing in things)
			{
				XmlNode prefab = thing.SelectSingleNode("PrefabName");

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
					RemoveChildrenRecursive(referenceId.InnerText, things, ref removeList);
				}
			}
		}
	}
}