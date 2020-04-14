using Pathfinding;
using System.Collections;
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


    List<AnimationInfo> AniList = new List<AnimationInfo>();
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
        //Counter += 1;

        //if (Counter == 120)
        //{
        //    t2.Start(Counter);
        //}
        //t2.Update(Counter);

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

        //
        if (t2.AniInfo.ContainsKey(Counter))
        {
            AniList.Add(t2.AniInfo[Counter]);
        }

        for  (int i = -0; i< AniList.Count;i++ )
        {



            AnimationInfo item = AniList[i];
            if (item.AnimationInterval>0)
            {
                //初始化寻路
                //Debug.Log("Pos:  " + item.obj.transform.position);
                item.obj.GetComponent<AIPath>().InitConfig(item.obj.transform.position, item.obj.transform.rotation, new Vector3(1.5f, 1.5f, 1.5f),Global.FrameRate);
                //获取当前帧位置
                Vector3 Pos;
                Quaternion Rot;
                item.obj.GetComponent<AIPath>().GetFramePosAndRotation(out Pos, out Rot);
                item.obj.transform.position = Pos;
                item.obj.transform.rotation = Rot;
                item.AnimationInterval--;

                AniList[i] = item;

                //Debug.Log("Left Frame: " + item.AnimationInterval);
            }
            

        }


    }
}

struct AnimationInfo
{
    public GameObject obj;

    public int AnimationInterval;

    
}


class AI_Rabit:AI_Behavior
{
    //int帧数 -> 对应的所有子弹
    public Dictionary<int, List<BulletInfo>> bullet = new Dictionary<int, List<BulletInfo>>();
    //人物移动帧数
    public Dictionary<int, AnimationInfo>  AniInfo= new Dictionary<int, AnimationInfo>();
    GameObject Boss;
    //子弹类型数量
    int BulletTypeNumber = 4;

    
    public AI_Rabit(GameObject obj):base(obj)
    {
        base.Idle_FrameInterval = 60;
        base.Run_FrameInterval = 240;
        base.Attack_FrameInterval = 300;
        Boss = obj;
    }
    public override void BossAttackLogic(int frame)
    {
        int Switch = Random.Range(1, 3);
        if (Switch == 1)
        {

            int AttackRate = 5;
            int AttackNumber = base.Attack_FrameInterval / AttackRate;
            RoundBulletSystem sys = new RoundBulletSystem();
            int InitAngle = Random.Range(1, 30);
            for (int i = 0; i < AttackNumber; i++)
            {
                int AttackInitFrame = frame + i * AttackRate + 20;
                Debug.Log("AttackFrame: " + AttackInitFrame);

                InitAngle += 5;
                List<BulletInfo> temp = sys.InitBullet(Boss.transform.position, 12, BulletTypeNumber, BossType.BossRaibit, InitAngle);
                foreach (BulletInfo item in temp)
                {
                    if (!bullet.ContainsKey(AttackInitFrame))
                    {
                        bullet.Add(AttackInitFrame, new List<BulletInfo> { item });
                    }
                    else
                    {
                        bullet[AttackInitFrame].Add(item);
                    }
                }
            }
        }
        else
        {

            int AttackRate = 10;
            int AttackNumber = base.Attack_FrameInterval / AttackRate;
            RoundBulletSystem sys = new RoundBulletSystem();
          
            for (int i = 0; i < AttackNumber; i++)
            {
                int AttackInitFrame = frame + i * AttackRate + 20;
                Debug.Log("AttackFrame: " + AttackInitFrame);
                int InitAngle = Random.Range(1, 30);
                List<BulletInfo> temp = sys.InitBullet(Boss.transform.position, 12, BulletTypeNumber, BossType.BossRaibit, InitAngle);
                foreach (BulletInfo item in temp)
                {
                    if (!bullet.ContainsKey(AttackInitFrame))
                    {
                        bullet.Add(AttackInitFrame, new List<BulletInfo> { item });
                    }
                    else
                    {
                        bullet[AttackInitFrame].Add(item);
                    }
                }
            }
        }
      
    }
    public override void BossRunLogic(int frame)
    {
    
        //Boss.transform.position = Pos;


        AnimationInfo temp = new AnimationInfo();
        temp.obj = Boss;
        temp.AnimationInterval = Run_FrameInterval;
        AniInfo.Add(frame+1, temp);


    }
}





enum RabitBulletEffectType
{
    RabitBullet_Yellow,
    RabitBullet_Red,
    RabitBullet_Purple,
    RabitBullet_Blue
}

