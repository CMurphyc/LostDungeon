using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttribute
{
    public int Attack_FrameInterval;
    public float SpinRate;

}

public class EnemyAI : MonoBehaviour
{
    AI_Enemy AI_Controller;
    public AI_Type AItype;
    bool Inited = false;
    int RoomID;
    BossAttribute attribute;
    //创建实例后需要初始化AI
    public void InitAI(AI_Type type,int roomid, BossAttribute attribute)
    {
        AItype = type;
        RoomID = roomid;
        AI_Controller = new AI_Enemy(AItype, RoomID, attribute);
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
        Vector2 MonsterPos = new Vector2();
        Vector2 tar= Vector2.zero;
        if (target != null)
        {
            MonsterPos = new Vector2((float)GetComponent<MonsterModel_Component>().position.x, (float)GetComponent<MonsterModel_Component>().position.y);

            if (isEnemy)
            {
                tar = new Vector2((float)target.GetComponent<PlayerModel_Component>().GetPlayerPosition().x, (float)target.GetComponent<PlayerModel_Component>().GetPlayerPosition().y);
            }
            else
            {
                tar = new Vector2((float)target.GetComponent<MonsterModel_Component>().position.x, (float)target.GetComponent<MonsterModel_Component>().position.y);

            }
        }

        AI_Controller.LogicUpdate(frame, MonsterPos, tar, MonsterObj);
    }
    public void UpdateView(GameObject MonsterObj)
    {
        
        AI_Controller.UpdateView(MonsterObj);
    }
}
class AI_Enemy : AI_BehaviorBase
{
    SystemManager sys;
    BossAttribute BossConfig;
    public AI_Enemy(AI_Type AItype, int RoomID,BossAttribute attribute) : base()
    {
        sys = GameObject.Find("GameEntry").GetComponent<GameMain>().WorldSystem;
        base.type = AItype;
        base.RoomID = RoomID;
        BossConfig = attribute;
        if (type == AI_Type.Normal_Melee || type == AI_Type.Boss_Rabit_Egg)
        {
            base.Idle_FrameInterval = 1000 / Global.FrameRate;
            base.Run_FrameInterval = 1;
            base.AttackDistance = 0.6f;
            base.Attack_FrameInterval = 20;
        }
        else if (type == AI_Type.Boss_Rabit)
        {
            base.Idle_FrameInterval = 40;
            base.Run_FrameInterval = 60;
            base.Attack_FrameInterval = 100;
            base.DashDistance = 7f;
            base.Teleport_FrameInterval = 1;
            base.DashToDistance = 2f;
            base.SummoningInterval = 20;
        }
        else if (type == AI_Type.Engineer_TerretTower)
        {
            base.Idle_FrameInterval = 1;
            base.AttackDistance = 5f;
            base.Attack_FrameInterval = attribute.Attack_FrameInterval;
        }

    }
    public override void BossTPLogic(int frame, GameObject obj , Vector2 ToPos , bool rot)
    {
        switch(type)
        {
            case AI_Type.Boss_Rabit:
                {
                    Debug.Log("Rabit TP");
                    obj.GetComponent<MonsterModel_Component>().position = new FixVector3(PackConverter.Vector3ToFixVector3((Vector3)ToPos));
                    if (rot)
                    {
                        obj.GetComponent<MonsterModel_Component>().Rotation = new Quaternion(0, 0, 0, 0);
                    }
                    else
                    {
                        obj.GetComponent<MonsterModel_Component>().Rotation = new Quaternion(0, 180f, 0, 0);

                    }
                    break;
                }
            default:
                break;
        }
    }
    public override void BossAttackLogic(int frame, GameObject obj, Vector2 TargetPosition)
    {
        switch(type)
        {
            case AI_Type.Nomral_Range:
                {
                    break;
                }
            case AI_Type.Normal_Melee:
                {
                    break;
                }
            case AI_Type.Boss_Rabit:
                {
                    Debug.Log("Rabit AttackeING");
                    //int BulletTypeNumber = 4;
                    int Switch = Random.Range(1, 3);
                    if (Switch == 1)
                    {

                        int AttackRate = 5;
                        int AttackNumber = base.Attack_FrameInterval / AttackRate;

                        int InitAngle = Random.Range(1, 30);
                        //BulletUnion bu = new BulletUnion(sys._battle);
                        for (int i = 0; i < AttackNumber; i++)
                        {
                            int AttackInitFrame = frame + i * AttackRate;
                            //Debug.Log("AttackFrame: " + AttackInitFrame);

                            InitAngle += 5;
                            int BulletNumber = 12;
                            float angle = 360 / BulletNumber;

                            List<FakeBulletUnion> bulletList = new List<FakeBulletUnion>();

                            for (int j = 0; j < BulletNumber; j++)
                            {
                                List<int> list = new List<int>();


                                float CurrentAngle = InitAngle + angle * j;
                                Vector2 toward = new Vector2(Mathf.Cos(CurrentAngle / 180f * Mathf.PI), Mathf.Sin(CurrentAngle / 180f * Mathf.PI));
                                toward = toward.normalized;

                                FakeBulletUnion bu = new FakeBulletUnion("Boss_Rabit", new FixVector2(obj.GetComponent<MonsterModel_Component>().position.x,
                                                                    obj.GetComponent<MonsterModel_Component>().position.y),
                                                                    new FixVector2((Fix64)toward.x,
                                                                    (Fix64)toward.y),
                                                                    (Fix64)0.1, (Fix64)1, sys._battle._monster.BossRoom,
                                                                    Resources.Load("Model/Boss/Boss_Rabit/bullet/bullet_30") as GameObject
                                                                    , list);

                                bulletList.Add(bu);
                            }

                            if (!sys._battle._monster.bulletEvent.ContainsKey(AttackInitFrame))
                            {
                                sys._battle._monster.bulletEvent.Add(AttackInitFrame, bulletList);
                            }
                            else
                            {
                                sys._battle._monster.bulletEvent[AttackInitFrame] = bulletList;
                            }
                        }


                    }
                    else
                    {
                        int EggNumber = 4;


                        Vector3 pos = new Vector3((float)obj.GetComponent<MonsterModel_Component>().position.x, (float)obj.GetComponent<MonsterModel_Component>().position.y, 0f);
                        GameObject Egg_Prefab = (GameObject)Resources.Load("Model/Boss/Boss_Rabit/enemy_egg");

                        List<FixVector3> PosList = new List<FixVector3> { new FixVector3((Fix64)pos.x - 1, (Fix64)pos.y - 1, (Fix64)0),
                            new FixVector3((Fix64)pos.x - 1, (Fix64)pos.y + 1, (Fix64)0), 
                            new FixVector3((Fix64)pos.x + 1, (Fix64)pos.y - 1, (Fix64)0), 
                            new FixVector3((Fix64)pos.x + 1, (Fix64)pos.y + 1, (Fix64)0) };

                        for (int i = 0; i < EggNumber; i++)
                        {
                            int RoomID = sys._battle._monster.BossRoom;
                            GameObject Egg_Instance = Object.Instantiate(Egg_Prefab, pos, Quaternion.identity);
                            Egg_Instance.GetComponent<MonsterModel_Component>().position = PosList[i];
                            Egg_Instance.GetComponent<MonsterModel_Component>().HP = (Fix64)2;
                            Egg_Instance.GetComponent<EnemyAI>().InitAI(AI_Type.Boss_Rabit_Egg, RoomID,null);

                           
                            if (sys._battle._monster.RoomToMonster.ContainsKey(RoomID))
                            {
                                sys._battle._monster.RoomToMonster[RoomID].Add(Egg_Instance);
                            }
                        }
                        RestartChange(frame + SummoningInterval);
                    }
                    break;
                }
            case AI_Type.Boss_Rabit_Egg:
                {
                    Debug.Log("蛋蛋攻击");
                    break;
                }
            case AI_Type.Engineer_TerretTower:
                {
                    //Debug.Log("Terret Attack");
                    //Debug.Log("Frame: "+ frame);
                    List<int> list = new List<int>();
                    BulletUnion bu = new BulletUnion(sys._battle);

                    Vector2 MonsPos = new Vector2((float)obj.GetComponent<MonsterModel_Component>().position.x, (float)obj.GetComponent<MonsterModel_Component>().position.y);
                    Vector2 toward = (TargetPosition - MonsPos).normalized;
                    TurnLogic(toward, obj.transform.Find("engineer_derivative_1_3"), BossConfig.SpinRate);

                    float degree = obj.transform.Find("engineer_derivative_1_3").transform.eulerAngles.z;
                    FixVector2 ShootPos = new FixVector2((Fix64)obj.transform.Find("engineer_derivative_1_3").Find("ShotBulletPosition").position.x, 
                                            (Fix64)obj.transform.Find("engineer_derivative_1_3").Find("ShotBulletPosition").position.y);

                    FixVector2 ShootToward = new FixVector2((Fix64) Mathf.Cos(degree* Mathf.PI/180f),
                                            (Fix64)Mathf.Sin(degree * Mathf.PI / 180f));
                    bu.BulletInit("AliasAI", ShootPos,new FixVector2((Fix64)ShootToward.x,
                                                                  (Fix64)ShootToward.y),
                                                                  (Fix64)0.1, (Fix64)1, base.RoomID,
                                                                  Resources.Load("Model/Boss/Boss_Rabit/bullet/bullet_30") as GameObject
                                                                  , list);
                    sys._battle._monster.bulletList.Add(bu);
                    break;
                }
            default:
                break;
        }
    }
    public override void BossRunLogic(int frame, GameObject Boss,Vector2 TargetPosition)
    {
        switch (type)
        {
            case AI_Type.Nomral_Range:
                break;
            case AI_Type.Normal_Melee:
                {

                    Vector3 MonsterPos = new Vector3((float)Boss.GetComponent<MonsterModel_Component>().position.x, (float)Boss.GetComponent<MonsterModel_Component>().position.y);
                    Boss.GetComponent<AIPath>().InitConfig(MonsterPos, Boss.GetComponent<MonsterModel_Component>().Rotation, new Vector3(1.5f, 1.5f, 1.5f), Global.FrameRate);
                    //获取当前帧位置
                    Vector3 Pos;
                    Quaternion Rot;
                    Boss.GetComponent<AIPath>().GetFramePosAndRotation(out Pos, out Rot);

                    FixVector3 FixMonsterPos = new FixVector3((Fix64)Pos.x, (Fix64)Pos.y, (Fix64)Pos.z);
                    Boss.GetComponent<MonsterModel_Component>().position = FixMonsterPos;
                    Boss.GetComponent<MonsterModel_Component>().Rotation = Rot;

                    break;
                }
            case AI_Type.Boss_Rabit:
                {
                    if (!sys._battle._monster.BossMove.ContainsKey(Boss))
                    {
                        
                        sys._battle._monster.BossMove.Add(Boss, Run_FrameInterval);
                    }
                    break;
                }
            case AI_Type.Boss_Rabit_Egg:
                {
                    Vector3 MonsterPos = new Vector3((float)Boss.GetComponent<MonsterModel_Component>().position.x, (float)Boss.GetComponent<MonsterModel_Component>().position.y);
                    Boss.GetComponent<AIPath>().InitConfig(MonsterPos, Boss.GetComponent<MonsterModel_Component>().Rotation, new Vector3(1.5f, 1.5f, 1.5f), Global.FrameRate);
                    //获取当前帧位置
                    Vector3 Pos;
                    Quaternion Rot;
                    Boss.GetComponent<AIPath>().GetFramePosAndRotation(out Pos, out Rot);

                    FixVector3 FixMonsterPos = new FixVector3((Fix64)Pos.x, (Fix64)Pos.y, (Fix64)Pos.z);
                    Boss.GetComponent<MonsterModel_Component>().position = FixMonsterPos;
                    Boss.GetComponent<MonsterModel_Component>().Rotation = Rot;
                    break;
                }
            case AI_Type.Engineer_TerretTower:
                {
                    Vector2 MonsPos = new Vector2((float)Boss.GetComponent<MonsterModel_Component>().position.x, (float)Boss.GetComponent<MonsterModel_Component>().position.y);
                    Vector2 toward = (TargetPosition - MonsPos).normalized;
                    TurnLogic(toward, Boss.transform.Find("engineer_derivative_1_3"), BossConfig.SpinRate);

                    break;
                }
        }

    }

    void TurnLogic(Vector2 dir, Transform tran , float change)
    {
        float A = tran.eulerAngles.z;
        float B = Mathf.Atan2(dir.y, dir.x) * 180 / Mathf.PI;
        while (A < 0) A += 360;
        while (B < 0) B += 360;
        if (A > B)
        {
            if (A - B < 360 - (A - B)) A-=change;
            else A+=change;
        }
        else
        {
            if ((B - A) < 360 - (B - A)) A+=change;
            else A-=change;
        }
        while (A > 360) A -= 360;
        while (A < 0) A += 360;
        if (A > 90 && A < 270) tran.GetComponent<SpriteRenderer>().flipY = true;
        else tran.GetComponent<SpriteRenderer>().flipY = false;
        tran.eulerAngles = new Vector3(0, 0, A);
    }

}
