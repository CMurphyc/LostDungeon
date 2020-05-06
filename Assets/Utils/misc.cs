using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class misc : MonoBehaviour
{
    private bool damaged;
    private GameObject damageImage;
    private Color flashColour;
    private float flashSpeed = 1f;
    private float full = 155f;
    void Start()
    {
        damaged = false;
        damageImage = gameObject;
        flashColour = new Color(100, 100, 100, 0.5f);
        damageImage.GetComponent<Image>().color = Color.clear;
    }
    void Update ()
    {
        if(damaged)
        {
            damageImage.GetComponent<Image>().color = Color.Lerp(flashColour, damageImage.GetComponent<Image>().color, flashSpeed * Time.deltaTime);
        }
        else
        {
            damageImage.GetComponent<Image>().color = Color.Lerp (damageImage.GetComponent<Image>().color, Color.clear, flashSpeed * Time.deltaTime);
        }
        damaged = false; 
    }

    private void PlayerBeAttacked()
    {
        damaged = true;
    }

    public static void ScreenFlash()
    {
        GameObject misc = GameObject.Find("BeAttackImage");
        misc.GetComponent<misc>().PlayerBeAttacked();
    }

}