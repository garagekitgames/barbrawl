using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;

namespace DamageEffect.Editors
{
    [CustomEditor(typeof(DamageEffectScript))]
    class DamageEffectScriptEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            EditorExtensions.DrawHeaderTexture();
            base.OnInspectorGUI();
        }
    }
}
