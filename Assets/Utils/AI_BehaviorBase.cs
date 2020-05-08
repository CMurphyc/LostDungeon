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
    public Fix64 AttackDistance = (Fix64)0;
    public int SummoningInterval = 0;
    public int Skill1_FrameInterval = 0;
    public int Skill1_Duration = 0;
    public int Skill2_FrameInterval = 0;
    public Fix64 Skill1_Radius =(Fix64)0;
    //boss

    public Fix64 DashDistance = (Fix64)0;
    //传送到距离玩家距离
    public Fix64 DashToDistance = (Fix64)0;
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

    public void LogicUpdate(int frame , FixVector2 NpcPosition, FixVector2 TargetPosition, GameObject obj)
    {
        // void UpdateBuffLogic() 待封装
        //debuff静止
        if (obj.GetComponent<MonsterModel_Component>().Debuff.Freeze.isFreeze)
        {
            //静止状态
            return;
        }



        switch (type)
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
            case AI_Type.Boss_Wizard:
                {
                    WizardAILogic(frame, NpcPosition, TargetPosition, obj);
                    break;
                }

            case AI_Type.Boss_DarkKnight:
                {

                    DarkKnightLogic(frame, NpcPosition, TargetPosition, obj);
                    break;
                }
            case AI_Type.Boss_DarkKnightSword:
                {
                    MeleeLogic(frame, NpcPosition, TargetPosition, obj);
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
        obj.transform.rotation = obj.GetComponent<MonsterModel_Component>().Rotation;




        //怪物Buff动画更新
        switch (type)
        {
            case AI_Type.Boss_DarkKnight:
                {
                    if (obj.GetComponent<MonsterModel_Component>().buff.Undefeadted )
                    {
                        obj.transform.Find("boss01_shield").gameObject.GetComponent<SpriteRenderer>().enabled = true;
                    }
                    else
                    {
                        obj.transform.Find("boss01_shield").gameObject.GetComponent<SpriteRenderer>().enabled = false;
                    }
                    break;
                }
        }

        //怪物受击动画
        if (CurrentState != (int)AI_BehaviorType.UnderAttack)
        {
            obj.GetComponent<SpriteRenderer>().color = new Color(255f / 255f, 255f / 255f, 255f / 255f);
        }
        //if (obj.tag == "Boss" && obj.GetComponent<MonsterModel_Component>().UnderAttack)
        //{
        //    obj.GetComponent<SpriteRenderer>().color = new Color(255f / 255f, 93f / 255f, 93f / 255f);
        //}
        if ( obj.GetComponent<MonsterModel_Component>().UnderAttack)
        {
            obj.GetComponent<SpriteRenderer>().color = new Color(255f / 255f, 93f / 255f, 93f / 255f);
        }

        if (obj.GetComponent<MonsterModel_Component>().Debuff.Freeze.isFreeze)
        {

            //静止状态 不更新状态机
            obj.GetComponent<Animator>().speed = 0;
            return;
        }
        else
        {
            obj.GetComponent<Animator>().speed = 1;
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
            case (int)AI_BehaviorType.BossSkill1:
                {

                    obj.GetComponent<Animator>().SetInteger("MainState", 2);

                    break;
                }
            case (int)AI_BehaviorType.BossSkill2:
                {

                    obj.GetComponent<Animator>().SetInteger("MainState", 2);

                    break;
                }
            default:
                break;
        }

    }

    public virtual void BossRunLogic(int frame,GameObject obj, FixVector2 TargetPosition)
    { }
    public virtual void BossAttackLogic(int frame, GameObject obj, FixVector2 TargetPosition)
    { }
    public virtual void BossTPLogic(int frame, GameObject obj, FixVector2 ToPos,bool rot)
    { }
    public virtual void BossSkill1(int frame, GameObject obj, FixVector2 TargetPosition)
    {

    }
    public virtual void BossSkill2(int frame, GameObject obj, FixVector2 TargetPosition)
    {

    }


    private void DarkKnightLogic(int frame, FixVector2 NpcPosition, FixVector2 TargetPosition, GameObject obj)
    {
        if (obj.GetComponent<MonsterModel_Component>().HP <= Fix64.Zero)
        {
            obj.GetComponent<AIDestinationSetter>().AI_Switch = false;
            CurrentState = (int)AI_BehaviorType.Dead;
            return;
        }

        //if (CurrentState == (int)AI_BehaviorType.Dead)
        //    return;
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
                    //int Skill = 0;
                    int Skill = Random.Range(0, 3);
                    switch (Skill)
                    {
                        case 0:
                            {
                                CurrentState = (int)AI_BehaviorType.Attack;
                                break;
                            }
                        case 1:
                            {
                                CurrentState = (int)AI_BehaviorType.BossSkill1;
                                break;
                            }
                        case 2:
                            {
                                CurrentState = (int)AI_BehaviorType.BossSkill2;
                                break;

                            }
                    }
                    TempState = CurrentState;
                }
                else
                {
                    CurrentState = (int)AI_BehaviorType.Run;
                    TempState = CurrentState;
                }


                switch (CurrentState)
                {
                    case (int)AI_BehaviorType.Run:
                        {
                            Fix64 distance2Player = FixVector2.Distance(NpcPosition, TargetPosition);
                            FixVector2 vec = ((TargetPosition - NpcPosition) * (Fix64)100).GetNormalized() * DashToDistance;
                            FixVector2 ToPosition = TargetPosition - vec;
                            if (distance2Player > DashDistance)
                            {
                                Dash = true;
                                BossTPLogic(frame, obj, ToPosition, TargetPosition.x >= NpcPosition.x);
                                NextChangeStateFrame = frame + Teleport_FrameInterval;
                            }
                            else
                            {
                                Dash = false;
                                BossRunLogic(frame, obj, FixVector2.Zero);
                                NextChangeStateFrame = frame + Run_FrameInterval;
                            }


                            break;
                        }
                    case (int)AI_BehaviorType.Attack:
                        {

                            BossAttackLogic(frame, obj, TargetPosition);
                            NextChangeStateFrame = frame + Attack_FrameInterval;

                            break;
                        }

                    case (int)AI_BehaviorType.BossSkill1:
                        {
                            BossSkill1(frame, obj, TargetPosition);
                            NextChangeStateFrame = frame + Skill1_FrameInterval;
                            break;
                        }
                    case (int)AI_BehaviorType.BossSkill2:
                        {
                            BossSkill2(frame, obj, TargetPosition);
                            NextChangeStateFrame = frame + Skill2_FrameInterval;
                            break;
                        }

                }

            }
        }

    }
    private void WizardAILogic(int frame, FixVector2 NpcPosition, FixVector2 TargetPosition, GameObject obj)
    {
        if (obj.GetComponent<MonsterModel_Component>().HP <= Fix64.Zero)
        {
            obj.GetComponent<AIDestinationSetter>().AI_Switch = false;
            CurrentState = (int)AI_BehaviorType.Dead;
            return;
        }
        //if (CurrentState == (int)AI_BehaviorType.Dead)
        //    return;
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
                    //int Skill = 0;
                    int Skill = Random.Range(0, 2);
                    switch (Skill)
                    {
                        case 0:
                            {
                                CurrentState = (int)AI_BehaviorType.Attack;
                                break;
                            }
                        case 1:
                            {
                                CurrentState = (int)AI_BehaviorType.BossSkill1;
                                break;
                            }
                    }
                    TempState = CurrentState;
                }
                else
                {
                    CurrentState = (int)AI_BehaviorType.Run;
                    TempState = CurrentState;
                }


                switch(CurrentState)
                {
                    case (int)AI_BehaviorType.Run:
                        {
                            Fix64 distance2Player = FixVector2.Distance(NpcPosition, TargetPosition);
                            FixVector2 vec = ((TargetPosition - NpcPosition) * (Fix64)100).GetNormalized() * DashToDistance;
                            FixVector2 ToPosition = TargetPosition - vec;
                            if (distance2Player > DashDistance)
                            {
                                Dash = true;
                                BossTPLogic(frame, obj, ToPosition, TargetPosition.x >= NpcPosition.x);
                                NextChangeStateFrame = frame + Teleport_FrameInterval;
                            }
                            else
                            {
                                Dash = false;
                                BossRunLogic(frame, obj, FixVector2.Zero);
                                NextChangeStateFrame = frame + Run_FrameInterval;
                            }


                            break;
                        }
                    case (int)AI_BehaviorType.Attack:
                        {

                            BossAttackLogic(frame, obj, TargetPosition);
                            NextChangeStateFrame = frame + Attack_FrameInterval;

                            break;
                        }

                    case (int)AI_BehaviorType.BossSkill1:
                        {
                            BossSkill1(frame, obj, TargetPosition);
                            NextChangeStateFrame = frame + Skill1_FrameInterval;
                            break;
                        }
                    case (int)AI_BehaviorType.BossSkill2:
                        {

                            break;
                        }

                }
              
            }
        }

    }
    private void RaibitLogic(int frame, FixVector2 NpcPosition, FixVector2 TargetPosition, GameObject obj)
    {
        if (obj.GetComponent<MonsterModel_Component>().HP <= Fix64.Zero)
        {
            obj.GetComponent<AIDestinationSetter>().AI_Switch = false;
            CurrentState = (int)AI_BehaviorType.Dead;
            return;
        }


        //if (CurrentState == (int)AI_BehaviorType.Dead)
        //{
        //    Debug.Log("Rabit dead");
        //    return;
        //}
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

                    Fix64 distance2Player = FixVector2.Distance(NpcPosition, TargetPosition);
                    FixVector2 vec = ((TargetPosition - NpcPosition)*(Fix64)100).GetNormalized() * DashToDistance;
                    FixVector2 ToPosition = TargetPosition - vec;
                    if (distance2Player > DashDistance)
                    {
                        Dash = true;
                        BossTPLogic(frame, obj , ToPosition , TargetPosition.x >= NpcPosition.x);
                        NextChangeStateFrame = frame + Teleport_FrameInterval;
                    }
                    else
                    {
                        Dash = false;
                        BossRunLogic(frame, obj, FixVector2.Zero);
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
    private void MeleeLogic (int frame, FixVector2 NpcPosition, FixVector2 TargetPosition, GameObject obj)
    {
        //Debug.Log("HP = " + obj.GetComponent<MonsterModel_Component>().HP);
        if (obj.GetComponent<MonsterModel_Component>().HP <= Fix64.Zero)
        {
            obj.GetComponent<AIDestinationSetter>().AI_Switch = false;
            CurrentState = (int)AI_BehaviorType.Dead;
            return;
        }
        Fix64 distance2Player = FixVector2.Distance(NpcPosition, TargetPosition);
      
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
                BossRunLogic(frame, obj, TargetPosition);
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
    private void RangeLogic(int frame, FixVector2 NpcPosition, FixVector2 TargetPosition, GameObject obj)
    {
        if (obj.GetComponent<MonsterModel_Component>().HP <= Fix64.Zero)
        {
            obj.GetComponent<AIDestinationSetter>().AI_Switch = false;
            CurrentState = (int)AI_BehaviorType.Dead;
            return;
        }
        Fix64 distance2Player = FixVector2.Distance(NpcPosition, TargetPosition);

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
                BossRunLogic(frame, obj, FixVector2.Zero);
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
    private void StaticRangeAILogic (int frame, FixVector2 NpcPosition, FixVector2 TargetPosition, GameObject obj)
    {


        if (obj.GetComponent<MonsterModel_Component>().HP <= Fix64.Zero)
        {
            obj.GetComponent<AIDestinationSetter>().AI_Switch = false;
            CurrentState = (int)AI_BehaviorType.Dead;
            return;
        }
        Fix64 distance2Player = FixVector2.Distance(NpcPosition, TargetPosition);

     
        if (distance2Player < AttackDistance && CurrentState != (int)AI_BehaviorType.Attack && TargetPosition != FixVector2.Zero)
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
        if (TargetPosition != FixVector2.Zero)
        {
            BossRunLogic(frame, obj, TargetPosition);
        }
            //NextChangeStateFrame = frame + Idle_FrameInterval;
      


    }

}