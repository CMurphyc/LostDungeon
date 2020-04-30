using UnityEngine;
using UnityEditor;


enum AI_BehaviorType
{
    Idle,
    Run,
    Attack,
    Dead,
    UnderAttack,
    Dash,
    BossSkill1,
    BossSkill2

}

[System.Serializable]
public class Monster
{
    public GameObject monsterGameObject;
    public AI_Type type;
    public int MonsterID;
}



[System.Serializable]
public enum AI_Type
{
    Normal_Melee,
    Nomral_Range,
    Boss_Rabit,
    Boss_Rabit_Egg,
    Engineer_TerretTower,
    Boss_Wizard,
    Boss_DarkKnight,
    Boss_DarkKnightSword
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
    public static string StartSync = "StartSync";
    public static string NextFloor = "NextFloor";
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

public struct TreasureData
{
    public int treasureId;   // 道具在配置表中的下标
    public TreasureType treasuretType;   // 道具的种类   改变人物或子弹属性 / 改变子弹形态 / 主动道具
    public GameObject treasureTable;   // 道具对应的桌子实体
    public GameObject treasureObejct;   // 道具实体
    public bool active;   // 道具是否已经被拾取
    public TreasureData(int _treasureId, TreasureType _treasureType, GameObject _treasureTable, GameObject _treasureObject, bool _active)
    {
        treasureId = _treasureId;
        treasuretType = _treasureType;
        treasureTable = _treasureTable;
        treasureObejct = _treasureObject;
        active = _active;
    }
}



[System.Serializable]
public class Door
{
    public GameObject upDoor;
    public GameObject downDoor;
    public GameObject leftDoor;
    public GameObject rightDoor;
}

public enum TreasureType
{
    Buff = 1,
    BulletChange = 2,
    Initiative = 3
}

public enum BulletChange
{
    Penetrate = 201,
    Sputtering = 202,
    LightningChain = 203,
    Freeze = 204,
    Poision = 205,
    Burn = 206,
    Dizziness = 207,
    Retard = 208,
    Bigger = 209,
    Smaller = 210,
    Longer = 211
}