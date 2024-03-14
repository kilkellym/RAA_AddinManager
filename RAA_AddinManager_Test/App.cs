#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Versioning;
using System.Windows.Markup;
using System.Windows;
using System.Linq;
using System.Reflection;


#endregion

namespace RAA_AddinManager_Test
{
	internal class App : IExternalApplication
	{
		public Result OnStartup(UIControlledApplication app)
		{
			TaskDialog.Show("Revit", "This is the Addin Manager");
			
			int newCounter = 0;
			int updateCounter = 0;

			string username = Environment.UserName;
			string revitVersion = app.ControlledApplication.VersionNumber;
			string branch = GetUserBranchAccess(username);

			string sourceAddinFolder = $"G:\\My Drive\\Courses\\C2022-002_Revit Add-in Academy\\_Release Files\\{branch}\\{revitVersion}";

			string userAppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			string localAddinFolder = $"{userAppData}\\Autodesk\\Revit\\Addins\\{revitVersion}";

			string[] sourceFiles = Directory.GetFiles(sourceAddinFolder, "*.*", SearchOption.AllDirectories);
			string[] localFiles = Directory.GetFiles(localAddinFolder, "*.*", SearchOption.AllDirectories);

			foreach (string sourceFile in sourceFiles)
			{
				string relativePath = sourceFile.Replace(sourceAddinFolder, "");
				string localFile = localAddinFolder + relativePath;

				if (!File.Exists(localFile))
				{
					string localFolder = Path.GetDirectoryName(localFile);
					if (!Directory.Exists(localFolder))
					{
						Directory.CreateDirectory(localFolder);
					}
					File.Copy(sourceFile, localFile);
					newCounter++;
				}
				else
				{
					FileInfo sourceInfo = new FileInfo(sourceFile);
					FileInfo localInfo = new FileInfo(localFile);
					
					if (sourceInfo.LastWriteTime > localInfo.LastWriteTime)
					{
						File.Copy(sourceFile, localFile, true);
						updateCounter++;
					}
				}
			}

			if(newCounter > 0 || updateCounter > 0)
			{
				string message = $"You have updates! ";
				if(newCounter > 0)
				{
					message += $"{newCounter} new add-in(s) added. ";
				}
				if(updateCounter > 0)
				{
					message += $"{updateCounter} existing add-in(s) updated.";
				}

				TaskDialog.Show("Addin Manager", message);
			}

			return Result.Succeeded;
		}

		private string GetUserBranchAccess(string userName)
		{
			// check if user is in the branch access list
			// if not in the list, return "Main"
			return "Main";
		}

		public Result OnShutdown(UIControlledApplication a)
		{
			return Result.Succeeded;
		}


	}
}
