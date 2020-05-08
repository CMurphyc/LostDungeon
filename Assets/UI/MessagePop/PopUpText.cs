using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// © 2017 TheFlyingKeyboard
// theflyingkeyboard.net
//Fades in or out UI Text
public class PopUpText : MonoBehaviour
{
    private Text textToUse;
    private float TimeToFade = 0.2f;

    private bool dp = false;
    private float Counter = 0;
    private float InsTime = 0;

    void Update()
    {
        if (dp == false) return;
        if (InsTime > 0)
        {
            InsTime -= Time.deltaTime;
        }
        else if (Counter > 0)
        {
            Counter -= Time.deltaTime;
            textToUse.color = new Color(textToUse.color.r, textToUse.color.g, textToUse.color.b, Counter / TimeToFade);
        }
        else
        {
            Destroy(gameObject);
        }

      
    }
    public void Init(string text, float time)
    {

        textToUse = GetComponent<Text>();
        textToUse.color = new Color(textToUse.color.r, textToUse.color.g, textToUse.color.b, 0);

        textToUse.text = text;
        InsTime = time;
        Counter = TimeToFade;
        textToUse.color = new Color(textToUse.color.r, textToUse.color.g, textToUse.color.b, 1);
        dp = true;
    }
}