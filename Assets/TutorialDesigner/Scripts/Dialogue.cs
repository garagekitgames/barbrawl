using UnityEngine;
using UnityEngine.UI;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif

namespace TutorialDesigner
{
	/// <summary>
	/// Definition for Dialogue position within the Canvas Gameobject in the scene
	/// </summary>
	public enum DialoguePanelPosition {middle, top, bottom, left, right};

	/// <summary>
	/// Definition for Image position within the Dialogue window. Either above or below the text
	/// </summary>
	public enum DialogueImagePosition {top, bottom};

	[System.Serializable]
	/// <summary>
	/// This is an interface between 1.) Editor scripts, where the user can change all kinds of dialogue settings,
	/// and 2.) all Gameobjects in the scene that have to do with the Dialogue. Text, Backgrounds, Images...
	/// </summary>
	public class Dialogue {	
		/// <summary>
		/// If the game should pause while displaying the dialogue
		/// </summary>
		public bool pauseGame;

		/// <summary>
		/// If Dialogue should have an animated appearance when it gets active
		/// </summary>
		public bool animate;

		/// <summary>
		/// The button result (clicked button) of up to 3 Dialogue buttons
		/// </summary>
		public int buttonResult = -1;

		/// <summary>
		/// Buttons panel. Variable size, depending on the number of visible buttons
		/// </summary>
		public Transform bPanel;

		/// <summary>
		/// If the Dialoguetext comes from Smart Localization, or static text
		/// </summary>
		public bool multiLanguage;

		/// <summary>
		/// If Multi-Language, this will be the ID of SavePoint's Language Database. 0 = Dialogue Text, 1-3 = Button Texts
		/// </summary>
		public string[] selectedLanguageText;

		/// <summary>
		/// Audioclip that plays when Dialogue opens
		/// </summary>
		public AudioClip audioClip;

		[SerializeField]
		private RectTransform rectTransform;

		#if UNITY_EDITOR
		/// <summary>
		/// If this is checked, user can see much more options in dialogue settings
		/// </summary>
		public bool advancedSettings;
	
		[SerializeField]
		private bool _worldSpace;
		/// <summary>
		/// This dialogue will be positioned in 3d space - it will be on canvas3D of the TutorialSystem Gameobject
		/// </summary>
		public bool worldSpace {
			set {
				if (value != _worldSpace) {
					SetCanvas(value);

					_worldSpace = value;
					// Scale and position after throwing on another canvas
					AdjustCanvasMeasure();
				}
			}
			get {
				return _worldSpace;
			}
		}

		/// <summary>
		/// Sets the canvas to either 2d or 3d
		/// </summary>
		/// <param name="threeD">3d Canvas?</param>
		/// <param name="original">Original RectTransform that keeps all position / rotation / scale parameters,
		/// before this object was parented</param>
		public void SetCanvas(bool threeD, RectTransform original = null) {
			Canvas canvas = TutorialEditor.savePoint.canvas;

			// Check 3D requirements and if necessary, create 3d Canvas
			if (threeD) canvas = TutorialEditor.savePoint.check3DCanvas();

			SetCanvas(canvas, original);
		}

		/// <summary>
		/// Sets the canvas and readjust recttransform after Unity modifies it while moving transform to another parent
		/// </summary>
		/// <param name="canvas">Canvas to which this Dialogue should be parented</param>
		/// <param name="original">Original RectTransform that keeps all position / rotation / scale parameters,
		/// before this object was parented</param>
		public void SetCanvas(Canvas canvas, RectTransform original = null) {
		// If Canvas exists, try to move over the dialogue into 3d Space
			if (canvas != null) {
				Vector2 anchoredPosition = Vector2.zero;
				Vector2 offsetMax = Vector2.zero;
				Vector2 offsetMin = Vector2.zero;
				Vector3 localScale = Vector3.zero;

				if (original != null) {
					anchoredPosition = original.anchoredPosition;
					offsetMax = original.offsetMax;
					offsetMin = original.offsetMin;
					localScale = original.localScale;
				}

				rectTransform.SetParent(canvas.transform);

				if (original != null) {
					rectTransform.anchoredPosition = anchoredPosition;
					rectTransform.offsetMax = offsetMax;
					rectTransform.offsetMin = offsetMin;
					rectTransform.localScale = localScale;
				}

				if (!worldSpace) rectTransform.anchoredPosition = panelOffset;
			}
		}

		/// <summary>
		/// Background design of the dialogue panel
		/// </summary>
		public Image dialogueBackgrImg;

		[SerializeField]
		private DialoguePanelPosition _panelPosition = DialoguePanelPosition.middle;
		/// <summary>
		/// Gets or sets the dialogue position on the screen
		/// </summary>
		public DialoguePanelPosition panelPosition {
			get {
				return _panelPosition;
			}
			set {                
				if (value != _panelPosition) {
					switch (value) {
						case DialoguePanelPosition.middle:
							rectTransform.anchorMin = new Vector2 (0.5f, 0.5f);
							rectTransform.anchorMax = new Vector2 (0.5f, 0.5f);
							rectTransform.pivot = new Vector2 (0.5f, 0.5f);
						break;
						case DialoguePanelPosition.top:
							rectTransform.anchorMin = new Vector2 (0.5f, 1f);
							rectTransform.anchorMax = new Vector2 (0.5f, 1f);
							rectTransform.pivot = new Vector2 (0.5f, 1f);
						break;
						case DialoguePanelPosition.bottom:
							rectTransform.anchorMin = new Vector2 (0.5f, 0f);
							rectTransform.anchorMax = new Vector2 (0.5f, 0f);
							rectTransform.pivot = new Vector2 (0.5f, 0f);
						break;
						case DialoguePanelPosition.left:
							rectTransform.anchorMin = new Vector2 (0f, 0.5f);
							rectTransform.anchorMax = new Vector2 (0f, 0.5f);
							rectTransform.pivot = new Vector2 (0f, 0.5f);
						break;
						case DialoguePanelPosition.right:
							rectTransform.anchorMin = new Vector2 (1f, 0.5f);
							rectTransform.anchorMax = new Vector2 (1f, 0.5f);
							rectTransform.pivot = new Vector2 (1f, 0.5f);
						break;
					}

					panelOffset = Vector2.zero;
					rectTransform.anchoredPosition = Vector2.zero;
				}

				_panelPosition = value;
			}
		}

		[SerializeField]
		private Vector2 _panelOffset;
		/// <summary>
		/// Gets or sets the panel offset to the screen border to wich it is aligned
		/// </summary>
		/// <value>Offset value</value>
		public Vector2 panelOffset {
			get {
				return _panelOffset;
			}
			set {
				if (value != _panelOffset) {
					rectTransform.anchoredPosition = value;
				}

				_panelOffset = value;
			}
		}

        /// <summary>
        /// The panel Layout Group Component
        /// </summary>
        public LayoutGroup panelLayout;

		/// <summary>
		/// Reference to the text object within dialogue
		/// </summary>
		public Text textObj;
		#endif

		[SerializeField]
		private string _text;  
		/// <summary>
		/// Gets or sets the dialogue text and refreshes Scene- /Gamewindow in realtime
		/// </summary>
		public string text {
			get
			{
				return _text;
			}
			#if UNITY_EDITOR
			set
			{
				if (_text != value)
				{
					if (textObj != null)
					{
						string resultText = CheckVariables(value, TutorialEditor.savePoint);

						textObj.text = resultText == "" ? value : resultText;
						// Make changes visible in scene immediately
						UnityEditor.EditorUtility.SetDirty(textObj);
					}
				}

				_text = value;
			}
			#endif
		}

		#if UNITY_EDITOR
		/// <summary>
		/// Optional Image that can be displayed within the dialogue. This is only a reference.
		/// User sets it by sprite
		/// </summary>
		public Image imgObj;

		/// <summary>
		/// Enables scaling by user
		/// </summary>
		public LayoutElement imgLayout;

		/// <summary>
		/// Ratio: width / height
		/// </summary>
		public float imgRatio;

		/// <summary>
		/// Initial image size is 1. Can be changed by user via slide control while keeping the ratio
		/// </summary>
		public float imgSize = 1f;

		[SerializeField]
		private Sprite _sprite;
		/// <summary>
		/// Content of the dialogue Image. This is what the user can see / change in the dialogue settings
		/// </summary>
		public Sprite sprite {
			get {
				return _sprite;
			}
			set {
				if (value != _sprite) {
					if (value != null) {
						imgObj.gameObject.SetActive(true);
					} else {
						imgObj.gameObject.SetActive(false);
					}

					imgObj.sprite = value;
					if (value != null) {
						imgRatio = value.textureRect.width / value.textureRect.height;
					}
				}

				_sprite = value;
			}
		}

		[SerializeField]
		private DialogueImagePosition _imagePosition = DialogueImagePosition.bottom;
		/// <summary>
		/// Gets or sets the image position within dialogue
		/// </summary>
		/// <value>The image position.</value>
		public DialogueImagePosition imagePosition {
			get {
				return _imagePosition;
			}
			set {
				if (value != _imagePosition) {
					switch (value) {
					case DialogueImagePosition.top:
						textObj.transform.SetSiblingIndex(1);
						break;
					case DialogueImagePosition.bottom:
						textObj.transform.SetSiblingIndex(0);
						break;
					}
				}

				_imagePosition = value;
			}
		}

		/// <summary>
		/// Array containing button captions
		/// </summary>
		public string[] buttonCaptions;

		private LayoutElement[] btnLayout;
		private Image[] buttonImgObj;
		private Text[] buttonTextObj; 
		#endif

		[SerializeField]
		private int _buttonCount;
		/// <summary>
		/// The number of buttons in dialogue. Can be up to 3 till now. F.i. "Yes", "No", "Cancel"
		/// </summary>
		public int buttonCount {
			get {
				return _buttonCount;
			}
			#if UNITY_EDITOR
			set {			
				// Unhide Responsebuttons if there are some, and caption them
				if (buttonTextObj != null) if (buttonTextObj.Length > 0) {
	                if (value != _buttonCount) {
	    				if (value > 0) {			 
	    					bPanel.gameObject.SetActive(true);
	    					for (int i=0; i<buttonCaptions.Length; i++) { 
	                            if (i < value) {                        
	                                buttonTextObj[i].transform.parent.gameObject.SetActive(true);  
	                                buttonTextObj[i].text = buttonCaptions[i]; 
	                            } else {
	                                buttonTextObj[i].transform.parent.gameObject.SetActive(false);   
	                            }
	    					}
	    				} else if (bPanel != null) {
	    					bPanel.gameObject.SetActive(false);
	    					for (int i=0; i<buttonCaptions.Length; i++) {							
								buttonTextObj[i].transform.parent.gameObject.SetActive(false);
	    					}
	    				}
	                }
				}

				_buttonCount = value;
			}
			#endif
		}

		#if UNITY_EDITOR
		/// <summary>
		/// This size controls every button's layout size. They're always of the same size
		/// </summary>
		public Vector2 buttonSize;

		[SerializeField]
		private Sprite _buttonBackgroundImg;
		/// <summary>
		/// Background design of the buttons
		/// </summary>
		/// <value>Must be a Sprite with borders defined</value>
		public Sprite buttonBackgroundImg {
			get {
				return _buttonBackgroundImg;
			}
			set {
				if (value != _buttonBackgroundImg) {
					foreach (Image img in buttonImgObj) {
						img.sprite = value;
					}
				}

				_buttonBackgroundImg = value;
			}
		}

        [SerializeField]
        private Color _buttonBackgroundColor = Color.white;
        /// <summary>
        /// Color of the Button Background Sprite
        /// </summary>
        /// <value>Must be a Sprite with borders defined</value>
        public Color buttonBackgroundColor {
            get {
                return _buttonBackgroundColor;
            }
            set {
                InitComponents();
                if (value != _buttonBackgroundColor) {
                    foreach (Image img in buttonImgObj) {
                        img.color = value;
                    }
                }

                _buttonBackgroundColor = value;
            }
        }

		/// <summary>
		/// Updates either buttonSize, or the LayoutElement Components of their Gameobject. Depending on onlyGUI
		/// </summary>
		/// <param name="onlyGUI"><c>true</c>Internally used (buttonSize only). <c>true</c>Change by user. LayoutObjects</param>
		/// <param name="size">New desired size</param>
		public void UpdateButtonSizes(bool onlyGUI, Vector2 size) {
			if (onlyGUI) {
				// Update only GUI here (buttonSize). Called after other Elements alter the Buttons LayoutObjects
				if (buttonImgObj != null) {
					if (buttonImgObj.Length > 0) {
                        if (buttonImgObj[0] != null)
					        buttonSize = buttonImgObj[0].rectTransform.rect.size;
					}
				}
			} else {
				// Update Button Layout Objects. Called after User resizes them by Inspector
				foreach (LayoutElement le in btnLayout) {
					le.preferredWidth = size.x;
					le.preferredHeight = size.y;
				}
			}
		}

		/// <summary>
		/// Adjusts the RectTransform of the Dialogue. Make sense after it was transformed to another canvas with different settings.
		/// </summary>
		public void AdjustCanvasMeasure() {
			if (worldSpace) {
				// 3D Canvas
				rectTransform.localScale /= 100;
				panelPosition = DialoguePanelPosition.middle;
				Transform cam = Camera.main.transform;
				rectTransform.anchoredPosition3D = cam.position + cam.forward * 2;
				rectTransform.rotation = Quaternion.Euler (0f, cam.rotation.eulerAngles.y, 0f);
			} else {
				// 2D Canvas
				rectTransform.localScale *= 100;
				rectTransform.anchoredPosition = Vector2.zero;
				rectTransform.rotation = Quaternion.identity;
			}
		}

		/// <summary>
		/// The name of the currently selected dialogue prefab
		/// </summary>
		public string selectedPrefabName;

		/// <summary>
		/// Array of all dialogue prefabs
		/// </summary>
		public static string[] dialoguePrefabs;
		#endif

		[SerializeField]
		private int _selectedDialogueID;
		/// <summary>
		/// ID of currently selected dialogue. It will be set by user via EditorGUI.Popup
		/// </summary>
		public int selectedDialogueID {
			get {
				return _selectedDialogueID;
			}
			#if UNITY_EDITOR
			set {		
                string path = TutorialEditor.TDPath + "Resources/Dialogues";
                //string[] files = AssetDatabase.FindAssets("t:prefab", new string[]{path});

				// Previous dialogue
				GameObject oldDialogue = GetGameObject();				
                if (oldDialogue != null) Undo.RegisterCompleteObjectUndo(oldDialogue, "Dialogue Change");

                if (value != 0 && dialoguePrefabs != null) {			                         
                    string objectName = dialoguePrefabs[value];
                    selectedPrefabName = path + "/" + objectName + ".prefab";
					TutorialEditor.savePoint.HideAllDialogues();
					CreatePrefab(worldSpace ? TutorialEditor.savePoint.canvas3D : TutorialEditor.savePoint.canvas, objectName);
					if (_selectedDialogueID != 0) {
						InitComponents(true);
						if (oldDialogue != null && worldSpace) EditorUtility.CopySerialized(oldDialogue.GetComponent<RectTransform>(), rectTransform);
					}
                } else {
                    ClearComponents ();
                }

                // Delete the old dialogue
                if (oldDialogue != null) Undo.DestroyObjectImmediate(oldDialogue);

				_selectedDialogueID = value;
			}
			#endif
		}

		#if UNITY_EDITOR
		/// <summary>
		/// Basic Constructor
		/// </summary>
		public Dialogue () {		            
			buttonCaptions = new string[3]; // Maximum 3
			selectedLanguageText = new string[4]; // Dialogue Text, and 3 Button Caption Texts
		}

        /// <summary>
        /// Initialization of all components
        /// </summary>
        public void InitComponents(bool force = false) {
            bool needToInitialize = false;
            if (buttonTextObj == null || textObj == null) {
                needToInitialize = true;
            } else if (buttonTextObj.Length == 0) {
                needToInitialize = true;
            }

            if (needToInitialize || force) {
                if (rectTransform == null) {
                    CreatePrefab(TutorialEditor.savePoint.canvas, Path.GetFileNameWithoutExtension(selectedPrefabName));
                    return;
                }

                if (dialogueBackgrImg != null) EditorUtility.CopySerialized(dialogueBackgrImg, rectTransform.GetComponent<Image>());
                dialogueBackgrImg = rectTransform.GetComponent<Image>();

                Vector2 tempOffset = _panelOffset;
                if ((int)panelPosition != 0) {
                    DialoguePanelPosition tempPos = _panelPosition;
                    _panelPosition = 0;
                    panelPosition = tempPos;
                }
                panelOffset = tempOffset;

                if (panelLayout != null) EditorUtility.CopySerialized(panelLayout, rectTransform.GetComponent<LayoutGroup>());
                panelLayout = rectTransform.GetComponent<LayoutGroup>();

                bPanel = rectTransform.Find("ButtonsPanel");
                if (textObj != null) {
                    EditorUtility.CopySerialized(textObj, rectTransform.Find("Text").GetComponent<Text>());
                    // Button Text Objects
                    foreach (Text t in bPanel.GetComponentsInChildren<Text>(true)) {
                        EditorUtility.CopySerialized(textObj, t);
                        t.alignment = TextAnchor.MiddleCenter;
                    }
                }

                Transform textTrans = rectTransform.Find("Text");
                if (textTrans != null) {
                    textObj = textTrans.GetComponent<Text>();   
                    textObj.text = text;
                } else {
                    if (rectTransform.Find("TextMesh") != null)
						Debug.LogWarning("No Text Object found, but TextMeshPro. Load TMPro Module: Window -> TutorialDesigner -> Load TextMeshPro Module, and reopen this scene afterwards.");
                }

                buttonTextObj = bPanel.GetComponentsInChildren<Text>(true);
                // Button Component Initialization Trigger
                if (buttonCount > 0) {
                    // Enables GameObjects for ButtonPanel and Buttons if available
                    int btCount = buttonCount;
                    buttonCount = 0; // set it to 0 and back to previous value
                    buttonCount = btCount; // to trigger variable change event at the top
                }

                if (buttonCaptions != null)
                    if (buttonCaptions.Length > 0 && buttonTextObj.Length > 0) {
                        for (int i = 0; i < buttonCaptions.Length; i++) {
                            buttonTextObj[i].text = buttonCaptions[i];
                        }
                    }

                Transform img = rectTransform.Find("Image");
                if (imgObj != null) {
                    EditorUtility.CopySerialized(imgObj, img.GetComponent<Image>());
                    imgObj = img.GetComponent<Image>();
                    sprite = imgObj.sprite;
                    if (sprite != null) imgObj.gameObject.SetActive(true);
                    if (imagePosition == DialogueImagePosition.top) textObj.transform.SetSiblingIndex(1);
                } else {
                    imgObj = img.GetComponent<Image>();
                }

                if (imgLayout != null) EditorUtility.CopySerialized(imgLayout, img.GetComponent<LayoutElement>());
                imgLayout = img.GetComponent<LayoutElement>();

                btnLayout = bPanel.GetComponentsInChildren<LayoutElement>(true);
                buttonImgObj = new Image[bPanel.childCount];
                for (int i = 0; i < buttonImgObj.Length; i++) {
                    buttonImgObj[i] = bPanel.GetChild(i).GetComponent<Image>();
                }

                if (buttonBackgroundImg == null) {
                    buttonBackgroundImg = buttonImgObj[0].sprite;
                } else {
                    for (int i = 0; i < buttonImgObj.Length; i++) {
                        buttonImgObj[i].sprite = buttonBackgroundImg;
                        if (buttonBackgroundColor != Color.white) buttonImgObj[i].color = buttonBackgroundColor;
                    }
                }

                if (buttonSize == Vector2.zero) {
                    buttonSize = new Vector2(50f, 50f);
                } else {
                    UpdateButtonSizes(false, buttonSize);
                }
            }
        }
		

        /// <summary>
        /// Reset all of the dialogue's components, so they must be freshly initialized
        /// </summary>
		public void ClearComponents() {
            buttonImgObj = null;
			_buttonBackgroundImg = null;
			rectTransform = null;
			buttonTextObj = null;
			textObj = null;
			imgObj = null;
			bPanel = null;
			btnLayout = null;
            text = "";
			worldSpace = false;
		}

		/// <summary>
		/// Creates the prefab for the dialogue. Will be a child of selected SavePoint.canvas
		/// </summary>
		/// <param name="canvas">Canvas which will be the parent of this dialogue</param>
		/// <param name="name">Prefab name</param>
		public void CreatePrefab(Canvas canvas, string name) {
			RectTransform prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<RectTransform>(selectedPrefabName) as RectTransform;
			RectTransform gObj = (RectTransform)GameObject.Instantiate(prefab);
			gObj.name = name;
			rectTransform = gObj.GetComponent<RectTransform>();

			SetCanvas(canvas, prefab);

			InitComponents();
			Undo.RegisterCreatedObjectUndo (gObj.gameObject, "Dialogue change");
		}

		/// <summary>
		/// Returns the dialogue's Gameobject if available
		/// </summary>
		public GameObject GetGameObject() {
			if (rectTransform != null)
				return rectTransform.gameObject;
			else
				return null;
		}

		/// <summary>
		/// Sets the referenced GameObject
		/// </summary>
		/// <param name="dialogueObj">Dialogue GameObject</param>
		public void SetGameObject(GameObject dialogueObj) {
			rectTransform = dialogueObj.GetComponent<RectTransform>();
		}

		/// <summary>
		/// Copies the settings to another dialogue. Used after duplication of a StepNode
		/// </summary>
		/// <param name="d">The other dialogue</param>
		public void CopySettingsTo(Dialogue d) {
			if (selectedDialogueID == 0) return;

			d.selectedDialogueID = selectedDialogueID;

			// Dialogue Panel
			if (worldSpace) {
				d.worldSpace = worldSpace;
				d.rectTransform.position = rectTransform.position;
				d.rectTransform.rotation = rectTransform.rotation;
				d.rectTransform.localScale = rectTransform.localScale;
			}
			d.panelPosition = panelPosition;
			d.panelOffset = panelOffset;
            d.panelLayout.padding = panelLayout.padding;
            d.animate = animate;
			EditorUtility.CopySerialized(dialogueBackgrImg, d.dialogueBackgrImg);

			// Text
			d.text = text;
			d.multiLanguage = multiLanguage;
			selectedLanguageText.CopyTo(d.selectedLanguageText, 0);
			EditorUtility.CopySerialized(textObj, d.textObj);

			// Image
			d.sprite = sprite;
			EditorUtility.CopySerialized(imgObj, d.imgObj);
			d.imagePosition = imagePosition;
			d.imgSize = imgSize;

			// Buttons
			d.buttonCount = buttonCount;
			buttonCaptions.CopyTo(d.buttonCaptions, 0);
			d.InitComponents();
			d.buttonBackgroundImg = buttonBackgroundImg;
            d.buttonBackgroundColor = buttonBackgroundColor;
			d.buttonSize = buttonSize;
			for (int i = 0; i < buttonTextObj.Length; i++) {
				EditorUtility.CopySerialized (buttonTextObj [i], d.buttonTextObj [i]);
				EditorUtility.CopySerialized (btnLayout [i], d.btnLayout [i]);
				EditorUtility.CopySerialized (buttonImgObj [i], d.buttonImgObj [i]);
			}

			d.pauseGame = pauseGame;
			d.audioClip = audioClip;
		}

		/// <summary>
		/// Apply text settings, set by user, to all Text objects within the buttons
		/// </summary>
		public void ApplyTextSettingsToButtons() {
            InitComponents();
            Undo.RecordObjects(buttonTextObj, "Text Settings");			
            foreach (Text t in buttonTextObj) {                
				t.fontSize = textObj.fontSize;
				t.fontStyle = textObj.fontStyle;
				t.color = textObj.color;
				t.material = textObj.material;
                t.font = textObj.font;
			}
		}
		#endif

		/// <summary>
		/// Activates or Deactivated the dialogue. Including visibility in Sceneview
		/// </summary>
		public void SetActive(bool value) {	            
			if (selectedDialogueID != 0) {
				if (rectTransform != null) {
					SavePoint sp = GameObject.Find ("TutorialSystem").GetComponent<SavePoint> ();

					// Check dialogue text for variables
					Transform textObject = rectTransform.Find("Text");
					if (textObject != null && text != null) {
						string resultText = CheckVariables (text, sp);
						if (resultText != "") textObject.GetComponent<Text>().text = resultText;
					}
					// Activate dialogue
					rectTransform.gameObject.SetActive (value);
					if (Application.isPlaying && value) {
						if (animate && Time.timeScale > 0) {
							// Dialogue must appear animated
                            sp.StartCoroutine(PopupAnimation());
						} else if (pauseGame){
							// Pause the game
							Time.timeScale = 0;
						}
					}
				}
				#if UNITY_EDITOR
                else  {
					selectedDialogueID = 0;
                }
				#endif
			}		
		}

		/// <summary>
		/// Check a text for variable keys and replace them with corresponding values
		/// </summary>
		/// <returns>Edited text</returns>
		/// <param name="original">Original text</param>
		/// <param name="sp">Referenced SavePoint that has the variables stored</param>
		public string CheckVariables(string original, SavePoint sp) {
			string resultText = "";

			if (sp != null) 
				if (sp.variableKeys != null)
					if (sp.variableKeys.Count > 0) {
						for (int i=0; i<sp.variableKeys.Count; i++) {								
							string key = sp.variableKeys [i];
							string replaceString = "{%" + key + "%}";
							if (original.Contains(replaceString)) {
								resultText = original.Replace(replaceString, sp.variableValues[i]);
							}
						}
			}

			return resultText;
		}

		/// <summary>
		/// Coroutine run by the StepNode. Watches if its text contains variables and replaces them during runtime
		/// </summary>
		/// <returns>The dialogue variables.</returns>
		/// <param name="sp">Sp.</param>
		public IEnumerator TrackDialogueVariables(SavePoint sp) {	
			Text textObject = rectTransform.Find("Text").GetComponent<Text>();
			while (rectTransform.gameObject.activeSelf) {
				// Check dialogue text for variables
				string resultText = CheckVariables (text, sp);
				if (resultText != "") textObject.text = resultText;

				yield return null;
			}
		}

		// Window appears animated. Scales up a little
		private IEnumerator PopupAnimation() {
			Vector3 targetScale = rectTransform.localScale;

			float t = 0.85f;
			while (t < 1f) {
				t += Time.deltaTime / Time.timeScale;
				rectTransform.localScale = targetScale * t * t;
				yield return null;
			}

			rectTransform.localScale = targetScale;
			if (pauseGame) Time.timeScale = 0;
		}

		/// <summary>
		/// Adds listeners to the buttons, which will (if pressed) set buttonResult to
		/// according to their position in hierarchy (SiblingIndex)
		/// </summary>
		public void AddButtonListeners() {
			if (buttonCount > 0) {			
				for (int i=0; i<buttonCount; i++) {				
					Transform button = bPanel.GetChild(i);	
					button.gameObject.SetActive(true);
					button.GetComponentInChildren<Button>().onClick.AddListener(delegate() {
						buttonResult = button.GetSiblingIndex();
					});
				}
			}
		}

        #if UNITY_EDITOR
		/// <summary>
		/// Updates the button captions, to be visible in Game- or Scene-Window
		/// </summary>
		public void UpdateButtonCaptions(int index=-1) {            
			if (buttonTextObj != null) {
				if (index == -1) {				
                    // No Index defined -> All buttons
					for (int i = 0; i < buttonTextObj.Length; i++) {
						buttonTextObj[i].text = buttonCaptions[i];
						EditorUtility.SetDirty (buttonTextObj[i]);
					}
				} else {	
                    // Particular button
					buttonTextObj[index].text = buttonCaptions[index];
					EditorUtility.SetDirty (buttonTextObj[index]);
				}
			}
		}
        #endif

        /// <summary>
        /// Updates the Dialogue text and Button captions during runtime. Used for SmartLocalization
        /// </summary>
        /// <param name="index">// Index -1 = Dialogue Text; Index >= 0 = Button captions</param>
        /// <param name="caption">Text that will be applied to either Dialogue text or Button</param>
        public void UpdateDialogueTextRuntime(int index, string caption) {
            if (index == -1) {                
                rectTransform.Find("Text").GetComponent<Text>().text = caption;
            } else {
                rectTransform.Find("ButtonsPanel").GetChild(index).GetComponentInChildren<Text>().text = caption;
            }
        }
	}
}
