using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// © 2017 TheFlyingKeyboard
// theflyingkeyboard.net
//Fades in or out UI Text
public class MessagePopup : MonoBehaviour
{
    public Text textToUse;
    public float TimeToFade;


    private float Counter = 0;

    void Start()
    {
        textToUse = GetComponent<Text>();
        textToUse.color= new Color(textToUse.color.r, textToUse.color.g, textToUse.color.b, 0);
    }
    void Update()
    {
        if (textToUse == null) return;
        if (Counter > 0)
        {
            Counter -= Time.deltaTime;
            textToUse.color = new Color(textToUse.color.r, textToUse.color.g, textToUse.color.b, Counter / TimeToFade);
        }
        else SartFadeOut();
    }
    /// <summary>
    /// 淡出
    /// </summary>
    public void SartFadeOut()
    {
        textToUse.color = new Color(textToUse.color.r, textToUse.color.g, textToUse.color.b, 1);
        Counter = TimeToFade;
    }
}