using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace TutorialDesigner
{
	/// <summary>
	/// Editor Window for settings Variables that can then be used within dialogues
	/// </summary>
	public class VariableEditor : EditorWindow {
		void OnGUI() {
			SavePoint sp = TutorialEditor.savePoint;
			// Keys + Values
			if (sp != null) if (sp.variableKeys != null) if (sp.variableKeys.Count > 0) {
				for (int i=0; i<sp.variableKeys.Count; i++) {
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.LabelField ("key:", GUILayout.MaxWidth(40));
					sp.variableKeys [i] = EditorGUILayout.TextField (sp.variableKeys [i]);
					EditorGUILayout.LabelField ("value:", GUILayout.MaxWidth(40));
					sp.variableValues [i] = EditorGUILayout.TextField (sp.variableValues [i]);
					if (GUILayout.Button ("X")) {
						sp.variableKeys.RemoveAt (i);
						sp.variableValues.RemoveAt (i);
					}
					EditorGUILayout.EndHorizontal();
				}
			}

			// Add New
			if (GUILayout.Button ("Add new", GUILayout.MaxWidth(60))) {
				if (sp.variableKeys == null) {
					sp.variableKeys = new List<string>();
					sp.variableValues = new List<string>();
				}
				sp.variableKeys.Add ("");				
				sp.variableValues.Add ("");
			}
		}
	}
}
