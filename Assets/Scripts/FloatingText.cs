using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FloatingText : MonoBehaviour {
    public Animator animator;
    private Text damageText;

    void OnEnable()
    {
        AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
        //  Debug.Log(clipInfo.Length);

        Destroy(gameObject, clipInfo[0].clip.length - 0.2f);
        damageText = animator.GetComponent<Text>();
    }

    public void SetText(string text, Color color)
    {
        damageText.text = text;
        damageText.color = color;
    }
}
