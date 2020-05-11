using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class EngineerBase
{
    public string name;             //名字
    public GameObject obj;          //人物预制体
    public int HP;                  //血量
    public float moveSpeed;         //移动速度
    public float damge;             //伤害
    public float bulletSpeed;       //子弹速度
    public int fireSpeed;           //射速
    public List<bulletType> bulletEffect;  //子弹附加效果
    public GameObject bulletObj;    //子弹预制体

    public float rangeSkill1;                    //炮台技能的释放范围
    public float areaSkill1;                    //炮台大小
    public float lastTimeSkill1;                //炮台持续帧数
    public float countdownSkill1;               //技能1冷却
    public int HPSkill1;                        //炮台血量
    public int damageSkill1;                    //炮台单发伤害
    public float bulletSpeedSkill1;             //炮台子弹速度
    public float fireSpeedSkill1;               //炮台射速
    public float controlTimeSkill1;             //炮台打中的敌人晕眩时间
    public List<bulletType> bulletEffectSkill1;        //炮台子弹附加效果
    public GameObject bulletObjSkill1;          //子弹预制体

    public float rangeSkill2;                   //手雷范围
    public float areaSkill2;                    //手雷大小
    public float throwTime;                     //手雷投掷时间,影响产生伤害判定时间
    public int damgeSkill2;                     //手雷伤害
    public float controlTimeSkill2;             //控制时间
    public float countdownSkill2;               //技能2冷却

    public GameObject effectArtillery;          //炮台预制体 
    public GameObject effectGernade;            //手雷预制体
    public GameObject effectGernadeExplosion;   //手雷爆炸特效

    public GameObject effectRocket;
    public float radiusSkill3;                  //火箭爆炸半径
    public float speedSkill3;                   //火箭飞行速度
    public int damageSkill3;                     //火箭伤害
    public float rangeSkill3;                   //指示器范围
    public float areaSkill3;
    public float countdownSkill3;

    public Sprite skill1Image;
    public Sprite skill2Image;
    public Sprite skill3Image;

    public SkillAreaType skill1Type;
    public SkillAreaType skill2Type;
    public SkillAreaType skill3Type;

    List <GameObject> grenade=new List<GameObject>();
    List <GameObject> explosion = new List<GameObject>();
    List<GameObject> Rocket = new List<GameObject>();

    SystemManager _sys;

    public void Free()
    {
        grenade.Clear();
        explosion.Clear();

        Rocket.Clear();
    }
        public EngineerBase(SystemManager sys)
    {
        EngineerConfig x =Resources.Load("Configs/Heros/EngineerConfig") as EngineerConfig;

        name = x.name;
        obj = x.obj;
        HP = x.HP;
        moveSpeed = x.moveSpeed;
        damge = x.damge;
        bulletSpeed = x.bulletSpeed;
        fireSpeed = x.fireSpeed;
        bulletEffect = x.bulletEffect;
        bulletObj = x.bulletObj;

        rangeSkill1 =x.rangSkill1;
        areaSkill1 = x.areaSkill1;
        lastTimeSkill1 = x.lastTimeSkill1;
        countdownSkill1 = x.countdownSkill1;
        HPSkill1 = x.HPSkill1;
        damageSkill1 = x.damageSkill1;
        bulletSpeedSkill1 = x.bulletSpeedSkill1;
        fireSpeedSkill1 = x.fireSpeedSkill1;
        controlTimeSkill1 = x.controlTimeSkill1;
        bulletEffectSkill1 = x.bulletEffectSkill1;
        bulletObjSkill1 = x.bulletObjSkill1;

        rangeSkill2 = x.rangeSkill2;
        areaSkill2 = x.areaSkill2;
        throwTime = x.throwTime;
        damgeSkill2 = x.damgeSkill2;
        controlTimeSkill2 = x.controlTimeSkill2;
        countdownSkill2 = x.countdownSkill2;

        effectArtillery = x.effectArtillery;
        effectGernade = x.effectGernade;
        effectGernadeExplosion = x.effectGernadeExplosion;


        effectRocket = x.effectRocket;
        radiusSkill3 = x.radiusSkill3;
        speedSkill3 = x.speedSkill3;
        damageSkill3 = x.damageSkill3;
        rangeSkill3 = x.rangeSkill3;
        areaSkill3 = x.areaSkill3;
        countdownSkill3 = x.countdownSkill3;

        skill1Image = x.skill1Image;
        skill2Image = x.skill2Image;
        skill3Image = x.skill3Image;


        _sys = sys;

        skill1Type = x.skill1Type;
        skill2Type = x.skill2Type;
        skill3Type = x.skill3Type;



    }
    public float Skill1Range()
    {
        //需要传入获得的player的天赋表
        return rangeSkill1;
    }

    public float Skill1Area()
    {
        return areaSkill1;
    }

    public float Skill2Range()
    {
        //需要传入获得的player的天赋表
        return rangeSkill2;
    }

    public float Skill2Area()
    {
        return areaSkill2;
    }

    public float Skill3Area()
    {
        return areaSkill3;
    }
    public float Skill3Range()
    {
        return rangeSkill3;
    }

    public int Skill1Logic(int uid ,int frame, int RoomID, List<int> gifted,Vector3 st,Vector3 ed, int dmgSrc)//返回值就是cd

    {
        /*
            FixVector2 PlayerPos = battle._player.playerToPlayer[UID].obj.GetComponent<PlayerModel_Component>().GetPlayerPosition();

            FixVector3 PlayerPos3 = new FixVector3(PlayerPos.x, PlayerPos.y, (Fix64)0);

            Vector3 PlayerPos2 = new Vector3((float)PlayerPos.x, (float)PlayerPos.y);
            //Debug.Log(PlayerPos2);
            */
        GameObject TerretInstance = GameObject.Instantiate(effectArtillery, ed, Quaternion.identity);
        TerretInstance.GetComponent<MonsterModel_Component>().position = new FixVector3(
            (Fix64)ed.x,
            (Fix64)ed.y, (Fix64)ed.z
            );

        AliasMonsterPack temp = new AliasMonsterPack();
        BossAttribute attribute = new BossAttribute();
        attribute.Attack_FrameInterval = 5;
        attribute.SpinRate = 3;
        TerretInstance.GetComponent<EnemyAI>().InitAI(AI_Type.Engineer_TerretTower, RoomID, attribute);
        TerretInstance.GetComponent<MonsterModel_Component>().HP = (Fix64)10;
        TerretInstance.GetComponent<MonsterModel_Component>().OwnderUID = dmgSrc;
        temp.obj = TerretInstance;
        temp.RemainingFrame = 200;
        switch (_sys._model._RoomListModule.roomType)
        {
            case RoomType.Pve:
                if (!_sys._battle._monster.RoomToAliasUnit.ContainsKey(RoomID))
                {
                    List<AliasMonsterPack> ListAlias = new List<AliasMonsterPack>();
                    ListAlias.Add(temp);
                        _sys._battle._monster.RoomToAliasUnit.Add(RoomID, ListAlias);
                }
                else
                {
                        _sys._battle._monster.RoomToAliasUnit[RoomID].Add(temp);

                }
                break;
            case RoomType.Pvp:
                _sys._pvpbattle._summon.AddAliasUnit(uid,RoomID, temp);
                break;
        }
        return (int)(countdownSkill1*1000/Global.FrameRate);
    }

    public int Skill2Logic(int frame,int RoomID, List<int> gifted, Vector3 st, Vector3 ed , int dmgSrc)
    {

        int damageFrame = frame;

        //手雷投出
        GameObject p = GameObject.Instantiate(effectGernade);
        if(gifted[0]==1)//天赋点了第一个减投掷时间
        {
            damageFrame += (int)(0.6f * 1000 / Global.FrameRate * throwTime);
            p.GetComponent<GrenadeTrail>().init(frame,(int)(0.6f*1000/Global.FrameRate* throwTime), st, ed);//这里修改天赋快了多少
        }
        else
        {
            damageFrame += (int)(1000 / Global.FrameRate * throwTime);
            p.GetComponent<GrenadeTrail>().init(frame, (int)(1000/Global.FrameRate* throwTime), st, ed);
        }
        grenade.Add(p);
        //爆炸特效
        GameObject ge = GameObject.Instantiate(effectGernadeExplosion);

        if(gifted[0]==1)
        {
            ge.GetComponent<ExplosionControl>().init(frame+(int)(0.6f * 1000 / Global.FrameRate * throwTime),
                (int) (0.7f*1000 / Global.FrameRate), ed);
        }
        else
        {
            ge.GetComponent<ExplosionControl>().init(frame+(int)(1000 / Global.FrameRate * throwTime), 
                (int)(0.7f * 1000 / Global.FrameRate), ed);
        }
        explosion.Add(ge);
        //伤害判定产生

        int tda = damgeSkill2;
        float tc = controlTimeSkill2;
        float tr = areaSkill2;
        if(gifted[4]==1)
        {
            tda *= 2;
        }
        
        if(gifted[2]==1)
        {
            tc *= 2;
        }

        if(gifted[1]==1)
        {
            tr*=1.5f;
        }

        SkillBase tmp = new SkillBase(0, tda, new FixVector2((Fix64)ed.x, (Fix64)ed.y), (Fix64)tr, (int)(tc * 1000 / Global.FrameRate),


            damageFrame, dmgSrc);

        //_parentManager._skill.Add(tmp, RoomID);
        switch (_sys._model._RoomListModule.roomType)
        {
            case RoomType.Pve:
                _sys._battle._skill.Add(tmp, RoomID);
                break;
            case RoomType.Pvp:
                _sys._pvpbattle._pvpskill.Add(tmp, RoomID);
                break;
        }

        if (gifted[3] == 1)
        {
            return (int)(1000 / Global.FrameRate * (countdownSkill2 - 2));
        }
        else
        {
            return (int)(1000 / Global.FrameRate * (countdownSkill2));
        }
    }


    public int Skill3Logic(int frame, int RoomID, List<int> gifted, Vector3 st, Vector3 dir, int dmgSrc)
    {
        dir -= st;
        dir.Normalize();
        dir*=0.1f;
        //火箭投出
        GameObject p = GameObject.Instantiate(effectRocket);

        p.GetComponent<RocketControl>().init(new FixVector2((Fix64)st.x, (Fix64)st.y),
            new FixVector2((Fix64)dir.x, (Fix64)dir.y),
            (Fix64)radiusSkill3,RoomID,damageSkill3,dmgSrc,_sys._battle
            );
        Rocket.Add(p);

        if (gifted[3] == 1)
        {
            return (int)(1000 / Global.FrameRate * (countdownSkill3 - 2));
        }
        else
        {
            return (int)(1000 / Global.FrameRate * (countdownSkill3));
        }
    }

    public void updateLogic(int frame)
    {
        //手雷部分
        List<GameObject> tg = new List<GameObject>();
        foreach(GameObject p in grenade)
        {
            if(p.GetComponent<GrenadeTrail>().endFrame==frame)
            {
                GameObject.Destroy(p);
            }
            else
            {
                p.GetComponent<GrenadeTrail>().updateLogic(frame);
                tg.Add(p);
            }
        }
        grenade.Clear();
        foreach(GameObject p in tg)
        {
            grenade.Add(p);
        }
        //手雷爆炸部分
        tg.Clear();
        foreach(GameObject p in explosion)
        {
            if (p.GetComponent<ExplosionControl>().getEndFrame() == frame)
            {
                GameObject.Destroy(p);
            }
            else
            {
                p.GetComponent<ExplosionControl>().updateLogic(frame);
                tg.Add(p);
            }
        }
        explosion.Clear();
        foreach (GameObject p in tg)
        {
            explosion.Add(p);
        }

        tg.Clear();
        foreach(GameObject p in Rocket)
        {
            if(p.GetComponent<RocketControl>().updateLogic(frame))
            {
                GameObject.Destroy(p);
            }
            else
            {
                tg.Add(p);
            }
        }
        Rocket.Clear();
        foreach(var p in tg)
        {
            Rocket.Add(p);
        }
        tg.Clear();
    }
}