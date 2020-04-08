using UnityEngine;
using System.Collections;



enum AI_BehaviorType
{
    Idle,
    Run,
    Attack,
    Dead

}
//使用说明 
//1.先Start()
//2.再Update()
class AI_Behavior
{

    int CurrentState = 0;
    int TempState = 0;
    // 配置
    public int Idle_FrameInterval = 0;
    public int Run_FrameInterval = 0;
    public int Attack_FrameInterval = 0;

    // 需初始化
    int NextChangeStateFrame = 0;
    GameObject Boss;


    public AI_Behavior(GameObject obj)
    {
        Boss = obj;

    }

    public void Start(int frame)
    {

        NextChangeStateFrame = frame + Idle_FrameInterval;

    }


    public void Update(int frame)
    {
        LogicUpdate(frame);
        UpdateView();

    }

    public void LogicUpdate(int frame)
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
                    Debug.Log("Run");
                    BossRunLogic(frame);
                    NextChangeStateFrame = frame + Run_FrameInterval;
                }
                else if (CurrentState == (int)AI_BehaviorType.Attack)
                {
                    Debug.Log("Attack");
                    BossAttackLogic(frame);
                    NextChangeStateFrame = frame + Attack_FrameInterval;
                }
            }
        }
    }

    public void UpdateView()
    {
        switch (CurrentState)
        {
            case (int)AI_BehaviorType.Idle:
                {
                    Boss.GetComponent<Animator>().SetInteger("MainState", 0);
                    break;
                }
            case (int)AI_BehaviorType.Run:
                {
                    Boss.GetComponent<Animator>().SetInteger("MainState", 1);
                    break;
                }
            case (int)AI_BehaviorType.Attack:
                {
                    Boss.GetComponent<Animator>().SetInteger("MainState", 2);
                    break;
                }
            case (int)AI_BehaviorType.Dead:
                {
                    Boss.GetComponent<Animator>().SetInteger("MainState", 3);
                    break;
                }

            default:
                break;
        }

    }
    public virtual void BossRunLogic(int frame)
    {



    }


    public virtual void BossAttackLogic(int frame)
    {



    }




}