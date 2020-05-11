using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class GhostBase
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


    public int skill1Duration;  // Dash持续帧数
    public float rangeSkill1;                    //炮台技能的释放范围
    public float areaSkill1;                    //炮台大小

    public int DashDistanceSkill1;                    //Dash 距离
    public float countdownSkill1;               //技能1冷却



    public int skill2Duration;  // 
    public float countdownSkill2;               //技能2冷却
    public float rangeSkill2;                    //炮台技能的释放范围
    public float areaSkill2;                    //炮台大小
    public int AttackBuff;

    public int skill3Duration;  // 


    public float countdownSkill3;               //技能3冷却


    public Sprite skill1Image;
    public Sprite skill2Image;
    public Sprite skill3Image;

    SkillModule _parentManager;


    public SkillAreaType skill1Type;
    public SkillAreaType skill2Type;
    public SkillAreaType skill3Type;


    public void Free()
    {
     
    }
    public GhostBase(SkillModule parent)
    {
        GhostConfig x = Resources.Load("Configs/Heros/GhostConfig") as GhostConfig;

        name = x.name;
        obj = x.obj;
        HP = x.HP;
        moveSpeed = x.moveSpeed;
        damge = x.damge;
        bulletSpeed = x.bulletSpeed;
        fireSpeed = x.fireSpeed;
        bulletEffect = x.bulletEffect;
        bulletObj = x.bulletObj;

        skill1Duration =(int) (x.timeSkilll1*Global.FrameRate);
        rangeSkill1 = x.rangeSkill1;
        areaSkill1 = x.areaSkill1;
        countdownSkill1 = x.countdownSkill1;
        DashDistanceSkill1 = (int)x.dashDistance;


        skill2Duration = (int)(x.timeSkill2 * Global.FrameRate);
        countdownSkill2 = x.countdownSkill2;
        rangeSkill2 = x.rangeSkill2;
        areaSkill2 = x.areaSkill2;
        AttackBuff = (int)x.attackSkill2;
        //throwTime = x.throwTime;
        //damgeSkill2 = x.damgeSkill2;
        //controlTimeSkill2 = x.controlTimeSkill2;
        //countdownSkill2 = x.countdownSkill2;

        skill3Duration = (int)(x.timeSkill3 * Global.FrameRate);
        countdownSkill3 = x.countdownSkill3;

          skill1Image= x.skill1Image;
        skill2Image = x.skill2Image;
        skill3Image = x.skill3Image;


    skill1Type = x.skill1Type;
        skill2Type = x.skill2Type;
        skill3Type = x.skill3Type;
        //effectArtillery = x.effectArtillery;
        //effectGernade = x.effectGernade;
        //effectGernadeExplosion = x.effectGernadeExplosion;

        //skill1Image = x.skill1Image;
        //skill2Image = x.skill2Image;

        _parentManager = parent;
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
    public float Skill3Range()
    {
        //需要传入获得的player的天赋表
        return 0;
    }

    public float Skill3Area()
    {
        return 0;
    }

    public int Skill1Logic(int frame, int RoomID, FixVector2 toward, GameObject obj)//返回值就是cd
    {
        obj.GetComponent<PlayerModel_Component>().DashDuration = skill1Duration;
        obj.GetComponent<PlayerModel_Component>().inDash = true;
        obj.GetComponent<PlayerModel_Component>().DashToward = toward;
        return (int)(countdownSkill1 * 1000 / Global.FrameRate);
    }

    public int Skill2Logic(GameObject obj)
    {
        if (!obj.GetComponent<PlayerModel_Component>().buff.AttackIncrease)
        {
            obj.GetComponent<PlayerModel_Component>().buff.AttackIncrease = true;
            obj.GetComponent<PlayerModel_Component>().buff.AttackIncrease_RemainingFrame = skill2Duration;

            obj.GetComponent<PlayerModel_Component>().BuffattackPoint = (Fix64)AttackBuff * obj.GetComponent<PlayerModel_Component>().attackPoint;
            obj.GetComponent<PlayerModel_Component>().attackPoint += obj.GetComponent<PlayerModel_Component>().BuffattackPoint;
        }
        return (int)(countdownSkill2 * 1000 / Global.FrameRate);
    }

    public int Skill3Logic(GameObject obj)
    {
        if (!obj.GetComponent<PlayerModel_Component>().buff.Invisible)
        {
            obj.GetComponent<PlayerModel_Component>().buff.Invisible = true;
            obj.GetComponent<PlayerModel_Component>().buff.Invisible_RemainingFrame = skill3Duration;
        }

        return (int)(countdownSkill3 * 1000 / Global.FrameRate);
    }




    public void updateLogic(int frame)
    {
        //dash logic
        foreach (var item in _parentManager._parentManager._player.playerToPlayer)
        {
            if (item.Value.obj.GetComponent<PlayerModel_Component>().inDash)
            {

                FixVector2 PlayerPos = item.Value.obj.GetComponent<PlayerModel_Component>().GetPlayerPosition();
                FixVector2 Toward = item.Value.obj.GetComponent<PlayerModel_Component>().DashToward;
      

                Fix64 DashDistanceByFrame = (Fix64)DashDistanceSkill1 / (Fix64)skill1Duration;
                FixVector2 MoveVec = Toward * DashDistanceByFrame;

                FixVector2 ToPos = PlayerPos + MoveVec;

                Polygon poly = new Polygon(PolygonType.Circle);
                Fix64 radius = (Fix64)0.1;
                poly.InitCircle(ToPos, radius);
               
                int PlayerUID = _parentManager._parentManager._player.FindPlayerUIDbyObject(item.Value.obj);
                if (PlayerUID == -1)
                    return;
                int RoomId = _parentManager._parentManager._player.playerToPlayer[PlayerUID].RoomID;
             
                if (_parentManager._parentManager._terrain.IsMovable(poly, RoomId))
                {
                    item.Value.obj.GetComponent<PlayerModel_Component>().Move(MoveVec);
                }
            }
        }
    }
}