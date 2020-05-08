using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MessageManager 
{

    List<GameObject> MessageList = new List<GameObject>();
    public MessageManager()
    {

    }

    public void PopText(string text, float time=0.5f)
    {
        GameObject PopUpText_Prefab = (GameObject)Resources.Load("UI/MessagePopup/Hint");
        GameObject Canvas = GameObject.Find("Canvas");
        if (Canvas != null)
        { 
            GameObject PopUpText_Instance = Object.Instantiate(PopUpText_Prefab, PopUpText_Prefab.transform.position,
            PopUpText_Prefab.transform.rotation, Canvas.transform);
         
            PopUpText_Instance.transform.localPosition = Vector3.zero;
            
            PopUpText_Instance.GetComponent<PopUpText>().Init(text, time);
        }

    }




}
