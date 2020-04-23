using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBase
{
    public int tag;                     //技能所属方
    public int damage;                   //伤害值
    public FixVector2 center;           //技能中心
    public Fix64 radius;                //技能伤害半径
    public int controlTime;             //控制帧数
    public int frame;                   //生效帧


    public SkillBase(int Tag,int Damage,FixVector2 Center,Fix64 Radius,int ControlTime,int Frame)
    {
        tag = Tag;
        damage = Damage;
        center = Center;
        radius = Radius;
        controlTime = ControlTime;
        frame = Frame;
    }

}
