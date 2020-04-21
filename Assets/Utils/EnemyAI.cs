using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EnemyAI : MonoBehaviour
{
    AI_Enemy AI_Controller;
    public AI_Type AItype;
    bool Inited = false;

    public void InitAI(AI_Type type)
    {
        AItype = type;
        AI_Controller = new AI_Enemy(AItype);

    }
    public void InitMonster(int frame)
    {
        if (Inited == false)
        {
            AI_Controller.Start(frame);
            Inited = true;
        }

    }
    public void UpdateLogic(GameObject target,int frame, GameObject MonsterObj)
    {
        Vector2 MonsterPos = new Vector2();
        Vector2 tar= new Vector2();
        if (target != null)
        {
            MonsterPos = new Vector2((float)GetComponent<MonsterModel_Component>().position.x, (float)GetComponent<MonsterModel_Component>().position.y);
          
            tar = new  Vector2((float)target.GetComponent<PlayerModel_Component>().GetPlayerPosition().x, (float)target.GetComponent<PlayerModel_Component>().GetPlayerPosition().y);
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

    public AI_Enemy(AI_Type AItype) : base()
    {
        sys = GameObject.Find("GameEntry").GetComponent<GameMain>().WorldSystem;
        base.type = AItype;
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
    public override void BossAttackLogic(int frame, GameObject obj)
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
                        for (int i = 0; i < EggNumber; i++)
                        {
                            GameObject Egg_Instance = Object.Instantiate(Egg_Prefab, pos, Quaternion.identity);
                            Egg_Instance.GetComponent<MonsterModel_Component>().position = obj.GetComponent<MonsterModel_Component>().position;
                            Egg_Instance.GetComponent<MonsterModel_Component>().HP = (Fix64)2;
                            Egg_Instance.GetComponent<EnemyAI>().InitAI(AI_Type.Boss_Rabit_Egg);

                            int RoomID = sys._battle._monster.BossRoom;
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
            default:
                break;
        }
    }
    public override void BossRunLogic(int frame, GameObject Boss)
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
        }

    }
}
