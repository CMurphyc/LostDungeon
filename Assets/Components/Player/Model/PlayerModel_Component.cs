using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerModel_Component : MonoBehaviour
{
    public int fullHealthPoint { get; set; }           // 玩家血量
    public int healthPoint { get; set; }                //玩家现在的血量
    public Fix64 playerSpeed { get; set; }           // 玩家移动速度

    public Fix64 BuffattackPoint { get; set; }           // 玩家Buff变更攻击力
    public Fix64 attackPoint { get; set; }           // 玩家攻击力
    public Fix64 shootSpeed { get; set; }           //射击速度 就是间隔  
    public int countDown1 { get; set; }                 //技能1倒计时
    public int countDown2 { get; set; }                 //技能2倒计时
    public int countDown3 { get; set; }                 //技能3倒计时
    public int attackCountDown { get; set; }            //攻击倒计时
    public int activeCountDown { get; set; }            //主动道具倒计时
    public Fix64 bulletSpeed { get; set; }              // 玩家射出子弹的速度

    public FixVector2 playerPosition { get; set; }   // 玩家位置
    public bool playerRotation { get; set; }         // 玩家朝向
    public FixVector2 weaponPosition { get; set; }   // 武器位置
    public FixVector2 weaponRotation { get; set; }   // 武器朝向

    public List<bulletType> bulletBuff = new List<bulletType>();

    public DeBuff debuff = new DeBuff();
    public Buff buff = new Buff();

    public int muteki;

    public int dead;


    public int revival;
    public int MaxRevival=100;
    public int PVPrevival;

    //dash Skill
    public bool inDash=false;
    public int DashDuration;
    public FixVector2 DashToward = new FixVector2();

    //void Awake()
    //{
    //    //Position = new FixVector3((Fix64)(-4),(Fix64)1,(Fix64)0);
    //    playerPosition = new FixVector2((Fix64)transform.position.x, (Fix64)transform.position.y);
    //}
    public void Init(int FullHealthPoint,Fix64 PlayerSpeed,Fix64 AttackPoint,Fix64 BulletSpeed,Fix64 ShootSpeed,List<bulletType> BulletBuff)
    {
        muteki = 0;
        dead = 0;
        revival = 0;
        PVPrevival = 0;
        fullHealthPoint = FullHealthPoint;
        healthPoint = FullHealthPoint;
        playerSpeed = PlayerSpeed;
        attackPoint = AttackPoint;
        bulletBuff = BulletBuff;
        bulletSpeed = BulletSpeed;
        shootSpeed = ShootSpeed;

        countDown1 = 0;
        countDown2 = 0;
        countDown3 = 0;
        activeCountDown = 0;
        attackCountDown = 0;
    }

    public void SetAttackCountdown()
    {
        attackCountDown =(int)( (Fix64)1/ shootSpeed*1000/Global.FrameRate);
    }

    public void UpdateLogic()
    {
        if (countDown1 != 0) countDown1--;
        if (countDown2 != 0) countDown2--;
        if (countDown3 != 0) countDown3--;
        if (attackCountDown != 0) attackCountDown--;
        if (muteki != 0) muteki--;

        if (revival >= MaxRevival)
        {
            healthPoint = (int)(0.1f * fullHealthPoint);
            revival = 0;
        }

        if (PVPrevival>= MaxRevival)
        {
            healthPoint = fullHealthPoint;
            PVPrevival = 0;
        }

        if (healthPoint == 0) dead = 1;
        else dead = 0;

        if (DashDuration>0)
        {
            DashDuration--;
        }
        else
        {
            inDash = false;
        }


        UpdateBuff();
     

    }
    private void UpdateBuff()
    {
        if (buff.AttackIncrease_RemainingFrame>0)
        {
            buff.AttackIncrease_RemainingFrame--;
        }
        else
        {
            if (buff.AttackIncrease)
            {
                attackPoint -= BuffattackPoint;
            }

            buff.AttackIncrease = false;

         
        }

        if (buff.Invisible_RemainingFrame > 0)
        {
            buff.Invisible_RemainingFrame--;
        }
        else
        {
            buff.Invisible = false;
        }
    }


    public void Change(int fullHP,int HP,Fix64 ShootSpeed ,Fix64 BulletSpeed,Fix64 AttackPoint,Fix64 PlayerSpeed,List<bulletType> buff)
    {
        fullHealthPoint += fullHP;
        healthPoint += HP;
        healthPoint = Mathf.Max(healthPoint, 0);
        shootSpeed = shootSpeed * ShootSpeed;
        bulletSpeed = bulletSpeed * BulletSpeed;
        attackPoint = attackPoint * AttackPoint;
        playerSpeed = playerSpeed * PlayerSpeed;
        foreach(var x in buff)
        {
            bulletBuff.Add(x);
        }
    }



    public int GetRevival()
    {
        return revival;
    }

    public void SetRevival(int p)
    {
        revival = p;
        if(revival>= MaxRevival)
        {
            healthPoint = (int)(0.1f * fullHealthPoint);
            revival = 0;
        }
    }

    public int GetAttackCountDown()
    {
        return attackCountDown;
    }

    public Fix64 GetShootSpeed()
    {
        return shootSpeed;
    }

    public void SetShootSpeed(Fix64 p)
    {
        shootSpeed = p;
    }

    public int GetMuteki()
    {
        return muteki;
    }

    public void SetMuteki(int p)
    {
        muteki = p;
    }

    public int GetDead()
    {
        return dead;
    }

    public void Dead()
    {
        dead = 1;
    }

    public void Revival()
    {
        dead = 0;
    }

    public int GetCountDown1()
    {
        return countDown1;
    }

    public void SetCountDown1(int p)
    {
        countDown1 = p;
    }

    public int GetCountDown2()
    {
        return countDown2;
    }

    public void SetCountDown2(int p)
    {
        countDown2 = p;
    }
    public int GetCountDown3()
    {
        return countDown3;
    }

    public void SetCountDown3(int p)
    {
        countDown3 = p;
    }

    public List<bulletType> GetBulletBuff()
    {
        return bulletBuff;
    }

    public void SetBulletBuff(List<bulletType> x)
    {
        bulletBuff = x;
    }


    public void SetFullHealthPoint(int _healthPoint)
    {
        fullHealthPoint = _healthPoint;
    }

    public int GetFullHealthPoint()
    {
        return fullHealthPoint;
    }

    public void SetHealthPoint(int _healthPoint)
    {
        healthPoint = _healthPoint;
    }

    public int GetHealthPoint()
    {
        return healthPoint;
    }

    public void SetPlayerSpeed(Fix64 _playerSpeed)
    {
        playerSpeed = _playerSpeed;
    }

    public Fix64 GetPlayerSpeed()
    {
        return playerSpeed;
    }

    public void SetAttackPoint(Fix64 _attackPoint)
    {
        attackPoint = _attackPoint;
    }

    public Fix64 GetAttackPoint()
    {
        return attackPoint;
    }

    public void SetPlayerPosition(FixVector2 _playerPosition)
    {
        playerPosition = _playerPosition;
    }

    public FixVector2 GetPlayerPosition()
    {
        return playerPosition;
    }

    public void SetPlayerRotation(bool _playerRotation)
    {
        playerRotation = _playerRotation;
    }

    public bool GetPlayerRotation()
    {
        return playerRotation;
    }

    public void SetWeaponPosition(FixVector2 _weaponPosition)
    {
        weaponPosition = _weaponPosition;
    }

    public FixVector2 GetWeaponPosition()
    {
        return weaponPosition;
    }

    public void SetWeaponRotation(FixVector2 _weaponRotation)
    {
        weaponRotation = _weaponRotation;
    }

    public FixVector2 GetWeaponRotation()
    {
        return weaponRotation;
    }

    public void SetBulletSpeed(Fix64 _bulletSpeed)
    {
        bulletSpeed = _bulletSpeed;
    }

    public Fix64 GetBulletSpeed()
    {
        return bulletSpeed;
    }
    public void Move(FixVector2 v)
    {
        playerPosition = playerPosition + v;
        if (v.x != (Fix64)0)
        {
            playerRotation = v.x < 0 ? true : false;
        }
    }
    public void SetPosition(FixVector2 v)
    {
        playerPosition = v;
    }



    public class DeBuff
    {
        public bool Poison = false;
        public int PoisonRemainingFrame;
        public int PoisonFrameDuration =100;
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

}
