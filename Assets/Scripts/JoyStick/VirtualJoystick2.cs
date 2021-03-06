﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

public class VirtualJoystick2 : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    private Image bgImg;
    private GameObject joystickImg;
    private GameObject pickImg;
    private Vector3 inputVector3;
    private bool pick;

    JoyStickModule joystick;

    //this component for attack and pick
    private void Awake()
    {
        pick = false;
        bgImg = GetComponent<Image>();
        joystickImg = transform.GetChild(0).gameObject;
        pickImg = transform.GetChild(1).gameObject;
        joystick = GameObject.Find("GameEntry").GetComponent<GameMain>().WorldSystem._model._JoyStickModule;
    }
    
    public void pickIcon()
    {
        if(!pick)
            pickImg.transform.localScale = Vector3.zero;
        pickImg.SetActive(true) ;
        joystickImg.SetActive(false) ;
        
        pick = true;
    }

    public void bigger(GameObject x)
    {
        x.transform.localScale = Vector3.Lerp(x.transform.localScale, new Vector3(1, 1, 1),0.4f);
    }

    public void attackIcon()
    {
        if(pick) joystickImg.transform.localScale = Vector3.zero;
        pickImg.SetActive(false);
        joystickImg.SetActive(true);
        
        pick = false;
    }


    public virtual void OnPointerDown(PointerEventData ped)
    {
        if(!pick)
        OnDrag(ped);
    }
    public virtual void OnPointerUp(PointerEventData ped)
    {
        if (!pick)
        {
            inputVector3 = Vector3.zero;
            joystickImg.GetComponent<Image>().rectTransform.anchoredPosition = inputVector3;
            joystick.Rjoystick = new Vector3(inputVector3.x, inputVector3.z, 0);
        }
        else
        {
            joystick.type = AttackType.Pick;

        }
    }

    public virtual void OnDrag(PointerEventData ped)
    {
        Vector2 pos;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(bgImg.rectTransform
                                                                    , ped.position
                                                                    , ped.pressEventCamera
                                                                    , out pos
                                                                   ))
        {
            
            pos.x = (pos.x / bgImg.rectTransform.sizeDelta.x)*2;
            pos.y = (pos.y / bgImg.rectTransform.sizeDelta.y)*2;
            
            //pos.x = pos.x * 2;
            //pos.y = pos.y * 2;
            inputVector3 = new Vector3(pos.x, 0, pos.y);
            inputVector3 = (inputVector3.magnitude > 1.0f) ? inputVector3.normalized : inputVector3;
            //inputVector = inputVector.normalized;

            joystickImg.GetComponent<Image>().rectTransform.anchoredPosition = 
                new Vector3(inputVector3.x * (bgImg.rectTransform.sizeDelta.x / 3), inputVector3.z * (bgImg.rectTransform.sizeDelta.y / 3), 0);
        }

        joystick.type = AttackType.BasicAttack;
        joystick.Rjoystick = new Vector3(inputVector3.x, inputVector3.z, 0);

    }

    public void Update()
    {
        if(pick)
        {
            bigger(pickImg);
        }
        else
        {
            bigger(joystickImg);
        }

    }

    public Vector3 GetVirtualJoystickInput()
    {
        return new Vector3(inputVector3.x, inputVector3.z, 0);
    }

}
