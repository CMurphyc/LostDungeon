using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class GuardianConfig : BaseConfig
{
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

    public int healSkill3;                    
    public float rangeSkill3;                   
    public float areaSkill3;
    public float countdownSkill3;           //lengque
    public float lastTimeSkill3;             //

    public Sprite skill1Image;
    public Sprite skill2Image;
    public Sprite skill3Image;

    public SkillAreaType skill1Type;
    public SkillAreaType skill2Type;
    public SkillAreaType skill3Type;

}

