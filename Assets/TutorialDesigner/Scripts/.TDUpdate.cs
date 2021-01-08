using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Reflection;

namespace TutorialDesigner {
	/// <summary>
	/// Update Class that will clean up files from old version and update old versions in general
	/// </summary>
    [InitializeOnLoad]
    public static class TDUpdate {
        // Clear Debug Log
        public static void ClearLogs() {
            var logEntries = System.Type.GetType("UnityEditor.LogEntries, UnityEditor.dll");
            var clearMethod = logEntries.GetMethod("Clear", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
            clearMethod.Invoke(null, null);
        }

        static TDUpdate() {   
            string mainScript = AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets("TutorialEditor")[0]);
            string TDPath = mainScript.Substring(0, mainScript.IndexOf("Scripts"));

            // Search files / folders for removal from old version
            List<string> files = new List<string>();
            files.Add(TDPath + "MultiLanguage.unitypackage");
            files.Add(TDPath + "SmartLocalization/SmartLocalization_Runtime.dll");
            files.Add(TDPath + "SmartLocalization/SmartLocalization_Runtime.XML");
            files.Add(TDPath + "SmartLocalization/smartloc_licenses.txt");
            files.Add(TDPath + "SmartLocalization/README.txt");
            files.Add(TDPath + "DemoGame/Scenes/Tutorial.unity");
            files.Add(TDPath + "DemoGame/Scenes/TutorialFinished.unity");

            List<string> dirs = new List<string>();
            dirs.Add(TDPath + "FullSerializer");
            dirs.Add(TDPath + "SmartLocalization/Documentation");
            dirs.Add(TDPath + "SmartLocalization/Editor");

            List<string> files2 = new List<string>();
            files2.Add(TDPath + "Resources/Dialogues/Dialogue1.prefab");
            files2.Add(TDPath + "Resources/Dialogues/Dialogue2.prefab");

            List<string> filesToRemove = new List<string>();
            foreach (string file in files) { if (File.Exists(file)) filesToRemove.Add(file); }

            List<string> dirsToRemove = new List<string>();
            foreach (string dir in dirs) { if (Directory.Exists(dir)) filesToRemove.Add(dir); }

            List<string> filesToRename = new List<string>();
            foreach (string file in files2) { if (File.Exists(file)) filesToRename.Add(file); }

            if (filesToRemove.Count > 0 || filesToRename.Count > 0 || dirsToRemove.Count > 0) {
                string removeFiles = Utilities.Various.StringListToLines(filesToRemove);
                string removeDirs = Utilities.Various.StringListToLines(dirsToRemove);
                string renameFiles = Utilities.Various.StringListToLines(filesToRename);

                if (EditorUtility.DisplayDialog("Tutorial Designer 1.3 Update",
                    "Some files have to be removed or renamed in order to keep compatability with this new version. Details:\n\n" +
                    ((removeFiles != "" || removeDirs != "") ? ("These files will be removed:\n" + removeFiles + removeDirs) : "") + "\n" +
                    (renameFiles != "" ? ("These files will be renamed:\n" + renameFiles) : ""), "Continue", "Cancel")) {
                    foreach (string file in filesToRemove) FileUtil.DeleteFileOrDirectory(file);
                    foreach (string dir in dirsToRemove) FileUtil.DeleteFileOrDirectory(dir);
                    foreach (string file in filesToRename) {
                        if (file.EndsWith("1.prefab")) File.Move(file, file.Replace("Dialogue1.prefab", "Dialogue_vertikal.prefab"));
                        else if (file.EndsWith("2.prefab")) File.Move(file, file.Replace("Dialogue2.prefab", "Dialogue_horizontal.prefab"));
                    }

                    AssetDatabase.Refresh();
                }
            } else {
                // No Update necessary. Hide this file
                File.Move(TDPath + "Scripts/TDUpdate.cs", TDPath + "Scripts/.TDUpdate.cs");
                AssetDatabase.Refresh();
                ClearLogs();
            }
        }
    }
}
