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
    public List<int> bulletEffect;  //子弹附加效果
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

    public GameObject effectFire;           //火特效
    public GameObject effectThunder;        //雷特效

    public Sprite skill1Image;
    public Sprite skill2Image;

    List<GameObject> fire = new List<GameObject>();
    List<GameObject> thunder = new List<GameObject>();

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

        effectFire= x.effectFire;
        effectThunder = x.effectThunder;

        skill1Image = x.skill1Image;
        skill2Image = x.skill2Image;

        _parentManager = parentManager;
    }

    public int Skill1Logic(int frame, int RoomID, List<int> gifted, Vector3 pos)//返回值就是cd
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
                frame+(int)(i*1000/Global.FrameRate));
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

    public int Skill2Logic(int frame, int RoomID, List<int> gifted, Vector3 pos)
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

        Debug.Log("woshele");

        SkillBase tmp = new SkillBase(0, tda, new FixVector2((Fix64)pos.x, (Fix64)pos.y), (Fix64)tr, (int)(tc * 1000 / Global.FrameRate), frame);

        _parentManager._skill.Add(tmp, RoomID);


        if (gifted[2] == 1)
        {
            return (int)(1000 / Global.FrameRate * (countdownSkill1 - 2));
        }
        else
        {
            return (int)(1000 / Global.FrameRate * (countdownSkill1));
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
    }
}
