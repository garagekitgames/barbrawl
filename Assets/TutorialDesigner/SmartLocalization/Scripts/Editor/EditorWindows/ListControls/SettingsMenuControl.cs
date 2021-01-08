﻿namespace TutorialDesigner.SmartLocalization.Editor
{
using UnityEngine;
using System.Collections;
using TutorialDesigner.SmartLocalization.ReorderableList;
using UnityEditor;
internal class SettingsMenuControl : ReorderableListControl  
{
	public SettingsMenuControl() : base(ReorderableListFlags.HideAddButton | ReorderableListFlags.DisableContextMenu){}
}
}