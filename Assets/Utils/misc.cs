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

    public enum color
    {
        BLUE,
        YELLOW,
        GREEN,
        RED
    }
    void Start()
    {
        damaged = false;
        damageImage = gameObject;
        flashColour = new Color(255, 255, 255, 0.5f);
        damageImage.GetComponent<Image>().color = Color.clear;
    }
    void Update ()
    {
        if(damaged)
        {
            damageImage.GetComponent<Image>().color = flashColour;
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
    private void ChangeColor(color screenColor)
    {
        switch(screenColor)
        {
            case misc.color.BLUE :
                flashColour = new Color(152f / 255, 245f / 255, 1f, 1f);
                break;
            case misc.color.YELLOW :
                flashColour = new Color(1f, 215f / 255, 0, 1f);
                break;
            case misc.color.GREEN :
                flashColour = new Color(69f / 255, 139f / 255, 0, 1f);
                break;
            case misc.color.RED :
                flashColour = new Color(205f / 255, 0, 0, 1f);
                break;
        }
    }

    public static void ScreenFlash(color screenColor)
    {
        GameObject Misc = GameObject.Find("BeAttackImage");
        Misc.GetComponent<misc>().ChangeColor(screenColor);
        Misc.GetComponent<misc>().PlayerBeAttacked();
    }

}

//蓝色、红色、绿色、黄色