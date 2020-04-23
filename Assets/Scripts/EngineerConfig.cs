using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class EngineerConfig : BaseConfig
{
    public float rangSkill1;                    //炮台技能的释放范围
    public float areaSkill1;                    //炮台大小
    public float lastTimeSkill1;                //炮台持续帧数
    public float countdownSkill1;               //技能1冷却
    public int HPSkill1;                        //炮台血量
    public int damageSkill1;                    //炮台单发伤害
    public float bulletSpeedSkill1;             //炮台子弹速度
    public float fireSpeedSkill1;               //炮台射速
    public float controlTimeSkill1;             //炮台打中的敌人晕眩时间
    public List<int> bulletEffectSkill1;        //炮台子弹附加效果
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

    public Sprite skill1Image;
    public Sprite skill2Image;

}

