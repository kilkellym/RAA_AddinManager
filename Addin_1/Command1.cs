#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

#endregion

namespace Addin_1
{
	[Transaction(TransactionMode.Manual)]
	public class Command1 : IExternalCommand
	{
		public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
		{
			// this is a variable for the Revit application
			UIApplication uiapp = commandData.Application;

			// this is a variable for the current Revit model
			Document doc = uiapp.ActiveUIDocument.Document;

			// Your code goes here
			string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
			TaskDialog.Show("Revit", $"Hello, Revit! This is command 1, version {version}");

			return Result.Succeeded;
		}
		internal static PushButtonData GetButtonData()
		{
			// use this method to define the properties for this command in the Revit ribbon
			string buttonInternalName = "btnCommand1";
			string buttonTitle = "Button 1";

			ButtonDataClass myButtonData1 = new ButtonDataClass(
				buttonInternalName,
				buttonTitle,
				MethodBase.GetCurrentMethod().DeclaringType?.FullName,
				Properties.Resources.Blue_32,
				Properties.Resources.Blue_16,
				"This is a tooltip for Button 1");

			return myButtonData1.Data;
		}
	}
}
