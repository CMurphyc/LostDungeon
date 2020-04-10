using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillAreaType
{
    Null=0,
    OuterCircle = 1,
    OuterCircle_InnerCube = 2,
    OuterCircle_InnerSector = 3,
    OuterCircle_InnerCircle = 4,
}
public class SkillIndiactor : MonoBehaviour
{
    private Dictionary<SkillAreaType,Transform> indiactor;
    private SkillJoystick Skill_Joystick;
    private SkillAreaType areaType;
    void Start()
    {
        indiactor = new Dictionary<SkillAreaType, Transform>();
        Skill_Joystick = transform.GetComponent<SkillJoystick>();
        Skill_Joystick.OnSkillJoystickDownEvent += OnSkillJoystickDownEvent;
        Skill_Joystick.OnSkillJoystickMoveEvent += OnSkillJoystickMoveEvent;
        Skill_Joystick.OnSkillJoystickUpEvent += OnSkillJoystickUpEvent;
        areaType = SkillAreaType.Null;
    }
    private void OnSkillJoystickDownEvent(Vector2 v, float maxRadius)
    {
        CreateSkillIndiactor(SkillAreaType.OuterCircle_InnerCircle);
    }
    private void OnSkillJoystickMoveEvent(Vector2 v, float maxRadius)
    {
        Debug.Log("Move");
        UpdateInnerElementPosition(v, maxRadius);  
    }
    private void OnSkillJoystickUpEvent()
    {
        DestroySkillIndiactor();
        Debug.Log("Up");
    }
    void Update()
    {
        UpdateSkillIndiactor();
    }
    //更新指示器
    private void UpdateSkillIndiactor()
    {
        switch(areaType)
        {
            case SkillAreaType.OuterCircle_InnerCircle:
                indiactor[areaType].position = transform.root.position;
                break;
            default:
                break;
        }
    }

    //更新内部指示器
    private void UpdateInnerElementPosition(Vector2 v,float maxRadius)
    {
        switch(areaType)
        {
            case SkillAreaType.OuterCircle_InnerCircle:
                indiactor[areaType].GetChild(0).localPosition = v.magnitude / maxRadius  *2f* v.normalized;
                //indiactor[areaType].GetChild(0).localPosition
                break;
            default:
                break;
        }
    }

    //显示指示器
    private void CreateSkillIndiactor(SkillAreaType areaType)
    {
        this.areaType = areaType;
        switch (areaType)
        {
            case SkillAreaType.OuterCircle_InnerCircle:
                if (indiactor.ContainsKey(areaType))
                {
                    indiactor[areaType].localScale= new Vector2(0.4f, 0.4f);
                    indiactor[areaType].GetChild(0).localScale = new Vector2(0.15f / 0.4f, 0.15f / 0.4f);
                    indiactor[areaType].gameObject.SetActive(true);
                }
                else
                {
                    GameObject it = Instantiate(Resources.Load("Joysticks/Circle")) as GameObject;
                    it.transform.localScale = new Vector2(0.4f, 0.4f);
                    indiactor.Add(areaType, it.transform);
                    it = Instantiate(Resources.Load("Joysticks/Circle")) as GameObject;
                    it.transform.SetParent(indiactor[areaType]);
                    it.transform.localScale = new Vector2(0.15f/0.4f, 0.15f/0.4f);
                }
                break;
            default:
                break;
        }
    }

    //撤销指示器
    private void DestroySkillIndiactor()
    {
        switch(areaType)
        {
            case SkillAreaType.OuterCircle_InnerCircle:
                if (indiactor.ContainsKey(areaType))
                {
                    indiactor[areaType].gameObject.SetActive(false);
                }
                break;
            default:
                break;
        }
    }
    public Vector2 GetSkillPosition()
    {
        
        return indiactor[areaType].GetChild(0).position;
    }
}
