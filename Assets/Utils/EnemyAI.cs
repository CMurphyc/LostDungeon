using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterData
{
    GameObject obj;

}

public class EnemyAI : MonoBehaviour
{
    
    AI_Enemy AI_Controller;

    bool Inited = false;
    void Start()
    {
        AI_Controller = new AI_Enemy();
    }
    public void InitMonster(int frame)
    {
        if (Inited == false)
        {
            AI_Controller.Start(frame);
            Inited = true;
        }

    }

    public void UpdateLogic(GameObject target,int frame)
    {
        Vector2 MonsterPos = new Vector2();
        Vector2 tar= new Vector2();
        
        

        if (target != null)
        {
            MonsterPos = new Vector2((float)GetComponent<MonsterModel_Component>().position.x, (float)GetComponent<MonsterModel_Component>().position.y);
          
            tar = new  Vector2((float)target.GetComponent<PlayerModel_Component>().GetPlayerPosition().x, (float)target.GetComponent<PlayerModel_Component>().GetPlayerPosition().y);
        }
        else
        {

            Debug.Log("null");
        }
        AI_Controller.LogicUpdate(frame, MonsterPos, tar, gameObject);
    }
    public void UpdateView()
    {
        
        AI_Controller.UpdateView(gameObject);
    }
}
class AI_Enemy : MeleeAI_Behavior
{
   
    
    public AI_Enemy() : base()
    {
        base.Idle_FrameInterval = 60;
        base.Run_FrameInterval = 1;
        base.Attack_FrameInterval = 60;
        base.AttackDistance = 0.07f;
    }

    public override void BossAttackLogic(int frame, GameObject obj)
    {
        //Debug.Log("Logic Monster HP: " + Boss.GetComponent<MonsterModel_Component>().HP);
        //if (Vector2.Distance(Boss.transform.position, TargetPosition) <=1)
        {


            Debug.Log("BeAttacked");
        }
    }
    public override void BossRunLogic(int frame, GameObject Boss)
    {
        //Debug.Log("Runnning Logic");

        Debug.Log("Logic Monster HP: " + Boss.GetComponent<MonsterModel_Component>().HP);

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
