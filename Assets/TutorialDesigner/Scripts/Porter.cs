#if UNITY_EDITOR
using System;
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace TutorialDesigner{	

    /// <summary>
    /// Class for Import and Export of the Tutorial Nodes to JSON format
    /// </summary>
	public static class Porter{

		public static bool disableAssetProcessor = false;

		private static List<Node> CopyNodes(List<Node> source) {
			List<Node> result = new List<Node>();

			foreach (Node node in source) {
				Node dup = node.Copy();
				EditorUtility.CopySerialized(node, dup);
				dup.name = dup.GetInstanceID().ToString();

				// Connectors
				List<Connector> connector_duplicates = new List<Connector>();
				foreach (Connector con in dup.connectors) {
					// Duplicate connectors for saving
					Connector con_dup = con.Copy();
					//Debug.Log("connector change: " + con.GetInstanceID().ToString() + " to " + con_dup.GetInstanceID().ToString());
					con_dup.name = con_dup.GetInstanceID().ToString();
					con_dup.homeNode = dup;

					connector_duplicates.Add(con_dup);
				}

				dup.connectors = connector_duplicates;
				result.Add(dup);
			}

			// Reconnect Connectors Connections to the duplicates of each other
			foreach (Node node in result) {
				foreach (Connector connector in node.connectors) {
					for (int connection = 0; connection < connector.connections.Count; connection++) {
						for (int n = 0; n < source.Count; n++) {
							for (int c = 0; c<source[n].connectors.Count; c++) {
								if (connector.connections[connection] == source[n].connectors[c]) {
									connector.connections[connection] = result[n].connectors[c];
								}
							}
						}
					}
				}
			}

			return result;
		}

		/// <summary>
		/// Save Tutorial as Asset
		/// </summary>
		public static void Save() {
			// Select path to save all data
			string path = EditorUtility.SaveFilePanelInProject("Save Tutorial", "MyTutorial", "asset", "Pick a name and path for saving your Tutorial");

			// Save operations
			if (!string.IsNullOrEmpty(path)) {
				string location = path.Substring(0, path.LastIndexOf('/'));
				string name = path.Replace(location, "").TrimStart('/', '\\', ' ');
				name = name.Remove(name.LastIndexOf(".asset"), 6);

				// Container object for all nodes
				TutorialAsset tutorial = ScriptableObject.CreateInstance<TutorialAsset>();
				tutorial.NodeDescriptions = new List<string>();
				AssetDatabase.CreateAsset(tutorial, path);

				// Create folder for dialogue prefabs
				string foldername = location + "/" + name;
				if (AssetDatabase.IsValidFolder(foldername)) {
					FileUtil.DeleteFileOrDirectory(foldername);
				}

				AssetDatabase.CreateFolder(location, name);

                foreach (Node node in TutorialEditor.savePoint.nodes) {
					if (node is StepNode) {
						((StepNode)node).StoreActionTargets();
						tutorial.NodeDescriptions.Add("Step: " + node.description);
					} else if (node is EventNode){
						tutorial.NodeDescriptions.Add("Event: " + node.description);
					}
                }

				// Prevent AssetModificationProcessor from saving multiple times on CreatePrefab method
				disableAssetProcessor = true;

				// Make Copies of the nodes in this tutorial and save them as assets
				List<Node> nodeCopies = CopyNodes(TutorialEditor.savePoint.nodes);
				foreach (Node node in nodeCopies) {
					AssetDatabase.AddObjectToAsset(node, tutorial);
					node.hideFlags = HideFlags.HideInHierarchy;

					foreach (Connector con in node.connectors) {
						AssetDatabase.AddObjectToAsset(con, node);
						con.hideFlags = HideFlags.HideInHierarchy;
					}

					// If node contains dialogue, save it as prefab to the folder
                    if (node is StepNode) {
						StepNode sn = (StepNode)node;

						if (sn.dialogue != null) {
							GameObject go = sn.dialogue.GetGameObject();
							if (go != null) {
								UnityEditor.PrefabUtility.CreatePrefab(foldername + "/" + node.name + ".prefab", go);
							}
						}
					}
				}

				// Refresh Project and save everything
				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();

				disableAssetProcessor = false;
			}
		}

		/// <summary>
		/// Open a saved Tutorial.
		/// </summary>
		public static void Open() {
			string path = EditorUtility.OpenFilePanelWithFilters("Open Tutorial", "Assets", new string[]{"Asset files", "asset"});

			if (!string.IsNullOrEmpty(path)) {
				if (path.Contains(Application.dataPath)) {
					path = "Assets/" + path.Replace(Application.dataPath, "").TrimStart('/', '\\', ' ');
				}

				UnityEngine.Object[] assets = AssetDatabase.LoadAllAssetsAtPath(path);

				List<Node> importedNodes = new List<Node>();
				foreach (UnityEngine.Object obj in assets) {
					if (obj != null) {
						if (obj.GetType().IsSubclassOf(typeof(Node))) {
							importedNodes.Add((Node)obj);
						}
					}
				}
				List<Node> nodeDuplicates = CopyNodes(importedNodes);

				string foldername = path.Remove(path.LastIndexOf(".asset"), 6);

				for (int n = 0; n < nodeDuplicates.Count; n++) {
					TutorialEditor.savePoint.nodes.Add(nodeDuplicates[n]);
					nodeDuplicates[n].InitAfterImport();
					if (nodeDuplicates[n] is StepNode) {
						StepNode sn = (StepNode)nodeDuplicates[n];

                        sn.GetActionTargets();
						GameObject resource = AssetDatabase.LoadAssetAtPath<GameObject>(foldername + "/" + importedNodes[n].name + ".prefab");
						if (resource != null) {
							GameObject dialogueObj = GameObject.Instantiate<GameObject>(resource);
							dialogueObj.name = "Dialogue";
							if (dialogueObj != null) {
								sn.dialogue.SetGameObject(dialogueObj);
								sn.dialogue.SetCanvas(sn.dialogue.worldSpace, dialogueObj.GetComponent<RectTransform>());
								sn.dialogue.InitComponents();
							}
						}
					}
				}

				TutorialEditor.savePoint.HideAllDialogues();
			}
		}
	}

	/// <summary>
	/// This is called by Unity when it is about to write serialized assets or scene files to disk.
	/// Everytime the scene is saved, all StepNodes will receive updated values in their ActionEntries,
	/// for storing UnityEvents in a serialized format
	/// </summary>
    public class FileModificationWarning : UnityEditor.AssetModificationProcessor {
        static string[] OnWillSaveAssets(string[] paths) {
			// Save only when user saves the scene. Not if Tutorial is saved by Porter
            if (TutorialEditor.savePoint != null && !Porter.disableAssetProcessor) {
                if (TutorialEditor.savePoint.nodes != null) {
                    foreach (Node node in TutorialEditor.savePoint.nodes) {
                        if (node is StepNode) {
                            ((StepNode)node).StoreActionTargets();
                        }
                    }   
                }
            }
            return paths;
        }
    }

	/// <summary>
	/// Action entry that will save a UnityEvent call in serializable format.
	/// </summary>
	/// <param name="go">Path to GameObject</param>
	/// <param name="co">Assemblyqualified Type Name of the attached Component</param>
	/// <param name="objectArgument">Used if co uses a GameObject or Component as argument</param>
	/// <param name="objectType">For object arguments, Unity already defines the Type name as string in a UnityEvent call</param>
    public struct ActionEntry{
		/// <summary>
		/// Path to GameObject
		/// </summary>
        public string go;
		/// <summary>
		/// Assemblyqualified Type Name of the attached Component
		/// </summary>
        public string co;
		/// <summary>
		/// Used if co uses a GameObject or Component as argument
		/// </summary>
        public string objectArgument;
		/// <summary>
		/// For object arguments, Unity already defines the Type name as string in a UnityEvent call
		/// </summary>
        public string objectType;
    }
}
#endif		
