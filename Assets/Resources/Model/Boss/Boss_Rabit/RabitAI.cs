using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabitAI : MonoBehaviour
{
    int Counter= 0;
    int state = 0;
    Animator ani;
    bool inited =false;

    int next_SwitchFrame = 130;

    int next_IdleFrame = 200;


    int IdleInterval = 120;

    AI_Behavior temp;
    AI_Rabit t2;
    
    // Start is called before the first frame update
    void Start()
    {
        ani = GetComponent<Animator>();
        //temp = new AI_Behavior(gameObject);
        t2 = new AI_Rabit(gameObject);
      
    }

    // Update is called once per frame
    void Update()
    {
        Counter += 1;

        if (Counter == 120)
        {

            t2.Start(Counter);
        }
        t2.Update(Counter);
       
    }
    
}




class AI_Rabit:AI_Behavior
{
    public AI_Rabit(GameObject obj):base(obj)
    {
        base.Idle_FrameInterval = 120;
        base.Run_FrameInterval = 120;
        base.Attack_FrameInterval = 60;
    }
    public override void BossAttackLogic(int frame)
    {
     
    }
    public override void BossRunLogic(int frame)
    {
      
    }
}



