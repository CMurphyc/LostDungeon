using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
[Serializable]
public class Skill_Data
{
    public string Skill_Name;
    public GameObject Effect;
    public float Damage;
    public float Release_Radius;          //施法半径
    public SkillAreaType Skill_Area_Type;
}
[CreateAssetMenu(menuName = "Editor/Config")]
[Serializable]
public class Skill_Config : ScriptableObject
{
    public List<Skill_Data> skill_config_list = new List<Skill_Data>();
}
