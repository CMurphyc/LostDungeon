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
    public static string CreateGame = "CreateGame";
    public static string GetRoomInfo = "GetRoomInfo";
    public static string RefreshHeroWindow = "RefreshHeroWindow";
    public static string GetRoomList = "GetRoomList";
    public static string LeaveRoom = "LeaveRoom";
    public static string StartGame = "StartGame";

    public static string BattleSyn = "BattleSyn";
}

public enum CharacterType
{
    Enginner,
    Warrior,
    Magician,

    None


}
public class PlayerInGameData
{
    public GameObject obj;
    public int RoomID;

    public int NextAttackFrame;
    public void ChangeRoomID(int val)
    {
        RoomID = val;
    }

}

public struct PlayerData
{
    public bool empty;
    public int uid;
    public bool ready;
    public string username;
    public GameObject obj;
    public CharacterType type;
    public PlayerData(bool emp = true, int id = 0, bool status = false, GameObject gameObject = null, CharacterType character = CharacterType.Enginner, string name = "")
    {
        empty = emp;
        uid = id;
        ready = status;
        obj = gameObject;
        type = character;
        username = name;
    }

}


public struct DoorData
{
    public int doorNum;
    public Vector3 transferPos;
    public DoorData(int _doorNum, Vector3 _transferPos)
    {
        doorNum = _doorNum;
        transferPos = _transferPos;
    }
}