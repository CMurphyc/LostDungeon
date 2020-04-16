using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel_Component : MonoBehaviour
{
    Fix64 healthPoint;   // 玩家血量
    Fix64 playerSpeed;   // 玩家移动速度
    Fix64 attackPoint;   // 玩家攻击力
    FixVector3 playerPosition;   // 玩家位置
    bool playerRotation;   // 玩家朝向
    FixVector3 weaponPosition;   // 武器位置
    FixVector3 weaponRotation;   // 武器朝向
    Fix64 bulletSpeed;   // 玩家射出子弹的速度
    Fix64 roomId;   // 玩家所处的房间ID

    void Awake()
    {
        //Position = new FixVector3((Fix64)(-4),(Fix64)1,(Fix64)0);
        playerPosition = new FixVector3((Fix64)transform.position.x, (Fix64)transform.position.y, (Fix64)transform.position.z);
    }

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

    public void SetPlayerPosition(FixVector3 _playerPosition)
    {
        playerPosition = _playerPosition;
    }

    public FixVector3 GetPlayerPosition()
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

    public void SetWeaponPosition(FixVector3 _weaponPosition)
    {
        weaponPosition = _weaponPosition;
    }

    public FixVector3 GetWeaponPosition()
    {
        return weaponPosition;
    }

    public void SetWeaponRotation(FixVector3 _weaponRotation)
    {
        weaponRotation = _weaponRotation;
    }

    public FixVector3 GetWeaponRotation()
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

    public void SetRoomId(Fix64 _roomId)
    {
        roomId = _roomId;
    }

    public Fix64 GetRoomId()
    {
        return roomId;
    }

    public void Move(Vector2 v)
    {
        playerPosition.x += v.x;
        playerPosition.y += v.y;
        if (v.x != 0)
        {
            playerRotation = v.x < 0 ? true : false;
        }
    }
}
