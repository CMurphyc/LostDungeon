using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;






//使用说明 
//1.先Start()
//2.再Update()
class MeleeAI_Behavior
{

    int CurrentState = 0;
    int TempState = 0;
    bool Attack = false;
    // 配置
    public int Idle_FrameInterval = 0;
    public int Run_FrameInterval = 0;
    public int Attack_FrameInterval = 0;
    public int AttackSpeedInterval = 0;
    public float AttackDistance = 0;
    // 需初始化
    int NextChangeStateFrame = 0;
    


    public MeleeAI_Behavior()
    {
      
    }

    public void Start(int frame)
    {
        NextChangeStateFrame = frame + Idle_FrameInterval;
    }


    public void Update(int frame, Vector2 NpcPosition, Vector2 TargetPosition)
    {
        //LogicUpdate(frame, NpcPosition, TargetPosition);
        //UpdateView();
    }

    public void LogicUpdate(int frame , Vector2 NpcPosition, Vector2 TargetPosition, GameObject obj)
    {

        //Debug.Log("FrameNumber: "+ frame);
        //Debug.Log("NextChangeFrame: " + NextChangeStateFrame);

        //Debug.Log(obj.GetComponent<MonsterModel_Component>().HP);
        if (obj.GetComponent<MonsterModel_Component>().HP<=Fix64.Zero)
        {
            //Debug.Log("HP =0 STATE = Dead");
            obj.GetComponent<AIDestinationSetter>().AI_Switch = false;
            CurrentState = (int)AI_BehaviorType.Dead;
            return;
        }

        float distance2Player = Vector2.Distance(NpcPosition, TargetPosition);

        if (distance2Player< AttackDistance && CurrentState!= (int)AI_BehaviorType.Attack)
        {

            Debug.Log("In Attack Range:" + distance2Player);
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

            if (CurrentState == (int)AI_BehaviorType.Run)
            {
                //Debug.Log("Run");
                BossRunLogic(frame, obj);
                NextChangeStateFrame = frame + Run_FrameInterval;
            }
            else if (CurrentState == (int)AI_BehaviorType.Attack)
            {
                //Debug.Log("Attack");
                BossAttackLogic(frame, obj);
                NextChangeStateFrame = frame + Attack_FrameInterval;
            }
        }
    }

    public void UpdateView(GameObject obj)
    {
        obj.transform.position = new Vector2((float)obj.GetComponent<MonsterModel_Component>().position.x, (float)obj.GetComponent<MonsterModel_Component>().position.y);
        obj.transform.rotation = obj.GetComponent<MonsterModel_Component>().Rotation;
        switch (CurrentState)
        {
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

            default:
                break;
        }

    }
    public virtual void BossRunLogic(int frame,GameObject obj)
    {



    }


    public virtual void BossAttackLogic(int frame, GameObject obj)
    {



    }




}