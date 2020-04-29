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



    public void UpdateLogic(GameObject target,int frame, GameObject MonsterObj,bool isEnemy , Fix64 GameCounter)
    {
        //Vector2 MonsterPos = new Vector2();
        //Vector2 tar= Vector2.zero;
        FixVector2 MonsterPos = new FixVector2();
        FixVector2 tar = FixVector2.Zero;
        if (target != null)
        {
            //MonsterPos = new Vector2((float)GetComponent<MonsterModel_Component>().position.x, (float)GetComponent<MonsterModel_Component>().position.y);
            MonsterPos = PackConverter.FixVector3ToFixVector2(GetComponent<MonsterModel_Component>().position);
            if (isEnemy)
            {
                //tar = new Vector2((float)target.GetComponent<PlayerModel_Component>().GetPlayerPosition().x, (float)target.GetComponent<PlayerModel_Component>().GetPlayerPosition().y);
                tar = target.GetComponent<PlayerModel_Component>().GetPlayerPosition();
            }
            else
            {
                //tar = new Vector2((float)target.GetComponent<MonsterModel_Component>().position.x, (float)target.GetComponent<MonsterModel_Component>().position.y);

                tar = PackConverter.FixVector3ToFixVector2(target.GetComponent<MonsterModel_Component>().position);

            }
        }

        AI_Controller.LogicUpdate(frame, MonsterPos, tar, MonsterObj,  GameCounter);
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
            base.AttackDistance =(Fix64)0.6f;
            base.Attack_FrameInterval = 20;
        }
        else if (type == AI_Type.Boss_Rabit)
        {
            base.Idle_FrameInterval = 40;
            base.Run_FrameInterval = 60;
            base.Attack_FrameInterval = 100;
            base.DashDistance = (Fix64)7f;
            base.Teleport_FrameInterval = 1;
            base.DashToDistance = (Fix64)2f;
            base.SummoningInterval = 20;
        }
        else if (type == AI_Type.Engineer_TerretTower)
        {
            base.Idle_FrameInterval = 1;
            base.AttackDistance = (Fix64)5f;
            base.Attack_FrameInterval = attribute.Attack_FrameInterval;
        }

    }
    public override void BossTPLogic(int frame, GameObject obj , FixVector2 ToPos , bool rot)
    {
        switch(type)
        {
            case AI_Type.Boss_Rabit:
                {
                    Debug.Log("Rabit TP");
                    obj.GetComponent<MonsterModel_Component>().position = PackConverter.FixVector2ToFixVector3(ToPos);
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
    public override void BossAttackLogic(int frame, GameObject obj, FixVector2 TargetPosition)
    {
        switch(type)
        {
            case AI_Type.Nomral_Range:
                {
                    break;
                }
            case AI_Type.Normal_Melee:
                {
                    Debug.Log("attacked!!!!!!!!");
                    foreach(var x in sys._battle._player.playerToPlayer)
                    {
                        PlayerInGameData p = x.Value;
                        Fix64 dist = FixVector2.Distance(p.obj.GetComponent<PlayerModel_Component>().playerPosition,
                            new FixVector2(obj.GetComponent<MonsterModel_Component>().position.x, obj.GetComponent<MonsterModel_Component>().position.y)
                            );
                        if(dist<=(Fix64)0.5f)
                        {
                            sys._battle._player.BeAttacked(p.obj, 100, 1);
                        }
                        
                        
                    }


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

                    FixVector2 MonsPos = PackConverter.FixVector3ToFixVector2(obj.GetComponent<MonsterModel_Component>().position);
                    FixVector2 toward = ((TargetPosition - MonsPos)*(Fix64)100).GetNormalized();


                    TurnLogic(toward, obj.transform.Find("engineer_derivative_1_3"), BossConfig.SpinRate);

                    float degree = obj.transform.Find("engineer_derivative_1_3").transform.eulerAngles.z;
                    FixVector2 ShootPos = new FixVector2((Fix64)obj.transform.Find("engineer_derivative_1_3").Find("ShotBulletPosition").position.x, 
                                            (Fix64)obj.transform.Find("engineer_derivative_1_3").Find("ShotBulletPosition").position.y);

                    FixVector2 ShootToward = new FixVector2(Fix64.Cos(degree* Fix64.PI/ (Fix64)180f),
                                            Fix64.Sin(degree * Fix64.PI / (Fix64)180f));



                    bu.BulletInit("AliasAI", ShootPos, ShootToward,
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
    public override void BossRunLogic(int frame, GameObject Boss, FixVector2 TargetPosition , Fix64 GameCounter)
    {
        switch (type)
        {
            case AI_Type.Nomral_Range:
                break;
            case AI_Type.Normal_Melee:
                {

                    Vector3 MonsterPos = new Vector3((float)Boss.GetComponent<MonsterModel_Component>().position.x, (float)Boss.GetComponent<MonsterModel_Component>().position.y);
                    Boss.GetComponent<AIPath>().InitConfig(MonsterPos, Boss.GetComponent<MonsterModel_Component>().Rotation, new Vector3(1.5f, 1.5f, 1.5f), Global.FrameRate, (float)GameCounter);
                    //获取当前帧位置
                    Vector3 Pos;
                    Quaternion Rot;
                    Boss.GetComponent<AIPath>().GetFramePosAndRotation(out Pos, out Rot);

                    FixVector3 FixMonsterPos = new FixVector3((Fix64)Pos.x, (Fix64)Pos.y, (Fix64)Pos.z);
                    Boss.GetComponent<MonsterModel_Component>().position = FixMonsterPos;
                    Boss.GetComponent<MonsterModel_Component>().Rotation = Rot;


                    //Debug.Log("MonterPos: " + FixMonsterPos);
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
                    Boss.GetComponent<AIPath>().InitConfig(MonsterPos, Boss.GetComponent<MonsterModel_Component>().Rotation, new Vector3(1.5f, 1.5f, 1.5f), Global.FrameRate, (float)GameCounter);
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
                    //Vector2 MonsPos = new Vector2((float)Boss.GetComponent<MonsterModel_Component>().position.x, (float)Boss.GetComponent<MonsterModel_Component>().position.y);
                    FixVector2 MonsPos = PackConverter.FixVector3ToFixVector2(Boss.GetComponent<MonsterModel_Component>().position);
                    FixVector2 toward = ((TargetPosition - MonsPos)*(Fix64)100).GetNormalized();
                    TurnLogic(toward, Boss.transform.Find("engineer_derivative_1_3"), BossConfig.SpinRate);

                    break;
                }
        }

    }

    void TurnLogic(FixVector2 dir, Transform tran , float change)
    {
        Fix64 A =(Fix64) tran.eulerAngles.z;
        Fix64 B = Fix64.Atan2(dir.y, dir.x) * (Fix64)180 / Fix64.PI;
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
        tran.eulerAngles = new Vector3(0, 0,(float) A);
    }

}
