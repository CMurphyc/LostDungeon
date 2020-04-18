using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterModel_Component : MonoBehaviour
{
    /// <summary>
    /// 最大生命值
    /// </summary>
    public Fix64 MaxHP { get; set; }

    /// <summary>
    /// 当前生命值
    /// </summary>
    public Fix64 HP { get; set; }

    /// <summary>
    /// 当前位置
    /// </summary>
    public FixVector3 position { get; set; }

    /// <summary>
    /// 当前旋转
    /// </summary>
    public Quaternion Rotation { get; set; }
    /// <summary>
    /// 当前攻击力
    /// </summary>
    public Fix64 Damage { get; set; }

    /// <summary>
    /// 当前移动速率
    /// </summary>
    public Fix64 MoveSpeed { get; set; }

    /// <summary>
    /// 当前攻击速率
    /// </summary>
    public Fix64 AttackSpeed { get; set; }


    /// <summary>
    /// 死亡后实体余留帧数
    /// </summary>
    public int FrameLeftFromDestroy = 60;
    /// <summary>
    /// 当前Debuff效果
    /// </summary>

    public DeBuff Debuff;
    /*
    private Fix64 MaxHP;
    private Fix64 HP;
    private FixVector3 position;
    private Fix64 Damage;
    private Fix64 MoveSpeed;
    private Fix64 AttackSpeed;

    public DeBuff debuff;
    
    /// <summary>
    /// 受到Value点伤害
    /// </summary>
    /// <param name="Value">伤害</param>
    public void  BeAttack<T> (T Value) where T:struct 
    {
        HP = HP - (dynamic)Damage;
    }

    /// <summary>
    /// 恢复当前Value点HP
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="Value"></param>
    public void AddHPValue<T>(T Value)where T:struct
    {
        HP = HP + (dynamic)Damage;
        if (HP > MaxHP) HP = MaxHP;
    }

    /// <summary>
    /// 获取当前HP值
    /// </summary>
    /// <returns></returns>
    public Fix64 GetHP()
    {
        return HP;
    }

    /// <summary>
    /// 获取当前位置
    /// </summary>
    /// <returns></returns>
    public FixVector3 GetPosition()
    {
        return position;
    }

    /// <summary>
    /// 获取当前攻击力
    /// </summary>
    /// <returns></returns>
    public Fix64 GetDamage()
    {
        return Damage;
    }

    /// <summary>
    /// 获取当前移动速率
    /// </summary>
    /// <returns></returns>
    public Fix64 GetMoveSpeed()
    {
        return MoveSpeed;
    }

    /// <summary>
    /// 获取当前攻击速度
    /// </summary>
    /// <returns></returns>
    public Fix64 GetAttackSpeed()
    {
        return AttackSpeed;
    }


    /// <summary>
    /// 获取HP最大值
    /// </summary>
    /// <returns></returns>
    public Fix64 GetMaxHP()
    {
        return MaxHP;
    }
    */
}

public class DeBuff
{
    /*
    public bool dd{get;set;}
    public bool dd{get;set;}
    public bool dd{get;set;}
    public bool dd{get;set;}
    public bool dd{get;set;}
    public bool dd{get;set;}
    public bool dd{get;set;}
    public bool dd{get;set;}
    public bool dd{get;set;}
    public bool dd{get;set;}
    */
}
