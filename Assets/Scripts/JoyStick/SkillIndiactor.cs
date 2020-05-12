using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillAreaType
{
    Null=0,
    OuterCircle_InnerCube = 1,          //正方形
    OuterCircle_InnerSector = 2,        //扇形
    OuterCircle_InnerCircle = 3,        //圆形
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

        if (transform.GetComponent<SkillJoystick1>() != null)
        {
            SkillJoystick1 Skill_Joystick = transform.GetComponent<SkillJoystick1>();
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

    public void Init(float Range,float Area,GameObject Player,SkillAreaType p)
    {
        SkillRange = Range;
        SkillArea = Area;
        Target = Player;
        areaType = p;
    }


    private void OnSkillJoystickDownEvent(Vector2 v, float maxRadius)
    {
        CreateSkillIndiactor();
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

    //更新指示器
    private void UpdateSkillIndiactor()
    {
        switch(areaType)
        {
            case SkillAreaType.OuterCircle_InnerCircle:
                {
                    if(indiactor.ContainsKey(areaType))
                    indiactor[areaType].position = Target.transform.position;
                    break;
                }
            case SkillAreaType.OuterCircle_InnerCube:
                {
                    if (indiactor.ContainsKey(areaType))
                        indiactor[areaType].position = Target.transform.position;
                    break;
                }
            default:
                break;
        }
    }
    //显示指示器
    private void CreateSkillIndiactor()
    {
        switch (areaType)
        {
            case SkillAreaType.OuterCircle_InnerCircle://圆
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
                    indiactor[areaType].position = Target.transform.position;
                }
                break;
            case SkillAreaType.OuterCircle_InnerCube:
                {
                    if (indiactor.ContainsKey(areaType))
                    {
                        indiactor[areaType].localScale = new Vector2(10, 10);
                        indiactor[areaType].GetChild(0).localScale = new Vector2(SkillRange, SkillArea);
                        indiactor[areaType].GetChild(0).localPosition = new Vector2(0, 0);
                        indiactor[areaType].gameObject.SetActive(true);
                        
                    }
                    else
                    {
                        GameObject it = Instantiate(Resources.Load("Model/Player/Circle")) as GameObject;   //大环
                        it.transform.localScale = new Vector2(10, 10);
                        indiactor.Add(areaType, it.transform);
                        it = Instantiate(Resources.Load("Model/Player/rect")) as GameObject;//内部
                        it.transform.SetParent(indiactor[areaType]);
                        it.transform.localScale = new Vector2(SkillRange, SkillArea);
                        indiactor[areaType].position = Target.transform.position;
                    }
                    break;
                }
            default:
                break;
        }
    }

    /*
     * OnBtnSkill3()
     * {
     *      int currentPlayerUID = sys.model.player.uid;
     *      
     *      FindPlayerRoom
     *      
     *      Room-> RoomMonster
     *      
     *      for (Monsters)
     *      {
     *      
     *          Monster.GetComponent<MOnsterModel_Comp>().debuff.freeze.isFreeze=true;
     *          Monster.GetComponent<MOnsterModel_Comp>().debuff.freeze.RemainingFrame =80;
     *      
     *      
     *      }
     *      
     * 
     * }
     * 
     */



    //更新内部指示器
    private void UpdateInnerElementPosition(Vector2 v,float maxRadius)//移动
    {
        switch(areaType)
        {
            case SkillAreaType.OuterCircle_InnerCircle:
                indiactor[areaType].GetChild(0).localPosition = v.magnitude / maxRadius  *2f* v.normalized;
                break;
            case SkillAreaType.OuterCircle_InnerCube:
                {
                    //Debug.Log("Degree : "+Mathf.Atan(v.y / v.x) * 180f / Mathf.PI);
                    if (v.x >= 0)
                    {
                        indiactor[areaType].GetChild(0).eulerAngles = new Vector3(0, 0, Mathf.Atan(v.y / v.x) * 180f / Mathf.PI - 90);

                    }
                    else
                    {
                        indiactor[areaType].GetChild(0).eulerAngles = new Vector3(0, 0, Mathf.Atan(v.y / v.x) * 180f / Mathf.PI + 90);
                    }
                       // new Quaternion(0, 0, Mathf.Atan(v.y / v.x),0);
                        
                    break;
                }

            default:
                break;
        }
    }

    //撤销指示器
    private void DestroySkillIndiactor()
    {
        /*
        switch(areaType)
        {
            case SkillAreaType.OuterCircle_InnerCircle:
            */
                if (indiactor.ContainsKey(areaType))
                {
                    indiactor[areaType].gameObject.SetActive(false);
                }
         /*       break;
        
            default:
                break;
        }
        */
    }
    public Vector2 GetSkillPosition()
    {
        if (areaType == SkillAreaType.OuterCircle_InnerCircle)
            return indiactor[areaType].GetChild(0).position;
        else 
            return indiactor[areaType].GetChild(0).transform.GetChild(0).transform.GetChild(0).position;
    }
}
