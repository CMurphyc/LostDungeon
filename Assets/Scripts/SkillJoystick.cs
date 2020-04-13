using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class SkillJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private GameMain Main;
    private GameObject Cancle_Joystick;
    private GameObject Attack_Joystick;
    private Vector2 JoyStickCenter;
    public Action<Vector2,float> OnSkillJoystickDownEvent;
    public Action<Vector2,float> OnSkillJoystickMoveEvent;
    public Action OnSkillJoystickUpEvent;

    public float maxRadius;

    void Start()
    {
        //Main = GameObject.FindWithTag("GameEntry").GetComponent<GameMain>();
        Cancle_Joystick = transform.parent.Find("CancleJoystick").gameObject;
        Attack_Joystick = transform.parent.Find("AttackJoystick").gameObject;
        JoyStickCenter = transform.position;
    }
    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        Vector2 v = eventData.position - JoyStickCenter;
        float length = Mathf.Clamp(v.magnitude, 0, maxRadius);
        transform.Find("thumb").localPosition = v.normalized * length;
        OnSkillJoystickDownEvent(v.normalized * length, maxRadius);

        Cancle_Joystick.SetActive(true);
        Attack_Joystick.SetActive(false);
    }
    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        Vector2 v = eventData.position - JoyStickCenter;
        float length = Mathf.Clamp(v.magnitude, 0, maxRadius);
        transform.Find("thumb").localPosition = v.normalized * length;
        OnSkillJoystickMoveEvent(v.normalized * length, maxRadius);
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        transform.Find("thumb").localPosition = Vector3.zero;
        OnSkillJoystickUpEvent();

        if(Cancle_Joystick.GetComponent<CancleJoystick>().GetIsIn()==false)
        {
            //释放技能
            GameObject it=Instantiate(Resources.Load("Effects/explosion")) as GameObject;
            it.transform.position = transform.GetComponent<SkillIndiactor>().GetSkillPosition();
        }
        Cancle_Joystick.GetComponent<CancleJoystick>().End();
        Attack_Joystick.SetActive(true);

    }
}
