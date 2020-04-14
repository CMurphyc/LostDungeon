using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathKnightAI : MonoBehaviour
{
    int Counter = 0;
    AI_DeathKnight t2;
    public GameObject target;
    List<AnimationInfo> AniList = new List<AnimationInfo>();
    // Start is called before the first frame update
    void Start()
    {
        t2 = new AI_DeathKnight(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        Counter += 1;

        if (Counter == 120)
        {
            t2.Start(Counter);
        }
        Vector2 vec2 = transform.position;
        Vector2 tar = target.transform.position;
        t2.Update(Counter, vec2, tar);

   
    }
}





class AI_DeathKnight : MeleeAI_Behavior
{
    public Dictionary<int, AnimationInfo> AniInfo = new Dictionary<int, AnimationInfo>();
    GameObject Boss;
    public AI_DeathKnight(GameObject obj) : base(obj)
    {
        //base.Idle_FrameInterval = 60;
        base.Run_FrameInterval = 1;
        base.Attack_FrameInterval = 60;
        base.AttackDistance = 1;
        Boss = obj;
    }

    public override void BossAttackLogic(int frame)
    {

        Debug.Log("Strat Attack");
    }
    public override void BossRunLogic(int frame)
    {
        Debug.Log("Start Run");


        Boss.GetComponent<AIPath>().InitConfig(Boss.transform.position, Boss.transform.rotation, new Vector3(1.5f, 1.5f, 1.5f), Global.FrameRate);
        //获取当前帧位置
        Vector3 Pos;
        Quaternion Rot;
        Boss.GetComponent<AIPath>().GetFramePosAndRotation(out Pos, out Rot);
        Boss.transform.position = Pos;
     
        Boss.transform.rotation = Rot;
        
      
    }
}