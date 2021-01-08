//
//  MaxInitialization.cs
//  AppLovin MAX Unity Plugin
//
//  Created by Thomas So on 5/24/19.
//  Copyright © 2019 AppLovin. All rights reserved.
//

using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class MaxInitialize
{
	private static readonly string _MigrationProgressBarTitle = "AppLovin MAX Migration";
	private static readonly string AndroidChangelog = "ANDROID_CHANGELOG.md";
	private static readonly string IosChangelog = "IOS_CHANGELOG.md";
	
	private static readonly List<string> _networks = new List<string> {
		"AdColony",
		"Amazon",
		"ByteDance",
		"Chartboost",
		"Facebook",
		"Fyber",
		"Google",
		"InMobi",
		"IronSource",
		"Maio",
		"Mintegral",
		"MyTarget",
		"MoPub",
		"Nend",
		"Ogury", 
		"Smaato",
		"Tapjoy",
		"TencentGDT",
		"UnityAds",
		"VerizonAds",
		"Vungle",
		"Yandex"
	};
	
	static MaxInitialize()
	{
#if UNITY_IOS
		// Check that the publisher is targeting iOS 9.0+
		if (!PlayerSettings.iOS.targetOSVersionString.StartsWith("9.") && !PlayerSettings.iOS.targetOSVersionString.StartsWith("1"))
		{
			Debug.LogError("Detected iOS project version less than iOS 9 - The AppLovin MAX SDK WILL NOT WORK ON < iOS9!!!");
		}
#endif
			
		string legacyDir = Path.Combine("Assets", "MaxSdk/Plugins");
		
		// Check for if directory from older versions of the AppLovin MAX Unity Plugin exists
		if (CheckExistence(legacyDir))
		{
			Debug.Log("Legacy directories from AppLovin MAX Unity Plugin found. Running migration...");
			
			string androidDir = Path.Combine("Assets", "MaxSdk/Plugins/Android/AppLovin");
			if (CheckExistence(androidDir))
			{
				Debug.Log("Deleting " + androidDir + "...");
				EditorUtility.DisplayProgressBar(_MigrationProgressBarTitle, "Deleting " + androidDir + "...", 0.33f);
				FileUtil.DeleteFileOrDirectory(androidDir);
			}
			
			string iOSDir = Path.Combine("Assets", "MaxSdk/Plugins/iOS/AppLovin");
			if (CheckExistence(iOSDir))
			{
				Debug.Log("Deleting " + iOSDir + "...");
				EditorUtility.DisplayProgressBar(_MigrationProgressBarTitle, "Deleting " + iOSDir + "...", 0.66f);
				FileUtil.DeleteFileOrDirectory(iOSDir);
			}
		}
	
		// Check if we have legacy adapter directories
		foreach (string network in _networks)
		{
			string newDir = Path.Combine("Assets", "MaxSdk/Mediation/" + network);
			
			// If new directory exists
			if (CheckExistence(newDir))
			{
				string legacyAndroidDir = Path.Combine("Assets", "MaxSdk/Plugins/Android/" + network);
				string legacyIOSDir = Path.Combine("Assets", "MaxSdk/Plugins/iOS/" + network);

				// Delete legacy iOS directory if exists
				if (CheckExistence(legacyIOSDir))
				{
					Debug.Log("Deleting " + legacyIOSDir + "...");
					FileUtil.DeleteFileOrDirectory(legacyIOSDir);					
				}
					
				// Delete legacy Android director(ies) if exists
				if (CheckExistence(legacyAndroidDir))
				{
					Debug.Log("Deleting " + legacyAndroidDir + "...");
					FileUtil.DeleteFileOrDirectory(legacyAndroidDir);
					
					// Check if it contains shared dependencies
					bool deletedSharedDependencies = false;
					if (network.Equals("Facebook"))
					{
						deletedSharedDependencies = true;
						FileUtil.DeleteFileOrDirectory(Path.Combine("Assets", "MaxSdk/Plugins/Android/Shared Dependencies/exoplayer-core.aar"));
						FileUtil.DeleteFileOrDirectory(Path.Combine("Assets", "MaxSdk/Plugins/Android/Shared Dependencies/exoplayer-dash.aar"));
						FileUtil.DeleteFileOrDirectory(Path.Combine("Assets", "MaxSdk/Plugins/Android/Shared Dependencies/recyclerview-v7.aar"));
					}
					else if (network.Equals("Fyber"))
					{
						deletedSharedDependencies = true;
						FileUtil.DeleteFileOrDirectory(Path.Combine("Assets", "MaxSdk/Plugins/Android/Shared Dependencies/gson.jar"));
					}
					else if (network.Equals("InMobi"))
					{
						deletedSharedDependencies = true;
						FileUtil.DeleteFileOrDirectory(Path.Combine("Assets", "MaxSdk/Plugins/Android/Shared Dependencies/picasso.jar"));
						FileUtil.DeleteFileOrDirectory(Path.Combine("Assets", "MaxSdk/Plugins/Android/Shared Dependencies/recyclerview-v7.aar"));
					}
					else if (network.Equals("Vungle"))
					{
						deletedSharedDependencies = true;
						FileUtil.DeleteFileOrDirectory(Path.Combine("Assets", "MaxSdk/Plugins/Android/Shared Dependencies/common.jar"));
						FileUtil.DeleteFileOrDirectory(Path.Combine("Assets", "MaxSdk/Plugins/Android/Shared Dependencies/converter-gson.jar"));
						FileUtil.DeleteFileOrDirectory(Path.Combine("Assets", "MaxSdk/Plugins/Android/Shared Dependencies/fetch.jar"));
						FileUtil.DeleteFileOrDirectory(Path.Combine("Assets", "MaxSdk/Plugins/Android/Shared Dependencies/gson.jar"));
						FileUtil.DeleteFileOrDirectory(Path.Combine("Assets", "MaxSdk/Plugins/Android/Shared Dependencies/okhttp.jar"));
						FileUtil.DeleteFileOrDirectory(Path.Combine("Assets", "MaxSdk/Plugins/Android/Shared Dependencies/okio.jar"));
						FileUtil.DeleteFileOrDirectory(Path.Combine("Assets", "MaxSdk/Plugins/Android/Shared Dependencies/retrofit.jar"));
					}

					if (deletedSharedDependencies)
					{
						Debug.Log("Deleting " + network + " shared dependencies..." );
					}
				}

				string androidChangelogFile = Path.Combine(newDir, AndroidChangelog);
				string iosChangelogFile = Path.Combine(newDir, IosChangelog);

				FileUtil.DeleteFileOrDirectory(androidChangelogFile);
				FileUtil.DeleteFileOrDirectory(iosChangelogFile);
			}
		}
		
		// Refresh UI
		AssetDatabase.Refresh();
			
		Debug.Log("AppLovin MAX Migration completed");
		EditorUtility.ClearProgressBar();
	}

	private static bool CheckExistence(string location)
	{
		return File.Exists(location) ||
		       Directory.Exists(location) ||
		       (location.EndsWith("/*") && Directory.Exists(Path.GetDirectoryName(location)));
	}
}
