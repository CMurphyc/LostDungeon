using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class MagicianConfig : BaseConfig
{
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
}

