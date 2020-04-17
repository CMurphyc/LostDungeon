using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    
    AI_Enemy AI_Controller;
    

    void Start()
    {
        AI_Controller = new AI_Enemy(gameObject);
    }

    public void UpdateLogic(GameObject target,int frame)
    {
        Vector2 MonsterPos = new Vector2();
        Vector2 tar= new Vector2();
        if (target != null)
        {
            MonsterPos = new Vector2((float)GetComponent<MonsterModel_Component>().position.x, (float)GetComponent<MonsterModel_Component>().position.y);
          
            tar =  PackConverter.FixVector3ToVector3(target.GetComponent<PlayerModel_Component>().GetPlayerPosition());
        }
        AI_Controller.LogicUpdate(frame, MonsterPos, tar);
    }
    public void UpdateView()
    {
        
        AI_Controller.UpdateView();
    }
}
class AI_Enemy : MeleeAI_Behavior
{
    GameObject Boss;
    public SystemManager sys;
    public AI_Enemy(GameObject obj) : base(obj)
    {
        base.Idle_FrameInterval = 60;
        base.Run_FrameInterval = 1;
        base.Attack_FrameInterval = 60;
        base.AttackDistance = 0.07f;
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
        //Debug.Log("Runnning Logic");
        Vector3 MonsterPos = new Vector3((float)Boss.GetComponent<MonsterModel_Component>().position.x, (float)Boss.GetComponent<MonsterModel_Component>().position.y);
        Boss.GetComponent<AIPath>().InitConfig(MonsterPos, Boss.GetComponent<MonsterModel_Component>().Rotation, new Vector3(1.5f, 1.5f, 1.5f), Global.FrameRate);
        //获取当前帧位置
        Vector3 Pos;
        Quaternion Rot;
        Boss.GetComponent<AIPath>().GetFramePosAndRotation(out Pos, out Rot);

        //Debug.Log("Pos "+Pos);
        //Debug.Log("Rot " + Rot);

        FixVector3 FixMonsterPos = new FixVector3((Fix64)Pos.x, (Fix64)Pos.y, (Fix64)Pos.z);
        Boss.GetComponent<MonsterModel_Component>().position = FixMonsterPos;
        Boss.GetComponent<MonsterModel_Component>().Rotation = Rot;

    }
}
