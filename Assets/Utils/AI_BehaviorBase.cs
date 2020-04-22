using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;






//使用说明 
//1.先Start()
//2.再Update()
class AI_BehaviorBase
{

    int CurrentState = 0;
    int TempState = 0;
    bool Attack = false;
    bool Dash = false;
    // 配置
    public int Idle_FrameInterval = 0;
    public int Run_FrameInterval = 0;
    public int Attack_FrameInterval = 0;
    public int AttackSpeedInterval = 0;
    public float AttackDistance = 0;
    public int SummoningInterval = 0;
    //boss
    
    public float DashDistance = 0;
    //传送到距离玩家距离
    public float DashToDistance = 0;
    public int Teleport_FrameInterval = 0;

    // 需初始化
    int NextChangeStateFrame = 0;
    public AI_Type type;
    public int RoomID;
    Vector3 Velocity = Vector3.zero;

    public AI_BehaviorBase()
    {
      
    }

    public void Start(int frame)
    {
        NextChangeStateFrame = frame + Idle_FrameInterval;
    }
    public void RestartChange(int Targetframe)
    {
        NextChangeStateFrame = Targetframe;
    }

    public void Update(int frame, Vector2 NpcPosition, Vector2 TargetPosition)
    {
        //LogicUpdate(frame, NpcPosition, TargetPosition);
        //UpdateView();
    }

    public void LogicUpdate(int frame , Vector2 NpcPosition, Vector2 TargetPosition, GameObject obj)
    {
        switch(type)
        {
            case AI_Type.Normal_Melee:
                {
                    MeleeLogic(frame, NpcPosition, TargetPosition, obj);
                    break;
                }
            case AI_Type.Boss_Rabit:
                {
                    RaibitLogic(frame, NpcPosition, TargetPosition, obj);
                    break;
                }
            case AI_Type.Boss_Rabit_Egg:
                {
                    MeleeLogic(frame, NpcPosition, TargetPosition, obj);
                    break;
                }
            case AI_Type.Nomral_Range:
                {
                    RangeLogic(frame, NpcPosition, TargetPosition, obj);
                    break;
                }
            case AI_Type.Engineer_TerretTower:
                {
                    StaticRangeAILogic(frame, NpcPosition, TargetPosition, obj);
                    break;
                }
            default:
                break;

        }
      
    }
    

    public void UpdateView(GameObject obj)
    {
        Vector3 TargetPos = new Vector2((float)obj.GetComponent<MonsterModel_Component>().position.x, (float)obj.GetComponent<MonsterModel_Component>().position.y);
        obj.transform.position = Vector3.SmoothDamp(obj.transform.position, TargetPos, ref Velocity,Global.FrameRate/1000f);
        //obj.transform.position = new Vector2((float)obj.GetComponent<MonsterModel_Component>().position.x, (float)obj.GetComponent<MonsterModel_Component>().position.y);


        obj.transform.rotation = obj.GetComponent<MonsterModel_Component>().Rotation;

        if (CurrentState != (int)AI_BehaviorType.UnderAttack)
        {
            obj.GetComponent<SpriteRenderer>().color = new Color(255f / 255f, 255f / 255f, 255f / 255f);
        }
        if (obj.tag == "Boss" && obj.GetComponent<MonsterModel_Component>().UnderAttack)
        {
            obj.GetComponent<SpriteRenderer>().color = new Color(255f / 255f, 93f / 255f, 93f / 255f);
        }
        switch (CurrentState)
        {
            case (int)AI_BehaviorType.Idle:
                {
                    obj.GetComponent<Animator>().SetInteger("MainState", 0);
                    break;
                }

            case (int)AI_BehaviorType.Run:
                {
                    obj.GetComponent<Animator>().SetInteger("MainState", 1);
                  
                    break;
                }
            case (int)AI_BehaviorType.Attack:
                {
                    obj.GetComponent<Animator>().SetInteger("MainState", 2);
                   
                    break;
                }
            case (int)AI_BehaviorType.Dead:
                {
                    obj.GetComponent<Animator>().SetInteger("MainState", 3);
                    break;
                }
            case (int)AI_BehaviorType.UnderAttack:
                {
                    obj.GetComponent<SpriteRenderer>().color = new Color(255f / 255f, 93f / 255f, 93f / 255f);
                    break;
                }
            default:
                break;
        }

    }

    public virtual void BossRunLogic(int frame,GameObject obj, Vector2 TargetPosition)
    { }
    public virtual void BossAttackLogic(int frame, GameObject obj, Vector2 TargetPosition)
    { }
    public virtual void BossTPLogic(int frame, GameObject obj, Vector2 ToPos,bool rot)
    { }

    private void RaibitLogic(int frame, Vector2 NpcPosition, Vector2 TargetPosition, GameObject obj)
    {
        if (CurrentState == (int)AI_BehaviorType.Dead)
            return;
        if (frame == NextChangeStateFrame)
        {
            if (CurrentState != (int)AI_BehaviorType.Idle)
            {
                CurrentState = 0;
                NextChangeStateFrame = frame + Idle_FrameInterval;

            }
            else
            {
                if (TempState != (int)AI_BehaviorType.Attack)
                {
                    CurrentState = (int)AI_BehaviorType.Attack;
                    TempState = CurrentState;
                }
                else
                {
                    CurrentState = (int)AI_BehaviorType.Run;
                    TempState = CurrentState;
                }

                if (CurrentState == (int)AI_BehaviorType.Run)
                {
                    //Debug.Log("Run");

                    float distance2Player = Vector2.Distance(NpcPosition, TargetPosition);
                    Vector2 vec = (TargetPosition - NpcPosition).normalized * DashToDistance;
                    Vector2 ToPosition = TargetPosition - vec;
                    if (distance2Player > DashDistance)
                    {
                        Dash = true;
                        BossTPLogic(frame, obj , ToPosition , TargetPosition.x >= NpcPosition.x);
                        NextChangeStateFrame = frame + Teleport_FrameInterval;
                    }
                    else
                    {
                        Dash = false;
                        BossRunLogic(frame, obj,Vector2.zero);
                        NextChangeStateFrame = frame + Run_FrameInterval;
                    }
                }
                else if (CurrentState == (int)AI_BehaviorType.Attack)
                {
                    //Debug.Log("Attack");
                    BossAttackLogic(frame, obj, TargetPosition);
                    NextChangeStateFrame = frame + Attack_FrameInterval;
                }
            }
        }

    }
    private void MeleeLogic (int frame, Vector2 NpcPosition, Vector2 TargetPosition, GameObject obj)
    {
        //Debug.Log("HP = " + obj.GetComponent<MonsterModel_Component>().HP);
        if (obj.GetComponent<MonsterModel_Component>().HP <= Fix64.Zero)
        {
            obj.GetComponent<AIDestinationSetter>().AI_Switch = false;
            CurrentState = (int)AI_BehaviorType.Dead;
            return;
        }
        float distance2Player = Vector2.Distance(NpcPosition, TargetPosition);
      
        if (distance2Player < AttackDistance && CurrentState != (int)AI_BehaviorType.Attack)
        {
            NextChangeStateFrame = frame;
            Attack = true;
        }
        else
        {
            Attack = false;
        }

        if (frame == NextChangeStateFrame)
        {
            if (Attack)
            {
                CurrentState = (int)AI_BehaviorType.Attack;
                TempState = CurrentState;
            }
            else
            {
                if (TempState == (int)AI_BehaviorType.Attack)
                {
                    CurrentState = (int)AI_BehaviorType.Run;
                    TempState = CurrentState;
                }
                else
                {
                    CurrentState = (int)AI_BehaviorType.Run;
                    TempState = CurrentState;
                }
            }

            if (obj.GetComponent<MonsterModel_Component>().UnderAttack)
            {
                CurrentState = (int)AI_BehaviorType.UnderAttack;
                TempState = CurrentState;
            }

            if (CurrentState == (int)AI_BehaviorType.Run)
            {
                //Debug.Log("Run");
                BossRunLogic(frame, obj,Vector2.zero);
                NextChangeStateFrame = frame + Run_FrameInterval;
            }
            else if (CurrentState == (int)AI_BehaviorType.Attack)
            {
                //Debug.Log("Attack");
                BossAttackLogic(frame, obj, TargetPosition);
                NextChangeStateFrame = frame + Attack_FrameInterval;
            }
            else if (CurrentState == (int)AI_BehaviorType.UnderAttack)
            {
                NextChangeStateFrame = frame + 1;
            }

        }
    }
    private void RangeLogic(int frame, Vector2 NpcPosition, Vector2 TargetPosition, GameObject obj)
    {

    }
    private void StaticRangeAILogic (int frame, Vector2 NpcPosition, Vector2 TargetPosition, GameObject obj)
    {

        //Debug.Log("NextChangeFrame: "+ NextChangeStateFrame);
        //Debug.Log("CurrentState: " + CurrentState);
        if (obj.GetComponent<MonsterModel_Component>().HP <= Fix64.Zero)
        {
            obj.GetComponent<AIDestinationSetter>().AI_Switch = false;
            CurrentState = (int)AI_BehaviorType.Dead;
            return;
        }
        float distance2Player = Vector2.Distance(NpcPosition, TargetPosition);

        //if (distance2Player < AttackDistance && TargetPosition!=Vector2.zero && CurrentState!= (int)AI_BehaviorType.Attack )
        if (distance2Player < AttackDistance && CurrentState != (int)AI_BehaviorType.Attack && TargetPosition != Vector2.zero)
        {
            //Debug.Log("ChangeState " );
            obj.GetComponent<AIDestinationSetter>().AI_Switch = true;
            NextChangeStateFrame = frame;
            Attack = true;
            //Debug.Log("Now NextChangeFrame: " + NextChangeStateFrame);
        }
        else
        {
            Attack = false;
            obj.GetComponent<AIDestinationSetter>().AI_Switch = false;
        }

        //if (Attack && CurrentState != (int)AI_BehaviorType.Attack)
        //{
        //    NextChangeStateFrame = frame;
        //}
        //else
        //{
        //}
        if (frame == NextChangeStateFrame)
        {
            if (Attack )
            {
                CurrentState = (int)AI_BehaviorType.Attack;
                TempState = CurrentState;
            }
            else
            {
                CurrentState = (int)AI_BehaviorType.Idle;
                TempState = CurrentState;
            }

            if (CurrentState == (int)AI_BehaviorType.Attack)
            {
                //Debug.Log("Attack");
                BossAttackLogic(frame, obj, TargetPosition);
                NextChangeStateFrame = frame + Attack_FrameInterval;
            }
            //else if (CurrentState == (int)AI_BehaviorType.Idle)
            //{
            //    BossRunLogic(frame, obj, TargetPosition);
            //    NextChangeStateFrame = frame + Idle_FrameInterval;
            //}

        }
        if (TargetPosition != Vector2.zero)
        {
            BossRunLogic(frame, obj, TargetPosition);
        }
            //NextChangeStateFrame = frame + Idle_FrameInterval;
      


    }

}