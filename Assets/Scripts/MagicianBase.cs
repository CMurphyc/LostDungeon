using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagicianBase
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

    public float rangeSkill1;                  //火魔法范围
    public float areaSkill1;                  //火魔法大小
    public float lastTimeSkill1;              //火魔法持续时间
    public int damageSkill1;                 //火魔法单次伤害
    public float intervalSkill1;              //伤害间隔
    public float countdownSkill1;             //技能1冷却

    public float rangeSkill2;                 //雷魔法范围
    public float areaSkill2;                  //雷魔法大小
    public int damageSkill2;                 //雷魔法伤害
    public float controlTime;                 //控制时间
    public float countdownSkill2;             //技能2冷却

    public float rangeSkill3;                 //冰魔法范围
    public float areaSkill3;                  //冰魔法大小
    public int damageSkill3;                 //冰魔法伤害
    public float lastTimeSkill3;                 //持续时间
    public float countdownSkill3;

    public GameObject effectFire;           //火特效
    public GameObject effectThunder;        //雷特效
    public GameObject effectIce;

    public Sprite skill1Image;
    public Sprite skill2Image;
    public Sprite skill3Image;

    public SkillAreaType skill1Type;
    public SkillAreaType skill2Type;
    public SkillAreaType skill3Type;

    List<GameObject> fire = new List<GameObject>();
    List<GameObject> thunder = new List<GameObject>();
    List<GameObject> ice = new List<GameObject>();

    BattleManager _parentManager;

    public float Skill1Range()
    {
        return rangeSkill1;
    }

    public float Skill1Area()
    {
        return areaSkill1;
    }

    public float Skill2Range()
    {
        return rangeSkill2;
    }
    public float Skill3Area()
    {
        return areaSkill3;
    }

    public float Skill3Range()
    {
        return rangeSkill3;
    }

    public float Skill2Area()
    {
        return areaSkill2;
    }

    public void Free()
    {
        fire.Clear();
        thunder.Clear();
    }
    public MagicianBase(BattleManager parentManager)
    {
        MagicianConfig x = Resources.Load("Configs/Heros/MagicianConfig") as MagicianConfig;

        name = x.name;
        obj = x.obj;
        HP = x.HP;
        moveSpeed = x.moveSpeed;
        damge = x.damge;
        bulletSpeed = x.bulletSpeed;
        fireSpeed = x.fireSpeed;
        bulletEffect = x.bulletEffect;
        bulletObj = x.bulletObj;


        rangeSkill1 = x.rangeSkill1;
        areaSkill1 = x.areaSkill1;
        lastTimeSkill1 = x.lastTimeSkill1;
        damageSkill1 = x.damageSkill1;
        intervalSkill1 = x.intervalSkill1;
        countdownSkill1 = x.countdownSkill1;

        rangeSkill2 = x.rangeSkill2;
        areaSkill2 = x.areaSkill2;
        damageSkill2 = x.damageSkill2;
        controlTime = x.controlTime;
        countdownSkill2 = x.countdownSkill2;

        rangeSkill3 = x.rangeSkill3;                 //冰魔法范围
        areaSkill3 = x.areaSkill3;                  //冰魔法大小
        damageSkill3 = x.damageSkill3;                 //冰魔法伤害
        lastTimeSkill3 = x.lastTimeSkill3;                 //持续时间
        countdownSkill3 = x.countdownSkill3;

        effectFire= x.effectFire;
        effectThunder = x.effectThunder;
        effectIce = x.effectIce;

        skill1Image = x.skill1Image;
        skill2Image = x.skill2Image;
        skill3Image = x.skill3Image;

        skill1Type = x.skill1Type;
        skill2Type = x.skill2Type;
        skill3Type = x.skill3Type;

        _parentManager = parentManager;
    }

    public int Skill1Logic(int frame, int RoomID, List<int> gifted, Vector3 pos, int dmgSrc)//返回值就是cd
    {
        //火焰特效
        GameObject ge = GameObject.Instantiate(effectFire);

        if (gifted[0] == 1)
        {
            ge.GetComponent<ExplosionControl>().init(frame ,
                (int)(lastTimeSkill1 * 1000 / Global.FrameRate), pos);
        }
        else
        {
            ge.GetComponent<ExplosionControl>().init(frame,
                (int)(lastTimeSkill1 * 1000 / Global.FrameRate), pos);
        }
        fire.Add(ge);
        //伤害判定产生
        
        int tda = damageSkill1;
        float tc = 0;
        float tr = areaSkill2;
        if (gifted[3] == 1)
        {
            tda = (int)(tda*1.5f);
        }

        if (gifted[2] == 1)
        {
            tc += (int)(100/Global.FrameRate);//0.1s
        }

        if (gifted[0] == 1)
        {
            tr *= 1.5f;
        }

        for(float i=0;i<=lastTimeSkill1;i+=0.5f)
        {
            SkillBase tmp = new SkillBase(0, tda, new FixVector2((Fix64)pos.x, (Fix64)pos.y), (Fix64)tr, (int)(tc * 1000 / Global.FrameRate), 
                frame+(int)(i*1000/Global.FrameRate), dmgSrc);
            _parentManager._skill.Add(tmp, RoomID);
        }
        if (gifted[1] == 1)
        {
            return (int)(1000 / Global.FrameRate * (countdownSkill1 - 1));
        }
        else
        {
            return (int)(1000 / Global.FrameRate * (countdownSkill1));
        }
    }

    public int Skill2Logic(int frame, int RoomID, List<int> gifted, Vector3 pos, int dmgSrc)
    {
        //闪电特效
        GameObject ge = GameObject.Instantiate(effectThunder);

        ge.GetComponent<ExplosionControl>().init(frame,
                (int)(0.7f * 1000 / Global.FrameRate), pos);

        thunder.Add(ge);

        //伤害判定
        int tda = damageSkill1;
        float tc = controlTime;        //控制
        float tr = areaSkill1;
        if (gifted[0] == 1)
        {
            tda = (int)(tda * 1.5f);
        }

        if (gifted[1] == 1)
        {
            tc *= 1.3f;
        }

        if (gifted[3] == 1)
        {
            tr *= 1.5f;
        }

        SkillBase tmp = new SkillBase(0, tda, new FixVector2((Fix64)pos.x, (Fix64)pos.y), (Fix64)tr, (int)(tc * 1000 / Global.FrameRate), frame, dmgSrc);

        _parentManager._skill.Add(tmp, RoomID);


        if (gifted[2] == 1)
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
        dir *= 0.1f;
        
        GameObject p = GameObject.Instantiate(effectIce);

        p.GetComponent<IceControl>().init(new FixVector2((Fix64)st.x, (Fix64)st.y),
            new FixVector2((Fix64)dir.x, (Fix64)dir.y),
            (Fix64)1f, RoomID, damageSkill3, dmgSrc, _parentManager
            );
        ice.Add(p);

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
        //fire部分
        List<GameObject> tg = new List<GameObject>();
        foreach (GameObject p in fire)
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
        fire.Clear();
        foreach (GameObject p in tg)
        {
            fire.Add(p);
        }
        //雷电部分
        tg.Clear();
        foreach (GameObject p in thunder)
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
        thunder.Clear();
        foreach (GameObject p in tg)
        {
            thunder.Add(p);
        }

        tg.Clear();

        foreach(GameObject p in ice)
        {
            if (p.GetComponent<IceControl>().updateLogic(frame))
            {
                GameObject.Destroy(p);
            }
            else
            {
                tg.Add(p);
            }
        }
        ice.Clear();
        foreach(var p in tg)
        {
            ice.Add(p);
        }
        tg.Clear();
    }
}
