using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    int Counter = 0;
    AI_Enemy AI_Controller;
    public GameObject target;

    void Start()
    {
        AI_Controller = new AI_Enemy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        //Counter += 1;
        //if (Counter == 120)
        //{
        //    t2.Start(Counter);
        //}
        //Vector2 vec2 = transform.position;
        //Vector2 tar = target.transform.position;
        //t2.Update(Counter, vec2, tar);

    }
    public void UpdateLogic(GameObject target,int frame)
    {
        Vector2 vec2 = new Vector2();
        Vector2 tar= new Vector2();
        if (target != null)
        {
            vec2 = transform.position;
            tar = target.transform.position;
        }
        AI_Controller.Update(Counter, vec2, tar);
    }
    public void UpdateView()
    {

    }
}
class AI_Enemy : MeleeAI_Behavior
{
    GameObject Boss;
    public SystemManager sys;
    public AI_Enemy(GameObject obj) : base(obj)
    {
        //base.Idle_FrameInterval = 60;
        base.Run_FrameInterval = 1;
        base.Attack_FrameInterval = 60;
        base.AttackDistance = 1;
        Boss = obj;
        
        sys = new SystemManager();
    }

    public override void BossAttackLogic(int frame)
    {
        //if (Vector2.Distance(Boss.transform.position, TargetPosition) <=1)
        {
            Debug.Log("BeAttacked");
        }
    }
    public override void BossRunLogic(int frame)
    {

        Boss.GetComponent<AIPath>().InitConfig(Boss.transform.position, Boss.transform.rotation, new Vector3(1.5f, 1.5f, 1.5f), Global.FrameRate);
        //获取当前帧位置
        Vector3 Pos;
        Quaternion Rot;
        Boss.GetComponent<AIPath>().GetFramePosAndRotation(out Pos, out Rot);
        Boss.transform.position = Pos;
        Boss.transform.rotation = Rot;

    }
}
