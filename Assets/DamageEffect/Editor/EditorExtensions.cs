
using UnityEditor;
using UnityEngine;

namespace DamageEffect.Editors
{
    public static class EditorExtensions
    {

        private const string thread = "http://edentec.org/unity/damage-effect/thread";
        private const string docs = "http://edentec.org//unity/damage-effect/docs";

        public static void DrawHeaderTexture()
        {
            Texture tex = (Texture2D) Resources.Load("SP_header");
            Rect rect = GUILayoutUtility.GetRect(0f, 0f);
            rect.width = tex.width;
            rect.height = tex.height;
            GUILayout.Space(rect.height + 4);
            GUI.DrawTexture(rect, tex);

            Rect rectButtons = rect;
            float buttWidth = rect.height;
            rectButtons.width = buttWidth;
            Texture threadTex = (Texture2D) Resources.Load("unity_icon_27");
            rectButtons.x = 235;
            if (GUI.Button(rectButtons, new GUIContent("", "Unity forums thread")))
            {
                OpenThread();
            }
            GUI.DrawTexture(ShrinkRect(rectButtons, 2), threadTex);

            Texture docsTex = (Texture2D) Resources.Load("docs_icon_27");
            rectButtons.x += buttWidth;
            if (GUI.Button(rectButtons, new GUIContent("", "Docs")))
            {
                OpenDocs();
            }
            GUI.DrawTexture(ShrinkRect(rectButtons, 2), docsTex);
        }

        private static Rect ShrinkRect(Rect rect, int pad)
        {
            rect.x += pad;
            rect.y += pad;
            rect.width -= 2*pad;
            rect.height -= 2*pad;
            return rect;
        }

        [MenuItem("DamageEffect/Unity forums thread")]
        static void OpenThread()
        {
            Help.BrowseURL(thread);
        }

        [MenuItem("DamageEffect/Docs")]
        static void OpenDocs()
        {
            Help.BrowseURL(docs);
        }
    }
}