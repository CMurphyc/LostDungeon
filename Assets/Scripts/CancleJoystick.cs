using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class CancleJoystick : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler
{
    private Image CancleImage;
    private bool IsIn=false;
    public Color In,Out;

    void Awake()
    {
        CancleImage = transform.GetComponent<Image>();
    }
    void Start()
    {
        CancleImage.color = Out;
    }
    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        CancleImage.color = In;
        IsIn = true;
    }
    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        CancleImage.color = Out;
        IsIn = false;
    }
    public bool GetIsIn()
    {
        return IsIn;
    }
    public void End()
    {
        CancleImage.color = Out;
        IsIn = false;
        gameObject.SetActive(false);
    }
}
