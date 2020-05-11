﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuardianBase
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

    public float timeSkilll1;//无敌持续时间
    public float rangeSkill1;//技能1范围
    public float areaSkill1;//技能1区域
    public float countdownSkill1;//技能1冷却

    public float rangeSkill2;
    public float areaSkill2;
    public int damageSkill2;                     //伤害
    public float controlTimeSkill2;             //控制时间
    public float countdownSkill2;               //技能2冷却
    public GameObject effectGully;          //

    public GameObject effectCrush;              //位移地板特效
    public int damageSkill3;
    public float rangeSkill3;
    public float areaSkill3;
    public float countdownSkill3;           //lengque
    public float controlTimeSkill3;             //控制时间

    public Sprite skill1Image;
    public Sprite skill2Image;
    public Sprite skill3Image;

    public SkillAreaType skill1Type;
    public SkillAreaType skill2Type;
    public SkillAreaType skill3Type;

    List<GameObject> gully = new List<GameObject>();
    List<Tuple<GameObject,int>> shield = new List<Tuple<GameObject, int>>();

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
        gully.Clear();
    }
    public GuardianBase(BattleManager parentManager)
    {
        GuardianConfig x = Resources.Load("Configs/Heros/GuardianConfig") as GuardianConfig;

        name = x.name;
        obj = x.obj;
        HP = x.HP;
        moveSpeed = x.moveSpeed;
        damge = x.damge;
        bulletSpeed = x.bulletSpeed;
        fireSpeed = x.fireSpeed;
        bulletEffect = x.bulletEffect;
        bulletObj = x.bulletObj;

        timeSkilll1 = x.timeSkilll1;
        rangeSkill1 = x.rangeSkill1;
        areaSkill1 = x.areaSkill1;
        countdownSkill1 = x.countdownSkill1;

        rangeSkill2 = x.rangeSkill2;
        areaSkill2 = x.areaSkill2;
        damageSkill2 = x.damageSkill2;
        controlTimeSkill2 = x.controlTimeSkill2;
        countdownSkill2 = x.countdownSkill2;



        damageSkill3 = x.damageSkill3;
        rangeSkill3 = x.rangeSkill3;
        areaSkill3 = x.areaSkill3;
        countdownSkill3 = x.countdownSkill3;           //lengque
        controlTimeSkill3 = x.controlTimeSkill3;             //控制时间


        effectGully = x.effectGully;
        effectCrush = x.effectCrush;

        skill1Image = x.skill1Image;
        skill2Image = x.skill2Image;
        skill3Image = x.skill3Image;


        skill1Type = x.skill1Type;
        skill2Type = x.skill2Type;
        skill3Type = x.skill3Type;

        _parentManager = parentManager;
    }

    public int Skill1Logic(int frame, int RoomID, List<int> gifted, GameObject p,int dmgSrc)//返回值就是cd
    {
        p.GetComponent<PlayerModel_Component>().SetMuteki((int)(timeSkilll1 * 1000 / Global.FrameRate));
        p.transform.GetChild(1).localScale = new Vector3(2, 2, 1);
        p.transform.GetChild(1).gameObject.transform.localPosition = new Vector3(0, 0, 0);
        p.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().color = new Color(255f / 255, 255f/255, 255f/255, 100f/255);

        

        shield.Add(new Tuple<GameObject,int>(p, frame + (int)(timeSkilll1 * 1000 / Global.FrameRate)));

        if (gifted[1] == 1)
        {
            return (int)(1000 / Global.FrameRate * (countdownSkill1 - 1));
        }
        else
        {
            return (int)(1000 / Global.FrameRate * (countdownSkill1));
        }
    }

    public int Skill2Logic(int frame, int RoomID, List<int> gifted, Vector3 pos,Vector3 dir, int dmgSrc)
    {
        dir -= pos;
        dir.Normalize();
        //效果
        GameObject ge = GameObject.Instantiate(effectGully);
        ge.GetComponent<ExplosionControl>().init(frame , (int)(1f * 1000 / Global.FrameRate), pos+dir*1f);
        gully.Add(ge);

        GameObject gr = GameObject.Instantiate(effectGully);
        gr.GetComponent<ExplosionControl>().init(frame+ (int)( 800 / Global.FrameRate), (int)(1f * 1000 / Global.FrameRate), pos + dir * 2f);
        gully.Add(gr);

        GameObject gt = GameObject.Instantiate(effectGully);
        gt.GetComponent<ExplosionControl>().init(frame+ (int)(1600 / Global.FrameRate), (int)(1f * 1000 / Global.FrameRate), pos + dir * 3f);
        gully.Add(gt);
        //伤害判定产生

        SkillBase tmp = new SkillBase(0, damageSkill2, new FixVector2((Fix64)(pos.x+dir.x*1f), (Fix64)(pos.y + dir.y * 1f)), (Fix64)1f, 
            (int)(1.5f * 1000 / Global.FrameRate), frame , dmgSrc);
        _parentManager._skill.Add(tmp, RoomID);

        SkillBase tmo = new SkillBase(0, damageSkill2, new FixVector2((Fix64)(pos.x + dir.x * 2f), (Fix64)(pos.y + dir.y * 2f)), (Fix64)1f,
            (int)(1.5f * 1000 / Global.FrameRate), frame + (int)(0.8f * 1000 / Global.FrameRate), dmgSrc);
        _parentManager._skill.Add(tmo, RoomID);

        SkillBase tmi = new SkillBase(0, damageSkill2, new FixVector2((Fix64)(pos.x + dir.x * 3f), (Fix64)(pos.y + dir.y * 3f)), (Fix64)1f,
            (int)(1.5f * 1000 / Global.FrameRate), frame + (int)(1.6f * 1000 / Global.FrameRate), dmgSrc);
        _parentManager._skill.Add(tmi, RoomID);


        if (gifted[2] == 1)
        {
            return (int)(1000 / Global.FrameRate * (countdownSkill2 - 2));
        }
        else
        {
            return (int)(1000 / Global.FrameRate * (countdownSkill2));
        }
    }


    public int Skill3Logic(int frame, int RoomID, List<int> gifted, Vector3 pos, int dmgSrc)
    {


        if (gifted[2] == 1)
        {
            return (int)(1000 / Global.FrameRate * (countdownSkill2 - 2));
        }
        else
        {
            return (int)(1000 / Global.FrameRate * (countdownSkill2));
        }
    }
    public void updateLogic(int frame)
    {
        List<Tuple<GameObject, int>> tp = new List<Tuple<GameObject, int>>();
        foreach(var p in shield)
        {
            Debug.Log(p.Item1.GetComponent<PlayerModel_Component>().muteki);
            if(p.Item2==frame)
            {
                Debug.Log("aaaaaaaaaaa");
                p.Item1.transform.GetChild(1).localScale = new Vector3(1, 1, 1);
                p.Item1.transform.GetChild(1).gameObject.transform.localPosition = new Vector3(-0.05f, -0.04f, 0);
                p.Item1.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
            }
            else
            {
                tp.Add(p);
            }
        }
        shield.Clear();
        foreach(var p in tp)
        {
            shield.Add(p);
        }
        tp.Clear();


        List<GameObject> qwe = new List<GameObject>();
        foreach(var x in gully)
        {
            if (x.GetComponent<ExplosionControl>().getEndFrame() != frame)
            {
                x.GetComponent<ExplosionControl>().updateLogic(frame);
                qwe.Add(x);
            }
            else
            {
                GameObject.Destroy(x);
            }
        }
        gully.Clear();
        foreach(var x in qwe)
        {
            gully.Add(x);
        }
        qwe.Clear();
    }
}