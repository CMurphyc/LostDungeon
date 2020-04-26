using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel_Component : MonoBehaviour
{
    private Fix64 healthPoint;           // 玩家血量
    private Fix64 playerSpeed;           // 玩家移动速度
    private Fix64 attackPoint;           // 玩家攻击力
    private FixVector2 playerPosition;   // 玩家位置
    private bool playerRotation;         // 玩家朝向
    private FixVector2 weaponPosition;   // 武器位置
    private FixVector2 weaponRotation;   // 武器朝向
    private Fix64 bulletSpeed;           // 玩家射出子弹的速度

    //void Awake()
    //{
    //    //Position = new FixVector3((Fix64)(-4),(Fix64)1,(Fix64)0);
    //    playerPosition = new FixVector2((Fix64)transform.position.x, (Fix64)transform.position.y);
    //}

    public void SetHealthPoint(Fix64 _healthPoint)
    {
        healthPoint = _healthPoint;
    }

    public Fix64 GetHealthPoint()
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
        
        playerPosition.x += v.x;
        playerPosition.y += v.y;
        if (v.x != (Fix64)0)
        {
            playerRotation = v.x < 0 ? true : false;
        }
    }
    public void SetPosition(FixVector2 v)
    {
        playerPosition.x = v.x;
        playerPosition.y = v.y;
    }
}
