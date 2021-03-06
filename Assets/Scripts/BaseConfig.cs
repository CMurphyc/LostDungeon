﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class BaseConfig: ScriptableObject
{
    public string name;             //名字
    public GameObject obj;          //人物预制体
    public int HP;                  //血量
    public float moveSpeed;         //移动速度
    public float damge;             //伤害
    public float bulletSpeed;       //子弹速度
    public float fireSpeed;           //射速
    public List<bulletType> bulletEffect;  //子弹附加效果
    public GameObject bulletObj;    //子弹预制体
}
