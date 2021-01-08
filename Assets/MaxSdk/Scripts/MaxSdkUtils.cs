using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

class MaxSdkUtils
{
    private static readonly char _DictKeyValueSeparator = (char) 28;
    private static readonly char _DictKeyValuePairSeparator = (char) 29;

#if UNITY_ANDROID
    private static readonly AndroidJavaClass MaxUnityPluginClass = new AndroidJavaClass("com.applovin.mediation.unity.MaxUnityPlugin");
#endif

    /// <summary>
    /// The native iOS and Android plugins forward dictionaries as a string such as:
    ///
    /// "key_1=value1
    ///  key_2=value2,
    ///  key=3-value3"
    ///  
    /// </summary>
    public static IDictionary<string, string> PropsStringToDict(string str)
    {
        var result = new Dictionary<string, string>();

        if (string.IsNullOrEmpty(str)) return result;

        var components = str.Split('\n');
        foreach (var component in components)
        {
            var ix = component.IndexOf('=');
            if (ix > 0 && ix < component.Length)
            {
                var key = component.Substring(0, ix);
                var value = component.Substring(ix + 1, component.Length - ix - 1);
                if (!result.ContainsKey(key))
                {
                    result[key] = value;
                }
            }
        }

        return result;
    }

    /// <summary>
    /// The native iOS and Android plugins forward dictionaries as a string such as:
    ///
    /// "key_1=value1,key_2=value2,key=3-value3"
    ///  
    /// </summary>
    public static String DictToPropsString(IDictionary<string, string> dict)
    {
        StringBuilder serialized = new StringBuilder();

        if (dict != null)
        {
            foreach (KeyValuePair<string, string> entry in dict)
            {
                if (entry.Key != null && entry.Value != null)
                {
                    serialized.Append(entry.Key);
                    serialized.Append(_DictKeyValueSeparator);
                    serialized.Append(entry.Value);
                    serialized.Append(_DictKeyValuePairSeparator);
                }
            }
        }

        return serialized.ToString();
    }

    /// <summary>
    /// Returns the hexidecimal color code string for the given Color.
    /// </summary>
    public static String ParseColor(Color color)
    {
        int a = (int) (color.a * Byte.MaxValue);
        int r = (int) (color.r * Byte.MaxValue);
        int g = (int) (color.g * Byte.MaxValue);
        int b = (int) (color.b * Byte.MaxValue);

        return BitConverter.ToString(new[]
        {
            Convert.ToByte(a),
            Convert.ToByte(r),
            Convert.ToByte(g),
            Convert.ToByte(b),
        }).Replace("-", "").Insert(0, "#");
    }

#if UNITY_IOS
    [DllImport("__Internal")]
    private static extern bool _MaxIsTablet();
#endif

    /// <summary>
    /// Returns whether or not the device is a tablet.
    /// </summary>
    public static bool IsTablet()
    {
#if UNITY_IOS
        return _MaxIsTablet();
#elif UNITY_ANDROID
        return MaxUnityPluginClass.CallStatic<bool>("isTablet");
#else
        return false;
#endif
    }
}
