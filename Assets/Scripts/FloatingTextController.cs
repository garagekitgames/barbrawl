using UnityEngine;
using System.Collections;

public class FloatingTextController : MonoBehaviour {
    public static FloatingText popupText;
    private static GameObject canvas;

    public static void Initialize()
    {
        canvas = GameObject.Find("Canvas");
        if (!popupText)
            popupText = Resources.Load<FloatingText>("Prefabs/PopUpHolder");
    }

    public static void CreateFloatingText(string text, Vector3 location, Color color)
    {
        FloatingText instance = Instantiate(popupText);
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(new Vector3(location.x + Random.Range(-.2f, .2f), location.y + Random.Range(-.2f, .2f), location.z + Random.Range(-.2f, .2f)));

        instance.transform.SetParent(canvas.transform, false);
        instance.transform.position = screenPosition;
        instance.SetText(text, color);
    }
}
