#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace TutorialDesigner
{
    namespace Utilities
    {
        /// <summary>
        /// Helper class with static functions to do GUI adjustments / calculations
        /// </summary>
        public class SkinOperations {
            /// <summary>
            /// Changes parts of GUI.skin, the global Unity skin. Used for custom UI elements
            /// </summary>
            /// <param name="skin">The source, that will be used for the skin change</param>
            /// <param name="e">Will receive the current event of OnGUI</param>
            public static void ChangeSkin(GUISkin skin, Event e) {
                if (e.type != EventType.Repaint) return;

                // Change Appearance of UnityEvent ////
                // Header
                GUI.skin.GetStyle ("RL Header").normal.background = skin.GetStyle ("RL Header").normal.background;
                // Background
                GUIStyle RLBackground = GUI.skin.GetStyle ("RL Background");
                GUIStyle _RLBackground = skin.GetStyle ("RL Background");
                RLBackground.normal.background = _RLBackground.normal.background;
                RLBackground.border = _RLBackground.border;
                // Footer
                GUI.skin.GetStyle ("RL Footer").normal.background = skin.GetStyle ("RL Footer").normal.background;
                // Element
                if (UnityEditorInternal.ReorderableList.defaultBehaviours != null) {
                    GUIStyle RLelement = UnityEditorInternal.ReorderableList.defaultBehaviours.elementBackground;
                    GUIStyle _RLelement = skin.GetStyle ("RL Element");
                    RLelement.onNormal.background = _RLelement.onNormal.background;
                    RLelement.onHover.background = _RLelement.onHover.background;
                    RLelement.onActive.background = _RLelement.onActive.background;
                    RLelement.onFocused.background = _RLelement.onFocused.background;
                }

                // Basic Styles ////
                GUIStyle MiniPopup = GUI.skin.GetStyle("MiniPopup");
                GUIStyle _MiniPopup = skin.GetStyle("MiniPopup");
                MiniPopup.normal = _MiniPopup.normal;
                MiniPopup.active = _MiniPopup.active;
                MiniPopup.focused = _MiniPopup.focused;

                GUIStyle ObjectField = GUI.skin.GetStyle ("ObjectField");
                GUIStyle _ObjectField = skin.GetStyle ("ObjectField");
                ObjectField.normal = _ObjectField.normal;
                ObjectField.onNormal = _ObjectField.onNormal;
                ObjectField.focused = _ObjectField.focused;
                ObjectField.border = _ObjectField.border;

                GUIStyle Toggle = GUI.skin.GetStyle ("Toggle");
                GUIStyle _Toggle = skin.GetStyle ("Toggle");
                Toggle.normal = _Toggle.normal;
                Toggle.onNormal = _Toggle.onNormal;
                Toggle.active = _Toggle.active;
                Toggle.onActive = _Toggle.onActive;
                Toggle.focused = _Toggle.focused;
                Toggle.onFocused = _Toggle.onFocused;
                Toggle.border = _Toggle.border;

                GUI.skin.box.normal = skin.box.normal;
                GUI.skin.textField.normal = skin.textField.normal;
                GUI.skin.textField.focused = skin.textField.focused;

                // Pro Skin
                if (EditorGUIUtility.isProSkin) {
                    GUIStyle _CLabel = skin.GetStyle("ControlLabel");
                    GUI.skin.GetStyle("ControlLabel").normal.textColor = _CLabel.normal.textColor;
                    GUI.skin.label.normal.textColor = _CLabel.normal.textColor;
                }
            }

            /// <summary>
            /// Create one colored 2d Texture for GUIStyle
            /// </summary>
            /// <returns>The one colored texture</returns>
            /// <param name="col">The desired color</param>
            public static Texture2D ColorToTex(Color col) {
                Texture2D tex = new Texture2D(1, 1);
                tex.SetPixel(1, 1, col);
                tex.Apply();
                tex.hideFlags = HideFlags.HideAndDontSave;
                return tex;
            }
        }

        /// <summary>
        /// Editor zoom Operation class
        /// </summary>
        public class EditorZoomArea
        {
            // Temp value for storing the matrix before altering
            private static Matrix4x4 prevMatrix;

            /// <summary>
            /// Begin the zoomed draw area in EditorWindow. Must be terminated by End()
            /// </summary>
            /// <param name="zoomScale">Zoom factor of the draw area</param>
            /// <param name="screenCoordsArea">Absolute area of Editor window on the screen</param>
            public static Rect Begin(float zoomScale, Rect screenCoordsArea) 
            {
                GUI.EndGroup(); //End the group that Unity began so we're not bound by the EditorWindow

                // Define the visible area of the zoomed area
                Rect clippedArea = new Rect (screenCoordsArea.position, screenCoordsArea.size * (1.0f / zoomScale));                

                // Adjust y of clippedArea Rect. Float values are necessary to balance Unity's dynamic window position
                double zoom = System.Math.Round(TutorialEditor.zoomFactor, 1);
                if (zoom == 1f) {
                    clippedArea.y = 21f;
                } else if (zoom == 0.8) {
                    clippedArea.y = 17.5f;
                } else if (zoom == 0.6) {
                    clippedArea.y = 15f;
                } else if (zoom == 0.4) {
                    clippedArea.y = 13f;
                } else if (zoom == 0.2) {
                    clippedArea.y = 11.5f;
                }

                // Begin the calculated draw group
                GUI.BeginGroup(clippedArea);

                // Store the current matrix before altering
                prevMatrix = GUI.matrix;

                // Zoom the whole Editor screen
                EditorGUIUtility.ScaleAroundPivot (new Vector2(zoomScale, zoomScale), clippedArea.min);

                // Return the calculated area
                return clippedArea;
            }

            /// <summary>
            /// Ends the zoomed draw area in EditorWindow
            /// </summary>
            public static void End() 
            {
                GUI.matrix = prevMatrix;
                GUI.EndGroup();
                GUI.BeginGroup(new Rect(0, 21, Screen.width, Screen.height));
            }
        }

        /// <summary>
        /// Helper class for uncategorized functions
        /// </summary>
        public static class Various {
            /// <summary>
            /// Get a list of files in a specific directory. Starts at ./ (Project Folder)
            /// </summary>
            /// <returns>An array of the found files</returns>
            /// <param name="path">Where so search</param>
            /// <param name="extension">Filter by file extension</param>
            /// <param name="leadingEntry">The first element in the result can be specified here</param>
            public static string[] GetFileList(string path, string extension, string leadingEntry, string endingFilter) {
                string[] guids = AssetDatabase.FindAssets("t:" + extension, new string[]{path});
                string[] files = new string[guids.Length];
                for (int i=0; i<guids.Length; i++) {
                    files[i] = AssetDatabase.GUIDToAssetPath(guids[i]);
                }

                List<string> result = new List<string>(guids.Length + (leadingEntry != "" ? 1 : 0));

                if (leadingEntry != "") result.Add(leadingEntry);
                foreach (string file in files) {
                    string assetname = Path.GetFileNameWithoutExtension(file);
                    if (assetname.EndsWith(endingFilter)) result.Add(assetname);
                }

                string[] convertedResult = new string[result.Count];
                result.CopyTo(convertedResult);
                return convertedResult;
            }

            /// <summary>
            /// Gets the file list, without leadingEntry but with file extension filter
            /// </summary>
            public static string[] GetFileList(string path, string extension) {
                return GetFileList (path, extension, "", "");
            }

            /// <summary>
            /// Returns all files on this path. No filter, no leadingEntry
            /// </summary>
            public static string[] GetFileList(string path) {
                return GetFileList (path, "*", "", "");
            }

            public static string GetPath(this Transform current) {
                if (current.parent == null)
                    return current.name;
                return current.parent.GetPath() + "/" + current.name;
            }

            public static string GetPath(this Component component) {
                return component.transform.GetPath();
            }

            public static GameObject FindGameObject(string path) {
                GameObject result = null;
                string objectName = "";

                if (path.Contains("/")) {
                    objectName = path.Substring(path.LastIndexOf('/') + 1);
                    foreach (GameObject g in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects()) {
                        foreach (Transform t in g.GetComponentsInChildren<Transform>(true)) {
                            if (t.name == objectName) {
                                if (path == GetPath(t)) {
                                    result = t.gameObject;
                                    break;
                                }
                            }
                        }
                    }
                } else {
                    objectName = path;
                    foreach (GameObject g in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects()) {
                        if (g.name == objectName) {
                            result = g;   
                            break;
                        }
                    }
                }

                return result;
            }

            public static bool NamespaceExists(string desiredNamespace) {
                foreach (System.Reflection.Assembly assembly in System.AppDomain.CurrentDomain.GetAssemblies()) {
                    foreach (System.Type type in assembly.GetTypes()) {
                        if (type.Namespace == desiredNamespace)
                            return true;
                    }
                }
                return false;
            }

            public static string StringListToLines(List<string> list) {
                string result = "";
                foreach (string s in list) result += s + "\n";

                return result;
            }
        }

		/// <summary>
		/// Loads modules from Tutorial Designer. A module can contain Scripts / Resources / Player defined Symbols (Preprocessor Directives)
		/// </summary>
        public static class ModuleLoader {
            public static void LoadTMPro() {
                string prePath = Application.dataPath + "/TutorialDesigner/";
                File.Move(prePath + "Scripts/Dialogue.cs", prePath + "Scripts/.Dialogue.tmp");
                File.Move(prePath + "Editor/StepNodeEditor.cs", prePath + "Editor/.StepNodeEditor.tmp");

                File.Move(prePath + "Scripts/Dialogue.forkTMP", prePath + "Scripts/Dialogue.cs");
                File.Move(prePath + "Editor/StepNodeEditor.forkTMP", prePath + "Editor/StepNodeEditor.cs");

                File.Move(prePath + "Scripts/.Dialogue.tmp", prePath + "Scripts/Dialogue.fork");
                File.Move(prePath + "Editor/.StepNodeEditor.tmp", prePath + "Editor/StepNodeEditor.fork");

                File.Move(prePath + "Resources/Dialogues/Dialogue_vertikal_TMP.forkTMP", prePath + "Resources/Dialogues/Dialogue_vertikal_TMP.prefab");
                File.Move(prePath + "Resources/Dialogues/Dialogue_horizontal_TMP.forkTMP", prePath + "Resources/Dialogues/Dialogue_horizontal_TMP.prefab"); 

                File.Delete(prePath + "Editor/StepNodeEditor.forkTMP.meta");
                File.Delete(prePath + "Scripts/Dialogue.forkTMP.meta");
                File.Delete(prePath + "Resources/Dialogues/Dialogue_vertikal_TMP.forkTMP.meta");
                File.Delete(prePath + "Resources/Dialogues/Dialogue_horizontal_TMP.forkTMP.meta");

                AssetDatabase.Refresh();

                SetSymbols("TD_MOD_TMPro", true);
            }

            public static void UnloadTMPro() {
                string prePath = Application.dataPath + "/TutorialDesigner/";
                File.Move(prePath + "Scripts/Dialogue.cs", prePath + "Scripts/.Dialogue.tmp");
                File.Move(prePath + "Editor/StepNodeEditor.cs", prePath + "Editor/.StepNodeEditor.tmp");

                File.Move(prePath + "Scripts/Dialogue.fork", prePath + "Scripts/Dialogue.cs");
                File.Move(prePath + "Editor/StepNodeEditor.fork", prePath + "Editor/StepNodeEditor.cs");

                File.Move(prePath + "Scripts/.Dialogue.tmp", prePath + "Scripts/Dialogue.forkTMP");
                File.Move(prePath + "Editor/.StepNodeEditor.tmp", prePath + "Editor/StepNodeEditor.forkTMP");

                File.Move(prePath + "Resources/Dialogues/Dialogue_vertikal_TMP.prefab", prePath + "Resources/Dialogues/Dialogue_vertikal_TMP.forkTMP"); 
                File.Move(prePath + "Resources/Dialogues/Dialogue_horizontal_TMP.prefab", prePath + "Resources/Dialogues/Dialogue_horizontal_TMP.forkTMP"); 
                File.Delete(prePath + "Resources/Dialogues/Dialogue_vertikal_TMP.prefab.meta");
                File.Delete(prePath + "Resources/Dialogues/Dialogue_horizontal_TMP.prefab.meta");
                File.Delete(prePath + "Scripts/Dialogue.fork.meta");
                File.Delete(prePath + "Editor/StepNodeEditor.fork.meta"); 
                AssetDatabase.Refresh();

                SetSymbols("TD_MOD_TMPro", false);
            }

            public static void SetSymbols(string name, bool value) {
                if (value) {
                    string symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
                    if (!symbols.Contains(name)) {
                        PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, symbols + ";" + name);
                    }
                } else {
                    string symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
                    if (symbols.Contains(name)) {
                        symbols = symbols.Replace(name + ";", "");
                        symbols = symbols.Replace(name, "");
                        PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, symbols);
                    }
                }

                EditorPrefs.SetString("TutorialDesigner." + name, value ? "yes" : "no");
            }

            public static void CheckAndFixSymbols(string name) {
                string key = EditorPrefs.GetString("TutorialDesigner." + PlayerSettings.productName + "." + name);
                if (key != "") {
                    if (key == "yes") {
                        SetSymbols(name, true);
                    } else if (key == "no") {
                        SetSymbols(name, false);
                    }
                }
            }
        }
    }
}
#endif
