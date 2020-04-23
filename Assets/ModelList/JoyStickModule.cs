using UnityEngine;
using System.Collections;

public enum AttackType
{
    BasicAttack,
    Skill1,
    Skill2,
   

}

public class JoyStickModule
{
    public Vector3 Ljoystick;
    public Vector3 Rjoystick;

    public AttackType type;
   
}
