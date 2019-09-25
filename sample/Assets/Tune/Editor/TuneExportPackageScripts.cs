// Call this command from your local sdk-unity folder via your command line terminal:
/* 
 /Applications/Unity/Unity.app/Contents/MacOS/Unity -quit -batchmode -serial <Unity Seat Serial Number> -username '<Unity username>' -password '<Unity Seat PW>' -projectPath <Path to unity-plugin/sample> -logfile -executeMethod TuneExportPackageScripts.GenerateExportPackage
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEditor;
using UnityEditorInternal;
using System.Linq;

public class TuneExportPackageScripts : MonoBehaviour
{

	static void GenerateExportPackage()
	{
		string[] filePaths = GetFilePaths();
		AssetDatabase.ExportPackage(filePaths, "../Tune.unitypackage", ExportPackageOptions.Default);
	}

	static string[] GetFilePaths()
	{
		List<string> shortenedFilePathList = new List<string>();

		try
		{
			string dataPath = Application.dataPath;

			/* Excludes files not in the Assets/Plugins or Assets/Tune folders;
			skips meta/gitignore/temportary files &
			specific files we do not want to include in the package */
			IEnumerable<string> fullFilePaths = Directory.GetFiles(dataPath, "*.*", SearchOption.AllDirectories)
				.Where(s => (s.Contains("Assets/Plugins") || s.Contains("Assets/Tune")) &&
				(!s.EndsWith("/Tune/Editor/CIBuildScripts.cs") &&
				!s.EndsWith("/Tune/Editor/TuneExportPackageScripts.cs") &&
				!s.EndsWith("/Tune/Plugins/Android/MyApplication.jar") &&
				!s.EndsWith("/Tune/Plugins/Android/com/hasoffers/unitytestapp/MyApplication.java") &&
				!s.EndsWith("/Tune/Plugins/iOS/MyAppDelegate.mm") &&
				!s.EndsWith(".meta") &&
				!s.EndsWith(".gitignore") &&
				!s.EndsWith(".DS_Store") &&
				!s.Contains("/Plugins/Android/android.") &&
				!s.Contains("/Plugins/Android/com.")));

			foreach (string fullPath in fullFilePaths)
			{
				int filePathStart = fullPath.IndexOf("Assets/");
				int substringLength = fullPath.Length - filePathStart;
				string shortenedFilePath = fullPath.Substring(filePathStart, substringLength);

				// adds just the relevant filepath substring and not the full system filepath
				shortenedFilePathList.Add(shortenedFilePath);
			}

			string[] shortenedFilePathArray = shortenedFilePathList.ToArray();
			return shortenedFilePathArray;
		}

		catch (UnauthorizedAccessException UAEx)
		{
			Console.WriteLine(UAEx.Message);
		}

		catch (PathTooLongException PathEx)
		{
			Console.WriteLine(PathEx.Message);
		}

		return null;
	}
}

