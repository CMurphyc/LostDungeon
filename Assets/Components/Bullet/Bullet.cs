using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum bulletType
{
    Penetrate = 1,
    Sputtering = 2,
    LightningChain = 3,
    Freeze = 11,
    Poision = 12,
    Burn = 13,
    Dizziness = 14,
    Retard = 15,
    Bigger = 21,
    Smaller = 22,
    Longer = 23

}
public class Bullet
{
    public string tag;
    public Vector2 anchor;
    public Vector2 toward;
    public float speed;
    public float damage;
    public int roomid;
    public bool active;
    public Vector2 bulletScale;
    public CommonCollider collider;
    public GameObject bulletPrefab;
    public List<int> itemList;
    public List<int> splitEffectList;
    public List<int> attackEffectList;
    public List<int> debuffList;
    public List<int> scaleEffectList;
    public Bullet(string tag, Vector2 anchor, Vector2 toward, float speed, float damage, int roomid, GameObject bulletPrefab, List<int> itemList)
    {
        this.tag = tag;
        this.anchor = anchor;
        this.toward = toward.normalized;
        this.speed = speed;
        this.damage = damage;
        this.roomid = roomid;
        this.bulletPrefab = bulletPrefab;
        this.itemList = itemList;

        this.active = true;
        this.bulletScale = new Vector2(1, 1);

        GetBulletCollider(5f);
        GetAllEffect();
    }
    private void GetBulletCollider(float radius)
    {
        collider.BuildCircleCollider(anchor, toward, radius);
    }
    private void GetAllEffect()
    {
        foreach(var it in itemList)
        {
            if(it == 0) splitEffectList.Add(it);
            else if(it < 10) attackEffectList.Add(it);
            else if(it < 20) debuffList.Add(it);
            else if(it < 30) scaleEffectList.Add(it);
        }
    }

    public int GetSplitNum()
    {
        return splitEffectList.Count;
    }
}

public class BulletBase
{
    //存储每个子弹的所有信息
    protected List<Bullet> spwanedBullet;
    //视图层子弹集合
    protected List<GameObject> bulletList;
    //子弹类型（这里直接传一个GameObject，实例化的都是它的拷贝）
    protected GameObject bulletPrefab;
    public virtual void BulletInit(string tag, Vector2 anchor, Vector2 toward, float speed, float damage, int roomid, GameObject bulletPrefab, List<int> itemList) {}
    public virtual void LogicUpdate() {}
    public virtual void ViewUpdate() {}
    public virtual void ContainerInit() {}
}

public class BulletUnion : BulletBase
{
    public override void ContainerInit()
    {
        spwanedBullet = new List<Bullet>();
        bulletList = new List<GameObject>();
    }

    //初始化所有子弹逻辑层logic的信息以及视图层prefab的信息
    public override void BulletInit(string tag, Vector2 anchor, Vector2 toward, float speed, float damage, int roomid, GameObject bulletPrefab, List<int> itemList)
    {        

        //逻辑层信息初始化

        //中心作为基准的子弹
        Bullet midBullet = new Bullet(tag, anchor, toward, speed, damage, roomid, bulletPrefab, itemList);

        //子弹偏移数量
        int splitNum = midBullet.GetSplitNum();

        //左侧偏移的子弹
        for(int i = 1; i <= splitNum; ++i) spwanedBullet.Add(new Bullet(tag, anchor, Converter.NormalVector2Rotate(toward, -15f * i), speed, damage, roomid, bulletPrefab, itemList));

        //中央的子弹
        spwanedBullet.Add(midBullet);

        //右侧偏移的子弹
        for(int i = 1; i <= splitNum; ++i) spwanedBullet.Add(new Bullet(tag, anchor, Converter.NormalVector2Rotate(toward, 15f * i), speed, damage, roomid, bulletPrefab, itemList));

        //视图层信息初始化
        //根据信息量实例化对应信息的子弹实体
        foreach(var it in spwanedBullet)
        {
            GameObject bulletInstance = GameObject.Instantiate(bulletPrefab, anchor, bulletPrefab.transform.rotation);
            bulletList.Add(bulletInstance);
        }
    }

    private bool CollideCheck(Bullet bullet, MonsterMoudle monster)
    {
        CollideDetecter collideDetecter = new CollideDetecter();
        return collideDetecter.CircleCollideRect(bullet.collider.circle, monster.collider.rectangle);
    }

    //溅射
    private void Sputtering(Bullet bullet)
    {
        for(int i = 0; i < MonsterMoudle.roomToMonster[bullet.roomid].Count; ++i)
        {
            //获取真实的敌对单位位置的接口，待对接
            //在溅射范围内
            if(Vector2.Distance(bullet.anchor, MonsterMoudle.roomToMonster[bullet.roomid][i].transform.position) <= 20)
            {
                //敌对单位受击接口，待对接
                MonsterMoudle monsterMoudle = MonsterMoudle.roomToMonster[bullet.roomid][i].GetComponent<MonsterMoudle>();
                monsterMoudle.BeAttacked(bullet.damage);
            }
        }
    }
    //闪电链
    private void LightningChain(Bullet bullet)
    {
        List<float> enemyDistance = new List<float>();
        for(int i = 0; i < MonsterMoudle.roomToMonster[bullet.roomid].Count; ++i)
        {
            //获取真实的敌对单位位置的接口，待对接
            enemyDistance.Add(Vector2.Distance(bullet.anchor, MonsterMoudle.roomToMonster[bullet.roomid][i].transform.position));
        }
        enemyDistance.Sort();

        //移除不需要的distance
        for(int i =  enemyDistance.Count; i >= 3; --i)
        {
            enemyDistance.RemoveAt(i);
        }

        for(int i = 0; i < MonsterMoudle.roomToMonster[bullet.roomid].Count; ++i)
        {
            //获取真实的敌对单位位置的接口，待对接
            if(Vector2.Distance(bullet.anchor, MonsterMoudle.roomToMonster[bullet.roomid][i].transform.position) == enemyDistance[0] || 
               Vector2.Distance(bullet.anchor, MonsterMoudle.roomToMonster[bullet.roomid][i].transform.position) == enemyDistance[1] || 
               Vector2.Distance(bullet.anchor, MonsterMoudle.roomToMonster[bullet.roomid][i].transform.position) == enemyDistance[2])
            {
                //敌对单位受击接口，待对接
                MonsterMoudle monsterMoudle = MonsterMoudle.roomToMonster[bullet.roomid][i].GetComponent<MonsterMoudle>();
                monsterMoudle.BeAttacked(bullet.damage);
            }
        }

    }
    //穿透
    private void Penetrate(Bullet bullet)
    {
        bullet.active = true;
    }

    private void BiggerBullet(Bullet bullet)
    {
        bullet.bulletScale *= 1.5f;
    }

    private void SmallerBullet(Bullet bullet)
    {
        bullet.bulletScale *= 0.8f;
    }

    private void LongerBullet(Bullet bullet)
    {
        bullet.bulletScale.x *= 1.5f;
    }

    public override void LogicUpdate()
    {
        for(int i = 0; i < spwanedBullet.Count; ++i)
        {
            for(int j = 0; j < MonsterMoudle.roomToMonster[spwanedBullet[i].roomid].Count; ++j)
            {
                //检测子弹与敌方单位的碰撞，这里敌方单位的碰撞盒通过GetComponent获取，待对接
                MonsterMoudle monsterMoudle = MonsterMoudle.roomToMonster[spwanedBullet[i].roomid][j].GetComponent<MonsterMoudle>();
                if(CollideCheck(spwanedBullet[i], monsterMoudle) == true)
                {
                    spwanedBullet[i].active = false;
                    //attackEffect逻辑层面的实现
                    foreach(var effect in spwanedBullet[i].attackEffectList)
                    {
                        switch(effect)
                        {
                            case (int)bulletType.Penetrate : 
                                Penetrate(spwanedBullet[i]);
                                break;
                            case (int)bulletType.Sputtering : 
                                Sputtering(spwanedBullet[i]);
                                break;
                            case (int)bulletType.LightningChain :
                                LightningChain(spwanedBullet[i]);
                                break;
                        }
                        // if(effect == (int)bulletType.Penetrate) Penetrate(spwanedBullet[i]);
                        // else if(effect == (int)bulletType.Sputtering) Sputtering(spwanedBullet[i]);
                        // else if(effect == (int)bulletType.LightningChain) LightningChain(spwanedBullet[i]);
                    }

                    //debuff传递逻辑层面的实现
                    foreach(var debuff in spwanedBullet[i].debuffList)
                    {
                        //待对接敌方单位的debuff接口
                        // switch(debuff)
                        // {
                        //     case : (int)bulletType.Freeze
                        // }
                        // if(debuff == (int)bulletType.Freeze)
                        // else if(debuff == (int)bulletType.Poision)
                        // else if(debuff == (int)bulletType.Burn)
                        // else if(debuff == (int)bulletType.Dizziness)
                        // else if(debuff == (int)bulletType.Retard)
                    }
                    //理论上一个子弹（不考虑穿刺）只可能击中一个怪物，所以特判穿刺之外的其他情况在找到一个碰撞的就可以停止遍历
                    if(spwanedBullet[i].active == false) break;
                }
            }
        }
        
        //更新逻辑层子弹的位置
        List<Bullet> latestBullet = new List<Bullet>();
        foreach(var it in spwanedBullet)
        {
            Bullet bullet = it;
            if(bullet.active == true) bullet.anchor += bullet.toward * bullet.speed;
            latestBullet.Add(bullet);
        }
        
        spwanedBullet.Clear();

        foreach(var it in latestBullet) spwanedBullet.Add(it);
    }

    public override void ViewUpdate()
    {
        for(int i = 0; i < spwanedBullet.Count; ++i)
        {
            if(spwanedBullet[i].active == true) 
            {
                //更新子弹scale
                bulletList[i].transform.localScale = spwanedBullet[i].bulletScale;
                //更新子弹位置
                //每次移动定位子弹speed的1/5，以免出现step太大导致的移动不平滑的问题
                bulletList[i].transform.position = Vector2.MoveTowards(bulletList[i].transform.position, spwanedBullet[i].anchor, spwanedBullet[i].speed / 5f);
            }
        }

        //数据层面的销毁子弹的存储集合
        List<Bullet> destroyBullet = new List<Bullet>();
        List<Bullet> liveBullet = new List<Bullet>();

        //视图层面的销毁子弹的存储集合
        List<GameObject> destroyBulletObject = new List<GameObject>();
        List<GameObject> liveBulletObject = new List<GameObject>();


        for(int i = 0; i < spwanedBullet.Count; ++i)
        {
            //为了移除应该destroy的子弹，因为不一定在最后一位，不能直接Remove，所以这边需要拿出来分类后再销毁
            if(spwanedBullet[i].active == true) 
            {
                liveBullet.Add(spwanedBullet[i]);
                liveBulletObject.Add(bulletList[i]);
            }
            else 
            {
                destroyBullet.Add(spwanedBullet[i]);
                destroyBulletObject.Add(bulletList[i]);
            }
        }

        spwanedBullet.Clear();
        bulletList.Clear();

        foreach(var it in liveBullet) spwanedBullet.Add(it);
        foreach(var it in liveBulletObject) bulletList.Add(it);

        //销毁该die的子弹
        for(int i = 0; i < destroyBulletObject.Count; ++i) UnityEngine.Object.Destroy(destroyBulletObject[i]);

    }
}




class Converter
{
    public static float DegreeToRadian(float degree)
    {
        return Mathf.PI / 180f * degree;
    }

    public static float RadianToDegree(float Radian)
    {
        return Radian * 180 / Mathf.PI;
    }

    //输入为角度制
    public static Vector2 NormalVector2Rotate(Vector2 v, float rotateAngle)
    {
        float length = Mathf.Sqrt(v.x * v.x + v.y * v.y);
        float originAngle = Mathf.Atan(v.y / v.x);

        Vector2 rotateVector = new Vector2(length * Mathf.Cos(originAngle + DegreeToRadian(rotateAngle)), length * Mathf.Sin(originAngle + DegreeToRadian(rotateAngle)));
        
        return rotateVector;
    }
}

