using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TianfuUIEvent : MonoBehaviour
{
    GameObject main;
    public GameObject Tianfu;
    private void Awake()
    {
        main = GameObject.FindWithTag("GameEntry");
    }
    public void OnBtnTianfu()
    {
        Tianfu.SetActive(true);
    }
    public void OnBtnSave()
    {
        
    }
    public void OnBtnExit()
    {
        Tianfu.SetActive(false);
    }

    /// <summary>
    /// 天赋技能待设计
    /// </summary>
    public void OnBtnSkill()
    {
        ChangeColor();
        switch (gameObject.name)
        {
            case "btnSkill0":
                Debug.Log("Skill0");
                break;
            case "Skill1":
                break;
            case "Skill2":
                break;
            case "Skill3":
                break;
        }
    }
    void ChangeColor()
    {
        Color now = GetComponent<Image>().color;
        if (now.a <= 0.4) now.a = 1;
        else now.a = 80.0f / 255;
        GetComponent<Image>().color = now;
    }
}
