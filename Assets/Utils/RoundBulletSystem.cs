using UnityEngine;
using System.Collections;
using System.Collections.Generic;

enum BossType
{
    BossRaibit,
    BossGhost,

}


struct BulletInfo
{
    public Vector2 Toward;
    public BossType BossType;
    public int BulletTytpe;
    public GameObject BulletObject;
    public Vector2 Position;
    public BulletInfo(Vector2 temp, BossType boss, int bullet, GameObject obj, Vector2 Location)
    {
        Toward = temp;
        BossType = boss;
        BulletTytpe = bullet;
        BulletObject = obj;
        Position = Location;
    }


}

//圆形弹幕工具 自定义圆上子弹数量， 根据数量平均分布在 圆边上, 随机子弹类型
class RoundBulletSystem
{

    // SpwanLocation 怪物中心店
    // BulletNumber 一次攻击发射的子弹数量
    public List<BulletInfo> InitBullet(Vector2 SpwanLocation, int BulletNumber, int BulletTypeNumber, BossType BossType , int InitAngle)
    {

        List<BulletInfo> SpwanedBullet = new List<BulletInfo>();

        int BulletType = Random.Range(0, BulletTypeNumber);

        float angle = 360 / BulletNumber;


        for (int i = 0; i < BulletNumber; i++)
        {
            float CurrentAngle = InitAngle+ angle * i;
            //Fix64 rad = ((Fix64)CurrentAngle * Fix64.Deg2Rad);
            //float x = (float)SpwanLocation.x + (float)Fix64.Cos(rad);
            //float y = (float)SpwanLocation.y + (float)Fix64.Sin(rad);

            float x = (float)SpwanLocation.x + Mathf.Cos(CurrentAngle);
            float y = (float)SpwanLocation.y + Mathf.Sin(CurrentAngle);

            //Vector2 vec = new Vector2((float)Fix64.Cos(rad), (float)Fix64.Sin(rad));

            Vector2 vec = new Vector2(Mathf.Cos(CurrentAngle / 180f * Mathf.PI), Mathf.Sin(CurrentAngle / 180f * Mathf.PI));
            vec = vec.normalized;

            GameObject Bullet_Prefab = GetBulletObject(BossType, BulletType);
            //GameObject Bullet_Instance = Object.Instantiate(Bullet_Prefab, SpwanLocation, Bullet_Prefab.transform.rotation);

            SpwanedBullet.Add(new BulletInfo(vec, BossType, BulletType, Bullet_Prefab, SpwanLocation));


        }
        return SpwanedBullet;
    }

    GameObject GetBulletObject(BossType boss, int BulletType)
    {

        GameObject temp = null;
        switch (boss)
        {
            case BossType.BossRaibit:
                {
                    if (BulletType == 0)
                    {
                        temp = (GameObject)Resources.Load("Model/Boss/Boss_Rabit/bullet/BossRabit_BulletYellow");
                    }
                    else if (BulletType == 1)
                    {
                        temp = (GameObject)Resources.Load("Model/Boss/Boss_Rabit/bullet/BossRabit_BulletRed");

                    }
                    else if (BulletType == 2)
                    {
                        temp = (GameObject)Resources.Load("Model/Boss/Boss_Rabit/bullet/BossRabit_BulletPurple");

                    }
                    else if (BulletType == 3)
                    {
                        temp = (GameObject)Resources.Load("Model/Boss/Boss_Rabit/bullet/BossRabit_BulletBlue");

                    }
                    break;
                }

            default:
                {
                    break;
                }

        }


        return temp;

    }



}
