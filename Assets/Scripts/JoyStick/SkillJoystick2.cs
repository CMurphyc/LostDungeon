using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class SkillJoystick2 : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private GameObject Cancle_Joystick;
    private Vector2 JoyStickCenter;
    public Action<Vector2, float> OnSkillJoystickDownEvent;  //点下事件
    public Action<Vector2, float> OnSkillJoystickMoveEvent;  //拖动事件
    public Action OnSkillJoystickUpEvent;                   //抬起事件

    public GameObject InnerJoystick;
    public GameObject OuterJoystick;
    private float maxRadius = 50;                           //内部摇杆最远滑到多远

    JoyStickModule joystick;
    void Start()
    {
        Cancle_Joystick = transform.parent.Find("CancelStickUI").gameObject;
        JoyStickCenter = transform.position;
        Cancle_Joystick.SetActive(false);
        joystick = GameObject.Find("GameEntry").GetComponent<GameMain>().WorldSystem._model._JoyStickModule;
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        Vector2 v = eventData.position - JoyStickCenter;
        float length = Mathf.Clamp(v.magnitude, 0, maxRadius);              //最大移动距离
        InnerJoystick.transform.localPosition = v.normalized * length;      //改变位置
        OnSkillJoystickDownEvent(v.normalized * length, maxRadius);

        OuterJoystick.SetActive(true);                                      //打开辅助提示
        InnerJoystick.SetActive(true);
        Cancle_Joystick.SetActive(true);
    }
    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        Vector2 v = eventData.position - JoyStickCenter;
        float length = Mathf.Clamp(v.magnitude, 0, maxRadius);
        InnerJoystick.transform.localPosition = v.normalized * length;
        OnSkillJoystickMoveEvent(v.normalized * length, maxRadius);
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        InnerJoystick.transform.localPosition = Vector3.zero;
        OnSkillJoystickUpEvent();


        if (Cancle_Joystick.GetComponent<CancleJoystick>().GetIsIn() == false)
        {
            Vector2 x = transform.GetComponent<SkillIndiactor>().GetSkillPosition();
            joystick.Rjoystick = x;
            joystick.type = AttackType.Skill2;
        }

        OuterJoystick.SetActive(false);
        InnerJoystick.SetActive(false);
        Cancle_Joystick.GetComponent<CancleJoystick>().End();
    }

    public void init(float r)
    {
        maxRadius = r;
    }


}
