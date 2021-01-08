#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;

namespace TutorialDesigner {
    /// <summary>
    /// The SideWindow is the part in the TutorialDesigner window, which will represent some kind of a basic menu,
    /// in combination with realtime monitoring during gameplay.
    /// </summary>
    public class SideWindow {
        /// <summary>
        /// Drawing position in EditorWindow
        /// </summary>
        public Rect rect;

        /// <summary>
        /// Monitor Text: The current active node
        /// </summary>
        public string currentNode;

        /// <summary>
        /// Monitor Text: The current active "global" node
        /// </summary>
        public string globalNode;

        /// <summary>
        /// The start node, from which the Tutorial will start
        /// </summary>
        public string startNode = "";

        /// <summary>
        /// Open or Closed. To see more of the TurorialDesigner Canvas, the SideWindow can be put aside
        /// </summary>
        public bool opened;

        // Different formatting styles for elements within SideWindow
        private GUIStyle sideWindowStyle, labelStyle, labelBold, labelBoldBlack, monitorStyle, monitorText;
        // Since this class doesn's use GUILayout, but GUI instead - this value is used for calculating
        // the elements y positions within Draw function
        float stackHeight;

        // Help Window with info and support
        private About aboutWindow;

        /// <summary>
        /// Initializes a new instance of the <see cref="TutorialDesigner.SideWindow"/> class.
        /// </summary>
        public SideWindow() {
            sideWindowStyle = new GUIStyle();
            labelStyle = TutorialEditor.customSkin.label;
            labelBold = new GUIStyle(labelStyle);
            labelBold.fontStyle = FontStyle.Bold;
            labelBoldBlack = new GUIStyle(labelBold);
            labelBoldBlack.normal.textColor = Color.black;
            monitorStyle = new GUIStyle(TutorialEditor.customSkin.textArea);
            monitorStyle.stretchHeight = false;
            monitorText = new GUIStyle();
            monitorText.normal.textColor = Color.black;
            monitorText.wordWrap = true;
            sideWindowStyle.normal.background = Utilities.SkinOperations.ColorToTex(new Color(0.23f, 0.23f, 0.23f));
            sideWindowStyle.padding = new RectOffset(5, 5, 5, 5);
            opened = true;
        }

        // Create Button that shows the About Window
        private void AboutButton() {
            if (GUI.Button(new Rect(5f, stackHeight += 25f, 190f, 20f),
                "About", TutorialEditor.customSkin.button)) {
                aboutWindow = null;
                aboutWindow = (About)EditorWindow.GetWindowWithRect<About>(new Rect(0f, 0f, 300f, 320f), false, "About");
                aboutWindow.Show();
            }
        }

        /// <summary>
        /// Basic Draw routine. Called in OnGUI in TutorialEditor
        /// </summary>
        /// <param name="position">Absolute position</param>
        public void Draw(Rect position) {
            float width = opened ? 200f : 25f;
            rect = new Rect(position.width - width, 0f, width, position.height);
            stackHeight = 0f;

            GUI.BeginGroup(rect, sideWindowStyle);
            // Open- / Close Button
            if (GUI.Button(new Rect(5f, stackHeight += 5f, opened ? 190f : 15f, 20f),
                new GUIContent(opened ? ">> Close >>" : "<", opened ? "Close Sidewindow" : "Open Sidewindow"),
                    TutorialEditor.customSkin.button)) {
                opened = !opened;
            }

            // Elements on opened SideWindow
            if (opened) {
                // If TutorialSystem Gameobject was not yet created
                if (!TutorialEditor.tutSystemCreated) {
                    // New Tutorial Button
                    if (GUI.Button(new Rect(5f, stackHeight += 25f, 190f, 20f),
                        "New Tutorial", TutorialEditor.customSkin.button)) {
                        TutorialEditor.CreateTutorialSystem();
                    }

                    AboutButton();
                    // If TutorialSystem was already created
                } else {
                    // Delete everything from TutorialDesigner Canvas
                    if (GUI.Button(new Rect(5f, stackHeight += 25f, 190f, 20f),
                        "Clear Workspace", TutorialEditor.customSkin.button)) {
                        Undo.RegisterCompleteObjectUndo(TutorialEditor.savePoint, "Clear Workspace");
                        foreach (Node n in TutorialEditor.savePoint.nodes) {
                            n.Remove();
                            Undo.DestroyObjectImmediate(n);
                        }
                        TutorialEditor.savePoint.nodes.Clear();
                        TutorialEditor.savePoint.startNode = null;
                    }

                    // Hide all Dialogue Gameobjects in current scene
                    if (GUI.Button(new Rect(5f, stackHeight += 25f, 190f, 20f),
                        "Hide Dialogues", TutorialEditor.customSkin.button)) {
                        TutorialEditor.savePoint.HideAllDialogues();
                    }

                    if (GUI.Button(new Rect(5f, stackHeight += 25f, 90f, 20f), "Open", TutorialEditor.customSkin.button)) {
                        Porter.Open();
                    }

                    if (GUI.Button(new Rect(105f, stackHeight, 90f, 20f), "Save", TutorialEditor.customSkin.button)) {
                        Porter.Save();
                    }                    

                    AboutButton();
#if TD_MOD_TMPro
                    if (TutorialEditor.savePoint != null) {
                        if (TutorialEditor.savePoint.updateNeeded != null)
                            if (TutorialEditor.savePoint.updateNeeded.Value) {
                                if (GUI.Button(new Rect(5f, stackHeight += 25f, 190f, 20f),
                                    "<color=yellow>Convert Texts to TMPro</color>", TutorialEditor.customSkin.button)) {
                                    TutorialEditor.self.UpdateTD();
                                }
                            }
                    }
#endif
                    // Basic Zooming in the EditorWindow
                    float zoomChange = 0f;
					GUI.Label(new Rect(5f, stackHeight += 30f, 120f, 20f),
						"Zoom : " + TutorialEditor.zoomFactor, labelBold);
					if (GUI.Button (new Rect(125f, stackHeight, 30f, 20f),
						"+", TutorialEditor.customSkin.button)) {
						zoomChange = 0.2f;
					}
					if (GUI.Button (new Rect(160f, stackHeight, 30f, 20f),
						"-", TutorialEditor.customSkin.button)) {
						zoomChange = -0.2f;
					}

					// If user changed zoomFactor
					if (zoomChange != 0) {
						TutorialEditor.zoomFactor = Mathf.Clamp (TutorialEditor.zoomFactor + zoomChange, 0.2f, 1f);
						// Store here for Serialization
						TutorialEditor.savePoint.zoomFactor = TutorialEditor.zoomFactor;
					}

					// Display StartNode if available
					Node startNode =  TutorialEditor.savePoint.startNode;
					string start = "missing";
					if (startNode != null) {
						bool isItStep = ((TutorialEditor.savePoint.startNode.nodeType & 1) == 1);
						start = (isItStep ? "Step - " : "Event - ") + startNode.description;
					}

					GUI.Label(new Rect(5f, stackHeight += 30f, 120f, 20f),
						"Startnode: ", labelBold);
					GUI.Label(new Rect(5f, stackHeight += 18f, 190f, 52f),
						start, labelStyle);

					GUI.BeginGroup(new Rect(5f, stackHeight += 60, 190f, 230f), monitorStyle);
					int groupStack = 0;

					// Tutorial Monitor
					GUI.Label (new Rect(10f, groupStack, 180f, 20f), "Tutorial Monitor", labelBoldBlack);
                    GUI.Label (new Rect(10f, groupStack += 20, 180f, 18f), "--- Default Path ---", monitorText);
					GUI.Label (new Rect(10f, groupStack += 18, 180f, 0f), "-> " + currentNode, monitorText);
                    GUI.Label (new Rect(10f, groupStack += 80, 180f, 18f), "--- Global ---", monitorText);
					GUI.Label (new Rect(10f, groupStack += 18, 180f, 0f), "-> " + globalNode, monitorText);

					GUI.EndGroup();
				}
			}

			GUI.EndGroup ();        
		}
	}
}
#endif
