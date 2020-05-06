using UnityEngine;
using System.Collections;

public enum SkillType
{
    BossPoison,

}

public class Skill_Component : MonoBehaviour
{
    public int RemainingFrame;
    public FixVector2 Position;
    public SkillType SkillType;

    //检测半径
    public Fix64 Radius;
   
}
