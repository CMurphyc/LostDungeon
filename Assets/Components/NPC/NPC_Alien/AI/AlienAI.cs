using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienAI : MonoBehaviour
{
    int Counter = 0;
    AI_Alien t2;
    public GameObject target;
    List<AnimationInfo> AniList = new List<AnimationInfo>();
    // Start is called before the first frame update
    void Start()
    {
        t2 = new AI_Alien(gameObject);
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


class AI_Alien : RangeAI_Behavior
{
    public Dictionary<int, AnimationInfo> AniInfo = new Dictionary<int, AnimationInfo>();
    GameObject Boss;
    public AI_Alien(GameObject obj) : base(obj)
    {
        base.Idle_FrameInterval = 60;
   
        base.Attack_FrameInterval = 60;
        
        Boss = obj;
    }

    public override void BossAttackLogic(int frame)
    {
        Debug.Log("AI_ Attack");


    }

}