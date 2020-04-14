using UnityEngine;
using UnityEditor;


enum AI_BehaviorType
{
    Idle,
    Run,
    Attack,
    Dead

}

public enum StateType
{
    Not_Ready,
    Ready
}

public enum ObjectType
{

    Player, //玩家
    NPC,    //非玩家控制单位
    Projectile, //抛射物
    Portol,      //传送门
};

public class EventMessageType
{
    public static string UserLogin = "Login";

}