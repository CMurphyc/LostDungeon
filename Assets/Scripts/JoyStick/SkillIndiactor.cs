using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillAreaType
{
    Null=0,
    OuterCircle = 1,    
    OuterCircle_InnerCube = 2,          //正方形
    OuterCircle_InnerSector = 3,        //扇形
    OuterCircle_InnerCircle = 4,
}

public class SkillIndiactor : MonoBehaviour
{
    private Dictionary<SkillAreaType,Transform> indiactor;
    private SkillAreaType areaType;      //技能范围类型

    private float SkillRange;       //施法距离
    private float SkillArea;        //施法范围
    private GameObject Target;           //玩家

    void Start()
    {
        //Main = GameObject.FindWithTag("GameEntry").GetComponent<GameMain>();
        indiactor = new Dictionary<SkillAreaType, Transform>();                     //

        if (transform.GetComponent<SkillJoystick>() != null)
        {
            SkillJoystick Skill_Joystick = transform.GetComponent<SkillJoystick>();
            Skill_Joystick.OnSkillJoystickDownEvent += OnSkillJoystickDownEvent;
            Skill_Joystick.OnSkillJoystickMoveEvent += OnSkillJoystickMoveEvent;
            Skill_Joystick.OnSkillJoystickUpEvent += OnSkillJoystickUpEvent;
        }
        else if(transform.GetComponent<SkillJoystick2>() != null)
        {
            SkillJoystick2 Skill_Joystick = transform.GetComponent<SkillJoystick2>();
            Skill_Joystick.OnSkillJoystickDownEvent += OnSkillJoystickDownEvent;
            Skill_Joystick.OnSkillJoystickMoveEvent += OnSkillJoystickMoveEvent;
            Skill_Joystick.OnSkillJoystickUpEvent += OnSkillJoystickUpEvent;
        }
        else if(transform.GetComponent<SkillJoystick3>() != null)
        {
            SkillJoystick3 Skill_Joystick = transform.GetComponent<SkillJoystick3>();
            Skill_Joystick.OnSkillJoystickDownEvent += OnSkillJoystickDownEvent;
            Skill_Joystick.OnSkillJoystickMoveEvent += OnSkillJoystickMoveEvent;
            Skill_Joystick.OnSkillJoystickUpEvent += OnSkillJoystickUpEvent;
        }
        
        areaType = SkillAreaType.Null;
    }

    public void Init(float Range,float Area,GameObject Player)
    {
        SkillRange = Range;
        SkillArea = Area;
        Target = Player;
    }


    private void OnSkillJoystickDownEvent(Vector2 v, float maxRadius)
    {
        CreateSkillIndiactor(SkillAreaType.OuterCircle_InnerCircle);
    }
    private void OnSkillJoystickMoveEvent(Vector2 v, float maxRadius)
    {
        UpdateInnerElementPosition(v, maxRadius);  
    }
    private void OnSkillJoystickUpEvent()
    {
        DestroySkillIndiactor();
    }

    void Update()
    {
        UpdateSkillIndiactor();
    }
    void SkillUpdate()
    {
        UpdateSkillIndiactor();
    }
    //更新指示器
    private void UpdateSkillIndiactor()
    {
        switch(areaType)
        {
            case SkillAreaType.OuterCircle_InnerCircle:
                indiactor[areaType].position = Target.transform.position; 
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
            case SkillAreaType.OuterCircle_InnerCircle://如果是画圆
                if (indiactor.ContainsKey(areaType))
                {
                    indiactor[areaType].localScale = new Vector2(SkillRange, SkillRange);
                    indiactor[areaType].GetChild(0).localScale = new Vector2(SkillArea, SkillArea);
                    indiactor[areaType].GetChild(0).localPosition = new Vector2(0, 0);
                    indiactor[areaType].gameObject.SetActive(true);
                }
                else
                {
                    GameObject it = Instantiate(Resources.Load("Model/Player/Circle")) as GameObject;   //大环
                    it.transform.localScale = new Vector2(SkillRange, SkillRange);
                    indiactor.Add(areaType, it.transform);
                    it = Instantiate(Resources.Load("Model/Player/Circle")) as GameObject;              //小环
                    it.transform.SetParent(indiactor[areaType]);
                    it.transform.localScale = new Vector2(SkillArea, SkillArea);
                }
                break;
            default:
                break;
        }
    }
    //更新内部指示器
    private void UpdateInnerElementPosition(Vector2 v,float maxRadius)//移动
    {
        switch(areaType)
        {
            case SkillAreaType.OuterCircle_InnerCircle:
                indiactor[areaType].GetChild(0).localPosition = v.magnitude / maxRadius  *2f* v.normalized;
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
