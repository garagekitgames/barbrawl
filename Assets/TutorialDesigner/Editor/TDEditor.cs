using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using TutorialDesigner.SmartLocalization.Editor;

namespace TutorialDesigner
{
    [CustomEditor(typeof(SavePoint))]
    public class TDEditor : Editor {                   

        private bool help = false;
        private Texture2D logo;
        private SerializedObject sObj;
        private SerializedProperty sProp;
        private string[] languagesShort; 

    	void Awake() {			
    		LoadLanguageDatabase();
    	}
		 /// <summary>
		 /// Raises the enable event.
		 /// </summary>
    	public void OnEnable() {
            SavePoint sp = (SavePoint)target;
            sObj = new SerializedObject(sp);
            sProp = sObj.FindProperty("alternateEvent");

        }

		/// <summary>
		/// Overrides the inspector GUI event.
		/// </summary>
        public override void OnInspectorGUI() {
            SavePoint sp = (SavePoint)target;

            if (logo == null) logo = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/TutorialDesigner/Textures/InspectorLogo.png");
            EditorGUILayout.LabelField(new GUIContent(logo), GUILayout.MinHeight(64));

            EditorGUI.BeginChangeCheck();
            Canvas newCanvas2d = (Canvas)EditorGUILayout.ObjectField("2D Canvas", sp.canvas, typeof(Canvas), true);
            if (newCanvas2d != sp.canvas) {
                Undo.RecordObject(sp, "Set 2d Canvas");
                sp.canvas = newCanvas2d;
            }
            
            if (EditorGUI.EndChangeCheck()) {
                if (sp.canvas != null) {
                    foreach (Node n in sp.nodes) {
                        if ((n.nodeType & 1) == 1) {
                            StepNode sn = (StepNode)n;
                            GameObject dialogue = sn.dialogue.GetGameObject();
                            if (dialogue != null) {
                                RectTransform rtBackup = dialogue.GetComponent<RectTransform>();
                                Vector2 anchoredPosition = rtBackup.anchoredPosition;
                                Vector2 offsetMax = rtBackup.offsetMax;
                                Vector2 offsetMin = rtBackup.offsetMin;
                                Vector3 localScale = rtBackup.localScale;
                                float z = rtBackup.localPosition.z;

                                dialogue.transform.SetParent(sp.canvas.transform);
                                rtBackup.anchoredPosition = anchoredPosition;
                                rtBackup.offsetMax = offsetMax;
                                rtBackup.offsetMin = offsetMin;
                                rtBackup.localScale = localScale;
                                rtBackup.localPosition = new Vector3(rtBackup.localPosition.x, rtBackup.localPosition.y, z);
                            }
                        }
                    }
                }
            }

			EditorGUI.BeginChangeCheck();
            Canvas newCanvas3d = (Canvas)EditorGUILayout.ObjectField("3D Canvas", sp.canvas3D, typeof(Canvas), true);
			if (sp.canvas3D != newCanvas3d) {
                Undo.RecordObject(sp, "Set 3d Canvas");
                sp.canvas3D = newCanvas3d;
            }
			if (EditorGUI.EndChangeCheck ()) {
				// If Canvas is not in World Space, display warning message and reset canvas3D
				if (sp.canvas3D != null) {
					if (sp.canvas3D.renderMode != RenderMode.WorldSpace) {
						Debug.LogWarning ("This canvas is NOT set to WorldSpace. It won't work as a 3D Canvas");
						sp.canvas3D = null;
					}
				}
			}
                      
            sp.tutorialName = EditorGUILayout.TextField("Tutorial Name", sp.tutorialName);
            
            
            bool newOTTtoggle = EditorGUILayout.Toggle("One-Time Tutorial", sp.oneTimeTutorial);
            if (newOTTtoggle != sp.oneTimeTutorial) {
                Undo.RecordObject(sp, "Toggle One Time Tutorial");
                sp.oneTimeTutorial = newOTTtoggle;
            }            
            
            if (sp.oneTimeTutorial) {
                // Help
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                help = EditorGUILayout.Toggle("Display Help", help);

                // Do global events anyway
                sp.doGlobalEventsAnyway = EditorGUILayout.Toggle("Do Global Events", sp.doGlobalEventsAnyway);

                // Alternate Event
                sObj.Update();
                EditorGUILayout.PropertyField(sProp, new GUIContent("AlternateEvent"));
                sObj.ApplyModifiedProperties();

                if (help) {
                    EditorGUILayout.LabelField("Tutorial should be appearing only once. This is done by writing 'TDesigner.[TutorialName].[SceneName]' into PlayerPrefs.\r\n" +
                    "Or 'TDesigner.[TutorialName].Global' if that sould go for all scenes in this project.", EditorStyles.wordWrappedLabel);                
                    EditorGUILayout.Space();
                }

                EditorGUILayout.LabelField("Current Keys in PlayerPrefs:", EditorStyles.boldLabel);

                string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

                // Global Key
                string keyName = "TDesigner." + sp.tutorialName + ".Global";
                string globalValue = PlayerPrefs.GetString(keyName);

                EditorGUILayout.BeginHorizontal();
                if (globalValue != "") {
                    EditorGUILayout.LabelField(keyName);
                    if (GUILayout.Button("Delete", GUILayout.MaxWidth(45))) {
                        PlayerPrefs.DeleteKey(keyName);
                    }
                }
                EditorGUILayout.EndHorizontal();

                // Scene Key
                keyName = "TDesigner." + sp.tutorialName + "." + sceneName;
                string sceneValue = PlayerPrefs.GetString(keyName);

                EditorGUILayout.BeginHorizontal();
                if (sceneValue != "") {
                    EditorGUILayout.LabelField(keyName);
                    if (GUILayout.Button("Delete", GUILayout.MaxWidth(45))) {
                        PlayerPrefs.DeleteKey(keyName);
                    }
                }
                EditorGUILayout.EndHorizontal();

                if (sceneValue == "" && globalValue == "") EditorGUILayout.LabelField("No keys set");

                if (help) {
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("This keys can be assigned during the Game. Drag TutorialSystem to a StepNode's Action, and chose " +
                        "'SavePoint.WriteTutorialDone'.\r\n  - true: for all scenes (global)\r\n  - false: only this scene", EditorStyles.wordWrappedLabel);
                }

                EditorGUILayout.EndVertical();
            }

			// If Multi-Language enabled, enable Popup menu to change the Tutorial Language for Dialogue texts.
			if (languagesShort != null)	if (languagesShort.Length > 0) {
				int oldLang = sp.selectedLanguage;
				int newLang = EditorGUILayout.Popup ("Dialogue Language", sp.selectedLanguage, languagesShort);
				if (newLang != oldLang) {
                    Undo.RecordObject(sp, "Language Change");
					// If Language changed, load the Language Database with current Language
					sp.selectedLanguage = newLang;
					LoadLanguageDatabase ();
				}
			}

			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.LabelField ("Variables");
			if (GUILayout.Button ("Edit", GUILayout.MinWidth(50))) {				
				VariableEditor.GetWindow<VariableEditor>(true);
			}
			EditorGUILayout.EndHorizontal ();

        }

		// If Multi-Language, load all Language data into serializable arrays and update existing Nodes
		private void LoadLanguageDatabase() {
			SavePoint sp = (SavePoint)target;

			Dictionary<string, Dictionary<string, string>> languages = LanguageHandlerEditor.LoadAllLanguageFiles();
			if (languages.Count > 0) {
				// Store Language keys and values from Smart Localization in serializable string arrays
				// Languages
				languagesShort = new string[languages.Count];
				languages.Keys.CopyTo (languagesShort, 0);
				Dictionary<string, string> languageDic = languages[languagesShort[sp.selectedLanguage]];
				// Keys, Values
				sp.languageKeys = new string[languageDic.Count];
				sp.languageValues = new string[languageDic.Count];
				languageDic.Keys.CopyTo (sp.languageKeys, 0);
				languageDic.Values.CopyTo (sp.languageValues, 0);

				// Update Nodes with Dialogues to selected Language
				foreach (Node n in sp.nodes) {
					if ((n.nodeType & 1) == 1) {
						StepNode sn = (StepNode)n;
						if (sn.dialogue.selectedDialogueID != 0 && sn.dialogue.multiLanguage) {							
							// Refresh the Dialogue Text in current Language
							for (int i=0; i<sn.dialogue.buttonCount + 1; i++) {
								int index = System.Array.IndexOf (sp.languageKeys, sn.dialogue.selectedLanguageText [i]);
								if (index == -1) index = 0; // -1 does not exist in an array
								string currentTextValue = sp.languageValues[index]; // Language - Value
								sn.dialogue.selectedLanguageText[i] = sp.languageKeys[index]; // Language - Key
																							  // Update Dialogue Text and buttons
								sn.dialogue.InitComponents();
								if (i == 0) {
									// Dialogue Text
									sn.dialogue.text = currentTextValue;
								}
								else {				
									// Button Captions
									sn.dialogue.buttonCaptions[i-1] = currentTextValue;
								}
							}
							sn.dialogue.UpdateButtonCaptions ();
						}
					}
				}
			}
		}
    }
}
