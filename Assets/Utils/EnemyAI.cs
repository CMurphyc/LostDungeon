using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttribute
{
    public int Attack_FrameInterval;
    public int SpinRate;

}

public class EnemyAI : MonoBehaviour
{
    AI_BehaviorTree AI_Controller;
    public AI_Type AItype;
    bool Inited = false;
    int RoomID;
    BossAttribute attribute;
    //创建实例后需要初始化AI
    public void InitAI(AI_Type type,int roomid, BossAttribute attribute)
    {
        AItype = type;
        RoomID = roomid;
        AI_Controller = new AI_BehaviorTree(AItype, RoomID, attribute);
    }
    public void InitMonster(int frame)
    {
        if (Inited == false)
        {
            AI_Controller.Start(frame);
            Inited = true;
        }
    }
    public void UpdateLogic(GameObject target,int frame, GameObject MonsterObj,bool isEnemy)
    {
        FixVector2 MonsterPos = new FixVector2();
        FixVector2 tar = FixVector2.Zero;
        if (target != null)
        {
           MonsterPos = PackConverter.FixVector3ToFixVector2(GetComponent<MonsterModel_Component>().position);
            if (isEnemy)
            {
               tar = target.GetComponent<PlayerModel_Component>().GetPlayerPosition();
            }
            else
            {
                tar = PackConverter.FixVector3ToFixVector2(target.GetComponent<MonsterModel_Component>().position);            }
        }
        AI_Controller.LogicUpdate(frame, MonsterPos, tar, MonsterObj);
    }
    public void UpdateView(GameObject MonsterObj)
    {
        
        AI_Controller.UpdateView(MonsterObj);
    }
}
