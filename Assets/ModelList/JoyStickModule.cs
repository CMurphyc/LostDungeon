using UnityEngine;
using System.Collections;

public enum AttackType
{
    BasicAttack,
    Skill1,
    Skill2,
    Skill3,
    ActiveSkill,
    Pick,

}

public class JoyStickModule
{
    public Vector3 Ljoystick;
    public Vector3 Rjoystick;

    public AttackType type;


    public void  Free()
    {
        if (Ljoystick!=null)
        {
            Ljoystick = new Vector3(0, 0, 0);
        }
        if (Rjoystick!=null)
        {
            Rjoystick = new Vector3(0, 0, 0);
        }
        type = 0;
    }
   
}
