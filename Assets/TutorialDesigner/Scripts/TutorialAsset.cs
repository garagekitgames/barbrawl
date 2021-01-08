using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TutorialDesigner
{
	/// <summary>
	/// Object for storing Tutorials. Contains some basic infos that can be seen if the asset is selected in the Projects window
	/// </summary>
	public class TutorialAsset : ScriptableObject
	{
		/// <summary>
		/// A custom description can be set for the saved tutorial asset
		/// </summary>
		public string customDescription;

		/// <summary>
		/// List of containing Node Descriptions as brief overview in the Inspector
		/// </summary>
		public List<string> NodeDescriptions;
	}
}