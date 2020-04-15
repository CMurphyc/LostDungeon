﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAI_Behavior 
{

    int CurrentState = 0;
    int TempState = 0;
   
    // 配置
    public int Idle_FrameInterval = 0;
    public int Run_FrameInterval = 0;
    public int Attack_FrameInterval = 0;
    public int AttackSpeedInterval = 0;
    public float AttackDistance = 0;
    // 需初始化
    int NextChangeStateFrame = 0;
    GameObject Boss;


    public RangeAI_Behavior(GameObject obj)
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
                if (CurrentState != (int)AI_BehaviorType.Attack)
                {
                    CurrentState = (int)AI_BehaviorType.Attack;
                   
                }

                if (CurrentState == (int)AI_BehaviorType.Attack)
                {
                    //Debug.Log("Attack");
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
