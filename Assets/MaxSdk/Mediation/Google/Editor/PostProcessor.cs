//
//  PostProcessor.cs
//  AppLovin MAX Unity Plugin
//
//  Created by Santosh Bagadi on 6/4/19.
//  Copyright © 2019 AppLovin. All rights reserved.
//

using System.IO;
using System.Linq;
using System.Xml.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
#if UNITY_IPHONE || UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif
using UnityEngine;

namespace AppLovinMax
{
    public class PostProcessor
    {
        [PostProcessBuild]
        public static void OnPostProcessBuild(BuildTarget buildTarget, string buildPath)
        {
#if UNITY_IPHONE || UNITY_IOS
        var plistPath = Path.Combine(buildPath, "Info.plist");
        var plist = new PlistDocument();
        plist.ReadFromFile(plistPath);

        var appId = "INSERT_YOUR_ADMOB_APP_ID_HERE";

        // Log error if the App ID is not set.
        if (!appId.StartsWith("ca-app-pub-"))
        {
            Debug.LogError("[AppLovin MAX] AdMob App ID is not set. Please enter a valid app ID in Assets/MaxSdk/Mediation/Google/Editor/PostProcessor.cs file.");
            return;
        }

        // Actually set (then write) AdMob app id to Info.plist if valid
        plist.root.SetString("GADApplicationIdentifier", appId);

        File.WriteAllText(plistPath, plist.WriteToString());

#elif UNITY_ANDROID

            var manifestPath = Path.Combine(Application.dataPath, "Plugins/Android/MaxMediationGoogle/AndroidManifest.xml");

            XDocument manifest;
            try
            {
                manifest = XDocument.Load(manifestPath);
            }
#pragma warning disable 0168
            catch (IOException exception)
#pragma warning restore 0168
            {
                Debug.LogError("[AppLovin MAX] Google mediation AndroidManifest.xml is missing. Ensure that MAX Google mediation plugin is imported correctly.");
                return;
            }

            // Get the `manifest` element.
            var elementManifest = manifest.Element("manifest");
            if (elementManifest == null)
            {
                Debug.LogError("[AppLovin MAX] Google mediation AndroidManifest.xml is invalid. Ensure that MAX Google mediation plugin is imported correctly.");
                return;
            }

            // Get the `application` element under `manifest`.
            var elementApplication = elementManifest.Element("application");
            if (elementApplication == null)
            {
                Debug.LogError("[AppLovin MAX] Google mediation AndroidManifest.xml is invalid. Ensure that MAX Google mediation plugin is imported correctly.");
                return;
            }

            // Get all the `meta-data` elements under `application`.
            var adMobMetaData = elementApplication.Descendants().First(element => element.Name.LocalName.Equals("meta-data"));
            XNamespace androidNamespace = "http://schemas.android.com/apk/res/android";

            if (!adMobMetaData.FirstAttribute.Name.Namespace.Equals(androidNamespace) ||
                !adMobMetaData.FirstAttribute.Name.LocalName.Equals("name") ||
                !adMobMetaData.FirstAttribute.Value.Equals("com.google.android.gms.ads.APPLICATION_ID"))
            {
                Debug.LogError("[AppLovin MAX] Google mediation AndroidManifest.xml is invalid. Ensure that MAX Google mediation plugin is imported correctly.");
                return;
            }

            // Log error if the AdMob App ID is not set.
            if (adMobMetaData.LastAttribute.Name.LocalName.Equals("value") &&
                !adMobMetaData.LastAttribute.Value.StartsWith("ca-app-pub-"))
            {
                Debug.LogError("[AppLovin MAX] AdMob App ID is not set. Please enter a valid app ID in the Android Manifest file located at Plugins/Android/MaxGoogleMediation/AndroidManifest.xml");
            }
#endif
        }
    }
}
