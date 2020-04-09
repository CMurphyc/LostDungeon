﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabitAI : MonoBehaviour
{
    int Counter= 0;
    int state = 0;
    Animator ani;
    bool inited =false;

    int next_SwitchFrame = 130;

    int next_IdleFrame = 200;


    int IdleInterval = 120;

    AI_Behavior temp;
    AI_Rabit t2;

    List<BulletInfo> arrow = new List<BulletInfo>();
    // Start is called before the first frame update
    void Start()
    {
        ani = GetComponent<Animator>();
        //temp = new AI_Behavior(gameObject);
        t2 = new AI_Rabit(gameObject);
      
    }

    // Update is called once per frame
    void Update()
    {
        Counter += 1;

        if (Counter == 120)
        {

            t2.Start(Counter);
        }
        t2.Update(Counter);
        



        //生成子弹
        if (t2.bullet.ContainsKey(Counter))
        {
            for (int i = 0; i < t2.bullet[Counter].Count;i++ )
            {
                GameObject Bullet_Instance = Object.Instantiate(t2.bullet[Counter][i].BulletObject, t2.bullet[Counter][i].Position, t2.bullet[Counter][i].BulletObject.transform.rotation);

                BulletInfo Bullet_InstanceInfo = t2.bullet[Counter][i];
                Bullet_InstanceInfo.BulletObject = Bullet_Instance;
                arrow.Add(Bullet_InstanceInfo);
             
            }
        }
        for (int i = 0; i < arrow.Count;i++)
        {
            arrow[i].BulletObject.transform.Translate(5 * arrow[i].Toward * Time.deltaTime);
        }


    }
}




class AI_Rabit:AI_Behavior
{
    public Dictionary<int, List<BulletInfo>> bullet = new Dictionary<int, List<BulletInfo>>();


    GameObject Boss;
    //子弹类型数量
    int BulletTypeNumber = 4;

    public AI_Rabit(GameObject obj):base(obj)
    {
        base.Idle_FrameInterval = 120;
        base.Run_FrameInterval = 120;
        base.Attack_FrameInterval = 60;
       
        Boss = obj;
    }
    public override void BossAttackLogic(int frame)
    {
        int AttackInitFrame = frame + 20;
        int AttackInitFrame2 = frame + 50;
        RoundBulletSystem sys = new RoundBulletSystem();
        List<BulletInfo>  temp=sys.InitBullet(Boss.transform.position, 12, BulletTypeNumber, BossType.BossRaibit);
        foreach( BulletInfo item in temp)
        {
            if (!bullet.ContainsKey(AttackInitFrame))
            {
                bullet.Add(AttackInitFrame, new List<BulletInfo> { item });

            }
            else
            {
                bullet[AttackInitFrame].Add(item);
            }

            if (!bullet.ContainsKey(AttackInitFrame2))
            {
                bullet.Add(AttackInitFrame2, new List<BulletInfo> { item });
            }
            else
            {
                bullet[AttackInitFrame2].Add(item);
            }
        }
    }
    public override void BossRunLogic(int frame)
    {
      
    }
}


enum RabitBulletEffectType
{
    RabitBullet_Yellow,
    RabitBullet_Red,
    RabitBullet_Purple,
    RabitBullet_Blue
}

