using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class GhostConfig : BaseConfig
{
    public float timeSkilll1;//持续时间
    public float rangeSkill1;//技能1范围
    public float areaSkill1;//技能1区域
    public float dashDistance; // 技能1 位移距离

    public float countdownSkill1;//技能1冷却

    public float rangeSkill2;
    public float areaSkill2;
    public float timeSkill2;
    public float attackSkill2;//攻击力加成
    public float speedSkill2;//速度加成
    public float countdownSkill2;               //技能2冷却

    //public GameObject effectGully;          

    //public float rangeSkill3;
    //public float areaSkill3;
    public float countdownSkill3;           //lengque
    public float timeSkill3;             //控制时间


    public Sprite skill1Image;
    public Sprite skill2Image;
    public Sprite skill3Image;

    public SkillAreaType skill1Type;
    public SkillAreaType skill2Type;
    public SkillAreaType skill3Type;

}