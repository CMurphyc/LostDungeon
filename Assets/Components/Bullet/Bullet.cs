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
    public FixVector2 anchor;
    public FixVector2 toward;
    public Fix64 speed;
    public Fix64 damage;
    public int roomid;
    public bool active;
    public FixVector2 bulletScale;
    public CommonCollider collider;
    public GameObject bulletPrefab;
    public List<int> itemList;
    public List<int> splitEffectList;
    public List<int> attackEffectList;
    public List<int> debuffList;
    public List<int> scaleEffectList;

    

    public Bullet(string tag, FixVector2 anchor, FixVector2 toward, Fix64 speed, Fix64 damage, int roomid, GameObject bulletPrefab, List<int> itemList)
    {
        BulletContainerInit();

        this.tag = tag;
        this.anchor = anchor;

        this.toward = toward.GetNormalized();

        this.speed = speed;
        this.damage = damage;
        this.roomid = roomid;
        this.bulletPrefab = bulletPrefab;
        this.itemList = itemList;

        this.active = true;
        this.bulletScale = new FixVector2(1, 1);

        GetBulletCollider((Fix64)5f);
        GetAllEffect();
    }
    private void BulletContainerInit()
    {
        itemList = new List<int>();
        splitEffectList = new List<int>();
        attackEffectList = new List<int>();
        debuffList = new List<int>();
        scaleEffectList = new List<int>();
}
    private void GetBulletCollider(Fix64 radius)
    {
        //collider.BuildCircleCollider(anchor, toward, radius);
    }
    private void GetAllEffect()
    {
        foreach (var it in itemList)
        {
            if (it == 0) splitEffectList.Add(it);
            else if (it < 10) attackEffectList.Add(it);
            else if (it < 20) debuffList.Add(it);
            else if (it < 30) scaleEffectList.Add(it);
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
    public virtual void BulletInit(string tag, FixVector2 anchor, FixVector2 toward, Fix64 speed, Fix64 damage, int roomid, GameObject bulletPrefab, List<int> itemList) { }
    public virtual void LogicUpdate() { }
    public virtual void ViewUpdate() { }
    public virtual void ContainerInit() { }
}

public class BulletUnion : BulletBase
{
    BattleManager _parentManager;

    public BulletUnion(BattleManager parent)
    {
        _parentManager = parent;
    }
    public override void ContainerInit()
    {
        spwanedBullet = new List<Bullet>();
        bulletList = new List<GameObject>();
    }

    //初始化所有子弹逻辑层logic的信息以及视图层prefab的信息
    public override void BulletInit(string tag, FixVector2 anchor, FixVector2 toward, Fix64 speed, Fix64 damage, int roomid, GameObject bulletPrefab, List<int> itemList)
    {
        //Debug.Log("传进来的vector：" + toward.x + " " + toward.y);
        ContainerInit();
        //逻辑层信息初始化

        //中心作为基准的子弹
        Bullet midBullet = new Bullet(tag, anchor, toward, speed, damage, roomid, bulletPrefab, itemList);

        //子弹偏移数量
        int splitNum = midBullet.GetSplitNum();

        //左侧偏移的子弹
        for (int i = 1; i <= splitNum; ++i) spwanedBullet.Add(new Bullet(tag, anchor, Converter.NormalFixVector2Rotate(toward, (Fix64)(-15f * i)), speed, damage, roomid, bulletPrefab, itemList));

        //中央的子弹
        if(midBullet.toward != FixVector2.Zero) spwanedBullet.Add(midBullet);

        //右侧偏移的子弹
        for (int i = 1; i <= splitNum; ++i) spwanedBullet.Add(new Bullet(tag, anchor, Converter.NormalFixVector2Rotate(toward, (Fix64)(15f * i)), speed, damage, roomid, bulletPrefab, itemList));

        //视图层信息初始化
        //根据信息量实例化对应信息的子弹实体
        foreach (var it in spwanedBullet)
        {
            GameObject bulletInstance = GameObject.Instantiate(bulletPrefab, Converter.FixVector2ToVector2(anchor), bulletPrefab.transform.rotation);
            bulletList.Add(bulletInstance);
        }
    }

    private bool CollideCheck(Bullet bullet, MonsterModel_Component monster)
    {
        return false;
        //CollideDetecter collideDetecter = new CollideDetecter();
        //return collideDetecter.CircleCollideRect(bullet.collider.circle, monster.collider.rectangle);
    }

    //溅射
    private void Sputtering(Bullet bullet)
    {
        for (int i = 0; i < _parentManager._monster.RoomToMonster[bullet.roomid].Count; ++i)
        {
            //获取真实的敌对单位位置的接口，待对接
            //在溅射范围内
            if (Vector2.Distance(Converter.FixVector2ToVector2(bullet.anchor), _parentManager._monster.RoomToMonster[bullet.roomid][i].transform.position) <= 20)
            {
                //敌对单位受击接口，待对接
                //MonsterModule MonsterModule = _parentManager._monster.RoomToMonster[bullet.roomid][i].GetComponent<MonsterModule>();
                //MonsterModule.BeAttacked(bullet.damage);
            }
        }
    }
    //闪电链
    private void LightningChain(Bullet bullet)
    {
        List<Fix64> enemyDistance = new List<Fix64>();
        for (int i = 0; i < _parentManager._monster.RoomToMonster[bullet.roomid].Count; ++i)
        {
            //获取真实的敌对单位位置的接口，待对接
            enemyDistance.Add(FixVector2.Distance(bullet.anchor, Converter.Vector2ToFixVector2(_parentManager._monster.RoomToMonster[bullet.roomid][i].transform.position)));
        }
        enemyDistance.Sort();

        //移除不需要的distance
        for (int i = enemyDistance.Count; i >= 3; --i)
        {
            enemyDistance.RemoveAt(i);
        }

        for (int i = 0; i < _parentManager._monster.RoomToMonster[bullet.roomid].Count; ++i)
        {
            //获取真实的敌对单位位置的接口，待对接
            if (FixVector2.Distance(bullet.anchor, Converter.Vector2ToFixVector2(_parentManager._monster.RoomToMonster[bullet.roomid][i].transform.position)) == enemyDistance[0] ||
               FixVector2.Distance(bullet.anchor, Converter.Vector2ToFixVector2(_parentManager._monster.RoomToMonster[bullet.roomid][i].transform.position)) == enemyDistance[1] ||
               FixVector2.Distance(bullet.anchor, Converter.Vector2ToFixVector2(_parentManager._monster.RoomToMonster[bullet.roomid][i].transform.position)) == enemyDistance[2])
            {
                //敌对单位受击接口，待对接
                //MonsterModule MonsterModule = _parentManager._monster.RoomToMonster[bullet.roomid][i].GetComponent<MonsterModule>();
                //MonsterModule.BeAttacked(bullet.damage);
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
        bullet.bulletScale *= (Fix64)1.5f;
    }

    private void SmallerBullet(Bullet bullet)
    {
        bullet.bulletScale *= (Fix64)0.8f;
    }

    private void LongerBullet(Bullet bullet)
    {
        bullet.bulletScale.x *= 1.5f;
    }

    public override void LogicUpdate()
    {
        for (int i = 0; i < spwanedBullet.Count; ++i)
        {
            for (int j = 0; j < _parentManager._monster.RoomToMonster[spwanedBullet[i].roomid].Count; ++j)
            {
                //检测子弹与敌方单位的碰撞，这里敌方单位的碰撞盒通过GetComponent获取，待对接
                MonsterModel_Component MonsterModule = _parentManager._monster.RoomToMonster[spwanedBullet[i].roomid][j].GetComponent<MonsterModel_Component>();
                CollideDetecter collideDetecter = new CollideDetecter();
                Rectangle rect = new Rectangle(new FixVector2((Fix64)MonsterModule.position.x, (Fix64)MonsterModule.position.y), new FixVector2((Fix64)1, (Fix64)1), (Fix64)1, (Fix64)1);


                if (collideDetecter.PointInRectangle(spwanedBullet[i].anchor, rect) == true)
                {
                    _parentManager._monster.BeAttacked(_parentManager._monster.RoomToMonster[spwanedBullet[i].roomid][j], 1f, spwanedBullet[i].roomid);
                       spwanedBullet[i].active = false;
                    //attackEffect逻辑层面的实现
                    foreach (var effect in spwanedBullet[i].attackEffectList)
                    {
                        switch (effect)
                        {
                            case (int)bulletType.Penetrate:
                                Penetrate(spwanedBullet[i]);
                                break;
                            case (int)bulletType.Sputtering:
                                Sputtering(spwanedBullet[i]);
                                break;
                            case (int)bulletType.LightningChain:
                                LightningChain(spwanedBullet[i]);
                                break;
                        }
                        // if(effect == (int)bulletType.Penetrate) Penetrate(spwanedBullet[i]);
                        // else if(effect == (int)bulletType.Sputtering) Sputtering(spwanedBullet[i]);
                        // else if(effect == (int)bulletType.LightningChain) LightningChain(spwanedBullet[i]);
                    }

                    //debuff传递逻辑层面的实现
                    foreach (var debuff in spwanedBullet[i].debuffList)
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
                    if (spwanedBullet[i].active == false) break;
                }
            }
        }

        //遍历墙体
        for (int i = 0; i < spwanedBullet.Count; ++i)
        {
            for (int j = 0; j < _parentManager._terrain.roomToStone[spwanedBullet[i].roomid].Count; ++j)
            {
                //检测子弹与敌方单位的碰撞，这里敌方单位的碰撞盒通过GetComponent获取，待对接

                Vector2 Pos = _parentManager._terrain.roomToStone[spwanedBullet[i].roomid][j].transform.position;
                FixVector2 vv = new FixVector2((Fix64)_parentManager._terrain.roomToStone[spwanedBullet[i].roomid][j].GetComponent<BoxCollider2D>().size.x,
                                               (Fix64)_parentManager._terrain.roomToStone[spwanedBullet[i].roomid][j].GetComponent<BoxCollider2D>().size.y);
                
                //Debug.Log("collider size is " + vv);
                CollideDetecter collideDetecter = new CollideDetecter();
                BoxCollider2D collider = _parentManager._terrain.roomToStone[spwanedBullet[i].roomid][j].GetComponent<BoxCollider2D>();
                //Debug.Log("wall anchor is " + new FixVector2((Fix64)(Pos.x + collider.offset.x), (Fix64)(Pos.y + collider.offset.y)));
                _parentManager._terrain.roomToStone[spwanedBullet[i].roomid][j].GetComponent<BoxCollider2D>();
                Rectangle rect = new Rectangle(new FixVector2((Fix64)(Pos.x + collider.offset.x), (Fix64)(Pos.y + collider.offset.y)), new FixVector2((Fix64)1, (Fix64)1), 
                    (Fix64)collider.size.x, 
                    (Fix64)collider.size.y);
                if (collideDetecter.PointInRectangle(spwanedBullet[i].anchor, rect) == true)
                {
                    Debug.Log("wall anchor is " + new FixVector2((Fix64)(Pos.x + collider.offset.x), (Fix64)(Pos.y + collider.offset.y)));
                    spwanedBullet[i].active = false;
                    //attackEffect逻辑层面的实现
                    foreach (var effect in spwanedBullet[i].attackEffectList)
                    {
                        switch (effect)
                        {
                            case (int)bulletType.Penetrate:
                                Penetrate(spwanedBullet[i]);
                                break;
                            case (int)bulletType.Sputtering:
                                Sputtering(spwanedBullet[i]);
                                break;
                            case (int)bulletType.LightningChain:
                                LightningChain(spwanedBullet[i]);
                                break;
                        }
                        // if(effect == (int)bulletType.Penetrate) Penetrate(spwanedBullet[i]);
                        // else if(effect == (int)bulletType.Sputtering) Sputtering(spwanedBullet[i]);
                        // else if(effect == (int)bulletType.LightningChain) LightningChain(spwanedBullet[i]);
                    }

                    //debuff传递逻辑层面的实现
                    foreach (var debuff in spwanedBullet[i].debuffList)
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
                    break;
                }
            }
        }

        //更新逻辑层子弹的位置
        List<Bullet> latestBullet = new List<Bullet>();
        foreach (var it in spwanedBullet)
        {
            Bullet bullet = it;
            if (bullet.active == true) bullet.anchor += bullet.toward * bullet.speed;
            latestBullet.Add(bullet);
        }

        spwanedBullet.Clear();

        foreach (var it in latestBullet) spwanedBullet.Add(it);
    }

    public override void ViewUpdate()
    {
        for (int i = 0; i < spwanedBullet.Count; ++i)
        {
            if (spwanedBullet[i].active == true)
            {
                //更新子弹scale
                bulletList[i].transform.localScale = Converter.FixVector2ToVector2(spwanedBullet[i].bulletScale);
                //更新子弹位置
                //每次移动定位子弹speed的1/5，以免出现step太大导致的移动不平滑的问题
                bulletList[i].transform.position = Vector2.MoveTowards(bulletList[i].transform.position, Converter.FixVector2ToVector2(spwanedBullet[i].anchor), (float)spwanedBullet[i].speed);
            }
        }

        //数据层面的销毁子弹的存储集合
        List<Bullet> destroyBullet = new List<Bullet>();
        List<Bullet> liveBullet = new List<Bullet>();

        //视图层面的销毁子弹的存储集合
        List<GameObject> destroyBulletObject = new List<GameObject>();
        List<GameObject> liveBulletObject = new List<GameObject>();


        for (int i = 0; i < spwanedBullet.Count; ++i)
        {
            //为了移除应该destroy的子弹，因为不一定在最后一位，不能直接Remove，所以这边需要拿出来分类后再销毁
            if (spwanedBullet[i].active == true)
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

        foreach (var it in liveBullet) spwanedBullet.Add(it);
        foreach (var it in liveBulletObject) bulletList.Add(it);

        //销毁该die的子弹
        for (int i = 0; i < destroyBulletObject.Count; ++i) UnityEngine.Object.Destroy(destroyBulletObject[i]);

    }
}




class Converter
{
    public static Fix64 DegreeToRadian(Fix64 degree)
    {
        return Mathf.PI / 180f * degree;
    }

    public static Fix64 RadianToDegree(Fix64 Radian)
    {
        return Radian * 180 / Mathf.PI;
    }

    public static Vector2 FixVector2ToVector2(FixVector2 v)
    {
        Vector2 newv = new Vector2((float)v.x, (float)v.y);
        return newv;
    }
    public static FixVector2 Vector2ToFixVector2(Vector2 v)
    {
        return new FixVector2((Fix64)(v.x), (Fix64)(v.y));
    }
    //输入为角度制
    public static FixVector2 NormalFixVector2Rotate(FixVector2 v, Fix64 rotateAngle)
    {
        Debug.Log("传进来的vector：" + v.x + " " + v.y);
        Fix64 length = Fix64.Sqrt(v.x * v.x + v.y * v.y);
        Fix64 originAngle = Fix64.Atan(v.y / v.x);

        FixVector2 rotateVector = new FixVector2(length * Fix64.Cos(originAngle + DegreeToRadian(rotateAngle)), length * Fix64.Sin(originAngle + DegreeToRadian(rotateAngle)));

        return rotateVector;
    }
}