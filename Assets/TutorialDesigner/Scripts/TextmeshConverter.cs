#if UNITY_EDITOR && TD_MOD_TMPro
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;

namespace TutorialDesigner {
	/// <summary>
	/// Class for converting UI Text components to TextMeshProUGUI components
	/// </summary>	
    class ReplaceTextForTextMeshPro : EditorWindow {
		/// <summary>
		/// Transforms singe Text Component to TextMeshProUGUI component
		/// </summary>
		/// <param name="currentUIText">Text component to transform</param>
		/// <param name="fontName">Name of Asset Font that will be searched for. If available in the project, the resulting TMPro component will get this Font Asset assigned</param>
		/// <param name="font">List of available Font Assets</param>
        public static void TextMeshTransmorphSingle(Text currentUIText, string fontName, List<TMP_FontAsset> fonts) {

            GameObject gameObject = currentUIText.gameObject;
            Selectable[] selectables = FindObjectsOfType<Selectable>();

            List<Selectable> targetGraphicRefs = new List<Selectable>();
            foreach (Selectable selectable in selectables) {
                if (selectable.targetGraphic == currentUIText) {
                    targetGraphicRefs.Add(selectable);
                }
            }
            int fontSize = currentUIText.fontSize;
            FontStyle fontStyle = currentUIText.fontStyle;
            HorizontalWrapMode horizWrap = currentUIText.horizontalOverflow;
            string textValue = currentUIText.text;
            TextAnchor anchor = currentUIText.alignment;
            Color color = currentUIText.color;
            float lineSpacing = currentUIText.lineSpacing;

            Vector3 scale = currentUIText.rectTransform.rect.size;

            DestroyImmediate(currentUIText);

            TextMeshProUGUI textMesh = gameObject.AddComponent<TextMeshProUGUI>();
            gameObject.name = "TextMesh";
            textMesh.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, scale.x);
            textMesh.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, scale.y);

            textMesh.fontSize = fontSize;
            textMesh.fontStyle = FontStyle2TMProFontStyle(fontStyle);
            textMesh.enableWordWrapping = (horizWrap == HorizontalWrapMode.Wrap);
            textMesh.text = textValue;
            textMesh.alignment = TextAnchor2TMProTextAlignmentOptions(anchor);
            textMesh.color = color;
            textMesh.lineSpacing = lineSpacing;

            foreach (TMP_FontAsset font in fonts) {
                if (font.name.StartsWith(fontName)) {
                    textMesh.font = font;
                    break;
                }
            }

            foreach (Selectable selectable in targetGraphicRefs) {
                selectable.targetGraphic = textMesh;
            }
        }

        private static TMPro.FontStyles FontStyle2TMProFontStyle(FontStyle style) {
            switch (style) {
                case FontStyle.Bold:
                return TMPro.FontStyles.Bold;
                case FontStyle.Normal:
                return TMPro.FontStyles.Normal;
                case FontStyle.Italic:
                return TMPro.FontStyles.Italic;
                case FontStyle.BoldAndItalic:
                return TMPro.FontStyles.Italic;    // No choice for Bold & Italic
                default:
                return TMPro.FontStyles.Normal;
            }
        }

        private static TMPro.TextAlignmentOptions TextAnchor2TMProTextAlignmentOptions(TextAnchor anchor) {
            switch (anchor) {
                case TextAnchor.LowerCenter:
                return TMPro.TextAlignmentOptions.Bottom;
                case TextAnchor.LowerLeft:
                return TMPro.TextAlignmentOptions.BottomLeft;
                case TextAnchor.LowerRight:
                return TMPro.TextAlignmentOptions.BottomRight;
                case TextAnchor.MiddleCenter:
                return TMPro.TextAlignmentOptions.Center;
                case TextAnchor.MiddleLeft:
                return TMPro.TextAlignmentOptions.MidlineLeft;
                case TextAnchor.MiddleRight:
                return TMPro.TextAlignmentOptions.MidlineRight;
                case TextAnchor.UpperLeft:
                return TMPro.TextAlignmentOptions.TopLeft;
                case TextAnchor.UpperCenter:
                return TMPro.TextAlignmentOptions.Top;
                case TextAnchor.UpperRight:
                return TMPro.TextAlignmentOptions.TopRight;
                default:
                return TMPro.TextAlignmentOptions.Left;
            }
        }
    }
}
#endif