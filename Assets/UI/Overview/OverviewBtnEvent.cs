using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverviewBtnEvent : MonoBehaviour
{
    private GameObject main;
    void Start()
    {
        main = GameObject.FindWithTag("GameEntry");
    }

    
    void Update()
    {
        
    }

    public void BackToMain()
    {
        Debug.Log("返回主菜单");
        main.GetComponent<GameMain>().WorldSystem._map.SwitchScene("Main");
    }
}
