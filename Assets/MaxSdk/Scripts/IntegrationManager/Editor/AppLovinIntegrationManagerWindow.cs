//
//  MaxIntegrationManager.cs
//  AppLovin MAX Unity Plugin
//
//  Created by Santosh Bagadi on 5/27/19.
//  Copyright © 2019 AppLovin. All rights reserved.
//

using System;
using UnityEditor;
using UnityEngine;

public class AppLovinIntegrationManagerWindow : EditorWindow
{
    private const string windowTitle = "AppLovin Integration Manager";
    private const string documentationGettingStartedLink = "https://dash.applovin.com/documentation/mediation/unity/getting-started";
    private const string documentationAdaptersLink = "https://dash.applovin.com/documentation/mediation/unity/mediation-adapters";
    private const string documentationNote = "Please ensure that integration instructions (e.g. permissions, ATS settings, etc) specific to each network are implemented as well. Click the link below for more info:";

    private Vector2 scrollPosition;
    private static readonly Vector2 windowMinSize = new Vector2(600, 750);
    private const float actionFieldWidth = 60f;
    private const float networkFieldMinWidth = 100f;
    private const float versionFieldMinWidth = 190f;
    private const float networkFieldWidthPercentage = 0.2f;
    private const float versionFieldWidthPercentage = 0.4f; // There are two version fields. Each take 40% of the width, network field takes the remaining 20%.
    private static float previousWindowWidth = windowMinSize.x;
    private static GUILayoutOption networkWidthOption = GUILayout.Width(networkFieldMinWidth);
    private static GUILayoutOption versionWidthOption = GUILayout.Width(versionFieldMinWidth);
    private static readonly GUILayoutOption fieldWidth = GUILayout.Width(actionFieldWidth);

    private GUIStyle titleLabelStyle;
    private GUIStyle headerLabelStyle;
    private GUIStyle wrapTextLabelStyle;
    private GUIStyle linkLabelStyle;

    private PluginData pluginData;
    private bool pluginDataLoadFailed;

    private AppLovinEditorCoroutine loadDataCoroutine;

    public static void ShowManager()
    {
        var manager = GetWindow<AppLovinIntegrationManagerWindow>(utility: true, title: windowTitle, focus: true);
        manager.minSize = windowMinSize;
    }

    #region Editor Window Lifecyle Methods

    private void Awake()
    {
        titleLabelStyle = new GUIStyle(EditorStyles.label)
        {
            fontSize = 14,
            fontStyle = FontStyle.Bold,
            fixedHeight = 20
        };

        headerLabelStyle = new GUIStyle(EditorStyles.label)
        {
            fontSize = 12,
            fontStyle = FontStyle.Bold,
            fixedHeight = 18
        };

        linkLabelStyle = new GUIStyle(EditorStyles.label)
        {
            normal = {textColor = Color.blue}
        };

        wrapTextLabelStyle = new GUIStyle(EditorStyles.label)
        {
            wordWrap = true
        };
    }

    private void OnEnable()
    {
        AppLovinIntegrationManager.downloadPluginProgressCallback = OnDownloadPluginProgress;
        
        // Plugin downloaded and imported. Update current versions for the imported package.
        AppLovinIntegrationManager.importPackageCompletedCallback = AppLovinIntegrationManager.UpdateCurrentVersions;

        Load();
    }

    private void OnDisable()
    {
        if (loadDataCoroutine != null)
        {
            loadDataCoroutine.Stop();
            loadDataCoroutine = null;
        }

        AppLovinIntegrationManager.Instance.CancelDownload();
        EditorUtility.ClearProgressBar();
    }

    private void OnGUI()
    {
        // Immediately after downloading and importing a plugin the entire IDE reloads and current versions can be null in that case. Will just show loading text in that case.
        if (pluginData == null || pluginData.AppLovinMax.CurrentVersions == null)
        {
            DrawEmptyPluginData();
            return;
        }

        // OnGUI is called on each frame draw, so we don't want to do any unnecessary calculation if we can avoid it. So only calculate it when the width actually changed.
        if (Math.Abs(previousWindowWidth - position.width) > 1)
        {
            previousWindowWidth = position.width;
            CalculateFieldWidth();
        }

        using (var scrollView = new EditorGUILayout.ScrollViewScope(scrollPosition, false, false))
        {
            scrollPosition = scrollView.scrollPosition;

            GUILayout.Space(5);

            // Draw AppLovin MAX plugin details
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("AppLovin MAX Plugin Details", titleLabelStyle);
            if (GUILayout.Button(new GUIContent {text = "?", tooltip = "Documentation"}, GUILayout.ExpandWidth(false)))
            {
                Application.OpenURL(documentationGettingStartedLink);
            }
            
            GUILayout.Space(5);
            GUILayout.EndHorizontal();
            DrawPluginDetails();

            // Draw mediated networks
            EditorGUILayout.LabelField("Mediated Networks", titleLabelStyle);
            DrawMediatedNetworks();

            // Draw documentation notes
            EditorGUILayout.LabelField(new GUIContent(documentationNote), wrapTextLabelStyle);
            if (GUILayout.Button(new GUIContent(documentationAdaptersLink), linkLabelStyle))
            {
                Application.OpenURL(documentationAdaptersLink);
            }
        }
    }

    #endregion

    #region UI Methods

    /// <summary>
    /// Shows failure or loading screen based on whether or not plugin data failed to load.
    /// </summary>
    private void DrawEmptyPluginData()
    {
        // Plugin data failed to load. Show error and retry button.
        if (pluginDataLoadFailed)
        {
            EditorGUILayout.LabelField("Failed to load plugin data. Please click retry or restart the integration manager.", headerLabelStyle);
            if (GUILayout.Button("Retry", fieldWidth))
            {
                pluginDataLoadFailed = false;
                Load();
            }
        }
        // Still loading, show loading label.
        else
        {
            EditorGUILayout.LabelField("Loading data...", headerLabelStyle);
        }
    }

    /// <summary>
    /// Draws AppLovin MAX plugin details.
    /// </summary>
    private void DrawPluginDetails()
    {
        var appLovinMax = pluginData.AppLovinMax;
        var upgradeButtonEnabled = !appLovinMax.LatestVersions.Equals(appLovinMax.CurrentVersions);

        GUILayout.BeginHorizontal();
        GUILayout.Space(10);
        using (new EditorGUILayout.VerticalScope("box"))
        {
            // Draw plugin version details
            DrawHeaders("Platform", false);
            DrawPluginDetailRow("Unity 3D", appLovinMax.CurrentVersions.Unity, appLovinMax.LatestVersions.Unity);
            DrawPluginDetailRow("Android", appLovinMax.CurrentVersions.Android, appLovinMax.LatestVersions.Android);
            DrawPluginDetailRow("iOS", appLovinMax.CurrentVersions.Ios, appLovinMax.LatestVersions.Ios);

            // BeginHorizontal combined with FlexibleSpace makes sure that the button is centered horizontally.
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            GUI.enabled = upgradeButtonEnabled;
            if (GUILayout.Button(new GUIContent("Upgrade"), fieldWidth))
            {
                AppLovinEditorCoroutine.StartCoroutine(AppLovinIntegrationManager.Instance.DownloadPlugin(appLovinMax));
            }

            GUI.enabled = true;
            GUILayout.Space(5);
            GUILayout.EndHorizontal();

            GUILayout.Space(5);
        }
        GUILayout.Space(5);
        GUILayout.EndHorizontal();
    }

    /// <summary>
    /// Draws the headers for a table.
    /// </summary>
    private void DrawHeaders(string firstColumnTitle, bool drawAction)
    {
        using (new EditorGUILayout.HorizontalScope())
        {
            GUILayout.Space(5);
            EditorGUILayout.LabelField(firstColumnTitle, headerLabelStyle, networkWidthOption);
            GUILayout.Button("Current Version", headerLabelStyle, versionWidthOption);
            GUILayout.Space(3);
            GUILayout.Button("Latest Version", headerLabelStyle, versionWidthOption);
            GUILayout.Space(3);
            if (drawAction)
            {
                GUILayout.Button("Action", headerLabelStyle, fieldWidth);
                GUILayout.Space(5);
            }
        }

        GUILayout.Space(4);
    }

    /// <summary>
    /// Draws the platform specific version details for AppLovin MAX plugin.
    /// </summary>
    private void DrawPluginDetailRow(string platform, string currentVersion, string latestVersion)
    {
        using (new EditorGUILayout.HorizontalScope())
        {
            GUILayout.Space(5);
            EditorGUILayout.LabelField(new GUIContent(platform), networkWidthOption);
            GUILayout.Button(new GUIContent(currentVersion), EditorStyles.label, versionWidthOption);
            GUILayout.Space(3);
            GUILayout.Button(new GUIContent(latestVersion), EditorStyles.label, versionWidthOption);
            GUILayout.Space(3);
        }

        GUILayout.Space(4);
    }

    /// <summary>
    /// Draws mediated network details table.
    /// </summary>
    private void DrawMediatedNetworks()
    {
        var networks = pluginData.MediatedNetworks;
        GUILayout.BeginHorizontal();
        GUILayout.Space(10);
        using (new EditorGUILayout.VerticalScope("box"))
        {
            DrawHeaders("Network", true);
            foreach (var network in networks)
            {
                DrawNetworkDetailRow(network);
            }
            GUILayout.Space(5);
        }
        GUILayout.Space(5);
        GUILayout.EndHorizontal();
    }

    /// <summary>
    /// Draws the network specific details for a given network.
    /// </summary>
    private void DrawNetworkDetailRow(Network network)
    {
        string action;
        var currentVersion = network.CurrentVersions.Unity;
        var latestVersion = network.LatestVersions.Unity;
        bool isActionEnabled;
        if (string.IsNullOrEmpty(currentVersion))
        {
            action = "Install";
            currentVersion = "Not Installed";
            isActionEnabled = true;
        }
        else if (currentVersion.Equals(latestVersion))
        {
            action = "Installed";
            isActionEnabled = false;
        }
        else
        {
            action = "Upgrade";
            isActionEnabled = true;
        }

        GUILayout.Space(4);
        using (new EditorGUILayout.HorizontalScope(GUILayout.ExpandHeight(false)))
        {
            GUILayout.Space(5);
            EditorGUILayout.LabelField(new GUIContent(network.DisplayName), networkWidthOption);
            GUILayout.Button(new GUIContent(currentVersion), EditorStyles.label, versionWidthOption);
            GUILayout.Space(3);
            GUILayout.Button(latestVersion, EditorStyles.label, versionWidthOption);
            GUILayout.Space(3);
            GUILayout.FlexibleSpace();
            GUI.enabled = isActionEnabled;

            if (GUILayout.Button(new GUIContent(action), fieldWidth))
            {
                // Download the plugin.
                AppLovinEditorCoroutine.StartCoroutine(AppLovinIntegrationManager.Instance.DownloadPlugin(network));
            }

            GUI.enabled = true;
            GUILayout.Space(5);
        }
    }

    /// <summary>
    /// Calculates the fields width based on the width of the window.
    /// </summary>
    private void CalculateFieldWidth()
    {
        var availableWidth = position.width - actionFieldWidth - 60; // NOTE: Magic number alert. This is the sum of all the spacing the fields and other UI elements.
        var networkLabelWidth = Math.Max(networkFieldMinWidth, availableWidth * networkFieldWidthPercentage);
        networkWidthOption = GUILayout.Width(networkLabelWidth);

        var versionLabelWidth = Math.Max(versionFieldMinWidth, availableWidth * versionFieldWidthPercentage);
        versionWidthOption = GUILayout.Width(versionLabelWidth);
    }

    #endregion

    #region Utility Methods

    /// <summary>
    /// Loads the plugin data to be displayed by this window.
    /// </summary>
    private void Load()
    {
        loadDataCoroutine = AppLovinEditorCoroutine.StartCoroutine(AppLovinIntegrationManager.Instance.LoadPluginData(data =>
        {
            if (data == null)
            {
                pluginDataLoadFailed = true;
            }
            else
            {
                pluginData = data;
                pluginDataLoadFailed = false;
            }

            Repaint();
        }));
    }

    /// <summary>
    /// Callback method that will be called with progress updates when the plugin is being downloaded.
    /// </summary>
    private static void OnDownloadPluginProgress(string pluginName, float progress, bool done)
    {
        // Download is complete. Clear progress bar.
        if (done)
        {
            EditorUtility.ClearProgressBar();
        }
        // Download is in progress, update progress bar.
        else
        {
            if (EditorUtility.DisplayCancelableProgressBar(windowTitle, string.Format("Downloading {0} plugin...", pluginName), progress))
            {
                AppLovinIntegrationManager.Instance.CancelDownload();
                EditorUtility.ClearProgressBar();
            }
        }
    }

    #endregion
}
