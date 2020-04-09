using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostAI : MonoBehaviour
{
    int Counter = 0;

    Ghost_AI t2;
    // Start is called before the first frame update
    void Start()
    {

        t2 = new Ghost_AI(gameObject);

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

class Ghost_AI : AI_Behavior
{
    GameObject Boss;
    public Ghost_AI(GameObject obj) : base(obj)
    {
        base.Idle_FrameInterval = 120;
        base.Run_FrameInterval = 120;
        base.Attack_FrameInterval = 120;
        Boss = obj;
    }
    public override void BossAttackLogic(int frame)
    {

    }
    public override void BossRunLogic(int frame)
    {

    }
}