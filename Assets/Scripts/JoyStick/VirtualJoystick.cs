using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    private Image bgImg;
    private Image joystickImg;
    private Vector3 inputVector3;

    JoyStickModule joystick;
    private void Awake()
    {
        bgImg = GetComponent<Image>();
        joystickImg = transform.GetChild(0).GetComponent<Image>();
        joystick = GameObject.Find("GameEntry").GetComponent<GameMain>().WorldSystem._model._JoyStickModule;
    }
    
    public virtual void OnPointerDown(PointerEventData ped)
    {
        OnDrag(ped);
    }
    public virtual void OnPointerUp(PointerEventData ped)
    {
        inputVector3 = Vector3.zero;
        joystickImg.rectTransform.anchoredPosition = inputVector3;
        if (this.name == "MoveStickUI")
        {
            joystick.Ljoystick = new Vector3(inputVector3.x, inputVector3.z, 0);
        }
        if (this.name == "AttackStickUI")
        {
            joystick.Rjoystick = new Vector3(inputVector3.x, inputVector3.z, 0);
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

            joystickImg.rectTransform.anchoredPosition = 
                new Vector3(inputVector3.x * (bgImg.rectTransform.sizeDelta.x / 3), inputVector3.z * (bgImg.rectTransform.sizeDelta.y / 3), 0);
        }

        if (this.name == "AttackStickUI")
        {
            joystick.type = AttackType.BasicAttack;
            joystick.Rjoystick = new Vector3(inputVector3.x, inputVector3.z, 0);
        }
        
        if (this.name == "MoveStickUI")
        {
            joystick.type = AttackType.BasicAttack;
            joystick.Ljoystick = new Vector3(inputVector3.x, inputVector3.z, 0);
        }
    }
    public Vector3 GetVirtualJoystickInput()
    {
        return new Vector3(inputVector3.x, inputVector3.z, 0);
    }

}
