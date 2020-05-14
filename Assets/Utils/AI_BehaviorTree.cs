using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;

class AI_BehaviorTree : AI_BehaviorBase
{
    SystemManager sys;
    BossAttribute BossConfig;
    public AI_BehaviorTree(AI_Type AItype, int RoomID, BossAttribute attribute) : base()
    {
        sys = GameObject.Find("GameEntry").GetComponent<GameMain>().WorldSystem;
        base.type = AItype;
        base.RoomID = RoomID;
        BossConfig = attribute;
        if (type == AI_Type.Normal_Melee || type == AI_Type.Boss_Rabit_Egg || type == AI_Type.Boss_DarkKnightSword)
        {
            base.Idle_FrameInterval = 1000 / Global.FrameRate;
            base.Run_FrameInterval = 1;
            base.AttackDistance = (Fix64)0.6f;
            base.Attack_FrameInterval = 20;
        }
        else if (type == AI_Type.Boss_Rabit)
        {
            base.Idle_FrameInterval = 40;
            base.Run_FrameInterval = 60;
            base.Attack_FrameInterval = 200;
            base.DashDistance = (Fix64)7f;
            base.Teleport_FrameInterval = 1;
            base.DashToDistance = (Fix64)3f;
            base.SummoningInterval = 20;
        }
        else if (type == AI_Type.Engineer_TerretTower)
        {
            base.Idle_FrameInterval = 1;
            base.AttackDistance = (Fix64)5f;
            base.Attack_FrameInterval = attribute.Attack_FrameInterval;
        }

        else if (type == AI_Type.Nomral_Range)
        {
            base.AttackDistance = (Fix64)5f;
            base.Idle_FrameInterval = 1000 / Global.FrameRate;
            base.Run_FrameInterval = 1;
            base.Attack_FrameInterval = 20;
        }

        else if (type == AI_Type.Boss_Wizard)
        {
            base.Idle_FrameInterval = 1;
            base.Run_FrameInterval = 60;
            base.Attack_FrameInterval = 100;
            base.DashDistance = (Fix64)6f;
            base.Teleport_FrameInterval = 1;
            base.DashToDistance = (Fix64)3f;
            base.SummoningInterval = 20;
            base.Skill1_FrameInterval = 1;
            base.Skill1_Duration = 300;
            base.Skill1_Radius = (Fix64)2.3;
        }
        else if (type == AI_Type.Boss_DarkKnight)
        {
            base.Idle_FrameInterval = 1;
            base.Run_FrameInterval = 60;
            base.Attack_FrameInterval = 100;
            base.DashDistance = (Fix64)6f;
            base.Teleport_FrameInterval = 1;
            base.DashToDistance = (Fix64)3f;
            base.SummoningInterval = 20;
            base.Skill1_FrameInterval = 20;
            base.Skill2_FrameInterval = 20;
        }
    }
    public override void BossTPLogic(int frame, GameObject obj, FixVector2 ToPos, bool rot)
    {
        switch (type)
        {
            case AI_Type.Boss_Rabit:
                {
                    //Debug.Log("Rabit TP");
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
            case AI_Type.Boss_Wizard:
                {
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
            case AI_Type.Boss_DarkKnight:
                {
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
        switch (type)
        {
            case AI_Type.Nomral_Range:
                {
                    //Debug.Log("Range Attack");

                    List<bulletType> list = new List<bulletType>();
                    BulletUnion bu = new BulletUnion(sys._battle);

                    FixVector2 MonsPos = PackConverter.FixVector3ToFixVector2(obj.GetComponent<MonsterModel_Component>().position);
                    FixVector2 toward = ((TargetPosition - MonsPos) * (Fix64)100).GetNormalized();

                    //float degree = obj.transform.Find("engineer_derivative_1_3").transform.eulerAngles.z;
                    FixVector2 ShootPos = MonsPos + toward;

                    //FixVector2 ShootToward = new FixVector2(Fix64.Cos(degree * Fix64.PI / (Fix64)180f),
                    //Fix64.Sin(degree * Fix64.PI / (Fix64)180f));

                    bu.BulletInit("Boss_Rabit", ShootPos, toward,
                                                                  (Fix64)0.1, (Fix64)5, base.RoomID,
                                                                  Resources.Load("Effects/Prefab/effect_fireball_0") as GameObject
                                                                  , list);
                    sys._battle._monster.bulletList.Add(bu);

                    break;
                }
            case AI_Type.Normal_Melee:
                {
                    FixVector2 MonsPos =PackConverter.FixVector3ToFixVector2( obj.GetComponent<MonsterModel_Component>().position);
                    foreach (var item in sys._battle._player.playerToPlayer)
                    {
                        GameObject Player = item.Value.obj;
                        FixVector2 PlayerPos = Player.GetComponent<PlayerModel_Component>().GetPlayerPosition();


                        Fix64 MonsD2P = FixVector2.Distance(MonsPos, PlayerPos);
                        if (MonsD2P <= AttackDistance)
                        {
                            sys._battle._player.BeAttacked(Player, 5, item.Value.RoomID);
                        }
                    }
                    break;
                }
            case AI_Type.Boss_Rabit:
                {
                    //Debug.Log("Rabit AttackeING");
                    //int BulletTypeNumber = 4;
                    int Switch = Random.Range(1, 3);
                    if (Switch == 1)
                    {

                        int AttackRate = 10;
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
                                List<bulletType> list = new List<bulletType>();


                                float CurrentAngle = InitAngle + angle * j;
                                Vector2 toward = new Vector2(Mathf.Cos(CurrentAngle / 180f * Mathf.PI), Mathf.Sin(CurrentAngle / 180f * Mathf.PI));
                                toward = toward.normalized;

                                FakeBulletUnion bu = new FakeBulletUnion(obj, "Boss_Rabit", new FixVector2(obj.GetComponent<MonsterModel_Component>().position.x,
                                                                    obj.GetComponent<MonsterModel_Component>().position.y),
                                                                    new FixVector2((Fix64)toward.x,
                                                                    (Fix64)toward.y),
                                                                    (Fix64)0.1, (Fix64)3, sys._battle._monster.BossRoom,
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
                            Egg_Instance.GetComponent<EnemyAI>().InitAI(AI_Type.Boss_Rabit_Egg, RoomID, null);


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
                    //Debug.Log("蛋蛋攻击");
                    GameObject egg_explosion = Object.Instantiate(Resources.Load("Model/dddppp/Effects/Prefabs/egg_explosion", typeof(GameObject)), obj.transform.position, obj.transform.rotation) as GameObject;
                    sys._battle._skill.Effects.Add(new KeyValuePair<GameObject, int>(egg_explosion, 10));
                    //Debug.Log(sys._battle._monster.FindRoomIDByMonster(obj));



                    //伤害判定
                    FixVector2 MonsPos = PackConverter.FixVector3ToFixVector2(obj.GetComponent<MonsterModel_Component>().position);
                    foreach (var item in sys._battle._player.playerToPlayer)
                    {
                        GameObject Player = item.Value.obj;
                        FixVector2 PlayerPos = Player.GetComponent<PlayerModel_Component>().GetPlayerPosition();


                        Fix64 MonsD2P = FixVector2.Distance(MonsPos, PlayerPos);
                        if (MonsD2P <= AttackDistance)
                        {
                            sys._battle._player.BeAttacked(Player, 10, item.Value.RoomID);
                        }
                    }


                    sys._battle._skill.Add(
                        new SkillBase(1, 1,
                        new FixVector2(obj.transform.GetComponent<MonsterModel_Component>().position.x, obj.transform.GetComponent<MonsterModel_Component>().position.y),
                        Fix64.One,
                        0,
                        frame + 5,-1),
                        sys._battle._monster.FindRoomIDByMonster(obj));
                    obj.transform.GetComponent<MonsterModel_Component>().HP = Fix64.Zero;
                    Object.Destroy(obj);
                    break;
                }
            case AI_Type.Engineer_TerretTower:
                {
                    //Debug.Log("Terret Attack");
                    //Debug.Log("Frame: "+ frame);
                    if (sys._model._RoomListModule.roomType == RoomType.Pve)
                    {
                        List<bulletType> list = new List<bulletType>();
                        BulletUnion bu = new BulletUnion(sys._battle);

                        FixVector2 MonsPos = PackConverter.FixVector3ToFixVector2(obj.GetComponent<MonsterModel_Component>().position);
                        FixVector2 toward = ((TargetPosition - MonsPos) * (Fix64)100).GetNormalized();


                        TurnLogic(toward, obj.transform.Find("engineer_derivative_1_3"), BossConfig.SpinRate);

                        float degree = obj.transform.Find("engineer_derivative_1_3").transform.eulerAngles.z;
                        FixVector2 ShootPos = new FixVector2((Fix64)obj.transform.Find("engineer_derivative_1_3").Find("ShotBulletPosition").position.x,
                                                (Fix64)obj.transform.Find("engineer_derivative_1_3").Find("ShotBulletPosition").position.y);

                        FixVector2 ShootToward = new FixVector2(Fix64.Cos(degree * Fix64.PI / (Fix64)180f),
                                                Fix64.Sin(degree * Fix64.PI / (Fix64)180f));



                        bu.BulletInit("AliasAI", ShootPos, ShootToward,
                                                                      (Fix64)0.2, (Fix64)2, base.RoomID,
                                                                      Resources.Load("Model/Bullet/Prefab/bullet_87") as GameObject
                                                                      , list, obj.GetComponent<MonsterModel_Component>().OwnderUID);
                        sys._battle._monster.bulletList.Add(bu);
                    }
                    else
                    {
                        List<bulletType> list = new List<bulletType>();
                        PVPBulletUnion bu = new PVPBulletUnion(sys._pvpbattle);

                        FixVector2 MonsPos = PackConverter.FixVector3ToFixVector2(obj.GetComponent<MonsterModel_Component>().position);
                        FixVector2 toward = ((TargetPosition - MonsPos) * (Fix64)100).GetNormalized();


                        TurnLogic(toward, obj.transform.Find("engineer_derivative_1_3"), BossConfig.SpinRate);

                        float degree = obj.transform.Find("engineer_derivative_1_3").transform.eulerAngles.z;
                        FixVector2 ShootPos = new FixVector2((Fix64)obj.transform.Find("engineer_derivative_1_3").Find("ShotBulletPosition").position.x,
                                                (Fix64)obj.transform.Find("engineer_derivative_1_3").Find("ShotBulletPosition").position.y);

                        FixVector2 ShootToward = new FixVector2(Fix64.Cos(degree * Fix64.PI / (Fix64)180f),
                                                Fix64.Sin(degree * Fix64.PI / (Fix64)180f));



                        bu.BulletInit(sys._pvpbattle._pvpplayer.FindPlayerTeamByUID(obj.GetComponent<MonsterModel_Component>().OwnderUID), ShootPos, ShootToward,
                                                                      (Fix64)0.2, (Fix64)2, base.RoomID,
                                                                      Resources.Load("Model/Bullet/Prefab/bullet_87") as GameObject
                                                                      , list, obj.GetComponent<MonsterModel_Component>().OwnderUID);
                        sys._pvpbattle._summon.bulletList.Add(bu);
                    }
                    break;
                }

            case AI_Type.Boss_Wizard:
                {

                    //Debug.Log("Wizard Attack:! ");

                    int SkillChosen = Random.Range(0, 2);

                    switch (SkillChosen)
                    {
                        case 0:
                            {
                                int AttackRate = 10;
                                int AttackNumber = base.Attack_FrameInterval / AttackRate;

                                int InitAngle = 0;
                                //BulletUnion bu = new BulletUnion(sys._battle);
                                for (int i = 0; i < AttackNumber; i++)
                                {
                                    int AttackInitFrame = frame + i * AttackRate;
                                    //Debug.Log("AttackFrame: " + AttackInitFrame);


                                    int BulletNumber = 4;
                                    float angle = 360 / BulletNumber;

                                    List<FakeBulletUnion> bulletList = new List<FakeBulletUnion>();

                                    for (int j = 0; j < BulletNumber; j++)
                                    {
                                        List<bulletType> list = new List<bulletType>();


                                        float CurrentAngle = InitAngle + angle * j;
                                        Vector2 toward = new Vector2(Mathf.Cos(CurrentAngle / 180f * Mathf.PI), Mathf.Sin(CurrentAngle / 180f * Mathf.PI));
                                        toward = toward.normalized;

                                        FakeBulletUnion bu = new FakeBulletUnion(obj, "Boss_Rabit", new FixVector2(obj.GetComponent<MonsterModel_Component>().position.x,
                                                                            obj.GetComponent<MonsterModel_Component>().position.y),
                                                                            new FixVector2((Fix64)toward.x,
                                                                            (Fix64)toward.y),
                                                                            (Fix64)0.1, (Fix64)5, sys._battle._monster.BossRoom,
                                                                            Resources.Load("Model/Bullet/Prefab/bullet_90") as GameObject
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

                                break;
                            }
                        case 1:
                            {
                                int AttackRate = 20;
                                int AttackNumber = base.Attack_FrameInterval / AttackRate;

                                int InitAngle = 0;
                                //BulletUnion bu = new BulletUnion(sys._battle);
                                for (int i = 0; i < AttackNumber; i++)
                                {
                                    int AttackInitFrame = frame + i * AttackRate;
                                    //Debug.Log("AttackFrame: " + AttackInitFrame);


                                    int BulletNumber = 12;
                                    float angle = 360 / BulletNumber;

                                    List<FakeBulletUnion> bulletList = new List<FakeBulletUnion>();

                                    for (int j = 0; j < BulletNumber; j++)
                                    {
                                        List<bulletType> list = new List<bulletType>();


                                        float CurrentAngle = InitAngle + angle * j;
                                        Vector2 toward = new Vector2(Mathf.Cos(CurrentAngle / 180f * Mathf.PI), Mathf.Sin(CurrentAngle / 180f * Mathf.PI));
                                        toward = toward.normalized;

                                        FakeBulletUnion bu = new FakeBulletUnion(obj, "Boss_Rabit", new FixVector2(obj.GetComponent<MonsterModel_Component>().position.x,
                                                                            obj.GetComponent<MonsterModel_Component>().position.y),
                                                                            new FixVector2((Fix64)toward.x,
                                                                            (Fix64)toward.y),
                                                                            (Fix64)0.1, (Fix64)5, sys._battle._monster.BossRoom,
                                                                            Resources.Load("Effects/Prefab/effect_poisonball_0") as GameObject
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

                                break;
                            }

                    }
                    //边攻击边移动
                    if (!sys._battle._monster.BossMove.ContainsKey(obj))
                    {

                        sys._battle._monster.BossMove.Add(obj, Attack_FrameInterval);
                    }
                    break;
                }

            case AI_Type.Boss_DarkKnight:
                {
                    //to do
                    int SkillChosen = Random.Range(0, 3);

                    switch (SkillChosen)
                    {
                        case 0:
                            {
                                //边攻击边移动
                                if (!sys._battle._monster.BossMove.ContainsKey(obj))
                                {

                                    sys._battle._monster.BossMove.Add(obj, Attack_FrameInterval);
                                }

                                int AttackRate = 25;
                                int AttackNumber = base.Attack_FrameInterval / AttackRate;

                                int InitAngle = 45;
                                //BulletUnion bu = new BulletUnion(sys._battle);
                                for (int i = 0; i < AttackNumber; i++)
                                {
                                    int AttackInitFrame = frame + i * AttackRate;
                                    //Debug.Log("AttackFrame: " + AttackInitFrame);


                                    int BulletNumber = 4;
                                    float angle = 360 / BulletNumber;

                                    List<FakeBulletUnion> bulletList = new List<FakeBulletUnion>();

                                    for (int j = 0; j < BulletNumber; j++)
                                    {
                                        List<bulletType> list = new List<bulletType>();


                                        float CurrentAngle = InitAngle + angle * j;
                                        Vector2 toward = new Vector2(Mathf.Cos(CurrentAngle / 180f * Mathf.PI), Mathf.Sin(CurrentAngle / 180f * Mathf.PI));
                                        toward = toward.normalized;

                                        FakeBulletUnion bu = new FakeBulletUnion(obj, "Boss_Rabit", new FixVector2(obj.GetComponent<MonsterModel_Component>().position.x,
                                                                            obj.GetComponent<MonsterModel_Component>().position.y),
                                                                            new FixVector2((Fix64)toward.x,
                                                                            (Fix64)toward.y),
                                                                            (Fix64)0.1, (Fix64)20, sys._battle._monster.BossRoom,
                                                                            Resources.Load("Model/Bullet/Prefab/bullet_97") as GameObject
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

                                break;
                            }
                        case 1:
                            {
                                //边攻击边移动
                                if (!sys._battle._monster.BossMove.ContainsKey(obj))
                                {

                                    sys._battle._monster.BossMove.Add(obj, Attack_FrameInterval);
                                }

                                int AttackRate = 20;
                                int AttackNumber = base.Attack_FrameInterval / AttackRate;

                                int InitAngle = 0;
                                //BulletUnion bu = new BulletUnion(sys._battle);
                                for (int i = 0; i < AttackNumber; i++)
                                {
                                    int AttackInitFrame = frame + i * AttackRate;
                                    //Debug.Log("AttackFrame: " + AttackInitFrame);


                                    int BulletNumber = 12;
                                    float angle = 360 / BulletNumber;

                                    List<FakeBulletUnion> bulletList = new List<FakeBulletUnion>();

                                    for (int j = 0; j < BulletNumber; j++)
                                    {
                                        List<bulletType> list = new List<bulletType>();
                                        float CurrentAngle = InitAngle + angle * j;
                                        Vector2 toward = new Vector2(Mathf.Cos(CurrentAngle / 180f * Mathf.PI), Mathf.Sin(CurrentAngle / 180f * Mathf.PI));
                                        toward = toward.normalized;

                                        FakeBulletUnion bu = new FakeBulletUnion(obj, "Boss_Rabit", new FixVector2(obj.GetComponent<MonsterModel_Component>().position.x,
                                                                            obj.GetComponent<MonsterModel_Component>().position.y),
                                                                            new FixVector2((Fix64)toward.x,
                                                                            (Fix64)toward.y),
                                                                            (Fix64)0.1, (Fix64)5, sys._battle._monster.BossRoom,
                                                                            Resources.Load("Model/Bullet/Prefab/bullet_92") as GameObject
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

                                break;
                            }

                        case 2:
                            {


                                int AttackRate = 5;
                                int AttackNumber = base.Attack_FrameInterval / AttackRate;

                                int InitAngle = Random.Range(1, 30);
                                //BulletUnion bu = new BulletUnion(sys._battle);
                                for (int i = 0; i < AttackNumber; i++)
                                {
                                    int AttackInitFrame = frame + i * AttackRate;
                                    //Debug.Log("AttackFrame: " + AttackInitFrame);

                                    InitAngle -= 5;
                                    int BulletNumber = 12;
                                    float angle = 360 / BulletNumber;

                                    List<FakeBulletUnion> bulletList = new List<FakeBulletUnion>();

                                    for (int j = 0; j < BulletNumber; j++)
                                    {
                                        List<bulletType> list = new List<bulletType>();


                                        float CurrentAngle = InitAngle + angle * j;
                                        Vector2 toward = new Vector2(Mathf.Cos(CurrentAngle / 180f * Mathf.PI), Mathf.Sin(CurrentAngle / 180f * Mathf.PI));
                                        toward = toward.normalized;

                                        FakeBulletUnion bu = new FakeBulletUnion(obj, "Boss_Rabit", new FixVector2(obj.GetComponent<MonsterModel_Component>().position.x,
                                                                            obj.GetComponent<MonsterModel_Component>().position.y),
                                                                            new FixVector2((Fix64)toward.x,
                                                                            (Fix64)toward.y),
                                                                            (Fix64)0.1, (Fix64)5, sys._battle._monster.BossRoom,
                                                                            Resources.Load("Model/Bullet/Prefab/bullet_89") as GameObject
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
                                break;
                            }

                    }
                    ////边攻击边移动
                    //if (!sys._battle._monster.BossMove.ContainsKey(obj))
                    //{

                    //    sys._battle._monster.BossMove.Add(obj, Attack_FrameInterval);
                    //}
                    break;

                  
                }

            case AI_Type.Boss_DarkKnightSword:
                {
                    Debug.Log("剑剑攻击");

                    FixVector2 MonsPos = PackConverter.FixVector3ToFixVector2(obj.GetComponent<MonsterModel_Component>().position);
                    foreach (var item in sys._battle._player.playerToPlayer)
                    {
                        GameObject Player = item.Value.obj;
                        FixVector2 PlayerPos = Player.GetComponent<PlayerModel_Component>().GetPlayerPosition();


                        Fix64 MonsD2P = FixVector2.Distance(MonsPos, PlayerPos);
                        if (MonsD2P <= AttackDistance)
                        {
                            sys._battle._player.BeAttacked(Player, 5, item.Value.RoomID);
                        }
                    }

                    break;
                }
            default:
                break;
        }
    }
    public override void BossSkill1(int frame, GameObject obj, FixVector2 TargetPosition)
    {
        switch (type)
        {
            case AI_Type.Boss_Wizard:
                {
                    GameObject PosionCircle_Prefab = (GameObject)Resources.Load("Effects/Prefab/PoisonCircle");

                    FixVector2 FixVecMonsterPos = PackConverter.FixVector3ToFixVector2(obj.GetComponent<MonsterModel_Component>().position);
                    Vector3 MonsterPos = PackConverter.FixVector3ToVector3(obj.GetComponent<MonsterModel_Component>().position);
                    GameObject PosionCircle_Instance = Object.Instantiate(PosionCircle_Prefab, MonsterPos, Quaternion.identity);

                    //初始化BOSS技能
                    PosionCircle_Instance.GetComponent<Skill_Component>().RemainingFrame = Skill1_Duration;
                    PosionCircle_Instance.GetComponent<Skill_Component>().Position = FixVecMonsterPos;
                    PosionCircle_Instance.GetComponent<Skill_Component>().SkillType = SkillType.BossPoison;

                    PosionCircle_Instance.GetComponent<Skill_Component>().Radius = Skill1_Radius;
                    sys._battle._monster.BossSkill.Add(PosionCircle_Instance);
                    //Monster

                    break;
                }
            case AI_Type.Boss_DarkKnight:
                {
                    //GameObject Shield_Prefab = (GameObject)Resources.Load("Model/Bullet/Prefab/boss01_shield");
                    obj.GetComponent<MonsterModel_Component>().buff.Undefeadted = true;
                    obj.GetComponent<MonsterModel_Component>().buff.Undefeadted_RemainingFrame = 100;



                    //sys._message.PopText("Boss无敌时间", 2fs);
                    break;
                }

        }

    }


    public override void BossSkill2(int frame, GameObject obj, FixVector2 TargetPosition)
    {
        switch (type)
        {
            case AI_Type.Boss_DarkKnight:
                {
                    int SwordNumber = 4;
                    Vector3 pos = new Vector3((float)obj.GetComponent<MonsterModel_Component>().position.x, (float)obj.GetComponent<MonsterModel_Component>().position.y, 0f);
                    GameObject sword_Prefab = (GameObject)Resources.Load("Model/Bullet/Prefab/boss01_sword");
                    List<FixVector3> PosList = new List<FixVector3> { new FixVector3((Fix64)pos.x - 0.5, (Fix64)pos.y - 0.5, (Fix64)0),
                            new FixVector3((Fix64)pos.x - 0.5, (Fix64)pos.y + 0.5, (Fix64)0),
                            new FixVector3((Fix64)pos.x + 0.5, (Fix64)pos.y - 0.5, (Fix64)0),
                            new FixVector3((Fix64)pos.x + 0.5, (Fix64)pos.y + 0.5, (Fix64)0) };
                    for (int i = 0; i < SwordNumber; i++)
                    {
                        int RoomID = sys._battle._monster.BossRoom;
                        GameObject Sword_Instance = Object.Instantiate(sword_Prefab, pos, Quaternion.identity);
                        Sword_Instance.GetComponent<MonsterModel_Component>().position = PosList[i];
                        Sword_Instance.GetComponent<MonsterModel_Component>().HP = (Fix64)2;
                        Sword_Instance.GetComponent<EnemyAI>().InitAI(AI_Type.Boss_DarkKnightSword, RoomID, null);
                        if (sys._battle._monster.RoomToMonster.ContainsKey(RoomID))
                        {
                            sys._battle._monster.RoomToMonster[RoomID].Add(Sword_Instance);
                        }
                    }
                    //sys._message.PopText("Boss释放剑雨，注意躲避", 2f);

                    break;

                   
                }

        }
    }

    public override void BossRunLogic(int frame, GameObject Boss, FixVector2 TargetPosition)
    {
        switch (type)
        {
            case AI_Type.Nomral_Range:
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
                    //Vector2 MonsPos = new Vector2((float)Boss.GetComponent<MonsterModel_Component>().position.x, (float)Boss.GetComponent<MonsterModel_Component>().position.y);
                    FixVector2 MonsPos = PackConverter.FixVector3ToFixVector2(Boss.GetComponent<MonsterModel_Component>().position);
                    FixVector2 toward = ((TargetPosition - MonsPos) * (Fix64)100).GetNormalized();
                    TurnLogic(toward, Boss.transform.Find("engineer_derivative_1_3"), BossConfig.SpinRate);

                    break;
                }

            case AI_Type.Boss_Wizard:
                {
                    if (!sys._battle._monster.BossMove.ContainsKey(Boss))
                    {

                        sys._battle._monster.BossMove.Add(Boss, Run_FrameInterval);
                    }
                    break;
                }
            case AI_Type.Boss_DarkKnight:
                {
                    if (!sys._battle._monster.BossMove.ContainsKey(Boss))
                    {

                        sys._battle._monster.BossMove.Add(Boss, Run_FrameInterval);
                    }
                    break;
                }
            case AI_Type.Boss_DarkKnightSword:
                {
                    Vector3 MonsterPos = new Vector3((float)Boss.GetComponent<MonsterModel_Component>().position.x, (float)Boss.GetComponent<MonsterModel_Component>().position.y);
                    Boss.GetComponent<AIPath>().InitConfig(MonsterPos, Boss.GetComponent<MonsterModel_Component>().Rotation, new Vector3(1.5f, 1.5f, 1.5f), Global.FrameRate);
                    //获取当前帧位置
                    Vector3 Pos;
                    Quaternion Rot;
                    Boss.GetComponent<AIPath>().GetFramePosAndRotation(out Pos, out Rot);
                    FixVector3 FixMonsterPos = new FixVector3((Fix64)Pos.x, (Fix64)Pos.y, (Fix64)Pos.z);
                    Boss.GetComponent<MonsterModel_Component>().position = FixMonsterPos;
                    //Boss.GetComponent<MonsterModel_Component>().Rotation = Rot;

                    Vector2 MonsPos = PackConverter.FixVector3ToVector3(Boss.GetComponent<MonsterModel_Component>().position);
                    Vector2 toward =(PackConverter.FixVector2ToVector2( TargetPosition) - MonsPos).normalized;
                 
                    Vector3 Euler = new Vector3(0,0,(float) (Mathf.Atan2(toward.y, toward.x) / Mathf.PI* 180) );
                  
                    Boss.GetComponent<MonsterModel_Component>().Rotation= Quaternion.Euler(Euler);

                    break;
                }
        }

    }

    void TurnLogic(FixVector2 dir, Transform tran, float change)
    {
        Fix64 A = (Fix64)tran.eulerAngles.z;
        Fix64 B = Fix64.Atan2(dir.y, dir.x) * (Fix64)180 / Fix64.PI;
        while (A < 0) A += 360;
        while (B < 0) B += 360;
        if (A > B)
        {
            if (A - B < 360 - (A - B)) A -= change;
            else A += change;
        }
        else
        {
            if ((B - A) < 360 - (B - A)) A += change;
            else A -= change;
        }
        while (A > 360) A -= 360;
        while (A < 0) A += 360;
        if (A > 90 && A < 270) tran.GetComponent<SpriteRenderer>().flipY = true;
        else tran.GetComponent<SpriteRenderer>().flipY = false;
        tran.eulerAngles = new Vector3(0, 0, (float)A);
    }

}
