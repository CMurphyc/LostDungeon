using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;


[Serializable]
public class PropData
{
    [Header("道具类别")]
    public TreasureType propType;
    [Header("道具ID")]
    public int propId;
    [Header("道具实体")]
    public GameObject propObject;
    [Header("改变总血量")]
    public int changefullHP;
    [Header("改变血量")]
    public int changeHP;
    [Header("改变子弹发射频率")]
    public float changeBulletFrequency;
    [Header("改变子弹速度")]
    public float changeBulletSpeed;
    [Header("改变子弹伤害")]
    public float changeDamage;
    [Header("改变移动速度")]
    public float changeSpeed;
    [Header("改变子弹效果")]
    public List<bulletType> bulletType;
}

[CreateAssetMenu(menuName = "Editor/PropConfig")]
[Serializable]
public class PropConfig : ScriptableObject
{
    public List<PropData> prop_config_list = new List<PropData>();
}
